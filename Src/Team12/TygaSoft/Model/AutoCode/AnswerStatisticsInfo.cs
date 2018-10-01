using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class AnswerStatisticsInfo
    {
	    public object Id { get; set; }

        public Guid UserId { get; set; } 

public Guid QuestionSubjectId { get; set; }

public Guid PaperId { get; set; } 

public Boolean IsTrue { get; set; } 

public Int32 Integral { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
