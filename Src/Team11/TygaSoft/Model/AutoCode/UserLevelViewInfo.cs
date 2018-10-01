using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class UserLevelViewInfo
    {
	    public Guid UserId { get; set; }

        public Int32 FunCode { get; set; } 

public Int32 EnumSource { get; set; } 

public Int32 TotalGold { get; set; } 

public Int32 TotalSilver { get; set; } 

public Int32 TotalIntegral { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
