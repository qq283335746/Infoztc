using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class PushUserInfo
    {
	    public Guid PushId { get; set; }

        public string PushUser { get; set; } 
    }
}
