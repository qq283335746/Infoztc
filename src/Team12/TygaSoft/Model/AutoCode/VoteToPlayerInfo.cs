using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class VoteToPlayerInfo
    {
	    public Guid Id { get; set; }

        public Guid PlayerId { get; set; } 

public Guid UserId { get; set; } 

public Int32 VoteCount { get; set; } 

public string Remark { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
