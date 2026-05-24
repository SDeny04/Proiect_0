using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Magazin.WPF.Utils
{
    public static class NotificationHelper
    {
        public static void ShowToast(Window owner, string message, bool isError = false)
        {
            var popup = new Popup
            {
                PlacementTarget = owner,
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Slide,
                StaysOpen = false
            };

            var border = new Border
            {
                Background = new SolidColorBrush(isError ? (Color)ColorConverter.ConvertFromString("#ef4444") : (Color)ColorConverter.ConvertFromString("#10b981")),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(15, 10, 15, 10),
                Margin = new Thickness(10)
            };

            var text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontWeight = FontWeights.SemiBold,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 400
            };

            border.Child = text;
            popup.Child = border;

            // Center horizontally at the bottom
            popup.CustomPopupPlacementCallback = (popupSize, targetSize, offset) =>
            {
                double x = (targetSize.Width - popupSize.Width) / 2;
                double y = targetSize.Height - popupSize.Height - 30; // 30px from bottom
                return new[] { new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal) };
            };
            popup.Placement = PlacementMode.Custom;

            popup.IsOpen = true;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            timer.Tick += (s, e) =>
            {
                popup.IsOpen = false;
                timer.Stop();
            };
            timer.Start();
        }
    }
}
