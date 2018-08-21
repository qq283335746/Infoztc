using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ActivitySubjectInfo
    {
	    public Guid Id { get; set; }

        public string Title { get; set; } 

public string ContentText { get; set; } 

public DateTime StartDate { get; set; } 

public DateTime EndDate { get; set; } 

public Byte ActivityType { get; set; } 

public Int32 MaxVoteCount { get; set; } 

public Int32 VoteMultiple { get; set; } 

public Int32 MaxSignUpCount { get; set; } 

public Int32 ActualSignUpCount { get; set; } 

public Int32 UpdateSignUpCount { get; set; } 

public Int32 Sort { get; set; } 

public string Remark { get; set; } 

public Boolean IsDisable { get; set; }

public DateTime InsertDate { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
