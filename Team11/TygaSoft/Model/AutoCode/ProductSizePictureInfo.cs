using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ProductSizePictureInfo
    {
        public object ProductId { get; set; }

        public Guid ProductItemId { get; set; }

        public string Named { get; set; }

        public Guid PictureId { get; set; }
    }
}
