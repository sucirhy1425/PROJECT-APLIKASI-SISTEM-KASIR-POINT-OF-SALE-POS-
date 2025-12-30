using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Kasir
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
            TestMySqlConnection(); // ← INI YANG WAJIB
        }

        private void TestMySqlConnection()
        {
            try
            {
                string connStr =
                    "Server=127.0.0.1;" +
                    "Port=3306;" +
                    "Database=kasir;" +
                    "Uid=root;" +
                    "Pwd=;" +
                    "SslMode=None;";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MessageBox.Show(
                        "Koneksi MySQL berhasil!\nVersi: " + conn.ServerVersion,
                        "SUKSES",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "GAGAL KONEK",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
