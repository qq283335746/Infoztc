using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ErnieInfo
    {
        public Guid Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Int32 UserBetMaxCount { get; set; } 

        public Boolean IsOver { get; set; } 

        public Boolean IsDisable { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
