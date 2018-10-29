using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class CategoryInfo
    {
        public object Id { get; set; }

        public string CategoryName { get; set; }

        public string CategoryCode { get; set; }

        public Guid ParentId { get; set; }

        public Guid PictureId { get; set; }

        public Int32 Sort { get; set; }

        public string Remark { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
