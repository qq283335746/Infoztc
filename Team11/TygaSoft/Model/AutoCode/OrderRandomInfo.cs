using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class OrderRandomInfo
    {
	    public string OrderCode { get; set; }

        public string Prefix { get; set; }

        public DateTime LastUpdatedDate { get; set; } 
    }
}
