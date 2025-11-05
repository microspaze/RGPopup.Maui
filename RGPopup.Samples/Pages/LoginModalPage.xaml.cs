using System;
using System.Threading.Tasks;
using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;


namespace RGPopup.Samples.Pages
{
    public partial class LoginModalPage
    {
        public LoginModalPage()
        {
            InitializeComponent();
        }

        private async void OnLogin(object sender, EventArgs e)
        {
            var loadingPage = new LoadingPopupPage();
            await Navigation.PushPopupAsync(loadingPage, parent: this);
            await Task.Delay(2000);
            await Navigation.RemovePopupPageAsync(loadingPage);
            await Navigation.PushPopupAsync(new LoginSuccessPopupPage(), parent: this);
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }
 
        private async void CloseAllPopup()
        {
            await Navigation.PopModalAsync();
        }
    }
}
