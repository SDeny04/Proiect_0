using Magazin.Logic;
using Magazin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Magazin.ConsoleApp
{
    class Program
    {
        static MagazinAdmin magazin = new MagazinAdmin();
        static UtilizatorAdmin uAdmin = new UtilizatorAdmin();
        static ComandaAdmin cAdmin = new ComandaAdmin();
        static Utilizator utilizatorLogat = null;

        static void Main(string[] args)
        {
            bool ruleaza = true;
            while (ruleaza)
            {
                if (utilizatorLogat == null) ruleaza = MeniuStart();
                else if (utilizatorLogat.Rol == TipRol.Admin) MeniuAdmin();
                else MeniuClient();
            }
        }

        static bool MeniuStart()
        {
            Console.WriteLine("\n=== MAGAZIN PC ===");
            Console.WriteLine("1. Login | 2. Creare Cont | 0. Iesire");
            Console.Write("Optiune: ");
            string opt = Console.ReadLine();
            if (opt == "1")
            {
                Console.Write("User: "); string u = Console.ReadLine();
                Console.Write("Pass: "); string p = Console.ReadLine();
                utilizatorLogat = uAdmin.Autentificare(u, p);
            }
            else if (opt == "2")
            {
                Console.Write("User nou: "); string us = Console.ReadLine();
                Console.Write("Parola noua: "); string pa = Console.ReadLine();
                uAdmin.Inregistrare(us, us, $"{us}@email.ro", pa, TipRol.Client);
                Console.WriteLine("Cont creat!");
            }
            return opt != "0";
        }

        static void MeniuAdmin()
        {
            Console.WriteLine($"\n--- ADMIN: {utilizatorLogat.Username} ---");
            Console.WriteLine("1. Adauga Produs | 2. Sterge | 3. Modifica | 4. Utilizatori | 5. Comenzi | 6. Produse | 0. Logout");
            Console.Write("Optiune: ");
            string opt = Console.ReadLine();
            switch (opt)
            {
                case "1": AdaugaProdusMeniu(); break;
                case "2":
                    Console.Write("ID de sters: ");
                    if (int.TryParse(Console.ReadLine(), out int id)) magazin.StergeProdus(id);
                    break;
                case "3": ModificaProdusMeniu(); break;
                case "4": foreach (var u in uAdmin.GetUtilizatori()) Console.WriteLine(u); break;
                case "5": foreach (var c in cAdmin.GetComenzi()) Console.WriteLine(c); break;
                case "6": foreach (var p in magazin.GetProduse()) Console.WriteLine(p); break;
                case "0": utilizatorLogat = null; break;
            }
        }

        static void MeniuClient()
        {
            Console.WriteLine($"\n--- CLIENT: {utilizatorLogat.Username} ---");
            Console.WriteLine("1. Catalog | 2. Cautare | 3. Cumpara | 4. Istoric | 0. Logout");
            Console.Write("Optiune: ");
            string opt = Console.ReadLine();
            switch (opt)
            {
                case "1": foreach (var p in magazin.GetProduse()) Console.WriteLine(p); break;
                case "2": MeniuCautari(); break;
                case "3": PlaseazaComanda(); break;
                case "4":
                    var mele = cAdmin.GetComenziClient(utilizatorLogat.Id);
                    foreach (var c in mele) Console.WriteLine(c);
                    break;
                case "0": utilizatorLogat = null; break;
            }
        }


        static void AdaugaProdusMeniu()
        {
            Console.WriteLine("\n--- ADAUGARE PRODUS NOU ---");
            Console.Write("Nume: "); string nume = Console.ReadLine();

            Console.WriteLine("Categorii disponibile: 0:Procesor, 1:PlacaVideo, 2:RAM, 3:PlacaBaza, 4:Sursa, 5:Stocare, 6:Periferice");
            Console.Write("Alege Categorie (ID): ");
            int cat = int.Parse(Console.ReadLine());

            Console.Write("Pret: ");
            double pret = double.Parse(Console.ReadLine());

            Console.WriteLine("Optiuni (poti alege mai multe adunand numerele sau cu virgula):");
            Console.WriteLine("1:Garantie, 2:SuportDrivere, 4:LivrareRapida, 8:Returnare14Zile");
            Console.Write("Introdu codurile (ex: 1,4): ");
            Optiuni optiuni = ParseOptiuni(Console.ReadLine());

            Console.Write("Stoc: ");
            int stoc = int.Parse(Console.ReadLine());

            magazin.AdaugaProdus(new Produs(nume, (Categorie)cat, pret, optiuni,stoc));
            Console.WriteLine("Produs adaugat!");
        }

        static void ModificaProdusMeniu()
        {
            Console.Write("\nID produs de modificat: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("--- Lasa GOL daca nu vrei sa modifici campul respectiv ---");

                Console.Write("Nume nou: ");
                string n = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(n)) n = null;

                Console.WriteLine("Categorii: 0:Procesor, 1:PlacaVideo, 2:RAM, 3:PlacaBaza, 4:Sursa, 5:Stocare, 6:Periferice");
                Console.Write("Categorie noua (ID): ");
                string cInput = Console.ReadLine();
                Categorie? c = string.IsNullOrWhiteSpace(cInput) ? (Categorie?)null : (Categorie)int.Parse(cInput);

                Console.Write("Pret nou: ");
                string pInput = Console.ReadLine();
                double? p = string.IsNullOrWhiteSpace(pInput) ? (double?)null : double.Parse(pInput);

                Console.WriteLine("Optiuni: 1:Garantie, 2:SuportDrivere, 4:LivrareRapida, 8:Returnare14Zile");
                Console.Write("Optiuni noi (ex: 1,8): ");
                string oInput = Console.ReadLine();
                Optiuni? o = string.IsNullOrWhiteSpace(oInput) ? (Optiuni?)null : ParseOptiuni(oInput);

                magazin.ModificaProdus(id, n, c, p, o);
                Console.WriteLine("Modificare finalizata!");
            }
        }

        static Optiuni ParseOptiuni(string input)
        {
            Optiuni rezultat = Optiuni.Niciuna;
            if (string.IsNullOrWhiteSpace(input)) return rezultat;

            string[] parti = input.Split(',');
            foreach (var p in parti)
            {
                if (int.TryParse(p.Trim(), out int val))
                    rezultat |= (Optiuni)val;
            }
            return rezultat;
        }


        static void MeniuCautari()
        {
            Console.WriteLine("\nCauta dupa:");
            Console.WriteLine("1. Nume | 2. Categorie | 3. Pret Maxim | 4. Optiuni (Garantie/Livrare)");
            string tip = Console.ReadLine();

            switch (tip)
            {
                case "1":
                    Console.Write("Introdu numele cautat: ");
                    var r1 = magazin.CautaDupaNume(Console.ReadLine());
                    foreach (var p in r1) Console.WriteLine(p);
                    break;
                case "2":
                    Console.Write("ID Categorie (0-Procesor, 1-PlacaVideo, etc): ");
                    if (int.TryParse(Console.ReadLine(), out int cat))
                    {
                        var r2 = magazin.CautaDupaCategorie((Categorie)cat);
                        foreach (var p in r2) Console.WriteLine(p);
                    }
                    break;
                case "3":
                    Console.Write("Pret maxim: ");
                    if (double.TryParse(Console.ReadLine(), out double pMax))
                    {
                        var r3 = magazin.CautaDupaPret(pMax);
                        foreach (var p in r3) Console.WriteLine(p);
                    }
                    break;
                case "4":
                    Console.WriteLine("Optiuni: 1:Garantie, 2:SuportDrivere, 4:LivrareRapida, 8:Returnare14Zile");
                    Console.Write("Introdu codul sau codurile separate prin virgula (ex: 1 sau 1,4): ");
                    string inputOpt = Console.ReadLine();

                    Optiuni cautate = ParseOptiuni(inputOpt);

                    var rezultate = magazin.CautaDupaOptiuni(cautate);

                    if (rezultate.Count == 0)
                        Console.WriteLine("Nu s-au gasit produse cu aceste optiuni.");
                    else
                        foreach (var p in rezultate) Console.WriteLine(p);
                    break;
            }
        }
        static void PlaseazaComanda()
        {
            
            Console.WriteLine("\n--- PLASARE COMANDA ---");
            foreach (var p in magazin.GetProduse())
            {
                Console.WriteLine(p);
            }

            
            Console.Write("\nIntrodu ID-urile dorite (ex: 1,2,5): ");
            string input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                
                List<int> listaIds = input.Split(',')
                                          .Select(s => s.Trim())
                                          .Where(s => int.TryParse(s, out _))
                                          .Select(int.Parse)
                                          .ToList();

                if (listaIds.Count > 0)
                {
                    string rezultat = cAdmin.ExecutaComanda(utilizatorLogat.Id, listaIds, magazin);
                        Console.WriteLine(rezultat);
                }
                else
                {
                    Console.WriteLine("Produse indisponibile");
                }
            }
        }

    }
}