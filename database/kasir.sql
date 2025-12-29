-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Waktu pembuatan: 17 Des 2025 pada 14.38
-- Versi server: 10.4.32-MariaDB
-- Versi PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `kasir`
--

-- --------------------------------------------------------

--
-- Struktur dari tabel `barang_jenis`
--

CREATE TABLE `barang_jenis` (
  `ID` int(11) NOT NULL,
  `jenis_barang` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data untuk tabel `barang_jenis`
--

INSERT INTO `barang_jenis` (`ID`, `jenis_barang`) VALUES
(1, 'Makanan'),
(2, 'Minuman');

-- --------------------------------------------------------

--
-- Struktur dari tabel `barang_list`
--

CREATE TABLE `barang_list` (
  `ID` int(11) NOT NULL,
  `kode_barang` varchar(25) DEFAULT NULL,
  `nama_barang` varchar(50) DEFAULT NULL,
  `jenis_barang` varchar(50) DEFAULT NULL,
  `supplier` varchar(50) DEFAULT NULL,
  `stock_barang` int(11) DEFAULT NULL,
  `harga_jual` int(11) DEFAULT NULL,
  `terjual` int(11) DEFAULT NULL,
  `harga_beli` int(11) DEFAULT NULL,
  `biaya_produksi` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data untuk tabel `barang_list`
--

INSERT INTO `barang_list` (`ID`, `kode_barang`, `nama_barang`, `jenis_barang`, `supplier`, `stock_barang`, `harga_jual`, `terjual`, `harga_beli`, `biaya_produksi`) VALUES
(1, 'MKN001', 'Rendang Payopali Lezat', 'Makanan', 'Paeto Store', 50, 150000, 8, 0, 90000),
(2, 'MKN002', 'Samyang Hot Chicken', 'Makanan', 'Brightfull', 38, 7000, 90, 4000, 0),
(4, 'MKN004', 'Yupi 1 Box', 'Makanan', 'Sehat', 200, 50000, 10, 30000, 0),
(6, 'MKN003', 'Roti Bantal', 'Makanan', 'Nippon Indosari', 100, 15000, 50, 10000, 0),
(8, 'MKN006', 'Mie Ayam', 'Makanan', 'PKL5', 779, 2500, 161, 0, 500),
(13, 'MNM002', 'AQUA 600ML', 'Minuman', 'Tirta Investama', 190, 3000, 10, 1500, 0),
(14, 'MNM003', 'Aqua 330ML', 'Minuman', 'Tirta Investama', 980, 2000, 20, 1000, 0),
(15, 'MNM005', 'Pristine', 'Minuman', 'Pristine Utama', 200, 3000, 0, 2000, 0);

-- --------------------------------------------------------

--
-- Struktur dari tabel `kasir_total`
--

CREATE TABLE `kasir_total` (
  `ID` int(11) NOT NULL,
  `total_transaksi` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data untuk tabel `kasir_total`
--

INSERT INTO `kasir_total` (`ID`, `total_transaksi`) VALUES
(1, 19);

-- --------------------------------------------------------

--
-- Struktur dari tabel `kasir_transaksi`
--

CREATE TABLE `kasir_transaksi` (
  `ID` int(11) NOT NULL,
  `id_transaksi` varchar(50) DEFAULT NULL,
  `nama_barang` varchar(50) DEFAULT NULL,
  `kode_barang` varchar(50) DEFAULT NULL,
  `jenis_barang` varchar(50) DEFAULT NULL,
  `stock_barang` varchar(50) DEFAULT NULL,
  `jumlah_barang` int(11) DEFAULT NULL,
  `harga_barang` int(11) DEFAULT NULL,
  `Kasir` varchar(50) DEFAULT NULL,
  `Diskon` int(11) DEFAULT NULL,
  `total_biaya` int(11) DEFAULT NULL,
  `waktu_transaksi` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data untuk tabel `kasir_transaksi`
--

INSERT INTO `kasir_transaksi` (`ID`, `id_transaksi`, `nama_barang`, `kode_barang`, `jenis_barang`, `stock_barang`, `jumlah_barang`, `harga_barang`, `Kasir`, `Diskon`, `total_biaya`, `waktu_transaksi`) VALUES
(1, 'TR00001', 'Samyang Hot Chicken', 'MKN002', 'Makanan', '128', 30, 14500, 'annasmalik', 1, 430650, '2025-12-13 20:20:45'),
(2, 'TR00002', 'Abon Ikan', 'MKN003', 'Makanan', '47', 20, 22400, 'annasmalik', 5, 425600, '2025-12-13 20:20:45'),
(3, 'TR00003', 'Abon Ikan', 'MKN003', 'Makanan', '27', 10, 22400, 'annasmalik', 0, 224000, '2025-12-13 20:24:03'),
(4, 'TR00004', 'Abon Ikan', 'MKN003', 'Makanan', '17', 7, 22400, 'annasmalik', 7, 145824, '2025-12-13 20:29:27'),
(5, 'TR00005', 'Rendang Payopali Lezat', 'MKN001', 'Makanan', '33', 2, 38500, 'annasmalik', 0, 77000, '2025-12-13 20:36:11'),
(6, 'TR00007', 'Rendang Payopali Lezat', 'MKN001', 'Makanan', '31', 5, 38500, 'annasmalik', 0, 192500, '2025-12-13 20:48:10'),
(7, 'TR00008', 'Yupi 1 Box', 'MKN004', 'Makanan', '400', 10, 50000, 'annasmalik', 0, 500000, '2025-12-13 23:01:36'),
(8, 'TR00009', 'Roti Bantal', 'MKN003', 'Makanan', '50', 40, 15000, 'annasmalik', 0, 600000, '2025-12-13 23:10:38'),
(9, 'TR00010', 'Kopiko', 'MKN005', 'Makanan', '150', 100, 25000, 'annasmalik', 0, 2500000, '2025-12-13 23:18:51'),
(10, 'TR00011', 'Kopiko', 'MKN005', 'Makanan', '50', 5, 25000, 'annasmalik', 0, 125000, '2025-12-14 11:55:14'),
(11, 'TR00012', 'Kispray', 'PRB001', 'Parabot', '40', 10, 10000, 'annasmalik', 0, 100000, '2025-12-14 13:30:11'),
(12, 'TR00013', 'Samsung', 'ELK001', 'Elektronik', '100', 1, 1000000, 'annasmalik', 0, 1000000, '2025-12-14 13:36:02'),
(13, 'TR00014', 'Mie Ayam', 'MKN006', 'Makanan', '100', 50, 2500, 'annasmalik', 0, 125000, '2025-12-14 19:04:15'),
(14, 'TR00015', 'Mie Ayam', 'MKN006', 'Makanan', '50', 50, 2500, 'annasmalik', 0, 125000, '2025-12-14 19:04:51'),
(15, 'TR00016', 'Mie Ayam', 'MKN006', 'Makanan', '40', 40, 2500, 'annasmalik', 0, 100000, '2025-12-14 19:10:15'),
(16, 'TR00016', 'Samyang Hot Chicken', 'MKN002', 'Makanan', '88', 50, 7000, 'annasmalik', 0, 350000, '2025-12-14 19:10:15'),
(17, 'TR00016', 'Roti Bantal', 'MKN003', 'Makanan', '10', 10, 15000, 'annasmalik', 0, 150000, '2025-12-14 19:10:15'),
(18, 'TR00017', 'AQUA 600ML', 'MNM002', 'Miniman', '200', 10, 3000, 'annasmalik', 0, 30000, '2025-12-14 19:16:22'),
(19, 'TR00018', 'Aqua 330ML', 'MNM003', 'Minuman', '1000', 10, 2000, 'putra1234', 0, 20000, '2025-12-17 12:36:29'),
(20, 'TR00018', 'Mie Ayam', 'MKN006', 'Makanan', '800', 20, 2500, 'putra1234', 0, 50000, '2025-12-17 12:36:29'),
(21, 'TR00019', 'Aqua 330ML', 'MNM003', 'Minuman', '990', 10, 2000, 'putra1234', 10, 18000, '2025-12-17 20:18:48'),
(22, 'TR00019', 'Mie Ayam', 'MKN006', 'Makanan', '780', 1, 2500, 'putra1234', 0, 2500, '2025-12-17 20:18:48');

-- --------------------------------------------------------

--
-- Struktur dari tabel `sqlite_sequence`
--

CREATE TABLE `sqlite_sequence` (
  `name` varchar(255) DEFAULT NULL,
  `seq` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Struktur dari tabel `user_account`
--

CREATE TABLE `user_account` (
  `ID` int(11) NOT NULL,
  `Username` varchar(50) DEFAULT NULL,
  `Password` varchar(50) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Firstname` varchar(50) DEFAULT NULL,
  `Lastname` varchar(50) DEFAULT NULL,
  `Type` varchar(25) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data untuk tabel `user_account`
--

INSERT INTO `user_account` (`ID`, `Username`, `Password`, `Email`, `Firstname`, `Lastname`, `Type`) VALUES
(1, 'annasmalik', 'd55ad283aa400af464c76d713c07ad', 'annasmalik@gmail.com', 'Annas ', 'Malik', 'Kasir'),
(2, 'putra1234', 'd55ad283aa400af464c76d713c07ad', 'putra@gmail.com', 'Adurrahman ', 'Putra', 'Kasir');

--
-- Indexes for dumped tables
--

--
-- Indeks untuk tabel `barang_jenis`
--
ALTER TABLE `barang_jenis`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `uq_jenis_barang` (`jenis_barang`);

--
-- Indeks untuk tabel `barang_list`
--
ALTER TABLE `barang_list`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fk_baranglist_jenis_text` (`jenis_barang`);

--
-- Indeks untuk tabel `kasir_total`
--
ALTER TABLE `kasir_total`
  ADD PRIMARY KEY (`ID`);

--
-- Indeks untuk tabel `kasir_transaksi`
--
ALTER TABLE `kasir_transaksi`
  ADD PRIMARY KEY (`ID`);

--
-- Indeks untuk tabel `user_account`
--
ALTER TABLE `user_account`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT untuk tabel yang dibuang
--

--
-- AUTO_INCREMENT untuk tabel `barang_jenis`
--
ALTER TABLE `barang_jenis`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT untuk tabel `barang_list`
--
ALTER TABLE `barang_list`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT untuk tabel `kasir_total`
--
ALTER TABLE `kasir_total`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT untuk tabel `kasir_transaksi`
--
ALTER TABLE `kasir_transaksi`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT untuk tabel `user_account`
--
ALTER TABLE `user_account`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Ketidakleluasaan untuk tabel pelimpahan (Dumped Tables)
--

--
-- Ketidakleluasaan untuk tabel `barang_list`
--
ALTER TABLE `barang_list`
  ADD CONSTRAINT `fk_baranglist_jenis_text` FOREIGN KEY (`jenis_barang`) REFERENCES `barang_jenis` (`jenis_barang`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
