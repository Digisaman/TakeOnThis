using MvvmHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using TakeOnThis.Helpers;
using TakeOnThis.View;

namespace TakeOnThis.ViewModel
{
    public class LobbyViewModel : ViewModelBase
    {
        public List<string> Rooms { get; }
        public LobbyViewModel()
        {
            Rooms = ChatService.GetRooms();
        }

        public string UserName
        {
            get => Settings.UserName;
            set
            {
                if (value == UserName)
                    return;
                Settings.UserName = value;
                OnPropertyChanged();
            }
        }


        public async Task GoToGroupChat(INavigation navigation, string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                return;

            if (string.IsNullOrWhiteSpace(UserName))
                return;

            Settings.Group = group;
            await navigation.PushModalAsync(new TakeOnThisNavigationPage(new GroupChatPage()));
        }
        
    }
}
