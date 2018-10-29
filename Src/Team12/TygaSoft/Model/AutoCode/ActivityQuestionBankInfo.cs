using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ActivityQuestionBankInfo
    {
	    public object ActivityReleaseId { get; set; }

        public Guid QuestionBankId { get; set; } 

public Int32 QuestionCount { get; set; } 
    }
}
