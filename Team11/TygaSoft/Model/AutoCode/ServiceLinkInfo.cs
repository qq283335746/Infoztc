using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ServiceLinkInfo
    {
        public object Id { get; set; }

        public Guid ServiceItemId { get; set; }

        public Guid PictureId { get; set; }

        public string Named { get; set; }

        public string Url { get; set; }

        public Int32 Sort { get; set; }

        public DateTime EnableStartTime { get; set; }

        public DateTime EnableEndTime { get; set; }

        public Boolean IsDisable { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
