using System;
using TakeOnThis.Shared.Models;
using Xamarin.Essentials;

namespace TakeOnThis.Helpers
{
    public static class Settings
    {
        public static string AppCenterAndroid = "AC_ANDROID";

        //#if DEBUG
        //        static readonly string defaultIP = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";


        //#else
        //        static readonly string defaultIP = "TakeOnThisr.azurewebsites.net";
        //#endif

        static readonly string mefiaInfo = "";

        static readonly string voteInfo = "";

        static readonly string defaultIP = "192.168.0.100";

        static readonly string defaultPort = "5000";


        public static bool UseHttps
        {
            get => false;
         
        }

        public static string ServerPort
        {
           
            get => Preferences.Get(nameof(ServerPort), defaultPort);
            set => Preferences.Set(nameof(ServerPort), value);
        }

        public static string ServerIP
        {
            get => Preferences.Get(nameof(ServerIP), defaultIP);
            set => Preferences.Set(nameof(ServerIP), value);
        }

        static Random random = new Random();
        static readonly string defaultName = $"{DeviceInfo.Platform} User {random.Next(1, 100)}";
        public static string UserName
        {
            get => Preferences.Get(nameof(UserName), defaultName);
            set => Preferences.Set(nameof(UserName), value);
        }


        public static string Group
        {
            //get => Preferences.Get(nameof(Group), string.Empty);
            get => TakeOnThis.Shared.Models.ChatSettings.DefaultChatGroup;

            set => Preferences.Set(nameof(Group), value);
        }

        public static string MediaInfo
        {
            get => Preferences.Get(nameof(MediaInfo), mefiaInfo);
            set => Preferences.Set(nameof(MediaInfo), value);
        }


        public static VoteInfo VoteInfo
        {
            //get => Preferences.Get(nameof(VoteInfo), voteInfo);
            //set => Preferences.Set(nameof(VoteInfo), value);
            get;
            set;
        }


    }
}
