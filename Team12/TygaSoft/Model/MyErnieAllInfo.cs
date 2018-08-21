using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.Model
{
    [Serializable]
    public class ErnieAllInfo
    {
        public Guid ErnieId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int UserBetMaxCount { get; set; }

        public Boolean IsOver { get; set; }

        public Boolean IsDisable { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public Guid ErnieItemId { get; set; }

        public string NumType { get; set; }

        public string Num { get; set; }

        public Double AppearRatio { get; set; }
    }
}
