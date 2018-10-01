using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class BrandProductInfo
    {
	    public object ProductId { get; set; }

        public Guid BrandId { get; set; } 
    }
}
