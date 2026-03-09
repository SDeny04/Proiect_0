using System;
using System.IO;

namespace MagazinConsole
{
    public class Utilizator
    {
        public static string Login()
        {
            string path = "Users.txt";

            if (!File.Exists(path))
            {
                Console.WriteLine("Fisierul Users.txt nu exista!");
                return null;
            }

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Parola: ");
            string parola = Console.ReadLine();

            string[] linii = File.ReadAllLines(path);

            foreach (string linie in linii)
            {
                string[] date = linie.Split(';');

                if (date[0] == username && date[1] == parola)
                {
                    Console.WriteLine("Login reusit!");
                    return date[2]; // rolul
                }
            }

            Console.WriteLine("Username sau parola gresita.");
            return null;
        }
    }
}