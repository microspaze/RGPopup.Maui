using System;
using RGPopup.Maui.Services;

namespace RGPopup.Samples.Pages
{
    public partial class UserAnimationFromResourcePage
    {
        public UserAnimationFromResourcePage()
        {
            InitializeComponent();
        }


        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
