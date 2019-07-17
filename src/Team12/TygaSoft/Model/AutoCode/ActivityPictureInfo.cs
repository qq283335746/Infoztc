using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ActivityPictureInfo
    {
	    public Guid ActivityId { get; set; }

        public Guid PictureId { get; set; } 
    }
}
