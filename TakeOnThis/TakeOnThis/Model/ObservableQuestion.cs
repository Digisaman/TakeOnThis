using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace TakeOnThis.Model
{
    public class ObservableQuestion : ObservableObject
    {   
        private int id ;

        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string title = "";

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

    }
}
