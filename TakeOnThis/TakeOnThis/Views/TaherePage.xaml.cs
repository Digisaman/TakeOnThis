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
    public partial class TaherePage : ContentPage
    {
        QuestionViewModel vm;
        QuestionViewModel VM
        {
            get => vm ?? (vm = (QuestionViewModel)BindingContext);
        }
        public TaherePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.CurrentCharcter = Shared.Models.Character.Tahere;
            if (!DesignMode.IsDesignModeEnabled)
                VM.LoadQuestionsCommand.Execute(null);
        }
    }
}