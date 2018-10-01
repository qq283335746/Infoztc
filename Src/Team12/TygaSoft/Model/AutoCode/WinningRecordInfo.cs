using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class WinningRecordInfo
    {
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }

        public Guid ActivityPrizeId { get; set; }

        public Guid UserId { get; set; }

        public string UserFlag { get; set; }

        public string MobilePhone { get; set; }

        public Int32 Status { get; set; }

        public string Remark { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
