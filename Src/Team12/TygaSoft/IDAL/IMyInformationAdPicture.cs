using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IInformationAdPicture
    {
        #region IInformationAdPicture Member

        int InsertModel(InformationAdPictureInfo model);

        /// <summary>
        /// ��ȡ���㵱ǰ�����������б�
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        DataSet GetListOW(object Id);

        #endregion
    }
}
