using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ErnieItemInfo
    {
	    public object Id { get; set; }

        public Guid ErnieId { get; set; } 

        public string NumType { get; set; } 

        public string Num { get; set; } 

        public Double AppearRatio { get; set; } 
    }
}
