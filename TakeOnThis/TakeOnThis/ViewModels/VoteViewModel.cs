using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TakeOnThis.Model;
using TakeOnThis.Shared.Models;
using static TakeOnThis.Shared.Models.Questions;

namespace TakeOnThis.ViewModels
{
    public class VoteViewModel : BaseViewModel
    {
        #region Properties
        public ObservableCollection<ObservableQuestion> Options { get; }

        public ObservableCollection<ObservableVoteResponse> Votes { get; }

        ObservableQuestion selectedQuestion = null;
        public ObservableQuestion SelectedQuestion
        {
            get { return this.selectedQuestion; }
            set { SetProperty(ref this.selectedQuestion, value); }
        }

        public string Subject { get; set; }
        #endregion
        #region Commands
        public MvvmHelpers.Commands.Command LoadVoteCommand { get; }

        public MvvmHelpers.Commands.Command SubmitVoteCommand { get; }
        #endregion

        public VoteViewModel()
        {
            Options = new ObservableCollection<ObservableQuestion>();

            LoadVoteCommand = new MvvmHelpers.Commands.Command(async () => await LoadQuestions());
            SubmitVoteCommand = new MvvmHelpers.Commands.Command(async () => await SubmitVote());
        }

        private async Task SubmitVote()
        {
            try
            {
                SelectedQuestion = Options.FirstOrDefault(c => c.IsSelected);
                if (this.SelectedQuestion == null)
                {
                    return;

                }

                SubmitVote submitVote = new SubmitVote
                {
                    Character = SelectedQuestion.Key,
                    Usernanme = Helpers.Settings.UserName
                };
                Uri uri = new Uri($"{(TakeOnThis.Helpers.Settings.UseHttps ? "https" : "http")}://{TakeOnThis.Helpers.Settings.ServerIP}:{TakeOnThis.Helpers.Settings.ServerPort}/api/media/submitvote");
                var json = JsonConvert.SerializeObject(submitVote);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(uri, data);
                VoteResponse voteResult = null;

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    voteResult = JsonConvert.DeserializeObject<VoteResponse>(content);
                    Votes.Clear();

                    Votes.Add(new ObservableVoteResponse
                    {
                        Charcter = Character.Tahere.ToString(),
                        VotePercent = voteResult.TahereVote
                    });

                    Votes.Add(new ObservableVoteResponse
                    {
                        Charcter = Character.Daniel.ToString(),
                        VotePercent = voteResult.DanielVote
                    });

                    Votes.Add(new ObservableVoteResponse
                    {
                        Charcter = Character.Laura.ToString(),
                        VotePercent = voteResult.LauraVote
                    });


                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task LoadQuestions()
        {
            try
            {
                Vote.Option[] options = Helpers.Settings.VoteInfo.Vote.Options.ToArray();
                this.Subject = Helpers.Settings.VoteInfo.Vote.Subject;
                Options.Clear();
                foreach (var item in options)
                {
                    Options.Add(new ObservableQuestion
                    {
                        Key = item.Key,
                        Title = item.Value
                    });
                }
            }
            catch(Exception ex)
            {

            }

        }
    }
}
