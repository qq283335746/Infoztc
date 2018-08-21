using System.Data;
using TygaSoft.Model;

namespace TygaSoft.BLL
{
    public partial class InformationPicture
    {
        #region InformationPicture Member

        public int InsertModel(InformationPictureInfo model)
        {
            return dal.InsertModel(model);
        }

        /// <summary>
        /// 获取满足当前条件的数据列表
        /// </summary>
        /// <param name="InformationId"></param>
        /// <returns></returns>
        public DataSet GetListOW(object InformationId)
        {
            return dal.GetListOW(InformationId);
        }

        #endregion
    }
}
