using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ActivityReleaseInfo
    {
	    public object Id { get; set; }

        public string Title { get; set; } 

public DateTime StartDate { get; set; } 

public DateTime EndDate { get; set; }

public int QuestionCount { get; set; } 

public string Remark { get; set; } 

public Boolean IsDisable { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
