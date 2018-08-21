using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.Model
{
    public class LineItemInfo
    {
        public object ProductId { get; set; }
        public string ProductName { get; set; }
        public int Line { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
