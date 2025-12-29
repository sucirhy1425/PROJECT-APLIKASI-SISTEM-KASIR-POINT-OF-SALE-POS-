using System;
using MySql.Data.MySqlClient;

namespace Kasir
{
    public class Account
    {
        private readonly string connStr =
            "Server=127.0.0.1;Port=3306;Database=kasir;Uid=root;Pwd=;SslMode=None;";

        private MySqlConnection OpenConn()
        {
            var conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }

        private string GetStringField(string username, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(username)) return string.Empty;

            using (var conn = OpenConn())
            using (var cmd = new MySqlCommand($"SELECT `{fieldName}` FROM user_account WHERE Username=@u LIMIT 1", conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                object val = cmd.ExecuteScalar();
                return (val == null || val == DBNull.Value) ? string.Empty : val.ToString();
            }
        }

        public string GetFirstname(string username) => GetStringField(username, "Firstname");
        public string GetLastname(string username) => GetStringField(username, "Lastname");
        public string GetEmail(string username) => GetStringField(username, "Email");
        public string GetType(string username) => GetStringField(username, "Type");
        public string GetPassword(string username) => GetStringField(username, "Password");

        public bool isExistsData(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;

            using (var conn = OpenConn())
            using (var cmd = new MySqlCommand("SELECT 1 FROM user_account WHERE Username=@u LIMIT 1", conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                object val = cmd.ExecuteScalar();
                return val != null;
            }
        }

        public void Add(string users, string password, string email, string firstname, string lastname, string type)
        {
            if (string.IsNullOrWhiteSpace(users)) throw new ArgumentException("Username wajib diisi.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password wajib diisi.");

            using (var conn = OpenConn())
            using (var cmd = new MySqlCommand(@"
INSERT INTO user_account (Username, Password, Email, Firstname, Lastname, Type)
VALUES (@username, @password, @email, @firstname, @lastname, @type)", conn))
            {
                cmd.Parameters.AddWithValue("@username", users);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email ?? "");
                cmd.Parameters.AddWithValue("@firstname", firstname ?? "");
                cmd.Parameters.AddWithValue("@lastname", lastname ?? "");
                cmd.Parameters.AddWithValue("@type", type ?? "Kasir");
                cmd.ExecuteNonQuery();
            }
        }

        public bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            using (var conn = OpenConn())
            using (var cmd = new MySqlCommand(@"
SELECT 1
FROM user_account
WHERE Username=@u AND Password=@p
LIMIT 1", conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);
                object val = cmd.ExecuteScalar();
                return val != null;
            }
        }

        public void UpdateUser(string username, string firstname, string lastname, string email)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username wajib diisi.");

            using (var conn = OpenConn())
            using (var cmd = new MySqlCommand(@"
UPDATE user_account
SET Firstname=@firstname, Lastname=@lastname, Email=@email
WHERE Username=@username", conn))
            {
                cmd.Parameters.AddWithValue("@firstname", firstname ?? "");
                cmd.Parameters.AddWithValue("@lastname", lastname ?? "");
                cmd.Parameters.AddWithValue("@email", email ?? "");
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdatePassword(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username wajib diisi.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password wajib diisi.");

            using (var conn = OpenConn())
            using (var cmd = new MySqlCommand(@"
UPDATE user_account
SET Password=@password
WHERE Username=@username", conn))
            {
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
