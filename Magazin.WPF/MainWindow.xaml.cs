using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Magazin.Logic;
using Magazin.Models;
using Magazin.StocareDate;
using Magazin.WPF.Utils;

namespace Magazin.WPF
{
    public class ComandaViewModel
    {
        public int Id { get; set; }
        public int IdClient { get; set; }
        public string ProduseText { get; set; }
        public string DataComenzii { get; set; }
        public string Total { get; set; }

        public ComandaViewModel(Comanda c, List<Produs> produse)
        {
            Id = c.Id;
            IdClient = c.IdClient;
            Total = c.Total.ToString("N2");
            DataComenzii = c.DataComenzii.ToString("dd/MM/yyyy HH:mm");

            var numeProduse = c.IdProduse.Select(id => 
            {
                var prod = produse.FirstOrDefault(p => p.Id == id);
                return prod != null ? prod.Nume : $"Produs necunoscut ({id})";
            });
            ProduseText = string.Join(", ", numeProduse);
        }
    }

    public partial class MainWindow : Window
    {
        private readonly Utilizator utilizatorConectat;
        private MagazinAdmin magazin = null!;
        private ComandaAdmin comandaAdmin = null!;
        private UtilizatorAdmin utilizatorAdmin = null!;

        private List<Produs> toateProdusele = new();
        private List<ComandaViewModel> toateComenzile = new();
        private List<Utilizator> totiUtilizatorii = new();
        private List<Produs> cosCumparaturi = new();

        private int tabActiv = 0; // 0=Produse, 1=Comenzi, 2=Utilizatori

        public MainWindow() : this(new Utilizator(1, "Administrator", "admin", "admin@magazin.ro", "admin", TipRol.Admin))
        {
        }

        public MainWindow(Utilizator user)
        {
            utilizatorConectat = user;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfigureazaCaleFisiere();

                magazin = new MagazinAdmin();
                comandaAdmin = new ComandaAdmin();
                utilizatorAdmin = new UtilizatorAdmin();
                
                // Populam Categoriile din Left Sidebar
                if (ListCategorii != null)
                {
                    ListCategorii.Items.Clear();
                    ListCategorii.Items.Add(new ListBoxItem { Content = "Toate", Tag = "Toate" });
                    foreach (Categorie cat in Enum.GetValues(typeof(Categorie)))
                    {
                        ListCategorii.Items.Add(new ListBoxItem { Content = cat.ToString(), Tag = cat.ToString() });
                    }
                    ListCategorii.SelectedIndex = 0;
                }

                if (TxtUsernameConectat != null) TxtUsernameConectat.Text = utilizatorConectat.Nume;
                if (TxtRolConectat != null)
                {
                    TxtRolConectat.Text = utilizatorConectat.Rol == TipRol.Admin ? "Administrator" : "Client";
                    TxtRolConectat.Foreground = new SolidColorBrush(utilizatorConectat.Rol == TipRol.Admin ? Color.FromRgb(59, 130, 246) : Color.FromRgb(16, 185, 129));
                }

                AplicaRestrictiiRol();
                IncarcaDate();
            }
            catch (Exception ex)
            {
                NotificationHelper.ShowToast(this, $"Eroare la încărcarea datelor:\n{ex.Message}", true);
            }
        }

        private void AplicaRestrictiiRol()
        {
            // Set tag for DataTrigger to show/hide edit buttons on cards
            if (ListProduse != null)
            {
                ListProduse.Tag = utilizatorConectat.Rol == TipRol.Admin ? "Admin" : "Client";
            }

            if (utilizatorConectat.Rol == TipRol.Client)
            {
                if (PanelButoaneProduse != null) PanelButoaneProduse.Visibility = Visibility.Collapsed;
                if (BtnTabUtilizatori != null) BtnTabUtilizatori.Visibility = Visibility.Collapsed;
                if (PanelButoaneUtilizatori != null) PanelButoaneUtilizatori.Visibility = Visibility.Collapsed;
            }
        }

        private void ConfigureazaCaleFisiere()
        {
            if (AdministrareProduseFisierText.BasePath != null) return;
            string? dir = AppDomain.CurrentDomain.BaseDirectory;
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
        }

        private void IncarcaDate()
        {
            try
            {
                toateProdusele = magazin.GetProduse();
                var comenziIncarcate = comandaAdmin.GetComenzi();
                if (utilizatorConectat.Rol == TipRol.Client)
                {
                    comenziIncarcate = comenziIncarcate.Where(c => c.IdClient == utilizatorConectat.Id).ToList();
                }
                toateComenzile = comenziIncarcate.Select(c => new ComandaViewModel(c, toateProdusele)).ToList();
                totiUtilizatorii = utilizatorAdmin.GetUtilizatori();
            }
            catch
            {
                toateProdusele = new();
                toateComenzile = new();
                totiUtilizatorii = new();
            }
            ActualizeazaGrid();
        }

        private void SetTabActiv(int tab)
        {
            tabActiv = tab;
            
            if (BtnTabProduse != null) BtnTabProduse.Foreground = new SolidColorBrush(Color.FromRgb(148, 163, 184));
            if (BtnTabComenzi != null) BtnTabComenzi.Foreground = new SolidColorBrush(Color.FromRgb(148, 163, 184));
            if (BtnTabUtilizatori != null) BtnTabUtilizatori.Foreground = new SolidColorBrush(Color.FromRgb(148, 163, 184));

            if (BtnTabProduse != null) BtnTabProduse.Background = new SolidColorBrush(Colors.Transparent);
            if (BtnTabComenzi != null) BtnTabComenzi.Background = new SolidColorBrush(Colors.Transparent);
            if (BtnTabUtilizatori != null) BtnTabUtilizatori.Background = new SolidColorBrush(Colors.Transparent);

            switch (tab)
            {
                case 0:
                    if (BtnTabProduse != null)
                    {
                        BtnTabProduse.Foreground = new SolidColorBrush(Colors.White);
                        BtnTabProduse.Background = new SolidColorBrush(Color.FromRgb(59, 130, 246));
                    }
                    if (GridMagazinProduse != null) GridMagazinProduse.Visibility = Visibility.Visible;
                    if (ListCategorii != null) ListCategorii.Visibility = Visibility.Visible;
                    if (PanelButoaneProduse != null && utilizatorConectat.Rol == TipRol.Admin) PanelButoaneProduse.Visibility = Visibility.Visible;
                    if (GridComenzi != null) GridComenzi.Visibility = Visibility.Collapsed;
                    if (GridUtilizatori != null) GridUtilizatori.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    if (BtnTabComenzi != null)
                    {
                        BtnTabComenzi.Foreground = new SolidColorBrush(Colors.White);
                        BtnTabComenzi.Background = new SolidColorBrush(Color.FromRgb(59, 130, 246));
                    }
                    if (GridMagazinProduse != null) GridMagazinProduse.Visibility = Visibility.Collapsed;
                    if (ListCategorii != null) ListCategorii.Visibility = Visibility.Collapsed;
                    if (PanelButoaneProduse != null) PanelButoaneProduse.Visibility = Visibility.Collapsed;
                    if (GridComenzi != null) GridComenzi.Visibility = Visibility.Visible;
                    if (GridUtilizatori != null) GridUtilizatori.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    if (BtnTabUtilizatori != null)
                    {
                        BtnTabUtilizatori.Foreground = new SolidColorBrush(Colors.White);
                        BtnTabUtilizatori.Background = new SolidColorBrush(Color.FromRgb(59, 130, 246));
                    }
                    if (GridMagazinProduse != null) GridMagazinProduse.Visibility = Visibility.Collapsed;
                    if (ListCategorii != null) ListCategorii.Visibility = Visibility.Collapsed;
                    if (PanelButoaneProduse != null) PanelButoaneProduse.Visibility = Visibility.Collapsed;
                    if (GridComenzi != null) GridComenzi.Visibility = Visibility.Collapsed;
                    if (GridUtilizatori != null && utilizatorConectat.Rol == TipRol.Admin) GridUtilizatori.Visibility = Visibility.Visible;
                    break;
            }

            if (TxtSearch != null) TxtSearch.Text = "";
            ActualizeazaGrid();
        }

        private void BtnTabProduse_Click(object sender, RoutedEventArgs e) => SetTabActiv(0);
        private void BtnTabComenzi_Click(object sender, RoutedEventArgs e) => SetTabActiv(1);
        private void BtnTabUtilizatori_Click(object sender, RoutedEventArgs e) => SetTabActiv(2);

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWin = new LoginWindow();
            this.Hide();
            if (loginWin.ShowDialog() == true && loginWin.LoggedUser != null)
            {
                var mainWin = new MainWindow(loginWin.LoggedUser);
                mainWin.Show();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }

        private void BtnContulMeu_Click(object sender, RoutedEventArgs e)
        {
            var contWin = new UtilizatorWindow(utilizatorConectat, isReadOnlyRole: true);
            contWin.Owner = this;
            if (contWin.ShowDialog() == true)
            {
                utilizatorAdmin.UpdateUtilizator(contWin.UtilizatorModificat);
                NotificationHelper.ShowToast(this, "Datele contului au fost actualizate!", false);
                // Optional: IncarcaDate(); - nu e neaparat nevoie daca nu afecteaza produsele
            }
        }

        private void BtnCosulMeu_Click(object sender, RoutedEventArgs e)
        {
            var cosWin = new CosWindow(cosCumparaturi, utilizatorConectat.Id, magazin, comandaAdmin);
            cosWin.Owner = this;
            cosWin.ShowDialog();
            
            if (cosWin.ComandaPlasata)
            {
                // Daca comanda a fost plasata cu succes, golim cosul si reimprospatam stocurile
                cosCumparaturi.Clear();
                IncarcaDate();
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e) => ActualizeazaGrid();
        private void ListCategorii_SelectionChanged(object sender, SelectionChangedEventArgs e) => ActualizeazaGrid();
        private void CboCriteriuCautare_SelectionChanged(object sender, SelectionChangedEventArgs e) => ActualizeazaGrid();
        private void CboSortare_SelectionChanged(object sender, SelectionChangedEventArgs e) => ActualizeazaGrid();
        private void ChkFiltruOptiune_Changed(object sender, RoutedEventArgs e) => ActualizeazaGrid();

        private void ActualizeazaGrid()
        {
            if (ListProduse == null || GridComenzi == null || GridUtilizatori == null) return;

            string filtru = TxtSearch?.Text?.Trim()?.ToLower() ?? "";
            string criteriu = (CboCriteriuCautare?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "General";

            switch (tabActiv)
            {
                case 0:
                    string catFiltru = "Toate";
                    if (ListCategorii?.SelectedItem is ListBoxItem lbi && lbi.Tag != null)
                    {
                        catFiltru = lbi.Tag.ToString() ?? "Toate";
                    }

                    // Preluare filtru de pret
                    double pretMin = 0;
                    double pretMax = double.MaxValue;
                    if (TxtFiltruPretMin != null && !string.IsNullOrWhiteSpace(TxtFiltruPretMin.Text)) double.TryParse(TxtFiltruPretMin.Text, out pretMin);
                    if (TxtFiltruPretMax != null && !string.IsNullOrWhiteSpace(TxtFiltruPretMax.Text)) double.TryParse(TxtFiltruPretMax.Text, out pretMax);

                    // Preluare optiuni extra
                    bool filtreazaGarantie = ChkFiltruGarantie?.IsChecked ?? false;
                    bool filtreazaDrivere = ChkFiltruDrivere?.IsChecked ?? false;
                    bool filtreazaLivrare = ChkFiltruLivrare?.IsChecked ?? false;
                    bool filtreazaRetur = ChkFiltruReturnare?.IsChecked ?? false;
                    bool doarInStoc = ChkFiltruStoc?.IsChecked ?? false;

                    var produseFiltrate = toateProdusele.Where(p => {
                        bool matchSearch = string.IsNullOrEmpty(filtru) ||
                                           (criteriu == "ID" && p.Id.ToString().Contains(filtru)) ||
                                           (criteriu == "Nume/Username" && p.Nume.ToLower().Contains(filtru)) ||
                                           (criteriu == "General" && (p.Nume.ToLower().Contains(filtru) || p.CategorieProdus.ToString().ToLower().Contains(filtru) || p.Id.ToString().Contains(filtru)));
                        
                        bool matchCat = catFiltru == "Toate" || p.CategorieProdus.ToString() == catFiltru;
                        
                        bool matchPret = p.Pret >= pretMin && p.Pret <= pretMax;
                        
                        bool matchStoc = !doarInStoc || p.Stoc > 0;

                        bool matchOptiuni = true;
                        if (filtreazaGarantie && !p.OptiuniProdus.HasFlag(Optiuni.Garantie)) matchOptiuni = false;
                        if (filtreazaDrivere && !p.OptiuniProdus.HasFlag(Optiuni.SuportDrivere)) matchOptiuni = false;
                        if (filtreazaLivrare && !p.OptiuniProdus.HasFlag(Optiuni.LivrareRapida)) matchOptiuni = false;
                        if (filtreazaRetur && !p.OptiuniProdus.HasFlag(Optiuni.Returnare14Zile)) matchOptiuni = false;

                        return matchSearch && matchCat && matchPret && matchStoc && matchOptiuni;
                    });

                    // Sorting
                    if (CboSortare != null)
                    {
                        switch (CboSortare.SelectedIndex)
                        {
                            case 0: // Nume A-Z
                                produseFiltrate = produseFiltrate.OrderBy(p => p.Nume);
                                break;
                            case 1: // Nume Z-A
                                produseFiltrate = produseFiltrate.OrderByDescending(p => p.Nume);
                                break;
                            case 2: // Pret Crescator
                                produseFiltrate = produseFiltrate.OrderBy(p => p.Pret);
                                break;
                            case 3: // Pret Descrescator
                                produseFiltrate = produseFiltrate.OrderByDescending(p => p.Pret);
                                break;
                        }
                    }

                    var prodList = produseFiltrate.Take(50).ToList();
                    ListProduse.ItemsSource = prodList;
                    VerificaEmpty(prodList.Count);
                    break;

                case 1:
                    var comenziFiltrate = toateComenzile.Where(c => {
                        return string.IsNullOrEmpty(filtru) || c.Id.ToString().Contains(filtru) || c.ProduseText.ToLower().Contains(filtru);
                    }).Take(50).ToList();

                    if (DgdComenzi != null) DgdComenzi.ItemsSource = comenziFiltrate;
                    VerificaEmpty(comenziFiltrate.Count);
                    break;

                case 2:
                    var utilizatoriFiltrati = totiUtilizatorii.Where(u => {
                        return string.IsNullOrEmpty(filtru) || u.Nume.ToLower().Contains(filtru) || u.Username.ToLower().Contains(filtru);
                    }).Take(50).ToList();

                    if (DgdUtilizatori != null) DgdUtilizatori.ItemsSource = utilizatoriFiltrati;
                    VerificaEmpty(utilizatoriFiltrati.Count);
                    break;
            }
        }

        private void VerificaEmpty(int count)
        {
            if (PanelEmpty != null) PanelEmpty.Visibility = count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        // ═══════ ADMIN ACTIONS (PRODUSE) ═══════
        private void BtnAdaugaProdus_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AdaugaProdusWindow();
            addWin.Owner = this;
            if (addWin.ShowDialog() == true)
            {
                if (addWin.ProdusNou != null)
                {
                    magazin.AdaugaProdus(addWin.ProdusNou);
                }
                IncarcaDate();
            }
        }

        private void BtnEditProductCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Produs produs)
            {
                var editWin = new EditeazaProdusWindow(produs);
                editWin.Owner = this;
                if (editWin.ShowDialog() == true)
                {
                    if (editWin.ProdusModificat != null)
                    {
                        magazin.UpdateProdus(editWin.ProdusModificat);
                    }
                    IncarcaDate();
                }
            }
        }

        private void BtnDeleteProductCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Produs produs)
            {
                var confirm = new ConfirmWindow($"Sunteți sigur că doriți să ștergeți produsul '{produs.Nume}'?");
                confirm.Owner = this;
                if (confirm.ShowDialog() == true)
                {
                    magazin.StergeProdus(produs.Id);
                    IncarcaDate();
                }
            }
        }

        // ═══════ STORE ACTIONS (PRODUSE) ═══════
        private void BtnInfoProductCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Produs produs)
            {
                var detaliiWin = new DetaliiProdusWindow(produs);
                detaliiWin.Owner = this;
                detaliiWin.ShowDialog();
            }
        }

        private void BtnAdaugaInCos_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Produs produs)
            {
                int dejaInCos = cosCumparaturi.Count(p => p.Id == produs.Id);
                if (dejaInCos >= produs.Stoc)
                {
                    NotificationHelper.ShowToast(this, $"Ne pare rău, ai adăugat deja tot stocul ({produs.Stoc}) în coș.", true);
                    return;
                }
                cosCumparaturi.Add(produs);
                NotificationHelper.ShowToast(this, $"\"{produs.Nume}\" a fost adăugat în coș!", false);
            }
        }

        // ═══════ UTILIZATORI EVENTS ═══════
        private void GridUtilizatori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool hasSelection = DgdUtilizatori.SelectedItem != null;
            if (BtnEditeazaUtilizator != null) BtnEditeazaUtilizator.IsEnabled = hasSelection;
            if (BtnStergeUtilizator != null) BtnStergeUtilizator.IsEnabled = hasSelection;
        }

        private void BtnAdaugaUtilizator_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new UtilizatorWindow();
            addWin.Owner = this;
            if (addWin.ShowDialog() == true) IncarcaDate();
        }

        private void BtnEditeazaUtilizator_Click(object sender, RoutedEventArgs e)
        {
            if (DgdUtilizatori.SelectedItem is Utilizator user)
            {
                var editWin = new UtilizatorWindow(user);
                editWin.Owner = this;
                if (editWin.ShowDialog() == true) IncarcaDate();
            }
        }

        private void BtnStergeUtilizator_Click(object sender, RoutedEventArgs e)
        {
            if (DgdUtilizatori.SelectedItem is Utilizator user)
            {
                if (user.Id == utilizatorConectat.Id)
                {
                    NotificationHelper.ShowToast(this, "Nu vă puteți șterge propriul cont curent!", true);
                    return;
                }

                var confirm = new ConfirmWindow($"Sigur ștergeți pe {user.Username}?");
                confirm.Owner = this;
                if (confirm.ShowDialog() == true)
                {
                    utilizatorAdmin.StergeUtilizator(user.Id);
                    IncarcaDate();
                }
            }
        }
    }
}
