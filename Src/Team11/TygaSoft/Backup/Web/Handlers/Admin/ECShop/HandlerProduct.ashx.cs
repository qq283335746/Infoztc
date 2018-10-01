using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Newtonsoft.Json;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Handlers.Admin.ECShop
{
    /// <summary>
    /// HandlerProduct 的摘要说明
    /// </summary>
    public class HandlerProduct : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string msg = "";
            try
            {
                string reqName = "";
                switch (context.Request.HttpMethod.ToUpper())
                {
                    case "GET":
                        reqName = context.Request.QueryString["reqName"];
                        break;
                    case "POST":
                        reqName = context.Request.Form["reqName"];
                        break;
                    default:
                        break;
                }

                switch (reqName)
                {
                    case "SaveProduct":
                        SaveProduct(context);
                        break;
                    case "GetJsonForDatagrid":
                        GetJsonForDatagrid(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (msg != "")
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + msg + "\"}");
            }
        }

        private void SaveProduct(HttpContext context)
        {
            string Id = context.Request.Form["ctl00$cphMain$hId"].Trim();
            string sProductName = context.Request.Form["ctl00$cphMain$txtProductName"].Trim();
            string sSubTitle = context.Request.Form["ctl00$cphMain$txtSubTitle"].Trim();
            decimal dOriginalPrice = 0;
            decimal dProductPrice = 0;
            float discount = 0;
            decimal.TryParse(context.Request.Form["ctl00$cphMain$txtOriginalPrice"].Trim(), out dOriginalPrice);
            decimal.TryParse(context.Request.Form["ctl00$cphMain$txtProductPrice"].Trim(), out dProductPrice);
            float.TryParse(context.Request.Form["ctl00$cphMain$txtDiscount"].Trim(), out discount);
            string sDiscountDescri = context.Request.Form["ctl00$cphMain$txtDiscountDescri"].Trim();
            int stockNum = 0;
            int.TryParse(context.Request.Form["ctl00$cphMain$txtStockNum"].Trim(), out stockNum);
            string sProductPictureId = context.Request.Form["productPictureId"].Trim();
            Guid productPictureId = Guid.Empty;
            Guid.TryParse(sProductPictureId, out productPictureId);
            string sCategoryId = context.Request.Form["categoryId"].Trim();
            Guid categoryId = Guid.Empty;
            Guid.TryParse(sCategoryId, out categoryId);
            string sBrandId = context.Request.Form["brandId"].Trim();
            Guid brandId = Guid.Empty;
            Guid.TryParse(sBrandId, out brandId);
            Guid customMenuId = Guid.Empty;
            Guid.TryParse(context.Request.Form["customMenuId"].Trim(), out customMenuId);
            string sOtherPicture = context.Request.Form["otherPic"].Trim();
            string sSizeItem = context.Request.Form["sizeItem"].Trim();
            string sSizePicture = context.Request.Form["sizePicture"].Trim();
            string sPayOption = "";
            
            string content = context.Request.Form["txtContent"].Trim();
            content = HttpUtility.HtmlDecode(content);
            Guid gId = Guid.Empty;
            if (Id != "") Guid.TryParse(Id, out gId);

            ProductInfo model = new ProductInfo();
            model.Id = gId;
            model.UserId = WebCommon.GetUserId();
            model.LastUpdatedDate = DateTime.Now;
            model.ProductName = sProductName;
            model.SubTitle = sSubTitle;
            model.OriginalPrice = dOriginalPrice;
            model.ProductPrice = dProductPrice;
            model.Discount = discount;
            model.DiscountDescri = sDiscountDescri;
            model.ProductPictureId = productPictureId;
            model.StockNum = stockNum;
            model.CustomMenuId = customMenuId;

            Product bll = new Product();
            ProductItem piBll = new ProductItem();
            CategoryProduct cpBll = new CategoryProduct();
            BrandProduct bpBll = new BrandProduct();

            ProductItemInfo piModel = new ProductItemInfo();
            piModel.OtherPicture = sOtherPicture;
            piModel.SizeItem = sSizeItem;
            piModel.SizePicture = sSizePicture;
            piModel.PayOption = sPayOption;
            piModel.CustomAttr = string.Empty;
            piModel.Descr = content;
            piModel.ProductId = gId;

            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                bll.Update(model);
                piBll.Update(piModel);

                CategoryProductInfo cpModel = cpBll.GetModel(gId);
                BrandProductInfo bpModel = bpBll.GetModel(gId);

                #region 所属分类

                if (!categoryId.Equals(Guid.Empty))
                {
                    if (cpModel == null)
                    {
                        cpModel = new CategoryProductInfo();
                        cpModel.ProductId = gId;
                        cpModel.CategoryId = categoryId;
                        cpBll.Insert(cpModel);
                    }
                    else
                    {
                        if (!cpModel.CategoryId.Equals(categoryId))
                        {
                            cpModel.CategoryId = categoryId;
                            cpBll.Update(cpModel);
                        }
                    }
                }
                else
                {
                    if (cpModel != null)
                    {
                        cpBll.Delete(gId, cpModel.CategoryId);
                    }
                }

                #endregion
                #region 所属品牌

                if (!brandId.Equals(Guid.Empty))
                {
                    if (bpModel == null)
                    {
                        bpModel = new BrandProductInfo();
                        bpModel.ProductId = gId;
                        bpModel.BrandId = brandId;
                        bpBll.Insert(bpModel);
                    }
                    else
                    {
                        if (!bpModel.BrandId.Equals(brandId))
                        {
                            bpModel.BrandId = brandId;
                            bpBll.Update(bpModel);
                        }
                    }
                }
                else
                {
                    if (bpModel != null)
                    {
                        bpBll.Delete(gId, bpModel.BrandId);
                    }
                }

                #endregion

                effect = 1;
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Guid productId = bll.Insert(model);
                    piModel.ProductId = productId;
                    piBll.Insert(piModel);
                    if (!categoryId.Equals(Guid.Empty))
                    {
                        CategoryProductInfo cpModel = new CategoryProductInfo();
                        cpModel.CategoryId = categoryId;
                        cpModel.ProductId = productId;
                        cpBll.Insert(cpModel);
                    }
                    if (!brandId.Equals(Guid.Empty))
                    {
                        BrandProductInfo bpModel = new BrandProductInfo();
                        bpModel.BrandId = brandId;
                        bpModel.ProductId = productId;
                        bpBll.Insert(bpModel);
                    }

                    effect = 1;

                    scope.Complete();
                }
            }

            if (effect != 1)
            {
                context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                return;
            }

            context.Response.Write("{\"success\": true,\"message\": \"操作成功\"}");
        }

        private void GetJsonForDatagrid(HttpContext context)
        {
            int totalRecords = 0;
            int pageIndex = 1;
            int pageSize = 10;
            if (!string.IsNullOrWhiteSpace(context.Request.Form["page"]))
            {
                int.TryParse(context.Request.Form["page"], out pageIndex);
            }
            if (!string.IsNullOrWhiteSpace(context.Request.Form["rows"]))
            {
                int.TryParse(context.Request.Form["rows"], out pageSize);
            }
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 10) pageSize = 10;

            string keyword = context.Request.Form["keyword"];
            string sCategoryId = context.Request.Form["categoryId"];
            string sBrandId = context.Request.Form["brandId"];

            string sqlWhere = "";
            ParamsHelper parms = null;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                sqlWhere += "and (p.ProductName like @ProductName or c.CategoryName like @CategoryName or b.BrandName like @BrandName) ";
                if (parms == null) parms = new ParamsHelper();
                SqlParameter parm = new SqlParameter("@ProductName",SqlDbType.NVarChar,50);
                parm.Value = keyword;
                parms.Add(parm);
                parm = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50);
                parm.Value = keyword;
                parms.Add(parm);
                parm = new SqlParameter("@BrandName", SqlDbType.NVarChar, 50);
                parm.Value = keyword;
                parms.Add(parm);
            }
            if (!string.IsNullOrWhiteSpace(sCategoryId))
            {
                Guid categoryId = Guid.Empty;
                Guid.TryParse(sCategoryId, out categoryId);
                if (!categoryId.Equals(Guid.Empty))
                {
                    sqlWhere += "and c.Id = @CategoryId ";
                    if (parms == null) parms = new ParamsHelper();
                    SqlParameter parm = new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier);
                    parm.Value = categoryId;
                    parms.Add(parm);
                }
            }
            if (!string.IsNullOrWhiteSpace(sBrandId))
            {
                Guid brandId = Guid.Empty;
                Guid.TryParse(sBrandId, out brandId);
                if (!brandId.Equals(Guid.Empty))
                {
                    sqlWhere += "and b.Id = @BrandId ";
                    if (parms == null) parms = new ParamsHelper();
                    SqlParameter parm = new SqlParameter("@BrandId", SqlDbType.UniqueIdentifier);
                    parm.Value = brandId;
                    parms.Add(parm);
                }
            }
                 
            ProductItem bll = new ProductItem();
            var list = bll.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
            if (list == null || list.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":"+JsonConvert.SerializeObject(list)+"}");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}