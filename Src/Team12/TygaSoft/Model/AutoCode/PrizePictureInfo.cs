using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class PrizePictureInfo
    {
	    public Guid ActivityPrizeId { get; set; }

        public Guid PictureId { get; set; } 
    }
}
