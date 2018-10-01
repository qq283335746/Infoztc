using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class CategoryBrandInfo
    {
	    public object BrandId { get; set; }

        public Guid CategoryId { get; set; } 
    }
}
