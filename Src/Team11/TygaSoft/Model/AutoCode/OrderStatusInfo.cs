using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class OrderStatusInfo
    {
        public object OrderId { get; set; }

        public string LineNum { get; set; }

        public Byte Status { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
