using Microsoft.Maui.Platform;
using Microsoft.Maui.Controls.Platform;
using Microsoft.UI.Xaml.Controls;
using RGPopup.Maui.Contracts;
using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Windows.Renders;
using WinPopup = global::Microsoft.UI.Xaml.Controls.Primitives.Popup;
using WindowsThickness = Microsoft.UI.Xaml.Thickness;
using XamlStyle = Microsoft.UI.Xaml.Style;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RGPopup.Maui.Windows.Impl
{
    internal class PopupPlatformWindows : IPopupPlatform
    {
        private readonly WinPopup _popup = new();
        private Microsoft.UI.Xaml.Window? _window;

        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => true;

        public async Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current?.MainPage;
            var mauiContext = Application.Current?.MainPage?.Handler?.MauiContext;
            var window = mauiContext?.Services?.GetService<Microsoft.UI.Xaml.Window>();
            if (window != null)
            {
                if (_window == null)
                {
                    _window = window;
                    _window.SizeChanged += OnWindowSizeChanged;
                }

                var pageHandler = page.GetOrCreateHandler<PopupPageHandlerWindows>();
                var renderer = pageHandler.PlatformView as PopupPageRenderer;
                renderer?.Prepare(_popup, _window);
                _popup.Child = renderer;
                _popup.XamlRoot = window?.Content.XamlRoot;
                _popup.IsOpen = true;

                page.ForceLayout();
            }

            await Task.Delay(5);
        }

        public async Task RemoveAsync(PopupPage page)
        {
            if (page == null) return;

            var pageHandler = page.GetHandler<PopupPageHandlerWindows>();
            var renderer = pageHandler?.PlatformView as PopupPageRenderer;
            var popup = renderer?.Container;
            if (popup != null)
            {
                popup.IsOpen = false;
                page.Parent = null;
            }

            await Task.Delay(5);
        }

        private void OnWindowSizeChanged(object sender, Microsoft.UI.Xaml.WindowSizeChangedEventArgs args)
        {
            if (_popup.Child is PopupPageRenderer renderer)
            {
                renderer.InvalidateArrange();
            }
        }
    }
}
