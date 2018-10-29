using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ActivityPictureNewInfo
    {
	    public Guid ActivityId { get; set; }

        public Guid PictureId { get; set; } 
    }
}
