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
        /// ��ȡ���㵱ǰ�����������б�
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
