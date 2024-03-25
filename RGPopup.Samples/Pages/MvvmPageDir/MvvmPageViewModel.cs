using System.Windows.Input;

namespace RGPopup.Samples.Pages
{
    public class MvvmPageViewModel
    {
        public ICommand BackgroundClickedCommand => new Command(BackgroundClickedCommandExecute);
        public ICommand TapGestureClickedCommand => new Command(BackgroundClickedCommandExecute);

        private void BackgroundClickedCommandExecute(object parameter)
        {
            var label = (Label)parameter;
            label.Text = "Great, it works!";
        }
    }
}
