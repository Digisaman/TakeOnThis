using TakeOnThis.ViewModels;
using TakeOnThis.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TakeOnThis.View;

namespace TakeOnThis
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            //Routing.RegisterRoute(nameof(BioPage), typeof(BioPage));
            //Routing.RegisterRoute(nameof(GroupChatPage), typeof(GroupChatPage));
            //Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));

        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
        }
        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }
}
