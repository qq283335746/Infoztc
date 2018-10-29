using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class PlayerPictureNewInfo
    {
	    public Guid PlayerId { get; set; }

        public Guid PictureId { get; set; } 

public Int32 Sort { get; set; } 

public Boolean IsHeadImg { get; set; } 
    }
}
