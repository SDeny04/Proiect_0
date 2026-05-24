using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Magazin.Logic;
using Magazin.Models;
using Magazin.WPF.Utils;

namespace Magazin.WPF
{
    public partial class LoginWindow : Window
    {
        private readonly UtilizatorAdmin utilizatorAdmin;
        public Utilizator? LoggedUser { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            utilizatorAdmin = new UtilizatorAdmin();
            GaranteazaUtilizatoriImpliciti();
        }

        private void GaranteazaUtilizatoriImpliciti()
        {
            try
            {
                var lista = utilizatorAdmin.GetUtilizatori();
                
                bool adminExists = lista.Exists(u => u.Username == "admin" && u.Parola == "admin");
                bool clientExists = lista.Exists(u => u.Username == "client" && u.Parola == "client");

                if (!adminExists)
                {
                    utilizatorAdmin.Inregistrare("Administrator", "admin", "admin@magazin.ro", "admin", TipRol.Admin);
                }
                
                if (!clientExists)
                {
                    utilizatorAdmin.Inregistrare("Client Implicit", "client", "client@magazin.ro", "client", TipRol.Client);
                }
            }
            catch (Exception ex)
            {
                NotificationHelper.ShowToast(this, $"Eroare la inițializarea utilizatorilor impliciți:\n{ex.Message}", true);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnGoToRegister_Click(object sender, RoutedEventArgs e)
        {
            PanelLogin.Visibility = Visibility.Collapsed;
            PanelRegister.Visibility = Visibility.Visible;
            TxtError.Visibility = Visibility.Collapsed;
        }

        private void BtnGoToLogin_Click(object sender, RoutedEventArgs e)
        {
            PanelRegister.Visibility = Visibility.Collapsed;
            PanelLogin.Visibility = Visibility.Visible;
            TxtError.Visibility = Visibility.Collapsed;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtLoginUser.Text.Trim();
            string password = TxtLoginPass.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Introduceți utilizatorul și parola!");
                return;
            }

            var user = utilizatorAdmin.Autentificare(username, password);
            if (user != null)
            {
                LoggedUser = user;
                DialogResult = true;
                Close();
            }
            else
            {
                var users = utilizatorAdmin.GetUtilizatori();
                string userListText = users.Count > 0 
                    ? string.Join(", ", users.Select(u => $"'{u.Username}':'{u.Parola}'"))
                    : "NICIUNUL";
                ShowError($"Nume utilizator sau parolă incorectă!\nConturi în DB: {userListText}");
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string nume = TxtRegNume.Text.Trim();
            string username = TxtRegUser.Text.Trim();
            string email = TxtRegEmail.Text.Trim();
            string password = TxtRegPass.Password.Trim();

            if (string.IsNullOrEmpty(nume) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowError("Toate câmpurile sunt obligatorii pentru înregistrare!");
                return;
            }

            try
            {
                var existing = utilizatorAdmin.GetUtilizatori();
                if (existing.Exists(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    ShowError("Acest nume de utilizator este deja folosit!");
                    return;
                }

                utilizatorAdmin.Inregistrare(nume, username, email, password, TipRol.Client);
                NotificationHelper.ShowToast(this, "Contul a fost creat cu succes! Vă puteți conecta acum.", false);
                
                PanelRegister.Visibility = Visibility.Collapsed;
                PanelLogin.Visibility = Visibility.Visible;
                TxtLoginUser.Text = username;
                TxtLoginPass.Password = password;
                TxtError.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ShowError($"Eroare la înregistrare: {ex.Message}");
            }
        }

        private void LoginInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnLogin_Click(sender, e);
            }
        }

        private void ShowError(string message)
        {
            TxtError.Text = message;
            TxtError.Visibility = Visibility.Visible;
        }
    }
}
