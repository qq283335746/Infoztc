using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class OrderRandom
    {
        private static readonly IOrderRandom dal = DataAccess.CreateOrderRandom();
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        #region OrderRandom Member

        public string GetOrderCode(string prefix)
        {
            while (true)
            {
                byte[] rndNum = new byte[13];
                rngCsp.GetBytes(rndNum);
                var rnd = new Random(BitConverter.ToInt32(rndNum, 0));

                var orderCode = string.Format("{0}{1}", prefix, (rnd.NextDouble() * int.MaxValue).ToString().PadLeft(10,'0'));
                if (!IsExist(orderCode))
                {
                    var model = new OrderRandomInfo();
                    model.OrderCode = orderCode;
                    model.Prefix = prefix;
                    model.LastUpdatedDate = DateTime.Now;
                    Insert(model);

                    return orderCode;
                }
            }
        }

        public bool IsExist(string orderCode)
        {
            return dal.IsExist(orderCode);
        }

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(OrderRandomInfo model)
        {
            return dal.Insert(model);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(OrderRandomInfo model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public int Delete(object orderCode)
        {
            return dal.Delete(orderCode);
        }

        /// <summary>
        /// 批量删除数据（启用事务）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteBatch(IList<object> list)
        {
            return dal.DeleteBatch(list);
        }

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public OrderRandomInfo GetModel(object orderCode)
        {
            return dal.GetModel(orderCode);
        }

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public IList<OrderRandomInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        /// <summary>
        /// 获取满足当前条件的数据列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public IList<OrderRandomInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetList(pageIndex, pageSize, sqlWhere, cmdParms);
        }

        /// <summary>
        /// 获取满足当前条件的数据列表
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public IList<OrderRandomInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetList(sqlWhere, cmdParms);
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public IList<OrderRandomInfo> GetList()
        {
            return dal.GetList();
        }

        #endregion
    }
}
