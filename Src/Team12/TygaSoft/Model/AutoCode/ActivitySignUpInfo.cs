using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ActivitySignUpInfo
    {
	    public Guid Id { get; set; }

        public Guid ActivityId { get; set; } 

public Guid UserId { get; set; } 

public string Remark { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
