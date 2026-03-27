using System;
using System.Collections.Generic;

namespace Magazin.Models
{
    public class Comanda
    {
        public int Id { get; set; }
        public int IdClient { get; set; } // Legătura cu Utilizatorul
        public List<int> IdProduse { get; set; } // Lista ID-urilor produselor cumpărate
        public DateTime DataComenzii { get; set; }
        public double Total { get; set; }

        public Comanda(int id, int idClient, List<int> idProduse, double total)
        {
            Id = id;
            IdClient = idClient;
            IdProduse = idProduse;
            DataComenzii = DateTime.Now;
            Total = total;
        }

        public override string ToString()
        {
            return $"Comanda #{Id} | Client ID: {IdClient} | Produse: {IdProduse.Count} | Total: {Total} RON | Data: {DataComenzii:dd/MM/yyyy}";
        }
    }
}