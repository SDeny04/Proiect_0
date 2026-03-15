using System;
using System.Collections.Generic;

namespace MagazinConsole
{
    public class Magazin
    {
        private List<Produs> produse = new List<Produs>();

        public void AdaugaProdus(Produs p)
        {
            produse.Add(p);
            Console.WriteLine("Produs adaugat!");
        }

        public void AfiseazaProduse()
        {
            if (produse.Count == 0)
            {
                Console.WriteLine("Nu exista produse.");
                return;
            }

            foreach (Produs p in produse)
            {
                p.Afisare();
            }
        }

        public void StergeProdus(int id)
        {
            Produs gasit = produse.Find(p => p.Id == id);

            if (gasit != null)
            {
                produse.Remove(gasit);
                Console.WriteLine("Produs sters.");
            }
            else
            {
                Console.WriteLine("Produsul nu exista.");
            }
        }

        public void CautaDupaNume(string nume)
        {
            foreach (Produs p in produse)
            {
                if (p.Nume.ToLower().Contains(nume.ToLower()))
                {
                    p.Afisare();
                }
            }
        }

        public void CautaDupaCategorie(Categorie categorie)
        {
            foreach (Produs p in produse)
            {
                if (p.CategorieProdus == categorie)
                {
                    p.Afisare();
                }
            }
        }

        public void CautaDupaPret(double pretMax)
        {
            foreach (Produs p in produse)
            {
                if (p.Pret <= pretMax)
                {
                    p.Afisare();
                }
            }
        }
    }
}