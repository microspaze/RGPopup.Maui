using System.Linq;
using System.Threading.Tasks;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace RGPopup.Samples.Pages
{
    public partial class LoginSuccessPopupPage : PopupPage
    {
        public LoginSuccessPopupPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            HidePopup();
        }

        private async void HidePopup()
        {
            await Task.Delay(4000);

            if (PopupNavigation.Instance.PopupStack.Contains(this))
                await PopupNavigation.Instance.RemovePageAsync(this);
        }
    }
}
