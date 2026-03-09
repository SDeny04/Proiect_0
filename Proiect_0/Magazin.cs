using System;
using System.IO;

namespace MagazinConsole
{
    public class Magazin
    {
        string fisier = "produse.txt";

        public void AdaugaProdus(Produs p)
        {
            string linie = $"{p.Id};{p.Nume};{p.Categorie};{p.Pret}";
            File.AppendAllText(fisier, linie + Environment.NewLine);

            Console.WriteLine("Produs adaugat!");
        }

        public void AfiseazaProduse()
        {
            if (!File.Exists(fisier))
            {
                Console.WriteLine("Nu exista produse.");
                return;
            }

            string[] linii = File.ReadAllLines(fisier);

            foreach (string linie in linii)
            {
                string[] date = linie.Split(';');

                Console.WriteLine(
                    $"ID: {date[0]} | " +
                    $"Nume: {date[1]} | " +
                    $"Categorie: {date[2]} | " +
                    $"Pret: {date[3]} RON"
                );
            }
        }

        public void StergeProdus(int id)
        {
            if (!File.Exists(fisier))
                return;

            string[] linii = File.ReadAllLines(fisier);

            using (StreamWriter writer = new StreamWriter(fisier))
            {
                foreach (string linie in linii)
                {
                    string[] date = linie.Split(';');

                    if (int.Parse(date[0]) != id)
                    {
                        writer.WriteLine(linie);
                    }
                }
            }

            Console.WriteLine("Produs sters.");
        }
    }
}