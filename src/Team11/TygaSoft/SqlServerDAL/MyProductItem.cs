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
    public partial class ProductItem
    {
        public Dictionary<string, string> GetKeyValueByProductId(object productId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@" select Id,Named from ProductItem where ProductId = @ProductId ");

            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(productId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(),parm))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dic.Add(reader[0].ToString(), reader.GetString(1));
                    }
                }
            }

            return dic;
        }

        public bool DeleteBatchByProductId(IList<object> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from ProductItem where ProductId = @ProductId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ProductId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            int effect = SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms != null ? parms.ToArray() : null);
            if (effect > 0) result = true;

            return result;
        }

        public ProductItemInfo GetModelByJoin(object Id)
        {
            ProductItemInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 pi.Id,pi.ProductId,pi.Named,pi.PictureId,pi.EnableStartTime,pi.EnableEndTime,pi.IsDisable, 
                        pp.OriginalPicture,pp.BPicture,pp.MPicture,pp.SPicture
			            from ProductItem pi
                        left join ProductPicture pp on pp.Id = pi.PictureId
						where pi.Id = @Id 
                      ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ProductItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);
                        model.EnableStartTime = reader.GetDateTime(4);
                        model.EnableEndTime = reader.GetDateTime(5);
                        model.IsDisable = reader.GetBoolean(6);

                        model.OriginalPicture = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        model.BPicture = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        model.MPicture = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        model.SPicture = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    }
                }
            }

            return model;
        }

        public IList<ProductItemInfo> GetListByProduct(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select pi.Id,pi.ProductId,pi.Named,pi.PictureId,pi.EnableStartTime,pi.EnableEndTime,pi.IsDisable,
                        ppi.FileExtension,ppi.FileDirectory,ppi.RandomFolder
                        from ProductItem pi
                        left join Picture_Product ppi on ppi.Id = pi.PictureId
                        ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ProductItemInfo> list = new List<ProductItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductItemInfo model = new ProductItemInfo();
                        model.Id = reader.GetGuid(0);
                        model.ProductId = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.PictureId = reader.GetGuid(3);
                        model.EnableStartTime = reader.GetDateTime(4);
                        model.EnableEndTime = reader.GetDateTime(5);
                        model.IsDisable = reader.GetBoolean(6);

                        model.FileExtension = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        model.FileDirectory = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        model.RandomFolder = reader.IsDBNull(9) ? "" : reader.GetString(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<ProductAllInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            totalRecords = 0;
            return null;

//            StringBuilder sb = new StringBuilder(300);
//            sb.Append(@"select count(1) from Product p
//                        left join ProductDetail pd on pd.ProductId = p.Id
//                        join ProductItem pi on pi.ProductId = p.Id
//                        left join ProductPicture pp on pp.Id = p.ProductPictureId
//                        left join ProductPicture ppi on ppi.Id = pi.ColorPictureId
//                        left join SizePicture sp on sp.Id = pi.SizePictureId
//                        left join CategoryProduct cp on cp.ProductId = p.Id
//                        left join Category c on c.Id = cp.CategoryId
//                        left join BrandProduct bp on bp.ProductId = p.Id
//                        left join Brand b on b.Id = bp.BrandId
//                        left join MenuProduct mp on mp.ProductId = p.Id
//                        left join HnztcDb.dbo.ContentType ct on ct.Id = mp.MenuId
//                        ");
//            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append(" where 1=1 " + sqlWhere);
//            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms);

//            if (totalRecords == 0) return new List<ProductAllInfo>();

//            sb.Clear();
//            int startIndex = (pageIndex - 1) * pageSize + 1;
//            int endIndex = pageIndex * pageSize;

//            sb.Append(@"select * from(select row_number() over(order by p.LastUpdatedDate desc) as RowNumber,
//			          p.Id,p.ProductName,p.SubTitle,p.ProductPictureId,p.OriginalPrice,p.ProductPrice,p.Discount,p.DiscountDescri,p.StockNum ProductStockNum,p.Sort,p.EnableStartTime,p.EnableEndTime,p.UserId,p.IsDisable,p.LastUpdatedDate,
//                      pd.OtherPicture,pd.PayOption,pd.ViewCount,                   
//                      pi.Id as ProductItemId,pi.ProductId,pi.SizeName,pi.SizePictureId,pi.ColorName,pi.ColorPictureId,pi.GoodsCode,pi.GoodsBarCode,pi.StockNum, 
//                      pp.OriginalPicture,pp.BPicture,pp.MPicture,pp.SPicture,
//                      ppi.OriginalPicture ColorOriginalPicture,ppi.BPicture ColorBPicture,ppi.MPicture ColorMPicture,ppi.SPicture ColorSPicture,
//                      sp.OriginalPicture SizeOriginalPicture,sp.BPicture SizeBPicture,sp.MPicture SizeMPicture,sp.SPicture SizeSPicture,
//                      c.CategoryName,b.BrandName,ct.TypeValue as MenuName
//					  from Product p
//                      left join ProductDetail pd on pd.ProductId = p.Id
//                      join ProductItem pi on pi.ProductId = p.Id
//                      left join ProductPicture pp on pp.Id = p.ProductPictureId
//                      left join ProductPicture ppi on ppi.Id = pi.ColorPictureId
//                      left join SizePicture sp on sp.Id = pi.SizePictureId
//                      left join CategoryProduct cp on cp.ProductId = p.Id
//                      left join Category c on c.Id = cp.CategoryId
//                      left join BrandProduct bp on bp.ProductId = p.Id
//                      left join Brand b on b.Id = bp.BrandId
//                      left join MenuProduct mp on mp.ProductId = p.Id
//                      left join HnztcDb.dbo.ContentType ct on ct.Id = mp.MenuId
//                      ");
//            if (!string.IsNullOrEmpty(sqlWhere)) sb.Append("where 1=1 " + sqlWhere);
//            sb.Append(" )as objTable where RowNumber between " + startIndex + " and " + endIndex + " ");

//            List<ProductAllInfo> list = new List<ProductAllInfo>();

//            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), cmdParms))
//            {
//                if (reader != null && reader.HasRows)
//                {
//                    while (reader.Read())
//                    {
//                        ProductAllInfo model = new ProductAllInfo();
//                        model.ProductId = reader.GetGuid(1);
//                        model.ProductName = reader.GetString(2);
//                        model.SubTitle = reader.GetString(3);
//                        model.ProductPictureId = reader.GetGuid(4);
//                        model.OriginalPrice = reader.GetDecimal(5);
//                        model.ProductPrice = reader.GetDecimal(6);
//                        model.Discount = reader.GetDouble(7);
//                        model.DiscountDescri = reader.GetString(8);
//                        model.StockNum = reader.GetInt32(9);
//                        model.Sort = reader.GetInt32(10);
//                        model.EnableStartTime = reader.GetDateTime(11);
//                        model.EnableEndTime = reader.GetDateTime(12);
//                        model.UserId = reader.GetGuid(13);
//                        model.IsDisable = reader.GetBoolean(14);
//                        model.LastUpdatedDate = reader.GetDateTime(15);

//                        model.OtherPicture = reader.GetString(16);
//                        model.PayOption = reader.GetString(17);
//                        model.ViewCount = reader.GetInt32(18);

//                        model.ProductItemId = reader.GetGuid(19);
//                        model.ProductId = reader.GetGuid(20);
//                        model.SizeName = reader.GetString(21);
//                        model.SizePictureId = reader.GetGuid(22);
//                        model.ColorName = reader.GetString(23);
//                        model.ColorPictureId = reader.GetGuid(24);
//                        model.GoodsCode = reader.GetString(25);
//                        model.GoodsBarCode = reader.GetString(26);
//                        model.StockNum = reader.GetInt32(27);

//                        model.OriginalPicture = reader.IsDBNull(28) ? "" : reader.GetString(28);
//                        model.BPicture = reader.IsDBNull(29) ? "" : reader.GetString(29);
//                        model.MPicture = reader.IsDBNull(30) ? "" : reader.GetString(30);
//                        model.SPicture = reader.IsDBNull(31) ? "" : reader.GetString(31);

//                        model.ColorOriginalPicture = reader.IsDBNull(32) ? "" : reader.GetString(32);
//                        model.ColorBPicture = reader.IsDBNull(33) ? "" : reader.GetString(33);
//                        model.ColorMPicture = reader.IsDBNull(34) ? "" : reader.GetString(34);
//                        model.ColorSPicture = reader.IsDBNull(35) ? "" : reader.GetString(35);

//                        model.SizeOriginalPicture = reader.IsDBNull(36) ? "" : reader.GetString(36);
//                        model.SizeBPicture = reader.IsDBNull(37) ? "" : reader.GetString(37);
//                        model.SizeMPicture = reader.IsDBNull(38) ? "" : reader.GetString(38);
//                        model.SizeSPicture = reader.IsDBNull(39) ? "" : reader.GetString(39);

//                        model.CategoryName = reader.IsDBNull(40) ? "" : reader.GetString(40);
//                        model.BrandName = reader.IsDBNull(41) ? "" : reader.GetString(41);
//                        model.MenuName = reader.IsDBNull(42) ? "" : reader.GetString(42);

//                        list.Add(model);
//                    }
//                }
//            }

//            return list;
        }

        private bool IsExist(object productId, string name, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@ProductId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@Named",SqlDbType.NVarChar,50),
                                   };
            parms[0].Value = Guid.Parse(productId.ToString());
            parms[1].Value = name;

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [ProductItem] where ProductId = @ProductId and lower(Named) = @Named and Id <> @Id ");

                Array.Resize(ref parms, 3);
                parms[2] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[2].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [ProductItem] where ProductId = @ProductId and lower(Named) = @Named ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }
    }
}
