using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ActivityPlayerNewInfo
    {
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }

        public Guid UserId { get; set; }

        public int No { get; set; }

        public string Named { get; set; }

        public Int32 Age { get; set; }

        public string Occupation { get; set; }

        public string Phone { get; set; }

        public string Location { get; set; }

        public string Professional { get; set; }

        public string Descr { get; set; }

        public Int32 VoteCount { get; set; }

        public Int32 VirtualVoteCount { get; set; }

        public string Remark { get; set; }

        public Boolean IsDisable { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
