using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ServiceItemInfo
    {
        public object Id { get; set; }

        public string Named { get; set; }

        public Guid ParentId { get; set; }

        public Guid PictureId { get; set; }

        public Int32 Sort { get; set; }

        public Boolean HasVote { get; set; }

        public Boolean HasContent { get; set; }

        public Boolean HasLink { get; set; }

        public DateTime EnableStartTime { get; set; }

        public DateTime EnableEndTime { get; set; }

        public Boolean IsDisable { get; set; } 

        public DateTime LastUpdatedDate { get; set; }
    }
}
