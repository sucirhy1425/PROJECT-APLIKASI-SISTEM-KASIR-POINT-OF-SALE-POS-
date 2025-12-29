using System;
using MySql.Data.MySqlClient;

namespace Kasir.class_element
{
    internal class TransaksiMySql
    {
        private readonly string connStr =
            "Server=127.0.0.1;Port=3306;Database=kasir;Uid=root;Pwd=;SslMode=None;";

        public int GetTotalTransaksi()
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT total_transaksi FROM kasir_total LIMIT 1";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    object val = cmd.ExecuteScalar();
                    return (val == null || val == DBNull.Value) ? 0 : Convert.ToInt32(val);
                }
            }
        }

        public void UpdateTotalTransaksi(int newTotal)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "UPDATE kasir_total SET total_transaksi=@t";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@t", newTotal);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddTransaksi(
            string idTransaksi,
            string namaBarang,
            string kodeBarang,
            string jenisBarang,
            int stockBarang,
            int jumlahBarang,
            int hargaBarang,
            string kasir,
            int diskon,
            int totalBiaya)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // tanggal sudah dihapus dari tabel, waktu_transaksi diisi otomatis oleh DB
                string sql = @"
INSERT INTO kasir_transaksi
(id_transaksi, nama_barang, kode_barang, jenis_barang, stock_barang, jumlah_barang, harga_barang, kasir, diskon, total_biaya)
VALUES
(@id, @nama, @kode, @jenis, @stock, @jumlah, @harga, @kasir, @diskon, @total);";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idTransaksi);
                    cmd.Parameters.AddWithValue("@nama", namaBarang);
                    cmd.Parameters.AddWithValue("@kode", kodeBarang);
                    cmd.Parameters.AddWithValue("@jenis", jenisBarang);
                    cmd.Parameters.AddWithValue("@stock", stockBarang);
                    cmd.Parameters.AddWithValue("@jumlah", jumlahBarang);
                    cmd.Parameters.AddWithValue("@harga", hargaBarang);
                    cmd.Parameters.AddWithValue("@kasir", kasir);
                    cmd.Parameters.AddWithValue("@diskon", diskon);
                    cmd.Parameters.AddWithValue("@total", totalBiaya);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
