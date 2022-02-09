namespace TakeOnThis.Shared.Models
{
    public class ServiceMessage
    {

        public Command Command { get; set; }

        public DownloadCategory DownloadCategory { get; set; }
        public string Text { get; set; }

        public string FileName { get; set; }
    }
}
