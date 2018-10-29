using System;

namespace TygaSoft.Model
{
    public partial class ServiceItemInfo
    {
        public string ParentName { get; set; }

        public string FileDirectory { get; set; }
        public string RandomFolder { get; set; }
        public string FileExtension { get; set; }

        public int TotalPraise { get; set; }
        public int TotalVole { get; set; }
        public bool IsUserPraise { get; set; }

        //public string OriginalPicture { get; set; }
        //public string BPicture { get; set; }
        //public string MPicture { get; set; }
        //public string SPicture { get; set; }
        //public string OtherPicture { get; set; }
    }
}
