using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Magazin.Models;
using Magazin.StocareDate;

namespace Magazin.Logic
{
    public class MagazinAdmin
    {
        private IStocareData stocare;
        private int nextId = 1;

        public MagazinAdmin()
        {
            stocare = StocareFactory.GetStocare();
            var produse = stocare.GetProduse();
            if (produse.Count > 0)
                nextId = produse.Max(p => p.Id) + 1;
        }

        public void AdaugaProdus(Produs produs)
        {
            produs.Id = nextId++;
            stocare.AdaugaProdus(produs);
        }

        public void StergeProdus(int id)
        {
            var produse = stocare.GetProduse();
            var produsDeSters = produse.FirstOrDefault(p => p.Id == id);

            if (produsDeSters != null)
            {
                File.WriteAllLines("produse.txt",
                    produse.Where(p => p.Id != id)
                           .Select(p => $"{p.Id};{p.Nume};{p.CategorieProdus};{p.Pret};{p.OptiuniProdus}"));
            }
            else
                Console.WriteLine("Produsul nu a fost găsit.");
        }
        public void ModificaProdus(int id, string nume = null, Categorie? categorie = null, double? pret = null, Optiuni? optiuni = null)
        {
            // Preluăm lista o singură dată
            var produse = stocare.GetProduse();

            // Căutăm produsul în lista preluată
            var produs = produse.FirstOrDefault(p => p.Id == id);

            if (produs == null)
            {
                Console.WriteLine("Produsul nu a fost găsit.");
                return;
            }

            // Modificăm doar câmpurile specificate
            if (!string.IsNullOrWhiteSpace(nume))
                produs.Nume = nume;
            if (categorie.HasValue)
                produs.CategorieProdus = categorie.Value;
            if (pret.HasValue)
                produs.Pret = pret.Value;
            if (optiuni.HasValue)
                produs.OptiuniProdus = optiuni.Value;

            // Rescriem fișierul folosind lista deja modificată din memorie
            File.WriteAllLines("produse.txt",
                produse.Select(p => $"{p.Id};{p.Nume};{p.CategorieProdus};{p.Pret};{p.OptiuniProdus}"));
        }

        public List<Produs> GetProduse() => stocare.GetProduse();

        public List<Produs> CautaDupaNume(string nume) =>
            stocare.GetProduse()
                   .Where(p => p.Nume.ToLower().Contains(nume.ToLower()))
                   .ToList();

        public List<Produs> CautaDupaCategorie(Categorie categorie) =>
            stocare.GetProduse()
                   .Where(p => p.CategorieProdus == categorie)
                   .ToList();

        public List<Produs> CautaDupaPret(double pretMax) =>
            stocare.GetProduse()
                   .Where(p => p.Pret <= pretMax)
                   .ToList();

        public List<Produs> CautaDupaOptiuni(Optiuni optiuni) =>
            stocare.GetProduse()
                   .Where(p => p.OptiuniProdus.HasFlag(optiuni))
                   .ToList();
    }
}