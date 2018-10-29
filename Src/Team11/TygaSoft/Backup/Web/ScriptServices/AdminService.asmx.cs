using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;
using Newtonsoft.Json;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.DBUtility;
using TygaSoft.WebHelper;
using TygaSoft.CustomProvider;

namespace TygaSoft.Web.ScriptServices
{
    /// <summary>
    /// AdminService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class AdminService : System.Web.Services.WebService
    {

        #region 菜单导航

        [WebMethod]
        public string GetTreeJsonForMenu()
        {
            string[] roles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
            var roleList = roles.ToList();
            //if (roleList.Contains("Administrators"))
            //{
            //    roleList.Remove("Administrators");
            //}
            //roleList.Add("Users");
            SitemapHelper.Roles = roleList;
            return SitemapHelper.GetTreeJsonForMenu();
        }

        #endregion

        #region 系统日志

        #endregion

        #region 用户角色

        [WebMethod]
        public string SaveRole(RoleInfo model)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MessageContent.Role_InvalidError;
            }

            model.RoleName = model.RoleName.Trim();
            if (string.IsNullOrEmpty(model.RoleName))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            if (Roles.RoleExists(model.RoleName))
            {
                return MessageContent.Submit_Exist;
            }

            Guid gId = Guid.Empty;
            if (model.RoleId != null)
            {
                Guid.TryParse(model.RoleId.ToString(), out gId);
            }

            try
            {

                Role bll = new Role();

                if (!gId.Equals(Guid.Empty))
                {
                    bll.Update(model);
                }
                else
                {
                    Roles.CreateRole(model.RoleName);
                }

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string DelRole(string itemAppend)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MessageContent.Role_InvalidError;
            }

            itemAppend = itemAppend.Trim();
            if (string.IsNullOrEmpty(itemAppend))
            {
                return MessageContent.Submit_InvalidRow;
            }
            try
            {
                string[] roleIds = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in roleIds)
                {
                    Roles.DeleteRole(item);
                }

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string SaveIsLockedOut(string userName)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MessageContent.Role_InvalidError;
            }

            try
            {
                MembershipUser user = Membership.GetUser(userName);
                if (user == null)
                {
                    return "当前用户不存在，请检查";
                }
                if (user.IsLockedOut)
                {
                    if (user.UnlockUser())
                    {
                        return "0";
                    }
                    else
                    {
                        return "操作失败，请联系管理员";
                    }
                }

                return "只有“已锁定”的用户才能执行此操作";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string SaveIsApproved(string userName)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MessageContent.Role_InvalidError;
            }

            try
            {
                MembershipUser user = Membership.GetUser(userName);
                if (user == null)
                {
                    return "当前用户不存在，请检查";
                }
                if (user.IsApproved)
                {
                    user.IsApproved = false;
                }
                else
                {
                    user.IsApproved = true;
                }

                Membership.UpdateUser(user);

                return user.IsApproved ? "1" : "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string SaveUserInRole(string userName, string roleName, bool isInRole)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MessageContent.Role_InvalidError;
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                return MessageContent.GetString(MessageContent.Request_InvalidArgument, "用户名");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return MessageContent.GetString(MessageContent.Request_InvalidArgument, "角色");
            }
            try
            {
                if (isInRole)
                {
                    if (!Roles.IsUserInRole(userName, roleName))
                    {
                        Roles.AddUserToRole(userName, roleName);
                    }
                }
                else
                {
                    if (Roles.IsUserInRole(userName, roleName))
                    {
                        Roles.RemoveUserFromRole(userName, roleName);
                    }
                }
                return "1";
            }
            catch (System.Configuration.Provider.ProviderException pex)
            {
                return pex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string GetUserInRole(string userName)
        {
            try
            {
                string[] roles = Roles.GetRolesForUser(userName);
                if (roles.Length == 0) return "";

                return string.Join(",", roles);
            }
            catch (Exception ex)
            {
                return "异常：" + ex.Message;
            }
        }

        [WebMethod]
        public string DelUser(string userName)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MessageContent.Role_InvalidError;
            }

            try
            {
                Membership.DeleteUser(userName);
                return "1";
            }
            catch (Exception ex)
            {
                return "" + MessageContent.AlertTitle_Ex_Error + "：" + ex.Message;
            }
        }

        [WebMethod]
        public string SaveUser(UserInfo model)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MessageContent.Role_InvalidError;
            }

            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            if (model.Password != model.CfmPsw)
            {
                return MessageContent.Request_InvalidCompareToPassword;
            }
            model.UserName = model.UserName.Trim();
            model.Password = model.Password.Trim();
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                model.Email = model.UserName + "tygaweb.com";
            }

            string errMsg = "";

            try
            {
                model.RoleName = model.RoleName.Trim().Trim(',');
                string[] roles = null;
                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    roles = model.RoleName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }

                MembershipCreateStatus status;
                MembershipUser user;

                using (TransactionScope scope = new TransactionScope())
                {
                    user = Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, model.IsApproved, out status);
                    if (roles != null && roles.Length > 0)
                    {
                        Roles.AddUserToRoles(model.UserName, roles);
                    }

                    scope.Complete();
                }

                if (user == null)
                {
                    return EnumMembershipCreateStatus.GetStatusMessage(status);
                }

                errMsg = "1";
            }
            catch (MembershipCreateUserException ex)
            {
                errMsg = EnumMembershipCreateStatus.GetStatusMessage(ex.StatusCode);
            }
            catch (HttpException ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }

        #endregion

        #region 数据字典

        [WebMethod]
        public string GetJsonForSysEnum()
        {
            SysEnum bll = new SysEnum();
            return bll.GetTreeJson();
        }

        [WebMethod]
        public string GetJsonForDicCode()
        {
            try
            {
                return SysEnumDataProxy.GetJsonForEnumCode("DicCode");
            }
            catch (Exception ex)
            {
                return "异常：" + ex.Message;
            }
        }

        [WebMethod]
        public string SaveSysEnum(SysEnumInfo sysEnumModel)
        {
            if (string.IsNullOrWhiteSpace(sysEnumModel.EnumName))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            if (string.IsNullOrWhiteSpace(sysEnumModel.EnumCode))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            if (string.IsNullOrWhiteSpace(sysEnumModel.EnumValue))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            Guid gId = Guid.Empty;
            Guid.TryParse(sysEnumModel.Id.ToString(), out gId);
            sysEnumModel.Id = gId;

            Guid parentId = Guid.Empty;
            Guid.TryParse(sysEnumModel.ParentId.ToString(), out parentId);
            sysEnumModel.ParentId = parentId;

            SysEnum bll = new SysEnum();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                effect = bll.Update(sysEnumModel);
            }
            else
            {
                effect = bll.Insert(sysEnumModel);
            }

            if (effect == 110)
            {
                return MessageContent.Submit_Exist;
            }
            if (effect > 0)
            {
                return "1";
            }
            else return MessageContent.Submit_Error;
        }

        [WebMethod]
        public string DelSysEnum(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return MessageContent.Submit_InvalidRow;
            }
            Guid gId = Guid.Empty;
            Guid.TryParse(id, out gId);
            if (gId.Equals(Guid.Empty))
            {
                return MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, "对应标识值");
            }
            SysEnum bll = new SysEnum();
            bll.Delete(gId);
            return "1";
        }

        #endregion

        #region 省市区

        [WebMethod]
        public string GetJsonForProvinceCity()
        {
            ProvinceCity bll = new ProvinceCity();
            return bll.GetTreeJson();
        }

        [WebMethod]
        public string SaveProvinceCity(ProvinceCityInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.Named))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            if (string.IsNullOrWhiteSpace(model.Pinyin))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            if (string.IsNullOrWhiteSpace(model.FirstChar))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            Guid gId = Guid.Empty;
            Guid.TryParse(model.Id.ToString(), out gId);
            model.Id = gId;

            Guid parentId = Guid.Empty;
            Guid.TryParse(model.ParentId.ToString(), out parentId);
            model.ParentId = parentId;
            model.LastUpdatedDate = DateTime.Now;

            ProvinceCity bll = new ProvinceCity();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                effect = bll.Update(model);
            }
            else
            {
                effect = bll.Insert(model);
            }

            if (effect == 110)
            {
                return MessageContent.Submit_Exist;
            }
            if (effect > 0)
            {
                return "1";
            }
            else return MessageContent.Submit_Error;
        }

        [WebMethod]
        public string DelProvinceCity(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return MessageContent.Submit_InvalidRow;
            }
            Guid gId = Guid.Empty;
            Guid.TryParse(id, out gId);
            if (gId.Equals(Guid.Empty))
            {
                return MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, "对应标识值");
            }
            ProvinceCity bll = new ProvinceCity();
            bll.Delete(gId);
            return "1";
        }

        #endregion

        #region 基础信息

        [WebMethod]
        public string GetPasswordByRandom()
        {
            Random rdm = new Random();
            int len = rdm.Next(6,10);
            string psw = (rdm.NextDouble() * int.MaxValue).ToString().PadLeft(6,'0');
            if(psw.Length > len) psw = psw.Substring(0,len);
            return psw;
        }

        #endregion

        #region 类别字典

        [WebMethod]
        public string GetJsonForContentType()
        {
            ContentType bll = new ContentType();
            return bll.GetTreeJson();
        }

        [WebMethod]
        public string SaveContentType(ContentTypeInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.TypeName))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            if (string.IsNullOrWhiteSpace(model.TypeCode))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            if (string.IsNullOrWhiteSpace(model.TypeValue))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            Guid gId = Guid.Empty;
            Guid.TryParse(model.Id.ToString(), out gId);
            model.Id = gId;

            Guid parentId = Guid.Empty;
            Guid.TryParse(model.ParentId.ToString(), out parentId);
            model.ParentId = parentId;
            model.LastUpdatedDate = DateTime.Now;

            ContentType bll = new ContentType();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                effect = bll.Update(model);
            }
            else
            {
                effect = bll.Insert(model);
            }

            if (effect == 110)
            {
                return MessageContent.Submit_Exist;
            }
            if (effect > 0)
            {
                return "1";
            }
            else return MessageContent.Submit_Error;
        }

        [WebMethod]
        public string DelContentType(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return MessageContent.Submit_InvalidRow;
            }
            Guid gId = Guid.Empty;
            Guid.TryParse(id, out gId);
            if (gId.Equals(Guid.Empty))
            {
                return MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, "对应标识值");
            }
            ContentType bll = new ContentType();
            bll.Delete(gId);
            return "1";
        }

        [WebMethod]
        public string GetJsonForContentTypeByTypeCode(string typeCode)
        {
            ContentType bll = new ContentType();
            return bll.GetTreeJsonForContentTypeByTypeCode(typeCode);
        }

        #endregion

        #region 公告资讯广告

        [WebMethod]
        public string DelAnnouncement(string itemsAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemsAppend = itemsAppend.Trim();
                if (string.IsNullOrEmpty(itemsAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemsAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Announcement bll = new Announcement();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string DelNotice(string itemsAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemsAppend = itemsAppend.Trim();
                if (string.IsNullOrEmpty(itemsAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemsAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Notice bll = new Notice();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string DelAdvertisement(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                IList<object> list = items.ToList<object>();

                Advertisement bll = new Advertisement();
                if (bll.DeleteBatchByJoin(list))
                {
                    return "1";
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        #endregion

        #region 电子商务

        #region 分类 品牌

        [WebMethod]
        public string GetJsonForCategory()
        {
            Category bll = new Category();
            return bll.GetTreeJson();
        }

        [WebMethod]
        public string SaveCategory(CategoryInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.CategoryName))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            if (string.IsNullOrWhiteSpace(model.CategoryCode))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            Guid gId = Guid.Empty;
            Guid.TryParse(model.Id.ToString(), out gId);
            model.Id = gId;

            Guid parentId = Guid.Empty;
            Guid.TryParse(model.ParentId.ToString(), out parentId);
            model.ParentId = parentId;

            model.LastUpdatedDate = DateTime.Now;

            Category bll = new Category();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                effect = bll.Update(model);
            }
            else
            {
                effect = bll.Insert(model);
            }

            if (effect == 110)
            {
                return MessageContent.Submit_Exist;
            }
            if (effect > 0)
            {
                return "1";
            }
            else return MessageContent.Submit_Error;
        }

        [WebMethod]
        public string DelCategory(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return MessageContent.Submit_InvalidRow;
            }
            Guid gId = Guid.Empty;
            Guid.TryParse(id, out gId);
            if (gId.Equals(Guid.Empty))
            {
                return MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, "对应标识值");
            }
            Category bll = new Category();
            bll.Delete(gId);

            return "1";
        }

        [WebMethod]
        public string GetJsonForBrand()
        {
            Brand bll = new Brand();
            return bll.GetTreeJson();
        }

        [WebMethod]
        public string SaveBrand(BrandInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.BrandName))
            {
                return MessageContent.Submit_Params_InvalidError;
            }
            if (string.IsNullOrWhiteSpace(model.BrandCode))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            Guid categoryId = Guid.Empty;
            Guid.TryParse(model.CategoryId.ToString(), out categoryId);

            Guid gId = Guid.Empty;
            Guid.TryParse(model.Id.ToString(), out gId);
            model.Id = gId;

            Guid parentId = Guid.Empty;
            Guid.TryParse(model.ParentId.ToString(), out parentId);
            model.ParentId = parentId;

            model.LastUpdatedDate = DateTime.Now;

            CategoryBrand cbBll = new CategoryBrand();
            Brand bll = new Brand();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                effect = bll.Update(model);
                CategoryBrandInfo cbModel = cbBll.GetModel(gId);

                if (!categoryId.Equals(Guid.Empty))
                {
                    if (cbModel == null)
                    {
                        cbModel = new CategoryBrandInfo();
                        cbModel.BrandId = gId;
                        cbModel.CategoryId = categoryId;
                        cbBll.Insert(cbModel);
                    }
                    else
                    {
                        if (!cbModel.CategoryId.Equals(categoryId))
                        {
                            cbModel.CategoryId = categoryId;
                            cbBll.Update(cbModel);
                        }
                    }
                }
                else
                {
                    if (cbModel != null)
                    {
                        cbBll.Delete(gId, cbModel.CategoryId);
                    }
                }
            }
            else
            {
                if (!categoryId.Equals(Guid.Empty))
                {
                    Guid brandId = bll.InsertAndGetId(model);
                    CategoryBrandInfo cbModel = new CategoryBrandInfo();
                    cbModel.BrandId = brandId;
                    cbModel.CategoryId = categoryId;
                    cbBll.Insert(cbModel);

                    effect = 1;
                }
                else
                {
                    effect = bll.Insert(model);
                }

            }

            if (effect == 110)
            {
                return MessageContent.Submit_Exist;
            }
            if (effect > 0)
            {
                return "1";
            }
            else return MessageContent.Submit_Error;
        }

        [WebMethod]
        public string DelBrand(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return MessageContent.Submit_InvalidRow;
            }
            Guid gId = Guid.Empty;
            Guid.TryParse(id, out gId);
            if (gId.Equals(Guid.Empty))
            {
                return MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, "对应标识值");
            }
            Brand bll = new Brand();
            bll.Delete(gId);

            return "1";
        }

        [WebMethod]
        public string DelCategoryPicture(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string inIds = "";
                foreach (string item in items)
                {
                    inIds += string.Format("'{0}',", item);
                }

                CategoryPicture bll = new CategoryPicture();
                var spList = bll.GetList(" and Id in (" + inIds.Trim(',') + ")");
                if (spList != null || spList.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var model in spList)
                        {
                            if (!string.IsNullOrWhiteSpace(model.OriginalPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.OriginalPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.BPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.BPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.MPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.MPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.SPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.SPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.OtherPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.OtherPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                        }

                        bll.DeleteBatch(items.ToList<object>());

                        scope.Complete();
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        #endregion

        #region 商品

        [WebMethod]
        public string DelProduct(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                IList<object> itemList = items.ToList<object>();

                Product bll = new Product();
                ProductItem piBll = new ProductItem();

                using (TransactionScope scope = new TransactionScope())
                {
                    bll.DeleteBatch(itemList);
                    bll.DeleteBatch(itemList);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string DelProductPicture(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string inIds = "";
                foreach (string item in items)
                {
                    inIds += string.Format("'{0}',",item);
                }

                ProductPicture bll = new ProductPicture();
                var ppList = bll.GetList(" and Id in (" + inIds.Trim(',') + ")");
                if (ppList != null || ppList.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var model in ppList)
                        {
                            if (!string.IsNullOrWhiteSpace(model.OriginalPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.OriginalPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.BPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.BPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.MPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.MPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.SPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.SPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.OtherPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.OtherPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                        }

                        bll.DeleteBatch(items.ToList<object>());

                        scope.Complete();
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string DelSizePicture(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string inIds = "";
                foreach (string item in items)
                {
                    inIds += string.Format("'{0}',", item);
                }

                SizePicture bll = new SizePicture();
                var spList = bll.GetList(" and Id in (" + inIds.Trim(',') + ")");
                if (spList != null || spList.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var model in spList)
                        {
                            if (!string.IsNullOrWhiteSpace(model.OriginalPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.OriginalPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.BPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.BPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.MPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.MPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.SPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.SPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.OtherPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.OtherPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                        }

                        bll.DeleteBatch(items.ToList<object>());

                        scope.Complete();
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        #endregion

        #region 订单

        [WebMethod]
        public string DelCart(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Cart bll = new Cart();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string DelOrder(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Orders bll = new Orders();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        #endregion

        #region 供应商

        [WebMethod]
        public string DelSupplier(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Supplier bll = new Supplier();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string SaveSupplier(SupplierInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.SupplierName))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            if (!string.IsNullOrWhiteSpace(model.Address))
            {
                if (string.IsNullOrWhiteSpace(model.ProvinceCityName))
                {
                    return "请选择省市区";
                }
            }

            Guid gId = Guid.Empty;
            Guid.TryParse(model.Id.ToString(), out gId);
            model.Id = gId;

            model.LastUpdatedDate = DateTime.Now;

            Supplier bll = new Supplier();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                effect = bll.Update(model);
            }
            else
            {
                effect = bll.Insert(model);
            }

            if (effect == 110)
            {
                return MessageContent.Submit_Exist;
            }

            if (effect < 1) return MessageContent.Submit_Error;

            return "1";
        }

        #endregion

        #endregion

        #region 服务

        [WebMethod]
        public string DelServicePicture(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string inIds = "";
                foreach (string item in items)
                {
                    inIds += string.Format("'{0}',", item);
                }

                ServicePicture bll = new ServicePicture();
                var spList = bll.GetList(" and Id in (" + inIds.Trim(',') + ")");
                if (spList != null || spList.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var model in spList)
                        {
                            if (!string.IsNullOrWhiteSpace(model.OriginalPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.OriginalPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.BPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.BPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.MPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.MPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.SPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.SPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(model.OtherPicture))
                            {
                                string fullPath = Server.MapPath("~" + model.OtherPicture);
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                            }
                        }

                        bll.DeleteBatch(items.ToList<object>());

                        scope.Complete();
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string GetTreeJsonForServiceItem()
        {
            ServiceItem bll = new ServiceItem();
            return bll.GetTreeJson();
        }

        [WebMethod]
        public string SaveServiceItem(ServiceItemInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.Named))
            {
                return MessageContent.Submit_Params_InvalidError;
            }

            Guid gId = Guid.Empty;
            Guid.TryParse(model.Id.ToString(), out gId);
            model.Id = gId;

            Guid parentId = Guid.Empty;
            Guid.TryParse(model.ParentId.ToString(), out parentId);
            model.ParentId = parentId;
            model.LastUpdatedDate = DateTime.Now;

            ServiceItem bll = new ServiceItem();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                var oldModel = bll.GetModel(gId);
                if (oldModel == null) return MessageContent.Submit_Data_NotExists;
                model.HasVote = oldModel.HasVote;
                model.HasContent = oldModel.HasContent;
                model.HasLink = oldModel.HasLink;
                effect = bll.Update(model);
            }
            else
            {
                model.HasVote = false;
                model.HasContent = false;
                model.HasLink = false;
                effect = bll.Insert(model);
            }

            if (effect == 110)
            {
                return MessageContent.Submit_Exist;
            }
            if (effect > 0)
            {
                return "1";
            }
            else return MessageContent.Submit_Error;
        }

        [WebMethod]
        public string DelServiceItem(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return MessageContent.Submit_InvalidRow;
            }
            Guid gId = Guid.Empty;
            Guid.TryParse(id, out gId);
            if (gId.Equals(Guid.Empty))
            {
                return MessageContent.GetString(MessageContent.Submit_Params_GetInvalidRegex, "对应标识值");
            }
            ServiceItem bll = new ServiceItem();
            bll.DeleteByJoin(gId);
            return "1";
        }

        [WebMethod]
        public string DelServiceContent(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ServiceContent bll = new ServiceContent();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string DelServiceLink(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ServiceLink bll = new ServiceLink();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        [WebMethod]
        public string DelServiceVote(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MessageContent.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ServiceVote bll = new ServiceVote();
                bll.DeleteBatch(items.ToList<object>());
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return MessageContent.AlertTitle_Ex_Error + "：" + errorMsg;
            }
            return "1";
        }

        #endregion
    }
}
