using System;
using System.Windows;
using Magazin.Models;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Magazin.WPF.Utils;

namespace Magazin.WPF
{
    public partial class AdaugaProdusWindow : Window
    {
        public Produs? ProdusNou { get; private set; }
        private readonly ProductViewModel viewModel;

        public AdaugaProdusWindow()
        {
            InitializeComponent();
            
            viewModel = new ProductViewModel();
            DataContext = viewModel;

            PopuleazaCategorii();
        }

        private void PopuleazaCategorii()
        {
            CboCategorie.ItemsSource = Enum.GetValues(typeof(Categorie));
            viewModel.CategorieProdus = Categorie.Procesor;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Validate();
            
            if (viewModel.IsValid)
            {
                ProdusNou = viewModel.GetProdus();
                DialogResult = true; // Inchide fereastra cu succes
            }
            else
            {
                NotificationHelper.ShowToast(this, "Vă rugăm să corectați erorile din formular înainte de a salva produsul.", true);
            }
        }
    }
}
