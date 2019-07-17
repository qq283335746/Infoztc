using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TygaSoft.WcfService
{
    [ServiceContract(Namespace = "TygaSoft.Services.HnztcService")]
    public partial interface IHnztc
    {
        #region HnztcDb

        #region IAdvertisement Member

        /// <summary>
        /// 获取广告区分页数据列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetSiteFunList")]
        string GetSiteFunList();

        /// <summary>
        /// 获取广告分页数据列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="siteFunId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetAdvertisementList")]
        string GetAdvertisementList(int pageIndex, int pageSize, Guid siteFunId);

        /// <summary>
        /// 获取当前广告详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetAdvertisementModel")]
        string GetAdvertisementModel(Guid Id);

        #endregion

        #region IAnnouncement Member

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetAnnouncementList")]
        string GetAnnouncementList(int pageIndex, int pageSize);

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetAnnouncementModel")]
        string GetAnnouncementModel(Guid Id);

        #endregion

        #region INotice Member

        /// <summary>
        /// 获取分页数据列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetNoticeListByPage")]
        string GetNoticeList(int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetNoticeModel")]
        string GetNoticeModel(Guid Id);

        #endregion

        #region IContentType Member

        /// <summary>
        /// 获取商城菜单一级层级数据列表
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetShopMenuListByRoot")]
        string GetShopMenuListByRoot();

        /// <summary>
        /// 获取当前菜单下的所有子项
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetContentTypeChildListByParentId")]
        string GetContentTypeChildListByParentId(Guid parentId);

        #endregion

        #region IProvinceCity Member

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetProvinceCityModel")]
        string GetProvinceCityModel(Guid Id);

        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetProvince")]
        string GetProvince();

        /// <summary>
        /// 获取市列表
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetCity")]
        string GetCity(Guid provinceId);

        /// <summary>
        /// 获取区列表
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetDistrict")]
        string GetDistrict(Guid cityId);

        #endregion

        #region IServiceItem Member

        /// <summary>
        /// 获取服务分类分页数据列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetServiceList")]
        string GetServiceList(string username, int pageIndex, int pageSize);

        /// <summary>
        /// 获取属于当前服务分类的所有子项分页数据列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="serviceItemId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetServiceListByServiceItemId")]
        string GetServiceListByServiceItemId(string username, int pageIndex, int pageSize, Guid serviceItemId);

        /// <summary>
        /// 获取当前服务投票详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetServiceVoteById")]
        string GetServiceVoteById(Guid Id);

        /// <summary>
        /// 获取当前服务图文详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetServiceContentById")]
        string GetServiceContentById(Guid Id);

        /// <summary>
        /// 获取当前服务链接详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetServiceLinkById")]
        string GetServiceLinkById(Guid Id);

        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="serviceItemId"></param>
        /// <returns></returns>
        [OperationContract(Name = "SaveServiceUserVole")]
        string SaveServiceUserVole(string username, Guid serviceItemId);

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="serviceItemId"></param>
        /// <returns></returns>
        [OperationContract(Name = "SaveServiceUserPraise")]
        string SaveServiceUserPraise(string username, Guid serviceItemId);


        #endregion

        #region IUserSignIn Member

        /// <summary>
        /// 每日签到
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [OperationContract(Name = "SaveUserSignIn")]
        string SaveUserSignIn(string username);

        /// <summary>
        /// 获取用户当前月的签到列表
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetUserSignInByMonth")]
        string GetUserSignInByMonth(string username);

        #endregion

        #region 金币 元宝 积分 相关

        [OperationContract(Name = "SaveUserLevelByEnumSource")]
        string SaveUserLevelByEnumSource(string username, string funName);

        [OperationContract(Name = "GetUserLevelByEnumSource")]
        string GetUserLevelByEnumSource(string username, string funName);

        #endregion

        #region About Site

        [OperationContract(Name = "GetAboutUs")]
        string GetAboutUs();

        #endregion

        #endregion
    }
}
