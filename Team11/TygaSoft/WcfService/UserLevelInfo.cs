using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TygaSoft.WcfService
{
    [DataContract(Namespace = "TygaSoft.Services.HnztcQueueService")]
    public class UserLevelInfo
    {
        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public bool IsReduce { get; set; }

        [DataMember]
        public int TotalGold { get; set; }

        [DataMember]
        public int TotalSilver { get; set; }

        [DataMember]
        public int TotalIntegral { get; set; }

        [DataMember]
        public int FunCode { get; set; }

        [DataMember]
        public int EnumSource { get; set; }

    }
}
