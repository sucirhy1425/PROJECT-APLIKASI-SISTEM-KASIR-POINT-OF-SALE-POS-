using DGVPrinterHelper;
using Kasir.class_element;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Kasir
{
    public partial class Main : MetroFramework.Forms.MetroForm
    {
        public Main()
        {
            InitializeComponent();
        // kontrol agar tidak auto select saat binding
        comboBarang.SelectedIndexChanged += comboBarang_SelectedIndexChanged;
        }

        // Username user kasir
        public string username;

        // MySQL only
        private Account acc;
        private Ultilities utils;

        // Flag agar event combobox tidak jalan saat DataSource baru di-set
        private bool _isComboReady = false;

        // List data keranjang
        private readonly List<string> _kode = new List<string>();
        private readonly List<string> _name = new List<string>();
        private readonly List<string> _jenis = new List<string>();
        private readonly List<string> _supplier = new List<string>();
        private readonly List<int> _stock = new List<int>();
        private readonly List<int> _jumlah = new List<int>();
        private readonly List<int> _rawHarga = new List<int>();
        private readonly List<int> _harga = new List<int>();   // total harga per item
        private readonly List<int> _diskon = new List<int>();

        private int TotalBiaya;
        private int cash;

        private void Main_Load(object sender, EventArgs e)
        {
            utils = new Ultilities();

            if (!Authentication())
                return;

            CreateBelanjaColumns();
            GetBiodata();
            LoadBarangToCombo();
            GetDiscNum();
            GetTransaksi();
        }

        private void CreateBelanjaColumns()
        {
            if (metroListView1 == null) return;

            metroListView1.View = View.Details;
            metroListView1.FullRowSelect = true;
            metroListView1.UseCompatibleStateImageBehavior = false;

            metroListView1.Columns.Clear();
            metroListView1.Columns.Add("Barang", 220);
            metroListView1.Columns.Add("Harga", 90, HorizontalAlignment.Right);
            metroListView1.Columns.Add("Jumlah", 70, HorizontalAlignment.Center);
        }

        public bool Authentication()
        {
            bool logged = false;
            Login login = new Login();
            login.ShowDialog();

            if (login.isLogged)
            {
                username = login.username;
                logged = true;
            }
            else
            {
                this.Close();
            }
            return logged;
        }

        public void GetBiodata()
        {
            acc = new Account();

            lblUser.Text =
                acc.GetFirstname(username)
                + " "
                + acc.GetLastname(username)
                + " (" + acc.GetType(username) + ")";
        }

        private void btnJenisBarang_Click(object sender, EventArgs e)
        {
            JenisBarang jenis = new JenisBarang();
            jenis.Show();
        }

        private void btnBarang_Click(object sender, EventArgs e)
        {
            Barang _barang = new Barang();
            _barang.ShowDialog();

            LoadBarangToCombo();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            utils = new Ultilities();

            Choose _choose = new Choose();
            _choose.ShowDialog();

            try
            {
                if (!string.IsNullOrEmpty(_choose.barangChoose))
                    comboBarang.SelectedIndex = comboBarang.Items.IndexOf(_choose.barangChoose);
            }
            catch (Exception ex)
            {
                utils.ShowMessage(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBarangToCombo()
        {
            _isComboReady = false;

            var barang = new BarangDataHelperMySql();
            var list = barang.GetNamaBarang();

            comboBarang.DataSource = null;
            comboBarang.Items.Clear();
            comboBarang.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBarang.DataSource = list;

            comboBarang.SelectedIndex = -1;

            ClearBarangFields();

            _isComboReady = true;
        }

        private void ClearBarangFields()
        {
            txtKode.Clear();
            txtJenisBarang.Clear();
            txtSupplier.Clear();
            txtStock.Clear();
            txtHargaBarang.Clear();
            txtJumlah.Clear();
        }

        private void comboBarang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isComboReady) return;
            if (comboBarang.SelectedIndex < 0) return;
            if (string.IsNullOrWhiteSpace(comboBarang.Text)) return;

            var barang = new BarangDataHelperMySql();
            var row = barang.GetBarangByNama(comboBarang.Text);
            if (row == null) return;

            txtKode.Text = row["kode_barang"].ToString();
            txtJenisBarang.Text = row["jenis_barang"].ToString();
            txtSupplier.Text = row["supplier"].ToString();
            txtStock.Text = row["stock_barang"].ToString();
            txtHargaBarang.Text = row["harga_jual"].ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            utils = new Ultilities();

            if (string.IsNullOrWhiteSpace(comboBarang.Text))
            {
                utils.ShowMessage("Silakan pilih barang terlebih dahulu.", "Penambahan Gagal",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtJumlah.Text))
            {
                utils.ShowMessage("Masukkan jumlah belanja.", "Penambahan Gagal",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtJumlah.Text, out int jumlah) || jumlah <= 0)
            {
                utils.ShowMessage("Jumlah harus berupa angka dan lebih dari 0.", "Penambahan Gagal",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int stock = int.Parse(string.IsNullOrWhiteSpace(txtStock.Text) ? "0" : txtStock.Text);
            if (stock <= 0 || jumlah > stock)
            {
                utils.ShowMessage("Stock barang tidak mencukupi!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboDiskon.SelectedIndex < 0)
                comboDiskon.SelectedIndex = 0;

            int hargaBarang = int.Parse(txtHargaBarang.Text);
            int diskon = int.Parse(comboDiskon.Text.Replace("%", ""));

            if (diskon > 0)
            {
                int subt = hargaBarang * diskon / 100;
                hargaBarang -= subt;
            }

            AddKeranjang(comboBarang.Text, jumlah, hargaBarang, diskon);
            AddTotalBiaya(hargaBarang, jumlah);
            AddDetails(comboBarang.Text, txtKode.Text, txtJenisBarang.Text, hargaBarang, jumlah);

            int totalItem = hargaBarang * jumlah;
            var item = new ListViewItem(comboBarang.Text);
            item.SubItems.Add("Rp" + totalItem.ToString("N0"));
            item.SubItems.Add(jumlah.ToString());
            metroListView1.Items.Add(item);

            _isComboReady = false;
            comboBarang.SelectedIndex = -1;
            _isComboReady = true;

            comboDiskon.SelectedIndex = -1;
            ClearBarangFields();
        }

        private void AddDetails(string name, string kode, string jenis, int harga, int jumlah)
        {
            utils = new Ultilities();

            utils.ClearLabel(lblDetailBarang, name);
            utils.ClearLabel(lblDetailKode, kode);
            utils.ClearLabel(lblDetailJenis, jenis);
            utils.ClearLabel(lblDetailJumlah, jumlah.ToString());

            int d = 0;
            if (comboDiskon.SelectedIndex >= 0)
                int.TryParse(comboDiskon.Text.Replace("%", ""), out d);

            if (d > 0)
                utils.ClearLabel(lblDetailHarga, "Rp" + harga.ToString("N0") + " (" + comboDiskon.Text + ")");
            else
                utils.ClearLabel(lblDetailHarga, "Rp" + harga.ToString("N0"));
        }

        private void RemoveDetails()
        {
            utils = new Ultilities();

            if (_kode.Count == 0)
            {
                utils.ClearLabel(lblDetailBarang, "?");
                utils.ClearLabel(lblDetailKode, "?");
                utils.ClearLabel(lblDetailJenis, "?");
                utils.ClearLabel(lblDetailJumlah, "?");
                utils.ClearLabel(lblDetailHarga, "?");
                return;
            }

            int last = _kode.Count - 1;
            utils.ClearLabel(lblDetailBarang, _name[last]);
            utils.ClearLabel(lblDetailKode, _kode[last]);
            utils.ClearLabel(lblDetailJenis, _jenis[last]);
            utils.ClearLabel(lblDetailJumlah, _jumlah[last].ToString());

            if (_diskon[last] > 0)
                utils.ClearLabel(lblDetailHarga, "Rp" + _harga[last].ToString("N0") + " (" + _diskon[last] + "%)");
            else
                utils.ClearLabel(lblDetailHarga, "Rp" + _harga[last].ToString("N0"));
        }

        private void AddTotalBiaya(int harga, int jumlah)
        {
            TotalBiaya += harga * jumlah;
            lblDetailTotalBiaya.Text = "Rp" + TotalBiaya.ToString("N0");
        }

        private void RemoveTotalBiaya(int hargaTotalItem)
        {
            TotalBiaya -= hargaTotalItem;
            lblDetailTotalBiaya.Text = "Rp" + TotalBiaya.ToString("N0");
        }

        private void AddKeranjang(string name, int jumlah, int hargaUnitAfterDiskon, int diskon)
        {
            _kode.Add(txtKode.Text);
            _name.Add(comboBarang.Text);
            _jenis.Add(txtJenisBarang.Text);
            _supplier.Add(txtSupplier.Text);
            _stock.Add(int.Parse(txtStock.Text));
            _jumlah.Add(jumlah);
            _diskon.Add(diskon);

            _rawHarga.Add(int.Parse(txtHargaBarang.Text));
            _harga.Add(hargaUnitAfterDiskon * jumlah);
        }

        private void ClearKeranjang()
        {
            _kode.Clear();
            _name.Clear();
            _jenis.Clear();
            _supplier.Clear();
            _stock.Clear();
            _jumlah.Clear();
            _rawHarga.Clear();
            _harga.Clear();
            _diskon.Clear();

            metroListView1.Items.Clear();
            RemoveDetails();

            if (cash > TotalBiaya)
            {
                int kembali = cash - TotalBiaya;
                utils.ShowMessage("Kembalian: Rp" + kembali.ToString("N0") + "\nTerima kasih telah berbelanja...",
                    "Kembalian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            TotalBiaya = 0;
            cash = 0;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            utils = new Ultilities();

            if (_name.Count <= 0)
            {
                utils.ShowMessage("Keranjang belanja kosong!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int index = _kode.Count - 1;

            RemoveTotalBiaya(_harga[index]);

            _kode.RemoveAt(index);
            _name.RemoveAt(index);
            _jenis.RemoveAt(index);
            _supplier.RemoveAt(index);
            _jumlah.RemoveAt(index);
            _harga.RemoveAt(index);
            _diskon.RemoveAt(index);
            _rawHarga.RemoveAt(index);
            _stock.RemoveAt(index);

            metroListView1.Items.RemoveAt(index);
            RemoveDetails();
        }

        private void GetDiscNum()
        {
            comboDiskon.Items.Clear();
            for (int i = 0; i < 101; i++)
                comboDiskon.Items.Add(i + "%");
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {
            utils = new Ultilities();

            var barangMy = new BarangDataHelperMySql();
            var tssMy = new TransaksiMySql();

            if (_kode.Count <= 0)
            {
                utils.ShowMessage("Keranjang belanja kosong!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cash _cash = new Cash();
            _cash.ShowDialog();

            if (!int.TryParse(_cash.txtCash.Text, out cash) || cash <= 0)
            {
                utils.ShowMessage("Nominal cash tidak valid.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cash < TotalBiaya)
            {
                utils.ShowMessage("Nominal cash tidak mencukupi", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int newTotalTrans = tssMy.GetTotalTransaksi() + 1;
            tssMy.UpdateTotalTransaksi(newTotalTrans);

            string IDTransaksi = "TR" + newTotalTrans.ToString("00000");

            for (int i = 0; i < _kode.Count; i++)
            {
                var row = barangMy.GetBarangByNama(_name[i]);
                if (row == null)
                {
                    utils.ShowMessage("Barang tidak ditemukan di MySQL: " + _name[i], "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int stockDb = Convert.ToInt32(row["stock_barang"]);
                int terjualDb = Convert.ToInt32(row["terjual"]);

                int newStock = stockDb - _jumlah[i];
                int newTerjual = terjualDb + _jumlah[i];

                if (newStock < 0)
                {
                    utils.ShowMessage("Stock tidak mencukupi untuk barang: " + _name[i], "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                barangMy.UpdateStock(_name[i], newStock);
                barangMy.UpdateTerjual(_name[i], newTerjual);

                // waktu_transaksi otomatis terisi oleh MySQL (DEFAULT CURRENT_TIMESTAMP)
                tssMy.AddTransaksi(
                    IDTransaksi,
                        _name[i],
                        _kode[i],
                        _jenis[i],
                        stockDb,
                        _jumlah[i],
                        _rawHarga[i],
                        username,
                        _diskon[i],
                        _harga[i]
                );
            }

            ClearKeranjang();
            GetTransaksi();
            lblDetailTotalBiaya.Text = "Rp0";
        }

        /// <summary>
        /// Ambil transaksi untuk DataGrid sesuai urutan kolom tampilan:
        /// Jenis | Stock | Jumlah | Harga Barang | Tanggal/Waktu | Kasir | Diskon | Total Biaya
        /// </summary>
        private void GetTransaksi()
        {
            try
            {
                var db = new DatabaseHelperMySql();
                using (var conn = db.GetConnection())
                {
                    conn.Open();

                    // Ambil kolom lengkap untuk grid (TANPA mengambil id)
                    string sql =
                    "SELECT " +
                    "id_transaksi, " +
                    "nama_barang, " +
                    "kode_barang, " +
                    "jenis_barang, " +
                    "stock_barang, " +
                    "jumlah_barang, " +
                    "harga_barang, " +
                    "waktu_transaksi, " +
                    "kasir, " +
                    "diskon, " +
                    "total_biaya " +
                    "FROM kasir_transaksi " +
                    "ORDER BY waktu_transaksi ASC;";

                    if (dataGridTransaksi.Rows.Count > 0)
                    {
                        int lastRow = dataGridTransaksi.Rows.Count - 1;
                        dataGridTransaksi.FirstDisplayedScrollingRowIndex = lastRow;
                        dataGridTransaksi.ClearSelection();
                        dataGridTransaksi.Rows[lastRow].Selected = true;
                    }


                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var rd = cmd.ExecuteReader())
                    {
                        dataGridTransaksi.Rows.Clear();

                        int no = 1;
                        while (rd.Read())
                        {
                            int hargaBarang = rd["harga_barang"] == DBNull.Value ? 0 : Convert.ToInt32(rd["harga_barang"]);
                            int totalBiaya = rd["total_biaya"] == DBNull.Value ? 0 : Convert.ToInt32(rd["total_biaya"]);

                            string waktu = "-";
                            if (rd["waktu_transaksi"] != DBNull.Value)
                                waktu = Convert.ToDateTime(rd["waktu_transaksi"]).ToString("dd-MM-yyyy HH:mm:ss");

                            // URUTAN HARUS SAMA dengan kolom DataGrid Anda:
                            // No | ID Transaksi | Nama Barang | Kode | Jenis | Stock | Jumlah | Harga Barang | Tanggal | Kasir | Diskon | Total Biaya
                            dataGridTransaksi.Rows.Add(new object[]
                            {
                        no,
                        rd["id_transaksi"].ToString(),
                        rd["nama_barang"].ToString(),
                        rd["kode_barang"].ToString(),
                        rd["jenis_barang"].ToString(),
                        rd["stock_barang"].ToString(),
                        rd["jumlah_barang"].ToString(),
                        "Rp" + hargaBarang.ToString("N0"),
                        waktu,
                        rd["kasir"].ToString(),
                        rd["diskon"].ToString(),
                        "Rp" + totalBiaya.ToString("N0")
                            });

                            no++;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Gagal load transaksi MySQL:\n" + ex.Message, "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }               
        }


        private void btnPengaturan_Click(object sender, EventArgs e)
        {
            Pengaturan setting = new Pengaturan();
            setting.username = username;
            setting.ShowDialog();
            GetBiodata();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard dash = new Dashboard();
            dash.username = this.username;
            dash.Show(this);
        }
        private void button1_Click(object sender, EventArgs e)
    {
        if (dataGridTransaksi == null || dataGridTransaksi.Rows.Count == 0)
        {
            MessageBox.Show("Data transaksi masih kosong.", "INFO",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Simpan setting grid (dengan null-safe)
        var oldMode = dataGridTransaksi.AutoSizeColumnsMode;

        Font oldFont = dataGridTransaksi.DefaultCellStyle.Font;
        if (oldFont == null) oldFont = dataGridTransaksi.Font;

        Font oldHeaderFont = dataGridTransaksi.ColumnHeadersDefaultCellStyle.Font;
        if (oldHeaderFont == null) oldHeaderFont = dataGridTransaksi.Font;

        try
        {
            // Kecilkan kolom & font supaya muat 11 kolom
            dataGridTransaksi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridTransaksi.DefaultCellStyle.Font = new Font(oldFont.FontFamily, 7f, FontStyle.Regular);
            dataGridTransaksi.ColumnHeadersDefaultCellStyle.Font = new Font(oldHeaderFont.FontFamily, 7f, FontStyle.Bold);

            DGVPrinter printer = new DGVPrinter();

            printer.Title = "Laporan Transaksi Kasir";
            printer.SubTitle = "Tanggal: " + DateTime.Now.ToString("dddd, dd MMMM yyyy");
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;

            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;

            printer.Footer = "Toko Grosir";
            printer.FooterSpacing = 10;

            // KUNCI: Landscape lewat printDocument (sesuai versi Anda)
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.printDocument.DefaultPageSettings.Margins = new Margins(5, 5, 5, 5);
            printer.printDocument.DefaultPageSettings.PaperSize = new PaperSize("Legal", 850, 1400);

            printer.PrintPreviewDataGridView(dataGridTransaksi);
        }
        finally
        {
            // Kembalikan setting grid
            dataGridTransaksi.AutoSizeColumnsMode = oldMode;
            dataGridTransaksi.DefaultCellStyle.Font = oldFont;
            dataGridTransaksi.ColumnHeadersDefaultCellStyle.Font = oldHeaderFont;
        }
    }

    // handler kosong yang tidak dipakai boleh dibiarkan, tidak mempengaruhi
    private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox4_Enter(object sender, EventArgs e) { }
        private void dataGridTransaksi_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void lblUser_Click(object sender, EventArgs e) { }
        private void metroLabel24_Click(object sender, EventArgs e) { }

        private void btnTentang_Click(object sender, EventArgs e)
        {
            Tentang about = new Tentang();
            about.Show();
        }
    }
}
