using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TygaSoft.WcfService
{
    [ServiceContract(Namespace = "TygaSoft.Services.ECShopService")]
    public partial interface IECShop
    {
        #region ICategory Member

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetCategoryModel")]
        string GetCategoryModel(Guid Id);

        /// <summary>
        /// 获取分类树数据
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetCategoryTreeJson")]
        string GetCategoryTreeJson();

        #endregion

        #region IBrand Member

        /// <summary>
        /// 获取品牌树数据
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetBrandTreeJson")]
        string GetBrandTreeJson();

        /// <summary>
        /// 获取当前品牌下的所有品牌
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetBrandListByParentId")]
        string GetBrandListByParentId(Guid parentId);

        /// <summary>
        /// 获取当前分类下的所有品牌
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetBrandListByCategoryId")]
        string GetBrandListByCategoryId(Guid categoryId);

        #endregion

        #region IProduct Member

        /// <summary>
        /// 获取分页数据列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetProductListByPage")]
        string GetProductList(int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取当前分类的商品分页数据列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetProductListByCategory")]
        string GetProductListByCategory(int pageIndex, int pageSize, out int totalRecords, Guid categoryId);

        /// <summary>
        /// 获取当前品牌的商品分页数据列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetProductListByBrand")]
        string GetProductListByBrand(int pageIndex, int pageSize, out int totalRecords, Guid brandId);

        /// <summary>
        /// 获取当前菜单功能点的商品分页数据列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetProductListByMenu")]
        string GetProductListByMenu(int pageIndex, int pageSize, out int totalRecords, Guid menuId);

        /// <summary>
        /// 获取商品明细
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetProductDetail")]
        string GetProductDetail(Guid productId);

        #endregion
    }
}
