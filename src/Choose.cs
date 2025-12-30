using System;
using System.Threading;
using System.Windows.Forms;
using Kasir.class_element; // BarangDataHelperMySql

namespace Kasir
{
    public partial class Choose : MetroFramework.Forms.MetroForm
    {
        public Choose()
        {
            InitializeComponent();
        }

        // Deklarasi objek class (MySQL)
        private BarangDataHelperMySql barang;
        private Ultilities utils;

        public string barangChoose = "";

        private void Choose_Load(object sender, EventArgs e)
        {
            GetDaftarBarang();
        }

        /// <summary>
        /// Mendapatkan daftar barang (MySQL)
        /// </summary>
        private void GetDaftarBarang()
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    barang = new BarangDataHelperMySql();
                    dataGridBarang.Rows.Clear();

                    var list = barang.GetAllBarang();

                    int no = 1;
                    foreach (var it in list)
                    {
                        int hargaJual = ToInt(it["harga_jual"]);
                        int hargaBeli = ToInt(it["harga_beli"]);
                        int biayaProd = ToInt(it["biaya_produksi"]);
                        int terjual = ToInt(it["terjual"]);

                        int untung = barang.Keuntungan(hargaJual, hargaBeli, biayaProd, terjual);

                        // Sesuaikan urutan kolom dengan DataGrid kamu:
                        // (di code lama: ID, Kode, Nama, Jenis, Supplier, Stock, HargaJual, HargaBeli, BiayaProduksi, Terjual, Keuntungan)
                        dataGridBarang.Rows.Add(new object[]
                        {
                            no++,                   // dulu ID, kalau kamu butuh ID asli, tambahkan field id di query helper
                            it["kode_barang"],
                            it["nama_barang"],
                            it["jenis_barang"],
                            it["supplier"],
                            it["stock_barang"],
                            "Rp" + hargaJual.ToString("N0"),
                            "Rp" + hargaBeli.ToString("N0"),
                            "Rp" + biayaProd.ToString("N0"),
                            terjual,
                            "Rp" + untung.ToString("N0")
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load daftar barang:\n" + ex.Message, "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool search = false;

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCari.Text))
                return;

            if (!search)
            {
                search = true;
                btnSearch.Text = "Batal";
                SearchSomethingMySql(txtCari.Text);
            }
            else
            {
                search = false;
                btnSearch.Text = "Search";
                txtCari.Text = "";

                new Thread(GetDaftarBarang).Start();
            }
        }

        /// <summary>
        /// Search barang (MySQL)
        /// </summary>
        private void SearchSomethingMySql(string keyword)
        {
            try
            {
                barang = new BarangDataHelperMySql();
                utils = new Ultilities();

                var list = barang.SearchBarang(keyword);

                Invoke((MethodInvoker)delegate
                {
                    dataGridBarang.Rows.Clear();

                    if (list.Count < 1)
                    {
                        utils.ShowMessage("Pencarian tidak ditemukan!", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        search = false;
                        txtCari.Text = "";
                        btnSearch.Text = "Search";

                        new Thread(GetDaftarBarang).Start();
                        return;
                    }

                    int no = 1;
                    foreach (var it in list)
                    {
                        int hargaJual = ToInt(it["harga_jual"]);
                        int hargaBeli = ToInt(it["harga_beli"]);
                        int biayaProd = ToInt(it["biaya_produksi"]);
                        int terjual = ToInt(it["terjual"]);
                        int untung = barang.Keuntungan(hargaJual, hargaBeli, biayaProd, terjual);

                        dataGridBarang.Rows.Add(new object[]
                        {
                            no++,
                            it["kode_barang"],
                            it["nama_barang"],
                            it["jenis_barang"],
                            it["supplier"],
                            it["stock_barang"],
                            "Rp" + hargaJual.ToString("N0"),
                            "Rp" + hargaBeli.ToString("N0"),
                            "Rp" + biayaProd.ToString("N0"),
                            terjual,
                            "Rp" + untung.ToString("N0")
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error search:\n" + ex.Message, "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch_Click(this, new EventArgs());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridBarang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Kolom 2 = Nama Barang (sesuai code lama)
            barangChoose = dataGridBarang.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
        }

        // ========== Helper parsing ==========
        private int ToInt(object o)
        {
            if (o == null || o == DBNull.Value) return 0;
            int.TryParse(o.ToString(), out int n);
            return n;
        }
    }
}
