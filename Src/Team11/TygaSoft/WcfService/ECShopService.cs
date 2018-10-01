using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.WcfService
{
    public partial class ECShopService : IECShop
    {
        string WebSiteHost = ConfigurationManager.AppSettings["WebSiteHost"].Trim('/');

        #region ICategory Member

        public string GetCategoryModel(Guid Id)
        {
            StringBuilder sb = new StringBuilder();
            Category bll = new Category();
            CategoryInfo model = bll.GetModel(Id);
            if (model == null)
            {
                return "[]";
                //return "{\"Success\":\"true\",\"Message\":\"\",\"Data\":\"[]\"}";
            }

            return sb.ToString();
            //return "{\"Success\":\"true\",\"Message\":\"\",\"Data\":\""+JsonConvert.SerializeObject(model)+"\"}";
        }

        public string GetCategoryTreeJson()
        {
            Category bll = new Category();
            return bll.GetTreeJson();
        }

        #endregion

        #region IBrand Member

        public string GetBrandTreeJson()
        {
            Brand bll = new Brand();
            return bll.GetTreeJson();
        }

        public string GetBrandListByParentId(Guid parentId)
        {
            Brand bll = new Brand();
            var list = bll.GetListByParentId(parentId);
            if (list == null || list.Count() == 0) return "[]";

            return "";
        }

        public string GetBrandListByCategoryId(Guid categoryId)
        {
            Brand bll = new Brand();
            var list = bll.GetListByCategoryId(categoryId);
            if (list == null || list.Count() == 0) return "[]";

            return "";
        }

        #endregion

        #region IProduct Member

        public string GetProductList(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            try
            {
                ProductItem bll = new ProductItem();
                var list = bll.GetListByJoin(pageIndex, pageSize, out totalRecords, string.Empty, null);
                if (list == null || list.Count() == 0) return "";

                StringBuilder sb = new StringBuilder(3000);
                sb.Append("<Rsp>");
                foreach (var model in list)
                {
                    //sb.Append("<N>");
                    //sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><SubTitle>{2}</SubTitle><OriginalPrice>{3}</OriginalPrice><ProductPrice>{4}</ProductPrice><Discount>{5}</Discount><DiscountDescri>{6}</DiscountDescri><StockNum>{7}</StockNum><OtherPicture>{8}</OtherPicture>",
                    //   model.ProductId, model.ProductName, model.SubTitle, model.OriginalPrice, model.ProductPrice, model.Discount, model.DiscountDescri, model.StockNum, model.OtherPicture
                    //);
                    //sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", WebSiteHost + model.OriginalPicture, WebSiteHost + model.BPicture, WebSiteHost + model.MPicture, WebSiteHost + model.SPicture);
                    //sb.Append("</N>");
                }
                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public string GetProductListByCategory(int pageIndex, int pageSize, out int totalRecords, Guid categoryId)
        {
            totalRecords = 0;
            try
            {
                Product pBll = new Product();
                var list = pBll.GetListByCategory(pageIndex, pageSize, out totalRecords, categoryId);
                if (list == null || list.Count() == 0) return "";

                StringBuilder sb = new StringBuilder(3000);
                sb.Append("<Rsp>");
                foreach (var model in list)
                {
                    //sb.Append("<N>");
                    //sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><SubTitle>{2}</SubTitle><OriginalPrice>{3}</OriginalPrice><ProductPrice>{4}</ProductPrice><Discount>{5}</Discount><DiscountDescri>{6}</DiscountDescri><StockNum>{7}</StockNum>",
                    //   model.Id, model.ProductName, model.SubTitle, model.OriginalPrice, model.ProductPrice, model.Discount, model.DiscountDescri, model.StockNum
                    //);
                    //sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", WebSiteHost + model.OriginalPicture, WebSiteHost + model.BPicture, WebSiteHost + model.MPicture, WebSiteHost + model.SPicture);
                    //sb.Append("</N>");
                }
                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public string GetProductListByBrand(int pageIndex, int pageSize, out int totalRecords, Guid brandId)
        {
            totalRecords = 0;
            try
            {
                Product pBll = new Product();
                var list = pBll.GetListByBrand(pageIndex, pageSize, out totalRecords, brandId);
                if (list == null || list.Count() == 0) return "";

                StringBuilder sb = new StringBuilder(3000);
                sb.Append("<Rsp>");
                foreach (var model in list)
                {
                    //sb.Append("<N>");
                    //sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><SubTitle>{2}</SubTitle><OriginalPrice>{3}</OriginalPrice><ProductPrice>{4}</ProductPrice><Discount>{5}</Discount><DiscountDescri>{6}</DiscountDescri><StockNum>{7}</StockNum>",
                    //   model.Id, model.ProductName, model.SubTitle, model.OriginalPrice, model.ProductPrice, model.Discount, model.DiscountDescri, model.StockNum
                    //);
                    //sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", WebSiteHost + model.OriginalPicture, WebSiteHost + model.BPicture, WebSiteHost + model.MPicture, WebSiteHost + model.SPicture);
                    //sb.Append("</N>");
                }
                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public string GetProductListByMenu(int pageIndex, int pageSize, out int totalRecords, Guid menuId)
        {
            totalRecords = 0;
            try
            {
                Product pBll = new Product();
                var list = pBll.GetListByMenu(pageIndex, pageSize, out totalRecords, menuId);
                if (list == null || list.Tables.Count == 0 || list.Tables[0].Rows.Count == 0) return "";

                DataRowCollection drc = list.Tables[0].Rows;
                StringBuilder sb = new StringBuilder(3000);
                sb.Append("<Rsp>");
                foreach (DataRow dr in drc)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<ProductId>{0}</ProductId><ProductName>{1}</ProductName><ProductItemId>{2}</ProductItemId><OriginalPrice>{3}</OriginalPrice><ProductPrice>{4}</ProductPrice><Discount>{5}</Discount><DiscountDescr>{6}</DiscountDescr>",
                       dr["ProductId"], dr["ProductItemName"], dr["ProductItemId"], dr["OriginalPrice"], dr["ProductPrice"], dr["Discount"], dr["DiscountDescr"]
                    );
                    sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", WebSiteHost + dr["OriginalPicture"], WebSiteHost + dr["BPicture"], WebSiteHost + dr["MPicture"], WebSiteHost + dr["SPicture"]);
                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public string GetProductDetail(Guid productId)
        {
            try
            {
                if (productId.Equals(Guid.Empty))
                {
                    return "";
                }
                Product bll = new Product();
                var model = bll.GetModelByJoin(productId);
                if (model == null) return "";

                StringBuilder sb = new StringBuilder(3000);
                //sb.Append("<Rsp>");
                //sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><SubTitle>{2}</SubTitle><OriginalPrice>{3}</OriginalPrice><ProductPrice>{4}</ProductPrice><Discount>{5}</Discount><DiscountDescri>{6}</DiscountDescri><StockNum>{7}</StockNum>",
                //       model.Id, model.ProductName, model.SubTitle, model.OriginalPrice, model.ProductPrice, model.Discount, model.DiscountDescri, model.StockNum
                //    );

                //ProductItem piBll = new ProductItem();
                //var productItemList = piBll.GetListByProductId(productId);

                //if (!string.IsNullOrWhiteSpace(model.OtherPicture))
                //{
                //    ProductPicture ppBll = new ProductPicture();
                //    var otherPictureList = ppBll.GetListInIdAppend(model.OtherPicture);
                //    if (otherPictureList != null && otherPictureList.Count > 0)
                //    {
                //        sb.Append("<OtherPictureList>");
                //        foreach (var pModel in otherPictureList)
                //        {
                //            sb.Append("<N>");
                //            sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", WebSiteHost + pModel.OriginalPicture, WebSiteHost + pModel.BPicture, WebSiteHost + pModel.MPicture, WebSiteHost + pModel.SPicture);
                //            sb.Append("</N>");
                //        }
                //        sb.Append("</OtherPictureList>");
                //    }
                //}
                //sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", WebSiteHost + model.OriginalPicture, WebSiteHost + model.BPicture, WebSiteHost + model.MPicture, WebSiteHost + model.SPicture);
                //sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        #endregion
    }
}
