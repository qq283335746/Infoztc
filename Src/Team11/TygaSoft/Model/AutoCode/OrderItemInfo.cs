using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class OrderItemInfo
    {
        public object OrderId { get; set; }

        public string LineNum { get; set; }

        public Guid ProductId { get; set; }

        public Decimal Price { get; set; }

        public Int32 Quantity { get; set; }
    }
}
