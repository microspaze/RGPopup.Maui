using System;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace RGPopup.Samples.Pages
{
    public partial class MvvmPage : PopupPage
    {
        public MvvmPage()
        {
            InitializeComponent();
            BindingContext = new MvvmPageViewModel();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
