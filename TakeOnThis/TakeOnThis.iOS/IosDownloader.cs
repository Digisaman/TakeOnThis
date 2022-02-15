using Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TakeOnThis.Interfaces;
using TakeOnThis.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosDownloader))]
namespace TakeOnThis.iOS
{

    public class IosDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public void DownloadFile(string url, string folder)
        {
            string appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string pathToNewFolder = Path.Combine(appDataDirectory, folder);
            string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
            if (!File.Exists(pathToNewFile))
            {
                Directory.CreateDirectory(pathToNewFolder);
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
            }
            else
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
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