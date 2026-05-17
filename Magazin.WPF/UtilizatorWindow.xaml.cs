using Magazin.Models;
using System.Windows;

namespace Magazin.WPF
{
    public partial class UtilizatorWindow : Window
    {
        public Utilizator UtilizatorModificat { get; private set; }
        private int _idUtilizator = 0;

        public UtilizatorWindow(Utilizator utilizator = null)
        {
            InitializeComponent();

            if (utilizator != null)
            {
                TxtTitlu.Text = "Editeaza Utilizator";
                _idUtilizator = utilizator.Id;
                TxtNume.Text = utilizator.Nume;
                TxtUsername.Text = utilizator.Username;
                TxtEmail.Text = utilizator.Email;
                TxtParola.Text = utilizator.Parola;
                CboRol.SelectedIndex = utilizator.Rol == TipRol.Client ? 0 : 1;
            }
            else
            {
                CboRol.SelectedIndex = 0;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNume.Text) || 
                string.IsNullOrWhiteSpace(TxtUsername.Text) || 
                string.IsNullOrWhiteSpace(TxtEmail.Text) || 
                string.IsNullOrWhiteSpace(TxtParola.Text))
            {
                TxtError.Text = "Toate campurile sunt obligatorii!";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            TipRol rol = CboRol.SelectedIndex == 0 ? TipRol.Client : TipRol.Admin;

            // Daca e editare, pastram ID-ul, daca e nou va fi ignorat/setat la salvare
            UtilizatorModificat = new Utilizator(_idUtilizator, TxtNume.Text.Trim(), TxtUsername.Text.Trim(), TxtEmail.Text.Trim(), TxtParola.Text.Trim(), rol);
            
            DialogResult = true;
        }
    }
}
