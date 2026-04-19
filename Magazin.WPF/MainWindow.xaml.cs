using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Magazin.Logic;
using Magazin.Models;
using Magazin.StocareDate;

namespace Magazin.WPF
{
    /// <summary>
    /// View model wrapper for Comanda to expose ProduseText for binding.
    /// </summary>
    public class ComandaViewModel
    {
        public int Id { get; set; }
        public int IdClient { get; set; }
        public List<int> IdProduse { get; set; } = new();
        public DateTime DataComenzii { get; set; }
        public double Total { get; set; }
        public string ProduseText => string.Join(", ", IdProduse);

        public ComandaViewModel(Comanda c)
        {
            Id = c.Id;
            IdClient = c.IdClient;
            IdProduse = c.IdProduse;
            DataComenzii = c.DataComenzii;
            Total = c.Total;
        }
    }

    public partial class MainWindow : Window
    {
        private MagazinAdmin magazin = null!;
        private ComandaAdmin comandaAdmin = null!;
        private UtilizatorAdmin utilizatorAdmin = null!;

        private List<Produs> toateProdusele = new();
        private List<ComandaViewModel> toateComenzile = new();
        private List<Utilizator> totiUtilizatorii = new();

        private int tabActiv = 0; // 0=Produse, 1=Comenzi, 2=Utilizatori

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Caută directorul cu fișierele de date (Magazin.UI/bin/Debug)
                ConfigureazaCaleFisiere();

                magazin = new MagazinAdmin();
                comandaAdmin = new ComandaAdmin();
                utilizatorAdmin = new UtilizatorAdmin();
                IncarcaDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea datelor:\n{ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfigureazaCaleFisiere()
        {
            // Caută fișierul produse.txt mergând în sus de la exe
            string? dir = AppDomain.CurrentDomain.BaseDirectory;

            // Urcă până la directorul soluției (unde sunt Magazin.UI, Magazin.WPF etc.)
            while (dir != null)
            {
                string uiPath = Path.Combine(dir, "Magazin.UI", "bin", "Debug", "net10.0");
                if (File.Exists(Path.Combine(uiPath, "produse.txt")))
                {
                    AdministrareProduseFisierText.BasePath = uiPath;
                    return;
                }
                dir = Directory.GetParent(dir)?.FullName;
            }

            // Dacă nu găsește, verifică directorul curent
            if (File.Exists("produse.txt"))
                return;

            MessageBox.Show("Nu s-au găsit fișierele de date (produse.txt, comenzi.txt, utilizatori.txt).\n" +
                           "Asigurați-vă că aplicația consolă a fost rulată cel puțin o dată.",
                           "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void IncarcaDate()
        {
            try
            {
                toateProdusele = magazin.GetProduse();
                toateComenzile = comandaAdmin.GetComenzi().Select(c => new ComandaViewModel(c)).ToList();
                totiUtilizatorii = utilizatorAdmin.GetUtilizatori();
            }
            catch
            {
                toateProdusele = new();
                toateComenzile = new();
                totiUtilizatorii = new();
            }

            ActualizeazaStatistici();
            ActualizeazaGrid();
        }

        // ═══════ STATISTICS ═══════
        private void ActualizeazaStatistici()
        {
            // Produse
            TxtTotalProduse.Text = toateProdusele.Count.ToString();
            int stocTotal = toateProdusele.Sum(p => p.Stoc);
            TxtStocTotal.Text = $"Stoc total: {stocTotal:N0} buc.";

            // Comenzi
            TxtTotalComenzi.Text = toateComenzile.Count.ToString();
            double venituri = toateComenzile.Sum(c => c.Total);
            TxtVenituri.Text = $"Venituri: {venituri:N2} RON";

            // Utilizatori
            TxtTotalUtilizatori.Text = totiUtilizatorii.Count.ToString();
            int admini = totiUtilizatorii.Count(u => u.Rol == TipRol.Admin);
            int clienti = totiUtilizatorii.Count(u => u.Rol == TipRol.Client);
            TxtAdmini.Text = $"Admini: {admini} | Clienți: {clienti}";

            // Categorii
            if (toateProdusele.Count > 0)
            {
                var categoriiDistincte = toateProdusele.Select(p => p.CategorieProdus).Distinct().Count();
                TxtTotalCategorii.Text = categoriiDistincte.ToString();

                var topCategorie = toateProdusele
                    .GroupBy(p => p.CategorieProdus)
                    .OrderByDescending(g => g.Count())
                    .First();
                TxtCategTop.Text = $"Top: {topCategorie.Key} ({topCategorie.Count()})";
            }
            else
            {
                TxtTotalCategorii.Text = "0";
                TxtCategTop.Text = "-";
            }
        }

        // ═══════ TAB SWITCHING ═══════
        private void SetTabActiv(int tab)
        {
            tabActiv = tab;

            // Reset styles
            BtnTabProduse.Style = (Style)FindResource("TabButtonStyle");
            BtnTabComenzi.Style = (Style)FindResource("TabButtonStyle");
            BtnTabUtilizatori.Style = (Style)FindResource("TabButtonStyle");

            // Set active
            switch (tab)
            {
                case 0:
                    BtnTabProduse.Style = (Style)FindResource("TabButtonActiveStyle");
                    break;
                case 1:
                    BtnTabComenzi.Style = (Style)FindResource("TabButtonActiveStyle");
                    break;
                case 2:
                    BtnTabUtilizatori.Style = (Style)FindResource("TabButtonActiveStyle");
                    break;
            }

            // Visibility
            GridProduse.Visibility = tab == 0 ? Visibility.Visible : Visibility.Collapsed;
            GridComenzi.Visibility = tab == 1 ? Visibility.Visible : Visibility.Collapsed;
            GridUtilizatori.Visibility = tab == 2 ? Visibility.Visible : Visibility.Collapsed;

            TxtSearch.Text = "";
            ActualizeazaGrid();
        }

        private void BtnTabProduse_Click(object sender, RoutedEventArgs e) => SetTabActiv(0);
        private void BtnTabComenzi_Click(object sender, RoutedEventArgs e) => SetTabActiv(1);
        private void BtnTabUtilizatori_Click(object sender, RoutedEventArgs e) => SetTabActiv(2);

        // ═══════ SEARCH / FILTER ═══════
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualizeazaGrid();
        }

        private void ActualizeazaGrid()
        {
            string filtru = TxtSearch?.Text?.Trim().ToLower() ?? "";

            switch (tabActiv)
            {
                case 0:
                    var produseFiltrate = string.IsNullOrEmpty(filtru)
                        ? toateProdusele
                        : toateProdusele.Where(p =>
                            p.Nume.ToLower().Contains(filtru) ||
                            p.CategorieProdus.ToString().ToLower().Contains(filtru) ||
                            p.Id.ToString().Contains(filtru)
                        ).ToList();

                    GridProduse.ItemsSource = produseFiltrate;
                    VerificaEmpty(produseFiltrate.Count);
                    break;

                case 1:
                    var comenziFiltrate = string.IsNullOrEmpty(filtru)
                        ? toateComenzile
                        : toateComenzile.Where(c =>
                            c.Id.ToString().Contains(filtru) ||
                            c.IdClient.ToString().Contains(filtru) ||
                            c.Total.ToString("N2").Contains(filtru)
                        ).ToList();

                    GridComenzi.ItemsSource = comenziFiltrate;
                    VerificaEmpty(comenziFiltrate.Count);
                    break;

                case 2:
                    var utilizatoriFiltrati = string.IsNullOrEmpty(filtru)
                        ? totiUtilizatorii
                        : totiUtilizatorii.Where(u =>
                            u.Nume.ToLower().Contains(filtru) ||
                            u.Username.ToLower().Contains(filtru) ||
                            u.Email.ToLower().Contains(filtru) ||
                            u.Rol.ToString().ToLower().Contains(filtru)
                        ).ToList();

                    GridUtilizatori.ItemsSource = utilizatoriFiltrati;
                    VerificaEmpty(utilizatoriFiltrati.Count);
                    break;
            }
        }

        private void VerificaEmpty(int count)
        {
            PanelEmpty.Visibility = count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        // ═══════ ADD PRODUCT ═══════
        private void BtnAdaugaProdus_Click(object sender, RoutedEventArgs e)
        {
            var adaugaWin = new AdaugaProdusWindow
            {
                Owner = this // Păstrează fereastra în centrul părințelui
            };

            if (adaugaWin.ShowDialog() == true && adaugaWin.ProdusNou != null)
            {
                try
                {
                    // Salvam în fisier via logic layer
                    magazin.AdaugaProdus(adaugaWin.ProdusNou);
                    
                    // Reîncarcă datele curat
                    IncarcaDate();

                    // Comutăm automat pe tab-ul de produse ca utilizatorul să vadă adăugarea
                    SetTabActiv(0);
                    
                    // (Opțional) curățăm căutarea ca să se vadă produsul dacă search-ul era activ
                    TxtSearch.Text = "";
                }
                catch (Exception ex)
                {
                   MessageBox.Show($"Eroare la adăugarea produsului:\n{ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ═══════ REFRESH ═══════
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                magazin = new MagazinAdmin();
                comandaAdmin = new ComandaAdmin();
                utilizatorAdmin = new UtilizatorAdmin();
                IncarcaDate();
                MessageBox.Show("Datele au fost reîncărcate cu succes!",
                    "Reîncărcare", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la reîncărcare:\n{ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
