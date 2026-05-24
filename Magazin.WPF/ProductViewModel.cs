using System;
using System.ComponentModel;
using Magazin.Models;

namespace Magazin.WPF
{
    public class ProductViewModel : ViewModelBase, IDataErrorInfo
    {
        private string _nume = string.Empty;
        private string _pretText = "0";
        private string _stocText = "0";
        private Categorie _categorieProdus = Categorie.Procesor;
        private bool _isNou = true;
        private bool _isResigilat;
        
        private bool _garantie;
        private bool _suportDrivere;
        private bool _livrareRapida;
        private bool _returnare14Zile;

        public string Nume
        {
            get => _nume;
            set
            {
                if (_nume != value)
                {
                    _nume = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PretText
        {
            get => _pretText;
            set
            {
                if (_pretText != value)
                {
                    _pretText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StocText
        {
            get => _stocText;
            set
            {
                if (_stocText != value)
                {
                    _stocText = value;
                    OnPropertyChanged();
                }
            }
        }

        public Categorie CategorieProdus
        {
            get => _categorieProdus;
            set
            {
                if (_categorieProdus != value)
                {
                    _categorieProdus = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsNou
        {
            get => _isNou;
            set
            {
                if (_isNou != value)
                {
                    _isNou = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsResigilat
        {
            get => _isResigilat;
            set
            {
                if (_isResigilat != value)
                {
                    _isResigilat = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Garantie
        {
            get => _garantie;
            set
            {
                if (_garantie != value)
                {
                    _garantie = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SuportDrivere
        {
            get => _suportDrivere;
            set
            {
                if (_suportDrivere != value)
                {
                    _suportDrivere = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool LivrareRapida
        {
            get => _livrareRapida;
            set
            {
                if (_livrareRapida != value)
                {
                    _livrareRapida = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Returnare14Zile
        {
            get => _returnare14Zile;
            set
            {
                if (_returnare14Zile != value)
                {
                    _returnare14Zile = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Error => string.Empty;

        private bool _showErrors = false;

        public void Validate()
        {
            _showErrors = true;
            OnPropertyChanged(nameof(Nume));
            OnPropertyChanged(nameof(PretText));
            OnPropertyChanged(nameof(StocText));
        }

        public string this[string columnName]
        {
            get
            {
                if (!_showErrors) return string.Empty;

                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Nume):
                        if (string.IsNullOrWhiteSpace(Nume))
                            error = "Numele produsului este obligatoriu.";
                        break;
                    case nameof(PretText):
                        if (!double.TryParse(PretText, out double p) || p <= 0)
                            error = "Prețul trebuie să fie un număr valid, mai mare ca zero.";
                        break;
                    case nameof(StocText):
                        if (!int.TryParse(StocText, out int s) || s < 0)
                            error = "Stocul trebuie să fie un număr întreg pozitiv sau zero.";
                        break;
                }
                return error;
            }
        }

        public bool IsValid => 
            !string.IsNullOrWhiteSpace(Nume) &&
            double.TryParse(PretText, out double p) && p > 0 &&
            int.TryParse(StocText, out int s) && s >= 0;

        public Produs GetProdus(int id = 0)
        {
            string finalNume = Nume.Trim();
            if (IsResigilat)
            {
                finalNume += " (Resigilat)";
            }

            double pret = double.Parse(PretText.Trim());
            int stoc = int.Parse(StocText.Trim());

            Optiuni optiuni = Optiuni.Niciuna;
            if (Garantie) optiuni |= Optiuni.Garantie;
            if (SuportDrivere) optiuni |= Optiuni.SuportDrivere;
            if (LivrareRapida) optiuni |= Optiuni.LivrareRapida;
            if (Returnare14Zile) optiuni |= Optiuni.Returnare14Zile;

            return new Produs(finalNume, CategorieProdus, pret, optiuni, stoc) { Id = id };
        }

        public void LoadFromProdus(Produs produs)
        {
            string nume = produs.Nume;
            if (nume.EndsWith("(Resigilat)"))
            {
                IsResigilat = true;
                IsNou = false;
                nume = nume.Replace("(Resigilat)", "").Trim();
            }
            else
            {
                IsNou = true;
                IsResigilat = false;
            }

            Nume = nume;
            CategorieProdus = produs.CategorieProdus;
            PretText = produs.Pret.ToString();
            StocText = produs.Stoc.ToString();

            Garantie = produs.OptiuniProdus.HasFlag(Optiuni.Garantie);
            SuportDrivere = produs.OptiuniProdus.HasFlag(Optiuni.SuportDrivere);
            LivrareRapida = produs.OptiuniProdus.HasFlag(Optiuni.LivrareRapida);
            Returnare14Zile = produs.OptiuniProdus.HasFlag(Optiuni.Returnare14Zile);
        }
    }
}
