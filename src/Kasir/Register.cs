using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kasir
{
    public partial class Register : MetroFramework.Forms.MetroForm
    {
        // Simpan referensi ke form Login
        private Login _loginForm;

        // Constructor untuk runtime (dipanggil dari Login)
        public Register(Login loginForm)
        {
            InitializeComponent();
            _loginForm = loginForm; // ✅ simpan form login yang dikirim
        }

        // Constructor kosong ini dibiarkan untuk DESIGNER
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        //Deklarasi Objek Class
        Account acc;
        Ultilities util;
        Encryption encrypt;

        /// <summary>
        /// Proses pendaftaran account
        /// </summary>
        private void btnDaftar_Click(object sender, EventArgs e)
        {
            acc = new Account();
            util = new Ultilities();
            encrypt = new Encryption();

            if (txtUsername.Text != string.Empty
                && txtPassword.Text != string.Empty
                && txtConfirmPassword.Text != string.Empty
                && txtEmail.Text != string.Empty
                && txtFirstname.Text != string.Empty
                && txtLastname.Text != string.Empty)
            {
                //Melakukan pengecekan kecocokan password dan confirm password
                if (txtPassword.Text == txtConfirmPassword.Text)
                {

                    //Username, Password harus terdiri lebih dari 8 karakter
                    if (txtPassword.Text.Length >= 8
                        && txtConfirmPassword.Text.Length >= 8
                        && txtUsername.Text.Length >= 8)
                    {
                        //Mendeteksi kevalidan email yang di mana terdapat tanda "@"
                        if (txtEmail.Text.Contains("@"))
                        {
                            try
                            {
                                if (!acc.isExistsData(txtUsername.Text))
                                {
                                    //Menambahkan akun yang terdaftar ke dalam database
                                    acc.Add(
                                        txtUsername.Text,
                                        encrypt.HashPassword(txtPassword.Text),
                                        txtEmail.Text,
                                        txtFirstname.Text,
                                        txtLastname.Text,
                                        "Kasir"
                                    );

                                    util.ShowMessage("Berhasil mendaftar akun!", "Pendaftaran Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Setelah daftar selesai, balik ke Login
                                    if (_loginForm != null)
                                    {
                                        _loginForm.Show();
                                    }

                                    this.Close();
                                }
                                else
                                {
                                    util.ShowMessage("Username tersebut sudah ada!", "Gagal Mendaftar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                util.ShowMessage(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            util.ShowMessage("Alamat email tidak valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        util.ShowMessage("Username dan Password harus\nterdiri lebih dari 8 karakter!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    util.ShowMessage("Password tidak sama!", "Gagal Mendaftar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                util.ShowMessage("Silahkan isi semua bagian!", "Gagal Mendaftar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroLabel5_Click(object sender, EventArgs e)
        {

        }

 

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            if (_loginForm != null)
            {
                _loginForm.Show();
            }
            else
            {
                // fallback: kalau entah kenapa null, buat login baru
                Login login = new Login();
                login.Show();
            }

            this.Close(); // tutup form Register
        }
    }
}
