using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class AnswerOptionInfo
    {
	    public object Id { get; set; }

        public Guid QuestionSubjectId { get; set; } 

public string OptionContent { get; set; } 

public Int32 Sort { get; set; } 

public Boolean IsTrue { get; set; } 

public string Remark { get; set; } 

public Boolean IsDisable { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
