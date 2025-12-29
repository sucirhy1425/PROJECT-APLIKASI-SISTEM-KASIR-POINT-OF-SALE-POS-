using MySql.Data.MySqlClient;

namespace Kasir
{
    public class DatabaseHelperMySql
    {
        private string connStr =
            "Server=127.0.0.1;" +
            "Port=3306;" +
            "Database=kasir;" +
            "Uid=root;" +
            "Pwd=;" +
            "SslMode=None;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connStr);
        }
    }
}
