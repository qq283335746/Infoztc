using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class AnnouncementInfo
    {
        public Guid Id { get; set; }

        public Guid ContentTypeId { get; set; }

        public string Title { get; set; }

        public string Descr { get; set; }

        public string ContentText { get; set; }

        public Int32 VirtualViewCount { get; set; }

        public Int32 ViewCount { get; set; }

        public Int32 Sort { get; set; }

        public Boolean IsDisable { get; set; }

        public DateTime LastUpdatedDate { get; set; } 
    }
}
