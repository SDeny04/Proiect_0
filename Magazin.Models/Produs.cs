using System;

namespace Magazin.Models
{
    [Flags]
    public enum Optiuni
    {
        Niciuna = 0,
        Garantie = 1,
        SuportDrivere = 2,
        LivrareRapida = 4,
        Returnare14Zile = 8
    }

    public enum Categorie
    {
        Procesor,
        PlacaVideo,
        RAM,
        PlacaDeBaza,
        Sursa,
        Stocare,
        Periferice
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
            return $"{Id}: {Nume} - {CategorieProdus} - {Pret} RON - Optiuni: {OptiuniProdus}";
        }
    }
}