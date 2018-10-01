using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ProductSizeInfo
    {
	    public Guid ProductItemId { get; set; }

        public Guid ProductId { get; set; } 

public string SizeAppend { get; set; } 
    }
}
