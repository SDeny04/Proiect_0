using System.Collections.Generic;
using System.Linq;
using Magazin.Models;

namespace Magazin.Logic
{
    public class MagazinService
    {
        private List<Produs> produse = new List<Produs>();
        private int nextId = 1;

        public void AdaugaProdus(Produs produs)
        {
            produs.Id = nextId++;
            produse.Add(produs);
        }

        public List<Produs> GetProduse()
        {
            return produse;
        }

        public bool StergeProdus(int id)
        {
            var produs = produse.FirstOrDefault(p => p.Id == id);
            if (produs != null)
            {
                produse.Remove(produs);
                return true;
            }
            return false;
        }

        
        public List<Produs> CautaDupaNume(string nume)
        {
            return produse
                   .Where(p => p.Nume.ToLower().Contains(nume.ToLower()))
                   .ToList();
        }

        
        public List<Produs> CautaDupaCategorie(Categorie categorie)
        {
            return produse
                   .Where(p => p.CategorieProdus == categorie)
                   .ToList();
        }

        
        public List<Produs> CautaDupaPret(double pretMax)
        {
            return produse
                   .Where(p => p.Pret <= pretMax)
                   .ToList();
        }

        
        public List<Produs> CautaDupaOptiuni(Optiuni optiuni)
        {
            return produse
                   .Where(p => p.OptiuniProdus.HasFlag(optiuni))
                   .ToList();
        }
    }
}