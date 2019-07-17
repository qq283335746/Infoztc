using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class VoteToPlayerNewInfo
    {
	    public Guid Id { get; set; }

        public Guid PlayerId { get; set; } 

public Guid UserId { get; set; } 

public string UserFlag { get; set; } 

public Int32 TotalVoteCount { get; set; } 

public string Remark { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
