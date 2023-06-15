using System;
using System.Threading.Tasks;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;


namespace RGPopup.Samples.Pages
{
    public partial class SystemOffsetPage : PopupPage
    {
        public SystemOffsetPage()
        {
            InitializeComponent();

            UpdateInfoText();

            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(2000);
                Padding = new Thickness(10, 10, 10, 10);

                await Task.Delay(2000);
                HasSystemPadding = false;

                await Task.Delay(2000);
                Padding = new Thickness();

                await Task.Delay(2000);
                HasSystemPadding = true;

                await Task.Delay(2000);
                SystemPaddingSides = RGPopup.Maui.Enums.PaddingSide.Left;

                await Task.Delay(2000);
                SystemPaddingSides = RGPopup.Maui.Enums.PaddingSide.Top;

                await Task.Delay(2000);
                SystemPaddingSides = RGPopup.Maui.Enums.PaddingSide.Right;

                await Task.Delay(2000);
                SystemPaddingSides = RGPopup.Maui.Enums.PaddingSide.Bottom;

                await Task.Delay(2000);
                SystemPaddingSides = RGPopup.Maui.Enums.PaddingSide.All;
            });
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(Padding):
                case nameof(HasSystemPadding):
                case nameof(SystemPadding):
                case nameof(SystemPaddingSides):
                case nameof(HasKeyboardOffset):
                case nameof(KeyboardOffset):
                    UpdateInfoText();
                    break;
            }
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        void UpdateInfoText()
        {
            InfoLabel.Text = $"Padding: {ThicknessToString(Padding)}\n" +
                $"HasSystemPadding: {HasSystemPadding}\n" +
                $"SystemPadding: {ThicknessToString(SystemPadding)}\n" +
                $"SystemPaddingSide: {SystemPaddingSides}\n" +
                $"HasKeyboardOffset: {HasKeyboardOffset}\n" +
                $"KeyboardOffset: {KeyboardOffset}";
        }

        string ThicknessToString(Thickness v)
        {
            return $"[{v.Left}, {v.Top}, {v.Right}, {v.Bottom}]";
        }
    }
}
