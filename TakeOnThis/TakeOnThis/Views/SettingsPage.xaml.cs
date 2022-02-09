using TakeOnThis.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TakeOnThis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel vm;
        SettingsViewModel VM
        {
            get => vm ?? (vm = (SettingsViewModel)BindingContext);
        }
        public SettingsPage()
        {
            InitializeComponent();
        }
    }
}