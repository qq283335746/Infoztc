using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class AdItemContentInfo
    {
	    public Guid AdItemId { get; set; }

        public string Descr { get; set; } 

public string ContentText { get; set; } 
    }
}
