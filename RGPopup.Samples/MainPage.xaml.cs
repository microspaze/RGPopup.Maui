using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;
using RGPopup.Samples.Helpers;
using RGPopup.Samples.Pages;
using System.Diagnostics;

namespace RGPopup.Samples
{
    public partial class MainPage : ContentPage
    {
        private LoginPopupPage _loginPopup;
        private SettingsContentView _settingContent;

        public MainPage()
        {
            InitializeComponent();

            PopupNavigation.Instance.Pushing += (sender, e) => Debug.WriteLine($"[Popup] Pushing: {e.Page.GetType().Name}");
            PopupNavigation.Instance.Pushed += (sender, e) => Debug.WriteLine($"[Popup] Pushed: {e.Page.GetType().Name}");
            PopupNavigation.Instance.Popping += (sender, e) => Debug.WriteLine($"[Popup] Popping: {e.Page.GetType().Name}");
            PopupNavigation.Instance.Popped += (sender, e) => Debug.WriteLine($"[Popup] Popped: {e.Page.GetType().Name}");

            _loginPopup = new LoginPopupPage();
            _loginPopup.Loaded += (sender, args) =>
            {
                #if ANDROID
                //Hide bottom navigation bar when page poped-up 
                if (sender is PopupPage { Handler.PlatformView: Android.Views.View { Parent: Android.Widget.FrameLayout decorView } })
                {
                    var uiFlags = Android.Views.SystemUiFlags.LayoutStable | Android.Views.SystemUiFlags.HideNavigation;
                    decorView.SystemUiVisibility = (Android.Views.StatusBarVisibility) uiFlags;
                }
                #endif
            };
        }

        private async void OnOpenPopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_loginPopup);
        }

        private async void OnUserAnimationPupup(object sender, EventArgs e)
        {
            var page = new UserAnimationPage();

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnOpenSystemOffsetPage(object sender, EventArgs e)
        {
            var page = new SystemOffsetPage();

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnOpenListViewPage(object sender, EventArgs e)
        {
            var page = new ListViewPage();

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnOpenUserAnimationFromResource(object sender, EventArgs e)
        {
            var page = new UserAnimationFromResourcePage();

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnOpenUserAnimationFromStyle(object sender, EventArgs e)
        {
            var page = new UserAnimationFromStylePage();

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnOpenSettingsPage(object sender, EventArgs e)
        {
            var page = new SettingsPage();

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnOpenSettingsContent(object sender, EventArgs e)
        {
            _settingContent ??= new SettingsContentView();
            await PopupHelper.ShowPopup(_settingContent, "Settings");
        }

        private async void OnOpenMvvmPage(object sender, EventArgs e)
        {
            var page = new MvvmPage();

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnTestCurrentViewController(object sender, EventArgs e)
        {
            var page = new TestCurrentViewController(0);

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnOpenCamera(object sender, EventArgs e)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);

                    await PopupNavigation.Instance.PushAsync(_loginPopup);
                }
            }
        }

        private async void OnOpenComment(object sender, EventArgs e)
        {
            var page = new CommentPopupPage(){ IsPopupWindowResizable = true };

            await PopupNavigation.Instance.PushAsync(page);
        }
    }
}