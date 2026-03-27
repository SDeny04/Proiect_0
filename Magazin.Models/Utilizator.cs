using System;

namespace Magazin.Models
{
    public enum TipRol
    {
        Client = 0,
        Admin = 1
    }

    public class Utilizator
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Parola { get; set; } // Într-o aplicație reală, aceasta se criptează!
        public TipRol Rol { get; set; }

        public Utilizator(int id, string nume, string username, string email, string parola, TipRol rol)
        {
            Id = id;
            Nume = nume;
            Username = username;
            Email = email;
            Parola = parola;
            Rol = rol;
        }

        public override string ToString()
        {
            return $"[{Id}] {Nume} (@{Username}) - Rol: {Rol}";
        }
    }
}