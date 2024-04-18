using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace RGPopup.Samples.Pages
{
    public partial class BasePopupPage : PopupPage
    {
        public BasePopupPage()
        {
            InitializeComponent();
        }

        public BasePopupPage(ContentView contentView, string title)
        {
            InitializeComponent();
            UpdateContent(contentView, title);
        }

        private void UpdateContent(ContentView contentView, string title)
        {
            var showTitle = !string.IsNullOrEmpty(title);
            viewContent.Content = contentView;
            titleBorder.IsVisible = showTitle;
            borderContent.Padding = showTitle ? new Thickness(20) : new Thickness(0);
            borderContent.StrokeThickness = showTitle ? 1.0 : 0.0;
            titleLabel.Text = title;
        }

        public async Task ShowPopup()
        {
            try
            {
                await Navigation.PushPopupAsync(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnDisappearing()
        {
            //Fix error: Java.Lang.IllegalStateException
            //Message=The specified child already has a parent. You must call removeView() on the child's parent first.
            //viewContent.Content.Handler?.DisconnectHandler();
            base.OnDisappearing();
        }
    }
}