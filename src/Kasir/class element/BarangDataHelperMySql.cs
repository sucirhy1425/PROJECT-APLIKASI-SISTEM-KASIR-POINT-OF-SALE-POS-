using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Kasir.class_element
{

    internal class BarangJenisDataHelperMySql
    {
        private readonly string connStr =
            "Server=127.0.0.1;Port=3306;Database=kasir;Uid=root;Pwd=;SslMode=None;";

        public List<string> GetAllJenis()
        {
            var list = new List<string>();

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT jenis_barang FROM barang_jenis ORDER BY jenis_barang ASC;";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        list.Add(rd.GetString("jenis_barang"));
                }
            }

            return list;
        }

        public bool IsJenisExists(string jenis)
        {
            if (string.IsNullOrWhiteSpace(jenis)) return false;

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT 1 FROM barang_jenis WHERE jenis_barang=@j LIMIT 1;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@j", jenis.Trim());
                    return cmd.ExecuteScalar() != null;
                }
            }
        }

        public void AddJenis(string jenis)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "INSERT INTO barang_jenis (jenis_barang) VALUES (@j);";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@j", jenis.Trim());
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveJenis(string jenis)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "DELETE FROM barang_jenis WHERE jenis_barang=@j;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@j", jenis.Trim());
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    internal class BarangDataHelperMySql
    {
        private readonly string connStr =
            "Server=127.0.0.1;Port=3306;Database=kasir;Uid=root;Pwd=;SslMode=None;";

            public int GetTotalBarang()
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM barang_list;", conn))
                        return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            public int GetTotalStock()
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT IFNULL(SUM(stock_barang),0) FROM barang_list;", conn))
                        return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            public long GetTotalOmzet()
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT IFNULL(SUM(harga_jual * terjual),0) FROM barang_list;", conn))
                        return Convert.ToInt64(cmd.ExecuteScalar());
                }
            }

            public long GetTotalProfit()
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        @"SELECT IFNULL(SUM((harga_jual - (harga_beli + biaya_produksi)) * terjual),0)
                      FROM barang_list;", conn))
                        return Convert.ToInt64(cmd.ExecuteScalar());
                }
            }

        public List<Dictionary<string, object>> GetDashboardRankFromTransaksi(int limit = 25)
        {
            var list = new List<Dictionary<string, object>>();

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
                SELECT
                nama_barang,
                harga_barang,
                SUM(jumlah_barang) AS terjual,
                SUM(total_biaya)   AS total
                FROM kasir_transaksi
                GROUP BY nama_barang, harga_barang
                ORDER BY total DESC
                LIMIT @lim;";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@lim", limit);

                    try
                    {
                        using (var rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                list.Add(new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                                {
                                    ["nama_barang"] = rd["nama_barang"],
                                    ["harga_barang"] = rd["harga_barang"],
                                    ["terjual"] = rd["terjual"],
                                    ["total"] = rd["total"],
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Ini supaya kamu tahu error aslinya apa
                        throw new Exception("GetDashboardRankFromTransaksi gagal: " + ex.Message, ex);
                    }
                }
            }

            return list;
        }




        public List<Dictionary<string, object>> GetAllBarang()
        {
            var list = new List<Dictionary<string, object>>();

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
                SELECT 
                kode_barang, nama_barang, jenis_barang, supplier,
                stock_barang, harga_jual, harga_beli, biaya_produksi, terjual
                FROM barang_list
                ORDER BY kode_barang ASC;";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                        {
                            ["kode_barang"] = rd["kode_barang"],
                            ["nama_barang"] = rd["nama_barang"],
                            ["jenis_barang"] = rd["jenis_barang"],
                            ["supplier"] = rd["supplier"],
                            ["stock_barang"] = rd["stock_barang"],
                            ["harga_jual"] = rd["harga_jual"],
                            ["harga_beli"] = rd["harga_beli"],
                            ["biaya_produksi"] = rd["biaya_produksi"],
                            ["terjual"] = rd["terjual"],
                        });
                    }
                }
            }

            return list;
        }



        public List<string> GetJenisBarang()
        {
            var list = new List<string>();

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT jenis_barang FROM barang_jenis ORDER BY jenis_barang ASC;";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        list.Add(rd.GetString("jenis_barang"));
                }
            }

            return list;
        }


        // ========== UNTUK COMBOBOX MAIN (sudah ada) ==========
        public List<string> GetNamaBarang()
        {
            var list = new List<string>();

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT nama_barang FROM barang_list ORDER BY nama_barang ASC";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        list.Add(rd.GetString("nama_barang"));
                }
            }

            return list;
        }

        public Dictionary<string, object> GetBarangByNama(string namaBarang)
        {
            if (string.IsNullOrWhiteSpace(namaBarang))
                return null;

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
                SELECT kode_barang, nama_barang, jenis_barang, supplier,
                       stock_barang, harga_jual, harga_beli, biaya_produksi, terjual
                FROM barang_list
                WHERE nama_barang = @nama
                LIMIT 1;";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", namaBarang);
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (!rd.Read()) return null;

                        return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                        {
                            ["kode_barang"] = rd["kode_barang"],
                            ["nama_barang"] = rd["nama_barang"],
                            ["jenis_barang"] = rd["jenis_barang"],
                            ["supplier"] = rd["supplier"],
                            ["stock_barang"] = rd["stock_barang"],
                            ["harga_jual"] = rd["harga_jual"],
                            ["harga_beli"] = rd["harga_beli"],
                            ["biaya_produksi"] = rd["biaya_produksi"],
                            ["terjual"] = rd["terjual"]
                        };

                    }
                }
            }
        }


        // ========== VALIDASI ==========
        public bool isBarangExists(string namaBarang)
        {
            if (string.IsNullOrWhiteSpace(namaBarang)) return false;

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT 1 FROM barang_list WHERE nama_barang=@n LIMIT 1;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@n", namaBarang);
                    return cmd.ExecuteScalar() != null;
                }
            }
        }

        public bool isKodeExists(string kodeBarang)
        {
            if (string.IsNullOrWhiteSpace(kodeBarang)) return false;

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT 1 FROM barang_list WHERE kode_barang=@k LIMIT 1;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@k", kodeBarang);
                    return cmd.ExecuteScalar() != null;
                }
            }
        }

        // ========== CRUD BARANG ==========
        public void AddBarang(string kode, string nama, string jenis, string supplier, int stock, int hargaJual, int hargaBeli, int biayaProduksi, int terjual)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
                INSERT INTO barang_list
                (kode_barang, nama_barang, jenis_barang, supplier, stock_barang, harga_jual, harga_beli, biaya_produksi, terjual)
                VALUES
                (@kode, @nama, @jenis, @supplier, @stock, @hj, @hb, @bp, @terjual);";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kode", kode);
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@jenis", jenis);
                    cmd.Parameters.AddWithValue("@supplier", supplier);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@hj", hargaJual);
                    cmd.Parameters.AddWithValue("@hb", hargaBeli);
                    cmd.Parameters.AddWithValue("@bp", biayaProduksi);
                    cmd.Parameters.AddWithValue("@terjual", terjual);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateBarang(string kode, string nama, string jenis, string supplier, int stock, int hargaJual, int hargaBeli, int biayaProduksi)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
                UPDATE barang_list
                SET
                nama_barang=@nama,
                jenis_barang=@jenis,
                supplier=@supplier,
                stock_barang=@stock,
                harga_jual=@hj,
                harga_beli=@hb,
                biaya_produksi=@bp
                WHERE kode_barang=@kode;";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kode", kode);
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@jenis", jenis);
                    cmd.Parameters.AddWithValue("@supplier", supplier);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@hj", hargaJual);
                    cmd.Parameters.AddWithValue("@hb", hargaBeli);
                    cmd.Parameters.AddWithValue("@bp", biayaProduksi);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveBarangByKode(string kode)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "DELETE FROM barang_list WHERE kode_barang=@kode;";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kode", kode);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ========== UPDATE STOCK/TERJUAL (sudah ada) ==========
        public void UpdateStock(string namaBarang, int newStock)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "UPDATE barang_list SET stock_barang=@s WHERE nama_barang=@n";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@s", newStock);
                    cmd.Parameters.AddWithValue("@n", namaBarang);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTerjual(string namaBarang, int newTerjual)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "UPDATE barang_list SET terjual=@t WHERE nama_barang=@n";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@t", newTerjual);
                    cmd.Parameters.AddWithValue("@n", namaBarang);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ========== SEARCH ==========
        public List<Dictionary<string, object>> SearchBarang(string keyword)
        {
            var list = new List<Dictionary<string, object>>();
            if (string.IsNullOrWhiteSpace(keyword)) return list;

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
SELECT 
    kode_barang, nama_barang, jenis_barang, supplier,
    stock_barang, harga_jual, harga_beli, biaya_produksi, terjual
FROM barang_list
WHERE
    kode_barang LIKE @kw
    OR nama_barang LIKE @kw
    OR jenis_barang LIKE @kw
    OR supplier LIKE @kw
ORDER BY nama_barang ASC;";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword.Trim() + "%");

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                            {
                                ["kode_barang"] = rd["kode_barang"],
                                ["nama_barang"] = rd["nama_barang"],
                                ["jenis_barang"] = rd["jenis_barang"],
                                ["supplier"] = rd["supplier"],
                                ["stock_barang"] = rd["stock_barang"],
                                ["harga_jual"] = rd["harga_jual"],
                                ["harga_beli"] = rd["harga_beli"],
                                ["biaya_produksi"] = rd["biaya_produksi"],
                                ["terjual"] = rd["terjual"],
                            });
                        }
                    }
                }
            }

            return list;
        }


        // ========== UTIL ==========
        public int Keuntungan(int hargaJual, int hargaBeli, int biayaProduksi, int terjual)
        {
            // logika asli Anda: keuntungan per unit = hargaJual - (hargaBeli + biayaProduksi)
            // total = keuntungan per unit * terjual
            int modal = hargaBeli + biayaProduksi;
            int untungPerUnit = hargaJual - modal;
            return untungPerUnit * terjual;
        }
    }
}
