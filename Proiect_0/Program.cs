using System;

namespace MagazinConsole
{
    class Program
    {
        static void Main()
        {
            string rol = Utilizator.Login();

            if (rol == null)
                return;

            Magazin magazin = new Magazin();
            int optiune;

            do
            {
                Console.WriteLine("\n=== MENIU ===");

                if (rol == "admin")
                {
                    Console.WriteLine("1. Adauga produs");
                    Console.WriteLine("2. Afiseaza produse");
                    Console.WriteLine("3. Sterge produs");
                    Console.WriteLine("0. Iesire");
                }
                else
                {
                    Console.WriteLine("1. Afiseaza produse");
                    Console.WriteLine("0. Iesire");
                }

                Console.Write("Alege optiunea: ");
                optiune = int.Parse(Console.ReadLine());

                if (rol == "admin")
                {
                    switch (optiune)
                    {
                        case 1:
                            Console.Write("ID produs: ");
                            int id = int.Parse(Console.ReadLine());

                            Console.Write("Nume produs: ");
                            string nume = Console.ReadLine();

                            Console.Write("Categorie: ");
                            string categorie = Console.ReadLine();

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
                    }
                }
                else
                {
                    if (optiune == 1)
                    {
                        magazin.AfiseazaProduse();
                    }
                }

            } while (optiune != 0);
        }
    }
}