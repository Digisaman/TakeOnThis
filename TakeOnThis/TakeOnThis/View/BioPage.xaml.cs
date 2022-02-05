using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeOnThis.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TakeOnThis.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BioPage : ContentPage
    {
        public BioPage()
        {
            InitializeComponent();
        }
    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    this.webBrowser.Source = $"http://{Settings.ServerIP}:{Settings.ServerPort}/index.html";
    //    this.webBrowser.Reload();
    //    //this.webBrowser.Reload();


    //}
}