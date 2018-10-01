using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Threading;
using TygaSoft.SysHelper;
using TygaSoft.CustomExceptions;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.WcfService
{
    public partial class HnztcService : IHnztc
    {
        public static readonly string WebSiteHost = ConfigurationManager.AppSettings["WebSiteHost"].Trim('/');

        #region HnztcDb

        #region IAdvertisement Member

        public string GetSiteFunList()
        {
            try
            {
                ContentType ctBll = new ContentType();

                SysEnumHelper.ContentType contentTypeEnum = SysEnumHelper.ContentType.AdvertisementFun;

                var list = ctBll.GetChildListJoinByParentCode(contentTypeEnum.ToString());

                StringBuilder sb = new StringBuilder();
                sb.Append("<Rsp>");
                foreach (var model in list)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><Code>{2}</Code><Value>{3}</Value>", model.Id, model.TypeName, model.TypeCode, model.TypeValue);
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

        public string GetAdvertisementList(int pageIndex, int pageSize, Guid siteFunId)
        {
            try
            {
                int totalRecords = 0;
                Advertisement bll = new Advertisement();
                AdvertisementLink adlBll = new AdvertisementLink();
                var list = bll.GetListByFunId(pageIndex, pageSize, out totalRecords, siteFunId);
                if (list == null || list.Count == 0) return "";

                StringBuilder sb = new StringBuilder();
                sb.Append("<Rsp>");
                foreach (var model in list)
                {
                    string adLinkAppend = "";
                    var adLinkList = adlBll.GetDsByAdId(model.Id);
                    if (adLinkList != null && adLinkList.Tables.Count > 0 && adLinkList.Tables[0].Rows.Count > 0)
                    {
                        adLinkAppend += "<AdImages>";

                        DataRowCollection drc = adLinkList.Tables[0].Rows;

                        foreach (DataRow dr in drc)
                        {
                            string dir = dr["FileDirectory"] == null ? "" : dr["FileDirectory"].ToString().Trim();
                            string rndCode = dr["RandomFolder"] == null ? "" : dr["RandomFolder"].ToString().Trim();
                            string fileEx = dr["FileExtension"] == null ? "" : dr["FileExtension"].ToString().Trim();
                            Dictionary<string, string> dic = null;
                            if (!string.IsNullOrWhiteSpace(dir) && !string.IsNullOrWhiteSpace(rndCode) && !string.IsNullOrWhiteSpace(fileEx))
                            {
                                EnumData.Platform platform = EnumData.Platform.Android;
                                dic = PictureUrlHelper.GetUrlByPlatform(dir, rndCode, fileEx, platform);
                            }

                            adLinkAppend += "<AdImageInfo>";
                            adLinkAppend += string.Format(@"<ImageId>{0}</ImageId><AdId>{1}</AdId><ActionType>{2}</ActionType><Url>{3}</Url><Sort>{4}</Sort><OriginalPicture>{5}</OriginalPicture><BPicture>{6}</BPicture><MPicture>{7}</MPicture><SPicture>{8}</SPicture>",
                                dr["Id"], dr["AdvertisementId"], dr["ActionTypeCode"], dr["Url"], dr["Sort"], string.Format("{0}{1}", WebSiteHost, dic == null ? "" : dic["OriginalPicture"]), string.Format("{0}{1}", WebSiteHost, dic == null ? "" : dic["BPicture"]), string.Format("{0}{1}", WebSiteHost, dic == null ? "" : dic["MPicture"]), string.Format("{0}{1}", WebSiteHost, dic == null ? "" : dic["SPicture"]));
                            adLinkAppend += "</AdImageInfo>";
                        }
                        adLinkAppend += "</AdImages>";
                    }

                    sb.Append("<AdRes>");
                    sb.AppendFormat(@"<Id>{0}</Id><Title>{1}</Title><SiteFun>{2}</SiteFun><LayoutPosition>{3}</LayoutPosition><Duration>{4}</Duration><VirtualViewCount>{5}</VirtualViewCount><ViewCount>{6}</ViewCount><AdLink>{7}</AdLink>",
                        model.Id, model.Title, model.SiteFunName, model.LayoutPositionName, model.Timeout, model.VirtualViewCount, model.ViewCount, adLinkAppend);
                    sb.Append("</AdRes>");
                }
                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public string GetAdvertisementModel(Guid Id)
        {
            try
            {
                if (Id.Equals(Guid.Empty)) return "";

                Advertisement bll = new Advertisement();
                var model = bll.GetModelByJoin(Id);
                if (model == null) return "";

                AccessStatisticQueueClient queueClient = new AccessStatisticQueueClient();
                Services.HnztcQueueService.AccessStatisticInfo accessStatisticInfo = new Services.HnztcQueueService.AccessStatisticInfo();
                accessStatisticInfo.TableName = "Advertisement";
                accessStatisticInfo.Id = Id;
                queueClient.SaveAccessStatistic(accessStatisticInfo);

                Regex r = new Regex("(<img)(.*)src=\"([^\"]*?)\"(.*)/>");

                AdvertisementLink adlBll = new AdvertisementLink();
                string adLinkAppend = "";
                var adLinkList = adlBll.GetDsByAdId(model.Id);
                if (adLinkList != null && adLinkList.Tables.Count > 0 && adLinkList.Tables[0].Rows.Count > 0)
                {
                    adLinkAppend += "<AdImages>";

                    DataRowCollection drc = adLinkList.Tables[0].Rows;
                    foreach (DataRow dr in drc)
                    {
                        string dir = dr["FileDirectory"] == null ? "" : dr["FileDirectory"].ToString().Trim();
                        string rndCode = dr["RandomFolder"] == null ? "" : dr["RandomFolder"].ToString().Trim();
                        string fileEx = dr["FileExtension"] == null ? "" : dr["FileExtension"].ToString().Trim();
                        Dictionary<string, string> dic = null;
                        if (!string.IsNullOrWhiteSpace(dir) && !string.IsNullOrWhiteSpace(rndCode) && !string.IsNullOrWhiteSpace(fileEx))
                        {
                            EnumData.Platform platform = EnumData.Platform.Android;
                            dic = PictureUrlHelper.GetUrlByPlatform(dir, rndCode, fileEx, platform);
                        }

                        adLinkAppend += "<AdImageInfo>";
                        adLinkAppend += string.Format(@"<ImageId>{0}</ImageId><AdId>{1}</AdId><ActionType>{2}</ActionType><Url>{3}</Url><Sort>{4}</Sort><OriginalPicture>{5}</OriginalPicture><BPicture>{6}</BPicture><MPicture>{7}</MPicture><SPicture>{8}</SPicture>",
                            dr["Id"], dr["AdvertisementId"], dr["ActionTypeCode"], dr["Url"], dr["Sort"], string.Format("{0}{1}", WebSiteHost, dic == null ? "" : dic["OriginalPicture"]), string.Format("{0}{1}", WebSiteHost, dic == null ? "" : dic["BPicture"]), string.Format("{0}{1}", WebSiteHost, dic == null ? "" : dic["MPicture"]), string.Format("{0}{1}", WebSiteHost, dic == null ? "" : dic["SPicture"]));
                        adLinkAppend += "</AdImageInfo>";
                    }
                    adLinkAppend += "</AdImages>";
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<Rsp>");
                sb.AppendFormat(@"<Id>{0}</Id><Title>{1}</Title><SiteFun>{2}</SiteFun><LayoutPosition>{3}</LayoutPosition><AdTime>{4}</AdTime><AdLink>{5}</AdLink><Descr>{6}</Descr><Content><![CDATA[{7}]]></Content><VirtualViewCount>{8}</VirtualViewCount><ViewCount>{9}</ViewCount>",
                    model.Id, model.Title, model.SiteFunName, model.LayoutPositionName, model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm"), adLinkAppend, model.Descr, r.Replace(model.ContentText, "$1$2src=\"" + WebSiteHost + "$3\" />"), model.VirtualViewCount, model.ViewCount);
                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return "";
            }
        }

        #endregion

        #region IAnnouncement Member

        public string GetAnnouncementList(int pageIndex, int pageSize)
        {
            try
            {
                int totalRecords = 0;
                Announcement bll = new Announcement();
                List<AnnouncementInfo> list = bll.GetListForTitle(pageIndex, pageSize, out totalRecords, "and IsDisable = 0 ", null);
                if (list == null || list.Count == 0) return "";
                StringBuilder sb = new StringBuilder();

                sb.Append("<Rsp>");

                foreach (var model in list)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<Id>{0}</Id><Title>{1}</Title><TypeId>{2}</TypeId><Date>{3}</Date><VirtualViewCount>{4}</VirtualViewCount><ViewCount>{5}</ViewCount>", model.Id, model.Title, model.ContentTypeId, model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm"), model.VirtualViewCount, model.ViewCount);
                    sb.Append("</N>");
                }

                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return string.Empty; ;
            }
        }

        public string GetAnnouncementModel(Guid Id)
        {
            try
            {
                Announcement bll = new Announcement();
                var model = bll.GetModel(Id);
                if (model == null) return "";

                Regex r = new Regex("(<img)(.*)src=\"([^\"]*?)\"(.*)/>");

                StringBuilder sb = new StringBuilder();

                sb.Append("<Rsp>");
                sb.AppendFormat("<Id>{0}</Id><Title>{1}</Title><TypeId>{2}</TypeId><Date>{3}</Date><VirtualViewCount>{4}</VirtualViewCount><ViewCount>{5}</ViewCount><Descr><![CDATA[{6}]]></Descr><Content><![CDATA[{7}]]></Content>", model.Id, model.Title, model.ContentTypeId, model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm"), model.VirtualViewCount, model.ViewCount, model.Descr, r.Replace(model.ContentText, "$1$2src=\"" + WebSiteHost + "$3\" />"));
                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return string.Empty; ;
            }
        }

        #endregion

        #region INotice Member

        public string GetNoticeList(int pageIndex, int pageSize, out int totalRecords)
        {
            StringBuilder sb = new StringBuilder();
            Notice bll = new Notice();
            var list = bll.GetListExceptContent(pageIndex, pageSize, out totalRecords, "", null);
            if (list == null || list.Count == 0) return "[]";
            return sb.ToString();
        }

        public string GetNoticeModel(Guid Id)
        {
            StringBuilder sb = new StringBuilder();
            Notice bll = new Notice();
            var model = bll.GetModel(Id);
            if (model == null) return "[]";
            return sb.ToString();
        }

        #endregion

        #region IContentType Member

        public string GetShopMenuListByRoot()
        {
            try
            {
                EnumHelper.ShopMenu menu = EnumHelper.ShopMenu.CustomMenu;
                ContentType bll = new ContentType();
                var list = bll.GetChildListJoinByParentCode(menu.ToString());
                if (list == null || list.Count == 0) return "";
                StringBuilder sb = new StringBuilder(1000);
                sb.Append("<Rsp>");
                foreach (var model in list)
                {
                    sb.Append("<N>");
                    sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><Code>{2}</Code><HasChild>{3}</HasChild>", model.Id, model.TypeName, model.TypeCode, model.HasChild);
                    sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", WebSiteHost + model.OriginalPicture, WebSiteHost + model.BPicture, WebSiteHost + model.MPicture, WebSiteHost + model.SPicture);
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

        public string GetContentTypeChildListByParentId(Guid parentId)
        {
            ContentType bll = new ContentType();
            var list = bll.GetChildListJoinByParentId(parentId);
            if (list == null || list.Count == 0) return "";
            StringBuilder sb = new StringBuilder(1000);
            sb.Append("<Rsp>");
            foreach (var model in list)
            {
                sb.Append("<N>");
                sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><Code>{2}</Code><HasChild>{3}</HasChild>", model.Id, model.TypeName, model.TypeCode, model.HasChild);
                sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", WebSiteHost + model.OriginalPicture, WebSiteHost + model.BPicture, WebSiteHost + model.MPicture, WebSiteHost + model.SPicture);
                sb.Append("</N>");
            }
            sb.Append("</Rsp>");

            return sb.ToString();
        }

        #endregion

        #region IProvinceCity Member

        public string GetProvinceCityModel(Guid Id)
        {
            StringBuilder sb = new StringBuilder();
            ProvinceCity bll = new ProvinceCity();
            ProvinceCityInfo model = bll.GetModel(Id);
            if (model == null) return "[]";
            return sb.ToString();
        }

        public string GetProvince()
        {
            StringBuilder sb = new StringBuilder();
            ProvinceCity bll = new ProvinceCity();
            ProvinceCityInfo parentModel = bll.GetModel("中国");
            if (parentModel == null) return "[]";
            SqlParameter parm = new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier);
            parm.Value = parentModel.Id;
            List<ProvinceCityInfo> list = bll.GetList("and ParentId = @ParentId", parm);
            if (list == null || list.Count == 0) return "[]";
            return sb.ToString();
        }

        public string GetCity(Guid provinceId)
        {
            StringBuilder sb = new StringBuilder();
            ProvinceCity bll = new ProvinceCity();
            SqlParameter parm = new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(provinceId.ToString());
            List<ProvinceCityInfo> list = bll.GetList("and ParentId = @ParentId", parm);
            if (list == null || list.Count == 0) return "[]";
            return sb.ToString();
        }

        public string GetDistrict(Guid cityId)
        {
            StringBuilder sb = new StringBuilder();
            ProvinceCity bll = new ProvinceCity();
            SqlParameter parm = new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(cityId.ToString());
            List<ProvinceCityInfo> list = bll.GetList("and ParentId = @ParentId", parm);
            if (list == null || list.Count == 0) return "[]";
            return sb.ToString();
        }

        #endregion

        #region IServiceItem Member

        public string GetServiceList(string username, int pageIndex, int pageSize)
        {
            try
            {
                object userId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(username))
                {
                    MembershipUser user = Membership.GetUser(username);
                    if (user != null)
                    {
                        userId = user.ProviderUserKey;
                    }
                }
                int totalRecords = 0;
                ServiceItem bll = new ServiceItem();
                var list = bll.GetListByRoot(userId, pageIndex, pageSize, out totalRecords);
                if (list == null || list.Count == 0) return "";

                StringBuilder sb = new StringBuilder(1000);
                sb.Append("<Rsp>");
                IList<object> listArr = new List<object>();
                foreach (var model in list)
                {
                    sb.Append("<N>");

                    listArr.Clear();
                    listArr.Add(model.Id);
                    listArr.Add(model.Named);
                    listArr.Add(model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm"));
                    listArr.Add(model.TotalPraise);
                    listArr.Add(model.TotalVole);
                    listArr.Add(model.IsUserPraise);
                    sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><LastUpdatedDate>{2}</LastUpdatedDate><TotalPraise>{3}</TotalPraise><TotalVole>{4}</TotalVole><IsUserPraise>{5}</IsUserPraise>", listArr.ToArray());

                    listArr.Clear();

                    Dictionary<string, string> dic = null;
                    if (!string.IsNullOrWhiteSpace(model.FileDirectory) && !string.IsNullOrWhiteSpace(model.RandomFolder) && !string.IsNullOrWhiteSpace(model.FileExtension))
                    {
                        EnumData.Platform platform = EnumData.Platform.Android;
                        dic = PictureUrlHelper.GetUrlByPlatform(model.FileDirectory, model.RandomFolder, model.FileExtension, platform);
                    }
                    listArr.Add(dic == null ? "" : WebSiteHost + dic["OriginalPicture"]);
                    listArr.Add(dic == null ? "" : WebSiteHost + dic["BPicture"]);
                    listArr.Add(dic == null ? "" : WebSiteHost + dic["MPicture"]);
                    listArr.Add(dic == null ? "" : WebSiteHost + dic["SPicture"]);
                    sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", listArr.ToArray());

                    sb.Append("</N>");
                }
                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                new CustomException(string.Format("服务-接口：string GetServiceList,异常：{0}", ex.Message), ex);
                return "";
            }
        }

        public string GetServiceListByServiceItemId(string username, int pageIndex, int pageSize, Guid serviceItemId)
        {
            try
            {
                object userId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(username))
                {
                    MembershipUser user = Membership.GetUser(username);
                    if (user != null)
                    {
                        userId = user.ProviderUserKey;
                    }
                }

                int totalRecords = 0;
                ServiceUnion suBll = new ServiceUnion();

                StringBuilder sb = new StringBuilder(3000);
                sb.Append("<Rsp>");

                var list = suBll.GetListByService(userId, pageIndex, pageSize, out totalRecords, serviceItemId);
                if (list == null || list.Count == 0) return "";

                IList<object> listArr = new List<object>();
                foreach (var suModel in list)
                {
                    sb.Append("<N>");

                    listArr.Clear();
                    listArr.Add(suModel.Id);
                    listArr.Add(suModel.Named);
                    listArr.Add(suModel.Descr);
                    listArr.Add(suModel.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm"));
                    listArr.Add(suModel.Flag);
                    listArr.Add(suModel.TotalPraise);
                    listArr.Add(suModel.TotalVole);
                    sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><Descr>{2}</Descr><LastUpdatedDate>{3}</LastUpdatedDate><Flag>{4}</Flag><TotalPraise>{5}</TotalPraise><TotalVole>{6}</TotalVole>", listArr.ToArray());

                    listArr.Clear();
                    Dictionary<string, string> dic = null;
                    if (!string.IsNullOrWhiteSpace(suModel.FileDirectory) && !string.IsNullOrWhiteSpace(suModel.RandomFolder) && !string.IsNullOrWhiteSpace(suModel.FileExtension))
                    {
                        EnumData.Platform platform = EnumData.Platform.Android;
                        dic = PictureUrlHelper.GetUrlByPlatform(suModel.FileDirectory, suModel.RandomFolder, suModel.FileExtension, platform);
                    }
                    listArr.Add(dic == null ? "" : WebSiteHost + dic["OriginalPicture"]);
                    listArr.Add(dic == null ? "" : WebSiteHost + dic["BPicture"]);
                    listArr.Add(dic == null ? "" : WebSiteHost + dic["MPicture"]);
                    listArr.Add(dic == null ? "" : WebSiteHost + dic["SPicture"]);
                    sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", listArr.ToArray());

                    sb.Append("</N>");
                }

                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                new CustomException(string.Format("服务-接口：string GetServiceListByServiceItemId,异常：{0}", ex.Message), ex);
                return "";
            }
        }

        public string GetServiceVoteById(Guid Id)
        {
            try
            {
                if (Id.Equals(Guid.Empty)) return "";

                ServiceVote svBll = new ServiceVote();
                var model = svBll.GetModelByJoin(Id);
                if (model == null) return "";

                Regex r = new Regex("(<img)(.*)src=\"([^\"]*?)\"(.*)/>");
                model.ContentText = r.Replace(model.ContentText, "$1$2src=\"" + WebSiteHost + "$3\" />");

                StringBuilder sb = new StringBuilder(500);
                sb.Append("<Rsp>");

                Dictionary<string, string> dic = null;
                if (!string.IsNullOrWhiteSpace(model.FileDirectory) && !string.IsNullOrWhiteSpace(model.RandomFolder) && !string.IsNullOrWhiteSpace(model.FileExtension))
                {
                    EnumData.Platform platform = EnumData.Platform.Android;
                    dic = PictureUrlHelper.GetUrlByPlatform(model.FileDirectory, model.RandomFolder, model.FileExtension, platform);
                }

                sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><Descr>{2}</Descr><Content><![CDATA[{3}]]></Content><LastUpdatedDate>{4}</LastUpdatedDate>", model.Id, model.Named, model.Descr, model.ContentText, model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm"));
                sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", dic == null ? "" : WebSiteHost + dic["OriginalPicture"], dic == null ? "" : WebSiteHost + dic["BPicture"], dic == null ? "" : WebSiteHost + dic["MPicture"], dic == null ? "" : WebSiteHost + dic["SPicture"]);

                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                new CustomException(string.Format("服务-接口：string GetServiceVoteById,异常：{0}", ex.Message), ex);
                return "";
            }
        }

        public string GetServiceContentById(Guid Id)
        {
            try
            {
                if (Id.Equals(Guid.Empty)) return "";

                ServiceContent scBll = new ServiceContent();
                var model = scBll.GetModelByJoin(Id);
                if (model == null) return "";

                Regex r = new Regex("(<img)(.*)src=\"([^\"]*?)\"(.*)/>");
                model.ContentText = r.Replace(model.ContentText, "$1$2src=\"" + WebSiteHost + "$3\" />");

                StringBuilder sb = new StringBuilder(3000);
                sb.Append("<Rsp>");

                IList<object> listArr = new List<object>();
                Dictionary<string, string> dic = null;
                if (!string.IsNullOrWhiteSpace(model.FileDirectory) && !string.IsNullOrWhiteSpace(model.RandomFolder) && !string.IsNullOrWhiteSpace(model.FileExtension))
                {
                    EnumData.Platform platform = EnumData.Platform.Android;
                    dic = PictureUrlHelper.GetUrlByPlatform(model.FileDirectory, model.RandomFolder, model.FileExtension, platform);
                }
                listArr.Add(dic == null ? "" : WebSiteHost + dic["OriginalPicture"]);
                listArr.Add(dic == null ? "" : WebSiteHost + dic["BPicture"]);
                listArr.Add(dic == null ? "" : WebSiteHost + dic["MPicture"]);
                listArr.Add(dic == null ? "" : WebSiteHost + dic["SPicture"]);
                sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><Descr>{2}</Descr><Content><![CDATA[{3}]]></Content><LastUpdatedDate>{4}</LastUpdatedDate>", model.Id, model.Named, model.Descr, model.ContentText, model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm"));
                sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", listArr.ToArray());

                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                new CustomException(string.Format("服务-接口：string GetServiceContentById,异常：{0}", ex.Message), ex);
                return "";
            }
        }

        public string GetServiceLinkById(Guid Id)
        {
            try
            {
                if (Id.Equals(Guid.Empty)) return "";

                ServiceLink scBll = new ServiceLink();
                var model = scBll.GetModelByJoin(Id);
                if (model == null) return "";

                IList<object> listArr = new List<object>();
                Dictionary<string, string> dic = null;
                if (!string.IsNullOrWhiteSpace(model.FileDirectory) && !string.IsNullOrWhiteSpace(model.RandomFolder) && !string.IsNullOrWhiteSpace(model.FileExtension))
                {
                    EnumData.Platform platform = EnumData.Platform.Android;
                    dic = PictureUrlHelper.GetUrlByPlatform(model.FileDirectory, model.RandomFolder, model.FileExtension, platform);
                }
                listArr.Add(dic == null ? "" : WebSiteHost + dic["OriginalPicture"]);
                listArr.Add(dic == null ? "" : WebSiteHost + dic["BPicture"]);
                listArr.Add(dic == null ? "" : WebSiteHost + dic["MPicture"]);
                listArr.Add(dic == null ? "" : WebSiteHost + dic["SPicture"]);

                StringBuilder sb = new StringBuilder(3000);
                sb.Append("<Rsp>");

                sb.AppendFormat("<Id>{0}</Id><Name>{1}</Name><Url>{2}</Url><LastUpdatedDate>{3}</LastUpdatedDate>", model.Id, model.Named, model.Url, model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm"));
                sb.AppendFormat("<OriginalPicture>{0}</OriginalPicture><BPicture>{1}</BPicture><MPicture>{2}</MPicture><SPicture>{3}</SPicture>", listArr.ToArray());

                sb.Append("</Rsp>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                new CustomException(string.Format("服务-接口：string GetServiceLinkById,异常：{0}", ex.Message), ex);
                return "";
            }
        }

        public string SaveServiceUserVole(string username, Guid serviceItemId)
        {
            try
            {
                if (serviceItemId.Equals(Guid.Empty))
                {
                    return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", false, "服务ID【" + serviceItemId + "】无效，请检查");
                }
                MembershipUser user = Membership.GetUser(username);
                if (user == null)
                {
                    return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", false, "用户【" + username + "】不存在或已被删除");
                }
                object userId = user.ProviderUserKey;

                ServiceUserVole bll = new ServiceUserVole();
                ServiceUserVoleInfo model = new ServiceUserVoleInfo();
                model.UserId = Guid.Parse(userId.ToString());
                model.ServiceItemId = serviceItemId;
                model.LastUpdatedDate = DateTime.Now;

                int effect = 0;

                using (TransactionScope scope = new TransactionScope())
                {
                    if (bll.IsExist(userId, serviceItemId)) effect = 110;
                    else
                    {
                        effect = bll.Insert(model);
                    }
                    scope.Complete();
                }

                if (effect == 110)
                {
                    return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", false, "您已投票，请不要重复投票！");
                }

                if (effect > 0)
                {
                    return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", true, "投票成功");
                }
                else
                {
                    return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", false, "投票失败，请正确操作，必要时请联系管理员");
                }

            }
            catch (Exception ex)
            {
                new CustomException(string.Format("服务-接口：string SaveServiceUserVole,异常：{0}", ex.Message), ex);
                return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", false, "系统异常，原因：" + ex.Message + "");
            }
        }

        public string SaveServiceUserPraise(string username, Guid serviceItemId)
        {
            try
            {
                if (serviceItemId.Equals(Guid.Empty))
                {
                    return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", false, "服务ID【" + serviceItemId + "】无效，请检查");
                }
                MembershipUser user = Membership.GetUser(username);
                if (user == null)
                {
                    return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", false, "用户【" + username + "】不存在或已被删除");
                }
                object userId = user.ProviderUserKey;

                ServiceUserPraise bll = new ServiceUserPraise();
                ServiceUserPraiseInfo model = new ServiceUserPraiseInfo();
                model.UserId = Guid.Parse(userId.ToString());
                model.ServiceItemId = serviceItemId;
                model.LastUpdatedDate = DateTime.Now;

                int effect = 0;

                using (TransactionScope scope = new TransactionScope())
                {
                    if (bll.IsExist(userId, serviceItemId)) effect = 110;
                    else
                    {
                        effect = bll.Insert(model);
                    }
                    scope.Complete();
                }

                if (effect == 110)
                {
                    return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg><HasPraise>{2}</HasPraise></Rsp>", false, "您已赞过，请不要重复操作！", true);
                }

                if (effect > 0)
                {
                    return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", true, "点赞成功");
                }
                else
                {
                    return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", false, "点赞失败，请正确操作，必要时请联系管理员");
                }

            }
            catch (Exception ex)
            {
                new CustomException(string.Format("服务-接口：string SaveServiceUserPraise,异常：{0}", ex.Message), ex);
                return string.Format("<Rsp><IsOK>{0}</IsOK><RtMsg>{1}</RtMsg></Rsp>", false, "系统异常，原因：" + ex.Message + "");
            }
        }

        #endregion

        #region IUserSignIn Member

        public string SaveUserSignIn(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username)) return ReturnError("登录标识不能为空字符串，请检查", "");
                var user = Membership.GetUser(username);
                if (user == null) return ReturnError("登录标识无效，请检查", "");
                var userId = user.ProviderUserKey;

                UserSignIn usiBll = new UserSignIn();
                var usiModel = usiBll.GetModelByUser(user.ProviderUserKey);

                HnztcQueueService hqs = new HnztcQueueService();
                XElement root = null;
                DateTime currTime = DateTime.Now;

                #region 如果从未签到过，无记录时，则新增一行记录

                if (usiModel == null)
                {
                    StringBuilder sb = new StringBuilder(1000);
                    var ts = DateTime.Parse(string.Format("{0}-01", currTime.AddMonths(1).ToString("yyyy-MM"))) - DateTime.Parse(string.Format("{0}-01", currTime.ToString("yyyy-MM")));
                    var totalDay = (int)ts.TotalDays;

                    sb.Append("<Root>");
                    sb.AppendFormat(@"<Xel Month=""{0}"">", currTime.ToString("yyyyMM"));
                    for (int i = 0; i < totalDay; i++)
                    {
                        int currGold = ((i + 1) == currTime.Day ? 1 : 0);
                        sb.AppendFormat(@"<Add Day=""{0}"" GoldLevel=""{1}"" />", string.Format("{0}-{1}", currTime.ToString("yyyy-MM"), (i + 1).ToString().PadLeft(2, '0')), currGold);
                    }
                    sb.AppendFormat("</Xel>");
                    sb.Append("</Root>");

                    usiModel = new UserSignInInfo();
                    usiModel.UserId = userId;
                    usiModel.LastUpdatedDate = DateTime.Now;
                    usiModel.SignInXml = sb.ToString();
                    if (usiBll.Insert(usiModel) < 1)
                    {
                        return ReturnError("签到失败，原因：数据连接操作异常，请稍后再重试", "");
                    }

                    hqs.SaveUserLevel(new UserLevelInfo { UserId = Guid.Parse(usiModel.UserId.ToString()), TotalGold = 1 });
                    return ReturnSuccess("签到成功！", 1);
                }

                #endregion

                root = XElement.Parse(usiModel.SignInXml);
                var lastXel = root.Elements("Xel").OrderByDescending(m => int.Parse(m.Attribute("Month").Value)).First();

                #region 如果是新的一年，则新增一行当前年数据

                if (!lastXel.Attribute("Month").Value.StartsWith(currTime.ToString("yyyy")))
                {
                    StringBuilder sb = new StringBuilder(1000);
                    var ts = DateTime.Parse(string.Format("{0}-01", currTime.AddMonths(1).ToString("yyyy-MM"))) - DateTime.Parse(string.Format("{0}-01", currTime.ToString("yyyy-MM")));
                    var totalDay = (int)ts.TotalDays;

                    sb.Append("<Root>");
                    sb.AppendFormat(@"<Xel Month=""{0}"">", currTime.ToString("yyyyMM"));
                    for (int i = 0; i < totalDay; i++)
                    {
                        int currGold = ((i + 1) == currTime.Day ? 1 : 0);
                        sb.AppendFormat(@"<Add Day=""{0}"" GoldLevel=""{1}"" />", string.Format("{0}-{1}", currTime.ToString("yyyy-MM"), (i + 1).ToString().PadLeft(2, '0')), currGold);
                    }
                    sb.AppendFormat("</Xel>");
                    sb.Append("</Root>");

                    usiModel = new UserSignInInfo();
                    usiModel.UserId = userId;
                    usiModel.LastUpdatedDate = DateTime.Now;
                    usiModel.SignInXml = sb.ToString();
                    if (usiBll.Insert(usiModel) < 1)
                    {
                        return ReturnError("签到失败，原因：数据连接操作异常，请稍后再重试", "");
                    }

                    hqs.SaveUserLevel(new UserLevelInfo { UserId = Guid.Parse(usiModel.UserId.ToString()), TotalGold = 1 });
                    return ReturnSuccess("签到成功！", 1);
                }

                #endregion

                var todayXel = lastXel.Descendants("Add").FirstOrDefault(m => m.Attribute("Day").Value.Trim() == currTime.ToString("yyyy-MM-dd"));

                #region 如果没有找到今天记录，则新增当前月的所有节点

                if (todayXel == null)
                {
                    StringBuilder sb = new StringBuilder(1000);
                    var ts = DateTime.Parse(string.Format("{0}-01", currTime.AddMonths(1).ToString("yyyy-MM"))) - DateTime.Parse(string.Format("{0}-01", currTime.ToString("yyyy-MM")));
                    var totalDay = (int)ts.TotalDays;

                    sb.AppendFormat(@"<Xel Month=""{0}"">", currTime.ToString("yyyyMM"));
                    for (int i = 0; i < totalDay; i++)
                    {
                        int currGold = ((i + 1) == currTime.Day ? 1 : 0);
                        sb.AppendFormat(@"<Add Day=""{0}"" GoldLevel=""{1}"" />", string.Format("{0}-{1}", currTime.ToString("yyyy-MM"), (i + 1).ToString().PadLeft(2, '0')), currGold);
                    }
                    sb.AppendFormat("</Xel>");

                    root.Add(sb.ToString());

                    usiModel.LastUpdatedDate = currTime;
                    usiModel.SignInXml = HttpUtility.HtmlDecode(root.ToString());
                    if (usiBll.Update(usiModel) < 1)
                    {
                        return ReturnError("数据库连接操作异常，请稍后再重试！", "");
                    }

                    hqs.SaveUserLevel(new UserLevelInfo { UserId = Guid.Parse(usiModel.UserId.ToString()), TotalGold = 1 });

                    return ReturnSuccess("签到成功！", 1);
                }

                #endregion

                if (int.Parse(todayXel.Attribute("GoldLevel").Value) > 0) return ReturnError("已签到，每天只能签到一次", "");
                int todayGold = 1;

                var prevXel = lastXel.Descendants("Add").First(m => m.Attribute("Day").Value.Trim() == currTime.AddDays(-1).ToString("yyyy-MM-dd"));
                if (int.Parse(prevXel.Attribute("GoldLevel").Value) > 0) todayGold = int.Parse(prevXel.Attribute("GoldLevel").Value) + 1;
                todayXel.SetAttributeValue("GoldLevel", todayGold);

                usiModel.LastUpdatedDate = currTime;
                usiModel.SignInXml = HttpUtility.HtmlDecode(root.ToString());
                if (usiBll.Update(usiModel) < 1)
                {
                    return ReturnError("数据库连接操作异常，请稍后再重试！", "");
                }

                hqs.SaveUserLevel(new UserLevelInfo { UserId = Guid.Parse(usiModel.UserId.ToString()), TotalGold = todayGold });

                return ReturnSuccess("签到成功！", todayGold);
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return ReturnError(ex.Message, "");
            }
        }

        public string GetUserSignInByMonth(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username)) return ReturnError("登录标识不能为空字符串，请检查", "");
                var user = Membership.GetUser(username);
                if (user == null) return ReturnError("登录标识无效，请检查", "");
                var userId = user.ProviderUserKey;

                UserSignIn usiBll = new UserSignIn();
                var usiModel = usiBll.GetModelByUser(user.ProviderUserKey);
                DateTime currTime = DateTime.Now;

                if (usiModel == null) return GetUserSignInByMonthForNull();

                XElement root = XElement.Parse(usiModel.SignInXml);
                var currMonthXel = root.Elements("Xel").FirstOrDefault(x => x.Attribute("Month").Value == currTime.ToString("yyyyMM"));
                if (currMonthXel == null) return GetUserSignInByMonthForNull();
                currMonthXel.SetAttributeValue("Month", currTime.ToString("yyyy-MM-dd"));

                return ReturnSuccess("", "<Root>" + currMonthXel.ToString() + "</Root>");
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return ReturnError(ex.Message, "");
            }
        }

        private string GetUserSignInByMonthForNull()
        {
            DateTime currTime = DateTime.Now;
            StringBuilder sb = new StringBuilder(1000);
            var ts = DateTime.Parse(string.Format("{0}-01", currTime.AddMonths(1).ToString("yyyy-MM"))) - DateTime.Parse(string.Format("{0}-01", currTime.ToString("yyyy-MM")));
            var totalDay = (int)ts.TotalDays;

            sb.Append("<Root>");
            sb.AppendFormat(@"<Xel Month=""{0}"">", currTime.ToString("yyyy-MM-dd"));
            for (int i = 0; i < totalDay; i++)
            {
                sb.AppendFormat(@"<Add Day=""{0}"" GoldLevel=""{1}"" />", string.Format("{0}-{1}", currTime.ToString("yyyy-MM"), (i + 1).ToString().PadLeft(2, '0')), 0);
            }
            sb.AppendFormat("</Xel>");
            sb.Append("</Root>");

            return ReturnSuccess("", sb.ToString());
        }

        #endregion

        #region 金币 元宝 积分 相关

        /// <summary>
        /// 系统赠送，暂行方案：1个金币/人/天
        /// </summary>
        /// <param name="username"></param>
        /// <param name="funName"></param>
        /// <returns></returns>
        public string SaveUserLevelByEnumSource(string username,string funName)
        {
            if (string.IsNullOrWhiteSpace(username)) return ReturnResult(false, "登录标识不能为空字符串，请检查", null);

            try
            {
                var user = Membership.GetUser(username);
                if (user == null) return ReturnResult(false, "登录标识无效，请检查", null);
                var userId = user.ProviderUserKey;

                EnumHelper eh = new EnumHelper();
                var funCode = eh.GetValue(typeof(EnumData.FunCode), funName, 0);
                if (funCode < 1) return ReturnResult(false, "参数funName值【" + funName + "】无效，请检查", null);
                var enumSource = (int)EnumData.UserLevelSource.Encourage;

                UserLevelProduce ulpBll = new UserLevelProduce();
                if (ulpBll.IsExist(userId, funCode, enumSource))
                {
                    return ReturnResult(true, "调用成功", 0);
                }

                //金币、元宝、颜色、积分、等级队列服务
                UserBaseQueueClient ubQueueClient = new UserBaseQueueClient();
                TygaSoft.Services.HnztcQueueService.UserLevelInfo userLevelInfo = new Services.HnztcQueueService.UserLevelInfo();
                userLevelInfo.UserId = Guid.Parse(userId.ToString());
                userLevelInfo.FunCode = funCode;
                userLevelInfo.EnumSource = (int)EnumData.UserLevelSource.Encourage;
                userLevelInfo.TotalGold = 1;
                ubQueueClient.SaveUserLevel(userLevelInfo);

                return ReturnResult(true, "调用成功", 1);
            }
            catch (Exception ex)
            {
                new CustomException("SaveUserLevelByEnumSource(string username,string funName)", ex);
                return ReturnResult(false, ex.Message, 0);
            }
        }

        public string GetUserLevelByEnumSource(string username, string funName)
        {
            if (string.IsNullOrWhiteSpace(username)) return ReturnResult(false,"登录标识不能为空字符串，请检查", null);
            var user = Membership.GetUser(username);
            if (user == null) return ReturnResult(false, "登录标识无效，请检查", null);
            var userId = user.ProviderUserKey;

            EnumHelper eh = new EnumHelper();
            var funCode = eh.GetValue(typeof(EnumData.FunCode), funName, 0);
            if (funCode < 1) return ReturnResult(false, "参数funName值【" + funName + "】无效，请检查", null);
            var enumSource = (int)EnumData.UserLevelSource.Encourage;

            UserLevelView ulvBll = new UserLevelView();
            var model = ulvBll.GetModel(userId, funCode, enumSource);
            if (model == null)
            {
                Thread.Sleep(5000);
                model = ulvBll.GetModel(userId, funCode, enumSource);
                if(model == null) return ReturnResult(false, "系统繁忙，请稍后...", null);
            }

            return ReturnResult(true, "调用成功", model.TotalGold);
        }

        #endregion

        #region About Site

        public string GetAboutUs()
        {
            var bll = new ContentDetail();
            var model = bll.GetModelByTypeCode(EnumData.AboutSite.AboutUs.ToString());
            if (model == null) return string.Empty;
            Regex r = new Regex("(<img)(.*)src=\"([^\"]*?)\"(.*)/>");

            StringBuilder sb = new StringBuilder();
            sb.Append("<Rsp>");
            sb.AppendFormat(@"<Title>{0}</Title><Descr>{1}</Descr><Content><![CDATA[{2}]]></Content>",
                model.Title, model.Descr, r.Replace(model.ContentText, "$1$2src=\"" + WebSiteHost + "$3\" />"));
            sb.Append("</Rsp>");

            return sb.ToString();
        }

        #endregion

        #endregion

        #region 辅助方法

        private string ReturnResult(bool isOk, string msg, params object[] data)
        {
            return string.Format("<Rsp><IsOk>{0}</IsOk><RtMsg>{1}</RtMsg><Data>{2}</Data></Rsp>", isOk, msg, data == null ? "" : data[0]);
        }

        private string ReturnError(string msg, object data)
        {
            StringBuilder result = new StringBuilder(300);
            result.AppendFormat("<Rsp><IsOk>{0}</IsOk><Msg>{1}</Msg><Data>{2}</Data></Rsp>", false, msg, data);
            return result.ToString();
        }

        private string ReturnSuccess(string msg, object data)
        {
            StringBuilder result = new StringBuilder(300);
            result.AppendFormat("<Rsp><IsOk>{0}</IsOk><Msg>{1}</Msg><Data>{2}</Data></Rsp>", true, msg, data);
            return result.ToString();
        }

        #endregion
    }
}
