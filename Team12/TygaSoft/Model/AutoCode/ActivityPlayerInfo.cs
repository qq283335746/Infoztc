using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ActivityPlayerInfo
    {
	    public Guid Id { get; set; }

        public Guid ActivityId { get; set; } 

public string Named { get; set; } 

public string HeadPicture { get; set; } 

public string DetailInformation { get; set; } 

public Int32 ActualVoteCount { get; set; } 

public Int32 UpdateVoteCount { get; set; } 

public string Remark { get; set; } 

public Boolean IsDisable { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
