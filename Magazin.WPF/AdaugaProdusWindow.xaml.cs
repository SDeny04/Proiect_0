using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Magazin.Models;

namespace Magazin.WPF
{
    public partial class AdaugaProdusWindow : Window
    {
        public Produs? ProdusNou { get; private set; }

        private readonly SolidColorBrush colorNormal = new SolidColorBrush(Colors.Black);
        private readonly SolidColorBrush colorError = new SolidColorBrush(Colors.Red);

        public AdaugaProdusWindow()
        {
            InitializeComponent();
            PopuleazaCategorii();
        }

        private void PopuleazaCategorii()
        {
            CboCategorie.ItemsSource = Enum.GetValues(typeof(Categorie));
            CboCategorie.SelectedIndex = 0;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValideazaDatele())
            {
                // Dupa ce validarea a trecut, ne construim produsul
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

                ProdusNou = new Produs(nume, categorie, pret, optiuni, stoc);
                DialogResult = true; // Închide fereastra cu succes
            }
        }

        private bool ValideazaDatele()
        {
            bool valid = true;
            string errorMsgs = "";

            // Nume validation
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

            // Pret validation
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

            // Stoc validation
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

            // Daca sunt erori, le afisam
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

        // Evenimente pentru resetarea culorilor in momentul în care userul corecteaza
        private void Input_Changed(object sender, TextChangedEventArgs e)
        {
            if (sender == TxtNume) LblNume.Foreground = colorNormal;
            if (sender == TxtPret) LblPret.Foreground = colorNormal;
            if (sender == TxtStoc) LblStoc.Foreground = colorNormal;
            
            // Ascundem eroarea generală odată ce utilizatorul modifică ceva
            TxtError.Visibility = Visibility.Collapsed;
        }

        private void CboCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LblCategorie.Foreground = colorNormal;
            TxtError.Visibility = Visibility.Collapsed;
        }
    }
}
