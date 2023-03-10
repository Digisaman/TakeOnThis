using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using TakeOnThis.Core;
using TakeOnThis.Interfaces;

namespace TakeOnThis.ViewModel
{
    public class ViewModelBase : BaseViewModel
    {
        ChatService chatService;
        public ChatService ChatService =>
            chatService ?? (chatService = DependencyService.Resolve<ChatService>());

        IDialogService dialogService;
        public IDialogService DialogService =>
            dialogService ?? (dialogService = DependencyService.Resolve<IDialogService>());
    }
}
