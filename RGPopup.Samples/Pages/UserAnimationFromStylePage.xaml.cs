using System;
using RGPopup.Maui.Services;

namespace RGPopup.Samples.Pages
{
    public partial class UserAnimationFromStylePage
    {
        public UserAnimationFromStylePage()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
