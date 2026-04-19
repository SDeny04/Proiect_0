using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Magazin.Models;

namespace Magazin.StocareDate
{
    public class AdministrareProduseFisierText : IStocareData
    {
        private const string FISIER_PRODUSE = "produse.txt";
        private const string FISIER_UTILIZATORI = "utilizatori.txt";
        private const string FISIER_COMENZI = "comenzi.txt";

        /// <summary>
        /// Directorul de bază unde sunt stocate fișierele de date.
        /// Dacă este null, se folosește directorul curent de execuție.
        /// </summary>
        public static string? BasePath { get; set; }

        private static string GetCale(string fisier)
        {
            if (!string.IsNullOrEmpty(BasePath))
                return Path.Combine(BasePath, fisier);
            return fisier;
        }

        public void AdaugaProdus(Produs produs)
        {
            using (StreamWriter sw = new StreamWriter(GetCale(FISIER_PRODUSE), true))
            {
                sw.WriteLine($"{produs.Id};{produs.Nume};{produs.CategorieProdus};{produs.Pret};{produs.OptiuniProdus};{produs.Stoc};");
            }
        }

        public List<Produs> GetProduse()
        {
            List<Produs> produse = new List<Produs>();
            if (!File.Exists(GetCale(FISIER_PRODUSE))) return produse;

            foreach (var linie in File.ReadAllLines(GetCale(FISIER_PRODUSE)))
            {
                if (string.IsNullOrWhiteSpace(linie)) continue;
                string[] campuri = linie.Split(';');
                int id = int.Parse(campuri[0]);
                string nume = campuri[1];
                Categorie categorie = (Categorie)Enum.Parse(typeof(Categorie), campuri[2]);
                double pret = double.Parse(campuri[3]);
                Optiuni optiuni = (Optiuni)Enum.Parse(typeof(Optiuni), campuri[4]);
                int stoc= int.Parse(campuri[5]);

                produse.Add(new Produs(nume, categorie, pret, optiuni, stoc) { Id = id });
            }
            return produse;
        }

        public void AdaugaUtilizator(Utilizator utilizator)
        {
            using (StreamWriter sw = new StreamWriter(GetCale(FISIER_UTILIZATORI), true))
            {
                sw.WriteLine($"{utilizator.Id};{utilizator.Nume};{utilizator.Username};{utilizator.Email};{utilizator.Parola};{utilizator.Rol}");
            }
        }

        public List<Utilizator> GetUtilizatori()
        {
            List<Utilizator> utilizatori = new List<Utilizator>();
            if (!File.Exists(GetCale(FISIER_UTILIZATORI))) return utilizatori;

            foreach (var linie in File.ReadAllLines(GetCale(FISIER_UTILIZATORI)))
            {
                if (string.IsNullOrWhiteSpace(linie)) continue;
                string[] campuri = linie.Split(';');
                int id = int.Parse(campuri[0]);
                string nume = campuri[1];
                string username = campuri[2];
                string email = campuri[3];
                string parola = campuri[4];
                TipRol rol = (TipRol)Enum.Parse(typeof(TipRol), campuri[5]);

                utilizatori.Add(new Utilizator(id, nume, username, email, parola, rol));
            }
            return utilizatori;
        }

        public void AdaugaComanda(Comanda comanda)
        {
            using (StreamWriter sw = new StreamWriter(GetCale(FISIER_COMENZI), true))
            {
                
                string produseString = string.Join(",", comanda.IdProduse);
                sw.WriteLine($"{comanda.Id};{comanda.IdClient};{produseString};{comanda.DataComenzii};{comanda.Total}");
            }
        }

        public List<Comanda> GetComenzi()
        {
            List<Comanda> comenzi = new List<Comanda>();
            if (!File.Exists(GetCale(FISIER_COMENZI))) return comenzi;

            foreach (var linie in File.ReadAllLines(GetCale(FISIER_COMENZI)))
            {
                if (string.IsNullOrWhiteSpace(linie)) continue;
                string[] campuri = linie.Split(';');
                int id = int.Parse(campuri[0]);
                int idClient = int.Parse(campuri[1]);

                List<int> idProduse = new List<int>();
                if (!string.IsNullOrWhiteSpace(campuri[2]))
                {
                    idProduse = campuri[2].Split(',').Select(int.Parse).ToList();
                }

                DateTime data = DateTime.Parse(campuri[3]);
                double total = double.Parse(campuri[4]);

                comenzi.Add(new Comanda(id, idClient, idProduse, total) { DataComenzii = data });
            }
            return comenzi;
        }
    }
}