using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ProductAttrInfo
    {
        public Guid ProductId { get; set; }

        public Guid ProductItemId { get; set; }

        public string AttrValue { get; set; }

    }
}
