using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Magazin.Logic;
using Magazin.Models;
using Magazin.WPF.Utils;

namespace Magazin.WPF
{
    public partial class CosWindow : Window
    {
        private List<Produs> produseInCos;
        private int idClient;
        private MagazinAdmin magazin;
        private ComandaAdmin comandaAdmin;

        public bool ComandaPlasata { get; private set; } = false;

        public CosWindow(List<Produs> produse, int idClient, MagazinAdmin magazin, ComandaAdmin comandaAdmin)
        {
            InitializeComponent();
            this.produseInCos = produse;
            this.idClient = idClient;
            this.magazin = magazin;
            this.comandaAdmin = comandaAdmin;

            IncarcaCos();
        }

        private void IncarcaCos()
        {
            ListProduseCos.ItemsSource = null;
            ListProduseCos.ItemsSource = produseInCos;

            double total = produseInCos.Sum(p => p.Pret);
            TxtTotal.Text = $"{total:N2} RON";
        }

        private void BtnStergeProdus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Produs produs)
            {
                produseInCos.Remove(produs);
                IncarcaCos();
            }
        }

        private void BtnPlaseazaComanda_Click(object sender, RoutedEventArgs e)
        {
            if (produseInCos.Count == 0)
            {
                NotificationHelper.ShowToast(this, "Coșul este gol!", true);
                return;
            }

            var confirm = new ConfirmWindow("Sunteți sigur că doriți să finalizați comanda?");
            confirm.Owner = this;
            if (confirm.ShowDialog() != true)
            {
                return;
            }

            var idProduse = produseInCos.Select(p => p.Id).ToList();
            string mesaj = comandaAdmin.ExecutaComanda(idClient, idProduse, magazin);

            if (mesaj.Contains("succes"))
            {
                ComandaPlasata = true;
                NotificationHelper.ShowToast(this, mesaj, false);
                produseInCos.Clear();
                this.Close();
            }
            else
            {
                NotificationHelper.ShowToast(this, mesaj, true);
                IncarcaCos(); // Refresh in case stock was depleted
            }
        }
    }
}
