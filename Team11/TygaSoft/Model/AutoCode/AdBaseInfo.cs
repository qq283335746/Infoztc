using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class AdBaseInfo
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Guid SiteFunId { get; set; }

        public Guid LayoutPositionId { get; set; }

        public Int32 Timeout { get; set; }

        public Int32 Sort { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Int32 VirtualViewCount { get; set; }

        public Int32 ViewCount { get; set; }

        public Boolean IsDisable { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
