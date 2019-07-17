using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class HWTVInfo
    {
	    public object Id { get; set; }

        public string HWTVName { get; set; } 

public object HWTVIcon { get; set; } 

public string ProgramAddress { get; set; } 

public Boolean IsTurnTo { get; set; } 

public Boolean IsDisable { get; set; } 

public Int32 Sort { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
