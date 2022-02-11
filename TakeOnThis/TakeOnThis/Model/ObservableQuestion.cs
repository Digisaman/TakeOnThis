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

        private string key;

        public string Key
        {
            get => key;
            set => SetProperty(ref key, value);
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

    public class ObservableVoteResponse : ObservableObject
    {
 

        private double votePercent;

        public double VotePercent
        {
            get => votePercent;
            set => SetProperty(ref votePercent, value);
        }

        private string character = "";

        public string Charcter
        {
            get => character;
            set => SetProperty(ref character, value);
        }

    }


}
