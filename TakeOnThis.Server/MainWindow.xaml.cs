using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using TakeOnThis.Server.Hubs;
using System.Windows.Controls;
using TakeOnThis.Shared.Models;
using TakeOnThis.Server.Controllers;

namespace TakeOnThis.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public static IHubContext<ChatHub> HUB { get; set; }

    
        private SecondaryWindow _SecondaryWindow;

      
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            if (settings["SecondaryStyle"] != null)
            {
                string windowStyle = settings["SecondaryStyle"].Value.ToString();
                WindowStyle style = (WindowStyle)Enum.Parse(typeof(WindowStyle), windowStyle);
                if (style == WindowStyle.None)
                {
                    rdSecondaryWindowFull.IsChecked = true;
                }
                else if (style == WindowStyle.SingleBorderWindow)
                {
                    rdSecondaryWindowNoraml.IsChecked = true;
                }
            }

//            FillVideoListBox();
            
        }

     

        private static void StartServer(string ip, string port)
        {
            try
            {
                //CreateWebHostBuilder(new string[] { }).Build().Run();

                var host = CreateWebHostBuilder(new string[] { ip, port }).Build();
                HUB = (IHubContext<ChatHub>)host.Services.GetService(typeof(IHubContext<ChatHub>));

                HUB.Clients.Group("Xamarin").SendAsync("ReceiveMessage", "User1", "Start");
                host.Run();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
          .ConfigureLogging(logging =>
          {
              logging.ClearProviders();
              logging.AddConsole();
          })
          .ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.ConfigureKestrel((context, options) =>
              {
                  IPAddress address = IPAddress.Parse(args[0]);
                  int port = Int32.Parse(args[1]);
                  options.Listen(address, port);
              })
              .UseStartup<Startup>();
          });




        private void FillVideoListBox()
        {
            string videoDirectory = $"{Directory.GetCurrentDirectory()}\\Video\\";
            DirectoryInfo d = new DirectoryInfo(videoDirectory);
            FileInfo[] Files = d.GetFiles();
            //foreach (var file in Files)
            //{
            //    this.lstVideos.Items.Add(file.Name);
            //}
            string[] fileNames = Files.Select(c => c.Name).ToArray();
            this.lstVideos.ItemsSource = fileNames;
        }


        

        private void btnvdOpenSecondary_Click(object sender, RoutedEventArgs e)
        {
            if (_SecondaryWindow != null)
            {
                _SecondaryWindow.Close();
            }
            _SecondaryWindow = new SecondaryWindow();

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings["SecondaryLeft"] != null && settings["SecondaryTop"] != null)
            {
                int left = Convert.ToInt32(settings["SecondaryLeft"].Value.ToString());
                int top = Convert.ToInt32(settings["SecondaryTop"].Value.ToString());
                _SecondaryWindow.Left = left;
                _SecondaryWindow.Top = top;
            }

            if (rdSecondaryWindowFull.IsChecked == true)
            {
                _SecondaryWindow.WindowStyle = WindowStyle.None;
                _SecondaryWindow.WindowState = WindowState.Maximized;
            }
            else if (rdSecondaryWindowNoraml.IsChecked == true)
            {
                _SecondaryWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                _SecondaryWindow.WindowState = WindowState.Normal;
            }
            _SecondaryWindow.Show();
        }

        private void btnvdCloseSecondary_Click(object sender, RoutedEventArgs e)
        {
            _SecondaryWindow.Close();
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                TabControl tab = (TabControl)e.Source;
              
                //do work when tab is changed
            }
        }

       

        private void rdSecondaryWindowNoraml_Checked(object sender, RoutedEventArgs e)
        {
            if (_SecondaryWindow != null)
            {
                _SecondaryWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                _SecondaryWindow.WindowState = WindowState.Normal;
            }
        }

        private void rdSecondaryWindowFull_Checked(object sender, RoutedEventArgs e)
        {
            if (_SecondaryWindow != null)
            {
                _SecondaryWindow.WindowStyle = WindowStyle.None;
                _SecondaryWindow.WindowState = WindowState.Maximized;

            }
        }

      


        
        private async void btnStartServer_Click(object sender, RoutedEventArgs e)
        {
            string localIP = Helpers.NetworkHelpers.GetLocalIPv4();
            string port = "5000";
            if (!string.IsNullOrEmpty(localIP))
            {
                txtServerAddress.Text = $"{localIP}:{port}";
                Task task = new Task(() => StartServer(localIP, port));
                task.Start();
                SetQuestionDataSource();
                MessageBox.Show("Server Started");
            }
            else
            {
                MessageBox.Show("IP NOT FOUND!");
            }
        }

        private void btnPlayVideo_Click(object sender, RoutedEventArgs e)
        {
            if (this.cmbBackgroundVideos.SelectedItem == null)
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are You Sure?", "Warning", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {

                string fileName = this.cmbBackgroundVideos.SelectionBoxItem.ToString();
                //string lang = this.cmbthLanguage.SelectionBoxItem.ToString();
                //string scence = this.cmbvdScence.SelectionBoxItem.ToString();
                string VideoDirectory = $"{Directory.GetCurrentDirectory()}\\Video";
                string videoFilePath = $"{VideoDirectory}\\{fileName}";
                if (File.Exists(videoFilePath))
                {

                    _SecondaryWindow.Play(videoFilePath);


                }
                else
                {
                    MessageBox.Show("File Does NOT Exist");
                }
            }
        }

        private void SendChat_Click(object sender, RoutedEventArgs e)
        {
            SendGroupMessage(new ServiceMessage
            {
                Command = Command.SendText,
                Text = txtChat.Text
            });
            txtChat.Text = "";
        }

        public void SendGroupMessage(ServiceMessage message)
        {
            string messageText = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            if (HUB != null)
            {
                HUB.Clients.Group(ChatSettings.DefaultChatGroup).SendAsync(ChatSettings.RecieveCommand, ChatSettings.ServerUser, messageText);
            }
        }

        private void SendImage_Click(object sender, RoutedEventArgs e)
        {
            if (this.cmbBackgroundVideos.SelectedItem == null)
            {
                return;
            }

            string fileName = this.cmbImages.SelectionBoxItem.ToString();
            SendGroupMessage(new ServiceMessage
            {
                Command = Command.SendImage,
                FileName = fileName
            });
        }

        private void btnRefreshQuestion_Click(object sender, RoutedEventArgs e)
        {
            SetQuestionDataSource();
        }

        private void SetQuestionDataSource()
        {
            this.dgQuestions.ItemsSource = null;
            this.dgQuestions.ItemsSource = MediaController.Questions;
        }
    }
}
