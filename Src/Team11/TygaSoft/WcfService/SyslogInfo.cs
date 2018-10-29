using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TygaSoft.WcfService
{
    [DataContract(Namespace = "TygaSoft.Services.HnztcSysService")]
    public class SyslogInfo
    {
        [DataMember]
        public object Id { get; set; }

        [DataMember]
        public string AppName { get; set; }

        [DataMember]
        public string MethodName { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public DateTime LastUpdatedDate { get; set; }
    }
}
