using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ProductAttrTemplateInfo
    {
        public object Id { get; set; }

        public string TName { get; set; }

        public string TValue { get; set; }

        public DateTime LastUpdatedDate { get; set; } 
    }
}
