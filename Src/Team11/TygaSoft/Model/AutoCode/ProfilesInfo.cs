using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ProfilesInfo
    {
	    public Guid Id { get; set; }

        public string Username { get; set; } 

public string AppName { get; set; } 

public Boolean IsAnonymous { get; set; } 

public DateTime LastActivityDate { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
