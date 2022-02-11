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
    public class QuestionViewModel : BaseViewModel
    {
        #region Properties
        public ObservableCollection<ObservableQuestion> Questions { get; }

        ObservableQuestion selectedQuestion = null;
        public ObservableQuestion SelectedQuestion
        {
            get { return this.selectedQuestion; }
            set { SetProperty(ref this.selectedQuestion, value); }
        }

        public Character CurrentCharcter { get; set; }
        #endregion
        #region Commands
        public MvvmHelpers.Commands.Command LoadQuestionsCommand { get; }

        public MvvmHelpers.Commands.Command SubmitQuestionCommand { get; }
        #endregion

        public QuestionViewModel()
        {
            Questions = new ObservableCollection<ObservableQuestion>();

            LoadQuestionsCommand = new MvvmHelpers.Commands.Command(async () => await LoadQuestions());
            SubmitQuestionCommand = new MvvmHelpers.Commands.Command(async () => await SubmitQuestion());
        }

        private async Task SubmitQuestion()
        {
            try
            {
                SelectedQuestion = Questions.FirstOrDefault(c => c.IsSelected);
                if (this.SelectedQuestion == null)
                {
                    return;

                }

                SubmitQuestion submitQuestion = new SubmitQuestion
                {
                    Character = CurrentCharcter.ToString(),
                    Id = this.SelectedQuestion.Id,
                    Title = this.SelectedQuestion.Title,
                    Usernanme = Helpers.Settings.UserName
                };
                Uri uri = new Uri($"{(TakeOnThis.Helpers.Settings.UseHttps ? "https" : "http")}://{TakeOnThis.Helpers.Settings.ServerIP}:{TakeOnThis.Helpers.Settings.ServerPort}/api/media/submitquestion");
                var json = JsonConvert.SerializeObject(submitQuestion);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(uri, data);
                if (response.IsSuccessStatusCode)
                {
                    //Dialog Service?
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task LoadQuestions()
        {
            if (CurrentCharcter == Character.None)
            {
                return;
            }

            QuestionDetail[] questions = new QuestionDetail[0];
            switch (CurrentCharcter)
            {
                case Character.Daniel:
                    questions = Helpers.Settings.VoteInfo.Questions.Daniel.ToArray();
                    break;
                case Character.Laura:
                    questions = Helpers.Settings.VoteInfo.Questions.Laura.ToArray();
                    break;
                case Character.Tahere:
                    questions = Helpers.Settings.VoteInfo.Questions.Tahere.ToArray();
                    break;

            }

            Questions.Clear();
            foreach (var item in questions)
            {
                Questions.Add(new ObservableQuestion
                {
                    Id = item.Id,
                    IsSelected = item.IsSelected,
                    Title = item.Title
                });
            }

        }
    }
}
