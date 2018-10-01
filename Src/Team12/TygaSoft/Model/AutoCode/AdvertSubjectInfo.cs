using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class AdvertSubjectInfo
    {
	    public Guid Id { get; set; }

        public string Title { get; set; } 

public Int32 Sort { get; set; } 

public Int32 PlayTime { get; set; } 

public string Remark { get; set; } 

public Boolean IsDisable { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
