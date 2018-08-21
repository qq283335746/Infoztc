using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ProductDetailInfo
    {
	    public object ProductId { get; set; }

        public Guid ProductItemId { get; set; } 

public Decimal OriginalPrice { get; set; } 

public Decimal ProductPrice { get; set; } 

public Double Discount { get; set; } 

public string DiscountDescr { get; set; } 

public string ContentText { get; set; } 

public string PayOption { get; set; } 

public Int32 ViewCount { get; set; } 
    }
}
