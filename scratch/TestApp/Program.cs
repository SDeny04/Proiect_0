using System;
using System.IO;
using System.Linq;

namespace TestApp
{
    class Program
    {
        static void Main()
        {
            string produsePath = @"B:\Visual Studio\Proiect_0\Magazin.UI\bin\Debug\net10.0\produse.txt";
            if (!File.Exists(produsePath)) { Console.WriteLine("File not found"); return; }
            
            var lines = File.ReadAllLines(produsePath);
            Console.WriteLine($"Numar linii initiale: {lines.Length}");
            if (lines.Length > 0)
            {
                var campuri = lines[0].Split(';');
                Console.WriteLine($"Linia 0 stoc inainte: {campuri[5]}");
                campuri[5] = "888";
                lines[0] = string.Join(";", campuri);
                
                File.WriteAllLines(produsePath, lines);
                Console.WriteLine("Am scris 888.");
            }

            var lines2 = File.ReadAllLines(produsePath);
            Console.WriteLine($"Linia 0 stoc dupa: {lines2[0].Split(';')[5]}");
        }
    }
}
