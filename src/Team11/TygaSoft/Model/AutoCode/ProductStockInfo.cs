using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ProductStockInfo
    {
        public Guid Id { get; set; }

        public object ProductId { get; set; }

        public Guid ProductItemId { get; set; }

        public string ProductSize { get; set; }

        public Int32 StockNum { get; set; }
    }
}
