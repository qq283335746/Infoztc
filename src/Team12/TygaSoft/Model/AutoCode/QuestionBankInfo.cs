using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class QuestionBankInfo
    {
	    public object Id { get; set; }

        public string Named { get; set; } 

public string Remark { get; set; } 

public Boolean IsDisable { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
