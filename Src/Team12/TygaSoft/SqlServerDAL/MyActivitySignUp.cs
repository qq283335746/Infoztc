using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DBUtility;

namespace TygaSoft.SqlServerDAL
{
    public partial class ActivitySignUp : IActivitySignUp
    {
        #region ±¨Ãû
        public DataSet GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ActivitySignUp AS asu ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by asu.LastUpdatedDate desc) as RowNumber,
			         as1.Title, tbUSER.UserName, tbUSER.Sex, tbUSER.MobilePhone, tbUSER.Nickname,asu.LastUpdatedDate
					  FROM ActivitySignUp AS asu 
					LEFT JOIN  ActivitySubject AS as1 ON as1.Id = asu.ActivityId
					LEFT JOIN (select U.UserId,U.UserName,UB.Nickname,UB.HeadPicture,UB.Sex,UB.MobilePhone 
											from HnztcAspnetDb.dbo.aspnet_Users U 
					left join HnztcSystemDb.dbo.UserBase UB on U.UserId=UB.UserId) AS tbUSER ON tbUSER.UserId = asu.UserId");
            sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public DataSet ExportExcel(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);

            sb.Append(@"SELECT as1.Title, tbUSER.UserName, tbUSER.Sex, tbUSER.MobilePhone, tbUSER.Nickname,asu.LastUpdatedDate
                          FROM ActivitySignUp AS asu 
                        LEFT JOIN  ActivitySubject AS as1 ON as1.Id = asu.ActivityId
                        LEFT JOIN (select U.UserId,U.UserName,UB.Nickname,UB.HeadPicture,UB.Sex,UB.MobilePhone 
                                                from HnztcAspnetDb.dbo.aspnet_Users U 
            left join HnztcSystemDb.dbo.UserBase UB on U.UserId=UB.UserId) AS tbUSER ON tbUSER.UserId = asu.UserId");
            sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            return ds;
        }

        public bool IsAlreadySignUp(string UserId, string ActivityId)
        {
            StringBuilder sb = new StringBuilder(250);
            bool bl = false;
            ParamsHelper parms = new ParamsHelper();
            string sqlWhere = " AND asu.ActivityId = @ActivityId AND asu.UserId=@UserId ";
            SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ActivityId);
            parms.Add(parm);
            parm = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(UserId);
            parms.Add(parm);

            sb.Append(@"SELECT as1.Title, tbUSER.UserName, tbUSER.Sex, tbUSER.MobilePhone, tbUSER.Nickname,asu.LastUpdatedDate
                          FROM ActivitySignUp AS asu 
                        LEFT JOIN  ActivitySubject AS as1 ON as1.Id = asu.ActivityId
                        LEFT JOIN (select U.UserId,U.UserName,UB.Nickname,UB.HeadPicture,UB.Sex,UB.MobilePhone 
                                                from HnztcAspnetDb.dbo.aspnet_Users U 
            left join HnztcSystemDb.dbo.UserBase UB on U.UserId=UB.UserId) AS tbUSER ON tbUSER.UserId = asu.UserId");
            sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms.ToArray());

            if(ds!=null&&ds.Tables[0].Rows.Count>0)
            {
                bl = true;
            }

            return bl;
        }

        public bool IsNotAtFull(string ActivityId)
        {
            StringBuilder sb = new StringBuilder(250);
            bool bl = false;
            ParamsHelper parms = new ParamsHelper();
            string sqlWhere = " AND as1.Id = @ActivityId ";
            SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ActivityId);
            parms.Add(parm);

            sb.Append(@"SELECT CASE WHEN as1.MaxSignUpCount>as1.ActualSignUpCount THEN 0 ELSE 1 END AtFull
                  FROM ActivitySubject AS as1 ");
            sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms.ToArray());

            if(ds!=null&&ds.Tables[0].Rows.Count>0)
            {
                if (ds.Tables[0].Rows[0][0] == "1")
                {
                    bl = false;
                }
                else
                {
                    bl = true;
                }
            }

            return bl;
        }

        public int SignUpCount(string ActivityId)
        {
            StringBuilder sb = new StringBuilder(250);
            ParamsHelper parms = new ParamsHelper();
            string sqlWhere = " AND as1.Id = @ActivityId ";
            SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(ActivityId);
            parms.Add(parm);

            sb.Append(@"SELECT as1.UpdateSignUpCount FROM ActivitySubject AS as1 ");
            sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms.ToArray());

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()))
                {
                    return int.Parse(ds.Tables[0].Rows[0][0].ToString());
                }

            }

            return 0;
        }
        #endregion
    }
}
