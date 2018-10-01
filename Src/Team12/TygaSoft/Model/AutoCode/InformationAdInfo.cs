using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class InformationAdInfo
    {
	    public Guid Id { get; set; }

        public string Title { get; set; } 

public string Descr { get; set; } 

public string ContentText { get; set; } 

public string Url { get; set; } 

public Int32 ViewType { get; set; } 

public DateTime StartDate { get; set; } 

public DateTime EndDate { get; set; } 

public Guid UserId { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
