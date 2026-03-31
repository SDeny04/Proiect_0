using System.Collections.Generic;
using System.Linq;
using Magazin.Models;
using Magazin.StocareDate;

namespace Magazin.Logic
{
    public class ComandaAdmin
    {
        private IStocareData stocare;
        private int nextId = 1;

        public ComandaAdmin()
        {
            stocare = StocareFactory.GetStocare();
            var comenzi = stocare.GetComenzi();
            if (comenzi.Count > 0)
                nextId = comenzi.Max(c => c.Id) + 1;
        }
        public bool ExecutaComanda(int idClient, List<int> listaIds, MagazinAdmin magazin)
        {
            List<int> produseConfirmate = new List<int>();

            foreach (int id in listaIds)
            {
                if (magazin.ScadeStoc(id, 1))
                {
                    produseConfirmate.Add(id);
                }
            }

            if (produseConfirmate.Count > 0)
            {
                AdaugaComanda(idClient, produseConfirmate);
                return true;
            }

            return false;
        }
        public void AdaugaComanda(int idClient, List<int> produseId)
        {
            var toateProdusele = stocare.GetProduse();
            double total = toateProdusele.Where(p => produseId.Contains(p.Id)).Sum(p => p.Pret);

            Comanda comandaNoua = new Comanda(nextId++, idClient, produseId, total);
            stocare.AdaugaComanda(comandaNoua);
        }

        public List<Comanda> GetComenzi() => stocare.GetComenzi();

        public List<Comanda> GetComenziClient(int idClient) =>
            stocare.GetComenzi().Where(c => c.IdClient == idClient).ToList();
    }
}