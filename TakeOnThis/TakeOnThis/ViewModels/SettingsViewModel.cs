using TakeOnThis.ViewModels;

namespace TakeOnThis.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        
        public MvvmHelpers.Commands.Command SaveSettingsCommand { get; }

        public SettingsViewModel()
        {   
            
            SaveSettingsCommand = new MvvmHelpers.Commands.Command(() => SaveSettings());
            serverIP = Helpers.Settings.ServerIP;
            serverPort = Helpers.Settings.ServerPort;
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

        private void SaveSettings()
        {
            Helpers.Settings.ServerIP = this.ServerIP;
            Helpers.Settings.ServerPort = this.ServerPort;
        }

        


    }
}
