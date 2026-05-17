using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Magazin.Models;

namespace Magazin.WPF
{
    public partial class EditeazaProdusWindow : Window
    {
        public Produs ProdusModificat { get; private set; }
        private int _idProdus;

        private readonly SolidColorBrush colorNormal = new SolidColorBrush(Colors.Black);
        private readonly SolidColorBrush colorError = new SolidColorBrush(Colors.Red);

        public EditeazaProdusWindow(Produs produs)
        {
            InitializeComponent();
            PopuleazaCategorii();
            IncarcaDateProdus(produs);
        }

        private void PopuleazaCategorii()
        {
            CboCategorie.ItemsSource = Enum.GetValues(typeof(Categorie));
        }

        private void IncarcaDateProdus(Produs produs)
        {
            _idProdus = produs.Id;
            
            // Scoatem "(Resigilat)" din nume daca exista, pentru a bifa radio button-ul
            string nume = produs.Nume;
            if (nume.EndsWith("(Resigilat)"))
            {
                RadioResigilat.IsChecked = true;
                nume = nume.Replace("(Resigilat)", "").Trim();
            }
            else
            {
                RadioNou.IsChecked = true;
            }

            TxtNume.Text = nume;
            CboCategorie.SelectedItem = produs.CategorieProdus;
            TxtPret.Text = produs.Pret.ToString();
            TxtStoc.Text = produs.Stoc.ToString();

            if (produs.OptiuniProdus.HasFlag(Optiuni.Garantie)) ChkGarantie.IsChecked = true;
            if (produs.OptiuniProdus.HasFlag(Optiuni.SuportDrivere)) ChkSuportDrivere.IsChecked = true;
            if (produs.OptiuniProdus.HasFlag(Optiuni.LivrareRapida)) ChkLivrareRapida.IsChecked = true;
            if (produs.OptiuniProdus.HasFlag(Optiuni.Returnare14Zile)) ChkReturnare14Zile.IsChecked = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValideazaDatele())
            {
                string nume = TxtNume.Text.Trim();
                if (RadioResigilat?.IsChecked == true)
                {
                    nume += " (Resigilat)";
                }
                Categorie categorie = (Categorie)CboCategorie.SelectedItem;
                double pret = double.Parse(TxtPret.Text.Trim());
                int stoc = int.Parse(TxtStoc.Text.Trim());

                Optiuni optiuni = Optiuni.Niciuna;
                if (ChkGarantie.IsChecked == true) optiuni |= Optiuni.Garantie;
                if (ChkSuportDrivere.IsChecked == true) optiuni |= Optiuni.SuportDrivere;
                if (ChkLivrareRapida.IsChecked == true) optiuni |= Optiuni.LivrareRapida;
                if (ChkReturnare14Zile.IsChecked == true) optiuni |= Optiuni.Returnare14Zile;

                ProdusModificat = new Produs(nume, categorie, pret, optiuni, stoc) { Id = _idProdus };
                DialogResult = true;
            }
        }

        private bool ValideazaDatele()
        {
            bool valid = true;
            string errorMsgs = "";

            if (string.IsNullOrWhiteSpace(TxtNume.Text))
            {
                LblNume.Foreground = colorError;
                valid = false;
                errorMsgs += "• Numele produsului este obligatoriu.\n";
            }
            else
            {
                LblNume.Foreground = colorNormal;
            }

            if (!double.TryParse(TxtPret.Text.Trim(), out double pretValue) || pretValue <= 0)
            {
                LblPret.Foreground = colorError;
                valid = false;
                errorMsgs += "• Prețul trebuie să fie un număr valid, mai mare ca zero.\n";
            }
            else
            {
                LblPret.Foreground = colorNormal;
            }

            if (!int.TryParse(TxtStoc.Text.Trim(), out int stocValue) || stocValue < 0)
            {
                LblStoc.Foreground = colorError;
                valid = false;
                errorMsgs += "• Stocul trebuie să fie un număr întreg pozitiv sau zero.\n";
            }
            else
            {
                LblStoc.Foreground = colorNormal;
            }

            if (!valid)
            {
                TxtError.Text = errorMsgs.TrimEnd();
                TxtError.Visibility = Visibility.Visible;
            }
            else
            {
                TxtError.Visibility = Visibility.Collapsed;
            }

            return valid;
        }

        private void Input_Changed(object sender, TextChangedEventArgs e)
        {
            if (sender == TxtNume) LblNume.Foreground = colorNormal;
            if (sender == TxtPret) LblPret.Foreground = colorNormal;
            if (sender == TxtStoc) LblStoc.Foreground = colorNormal;
            TxtError.Visibility = Visibility.Collapsed;
        }

        private void CboCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LblCategorie.Foreground = colorNormal;
            TxtError.Visibility = Visibility.Collapsed;
        }
    }
}
