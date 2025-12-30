using Kasir.class_element;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Kasir
{
    public partial class Barang : MetroFramework.Forms.MetroForm
    {
        public Barang()
        {
            InitializeComponent();
        }
    // Deklarasi objek class (MySQL)
    private BarangDataHelperMySql barang;
        private Ultilities utils;

        // Identifier utama untuk edit/hapus
        private string indexKode = string.Empty;

        // Flag agar event radio tidak “menghapus” textbox saat kita sedang isi dari grid
        private bool _syncing = false;
        private bool _isFillingFromGrid = false;
        private int _oldHargaBeli = 0;
        private int _oldBiayaProduksi = 0;
        private bool _oldRadioHargaBeli = true;

        private void Barang_Load(object sender, EventArgs e)
        {
            metroTabControl1.SelectedIndex = 0;

            GetJenisData();
            ClearTextAddMode();
            GetDaftarBarang();
        }

        private void groupDesc_Enter(object sender, EventArgs e) { }
        private void metroTabPage1_Click(object sender, EventArgs e) { }
        private void dataGridTransaksi_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Add Barang Selected
            if (metroTabControl1.SelectedTab.Text == "Add Barang")
            {
                metroTabControl1.SelectedIndex = 0;
                TabEdit.Text = "Edit Barang";
                TabRemove.Text = "Remove Barang";
                TabAdd.Text = "Add Barang";
                groupDesc.Enabled = true;

                btnTambah.Enabled = true;
                btnHapus.Enabled = false;
                btnSimpan.Enabled = false;
                txtKode.Enabled = true;

                ClearTextAddMode();
                indexKode = string.Empty;
            }

            // Edit Barang Selected
            if (metroTabControl1.SelectedTab.Text == "Edit Barang")
            {
                metroTabControl1.SelectedIndex = 0;
                TabEdit.Text = "Add Barang";
                TabRemove.Text = "Remove Barang";
                TabAdd.Text = "Edit Barang";
                groupDesc.Enabled = true;

                btnTambah.Enabled = false;
                btnHapus.Enabled = false;
                btnSimpan.Enabled = true;
                txtKode.Enabled = false;

                ClearTextNonAddMode();
                indexKode = string.Empty;
            }

            // Remove Barang Selected
            if (metroTabControl1.SelectedTab.Text == "Remove Barang")
            {
                metroTabControl1.SelectedIndex = 0;
                TabEdit.Text = "Edit Barang";
                TabRemove.Text = "Add Barang";
                TabAdd.Text = "Remove Barang";
                groupDesc.Enabled = false;

                btnTambah.Enabled = false;
                btnHapus.Enabled = true;
                btnSimpan.Enabled = false;

                ClearTextNonAddMode();
                indexKode = string.Empty;
            }
        }

        /// <summary>
        /// Mendapatkan daftar Jenis Barang (MySQL)
        /// </summary>
        public void GetJenisData()
        {
            barang = new BarangDataHelperMySql();
            comboJenisBarang.Items.Clear();

            foreach (string jenis in barang.GetJenisBarang())
                comboJenisBarang.Items.Add(jenis);

            comboJenisBarang.SelectedIndex = -1;
            comboJenisBarang.Text = "";
        }

        /// <summary>
        /// Membersihkan textbox untuk mode ADD
        /// </summary>
        private void ClearTextAddMode()
        {
            utils = new Ultilities();

            utils.ClearTextBox(txtKode, "");
            utils.ClearTextBox(txtNamaBarang, "");
            comboJenisBarang.SelectedIndex = -1;
            comboJenisBarang.Text = "";

            utils.ClearTextBox(txtSupplier, "");
            utils.ClearTextBox(txtStock, "");
            utils.ClearTextBox(txtHargaJual, "");
            utils.ClearTextBox(txtHargaBeli, "");
            utils.ClearTextBox(txtBiayaProduksi, "");

            // Default: Harga Beli aktif, Biaya Produksi nonaktif
            _syncing = true;
            radioHargaBeli.Checked = true;
            radioBiayaProduksi.Checked = false;
            txtHargaBeli.Enabled = true;
            txtBiayaProduksi.Enabled = false;
            txtBiayaProduksi.Text = ""; // tidak dipakai
            _syncing = false;

        }

        /// <summary>
        /// Membersihkan textbox untuk mode EDIT/REMOVE
        /// </summary>
        private void ClearTextNonAddMode()
        {
            utils = new Ultilities();

            utils.ClearTextBox(txtKode, "");
            utils.ClearTextBox(txtNamaBarang, "");
            comboJenisBarang.SelectedIndex = -1;
            comboJenisBarang.Text = "";

            utils.ClearTextBox(txtSupplier, "");
            utils.ClearTextBox(txtStock, "");
            utils.ClearTextBox(txtHargaJual, "");
            utils.ClearTextBox(txtHargaBeli, "");
            utils.ClearTextBox(txtBiayaProduksi, "");

            // Biarkan radio tetap sinkron saat row dipilih
            txtHargaBeli.Enabled = true;
            txtBiayaProduksi.Enabled = true;
        }

        /// <summary>
        /// Menampilkan daftar barang dari MySQL ke grid
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
                MessageBox.Show("Gagal load daftar barang:\n" + ex.Message, "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== RADIO SYNC ==================
        private void radioHargaBeli_CheckedChanged(object sender, EventArgs e)
        {
            if (_syncing || !radioHargaBeli.Checked) return;

            _syncing = true;
            ApplyRadioState(clearOther: !btnSimpan.Enabled); // clear hanya di ADD
            _syncing = false;
        }

        private void radioBiayaProduksi_CheckedChanged(object sender, EventArgs e)
        {
            if (_syncing || !radioBiayaProduksi.Checked) return;

            _syncing = true;
            ApplyRadioState(clearOther: !btnSimpan.Enabled); // clear hanya di ADD
            _syncing = false;
        }


        private void ApplyRadioState(bool clearOther = false)
        {
            if (radioHargaBeli.Checked)
            {
                txtHargaBeli.Enabled = true;
                txtBiayaProduksi.Enabled = false;
                if (clearOther) txtBiayaProduksi.Text = "";
            }
            else
            {
                txtBiayaProduksi.Enabled = true;
                txtHargaBeli.Enabled = false;
                if (clearOther) txtHargaBeli.Text = "";
            }
        }

        // ================== ADD ==================
        private void btnTambah_Click(object sender, EventArgs e)
        {
            barang = new BarangDataHelperMySql();
            utils = new Ultilities();

            // Validasi field utama (tanpa memaksa 2 textbox modal terisi)
            if (string.IsNullOrWhiteSpace(txtKode.Text) ||
                string.IsNullOrWhiteSpace(txtNamaBarang.Text) ||
                string.IsNullOrWhiteSpace(comboJenisBarang.Text) ||
                string.IsNullOrWhiteSpace(txtSupplier.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text) ||
                string.IsNullOrWhiteSpace(txtHargaJual.Text))
            {
                utils.ShowMessage("Silahkan isi semua bagian yang wajib!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ambil modal sesuai radio
            int hargaBeli, biayaProduksi;
            if (!TryGetModal(out hargaBeli, out biayaProduksi)) return;

            // Cek duplikasi
            if (barang.isBarangExists(txtNamaBarang.Text) || barang.isKodeExists(txtKode.Text))
            {
                utils.ShowMessage("Barang/kode tersebut sudah ada!", "Gagal Tambah Barang",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                barang.AddBarang(
                    txtKode.Text.Trim(),
                    txtNamaBarang.Text.Trim(),
                    comboJenisBarang.Text.Trim(),
                    txtSupplier.Text.Trim(),
                    SafeToInt(txtStock.Text),
                    SafeToInt(txtHargaJual.Text),
                    hargaBeli,
                    biayaProduksi,
                    0
                );

                utils.ShowMessage("Berhasil menambah barang", "Tambah Barang Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearTextAddMode();
                new Thread(GetDaftarBarang).Start();
            }
            catch (Exception ex)
            {
                utils.ShowMessage(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== GRID CLICK (EDIT/REMOVE) ==================
        private void dataGridBarang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (metroTabControl1.SelectedTab.Text == "Remove Barang" || metroTabControl1.SelectedTab.Text == "Edit Barang")
            {
                var r = dataGridBarang.Rows[e.RowIndex];

                string kode = r.Cells[1].Value?.ToString() ?? "";
                string nama = r.Cells[2].Value?.ToString() ?? "";
                string jenis = r.Cells[3].Value?.ToString() ?? "";
                string supplier = r.Cells[4].Value?.ToString() ?? "";
                string stock = r.Cells[5].Value?.ToString() ?? "";

                string hargaJual = (r.Cells[6].Value?.ToString() ?? "").Replace("Rp", "").Replace(",", "").Trim();
                string hargaBeli = (r.Cells[7].Value?.ToString() ?? "").Replace("Rp", "").Replace(",", "").Trim();
                string biayaProd = (r.Cells[8].Value?.ToString() ?? "").Replace("Rp", "").Replace(",", "").Trim();

                _isFillingFromGrid = true;
                try
                {
                    txtKode.Text = kode;
                    indexKode = kode;

                    txtNamaBarang.Text = nama;
                    comboJenisBarang.Text = jenis;
                    txtSupplier.Text = supplier;
                    txtStock.Text = stock;

                    txtHargaJual.Text = hargaJual;
                    txtHargaBeli.Text = hargaBeli;
                    txtBiayaProduksi.Text = biayaProd;

                    // Radio mengikuti data DB:
                    // jika biaya_produksi > 0 => Biaya Produksi, else => Harga Beli
                    int bp = SafeToInt(biayaProd);
                    _oldHargaBeli = SafeToInt(hargaBeli);
                    _oldBiayaProduksi = SafeToInt(biayaProd);
                    _oldRadioHargaBeli = radioHargaBeli.Checked;

                    _syncing = true;
                    radioBiayaProduksi.Checked = (bp > 0);
                    radioHargaBeli.Checked = (bp <= 0);
                    _syncing = false;

                    ApplyRadioState(clearOther: false);
                }
                finally
                {
                    _isFillingFromGrid = false;
                }
            }
        }

        // ================== UPDATE ==================
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            barang = new BarangDataHelperMySql();
            utils = new Ultilities();

            if (string.IsNullOrWhiteSpace(indexKode))
                return;

            if (string.IsNullOrWhiteSpace(txtNamaBarang.Text) ||
                string.IsNullOrWhiteSpace(comboJenisBarang.Text) ||
                string.IsNullOrWhiteSpace(txtSupplier.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text) ||
                string.IsNullOrWhiteSpace(txtHargaJual.Text))
            {
                utils.ShowMessage("Silahkan isi semua bagian yang wajib!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int hargaBeli, biayaProduksi;
            if (!TryGetModal(out hargaBeli, out biayaProduksi)) return;

            try
            {
                barang.UpdateBarang(
                    txtKode.Text.Trim(),
                    txtNamaBarang.Text.Trim(),
                    comboJenisBarang.Text.Trim(),
                    txtSupplier.Text.Trim(),
                    SafeToInt(txtStock.Text),
                    SafeToInt(txtHargaJual.Text),
                    hargaBeli,
                    biayaProduksi
                );

                utils.ShowMessage("Berhasil mengubah data!", "Edit Barang Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearTextNonAddMode();
                indexKode = string.Empty;

                new Thread(GetDaftarBarang).Start();
            }
            catch (Exception ex)
            {
                utils.ShowMessage(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== DELETE ==================
        private void btnHapus_Click(object sender, EventArgs e)
        {
            barang = new BarangDataHelperMySql();
            utils = new Ultilities();

            if (string.IsNullOrWhiteSpace(indexKode))
                return;

            try
            {
                barang.RemoveBarangByKode(indexKode);

                utils.ShowMessage("Barang berhasil terhapus!", "Hapus Barang Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearTextNonAddMode();
                indexKode = string.Empty;

                new Thread(GetDaftarBarang).Start();
            }
            catch (Exception ex)
            {
                utils.ShowMessage(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== SEARCH ==================
        private bool search = false;

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!search)
            {
                if (string.IsNullOrWhiteSpace(txtCari.Text))
                    return;

                search = true;
                btnSearch.Text = "Batal";

                SearchSomethingMySql(txtCari.Text.Trim());
            }
            else
            {
                // === MODE BATAL ===
                search = false;
                btnSearch.Text = "Search";
                txtCari.Text = string.Empty;

                // REFRESH KE DATA SEMULA
                GetDaftarBarang();
            }
        }


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
                        txtCari.Text = string.Empty;
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

        // ================== MODAL VALIDATION (RADIO) ==================
        private bool TryGetModal(out int hargaBeli, out int biayaProduksi)
        {
            hargaBeli = 0;
            biayaProduksi = 0;

            bool isEditMode = (btnSimpan.Enabled && !btnTambah.Enabled); // indikasi tab Edit

            if (radioHargaBeli.Checked)
            {
                // Kalau Edit dan textbox kosong, pertahankan nilai lama
                if (string.IsNullOrWhiteSpace(txtHargaBeli.Text))
                {
                    if (isEditMode)
                    {
                        hargaBeli = _oldHargaBeli;
                        biayaProduksi = 0; // karena pilih Harga Beli
                        return true;
                    }

                    utils.ShowMessage("Harga beli wajib diisi (sesuai pilihan).", "Validasi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHargaBeli.Focus();
                    return false;
                }

                hargaBeli = SafeToInt(txtHargaBeli.Text);
                biayaProduksi = 0;
                return true;
            }
            else // radioBiayaProduksi
            {
                if (string.IsNullOrWhiteSpace(txtBiayaProduksi.Text))
                {
                    if (isEditMode)
                    {
                        biayaProduksi = _oldBiayaProduksi;
                        hargaBeli = 0; // karena pilih Biaya Produksi
                        return true;
                    }

                    utils.ShowMessage("Biaya produksi wajib diisi (sesuai pilihan).", "Validasi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBiayaProduksi.Focus();
                    return false;
                }

                biayaProduksi = SafeToInt(txtBiayaProduksi.Text);
                hargaBeli = 0;
                return true;
            }
        }


        // ================== HELPER PARSING ==================
        private int SafeToInt(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;

            // buang semua karakter non-digit (Rp, spasi, titik, koma, dll)
            var digits = new string(s.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(digits)) return 0;

            int.TryParse(digits, out int n);
            return n;
        }


        private int ToInt(object o)
        {
            if (o == null || o == DBNull.Value) return 0;
            int.TryParse(o.ToString(), out int n);
            return n;
        }
    }
}
