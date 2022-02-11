using System.Collections.Generic;

namespace TakeOnThis.Shared.Models
{
    public partial class VoteInfo
    {
        public Questions Questions { get; set; }

        public Vote Vote { get; set; }
    }

    public class Questions
    {
        public List<QuestionDetail> Daniel { get; set; }

        public List<QuestionDetail> Laura { get; set; }

        public List<QuestionDetail> Tahere { get; set; }

        public Questions()
        {
            Daniel = new List<QuestionDetail>();
            Laura = new List<QuestionDetail>(); 
            Tahere = new List<QuestionDetail>();    
        }

        public class QuestionDetail
        {
            public int Id { get; set; }

            public string Title { get; set; }
        }
    }

    public class Vote
    {
        public string Subject { get; set; }

        public List<Option> Options { get; set; }

        public Vote()
        {
            Options = new List<Option>();
        }

        public class Option
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }
    }
}
