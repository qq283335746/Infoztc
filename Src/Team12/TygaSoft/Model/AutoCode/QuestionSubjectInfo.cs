using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class QuestionSubjectInfo
    {
	    public object Id { get; set; }

        public Guid QuestionBankId { get; set; } 

public string QuestionContent { get; set; } 

public Int32 QuestionType { get; set; } 

public Int32 Sort { get; set; } 

public string Remark { get; set; } 

public Boolean IsDisable { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
