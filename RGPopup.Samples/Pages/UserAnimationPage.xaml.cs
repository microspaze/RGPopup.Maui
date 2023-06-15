using System;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace RGPopup.Samples.Pages
{
    public partial class UserAnimationPage : PopupPage
    {
        public UserAnimationPage()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
