using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.Model
{
    [Serializable]
    public class CartItemInfo
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
