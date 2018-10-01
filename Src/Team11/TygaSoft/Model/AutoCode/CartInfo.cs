using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class CartInfo
    {
        public object ProfileId { get; set; }

        public object ProductId { get; set; }

        public object CategoryId { get; set; }

        public Decimal Price { get; set; }

        public Int32 Quantity { get; set; }

        public string Named { get; set; }

        public Boolean IsShoppingCart { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
