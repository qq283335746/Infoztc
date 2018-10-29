using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class TVProgramInfo
    {
	    public object Id { get; set; }

        public string ProgramName { get; set; } 

public Guid HWTVId { get; set; } 

public string ProgramAddress { get; set; } 

public Int32 Time { get; set; } 

public Boolean IsDisable { get; set; } 

public Int32 Sort { get; set; } 

public DateTime LastUpdatedDate { get; set; }
public string TVScID { get; set; }
    }
}
