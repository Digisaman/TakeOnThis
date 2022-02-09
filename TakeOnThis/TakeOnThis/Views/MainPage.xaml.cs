using Xamarin.Forms;
using TakeOnThis.ViewModels;
using Xamarin.Forms.Xaml;

namespace TakeOnThis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        //LobbyViewModel vm;
        //LobbyViewModel VM
        //{
        //    get => vm ?? (vm = (LobbyViewModel)BindingContext);
        //}
        public MainPage()
        {
            InitializeComponent();
        }

        
        //async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    await VM.GoToGroupChat(Navigation, e.SelectedItem as string);
        //    ((ListView)sender).SelectedItem = null;
        //}
    }
}
