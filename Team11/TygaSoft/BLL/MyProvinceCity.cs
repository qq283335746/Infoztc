using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.Model;

namespace TygaSoft.BLL
{
    public partial class ProvinceCity
    {
        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <returns></returns>
        public Dictionary<object, string> GetProvince()
        {
            return dal.GetProvince();
        }

        /// <summary>
        /// 获取当前父节点下的所有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Dictionary<object, string> GetChild(Guid parentId)
        {
            return dal.GetChild(parentId);
        }

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ProvinceCityInfo GetModel(string name)
        {
            return dal.GetModel(name);
        }
    }
}
