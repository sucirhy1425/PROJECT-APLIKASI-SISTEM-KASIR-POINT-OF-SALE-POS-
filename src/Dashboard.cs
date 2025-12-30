using System;
using System.Windows.Forms;
using System.Drawing;
using Kasir.class_element; // BarangDataHelperMySql

namespace Kasir
{
    public partial class Dashboard : MetroFramework.Forms.MetroForm
    {
        public Dashboard()
        {
            InitializeComponent();
            this.Resize += Dashboard_Resize;
        }

        // Username kasir yang login (dikirim dari Main)
        public string username;

        // Helper
        private Ultilities utils;
        private BarangDataHelperMySql barangMy;

        private void Dashboard_Load(object sender, EventArgs e)
        {
            utils = new Ultilities();
            barangMy = new BarangDataHelperMySql();

            // Setup Grid
            SetupGridBarangTerlaris();
            gridBarangTerlaris.AllowUserToResizeColumns = true;
            gridBarangTerlaris.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            // Load data realtime dari transaksi
            LoadBarangTerlarisRealtime();

            // Auto fit kolom setelah data masuk
            FitGridColumns();

            // ====== DETAIL USAHA ======
            GetTotalBarang();
            GetTotalStock();
            GetTotalPay();
            GetProfit();
        }

        private void Dashboard_Resize(object sender, EventArgs e)
        {
            FitGridColumns();
        }

        // ===================== GRID SETUP =====================

        private void SetupGridBarangTerlaris()
        {
            // konfigurasi dasar MetroGrid/DataGridView
            gridBarangTerlaris.ReadOnly = true;
            gridBarangTerlaris.AllowUserToAddRows = false;
            gridBarangTerlaris.AllowUserToDeleteRows = false;
            gridBarangTerlaris.AllowUserToResizeRows = false;
            gridBarangTerlaris.MultiSelect = false;
            gridBarangTerlaris.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridBarangTerlaris.RowHeadersVisible = false;
            gridBarangTerlaris.AutoGenerateColumns = false;

            // Penting: agar teks tidak dipotong jadi "..."
            gridBarangTerlaris.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            gridBarangTerlaris.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // Kolom
            gridBarangTerlaris.Columns.Clear();

            gridBarangTerlaris.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRank",
                HeaderText = "Rank",
                Width = 60
            });

            gridBarangTerlaris.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colBarang",
                HeaderText = "Barang",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet, // supaya bisa ditarik manual
                Width = 260,
                MinimumWidth = 120
            });


            gridBarangTerlaris.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colHarga",
                HeaderText = "Harga",
                Width = 120,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            gridBarangTerlaris.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTerjual",
                HeaderText = "Terjual",
                Width = 80,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            gridBarangTerlaris.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotal",
                HeaderText = "Total",
                Width = 160,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Tooltip untuk isi panjang (opsional tapi membantu)
            gridBarangTerlaris.CellToolTipTextNeeded += GridBarangTerlaris_CellToolTipTextNeeded;
        }

        private void GridBarangTerlaris_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            object val = gridBarangTerlaris.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            e.ToolTipText = val?.ToString() ?? "";
        }

        private void FitGridColumns()
        {
            // Pastikan grid sudah ada kolom
            if (gridBarangTerlaris.Columns.Count == 0) return;

            // Pakai lebar tetap untuk kolom angka, kolom Barang Fill.
            // Kalau Anda mau auto sesuai isi untuk Harga/Total bisa pakai DisplayedCells,
            // tapi biasanya Width tetap lebih rapi.
            gridBarangTerlaris.Columns["colRank"].Width = 60;
            gridBarangTerlaris.Columns["colHarga"].Width = 120;
            gridBarangTerlaris.Columns["colTerjual"].Width = 80;
            gridBarangTerlaris.Columns["colTotal"].Width = 180;

            // Barang tetap Fill (mengambil sisa otomatis)
            gridBarangTerlaris.Columns["colBarang"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // ===================== DATA LOAD REALTIME =====================

        private void LoadBarangTerlarisRealtime()
        {
            var data = barangMy.GetDashboardRankFromTransaksi(25);

            gridBarangTerlaris.Rows.Clear();

            int rank = 1;
            foreach (var it in data)
            {
                // Defensive conversion
                string nama = it["nama_barang"]?.ToString() ?? "";

                long harga = 0;
                long terjual = 0;
                long total = 0;

                if (it.ContainsKey("harga_barang")) harga = Convert.ToInt64(it["harga_barang"]);
                if (it.ContainsKey("terjual")) terjual = Convert.ToInt64(it["terjual"]);
                if (it.ContainsKey("total")) total = Convert.ToInt64(it["total"]);

                gridBarangTerlaris.Rows.Add(
                    rank++,
                    nama,
                    "Rp" + harga.ToString("N0"),
                    terjual.ToString(),
                    "Rp" + total.ToString("N0")
                );
            }
        }

        // ===================== DETAIL USAHA =====================

        private void GetTotalBarang()
        {
            lblTotalBarang.Text = barangMy.GetTotalBarang().ToString();
        }

        private void GetTotalStock()
        {
            lblTotalStock.Text = barangMy.GetTotalStock().ToString();
        }

        private void GetTotalPay()
        {
            long total = barangMy.GetTotalOmzet();
            lblTotalTransaksi.Text = "Rp" + total.ToString("N0");
        }

        private void GetProfit()
        {
            long profit = barangMy.GetTotalProfit();
            lblProfit.Text = "Rp" + profit.ToString("N0");
        }

        private void btnInfoProfit_Click(object sender, EventArgs e)
        {
            utils.ShowMessage(
                "Perolehan profit dihitung berdasarkan:\n" +
                "(Harga Jual - (Harga Beli + Biaya Produksi)) x Terjual.\n\n" +
                "Catatan: Jika nanti ingin full realtime dari tabel transaksi,\n" +
                "maka perhitungan omzet/profit sebaiknya diambil dari tabel transaksi/total.",
                "Informasi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void metroLabel24_Click(object sender, EventArgs e) { }
        private void lblUser_Click(object sender, EventArgs e) { }
    }
}
