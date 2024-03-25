using System;
using RGPopup.Maui.Services;

namespace RGPopup.Samples.Pages
{
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void OnTapGestureClose(object sender, TappedEventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
