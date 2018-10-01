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
    public partial class InformationAdPicture
    {
        #region InformationPicture Member

        public int InsertModel(InformationAdPictureInfo model)
        {
            return dal.InsertModel(model);
        }

        /// <summary>
        /// ��ȡ���㵱ǰ�����������б�
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public DataSet GetListOW(object playerId)
        {
            return dal.GetListOW(playerId);
        }

        #endregion
    }
}
