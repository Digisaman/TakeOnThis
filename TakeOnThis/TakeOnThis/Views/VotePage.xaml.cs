using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeOnThis.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TakeOnThis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VotePage : ContentPage
    {
        VoteViewModel vm;
        VoteViewModel VM
        {
            get => vm ?? (vm = (VoteViewModel)BindingContext);
        }
        public VotePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!DesignMode.IsDesignModeEnabled)
                VM.LoadVoteCommand.Execute(null);
        }
    }
}