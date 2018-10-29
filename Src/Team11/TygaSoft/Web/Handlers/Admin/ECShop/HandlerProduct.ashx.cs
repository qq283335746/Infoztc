using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Handlers.Admin.ECShop
{
    /// <summary>
    /// HandlerProduct 的摘要说明
    /// </summary>
    public class HandlerProduct : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
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
                    case "SaveProductItem":
                        SaveProductItem(context);
                        break;
                    case "SaveProductDetail":
                        SaveProductDetail(context);
                        break;
                    case "SaveProductAttr":
                        SaveProductAttr(context);
                        break;
                    case "SaveProductSizePicture":
                        SaveProductSizePicture(context);
                        break;
                    case "SaveProductSize":
                        SaveProductSize(context);
                        break;
                    case "SaveProductImage":
                        SaveProductImage(context);
                        break;
                    case "SaveProductStock":
                        SaveProductStock(context);
                        break;
                    case "SaveProductAttrTemplate":
                        SaveProductAttrTemplate(context);
                        break;
                    case "GetJsonForDatagrid":
                        GetJsonForDatagrid(context);
                        break;
                    case "GetProductItemJsonForDatagrid":
                        GetProductItemJsonForDatagrid(context);
                        break;
                    case "GetProductItemJsonForCombobox":
                        GetProductItemJsonForCombobox(context);
                        break;
                    case "GetProductSizeJsonForCombobox":
                        GetProductSizeJsonForCombobox(context);
                        break;
                    case "GetProductDetailJsonForDatagrid":
                        GetProductDetailJsonForDatagrid(context);
                        break;
                    case "GetProductAttrJsonForDatagrid":
                        GetProductAttrJsonForDatagrid(context);
                        break;
                    case "GetProductSizePictureJsonForDatagrid":
                        GetProductSizePictureJsonForDatagrid(context);
                        break;
                    case "GetProductSizeJsonForDatagrid":
                        GetProductSizeJsonForDatagrid(context);
                        break;
                    case "GetProductImageJsonForDatagrid":
                        GetProductImageJsonForDatagrid(context);
                        break;
                    case "GetProductStockJsonForDatagrid":
                        GetProductStockJsonForDatagrid(context);
                        break;
                    case "GetProductAttrTemplateForDatagrid":
                        GetProductAttrTemplateForDatagrid(context);
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
            context.Response.ContentType = "text/plain";

            string Id = context.Request.Form["ctl00$cphMain$hId"].Trim();
            string sName = context.Request.Form["ctl00$cphMain$txtName"].Trim();
            string sSort = context.Request.Form["ctl00$cphMain$txtSort"].Trim();
            string sEnableStartTime = context.Request.Form["ctl00$cphMain$txtEnableStartTime"].Trim();
            string sEnableEndTime = context.Request.Form["ctl00$cphMain$txtEnableEndTime"].Trim();
            string sIsEnable = context.Request.Form["isEnable"].Trim();
            string sIsDisable = context.Request.Form["isDisable"].Trim();
            string sPictureId = context.Request.Form["ctl00$cphMain$hImgProductPictureId"].Trim();

            int sort = 0;
            if (sSort != "")
            {
                int.TryParse(sSort, out sort);
            }
            DateTime enableStartTime = DateTime.MinValue;
            DateTime enableEndTime = DateTime.MinValue;
            DateTime.TryParse(sEnableStartTime, out enableStartTime);
            DateTime.TryParse(sEnableEndTime, out enableEndTime);

            Guid pictureId = Guid.Empty;
            Guid.TryParse(sPictureId, out pictureId);
            string sCategoryId = context.Request.Form["categoryId"].Trim();
            Guid categoryId = Guid.Empty;
            Guid.TryParse(sCategoryId, out categoryId);
            string sBrandId = context.Request.Form["brandId"].Trim();
            Guid brandId = Guid.Empty;
            Guid.TryParse(sBrandId, out brandId);
            Guid menuId = Guid.Empty;
            Guid.TryParse(context.Request.Form["menuId"].Trim(), out menuId);
            bool isEnable = false;
            bool.TryParse(sIsEnable, out isEnable);
            bool isDisable = false;
            bool.TryParse(sIsDisable, out isDisable);

            Guid gId = Guid.Empty;
            if (Id != "") Guid.TryParse(Id, out gId);

            ProductInfo model = new ProductInfo();
            model.Id = gId;
            model.UserId = WebCommon.GetUserId();
            model.LastUpdatedDate = DateTime.Now;
            model.IsDisable = isDisable;
            model.Named = sName;
            model.PictureId = pictureId;
            model.Sort = sort;
            model.EnableStartTime = enableStartTime;
            model.EnableEndTime = enableEndTime;
            model.IsEnable = isEnable;
            model.MenuId = menuId;

            Product bll = new Product();
            CategoryProduct cpBll = new CategoryProduct();
            BrandProduct bpBll = new BrandProduct();
            MenuProduct mpBll = new MenuProduct();

            int effect = -1;

            using (TransactionScope scope = new TransactionScope())
            {
                if (!gId.Equals(Guid.Empty))
                {
                    #region 编辑数据

                    bll.Update(model);

                    CategoryProductInfo cpModel = cpBll.GetModel(gId);
                    BrandProductInfo bpModel = bpBll.GetModel(gId);
                    MenuProductInfo mpModel = mpBll.GetModel(gId);

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
                    #region 所属展示区（菜单）

                    if (!menuId.Equals(Guid.Empty))
                    {
                        if (mpModel == null)
                        {
                            mpModel = new MenuProductInfo();
                            mpModel.ProductId = gId;
                            mpModel.MenuId = menuId;
                            mpBll.Insert(mpModel);
                        }
                        else
                        {
                            if (!mpModel.MenuId.Equals(menuId))
                            {
                                mpModel.MenuId = brandId;
                                mpBll.Update(mpModel);
                            }
                        }
                    }
                    else
                    {
                        if (mpModel != null)
                        {
                            mpBll.Delete(gId, mpModel.MenuId);
                        }
                    }

                    #endregion

                    effect = 1;

                    #endregion
                }
                else
                {
                    #region 新增数据

                    gId = bll.InsertByOutput(model);
                    if (!categoryId.Equals(Guid.Empty))
                    {
                        CategoryProductInfo cpModel = new CategoryProductInfo();
                        cpModel.CategoryId = categoryId;
                        cpModel.ProductId = gId;
                        cpBll.Insert(cpModel);
                    }
                    if (!brandId.Equals(Guid.Empty))
                    {
                        BrandProductInfo bpModel = new BrandProductInfo();
                        bpModel.BrandId = brandId;
                        bpModel.ProductId = gId;
                        bpBll.Insert(bpModel);
                    }
                    if (!menuId.Equals(Guid.Empty))
                    {
                        MenuProductInfo mpModel = new MenuProductInfo();
                        mpModel.MenuId = menuId;
                        mpModel.ProductId = gId;
                        mpBll.Insert(mpModel);
                    }

                    effect = 1;

                    #endregion
                }

                scope.Complete();
            }

            if (effect != 1)
            {
                context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                return;
            }

            context.Response.Write("{\"success\": true,\"message\": \"" + gId + "\"}");
        }

        private void SaveProductItem(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string sProductItemId = context.Request.Form["hProductItemId"];
            string sProductId = context.Request.Form["productId"];
            string sNamed = context.Request.Form["txtName"].Trim();
            if (string.IsNullOrWhiteSpace(sNamed))
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                return;
            }
            string sPictureId = context.Request.Form["hImgProductItemPictureId"].Trim(); ;
            string sEnableStartTime = context.Request.Form["txtEnableStartTime_ProductItem"].Trim();
            string sEnableEndTime = context.Request.Form["txtEnableEndTime_ProductItem"].Trim();
            string sIsEnable = context.Request.Form["isEnable"].Trim();
            string sIsDisable = context.Request.Form["isDisable"].Trim();

            Guid productItemId = Guid.Empty;
            Guid.TryParse(sProductItemId, out productItemId);
            Guid productId = Guid.Empty;
            Guid.TryParse(sProductId, out productId);
            Guid pictureId = Guid.Empty;
            Guid.TryParse(sPictureId, out pictureId);
            DateTime enableStartTime = DateTime.MinValue;
            DateTime enableEndTime = DateTime.MinValue;
            DateTime.TryParse(sEnableStartTime, out enableStartTime);
            DateTime.TryParse(sEnableEndTime, out enableEndTime);
            bool isEnable = false;
            bool.TryParse(sIsEnable, out isEnable);
            bool isDisable = false;
            bool.TryParse(sIsDisable, out isDisable);

            ProductItemInfo model = new ProductItemInfo();
            model.Id = productItemId;
            model.ProductId = productId;
            model.Named = sNamed;
            model.PictureId = pictureId;
            model.EnableStartTime = enableStartTime;
            model.EnableEndTime = enableEndTime;
            model.IsEnable = isEnable;
            model.IsDisable = isDisable;

            int effect = 0;

            ProductItem bll = new ProductItem();

            using (TransactionScope scope = new TransactionScope())
            {
                if (!productItemId.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                }
                else
                {
                    effect = bll.Insert(model);
                }

                scope.Complete();
            }

            if (effect == 110)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                return;
            }

            if (effect != 1)
            {
                context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                return;
            }

            context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
        }

        private void SaveProductDetail(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string sAction = context.Request.Form["hAction_ProductDetail"].Trim();

            string sProductId = context.Request.Form["productId"].Trim();
            string sProductItemId = context.Request.Form["productItemId"].Trim();
            string content = context.Request.Form["content"].Trim();
            content = HttpUtility.HtmlDecode(content);

            decimal dOriginalPrice = 0;
            decimal dProductPrice = 0;
            float discount = 0;
            decimal.TryParse(context.Request.Form["txtOriginalPrice"].Trim(), out dOriginalPrice);
            decimal.TryParse(context.Request.Form["txtProductPrice"].Trim(), out dProductPrice);
            float.TryParse(context.Request.Form["txtDiscount"].Trim(), out discount);
            string sDiscountDescr = context.Request.Form["txtDiscountDescri"].Trim();
            Guid productId = Guid.Empty;
            Guid productItemId = Guid.Empty;
            Guid.TryParse(sProductId, out productId);
            Guid.TryParse(sProductItemId, out productItemId);

            ProductDetailInfo model = new ProductDetailInfo();
            model.ProductId = productId;
            model.ProductItemId = productItemId;
            model.OriginalPrice = dOriginalPrice;
            model.ProductPrice = dProductPrice;
            model.Discount = discount;
            model.DiscountDescr = sDiscountDescr;
            model.ContentText = content;
            model.PayOption = "";
            model.ViewCount = 0;

            ProductDetail bll = new ProductDetail();
            int effect = -1;

            using (TransactionScope scope = new TransactionScope())
            {
                if (sAction == "add")
                {
                    if (!bll.IsExist(productId, productItemId))
                    {
                        effect = bll.Insert(model);
                    }
                    else
                    {
                        throw new ArgumentException("每个商品项应仅且只有一行详情数据，请不要重复操作！");
                    }
                }
                else
                {
                    effect = bll.Update(model);
                }

                scope.Complete();
            }

            if (effect == 110)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                return;
            }
            if (effect < 1)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_InvalidError + "\"}");
                return;
            }

            context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
        }

        private void SaveProductAttr(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string sAttrValue = context.Request.Form["attrValue"].Trim();
            string sProductId = context.Request.Form["productId"].Trim();
            string sProductItemId = context.Request.Form["productItemId"].Trim();

            if (string.IsNullOrWhiteSpace(sAttrValue))
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                return;
            }
            sAttrValue = HttpUtility.HtmlDecode(sAttrValue);

            try
            {
                XElement root = XElement.Parse(sAttrValue);
            }
            catch
            {
                context.Response.Write("{\"success\": false,\"message\": \"属性值参数必须符合xml格式的数据规范\"}");
                return;
            }
            Guid productId = Guid.Empty;
            Guid.TryParse(sProductId, out productId);
            Guid productItemId = Guid.Empty;
            Guid.TryParse(sProductItemId, out productItemId);

            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"success\": false,\"message\": \"商品ID值为空字符串或格式不正确，请检查\"}");
                return;
            }
            if (productItemId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"success\": false,\"message\": \"商品项ID值为空字符串或格式不正确，请检查\"}");
                return;
            }

            ProductAttrInfo model = new ProductAttrInfo();
            model.ProductId = productId;
            model.ProductItemId = productItemId;
            model.AttrValue = sAttrValue;

            ProductAttr bll = new ProductAttr();
            int effect = -1;

            using (TransactionScope scope = new TransactionScope())
            {
                var oldModel = bll.GetModel(productItemId);
                if (oldModel == null)
                {
                    effect = bll.Insert(model);
                }
                else
                {
                    effect = bll.Update(model);
                }

                scope.Complete();
            }

            if (effect == 110)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                return;
            }
            if (effect < 1)
            {
                context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                return;
            }

            context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
        }

        private void SaveProductSizePicture(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string sAction = context.Request.Form["hAction_ProductSizePicture"].Trim();

            string sProductId = context.Request.Form["productId"].Trim();
            string sProductItemId = context.Request.Form["productItemId"].Trim();
            string sName = context.Request.Form["txtName"].Trim();
            string sPictureId = context.Request.Form["hImgProductSizePictureId"].Trim();

            Guid productId = Guid.Empty;
            Guid productItemId = Guid.Empty;
            Guid.TryParse(sProductId, out productId);
            Guid.TryParse(sProductItemId, out productItemId);
            Guid pictureId = Guid.Empty;
            Guid.TryParse(sPictureId, out pictureId);

            ProductSizePictureInfo model = new ProductSizePictureInfo();
            model.ProductId = productId;
            model.ProductItemId = productItemId;
            model.Named = sName;
            model.PictureId = pictureId;

            ProductSizePicture bll = new ProductSizePicture();
            int effect = -1;

            using (TransactionScope scope = new TransactionScope())
            {
                if (sAction == "add")
                {
                    if (!bll.IsExist(productId, productItemId))
                    {
                        effect = bll.Insert(model);
                    }
                    else
                    {
                        throw new ArgumentException("每个商品项应仅且只有一行尺码表数据，请不要重复操作！");
                    }
                }
                else
                {
                    effect = bll.Update(model);
                }

                scope.Complete();
            }

            if (effect == 110)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                return;
            }
            if (effect < 1)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_InvalidError + "\"}");
                return;
            }

            context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
        }

        private void SaveProductSize(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string sProductId = context.Request.Form["productId"].Trim();
            string sProductItemId = context.Request.Form["productItemId"].Trim();
            string sSizeAppend = context.Request.Form["sizeAppend"].Trim();
            if (string.IsNullOrWhiteSpace(sSizeAppend))
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Params_InvalidError + "\"}");
                return;
            }
            sSizeAppend = HttpUtility.HtmlDecode(sSizeAppend);

            try
            {
                XElement root = XElement.Parse(sSizeAppend);
            }
            catch
            {
                context.Response.Write("{\"success\": false,\"message\": \"尺码参数必须符合xml格式的数据规范\"}");
                return;
            }

            Guid productId = Guid.Empty;
            Guid.TryParse(sProductId, out productId);
            Guid productItemId = Guid.Empty;
            Guid.TryParse(sProductItemId, out productItemId);
            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"success\": false,\"message\": \"商品ID值为空字符串或格式不正确，请检查\"}");
                return;
            }
            if (productItemId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"success\": false,\"message\": \"商品项ID值为空字符串或格式不正确，请检查\"}");
                return;
            }

            ProductSizeInfo model = new ProductSizeInfo();
            model.ProductId = productId;
            model.ProductItemId = productItemId;
            model.SizeAppend = sSizeAppend;

            ProductSize bll = new ProductSize();
            int effect = -1;

            using (TransactionScope scope = new TransactionScope())
            {
                if (!bll.IsExist(productId, productItemId))
                {
                    effect = bll.Insert(model);
                }
                else 
                {
                    effect = bll.Update(model);
                }

                scope.Complete();
            }

            if (effect < 1)
            {
                context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                return;
            }

            context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
        }

        private void SaveProductImage(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string sProductId = context.Request.Form["productId"].Trim();
            string sProductItemId = context.Request.Form["productItemId"].Trim();
            string sPictureAppend = context.Request.Form["pictureAppend"].Trim();
            if (string.IsNullOrWhiteSpace(sPictureAppend))
            {
                context.Response.Write("{\"success\": false,\"message\": \"未选择任何图片，请检查\"}");
                return;
            }
            sPictureAppend = HttpUtility.HtmlDecode(sPictureAppend);

            try
            {
                XElement xel = XElement.Parse(sPictureAppend);
                var q = from x in xel.Descendants("Add")
                        select new { pictureId = x.Attribute("PictureId").Value };
                foreach (var item in q)
                {
                    Guid pictureId = Guid.Empty;
                    Guid.TryParse(item.pictureId, out pictureId);
                    if (pictureId.Equals(Guid.Empty))
                    {
                        context.Response.Write("{\"success\": false,\"message\": \"图片选择值应满足GUID类型，请检查\"}");
                        return;
                    }
                }
            }
            catch
            {
                context.Response.Write("{\"success\": false,\"message\": \"图片属性值参数必须符合xml格式的数据规范\"}");
                return;
            }

            Guid productId = Guid.Empty;
            Guid.TryParse(sProductId, out productId);
            Guid productItemId = Guid.Empty;
            Guid.TryParse(sProductItemId, out productItemId);

            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"success\": false,\"message\": \"商品ID不存在或格式不正确，请检查\"}");
                return;
            }
            if (productItemId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"success\": false,\"message\": \"商品ID不存在或格式不正确，请检查\"}");
                return;
            }

            ProductImageInfo model = new ProductImageInfo();
            model.ProductId = productId;
            model.ProductItemId = productItemId;
            model.PictureAppend = sPictureAppend;

            ProductImage bll = new ProductImage();
            int effect = -1;

            using (TransactionScope scope = new TransactionScope())
            {
                if (!bll.IsExist(productId, productItemId))
                {
                    effect = bll.Insert(model);
                }
                else
                {
                    effect = bll.Update(model);
                }

                scope.Complete();
            }

            if (effect < 1)
            {
                context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                return;
            }

            context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
        }

        private void SaveProductStock(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string sId = context.Request.Form["hProductStockId"].Trim();
            string sProductId = context.Request.Form["productId"].Trim();
            string sProductItemId = context.Request.Form["productItemId"].Trim();
            string productSize = context.Request.Form["productSize"].Trim();
            string sStockNum = context.Request.Form["txtName"].Trim();

            Guid Id = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(sId))
            {
                Guid.TryParse(sId, out Id);
                if (Id.Equals(Guid.Empty))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.GetString(MessageContent.Request_InvalidArgument,"Id") + "\"}");
                    return;
                }
            }
            Guid productId = Guid.Empty;
            Guid.TryParse(sProductId, out productId);
            Guid productItemId = Guid.Empty;
            Guid.TryParse(sProductItemId, out productItemId);

            int stockNum = 0;
            int.TryParse(sStockNum, out stockNum);
            if (stockNum <= 0)
            {
                context.Response.Write("{\"success\": false,\"message\": \"库存必须是大于零的整数，请正确输入\"}");
                return;
            }

            ProductStockInfo model = new ProductStockInfo();
            model.Id = Id;
            model.ProductId = productId;
            model.ProductItemId = productItemId;
            model.ProductSize = productSize;
            model.StockNum = stockNum;

            ProductStock bll = new ProductStock();
            int effect = -1;

            using (TransactionScope scope = new TransactionScope())
            {
                if (bll.IsExist(productId, productItemId, productSize, Id))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                    return;
                }

                if (!Id.Equals(Guid.Empty))
                {
                    effect = bll.Update(model);
                }
                else
                {
                    effect = bll.Insert(model);
                }

                scope.Complete();
            }

            if (effect == 110)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
                return;
            }
            if (effect < 1)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_InvalidError + "\"}");
                return;
            }

            context.Response.Write("{\"success\": true,\"message\": \"" + MessageContent.Submit_Success + "\"}");
        }

        private void SaveProductAttrTemplate(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string name = context.Request.Form["name"].Trim();
            string value = context.Request.Form["value"].Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                context.Response.Write("{\"success\": false,\"message\": \"请输入模板名称！\"}");
                return;
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                context.Response.Write("{\"success\": false,\"message\": \"模板值不能为空字符串，请检查！\"}");
                return;
            }

            ProductAttrTemplate bll = new ProductAttrTemplate();
            ProductAttrTemplateInfo model = new ProductAttrTemplateInfo();
            model.TName = name;
            model.TValue = value;
            model.LastUpdatedDate = DateTime.Now;
            int effect = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                effect = bll.Insert(model);

                scope.Complete();
            }

            if (effect == 110)
            {
                context.Response.Write("{\"success\": false,\"message\": \"已存在相同的模板名称，请勿重复操作！\"}");
                return;
            }

            if (effect > 0)
            {
                context.Response.Write("{\"success\": true,\"message\": \"操作成功！\"}");
            }
            else
            {
                context.Response.Write("{\"success\": false,\"message\": \"保存失败，请检查！\"}");
            }
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
                SqlParameter parm = new SqlParameter("@ProductName", SqlDbType.NVarChar, 50);
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

            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":" + JsonConvert.SerializeObject(list) + "}");
        }

        private void GetProductItemJsonForDatagrid(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int totalRecords = 0;
            //int pageIndex = 1;
            //int pageSize = 10;
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["page"]))
            //{
            //    int.TryParse(context.Request.Form["page"], out pageIndex);
            //}
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["rows"]))
            //{
            //    int.TryParse(context.Request.Form["rows"], out pageSize);
            //}
            //if (pageIndex < 1) pageIndex = 1;
            //if (pageSize < 10) pageSize = 10;

            Guid productId = Guid.Empty;
            Guid.TryParse(context.Request.Form["productId"], out productId);
            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            string keyword = context.Request.Form["keyword"];

            //string sqlWhere = "";
            //ParamsHelper parms = null;

            if (!string.IsNullOrWhiteSpace(keyword))
            {

            }

            ProductItem bll = new ProductItem();
            var list = bll.GetListByProductId(productId);
            if (list == null || list.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            totalRecords = list.Count;
            StringBuilder sb = new StringBuilder(1000);
            foreach (var model in list)
            {
                if (!string.IsNullOrWhiteSpace(model.FileExtension) && !string.IsNullOrWhiteSpace(model.FileDirectory) && !string.IsNullOrWhiteSpace(model.RandomFolder))
                {
                    model.SPicture = PictureUrlHelper.GetMPicture(model.FileDirectory, model.RandomFolder, model.FileExtension);
                }
                sb.Append("{\"Id\":\"" + model.Id + "\",\"ProductId\":\"" + model.ProductId + "\",\"Named\":\"" + model.Named + "\",\"SPicture\":\"" + model.SPicture + "\",\"Sort\":\"" + model.Sort + "\",\"EnableStartTime\":\"" + model.EnableStartTime.ToString("yyyy-MM-dd HH:mm") + "\"");
                sb.Append(",\"EnableEndTime\":\"" + model.EnableEndTime.ToString("yyyy-MM-dd HH:mm") + "\",\"IsEnable\":\"" + model.IsEnable.ToString().ToLower() + "\",\"IsDisable\":\"" + model.IsDisable.ToString().ToLower() + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void GetProductItemJsonForCombobox(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Guid productId = Guid.Empty;
            Guid.TryParse(context.Request.Form["productId"], out productId);
            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("[]");
                return;
            }

            ProductItem bll = new ProductItem();
            var dic = bll.GetKeyValueByProductId(productId);
            if (dic == null || dic.Count == 0)
            {
                context.Response.Write("[{\"id\":\"-1\",\"text\":\"请选择\"}]");
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"id\":\"-1\",\"text\":\"请选择\"}");
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.Append(",{\"id\":\"" + kvp.Key + "\",\"text\":\"" + kvp.Value + "\"}");
            }

            context.Response.Write("[" + sb.ToString().Trim(',') + "]");
        }

        private void GetProductSizeJsonForCombobox(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Guid productId = Guid.Empty;
            Guid.TryParse(context.Request.QueryString["productId"], out productId);
            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("[{\"id\":\"-1\",\"text\":\"请选择\"}]");
                return;
            }
            Guid productItemId = Guid.Empty;
            Guid.TryParse(context.Request.QueryString["productItemId"], out productItemId);
            if (productItemId.Equals(Guid.Empty))
            {
                context.Response.Write("[{\"id\":\"-1\",\"text\":\"请选择\"}]");
                return;
            }

            ProductSize bll = new ProductSize();
            var dic = bll.GetKeyValue(productId, productItemId);
            if (dic == null || dic.Count == 0)
            {
                context.Response.Write("[{\"id\":\"-1\",\"text\":\"请选择\"}]");
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"id\":\"-1\",\"text\":\"请选择\"}");
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.Append(",{\"id\":\"" + kvp.Key + "\",\"text\":\"" + kvp.Value + "\"}");
            }

            context.Response.Write("[" + sb.ToString().Trim(',') + "]");
        }

        private void GetProductDetailJsonForDatagrid(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int totalRecords = 0;
            //int pageIndex = 1;
            //int pageSize = 10;
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["page"]))
            //{
            //    int.TryParse(context.Request.Form["page"], out pageIndex);
            //}
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["rows"]))
            //{
            //    int.TryParse(context.Request.Form["rows"], out pageSize);
            //}
            //if (pageIndex < 1) pageIndex = 1;
            //if (pageSize < 10) pageSize = 10;

            Guid productId = Guid.Empty;
            Guid.TryParse(context.Request.Form["productId"], out productId);
            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            string keyword = context.Request.Form["keyword"];

            //string sqlWhere = "";
            //ParamsHelper parms = null;

            if (!string.IsNullOrWhiteSpace(keyword))
            {

            }

            ProductDetail bll = new ProductDetail();
            var ds = bll.GetDsByProductId(productId);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            var rows = ds.Tables[0].Rows;
            totalRecords = rows.Count;
            StringBuilder sb = new StringBuilder(1000);
            foreach (DataRow row in rows)
            {
                sb.Append("{\"ProductId\":\"" + row["ProductId"] + "\",\"ProductItemId\":\"" + row["ProductItemId"] + "\",\"ProductItemName\":\"" + row["ProductItemName"] + "\",\"OriginalPrice\":\"" + row["OriginalPrice"] + "\",\"ProductPrice\":\"" + row["ProductPrice"] + "\"");
                sb.Append(",\"Discount\":\"" + row["Discount"] + "\",\"DiscountDescr\":\"" + row["DiscountDescr"] + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void GetProductAttrJsonForDatagrid(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int totalRecords = 0;
            //int pageIndex = 1;
            //int pageSize = 10;
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["page"]))
            //{
            //    int.TryParse(context.Request.Form["page"], out pageIndex);
            //}
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["rows"]))
            //{
            //    int.TryParse(context.Request.Form["rows"], out pageSize);
            //}
            //if (pageIndex < 1) pageIndex = 1;
            //if (pageSize < 10) pageSize = 10;

            Guid productId = Guid.Empty;
            Guid.TryParse(context.Request.Form["productId"], out productId);
            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            string keyword = context.Request.Form["keyword"];

            //string sqlWhere = "";
            //ParamsHelper parms = null;

            if (!string.IsNullOrWhiteSpace(keyword))
            {

            }

            ProductAttr bll = new ProductAttr();
            var ds = bll.GetDsByProductId(productId);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            var rows = ds.Tables[0].Rows;
            totalRecords = rows.Count;
            StringBuilder sb = new StringBuilder(1000);
            foreach (DataRow row in rows)
            {
                StringBuilder sbAttrValue = new StringBuilder();
                XElement xel = XElement.Parse(row["AttrValue"].ToString());
                var q = from x in xel.Descendants("Attr")
                        select new { name = x.Element("Name").Value, value = x.Element("Value").Value };
                foreach (var item in q)
                {
                    sbAttrValue.AppendFormat("{0}：{1}，", item.name, item.value);
                }

                sb.Append("{\"ProductId\":\"" + row["ProductId"] + "\",\"ProductItemId\":\"" + row["ProductItemId"] + "\",\"ProductItemName\":\"" + row["ProductItemName"] + "\",\"AttrValue\":\"" + sbAttrValue.ToString().Trim('，') + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void GetProductSizePictureJsonForDatagrid(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int totalRecords = 0;
            //int pageIndex = 1;
            //int pageSize = 10;
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["page"]))
            //{
            //    int.TryParse(context.Request.Form["page"], out pageIndex);
            //}
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["rows"]))
            //{
            //    int.TryParse(context.Request.Form["rows"], out pageSize);
            //}
            //if (pageIndex < 1) pageIndex = 1;
            //if (pageSize < 10) pageSize = 10;

            Guid productId = Guid.Empty;
            Guid.TryParse(context.Request.Form["productId"], out productId);
            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            string keyword = context.Request.Form["keyword"];

            //string sqlWhere = "";
            //ParamsHelper parms = null;

            if (!string.IsNullOrWhiteSpace(keyword))
            {

            }

            ProductSizePicture bll = new ProductSizePicture();
            var ds = bll.GetDsByProductId(productId);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            var rows = ds.Tables[0].Rows;
            totalRecords = rows.Count;
            StringBuilder sb = new StringBuilder(1000);
            foreach (DataRow row in rows)
            {
                sb.Append("{\"ProductId\":\"" + row["ProductId"] + "\",\"ProductItemId\":\"" + row["ProductItemId"] + "\",\"ProductItemName\":\"" + row["ProductItemName"] + "\",\"Named\":\"" + row["Named"] + "\"");
                sb.Append(",\"SPicture\":\"" + string.Format("{0}{1}/PC/{1}_2{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]) + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void GetProductSizeJsonForDatagrid(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int totalRecords = 0;
            //int pageIndex = 1;
            //int pageSize = 10;
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["page"]))
            //{
            //    int.TryParse(context.Request.Form["page"], out pageIndex);
            //}
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["rows"]))
            //{
            //    int.TryParse(context.Request.Form["rows"], out pageSize);
            //}
            //if (pageIndex < 1) pageIndex = 1;
            //if (pageSize < 10) pageSize = 10;

            Guid productId = Guid.Empty;
            Guid.TryParse(context.Request.Form["productId"], out productId);
            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            string keyword = context.Request.Form["keyword"];

            //string sqlWhere = "";
            //ParamsHelper parms = null;

            if (!string.IsNullOrWhiteSpace(keyword))
            {

            }

            ProductSize bll = new ProductSize();
            var ds = bll.GetDsByProductId(productId);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            var rows = ds.Tables[0].Rows;
            totalRecords = rows.Count;
            StringBuilder sb = new StringBuilder(1000);
            foreach (DataRow row in rows)
            {
                StringBuilder sbSize = new StringBuilder(500);
                XElement xel = XElement.Parse(row["SizeAppend"].ToString());
                var q = from x in xel.Descendants("Data")
                        select new { name = x.Element("Name").Value };
                foreach (var item in q)
                {
                    sbSize.AppendFormat("{0}，", item.name);
                }

                sb.Append("{\"ProductId\":\"" + row["ProductId"] + "\",\"ProductItemId\":\"" + row["ProductItemId"] + "\",\"ProductItemName\":\"" + row["ProductItemName"] + "\",\"SizeAppend\":\"" + sbSize.ToString().Trim('，') + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void GetProductImageJsonForDatagrid(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int totalRecords = 0;
            //int pageIndex = 1;
            //int pageSize = 10;
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["page"]))
            //{
            //    int.TryParse(context.Request.Form["page"], out pageIndex);
            //}
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["rows"]))
            //{
            //    int.TryParse(context.Request.Form["rows"], out pageSize);
            //}
            //if (pageIndex < 1) pageIndex = 1;
            //if (pageSize < 10) pageSize = 10;

            Guid productId = Guid.Empty;
            Guid.TryParse(context.Request.Form["productId"], out productId);
            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            string keyword = context.Request.Form["keyword"];

            //string sqlWhere = "";
            //ParamsHelper parms = null;

            if (!string.IsNullOrWhiteSpace(keyword))
            {

            }

            ProductImage bll = new ProductImage();
            PictureProduct ppBll = new PictureProduct();

            var ds = bll.GetDsByProductId(productId);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            var rows = ds.Tables[0].Rows;
            totalRecords = rows.Count;
            StringBuilder sb = new StringBuilder(1000);
            foreach (DataRow row in rows)
            {
                XElement xel = XElement.Parse(row["PictureAppend"].ToString());

                StringBuilder sbInIds = new StringBuilder(1000);
                var q = from x in xel.Descendants("Add")
                        select new { pictureId = x.Attribute("PictureId").Value };
                foreach (var item in q)
                {
                    sbInIds.AppendFormat("'{0}',",item.pictureId);
                }

                var pictureList = ppBll.GetListInIds(sbInIds.ToString().Trim(','));

                string pictureAppend = "";
                foreach (var picModel in pictureList)
                {
                    pictureAppend += PictureUrlHelper.GetMPicture(picModel.FileDirectory, picModel.RandomFolder, picModel.FileExtension) + ",";
                }
                sb.Append("{\"ProductId\":\"" + row["ProductId"] + "\",\"ProductItemId\":\"" + row["ProductItemId"] + "\",\"ProductItemName\":\"" + row["ProductItemName"] + "\",\"PictureAppend\":\"" + pictureAppend.Trim(',') + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void GetProductStockJsonForDatagrid(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int totalRecords = 0;
            //int pageIndex = 1;
            //int pageSize = 10;
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["page"]))
            //{
            //    int.TryParse(context.Request.Form["page"], out pageIndex);
            //}
            //if (!string.IsNullOrWhiteSpace(context.Request.Form["rows"]))
            //{
            //    int.TryParse(context.Request.Form["rows"], out pageSize);
            //}
            //if (pageIndex < 1) pageIndex = 1;
            //if (pageSize < 10) pageSize = 10;

            Guid productId = Guid.Empty;
            Guid.TryParse(context.Request.Form["productId"], out productId);
            if (productId.Equals(Guid.Empty))
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            string keyword = context.Request.Form["keyword"];

            //string sqlWhere = "";
            //ParamsHelper parms = null;

            if (!string.IsNullOrWhiteSpace(keyword))
            {

            }

            ProductStock bll = new ProductStock();
            var ds = bll.GetDsByProductId(productId);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            var rows = ds.Tables[0].Rows;
            totalRecords = rows.Count;
            StringBuilder sb = new StringBuilder(1000);
            foreach (DataRow row in rows)
            {
                sb.Append("{\"Id\":\"" + row["Id"] + "\",\"ProductId\":\"" + row["ProductId"] + "\",\"ProductItemId\":\"" + row["ProductItemId"] + "\",\"ProductSize\":\"" + row["ProductSize"] + "\",\"ProductItemName\":\"" + row["ProductItemName"] + "\",\"StockNum\":\"" + row["StockNum"] + "\"");
                sb.Append("},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
        }

        private void GetProductAttrTemplateForDatagrid(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

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

            string sqlWhere = "";
            ParamsHelper parms = null;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sqlWhere += "and TName like @TName ";
                SqlParameter parm = new SqlParameter("@TName", SqlDbType.NVarChar, 1000);
                parm.Value = "%" + keyword + "%";

                if (parms == null) parms = new ParamsHelper();
                parms.Add(parm);
            }

            ProductAttrTemplate bll = new ProductAttrTemplate();
            var list = bll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
            if (list == null || list.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }

            StringBuilder sb = new StringBuilder(1000);
            foreach (var model in list)
            {
                sb.Append("{\"Id\":\"" + model.Id + "\",\"TName\":\"" + model.TName + "\",\"TValue\":" + model.TValue + "},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
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