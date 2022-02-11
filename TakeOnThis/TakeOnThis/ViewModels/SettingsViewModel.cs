using TakeOnThis.Interfaces;
using TakeOnThis.ViewModels;
using Xamarin.Forms;

namespace TakeOnThis.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        
        public MvvmHelpers.Commands.Command SaveSettingsCommand { get; }

        public SettingsViewModel()
        {   
            
            SaveSettingsCommand = new MvvmHelpers.Commands.Command(() => SaveSettings());
            serverIP = Helpers.Settings.ServerIP;
            serverPort = Helpers.Settings.ServerPort;
            username = Helpers.Settings.UserName;
        }


        string serverIP = "";
        public string ServerIP
        {
            get { return this.serverIP; }
            set { SetProperty(ref this.serverIP, value); }
        }

        string serverPort = "";
        public string ServerPort
        {
            get { return this.serverPort; }
            set { SetProperty(ref this.serverPort, value); }
        }

        string username = "";
        public string Username
        {
            get { return this.username; }
            set { SetProperty(ref this.username, value); }
        }

        private void SaveSettings()
        {
            Helpers.Settings.ServerIP = this.ServerIP;
            Helpers.Settings.ServerPort = this.ServerPort;
            Helpers.Settings.UserName = this.Username;
            //var dialogService = DependencyService.Get<IDialogService>();
            //dialogService.DisplayAlert("Information", "Seetings Saved", "");
        }

        


    }
}
