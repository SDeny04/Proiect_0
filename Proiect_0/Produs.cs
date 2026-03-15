using System;

namespace MagazinConsole
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

    public class Produs
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public Categorie CategorieProdus { get; set; }
        public double Pret { get; set; }

        public Produs(int id, string nume, Categorie categorie, double pret)
        {
            Id = id;
            Nume = nume;
            CategorieProdus = categorie;
            Pret = pret;
        }

        public void Afisare()
        {
            Console.WriteLine($"ID: {Id} | Nume: {Nume} | Categorie: {CategorieProdus} | Pret: {Pret} RON");
        }
    }
}