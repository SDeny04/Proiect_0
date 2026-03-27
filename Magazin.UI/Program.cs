using System;
using Magazin.Models;
using Magazin.Logic;

namespace Magazin.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            MagazinAdmin magazin = new MagazinAdmin();
            bool ruleaza = true;

            while (ruleaza)
            {
                Console.WriteLine("\n=== Magazin Componente PC ===");
                Console.WriteLine("1. Adauga produs");
                Console.WriteLine("2. Sterge produs");
                Console.WriteLine("3. Afiseaza produse");
                Console.WriteLine("4. Cauta dupa nume");
                Console.WriteLine("5. Cauta dupa categorie");
                Console.WriteLine("6. Cauta dupa pret maxim");
                Console.WriteLine("7. Cauta dupa optiuni");
                Console.WriteLine("8. Modifica produs");
                Console.WriteLine("0. Iesire");
                Console.Write("Optiune: ");
                string opt = Console.ReadLine();

                switch (opt)
                {
                    case "1":
                        Console.Write("Nume produs: ");
                        string nume = Console.ReadLine();

                        Console.WriteLine("Categorie (0=Procesor, 1=PlacaVideo, 2=RAM, 3=PlacaDeBaza, 4=Sursa, 5=Stocare, 6=Periferice):");
                        int cat = int.Parse(Console.ReadLine());

                        Console.Write("Pret: ");
                        double pret = double.Parse(Console.ReadLine());

                        Console.WriteLine("Selecteaza optiuni (separate prin virgula):");
                        Console.WriteLine("1 - Garantie, 2 - SuportDrivere, 4 - LivrareRapida, 8 - Returnare14Zile");

                        string[] opturiInput = Console.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries);
                        Optiuni optiuni = Optiuni.Niciuna;

                        foreach (var o in opturiInput)
                        {
                            if (int.TryParse(o.Trim(), out int val))
                                optiuni |= (Optiuni)val;
                        }

                        Produs p = new Produs(nume, (Categorie)cat, pret, optiuni);
                        magazin.AdaugaProdus(p);
                        Console.WriteLine("Produs adaugat cu succes!");
                        break;

                    case "2":
                        Console.Write("ID produs de sters: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                            magazin.StergeProdus(id);
                        else
                            Console.WriteLine("ID invalid!");
                        break;

                    case "3":
                        Console.WriteLine("\nLista produse:");
                        foreach (var prod in magazin.GetProduse())
                            Console.WriteLine(prod);
                        break;

                    case "4":
                        Console.Write("Cauta dupa nume: ");
                        string cautaNume = Console.ReadLine();
                        var rezultateNume = magazin.CautaDupaNume(cautaNume);
                        if (rezultateNume.Count == 0)
                            Console.WriteLine("Nu s-au gasit produse.");
                        else
                            foreach (var prod in rezultateNume)
                                Console.WriteLine(prod);
                        break;

                    case "5":
                        Console.WriteLine("Categorie (0=Procesor, 1=PlacaVideo, 2=RAM, 3=PlacaDeBaza, 4=Sursa, 5=Stocare, 6=Periferice):");
                        int c = int.Parse(Console.ReadLine());
                        var rezultateCat = magazin.CautaDupaCategorie((Categorie)c);
                        if (rezultateCat.Count == 0)
                            Console.WriteLine("Nu s-au gasit produse.");
                        else
                            foreach (var prod in rezultateCat)
                                Console.WriteLine(prod);
                        break;

                    case "6":
                        Console.Write("Pret maxim: ");
                        if (double.TryParse(Console.ReadLine(), out double pretMax))
                        {
                            var rezultatePret = magazin.CautaDupaPret(pretMax);
                            if (rezultatePret.Count == 0)
                                Console.WriteLine("Nu s-au gasit produse.");
                            else
                                foreach (var prod in rezultatePret)
                                    Console.WriteLine(prod);
                        }
                        else
                        {
                            Console.WriteLine("Pret invalid!");
                        }
                        break;

                    case "7":
                        Console.WriteLine("Selecteaza optiuni de cautare (separate prin virgula):");
                        Console.WriteLine("1 - Garantie, 2 - SuportDrivere, 4 - LivrareRapida, 8 - Returnare14Zile");

                        string[] opturiInputCautare = Console.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries);
                        Optiuni optiuniCautare = Optiuni.Niciuna;

                        foreach (var o in opturiInputCautare)
                        {
                            if (int.TryParse(o.Trim(), out int val))
                                optiuniCautare |= (Optiuni)val;
                        }

                        var rezultateOptiuni = magazin.CautaDupaOptiuni(optiuniCautare);
                        if (rezultateOptiuni.Count == 0)
                            Console.WriteLine("Nu s-au gasit produse.");
                        else
                            foreach (var prod in rezultateOptiuni)
                                Console.WriteLine(prod);
                        break;

                    case "8":
                        Console.Write("ID produs de modificat: ");
                        if (int.TryParse(Console.ReadLine(), out int idMod))
                        {
                            Console.Write("Nume nou (lasa gol pentru a pastra): ");
                            string numeNou = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(numeNou)) numeNou = null;

                            Console.WriteLine("Categorie noua (0=Procesor, 1=PlacaVideo, 2=RAM, 3=PlacaDeBaza, 4=Sursa, 5=Stocare, 6=Periferice) [lasa gol pentru a pastra]:");
                            string catInput = Console.ReadLine();
                            Categorie? catNou = string.IsNullOrWhiteSpace(catInput) ? (Categorie?)null : (Categorie)int.Parse(catInput);

                            Console.Write("Pret nou (lasa gol pentru a pastra): ");
                            string pretInput = Console.ReadLine();
                            double? pretNou = string.IsNullOrWhiteSpace(pretInput) ? (double?)null : double.Parse(pretInput);

                            Console.WriteLine("Selecteaza optiuni noi (separate prin virgula) [lasa gol pentru a pastra]:");
                            Console.WriteLine("1 - Garantie, 2 - SuportDrivere, 4 - LivrareRapida, 8 - Returnare14Zile");
                            string optInput = Console.ReadLine();
                            Optiuni? optiuniMod = null;

                            if (!string.IsNullOrWhiteSpace(optInput))
                            {
                                optiuniMod = Optiuni.Niciuna;
                                string[] opturiInputMod = optInput.Split(',', StringSplitOptions.RemoveEmptyEntries);
                                foreach (var o in opturiInputMod)
                                {
                                    if (int.TryParse(o.Trim(), out int val))
                                        optiuniMod |= (Optiuni)val;
                                }
                            }

                            // Apelăm funcția de modificare din MagazinAdmin
                            magazin.ModificaProdus(idMod, numeNou, catNou, pretNou, optiuniMod);
                            Console.WriteLine("Modificare aplicata cu succes!");
                        }
                        else
                        {
                            Console.WriteLine("ID invalid!");
                        }
                        break;

                    case "0":
                        ruleaza = false;
                        break;

                    default:
                        Console.WriteLine("Optiune invalida!");
                        break;
                }
            }
        }
    }
}