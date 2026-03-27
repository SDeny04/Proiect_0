using System.Collections.Generic;
using System.Linq;
using Magazin.Models;
using Magazin.StocareDate;

namespace Magazin.Logic
{
    public class UtilizatorAdmin
    {
        private IStocareData stocare;
        private int nextId = 1;

        public UtilizatorAdmin()
        {
            stocare = StocareFactory.GetStocare();
            var utilizatori = stocare.GetUtilizatori();
            if (utilizatori.Count > 0)
                nextId = utilizatori.Max(u => u.Id) + 1;
        }

        public void Inregistrare(string nume, string username, string email, string parola, TipRol rol)
        {
            Utilizator u = new Utilizator(nextId++, nume, username, email, parola, rol);
            stocare.AdaugaUtilizator(u);
        }

        public Utilizator Autentificare(string username, string parola)
        {
            return stocare.GetUtilizatori()
                          .FirstOrDefault(u => u.Username == username && u.Parola == parola);
        }

        public List<Utilizator> GetUtilizatori() => stocare.GetUtilizatori();
    }
}