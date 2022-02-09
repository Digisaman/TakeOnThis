using System.Collections.Generic;

namespace TakeOnThis.Shared.Models
{
    public partial class MediaInfo
    {
        public MediaInfo()
        {   
            IMAGE = new List<MediaFile>();
            VIDEO = new List<MediaFile>();
        }
        public List<MediaFile> IMAGE { get; set; }
        

        public List<MediaFile> VIDEO { get; set; }

        public DownloadCategory CurrentCategory { get; set; }
    }

    public class MediaFile
    {
        public string FileName { get; set; }

        public string Version { get; set; }

        public string Url { get; set; }

        public bool IsAvailable { get; set; }
        public long FileSize { get; set; }
    }
}
