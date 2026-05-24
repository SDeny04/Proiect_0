using System;
using System.Windows;
using Magazin.Models;
using Magazin.WPF.Utils;

namespace Magazin.WPF
{
    public partial class EditeazaProdusWindow : Window
    {
        public Produs? ProdusModificat { get; private set; }
        private readonly int idProdus;
        private readonly ProductViewModel viewModel;

        public EditeazaProdusWindow(Produs produs)
        {
            InitializeComponent();

            idProdus = produs.Id;
            viewModel = new ProductViewModel();
            DataContext = viewModel;

            PopuleazaCategorii();
            viewModel.LoadFromProdus(produs);
        }

        private void PopuleazaCategorii()
        {
            CboCategorie.ItemsSource = Enum.GetValues(typeof(Categorie));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Validate();
            
            if (viewModel.IsValid)
            {
                ProdusModificat = viewModel.GetProdus(idProdus);
                DialogResult = true;
            }
            else
            {
                NotificationHelper.ShowToast(this, "Vă rugăm să corectați erorile din formular înainte de a salva modificările.", true);
            }
        }
    }
}
