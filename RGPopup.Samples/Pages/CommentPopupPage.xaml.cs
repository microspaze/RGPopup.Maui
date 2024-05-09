using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace RGPopup.Samples.Pages
{
    public partial class CommentPopupPage : PopupPage
    {
        public CommentPopupPage()
        {
            InitializeComponent();
        }

        private async void OnCancelClicked(object? sender, EventArgs e)
        {
            await PopupNavigation.Instance.RemovePageAsync(this);
        }
    }
}