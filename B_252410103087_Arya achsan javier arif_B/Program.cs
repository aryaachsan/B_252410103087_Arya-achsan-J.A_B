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
            throw new ArgumentException("Harus ada stok")
        IdAgen = idAgen;
        NamaAgen = namaAgen;
        StokAgen = stokAgen;
    }
    public void TampilInfo()
    {
        Console.WriteLine($"ID Agen    : {IdAgen}");
        Console.WriteLine($"Nama Agen  : {NamaAgen}");
        Console.WriteLine($"Stok Agen  : {StokAgen}");
    }

}

public class Produkgalon
{
    private string idProduk;
    private string namaProduk;
    private int stokGudang;

    public int StokGudang
    {
        get { return stokGudang; }
        private set
        {
            if (value < 0)
                throw new ArgumentException("Stok gudang tidak kosong!!!!");

            stokGudang = value;
        }
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