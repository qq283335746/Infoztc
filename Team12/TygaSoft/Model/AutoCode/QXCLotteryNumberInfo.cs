using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class QXCLotteryNumberInfo
    {
	    public object Id { get; set; }

        public string QS { get; set; } 

public string HNQS { get; set; } 

public DateTime LotteryTime { get; set; } 

public string LotteryNo { get; set; } 

public DateTime ExpiryClosingDate { get; set; } 

public Int64 SalesVolume { get; set; } 

public Int64 Progressive { get; set; } 

public string ContentText { get; set; } 

public Guid UserId { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
