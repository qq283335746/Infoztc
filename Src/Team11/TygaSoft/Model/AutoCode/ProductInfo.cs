using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ProductInfo
    {
        public object Id { get; set; }

        public string Named { get; set; }

        public Guid PictureId { get; set; }

        public Int32 Sort { get; set; }

        public DateTime EnableStartTime { get; set; }

        public DateTime EnableEndTime { get; set; }

        public Boolean IsEnable { get; set; }

        public Boolean IsDisable { get; set; }

        public Guid UserId { get; set; }

        public decimal Price { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
