using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class AdItemInfo
    {
        public Guid Id { get; set; }

        public Guid AdvertisementId { get; set; }

        public Guid PictureId { get; set; }

        public Guid ActionTypeId { get; set; }

        public Int32 Sort { get; set; }

        public Boolean IsDisable { get; set; }
    }
}
