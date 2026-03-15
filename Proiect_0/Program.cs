using System;

namespace MagazinConsole
{
    class Program
    {
        static void Main()
        {
            Magazin magazin = new Magazin();
            int optiune;

            do
            {
                Console.WriteLine("\n=== MENIU MAGAZIN ===");
                Console.WriteLine("1 Adauga produs");
                Console.WriteLine("2 Afiseaza produse");
                Console.WriteLine("3 Sterge produs");
                Console.WriteLine("4 Cauta dupa nume");
                Console.WriteLine("5 Cauta dupa categorie");
                Console.WriteLine("6 Cauta dupa pret");
                Console.WriteLine("0 Iesire");

                Console.Write("Optiune: ");
                optiune = int.Parse(Console.ReadLine());

                switch (optiune)
                {
                    case 1:

                        Console.Write("ID: ");
                        int id = int.Parse(Console.ReadLine());

                        Console.Write("Nume: ");
                        string nume = Console.ReadLine();

                        Console.WriteLine("Alege categoria:");
                        Console.WriteLine("1 Procesor");
                        Console.WriteLine("2 Placa Video");
                        Console.WriteLine("3 RAM");
                        Console.WriteLine("4 Placa de baza");
                        Console.WriteLine("5 Sursa");
                        Console.WriteLine("6 Stocare");
                        Console.WriteLine("7 Periferice");

                        int categorieInput = int.Parse(Console.ReadLine());

                        Categorie categorie = (Categorie)categorieInput;

                        Console.Write("Pret: ");
                        double pret = double.Parse(Console.ReadLine());

                        Produs p = new Produs(id, nume, categorie, pret);
                        magazin.AdaugaProdus(p);

                        break;

                    case 2:
                        magazin.AfiseazaProduse();
                        break;

                    case 3:
                        Console.Write("ID produs de sters: ");
                        int idStergere = int.Parse(Console.ReadLine());
                        magazin.StergeProdus(idStergere);
                        break;

                    case 4:
                        Console.Write("Nume produs: ");
                        string cautaNume = Console.ReadLine();
                        magazin.CautaDupaNume(cautaNume);
                        break;

                    case 5:

                        Console.WriteLine("Categorie:");
                        Console.WriteLine("1 Procesor");
                        Console.WriteLine("2 Placa Video");
                        Console.WriteLine("3 RAM");
                        Console.WriteLine("4 Placa de baza");
                        Console.WriteLine("5 Sursa");
                        Console.WriteLine("6 Stocare");
                        Console.WriteLine("7 Periferice");

                        int cat = int.Parse(Console.ReadLine());

                        magazin.CautaDupaCategorie((Categorie)cat);

                        break;

                    case 6:
                        Console.Write("Pret maxim: ");
                        double pretMax = double.Parse(Console.ReadLine());
                        magazin.CautaDupaPret(pretMax);
                        break;
                }

            } while (optiune != 0);
        }
    }
}