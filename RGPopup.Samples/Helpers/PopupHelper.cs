using RGPopup.Samples.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGPopup.Samples.Helpers
{
    public class PopupHelper
    {
        public static async Task ShowPopup(ContentView content, string title = "")
        {
            var popup = new BasePopupPage(content, title);
            await popup.ShowPopup();
        }
    }
}
