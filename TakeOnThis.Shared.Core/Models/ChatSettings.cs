using System;
using System.Collections.Generic;
using System.Text;

namespace TakeOnThis.Shared.Models
{
    public class ChatSettings
    {
        public static readonly string DefaultChatGroup = "TakeOnThis";

        public static readonly string RecieveCommand = "ReceiveMessage";

        public static readonly string EnteredCommand = "Entered";

        public static readonly string LeftCommand = "Left";

        public static readonly string EnteredOrLeftCommand = "EnteredOrLeft";


        public static readonly string ServerUser = "SERVER";

        public static readonly string DefaultHttpPort = "5000";

        public static readonly string DefaultHttpsPort = "5001";
    }
}
