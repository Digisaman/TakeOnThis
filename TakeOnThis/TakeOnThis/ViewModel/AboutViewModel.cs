using System.Windows.Input;
using TakeOnThis.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TakeOnThis.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenOwnerCommand = new Command(async () => await Browser.OpenAsync("https://www.seemia.com/takeonthis"));
            OpenDeveloperCommand = new Command(async () => await Browser.OpenAsync("http://samanpirooz.com/takeonthis"));
            VersionTracking.Track();
        }

        public ICommand OpenOwnerCommand { get; }
        public ICommand OpenDeveloperCommand { get; }


        public string CurrentVersion
        {
            get
            {
                return VersionTracking.CurrentVersion; 
            }
        }

        public string CurrentBuild
        {
            get
            {
                return VersionTracking.CurrentBuild;
            }
        }
    }
}