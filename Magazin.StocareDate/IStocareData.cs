using System.Collections.Generic;
using Magazin.Models;

namespace Magazin.StocareDate
{
    public interface IStocareData
    {
        void AdaugaProdus(Produs produs);
        List<Produs> GetProduse();
    }
}