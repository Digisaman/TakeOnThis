using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TakeOnThis.Shared.Models;
using TakeOnThis.Interfaces;
using TakeOnThis.Views;

namespace TakeOnThis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilePage : ContentPage
    {
        //FileViewModel vm;
        //FileViewModel VM
        //{
        //    get => vm ?? (vm = (FileViewModel)BindingContext);
        //}
        private MediaInfo mediaInfo;
        private int currentIndex;
        private IDownloader downloader = null;
        private float progress;

        public FilePage()
        {
            InitializeComponent();
            try
            {
                downloader = DependencyService.Get<IDownloader>();
                downloader.OnFileDownloaded += Downloader_OnFileDownloaded;
                this.mediaInfo = new MediaInfo();
            }
            catch (Exception ex)
            {

            }
        }

        private async void Downloader_OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                //DisplayAlert("TakeOnThins", "File Saved Successfully", "Close");
                if (mediaInfo.CurrentCategory == DownloadCategory.Video)
                {
                    if ((currentIndex+1) < mediaInfo.VIDEO.Count)
                    {
                        progress = ((float)(currentIndex + 1) / (float)mediaInfo.VIDEO.Count);
                        // directly set the new progress value
                        defaultProgressBar.Progress = progress;

                        // animate to the new value over 750 milliseconds using Linear easing
                        await defaultProgressBar.ProgressTo(progress, 500, Easing.Linear);

                        this.currentIndex++;
                        downloader.DownloadFile(mediaInfo.VIDEO[currentIndex].Url, mediaInfo.CurrentCategory.ToString());
                    }
                    else
                    {
                        await defaultProgressBar.ProgressTo(1, 500, Easing.Linear);
                        mediaInfo.CurrentCategory = DownloadCategory.Image;
                        currentIndex = 0;
                        downloader.DownloadFile(mediaInfo.IMAGE[currentIndex].Url, mediaInfo.CurrentCategory.ToString());
                        this.lblTitle.Text = "Downloading Images...";
                    }
                }
                else if (mediaInfo.CurrentCategory == DownloadCategory.Image)
                {
                    if ((currentIndex + 1) < mediaInfo.IMAGE.Count)
                    {
                        progress = ((float)(currentIndex + 1) / (float)mediaInfo.IMAGE.Count);
                        // directly set the new progress value
                        defaultProgressBar.Progress = progress;

                        // animate to the new value over 750 milliseconds using Linear easing
                        await defaultProgressBar.ProgressTo(progress, 500, Easing.Linear);

                        this.currentIndex++;
                        downloader.DownloadFile(mediaInfo.IMAGE[currentIndex].Url, mediaInfo.CurrentCategory.ToString());
                    }
                    else
                    {
                        progress = 1;
                        currentIndex = 0;
                        
                        await defaultProgressBar.ProgressTo(progress, 500, Easing.Linear);
                        this.lblTitle.Text = "Download Completed";
                        Thread.Sleep(1000);
                        NavigateMain();
                    }
                }
            }
            else
            {
                //DisplayAlert("TakeOnThins", "Error while saving the file", "Close");
            }
        }

        //private void DownloadClicked(object sender, EventArgs e)
        //{
        //    string fileName = $"VD-{cmbCharachter.SelectedItem.ToString()}-AUD-{cmbLanguage.SelectedItem.ToString()}-SC-{cmbScene.SelectedItem.ToString()}.mp3";

        //    string url = $"{(Helpers.Settings.UseHttps ? "https" : "http")}://{Helpers.Settings.ServerIP}:{Helpers.Settings.ServerPort}/Video/Audio/{fileName}";

        //    downloader.DownloadFile(url, "Audio");
        //}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            this.mediaInfo = await GetMediaInfo();

            Helpers.Settings.VoteInfo = await GetVoteInfo();


            this.currentIndex = 0;
            
            if (this.mediaInfo.VIDEO.Count > 0)
            {
                this.lblTitle.Text = "Downloading Video files...";
                mediaInfo.CurrentCategory = DownloadCategory.Video;
                downloader.DownloadFile(mediaInfo.VIDEO[currentIndex].Url, mediaInfo.CurrentCategory.ToString());
            }

        }

        public async Task<MediaInfo> GetMediaInfo()
        {
            MediaInfo info = null;
            try
            {
                Uri uri = new Uri($"{(TakeOnThis.Helpers.Settings.UseHttps ? "https" : "http")}://{TakeOnThis.Helpers.Settings.ServerIP}:{TakeOnThis.Helpers.Settings.ServerPort}/api/media/GetMediaInfo");
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    info = JsonConvert.DeserializeObject<MediaInfo>(content);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return info;
        }

        public async Task<VoteInfo> GetVoteInfo()
        {
            VoteInfo info = null;
            try
            {
                Uri uri = new Uri($"{(TakeOnThis.Helpers.Settings.UseHttps ? "https" : "http")}://{TakeOnThis.Helpers.Settings.ServerIP}:{TakeOnThis.Helpers.Settings.ServerPort}/api/media/GetVoteInfo");
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    info = JsonConvert.DeserializeObject<VoteInfo>(content);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return info;
        }

        private async void NavigateMain()
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }
}