﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using TakeOnThis.Shared.Models;
using static TakeOnThis.Shared.Models.Questions;

namespace TakeOnThis.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {

        #region Properties
        private static ObservableCollection<SubmitQuestion> _Questions;
        public static ObservableCollection<SubmitQuestion> Questions
        {
            get
            {
                if (_Questions == null)
                {
                    _Questions = new ObservableCollection<SubmitQuestion>();
                }
                return _Questions;
            }
            
        }

        private static ObservableCollection<SubmitVote> _Votes;
        public static ObservableCollection<SubmitVote> Votes
        {
            get
            {
                if (_Votes == null)
                {
                    _Votes = new ObservableCollection<SubmitVote>();
                }
                return _Votes;
            }

        }
        #endregion

        [HttpGet]
        [Route(nameof(GetCurrentTime))]
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        [HttpGet]
        [Route(nameof(GetMediaInfo))]
        public MediaInfo GetMediaInfo()
        {
            DirectoryInfo mediaDirectory = new DirectoryInfo($"{Directory.GetCurrentDirectory()}\\wwwroot\\MEDIA");

            FileInfo[] files = mediaDirectory.GetFiles("*.*", SearchOption.AllDirectories);

            MediaInfo mediaInfo = null;
            string mediaInfoFilePath = $"{mediaDirectory}\\MediaInfo.json";
            using (StreamReader streamReader = new StreamReader(mediaInfoFilePath))
            {
                string content = streamReader.ReadToEnd();
                mediaInfo = JsonConvert.DeserializeObject<MediaInfo>(content);
            }

            string localIP = Helpers.NetworkHelpers.GetLocalIPv4();
            string port = "5000";
            string protocol = "http";

            List<string> urls = new List<string>();
            foreach (MediaFile mediafile in mediaInfo.IMAGE)
            {
                FileInfo file = files.FirstOrDefault(c => c.Name == mediafile.FileName);
                if (file != null)
                {
                    mediafile.Url = $"{protocol}://{localIP}:{port}/MEDIA/IMAGE/{file.Name}";
                    mediafile.IsAvailable = true;
                    mediafile.FileSize = file.Length;
                }
            }

            foreach (MediaFile mediafile in mediaInfo.VIDEO)
            {
                FileInfo file = files.FirstOrDefault(c => c.Name == mediafile.FileName);
                if (file != null)
                {
                    mediafile.Url = $"{protocol}://{localIP}:{port}/MEDIA/VIDEO/{file.Name}";
                    mediafile.IsAvailable = true;
                    mediafile.FileSize = file.Length;
                }
            }

         
            mediaInfo.IMAGE = mediaInfo.IMAGE.Where(c => c.IsAvailable).ToList();
            mediaInfo.VIDEO = mediaInfo.VIDEO.Where(c => c.IsAvailable).ToList();


            return mediaInfo;



        }


        [HttpGet]
        [Route(nameof(GetVoteInfo))]
        public VoteInfo GetVoteInfo()
        {
            DirectoryInfo mediaDirectory = new DirectoryInfo($"{Directory.GetCurrentDirectory()}\\wwwroot\\MEDIA");


            VoteInfo voteInfo = null;
            string voteInfoFilePath = $"{mediaDirectory}\\VoteInfo.json";
            using (StreamReader streamReader = new StreamReader(voteInfoFilePath))
            {
                string content = streamReader.ReadToEnd();
                voteInfo = JsonConvert.DeserializeObject<VoteInfo>(content);
            }

            return voteInfo;



        }


        [HttpPost]
        [Route(nameof(SubmitQuestion))]
        public void SubmitQuestion([FromBody] SubmitQuestion question)
        {
            try
            {
                SubmitQuestion existingQuestion = Questions.FirstOrDefault(c => c.Usernanme == question.Usernanme
                    && c.Character == question.Character);
                if (existingQuestion != null)
                {
                    existingQuestion.Character = question.Character;
                    existingQuestion.Id = question.Id;
                    existingQuestion.Title = question.Title;
                    existingQuestion.Usernanme = question.Usernanme;
                }
                else
                {
                    Questions.Add(question);
                }
            }
            catch(Exception ex)
            {

            }
        }

        [HttpPost]
        [Route(nameof(SubmitVote))]
        public VoteResponse SubmitVote([FromBody] SubmitVote vote)
        {
            
            try
            {
                
                SubmitVote existingQuestion = Votes.FirstOrDefault(c => c.Usernanme == vote.Usernanme);
                if (existingQuestion != null)
                {
                    existingQuestion.Character = vote.Character;
                    existingQuestion.Usernanme = vote.Usernanme;
                }
                else
                {
                    Votes.Add(vote);
                }

                int tahereCount = Votes.Count(c => c.Character == Character.Tahere.ToString());
                int danielCount = Votes.Count(c => c.Character == Character.Daniel.ToString());
                int lauraCount = Votes.Count(c => c.Character == Character.Laura.ToString());
                VoteResponse response = new VoteResponse();

                response.TahereVote = (((double)tahereCount) / Votes.Count)*100;
                response.DanielVote = (((double)danielCount) / Votes.Count)*100;
                response.LauraVote =(((double)lauraCount) / Votes.Count)*100;
                return response;
            }
            catch (Exception ex)
            {
                return new VoteResponse();
            }
        }


    }
}
