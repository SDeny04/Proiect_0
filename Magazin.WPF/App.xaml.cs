using System;
using System.IO;
using System.Windows;
using Magazin.StocareDate;

namespace Magazin.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ConfigureazaCaleFisiere();

            // Set shutdown mode to explicit to prevent app from closing when LoginWindow closes
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var loginWin = new LoginWindow();
            if (loginWin.ShowDialog() == true && loginWin.LoggedUser != null)
            {
                var mainWin = new MainWindow(loginWin.LoggedUser);
                MainWindow = mainWin;
                
                // Revert shutdown mode so the app exits when MainWindow closes
                ShutdownMode = ShutdownMode.OnLastWindowClose;
                
                mainWin.Show();
            }
            else
            {
                Shutdown();
            }
        }

        private void ConfigureazaCaleFisiere()
        {
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

            if (File.Exists("produse.txt"))
                return;

            MessageBox.Show("Nu s-au găsit fișierele de date (produse.txt, comenzi.txt, utilizatori.txt).\n" +
                           "Asigurați-vă că aplicația consolă a fost rulată cel puțin o dată.",
                           "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
