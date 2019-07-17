using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class BrandInfo
    {
        public object Id { get; set; }

        public string BrandName { get; set; }

        public string BrandCode { get; set; }

        public Guid ParentId { get; set; }

        public Guid PictureId { get; set; }

        public Int32 Sort { get; set; }

        public string Remark { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
