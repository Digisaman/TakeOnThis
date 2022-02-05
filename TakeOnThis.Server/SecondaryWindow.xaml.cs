﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TakeOnThis.Server
{
    /// <summary>
    /// Interaction logic for SecondaryWindow.xaml
    /// </summary>
    public partial class SecondaryWindow : Window
    {
        public SecondaryWindow()
        {
            InitializeComponent();
        }

        private void videoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        public void Play(string videoFilePath)
        {
            try
            {
                this.videoPlayer.Visibility = Visibility.Visible;
                videoFilePath = videoFilePath.Replace("\\", "/");
                this.videoPlayer.Source = new Uri(videoFilePath, UriKind.Absolute);
                this.videoPlayer.Play();
                this.videoPlayer.IsMuted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void Stop()
        {
            try
            {
                this.videoPlayer.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DisplayCurrentSub(string sub)
        {
            this.txtSub.Text = sub;
        }

        public void ToggleSubVisibility(bool displaySub)
        {
            this.txtSub.Visibility = (displaySub ? Visibility.Visible : Visibility.Hidden);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                settings["SecondaryLeft"].Value = this.Left.ToString();
                settings["SecondaryTop"].Value = this.Top.ToString();
                settings["SecondaryStyle"].Value = this.WindowStyle.ToString();
                settings["SecondaryState"].Value = this.WindowState.ToString();

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }

        }
    }
}
