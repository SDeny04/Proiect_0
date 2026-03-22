using System;
using Magazin.Logic;
using Magazin.Models;

namespace Magazin.UI
{
    class Program
    {
        static void Main()
        {
            MagazinService magazin = new MagazinService();
            int optiune;

            do
            {
                System.Console.WriteLine("\n=== MENIU MAGAZIN ===");
                System.Console.WriteLine("1. Adauga produs");
                System.Console.WriteLine("2. Afiseaza produse");
                System.Console.WriteLine("3. Sterge produs");
                System.Console.WriteLine("4. Cauta dupa nume");
                System.Console.WriteLine("5. Cauta dupa categorie");
                System.Console.WriteLine("6. Cauta dupa pret");
                System.Console.WriteLine("7. Cauta dupa optiuni");
                System.Console.WriteLine("0. Iesire");

                System.Console.Write("Optiune: ");
                optiune = int.Parse(System.Console.ReadLine());

                switch (optiune)
                {
                    case 1:
                        
                        System.Console.Write("Nume produs: ");
                        string nume = System.Console.ReadLine();

                        System.Console.WriteLine("Categorie:");
                        System.Console.WriteLine("1 Procesor");
                        System.Console.WriteLine("2 Placa Video");
                        System.Console.WriteLine("3 RAM");
                        System.Console.WriteLine("4 Placa de baza");
                        System.Console.WriteLine("5 Sursa");
                        System.Console.WriteLine("6 Stocare");
                        System.Console.WriteLine("7 Periferice");

                        int cat = int.Parse(System.Console.ReadLine());

                        System.Console.Write("Pret: ");
                        double pret = double.Parse(System.Console.ReadLine());

                        
                        System.Console.WriteLine("Optiuni (introdu numere separate prin virgula):");
                        System.Console.WriteLine("1 Garantie");
                        System.Console.WriteLine("2 Suport Drivere");
                        System.Console.WriteLine("4 Livrare Gratuita");
                        System.Console.WriteLine("8 Retur 30 zile");
                        System.Console.WriteLine("16 Montaj inclus");
                        System.Console.WriteLine("32 Discount");

                        string inputOptiuni = System.Console.ReadLine();
                        Optiuni optiuniProdus = Optiuni.Nimic;

                        if (!string.IsNullOrEmpty(inputOptiuni))
                        {
                            string[] valori = inputOptiuni.Split(',');
                            foreach (var val in valori)
                            {
                                optiuniProdus |= (Optiuni)int.Parse(val.Trim());
                            }
                        }

                        
                        magazin.AdaugaProdus(
                            new Produs(nume, (Categorie)cat, pret, optiuniProdus)
                        );

                        System.Console.WriteLine("Produs adaugat!");
                        break;

                    case 2:
                        
                        var produse = magazin.GetProduse();
                        if (produse.Count == 0)
                        {
                            System.Console.WriteLine("Nu exista produse.");
                        }
                        else
                        {
                            foreach (var p in produse)
                            {
                                System.Console.WriteLine(p);
                            }
                        }
                        break;

                    case 3:
                        
                        System.Console.Write("ID produs de sters: ");
                        int idStergere = int.Parse(System.Console.ReadLine());

                        if (magazin.StergeProdus(idStergere))
                            System.Console.WriteLine("Produs sters!");
                        else
                            System.Console.WriteLine("Produs inexistent!");
                        break;

                    case 4:
                       
                        System.Console.Write("Nume: ");
                        string cautaNume = System.Console.ReadLine();
                        var rez1 = magazin.CautaDupaNume(cautaNume);
                        foreach (var p in rez1)
                        {
                            System.Console.WriteLine(p);
                        }
                        break;

                    case 5:
                        
                        System.Console.Write("Categorie (1-7): ");
                        int c = int.Parse(System.Console.ReadLine());
                        var rez2 = magazin.CautaDupaCategorie((Categorie)c);
                        foreach (var p in rez2)
                        {
                            System.Console.WriteLine(p);
                        }
                        break;

                    case 6:
                        
                        System.Console.Write("Pret maxim: ");
                        double pretMax = double.Parse(System.Console.ReadLine());
                        var rez3 = magazin.CautaDupaPret(pretMax);
                        foreach (var p in rez3)
                        {
                            System.Console.WriteLine(p);
                        }
                        break;

                    case 7:
                        
                        System.Console.WriteLine("Optiune de cautat (ex: 1, 2, 4 etc.):");
                        string inputOptCauta = System.Console.ReadLine();
                        Optiuni cautaOpt = Optiuni.Nimic;

                        if (!string.IsNullOrEmpty(inputOptCauta))
                        {
                            string[] val = inputOptCauta.Split(',');
                            foreach (var v in val)
                            {
                                cautaOpt |= (Optiuni)int.Parse(v.Trim());
                            }
                        }

                        var rez4 = magazin.CautaDupaOptiuni(cautaOpt);
                        foreach (var p in rez4)
                        {
                            System.Console.WriteLine(p);
                        }
                        break;
                }

            } while (optiune != 0);

            System.Console.WriteLine("Aplicatia s-a inchis.");
        }
    }
}