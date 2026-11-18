using System;
using System.Collections.Generic;

public class Agen
{
    public string IdAgen { get; private set; }
    public string NamaAgen { get; private set; }
    public int StokAgen { get; private set; }

    public Agen(string idAgen, string namaAgen, int stokAgen = 0)
    {
        if (stokAgen < 0)
            throw new ArgumentException("Stok agen tidak boleh negatif.");

        IdAgen = idAgen;
        NamaAgen = namaAgen;
        StokAgen = stokAgen;
    }

    public void TambahStok(int jumlah)
    {
        if (jumlah > 0)
            StokAgen += jumlah;
    }

    public void TampilInfo()
    {
        Console.WriteLine($"ID Agen    : {IdAgen}");
        Console.WriteLine($"Nama Agen  : {NamaAgen}");
        Console.WriteLine($"Stok Agen  : {StokAgen} galon");
        Console.WriteLine("--------------------------------");
    }
}


public abstract class ProdukGalon
{
    private string idProduk;
    private string namaProduk;
    private int stokGudang;

    public string IdProduk
    {
        get { return idProduk; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                idProduk = value;
            else
                throw new ArgumentException("ID produk tidak boleh kosong.");
        }
    }

    public string NamaProduk
    {
        get { return namaProduk; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                namaProduk = value;
            else
                throw new ArgumentException("Nama produk tidak boleh kosong.");
        }
    }

    public int StokGudang
    {
        get { return stokGudang; }
        private set
        {
            if (value < 0)
                throw new ArgumentException("Stok gudang tidak boleh negatif.");

            stokGudang = value;
        }
    }

    protected ProdukGalon(string idProduk, string namaProduk, int stokGudang)
    {
        IdProduk = idProduk;
        NamaProduk = namaProduk;
        StokGudang = stokGudang;
    }

    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0 || jumlah > StokGudang)
            return false;

        StokGudang -= jumlah;
        return true;
    }

    
    public void TambahStok(int jumlah)
    {
        if (jumlah > 0)
            StokGudang += jumlah;
    }

  
    public abstract int HitungBiaya(int jumlah);

    public virtual void TampilInfo()
    {
        Console.WriteLine($"ID Produk  : {IdProduk}");
        Console.WriteLine($"Nama Produk: {NamaProduk}");
        Console.WriteLine($"Stok Gudang: {StokGudang} galon");
    }
}


public class GalonAkua : ProdukGalon
{
    public GalonAkua(string idProduk, string namaProduk, int stokGudang)
        : base(idProduk, namaProduk, stokGudang)
    {
    }

   
    public override int HitungBiaya(int jumlah)
    {
        return jumlah * 20000;
    }

    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine("Harga      : Rp20.000/galon");
    }
}


public class GalonBerkeran : ProdukGalon
{
    public string JenisKeran { get; set; }

    public GalonBerkeran(
        string idProduk,
        string namaProduk,
        int stokGudang,
        string jenisKeran)
        : base(idProduk, namaProduk, stokGudang)
    {
        JenisKeran = jenisKeran;
    }

 
    public override int HitungBiaya(int jumlah)
    {
        return jumlah * 12000;
    }

    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine("Jenis Keran: " + JenisKeran);
        Console.WriteLine("Harga      : Rp12.000/galon");
    }
}


public interface DapatDidistribusikan
{
    void KirimPaket();
}

public class DistribusiGalon : DapatDidistribusikan
{
    private Agen agenTujuan;
    private ProdukGalon produk;
    private int jumlah;

    public DistribusiGalon(Agen agenTujuan, ProdukGalon produk, int jumlah)
    {
        this.agenTujuan = agenTujuan;
        this.produk = produk;
        this.jumlah = jumlah;
    }

    public void KirimPaket()
    {
        Console.WriteLine("\n--- PROSES PENGIRIMAN PAKET ---");

        // Validasi a: jumlah harus lebih dari 0.
        if (jumlah <= 0)
        {
            Console.WriteLine("Pengiriman gagal: jumlah galon harus lebih dari 0.");
            return;
        }

        // Validasi b: jumlah tidak boleh melebihi stok gudang.
        if (jumlah > produk.StokGudang)
        {
            Console.WriteLine(
                $"Pengiriman gagal: stok gudang hanya {produk.StokGudang} galon.");
            return;
        }

        // Jika lolos validasi, stok gudang dikurangi dan stok agen ditambah.
        bool berhasil = produk.KurangiStok(jumlah);

        if (berhasil)
        {
            agenTujuan.TambahStok(jumlah);

            Console.WriteLine(
                $"Paket berhasil dikirimkan ke agen {agenTujuan.NamaAgen}!");
            Console.WriteLine($"Produk       : {produk.NamaProduk}");
            Console.WriteLine($"Jumlah       : {jumlah} galon");
            Console.WriteLine($"Total biaya  : Rp{produk.HitungBiaya(jumlah):N0}");
            Console.WriteLine($"Sisa gudang  : {produk.StokGudang} galon");
            Console.WriteLine($"Stok agen    : {agenTujuan.StokAgen} galon");
        }
        else
        {
            Console.WriteLine("Pengiriman gagal: stok tidak dapat dikurangi.");
        }
    }
}


public class SindikatGalon
{
   
    private List<Agen> daftarAgen = new List<Agen>();
    private List<ProdukGalon> daftarProduk = new List<ProdukGalon>();

    public void TambahAgen(Agen agen)
    {
        daftarAgen.Add(agen);
    }

    public void TambahProduk(ProdukGalon produk)
    {
        daftarProduk.Add(produk);
    }

    public void TampilkanSindikat()
    {
        Console.WriteLine("\n========== DATA SINDIKAT GALON ==========");

        Console.WriteLine("\nDAFTAR AGEN:");
        foreach (Agen agen in daftarAgen)
        {
            agen.TampilInfo();
        }

        Console.WriteLine("\nDAFTAR PRODUK:");
        foreach (ProdukGalon produk in daftarProduk)
        {
            produk.TampilInfo();
            Console.WriteLine("--------------------------------");
        }
    }
}


public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("       SISTEM MANAJEMEN MAFIA GALON");
        Console.WriteLine("========================================");

        // 1. Membuat minimal dua objek Agen.
        Agen agen1 = new Agen("A001", "Agen jember", 10);
        Agen agen2 = new Agen("A002", "Agen Banyuwangi", 5);

        Console.WriteLine("\n--- INFORMASI AGEN ---");
        agen1.TampilInfo();
        agen2.TampilInfo();

        // Membuat objek produk turunan dari ProdukGalon.
        GalonAkua akua = new GalonAkua("P001", "Galon Akua", 100);
        GalonBerkeran berkeran =
            new GalonBerkeran("P002", "Galon Berkeran", 80, "Keran Putar");

        // 4. POLYMORPHISM:
        // Variabel bertipe ProdukGalon memanggil implementasi
        // HitungBiaya() sesuai objek sebenarnya.
        Console.WriteLine("\n--- DEMONSTRASI POLYMORPHISM ---");

        ProdukGalon produk1 = akua;
        ProdukGalon produk2 = berkeran;

        Console.WriteLine(
            $"Biaya 3 Galon Akua       : Rp{produk1.HitungBiaya(3):N0}");
        Console.WriteLine(
            $"Biaya 3 Galon Berkeran   : Rp{produk2.HitungBiaya(3):N0}");

        // 6. Membuat dan menjalankan proses distribusi.
        DistribusiGalon distribusi1 =
            new DistribusiGalon(agen1, produk1, 20);
        distribusi1.KirimPaket();

        // Contoh pengiriman gagal karena jumlah melebihi stok gudang.
        DistribusiGalon distribusi2 =
            new DistribusiGalon(agen2, produk2, 100);
        distribusi2.KirimPaket();

        // 7. Menggunakan class SindikatGalon (opsional).
        SindikatGalon sindikat = new SindikatGalon();

        sindikat.TambahAgen(agen1);
        sindikat.TambahAgen(agen2);
        sindikat.TambahProduk(akua);
        sindikat.TambahProduk(berkeran);

        sindikat.TampilkanSindikat();

        Console.WriteLine("\nProgram selesai. Tekan tombol apa saja untuk keluar.");
        Console.ReadKey();
    }
}
