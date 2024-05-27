using System;
using RGPopup.Maui.Services;

namespace RGPopup.Samples.Pages
{
    public partial class ScrollViewDisabledPage
    {
        public ScrollViewDisabledPage()
        {
            InitializeComponent();
            Lista.ItemsSource = new List<string>
            {
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView",
                "Test ListView"
            };
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
