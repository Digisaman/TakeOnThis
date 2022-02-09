using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TakeOnThis.View
{
    public class TakeOnThisNavigationPage : NavigationPage
    {
        public TakeOnThisNavigationPage(Page page) : base(page)
        {

        }

        public TakeOnThisNavigationPage() : base()
        {

        }

        void SetColor()
        {
            BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            BarTextColor = (Color)Application.Current.Resources["PrimaryTextColor"];
        }
    }
}
