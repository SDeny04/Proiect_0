using System;
using System.Collections.Generic;
using System.Text;

namespace Magazin.Models
{
    [Flags]
    public enum Optiuni
    {
        Niciuna = 0,
        Garantie = 1,
        SuportDrivere = 2,
        LivrareRapida = 4,
        Returnare14Zile = 8
    }

    public enum Categorie
    {
        Procesor,
        PlacaVideo,
        RAM,
        PlacaDeBaza,
        Sursa,
        Stocare,
        Periferice
    }
}
