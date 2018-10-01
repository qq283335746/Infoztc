using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class TopicSubjectInfo
    {
	    public object Id { get; set; }

        public Guid UserId { get; set; } 

public string Title { get; set; } 

public string ContentText { get; set; } 

public Boolean IsTop { get; set; } 

public Int32 Sort { get; set; } 

public string Remark { get; set; } 

public Boolean IsDisable { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
