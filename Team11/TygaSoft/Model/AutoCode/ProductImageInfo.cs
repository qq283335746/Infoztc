using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ProductImageInfo
    {
        public Guid ProductId { get; set; }

        public Guid ProductItemId { get; set; }

        public string PictureAppend { get; set; }
    }
}
