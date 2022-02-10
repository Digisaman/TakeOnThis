using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeOnThis.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TakeOnThis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BioPage : ContentPage
    {
        public BioPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                this.webBrowser.Source = $"http://{Settings.ServerIP}:{Settings.ServerPort}/index.html";
                this.webBrowser.Reload();
            }
            catch (Exception ex)
            {

            }
            //this.webBrowser.Reload();


        }

        private void webBrowser_Navigated(object sender, WebNavigatedEventArgs e)
        {
            this.progressBar.IsVisible = false;
        }

        private void webBrowser_Navigating(object sender, WebNavigatingEventArgs e)
        {
            this.progressBar.IsVisible = true;
        }
    }

   
}