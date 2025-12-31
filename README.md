<div align="center">

# 🧾 Aplikasi Kasir Desktop (Point of Sales)
<div align="center">
  <img src="src/assets/cashier-logo.png" width="150" alt="Logo Aplikasi Kasir"/>
</div>

**Tugas Kuliah Pemrograman Visual (Desktop) berbasis C# WinForms**

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET%20Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-00000F?style=for-the-badge&logo=mysql&logoColor=white)
![WinForms](https://img.shields.io/badge/WinForms-desktop-blue?style=for-the-badge)

<p align="center">
  <a href="#-fitur-utama">Fitur</a> •
  <a href="#-teknologi-yang-digunakan">Teknologi</a> •
  <a href="#-struktur-database">Database</a> •
  <a href="#%EF%B8%8F-cara-menjalankan-aplikasi">Instalasi</a> •
  <a href="#-tim-pengembang">Tim</a>
</p>

</div>

---

## 📖 Tentang Aplikasi

Aplikasi Kasir Desktop ini dirancang untuk membantu proses transaksi penjualan, pengelolaan barang, dan pencatatan laporan secara **terstruktur, real-time, dan efisien**. Aplikasi ini menggunakan **MySQL (MariaDB)** sebagai database server untuk menjamin keamanan dan integritas data.

Tujuan utama pengembangan aplikasi ini adalah untuk memenuhi tugas mata kuliah **Pemrograman Visual**, dengan fokus pada stabilitas sistem, validasi data yang ketat, dan pengalaman pengguna yang responsif.
<div align="center">
  <img src="src/assets/view.png" width="150" alt="Logo Aplikasi Kasir"/>
</div>

---

## 📌 Fitur Utama

### 🔐 Autentikasi & Keamanan
* **Login Kasir:** Sistem login berbasis akun database.
* **Tracking User:** Informasi kasir yang bertugas tercatat otomatis pada setiap detail transaksi.

### 📦 Manajemen Barang (Inventory)
Pengelolaan data barang yang tersimpan di database MySQL dengan detail:
* Kode Barang, Nama, & Jenis Barang.
* Supplier & Stock Real-time.
* Harga Beli, Harga Jual, & Biaya Produksi.
* Counter Jumlah Terjual.

### 🛒 Transaksi Penjualan (Point of Sale)
* **Live Search:** Pemilihan barang melalui ComboBox yang terhubung langsung ke database.
* **Auto Validation:** Validasi stok otomatis (mencegah transaksi jika stok kurang).
* **Diskon Fleksibel:** Input diskon per item (0–100%).
* **Keranjang Belanja:** Menggunakan `ListView` untuk menampung item sementara.
* **Kalkulasi Otomatis:** Menghitung total biaya, pembayaran, dan kembalian secara instan.

### 📊 Laporan & Dashboard
* **Dashboard Interaktif:** Menampilkan barang terlaris, omzet, dan ranking performa penjualan.
* **Riwayat Transaksi:** Ditampilkan menggunakan `DataGridView` dengan format harga Rupiah (Rp).
* **Export & Print:** Cetak laporan transaksi profesional menggunakan **DGVPrinter** dengan fitur *Preview*.

---

## 🛠️ Teknologi yang Digunakan

Berikut adalah *tech stack* yang digunakan dalam membangun aplikasi ini:

| Komponen | Teknologi | Keterangan |
| :--- | :--- | :--- |
| **Bahasa Pemrograman** | C# | Logic utama aplikasi |
| **Framework** | .NET Framework 4.5.2 | Basis runtime aplikasi |
| **User Interface** | WinForms + MetroFramework | Desain antarmuka modern |
| **Database** | MySQL / MariaDB | Dijalankan via XAMPP |
| **Data Access** | MySql.Data (ADO.NET) | Konektivitas database |
| **IDE** | Visual Studio 2022 | Lingkungan pengembangan |
| **Reporting** | DGVPrinterHelper | Utilitas cetak laporan |

---

## 🗄️ Struktur Database

Aplikasi ini menggunakan 5 tabel utama. Berikut adalah skema ringkasnya:

<details>
<summary><b>Klik untuk melihat detail tabel (SQL)</b></summary>
<br>

```sql
CREATE TABLE barang_list (
  id INT AUTO_INCREMENT PRIMARY KEY,
  kode_barang VARCHAR(25),
  nama_barang VARCHAR(50),
  jenis_barang VARCHAR(50),
  supplier VARCHAR(50),
  stock_barang INT,
  harga_jual INT,
  terjual INT,
  harga_beli INT,
  biaya_produksi INT
);


CREATE TABLE master_jenis_barang (
  id INT AUTO_INCREMENT PRIMARY KEY,
  jenis_barang VARCHAR(50)
);


CREATE TABLE kasir_transaksi (
  id INT AUTO_INCREMENT PRIMARY KEY,
  id_transaksi VARCHAR(50),
  nama_barang VARCHAR(50),
  kode_barang VARCHAR(50),
  jenis_barang VARCHAR(50),
  stock_barang VARCHAR(50),
  jumlah_barang INT,
  harga_barang INT,
  kasir VARCHAR(50),
  diskon INT,
  total_biaya INT,
  waktu_transaksi DATETIME DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE kasir_total (
  id INT AUTO_INCREMENT PRIMARY KEY,
  total_transaksi INT
);


CREATE TABLE account (
  id INT AUTO_INCREMENT PRIMARY KEY,
  username VARCHAR(50),
  password VARCHAR(50),
  email VARCHAR(50),
  firstname VARCHAR(50),
  lastname VARCHAR(50),
  type VARCHAR(25)
);

```
</details>

---

## 🔄 Alur Kerja Aplikasi

Berikut adalah flow bagaimana aplikasi ini bekerja:

1.  **Login User:** User masuk (login) menggunakan akun kasir yang terdaftar.
2.  **Inisialisasi Data:** Aplikasi memuat daftar barang terbaru dari server MySQL.
3.  **Proses Transaksi:**
    * Kasir memilih barang (data otomatis tampil).
    * Input jumlah beli → Barang masuk ke keranjang.
    * *System Check:* Stock divalidasi secara real-time.
4.  **Pembayaran:** Kasir memasukkan nominal uang, sistem menghitung kembalian.
5.  **Finalisasi:**
    * Stock barang berkurang otomatis.
    * Jumlah terjual bertambah.
    * Transaksi disimpan permanen di database `kasir_transaksi`.
6.  **Pelaporan:** Laporan transaksi dapat dilihat di GridView atau dicetak.

---

## ⚙️ Cara Menjalankan Aplikasi

Ikuti langkah-langkah berikut untuk menjalankan project di komputer lokal Anda:

### 1. Persiapan Database
* Pastikan **XAMPP** (Modul MySQL) sudah aktif.
* Buka **phpMyAdmin** dan buat database baru bernama `kasir`.
* **Import** struktur tabel yang disediakan ke dalam database tersebut.

### 2. Konfigurasi Koneksi
Buka source code di Visual Studio, cari bagian pengaturan koneksi database (biasanya di `Helper` atau `Module`), dan sesuaikan `connection string`:

```csharp
string connectionString = "Server=127.0.0.1; Port=3306; Database=kasir; Uid=root; Pwd=; SslMode=None;";
```

### 3. Build & Run
* Buka file (`.sln`) menggunakan Visual Studio 2022.
* Tekan `F5` atau klik tombol Start untuk menjalankan aplikasi.

---

## 👥 Tim Pengembang

**Tugas Kelompok - Pemrograman Visual (Desktop)**

Dosen Pengampu: **Pak Yogi Yulianto, M.Kom**

| No | Nama Anggota | NIM |
| :---: | :--- | :--- |
| 1. | **ANNAS MALIK** | 312310520 |
| 2. | **NUR SUCI RAHAYU** | 312310684 |
| 3. | **M TAESAR MAULANA** | 312310711 |
| 4. | **ABDURRAHMAN PUTRA** | 312310787 |

---

## 📄 Lisensi
Project ini dibuat untuk keperluan akademik dan pembelajaran. Anda bebas menggunakan, memodifikasi, dan mengembangkan project ini lebih lanjut.

---

<div align="center"> Built with ❤️ using C# WinForms </div>
