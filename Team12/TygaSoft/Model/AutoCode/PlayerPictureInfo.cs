using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class PlayerPictureInfo
    {
	    public Guid PlayerId { get; set; }

        public Guid PictureId { get; set; } 
    }
}
