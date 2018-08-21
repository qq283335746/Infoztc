using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class InitItemsInfo
    {
	    public Guid Id { get; set; }

        public string ItemType { get; set; } 

public string ItemTypeName { get; set; } 

public string ItemName { get; set; } 

public string ItemCode { get; set; } 

public string ItemKey { get; set; } 

public Boolean IsDisable { get; set; } 

public DateTime EditTime { get; set; } 
    }
}
