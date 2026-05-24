using System.Windows;
using System.Windows.Input;

namespace Magazin.WPF.Utils
{
    public partial class ConfirmWindow : Window
    {
        public ConfirmWindow(string message)
        {
            InitializeComponent();
            TxtMessage.Text = message;
            
            // Allow dragging the window
            this.MouseLeftButtonDown += (s, e) => { if (e.ButtonState == MouseButtonState.Pressed) this.DragMove(); };
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
