using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class UserErnieInfo
    {
	    public Guid Id { get; set; }

        public Guid UserId { get; set; } 

public Guid ErnieId { get; set; } 

public Int32 WinGold { get; set; } 

public Int32 WinSilver { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
