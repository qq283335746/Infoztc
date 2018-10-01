using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class PushMsgInfo
    {
	    public Guid Id { get; set; }

        public string Title { get; set; } 

public string PushContent { get; set; } 

public string PushType { get; set; } 

public Boolean IsSendOk { get; set; } 

public string SendRange { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
