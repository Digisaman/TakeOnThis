﻿using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using TakeOnThis.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(TakeOnThis.Droid.AndroidDownloader))]
namespace TakeOnThis.Droid
{

    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public void DownloadFile(string url, string folder)
        {


            try
            {

                string appDataDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                string pathToNewFolder = Path.Combine(appDataDirectory, folder);
                string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                //if (!File.Exists(pathToNewFile))
                //{
                    Directory.CreateDirectory(pathToNewFolder);
                    WebClient webClient = new WebClient();
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
                //}
                //else
                //{
                //    if (OnFileDownloaded != null)
                //        OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
                //}
            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
            else
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
            }
        }


    }
}