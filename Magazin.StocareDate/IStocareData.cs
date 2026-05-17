using System.Collections.Generic;
using Magazin.Models;

namespace Magazin.StocareDate
{
    public interface IStocareData
    {
        void AdaugaProdus(Produs produs);
        List<Produs> GetProduse();
        void UpdateProdus(Produs produs);

        void AdaugaUtilizator(Utilizator utilizator);
        List<Utilizator> GetUtilizatori();

        void AdaugaComanda(Comanda comanda);
        List<Comanda> GetComenzi();
    }
}