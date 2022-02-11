namespace TakeOnThis.Shared.Models
{
    public class SubmitQuestion
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Character { get; set; }

        public string Usernanme { get; set; }
    }

    public class SubmitVote
    {
        public string Character { get; set; }

        public string Usernanme { get; set; }
    }

    public class VoteResponse
    {
        public double DanielVote { get; set; }

        public double LauraVote { get; set; }

        public double TahereVote { get; set; }
    }


}
