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
    public partial class Product
    {
        #region IProduct Member

        public Guid InsertByOutput(ProductInfo model)
        {
            if (IsExist(model.Named, model.UserId, null))
            {
                throw new ArgumentException("已存在相同记录，请不要重复操作");
            }

            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into Product (Named,PictureId,Sort,EnableStartTime,EnableEndTime,IsEnable,IsDisable,UserId,LastUpdatedDate)
			            output inserted.Id values
						(@Named,@PictureId,@Sort,@EnableStartTime,@EnableEndTime,@IsEnable,@IsDisable,@UserId,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Named",SqlDbType.NVarChar,50),
                                        new SqlParameter("@PictureId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@Sort",SqlDbType.Int),
                                        new SqlParameter("@EnableStartTime",SqlDbType.DateTime),
                                        new SqlParameter("@EnableEndTime",SqlDbType.DateTime),
                                        new SqlParameter("@IsEnable",SqlDbType.Bit),
                                        new SqlParameter("@IsDisable",SqlDbType.Bit),
                                        new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Named;
            parms[1].Value = model.PictureId;
            parms[2].Value = model.Sort;
            parms[3].Value = model.EnableStartTime;
            parms[4].Value = model.EnableEndTime;
            parms[5].Value = model.IsEnable;
            parms[6].Value = model.IsDisable;
            parms[7].Value = model.UserId;
            parms[8].Value = model.LastUpdatedDate;

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return Guid.Parse(obj.ToString());

            return Guid.Empty;
        }

        public ProductInfo GetModelByJoin(object Id)
        {
            ProductInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 p.Id,p.Named,p.PictureId,p.Sort,p.EnableStartTime,p.EnableEndTime,p.IsEnable,p.IsDisable,p.UserId,p.LastUpdatedDate
                        ,cp.CategoryId,bp.BrandId,mp.MenuId
                        ,pp.FileDirectory,pp.RandomFolder,pp.FileExtension
			            from Product p 
                        left join Picture_Product pp on pp.Id = p.PictureId
                        left join CategoryProduct cp on cp.ProductId = p.Id
                        left join BrandProduct bp on bp.ProductId = p.Id
                        left join MenuProduct mp on mp.ProductId = p.Id
						where p.Id = @Id 
                     ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ProductInfo();
                        model.Id = reader.GetGuid(0);
                        model.Named = reader.GetString(1);
                        model.PictureId = reader.GetGuid(2);
                        model.Sort = reader.GetInt32(3);
                        model.EnableStartTime = reader.GetDateTime(4);
                        model.EnableEndTime = reader.GetDateTime(5);
                        model.IsEnable = reader.GetBoolean(6);
                        model.IsDisable = reader.GetBoolean(7);
                        model.UserId = reader.GetGuid(8);
                        model.LastUpdatedDate = reader.GetDateTime(9);

                        model.CategoryId = reader.IsDBNull(10) ? Guid.Empty : reader.GetGuid(10);
                        model.BrandId = reader.IsDBNull(11) ? Guid.Empty : reader.GetGuid(11);
                        model.MenuId = reader.IsDBNull(12) ? Guid.Empty : reader.GetGuid(12);

                        model.FileDirectory = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        model.RandomFolder = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        model.FileExtension = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    }
                }
            }

            return model;
        }

        public ProductInfo GetModelByJoinAll(object Id)
        {
            return null;

//            ProductInfo model = null;

//            StringBuilder sb = new StringBuilder(300);
//            sb.Append(@"select top 1 p.Id,ProductName,p.SubTitle,p.ProductPictureId,p.OriginalPrice,p.ProductPrice,p.Discount,p.DiscountDescri,p.StockNum,p.UserId,p.LastUpdatedDate,
//                        pd.OtherPicture,pd.PayOption,pd.ViewCount,pd.ContentText,
//                        cp.CategoryId,bp.BrandId,mp.MenuId
//			            from Product p 
//                        left join ProductDetail pd on pd.ProductId = p.Id
//                        left join CategoryProduct cp on cp.ProductId = p.Id
//                        left join BrandProduct bp on bp.ProductId = p.Id
//                        left join MenuProduct mp on mp.ProductId = p.Id
//						where p.Id = @Id 
//                     ");
//            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
//            parm.Value = Guid.Parse(Id.ToString());

//            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
//            {
//                if (reader != null)
//                {
//                    while (reader.Read())
//                    {
//                        model = new ProductInfo();
//                        model.Id = reader.GetGuid(0);
//                        model.ProductName = reader.GetString(1);
//                        model.SubTitle = reader.GetString(2);
//                        model.ProductPictureId = reader.GetGuid(3);
//                        model.OriginalPrice = reader.GetDecimal(4);
//                        model.ProductPrice = reader.GetDecimal(5);
//                        model.Discount = reader.GetDouble(6);
//                        model.DiscountDescri = reader.GetString(7);
//                        model.StockNum = reader.GetInt32(8);
//                        model.UserId = reader.GetGuid(9);
//                        model.LastUpdatedDate = reader.GetDateTime(10);
//                        model.OtherPicture = reader.GetString(11);
//                        model.PayOption = reader.GetString(12);
//                        model.ViewCount = reader.GetInt32(13);
//                        model.ContentText = reader.GetString(14);
//                        model.CategoryId = reader.IsDBNull(15) ? Guid.Empty : reader.GetGuid(15);
//                        model.BrandId = reader.IsDBNull(16) ? Guid.Empty : reader.GetGuid(16);
//                        model.MenuId = reader.IsDBNull(17) ? Guid.Empty : reader.GetGuid(17);
//                    }
//                }
//            }

//            return model;
        }

        public DataSet GetProductListByMenu(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(1000);
            sb.Append(@"select count(1) from Product p
                        join ProductItem pi on pi.ProductId = p.Id
                        left join ProductPicture pp on pp.Id = pi.PictureId
                        left join MenuProduct mp on mp.ProductId = p.Id
                      ");

            sb.Append(@" where 1=1 and pi.IsDisable = 0 and pi.IsEnable = 0 and (GETDATE() between pi.EnableStartTime and pi.EnableEndTime) ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append(sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);
            if (totalRecords == 0) return new DataSet();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by pi.Sort ) as RowNumber,
			          pi.Id ProductItemId,pi.ProductId,pi.Named ProductItemName,
                      pd.OriginalPrice,pd.ProductPrice,pd.Discount,pd.DiscountDescr,
					  pp.OriginalPicture,pp.BPicture,pp.MPicture,pp.SPicture
                      from Product p 
                      join ProductItem pi on pi.ProductId = p.Id
                      left join ProductDetail pd on pd.ProductItemId = pi.Id
                      left join ProductPicture pp on pp.Id = pi.PictureId
                      left join MenuProduct mp on mp.ProductId = p.Id
                      ");

            sb.Append(@" where 1=1 and pi.IsDisable = 0 and pi.IsEnable = 0 and (GETDATE() between pi.EnableStartTime and pi.EnableEndTime) ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append(sqlWhere);
            sb.Append(" )as objTable where RowNumber between " + startIndex + " and " + endIndex + " ");

            return SqlHelper.ExecuteDataset(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);
        }

        public DataSet GetListByAdmin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*)
                        from Product p
                        left join CategoryProduct cp on cp.ProductId = p.Id
                        left join Category c on cp.CategoryId = c.Id
                        left join BrandProduct bp on bp.ProductId = p.Id
                        left join Brand b on b.Id = bp.BrandId
                        left join MenuProduct mp on mp.ProductId = p.Id 
                        left join HnztcDb.dbo.ContentType ct on mp.MenuId = ct.Id
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return null;

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by p.LastUpdatedDate desc,p.Sort) as RowNumber,
			            p.Id ProductId,p.Named ProductName,p.PictureId ProductPicture,p.Sort ProductSort,
                        p.EnableStartTime ProductEnableStartTime,p.EnableEndTime ProductEnableEndTime,p.IsEnable ProductIsEnable,
                        p.IsDisable ProductIsDisable,p.LastUpdatedDate,c.CategoryName,b.BrandName,ct.TypeValue MenuName
                        ,pp.FileDirectory,pp.RandomFolder,pp.FileExtension
					   from Product p
                       left join Picture_Product pp on pp.Id = p.PictureId
                       left join CategoryProduct cp on cp.ProductId = p.Id
                       left join Category c on cp.CategoryId = c.Id
                       left join BrandProduct bp on bp.ProductId = p.Id
                       left join Brand b on b.Id = bp.BrandId
                       left join MenuProduct mp on mp.ProductId = p.Id 
                       left join HnztcDb.dbo.ContentType ct on mp.MenuId = ct.Id
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            return SqlHelper.ExecuteDataset(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);
        }

        public IList<ProductInfo> GetListByCategory(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            totalRecords = 0;
            return null;

//            StringBuilder sb = new StringBuilder(300);
//            sb.Append(@"select count(1) from Product p
//                             left join ProductPicture pp on pp.Id = p.ProductPictureId
//                             left join CategoryProduct cp on cp.ProductId = p.Id
//                             ");
//            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append(" where 1=1 " + sqlWhere);
//            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

//            if (totalRecords == 0) return new List<ProductInfo>();

//            sb.Clear();
//            int startIndex = (pageIndex - 1) * pageSize + 1;
//            int endIndex = pageIndex * pageSize;

//            sb.Append(@"select * from(select row_number() over(order by p.LastUpdatedDate desc) as RowNumber,
//			          p.Id,p.ProductName,p.SubTitle,p.ProductPictureId,p.OriginalPrice,p.ProductPrice,p.Discount,p.DiscountDescri,p.StockNum,p.UserId,p.LastUpdatedDate,
//					  pp.OriginalPicture,pp.BPicture,pp.MPicture,pp.SPicture
//                      from Product p 
//                      left join ProductPicture pp on pp.Id = p.ProductPictureId
//                      left join CategoryProduct cp on cp.ProductId = p.Id
//                      ");
//            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append("where 1=1 " + sqlWhere);
//            sb.Append(" )as objTable where RowNumber between " + startIndex + " and " + endIndex + " ");

//            List<ProductInfo> list = new List<ProductInfo>();

//            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
//            {
//                if (reader != null && reader.HasRows)
//                {
//                    while (reader.Read())
//                    {
//                        ProductInfo model = new ProductInfo();
//                        model.Id = reader.GetGuid(1);
//                        model.ProductName = reader.GetString(2);
//                        model.SubTitle = reader.GetString(3);
//                        model.ProductPictureId = reader.GetGuid(4);
//                        model.OriginalPrice = reader.GetDecimal(5);
//                        model.ProductPrice = reader.GetDecimal(6);
//                        model.Discount = reader.GetDouble(7);
//                        model.DiscountDescri = reader.GetString(8);
//                        model.StockNum = reader.GetInt32(9);
//                        model.UserId = reader.GetGuid(10);
//                        model.LastUpdatedDate = reader.GetDateTime(11);
//                        model.OriginalPicture = reader.IsDBNull(12) ? "" : reader.GetString(12);
//                        model.BPicture = reader.IsDBNull(13) ? "" : reader.GetString(13);
//                        model.MPicture = reader.IsDBNull(14) ? "" : reader.GetString(14);
//                        model.SPicture = reader.IsDBNull(15) ? "" : reader.GetString(15);

//                        list.Add(model);
//                    }
//                }
//            }

//            return list;
        }

        public IList<ProductInfo> GetListByBrand(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            totalRecords = 0;
            return null;

//            StringBuilder sb = new StringBuilder(300);

//            sb.Append(@"select count(1) from Product p
//                             left join ProductPicture pp on pp.Id = p.ProductPictureId
//                             left join BrandProduct bp on bp.ProductId = p.Id
//                             ");
//            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append(" where 1=1 " + sqlWhere);
//            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

//            if (totalRecords == 0) return new List<ProductInfo>();

//            sb.Clear();
//            int startIndex = (pageIndex - 1) * pageSize + 1;
//            int endIndex = pageIndex * pageSize;

//            sb.Append(@"select * from(select row_number() over(order by p.LastUpdatedDate desc) as RowNumber,
//			          p.Id,p.ProductName,p.SubTitle,p.ProductPictureId,p.OriginalPrice,p.ProductPrice,p.Discount,p.DiscountDescri,p.StockNum,p.UserId,p.LastUpdatedDate,
//					  pp.OriginalPicture,pp.BPicture,pp.MPicture,pp.SPicture
//                      from Product p 
//                      left join ProductPicture pp on pp.Id = p.ProductPictureId
//                      left join BrandProduct bp on bp.ProductId = p.Id
//                      ");
//            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append("where 1=1 " + sqlWhere);
//            sb.Append(" )as objTable where RowNumber between " + startIndex + " and " + endIndex + " ");

//            List<ProductInfo> list = new List<ProductInfo>();

//            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
//            {
//                if (reader != null && reader.HasRows)
//                {
//                    while (reader.Read())
//                    {
//                        ProductInfo model = new ProductInfo();
//                        model.Id = reader.GetGuid(1);
//                        model.ProductName = reader.GetString(2);
//                        model.SubTitle = reader.GetString(3);
//                        model.ProductPictureId = reader.GetGuid(4);
//                        model.OriginalPrice = reader.GetDecimal(5);
//                        model.ProductPrice = reader.GetDecimal(6);
//                        model.Discount = reader.GetDouble(7);
//                        model.DiscountDescri = reader.GetString(8);
//                        model.StockNum = reader.GetInt32(9);
//                        model.UserId = reader.GetGuid(10);
//                        model.LastUpdatedDate = reader.GetDateTime(11);
//                        model.OriginalPicture = reader.IsDBNull(12) ? "" : reader.GetString(12);
//                        model.BPicture = reader.IsDBNull(13) ? "" : reader.GetString(13);
//                        model.MPicture = reader.IsDBNull(14) ? "" : reader.GetString(14);
//                        model.SPicture = reader.IsDBNull(15) ? "" : reader.GetString(15);

//                        list.Add(model);
//                    }
//                }
//            }

//            return list;
        }

        public IList<ProductInfo> GetListByMenu(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            totalRecords = 0;
            return null;

//            StringBuilder sb = new StringBuilder(300);
//            sb.Append(@"select count(1) from Product p
//                             join ProductItem pi on pi.ProductId = p.Id
//                             left join ProductPicture pp on pp.Id = pi.PictureId
//                             left join MenuProduct mp on mp.ProductId = p.Id
//                             left join ContentType ct on ct.Id = mp.MenuId
//                             ");
//            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append(" where 1=1 " + sqlWhere);
//            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

//            if (totalRecords == 0) return new List<ProductInfo>();

//            sb.Clear();
//            int startIndex = (pageIndex - 1) * pageSize + 1;
//            int endIndex = pageIndex * pageSize;

//            sb.Append(@"select * from(select row_number() over(order by p.LastUpdatedDate desc) as RowNumber,
//			          p.Id,p.ProductName,p.SubTitle,p.OriginalPrice,p.ProductPrice,p.Discount,p.DiscountDescri,p.ProductPictureId,p.StockNum,p.CustomMenuId,p.UserId,p.LastUpdatedDate,
//					  pp.OriginalPicture,pp.BPicture,pp.MPicture,pp.SPicture
//                      from Product p 
//                      join ProductItem pi on pi.ProductId = p.Id
//                      left join ProductPicture pp on pp.Id = pi.PictureId
//                      left join MenuProduct mp on mp.ProductId = p.Id
//                      left join ContentType ct on ct.Id = mp.MenuId
//                      ");
//            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append("where 1=1 " + sqlWhere);
//            sb.Append(" )as objTable where RowNumber between " + startIndex + " and " + endIndex + " ");

//            List<ProductInfo> list = new List<ProductInfo>();

//            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
//            {
//                if (reader != null && reader.HasRows)
//                {
//                    while (reader.Read())
//                    {
//                        ProductInfo model = new ProductInfo();
//                        model.Id = reader.GetGuid(1);
//                        model.ProductName = reader.GetString(2);
//                        model.SubTitle = reader.GetString(3);
//                        model.ProductPictureId = reader.GetGuid(4);
//                        model.OriginalPrice = reader.GetDecimal(5);
//                        model.ProductPrice = reader.GetDecimal(6);
//                        model.Discount = reader.GetDouble(7);
//                        model.DiscountDescri = reader.GetString(8);
//                        model.StockNum = reader.GetInt32(9);
//                        model.UserId = reader.GetGuid(10);
//                        model.LastUpdatedDate = reader.GetDateTime(11);
//                        model.OriginalPicture = reader.IsDBNull(12) ? "" : reader.GetString(12);
//                        model.BPicture = reader.IsDBNull(13) ? "" : reader.GetString(13);
//                        model.MPicture = reader.IsDBNull(14) ? "" : reader.GetString(14);
//                        model.SPicture = reader.IsDBNull(15) ? "" : reader.GetString(15);

//                        list.Add(model);
//                    }
//                }
//            }

//            return list;
        }

        private bool IsExist(string name, object userId, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@Named",SqlDbType.NVarChar, 30),
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = name;
            parms[1].Value = Guid.Parse(userId.ToString());

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [Product] where lower(Named) = @Named and UserId = @UserId and Id <> @Id ");

                Array.Resize(ref parms, 3);
                parms[2] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[2].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [Product] where lower(Named) = @Named and UserId = @UserId ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
