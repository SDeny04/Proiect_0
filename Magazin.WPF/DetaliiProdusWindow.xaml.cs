using System.Windows;
using Magazin.Models;

namespace Magazin.WPF
{
    public partial class DetaliiProdusWindow : Window
    {
        public DetaliiProdusWindow(Produs produs)
        {
            InitializeComponent();
            TxtNume.Text = produs.Nume;
            TxtCategorie.Text = produs.CategorieProdus.ToString();
            TxtPret.Text = $"{produs.Pret:N2} RON";
            TxtStoc.Text = $"{produs.Stoc} bucăți";
            
            if (produs.Stoc == 0)
            {
                TxtStoc.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(239, 68, 68)); // red
            }

            TxtOptiuni.Text = produs.OptiuniProdus == Optiuni.Niciuna ? "Fără opțiuni extra" : produs.OptiuniProdus.ToString();
        }

        private void BtnInchide_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
