using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGPopup.Samples.Pages
{
    public class BasePopupPage : PopupPage
    {
        public BasePopupPage(ContentView content)
        {
            Content = content;
        }

        public async Task ShowPopup()
        {
            try
            {
                await Navigation.PushPopupAsync(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
