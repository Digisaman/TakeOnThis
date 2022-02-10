using MvvmHelpers;
using Xamarin.Forms;
using TakeOnThis.Model;
using System.Threading.Tasks;
using System;
using TakeOnThis.Helpers;
using System.Linq;
using System.Collections.ObjectModel;
using TakeOnThis.Shared.Models;
using Newtonsoft.Json;
using System.IO;

namespace TakeOnThis.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        public ChatMessage ChatMessage { get; }

        public ObservableCollection<ChatMessage> Messages { get; }
        public ObservableCollection<User> Users { get; }

        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SetProperty(ref isConnected, value);
                });
            }
        }
        

        public MvvmHelpers.Commands.Command SendMessageCommand { get; }
        public MvvmHelpers.Commands.Command ConnectCommand { get; }
        public MvvmHelpers.Commands.Command DisconnectCommand { get; }

        Random random;
        public ChatViewModel()
        {
            if (DesignMode.IsDesignModeEnabled)
                return;

            Title = Settings.Group;

            ChatMessage = new ChatMessage();
            Messages = new ObservableCollection<ChatMessage>();
            Users = new ObservableCollection<User>();
            SendMessageCommand = new MvvmHelpers.Commands.Command(async () => await SendMessage());
            ConnectCommand = new MvvmHelpers.Commands.Command(async () => await Connect());
            DisconnectCommand = new MvvmHelpers.Commands.Command(async () => await Disconnect());
            random = new Random();

            ChatService.Init(Settings.ServerIP, Settings.UseHttps);

            ChatService.OnReceivedMessage += (sender, args) =>
            {
                SendLocalMessage(args.Message, args.User);
                AddRemoveUser(args.User, true);
            };

            ChatService.OnEnteredOrExited += (sender, args) =>
            {
                AddRemoveUser(args.User, args.Message.Contains("entered"));
            };

            ChatService.OnConnectionClosed += (sender, args) =>
            {
                SendLocalMessage(args.Message, args.User);  
            };
        }


        async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                IsBusy = true;
                await ChatService.ConnectAsync();
                await ChatService.JoinChannelAsync(Settings.Group, Settings.UserName);
                IsConnected = true;

                AddRemoveUser(Settings.UserName, true);
                await Task.Delay(500);
                SendLocalMessage("Connected...", Settings.UserName);
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Connection error: {ex.Message}", Settings.UserName);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task Disconnect()
        {
            if (!IsConnected)
                return;
            await ChatService.LeaveChannelAsync(Settings.Group, Settings.UserName);
            await ChatService.DisconnectAsync();
            IsConnected = false;
            SendLocalMessage("Disconnected...", Settings.UserName);
        }

        async Task SendMessage()
        {
            if(!IsConnected)
            {
                await DialogService.DisplayAlert("Not connected", "Please connect to the server and try again.", "OK");
                return;
            }
            try
            {
                IsBusy = true;
                await ChatService.SendMessageAsync(Settings.Group,
                    Settings.UserName,
                    ChatMessage.Message);

                ChatMessage.Message = string.Empty;
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Send failed: {ex.Message}", Settings.UserName);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SendLocalMessage(string message, string user)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var first = Users.FirstOrDefault(u => u.Name == user);

                if (message.StartsWith("{"))
                {
                    ServiceMessage serviceMessage = JsonConvert.DeserializeObject<ServiceMessage>(message);
                    if (serviceMessage != null)
                    {
                        switch (serviceMessage.Command)
                        {


                            case TakeOnThis.Shared.Models.Command.SendText:
                                InserText(user, first, serviceMessage);
                                break;
                            case TakeOnThis.Shared.Models.Command.SendImage:
                                InserImage(user, first, serviceMessage);
                                break;

                                //case TakeOnThis.Shared.Models.Command.SendVideo:
                                //    Messages.Insert(0, new ChatMessage
                                //    {
                                //        Message = "Display Video",
                                //        Type = MessageType.Video,
                                //        User = user,
                                //        Color = first?.Color ?? Color.FromRgba(0, 0, 0, 0)
                                //    });
                                break;
                        }
                    }
                }
                else
                {
                    //Messages.Clear();
                    Messages.Insert(0, new ChatMessage
                    {
                        Message = message,
                        User = user,
                        Color = first?.Color ?? Color.FromRgba(0, 0, 0, 0)
                    });
                }
            });
        }

        private void InserImage(string user, User first, ServiceMessage serviceMessage)
        {
            Messages.Insert(0, new ChatMessage
            {
                ImageSource = ImageSource.FromFile(GetFileAddress(MessageType.Image, serviceMessage.FileName)),
                Message = "Display Image",
                Type = MessageType.Image,
                User = user,
                Color = first?.Color ?? Color.FromRgba(0, 0, 0, 0)
            });
        }

        private void InserText(string user, User first, ServiceMessage serviceMessage)
        {
            Messages.Insert(0, new ChatMessage
            {
                //ImageSource = ImageSource.FromFile(GetFileAddress(MessageType.Video, serviceMessage.FileName)),
                Message = serviceMessage.Text,
                Type = MessageType.Text,
                User = user,
                Color = first?.Color ?? Color.FromRgba(0, 0, 0, 0)
            });
        }

        private string GetFileAddress(MessageType messageType, string filename)
        {
            string fileName = "";
            try
            {
                string subtitleDirectoty = "";
                switch (messageType)
                {
                    case MessageType.Image:  
                        subtitleDirectoty = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DownloadCategory.Image.ToString());
                        fileName = Path.Combine(subtitleDirectoty, filename);
                        break;
                    case MessageType.Video:
                        subtitleDirectoty = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DownloadCategory.Video.ToString());
                        fileName = Path.Combine(subtitleDirectoty, filename);
                        break;

                }
            }
            catch (Exception ex)
            {

            }
            return fileName;

        }

        void AddRemoveUser(string name, bool add)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;
            if (add)
            {
                if (!Users.Any(u => u.Name == name))
                {
                    var color = Messages.FirstOrDefault(m => m.User == name)?.Color ?? Color.FromRgba(0, 0, 0, 0);
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            Users.Add(new User { Name = name, Color = color });
                        });
                }
            }
            else
            {
                var user = Users.FirstOrDefault(u => u.Name == name);
                if (user != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Users.Remove(user);
                    });
                }
            }
        }
    }
}
