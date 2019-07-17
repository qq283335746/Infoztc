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
    public partial class QuestionSubject
    {
        #region IQuestionSubject Member
        public QuestionSubjectInfo GetModelOW(object Id)
        {
            QuestionSubjectInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 S.Id,S.QuestionBankId,S.QuestionContent,S.QuestionType,S.Sort,S.Remark,S.IsDisable,S.LastUpdatedDate,B.Named
			                   from QuestionSubject S, QuestionBank B
							   where S.QuestionBankId = B.Id and Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new QuestionSubjectInfo();
                        model.Id = reader.GetGuid(0);
                        model.QuestionBankId = reader.GetGuid(1);
                        model.QuestionContent = reader.GetString(2);
                        model.QuestionType = reader.GetInt32(3);
                        model.Sort = reader.GetInt32(4);
                        model.Remark = reader.GetString(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                        model.QuestionBankName = reader.GetString(8);
                    }
                }
            }

            return model;
        }

        public IList<QuestionSubjectInfo> GetListOW(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from QuestionSubject ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<QuestionSubjectInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by S.LastUpdatedDate desc) as RowNumber,
			            S.Id,S.QuestionBankId,S.QuestionContent,S.QuestionType,S.Sort,S.Remark,S.LastUpdatedDate,B.Named
			            from QuestionSubject S, QuestionBank B ");
            sb.AppendFormat(" where S.QuestionBankId = B.Id {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<QuestionSubjectInfo> list = new List<QuestionSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuestionSubjectInfo model = new QuestionSubjectInfo();
                        model.Id = reader.GetGuid(1);
                        model.QuestionBankId = reader.GetGuid(2);
                        model.QuestionContent = reader.GetString(3);
                        model.QuestionType = reader.GetInt32(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                        model.QuestionBankName = reader.GetString(8);
                        model.QuestionTypeName = model.QuestionType == 0 ? "单选题" : "多选题";
                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<QuestionSubjectInfo> GetListOW(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			            S.Id,S.QuestionBankId,S.QuestionContent,S.QuestionType,S.Sort,S.Remark,S.LastUpdatedDate,B.Named
			            from QuestionSubject S, QuestionBank B ");
            sb.AppendFormat(" where S.QuestionBankId = B.Id {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<QuestionSubjectInfo> list = new List<QuestionSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuestionSubjectInfo model = new QuestionSubjectInfo();
                        model.Id = reader.GetGuid(1);
                        model.QuestionBankId = reader.GetGuid(2);
                        model.QuestionContent = reader.GetString(3);
                        model.QuestionType = reader.GetInt32(4);
                        model.Sort = reader.GetInt32(5);
                        model.Remark = reader.GetString(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                        model.QuestionBankName = reader.GetString(8);
                        model.QuestionTypeName = model.QuestionType == 0 ? "单选题" : "多选题";
                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<QuestionSubjectInfo> GetRandomList(int pageSize, Guid questionBankId)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.AppendFormat(@"select top {0} Id,QuestionBankId,QuestionContent,QuestionType,Sort,Remark,IsDisable,LastUpdatedDate
                        from QuestionSubject where QuestionBankId=@QuestionBankId and IsDisable=0 order by NEWID()", pageSize);

            SqlParameter[] parms = {
                                       new SqlParameter("@QuestionBankId",SqlDbType.UniqueIdentifier),
                                   };
            parms[0].Value = questionBankId;

            IList<QuestionSubjectInfo> list = new List<QuestionSubjectInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcTeamDbConnString, CommandType.Text, sb.ToString(), parms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuestionSubjectInfo model = new QuestionSubjectInfo();
                        model.Id = reader.GetGuid(0);
                        model.QuestionBankId = reader.GetGuid(1);
                        model.QuestionContent = reader.GetString(2);
                        model.QuestionType = reader.GetInt32(3);
                        model.Sort = reader.GetInt32(4);
                        model.Remark = reader.GetString(5);
                        model.IsDisable = reader.GetBoolean(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }
        #endregion
    }
}
