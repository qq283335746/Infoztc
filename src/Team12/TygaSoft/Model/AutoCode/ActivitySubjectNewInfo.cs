using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ActivitySubjectNewInfo
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ContentText { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string SignUpRule { get; set; }

        public Int32 MaxVoteCount { get; set; }

        public Int32 VoteMultiple { get; set; }

        public Int32 MaxSignUpCount { get; set; }

        public Int32 SignUpCount { get; set; }

        public Int64 ViewCount { get; set; }

        public Int32 VirtualSignUpCount { get; set; }

        public Int32 Sort { get; set; }

        public string HiddenAttribute { get; set; }

        public Boolean IsPrize { get; set; }

        public string PrizeRule { get; set; }

        public int PrizeProbability { get; set; }

        public string Remark { get; set; }

        public Boolean IsDisable { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
