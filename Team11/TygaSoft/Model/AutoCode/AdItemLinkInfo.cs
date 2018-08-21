using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class AdItemLinkInfo
    {
	    public Guid AdItemId { get; set; }

        public string Url { get; set; } 

public Guid ProductId { get; set; } 
    }
}
