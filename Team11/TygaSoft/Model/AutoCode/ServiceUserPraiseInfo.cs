using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ServiceUserPraiseInfo
    {
	    public object Id { get; set; }

        public Guid UserId { get; set; } 

public Guid ServiceItemId { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
