using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class InformationInfo
    {
	    public Guid Id { get; set; }

        public string Title { get; set; } 

public string Summary { get; set; } 

public string ContentText { get; set; } 

public string Source { get; set; } 

public Int64 ViewCount { get; set; } 

public Int32 Sort { get; set; } 

public Byte ViewType { get; set; } 

public string Remark { get; set; } 

public Boolean IsDisable { get; set; } 

public Guid UserId { get; set; }

public DateTime LastUpdatedDate { get; set; }

public Boolean IsPush { get; set; } 
    }
}
