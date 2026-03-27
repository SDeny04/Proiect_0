using System;
using System.Collections.Generic;
using System.IO;
using Magazin.Models;

namespace Magazin.StocareDate
{
    public class AdministrareProduseFisierText : IStocareData
    {
        private const string NUME_FISIER = "produse.txt";

        public void AdaugaProdus(Produs produs)
        {
            using (StreamWriter sw = new StreamWriter(NUME_FISIER, true))
            {
                sw.WriteLine($"{produs.Id};{produs.Nume};{produs.CategorieProdus};{produs.Pret};{produs.OptiuniProdus}");
            }
        }

        public List<Produs> GetProduse()
        {
            List<Produs> produse = new List<Produs>();
            if (!File.Exists(NUME_FISIER))
                return produse;

            foreach (var linie in File.ReadAllLines(NUME_FISIER))
            {
                string[] campuri = linie.Split(';');
                int id = int.Parse(campuri[0]);
                string nume = campuri[1];
                Categorie categorie = (Categorie)Enum.Parse(typeof(Categorie), campuri[2]);
                double pret = double.Parse(campuri[3]);
                Optiuni optiuni = (Optiuni)Enum.Parse(typeof(Optiuni), campuri[4]);

                var produs = new Produs(nume, categorie, pret, optiuni) { Id = id };
                produse.Add(produs);
            }

            return produse;
        }
    }
}