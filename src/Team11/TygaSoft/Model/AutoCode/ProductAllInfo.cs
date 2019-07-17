using System;

namespace TygaSoft.Model
{
    public class ProductAllInfo
    {
        #region ProductInfo

        public object ProductId { get; set; }
        public string ProductName { get; set; }
        public string SubTitle { get; set; }
        public Guid ProductPictureId { get; set; }
        public Decimal OriginalPrice { get; set; }
        public Decimal ProductPrice { get; set; }
        public Double Discount { get; set; }
        public string DiscountDescri { get; set; }
        //public Int32 StockNum { get; set; }
        public Int32 Sort { get; set; }
        public DateTime EnableStartTime { get; set; }
        public DateTime EnableEndTime { get; set; }
        public Guid UserId { get; set; }
        public Boolean IsDisable { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        #endregion

        #region ProductDetailInfo

        public string OtherPicture { get; set; }
        public string PayOption { get; set; }
        public Int32 ViewCount { get; set; }
        public string ContentText { get; set; }

        #endregion

        #region ProductItemInfo

        public object ProductItemId { get; set; }
        public string Named { get; set; }
        public Guid PictureId { get; set; }

        public string ProductItemOriginalPicture { get; set; }
        public string ProductItemBPicture { get; set; }
        public string ProductItemMPicture { get; set; }
        public string ProductItemSPicture { get; set; }

        #endregion

        #region ProductPictureInfo

        public string OriginalPicture { get; set; }

        public string BPicture { get; set; }

        public string MPicture { get; set; }

        public string SPicture { get; set; }

        #endregion

        #region Category Brand

        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string MenuName { get; set; }

        #endregion
    }
}
