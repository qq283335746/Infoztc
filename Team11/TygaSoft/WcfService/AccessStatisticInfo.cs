using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TygaSoft.WcfService
{
    [DataContract(Namespace = "TygaSoft.Services.HnztcQueueService")]
    public class AccessStatisticInfo
    {
        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public bool IsReduce { get; set; }

        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int ViewCount { get; set; }

    }
}
