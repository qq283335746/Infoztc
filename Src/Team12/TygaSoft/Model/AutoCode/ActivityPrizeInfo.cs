using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ActivityPrizeInfo
    {
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }

        public string PrizeName { get; set; }

        public Int32 PrizeCount { get; set; }

        public string PrizeContent { get; set; }

        public Int32 Sort { get; set; }

        public string BusinessName { get; set; }

        public string BusinessPhone { get; set; }

        public string BusinessAddress { get; set; }

        public Int32 WinningTimes { get; set; }

        public Int32 UpdateWinningTimes { get; set; }

        public string Remark { get; set; }

        public Boolean IsDisable { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
