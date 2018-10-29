using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ServiceUnionInfo
    {
        public object Id { get; set; }

        public string Named { get; set; }

        public string Descr { get; set; }

        public int Sort { get; set; }

        public object PictureId { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string Flag { get; set; }

        public int TotalPraise { get; set; }

        public int TotalVole { get; set; }

        public bool IsUserPraise { get; set; }

        public string FileExtension { get; set; }

        public string FileDirectory { get; set; }

        public string RandomFolder { get; set; }
    }
}
