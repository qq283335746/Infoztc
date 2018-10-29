using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class SysLogInfo
    {
        public object Id { get; set; }

        public string AppName { get; set; }

        public string MethodName { get; set; }

        public string Message { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
