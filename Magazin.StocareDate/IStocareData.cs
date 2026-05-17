using System.Collections.Generic;
using Magazin.Models;

namespace Magazin.StocareDate
{
    public interface IStocareData : IUtilizatorCRUD
    {
        void AdaugaProdus(Produs produs);
        List<Produs> GetProduse();
        void UpdateProdus(Produs produs);
        void DeleteProdus(int id);

        void AdaugaComanda(Comanda comanda);
        List<Comanda> GetComenzi();
    }
}