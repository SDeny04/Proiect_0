namespace MagazinConsole
{
    public class Produs
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Categorie { get; set; }
        public double Pret { get; set; }

        public Produs(int id, string nume, string categorie, double pret)
        {
            Id = id;
            Nume = nume;
            Categorie = categorie;
            Pret = pret;
        }
    }
}