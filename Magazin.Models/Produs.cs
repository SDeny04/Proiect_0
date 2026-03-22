using System;

namespace Magazin.Models
{
    public enum Categorie
    {
        Procesor = 1,
        PlacaVideo,
        RAM,
        PlacaDeBaza,
        Sursa,
        Stocare,
        Periferice
    }

    [Flags]
    public enum Optiuni
    {
        Nimic = 0,
        Garantie = 1,
        SuportDrivere = 2,
        LivrareGratuita = 4,
        Retur30Zile = 8,
        MontajInclus = 16,
        Discount = 32
    }

    public class Produs
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public Categorie CategorieProdus { get; set; }
        public double Pret { get; set; }
        public Optiuni OptiuniProdus { get; set; }

        public Produs(string nume, Categorie categorie, double pret, Optiuni optiuni)
        {
            Nume = nume;
            CategorieProdus = categorie;
            Pret = pret;
            OptiuniProdus = optiuni;
        }

        public override string ToString()
        {
            return $"{Id} | {Nume} | {CategorieProdus} | {Pret} RON | Optiuni: {OptiuniProdus}";
        }
    }
}