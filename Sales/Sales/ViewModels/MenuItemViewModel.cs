namespace Sales.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Views;
    using Xamarin.Forms;

    public class MenuItemViewModel
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        
        #region Commands
        public ICommand GotoCommand
        {
            get
            {
                return new RelayCommand(Goto);
            }
        }

        private async void Goto()
        {
            App.Master.IsPresented = false;

            if (this.PageName == "LoginPage")
            {
                Settings.TokenType = string.Empty;
                Settings.AccessToken = string.Empty;
                Settings.IsRemembered = false;

                MainViewModel.GetInstance().Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(
                    new LoginPage());
            }
            else if (this.PageName == "AboutPage")
            {
                App.Master.IsPresented = false;
                await App.Navigator.PushAsync(new MapPage());
            }
            else if (this.PageName == "MyProfilePage")
            {
                //MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
                //App.Navigator.PushAsync(new MyProfilePage());
            }
        }
        #endregion
    }
}
