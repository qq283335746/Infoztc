using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class CategoryProductInfo
    {
	    public object ProductId { get; set; }

        public Guid CategoryId { get; set; } 
    }
}
