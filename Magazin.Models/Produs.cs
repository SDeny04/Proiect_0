using System;
using Magazin.Models;
namespace Magazin.Models
{
    

    public class Produs
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public Categorie CategorieProdus { get; set; }
        public double Pret { get; set; }
        public Optiuni OptiuniProdus { get; set; }
        public int Stoc { get; set; }

        public Produs(string nume, Categorie categorie, double pret, Optiuni optiuni, int stoc)
        {
            Nume = nume;
            CategorieProdus = categorie;
            Pret = pret;
            OptiuniProdus = optiuni;
            Stoc = stoc;
        }

        public override string ToString()
        {
            return $"{Id}: {Nume} - {CategorieProdus} - {Pret} RON - Optiuni: {OptiuniProdus}- Stoc: {Stoc}";
        }
    }
}