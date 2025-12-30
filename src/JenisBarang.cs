using System;
using System.Windows.Forms;
using Kasir.class_element;

namespace Kasir
{
    public partial class JenisBarang : MetroFramework.Forms.MetroForm
    {
        public JenisBarang()
        {
            InitializeComponent();
        }

        BarangJenisDataHelperMySql jenisHelper;
        Ultilities utils;

        private void JenisBarang_Load(object sender, EventArgs e)
        {
            GetJenisData();
        }

        public void GetJenisData()
        {
            jenisHelper = new BarangJenisDataHelperMySql();

            comboDaftarJenis.Items.Clear();
            foreach (var jenis in jenisHelper.GetAllJenis())
                comboDaftarJenis.Items.Add(jenis);

            comboDaftarJenis.SelectedIndex = -1;
            comboDaftarJenis.Text = "";
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            jenisHelper = new BarangJenisDataHelperMySql();
            utils = new Ultilities();

            if (string.IsNullOrWhiteSpace(txtJenis.Text))
            {
                utils.ShowMessage("Silahkan masukkan jenis barang!", "Masukkan Jenis Barang",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (jenisHelper.IsJenisExists(txtJenis.Text))
            {
                utils.ShowMessage("Jenis tersebut sudah ada!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                jenisHelper.AddJenis(txtJenis.Text);
                txtJenis.Text = "";
                utils.ShowMessage("Berhasil menambah jenis barang!", "Tambah Jenis Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetJenisData();
            }
            catch (Exception ex)
            {
                utils.ShowMessage(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            jenisHelper = new BarangJenisDataHelperMySql();
            utils = new Ultilities();

            if (string.IsNullOrWhiteSpace(comboDaftarJenis.Text))
                return;

            try
            {
                // Catatan: kalau ada barang yang masih memakai jenis ini, sebaiknya dicegah.
                jenisHelper.RemoveJenis(comboDaftarJenis.Text);

                utils.ShowMessage("Berhasil menghapus jenis barang!", "Hapus Jenis Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetJenisData();
            }
            catch (Exception ex)
            {
                utils.ShowMessage(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
