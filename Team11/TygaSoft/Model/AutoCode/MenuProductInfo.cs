using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class MenuProductInfo
    {
	    public object ProductId { get; set; }

        public Guid MenuId { get; set; } 
    }
}
