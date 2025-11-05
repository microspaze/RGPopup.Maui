using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using RGPopup.Maui.Contracts;
using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Windows.Renders;
using System.Text.Json;
using System.Text.Json.Serialization;
using WindowsThickness = Microsoft.UI.Xaml.Thickness;
using WinPopup = global::Microsoft.UI.Xaml.Controls.Primitives.Popup;
using XamlStyle = Microsoft.UI.Xaml.Style;

namespace RGPopup.Maui.Windows.Impl
{
    internal class PopupPlatformWindows : IPopupPlatform
    {
        private readonly HashSet<WinPopup> _popups = new();
        private Microsoft.UI.Xaml.Window? _window;
        private Microsoft.Maui.Controls.Page? _mainPage => Application.Current?.Windows[0].Page;

        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => true;

        public async Task AddAsync(PopupPage page, Microsoft.Maui.Controls.Page? parent = null)
        {
            page.Parent = parent ?? _mainPage;
            var mauiContext = page.Parent?.Handler?.MauiContext;
            var window = mauiContext?.Services?.GetService<Microsoft.UI.Xaml.Window>();
            if (window != null)
            {
                if (_window == null)
                {
                    _window = window;
                    _window.SizeChanged += OnWindowSizeChanged;
                }

                var popup = new WinPopup();
                var pageHandler = page.GetOrCreateHandler<PopupPageHandlerWindows>();
                var renderer = pageHandler.PlatformView as PopupPageRenderer;
                renderer?.Prepare(popup, _window);
                popup.Child = renderer;
                popup.XamlRoot = window?.Content.XamlRoot;
                popup.IsOpen = true;
                _popups.Add(popup);

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
                page.Handler?.DisconnectHandler();
                _popups.Remove(popup);
            }

            await Task.Delay(5);
        }

        private void OnWindowSizeChanged(object sender, Microsoft.UI.Xaml.WindowSizeChangedEventArgs args)
        {
            foreach(var popup in _popups)
            {
                if (popup.Child is PopupPageRenderer renderer)
                {
                    renderer.InvalidateArrange();
                }
            }
        }
    }
}
