using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class Product
    {
        #region Product Member

        public Guid InsertByOutput(ProductInfo model)
        {
            return dal.InsertByOutput(model);
        }

        public ProductInfo GetModelByJoin(object Id)
        {
            return dal.GetModelByJoin(Id);
        }

        public DataSet GetProductListByMenu(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetProductListByMenu(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        public DataSet GetListByAdmin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListByAdmin(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        public IList<ProductInfo> GetListByCategory(int pageIndex, int pageSize, out int totalRecords, Guid categoryId)
        {
            string sqlWhere = @"and cp.CategoryId = @CategoryId ";
            SqlParameter parm = new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier);
            parm.Value = categoryId;

            return dal.GetListByCategory(pageIndex, pageSize, out totalRecords, sqlWhere, parm);
        }

        public IList<ProductInfo> GetListByBrand(int pageIndex, int pageSize, out int totalRecords, Guid brandId)
        {
            string sqlWhere = @"and bp.BrandId = @BrandId ";
            SqlParameter parm = new SqlParameter("@BrandId", SqlDbType.UniqueIdentifier);
            parm.Value = brandId;

            return dal.GetListByBrand(pageIndex, pageSize, out totalRecords, sqlWhere, parm);
        }

        public DataSet GetListByMenu(int pageIndex, int pageSize, out int totalRecords, Guid menuId)
        {
            string sqlWhere = @"and mp.MenuId = @MenuId ";
            SqlParameter parm = new SqlParameter("@MenuId", SqlDbType.UniqueIdentifier);
            parm.Value = menuId;

            return dal.GetProductListByMenu(pageIndex, pageSize, out totalRecords, sqlWhere, parm);
        }

        #endregion
    }
}
