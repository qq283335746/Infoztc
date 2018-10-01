using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TygaSoft.DBUtility;
using TygaSoft.IProfile;
using TygaSoft.Model;

namespace TygaSoft.SqlProfileDAL
{
    class TygaSoftProfileProvider : ITygaSoftProfileProvider
    {
        private const int AUTH_ANONYMOUS = 0;
        private const int AUTH_AUTHENTICATED = 1;
        private const int AUTH_ALL = 2;

        public int CreateProfileForUser(Guid profileId, string userName, bool isAuthenticated, string appName)
        {
            string cmdText = @"INSERT INTO Profiles (Id,Username, AppName, LastActivityDate, LastUpdatedDate, IsAnonymous) 
                                 Values(@Id,@Username, @AppName, @LastActivityDate, @LastUpdatedDate, @IsAnonymous);";

            SqlParameter[] parms = {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier),
                new SqlParameter("@Username", SqlDbType.NVarChar, 50),
                new SqlParameter("@AppName", SqlDbType.NVarChar, 50),
                new SqlParameter("@LastActivityDate", SqlDbType.DateTime),
                new SqlParameter("@LastUpdatedDate", SqlDbType.DateTime),
                new SqlParameter("@IsAnonymous", SqlDbType.Bit)
            };
            parms[0].Value = profileId;
            parms[1].Value = userName;
            parms[2].Value = appName;
            parms[3].Value = DateTime.Now;
            parms[4].Value = DateTime.Now;
            parms[5].Value = !isAuthenticated;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, cmdText, parms);
        }

        public bool DeleteProfile(string userName, string appName)
        {
            Guid profileId = GetProfileId(userName, false, true, appName);

            string sqlDelete = "DELETE FROM Profiles WHERE Id = @Id;";
            SqlParameter param = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            param.Value = profileId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sqlDelete, param) > 0;
        }

        public IList<CartItemInfo> GetCartItems(string userName, string appName, bool isShoppingCart)
        {
            string sqlSelect = @"SELECT c.ProfileId,c.ProductId,c.CategoryId,c.Price,c.Quantity,c.Named,c.IsShoppingCart,c.LastUpdatedDate 
                                 FROM Profiles p INNER JOIN Cart c ON p.Id = c.ProfileId WHERE p.Username = @Username AND p.AppName = @AppName 
                                 AND IsShoppingCart = @IsShoppingCart;";

            SqlParameter[] parms = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50),
                new SqlParameter("@AppName", SqlDbType.NVarChar,50),
                new SqlParameter("@IsShoppingCart", SqlDbType.Bit)};
            parms[0].Value = userName;
            parms[1].Value = appName;
            parms[2].Value = isShoppingCart;

            IList<CartItemInfo> cartItems = new List<CartItemInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sqlSelect, parms))
            {
                while (reader.Read())
                {
                    CartItemInfo model = new CartItemInfo();
                    model.ProfileId = reader.GetGuid(0);
                    model.ProductId = reader.GetGuid(1);
                    model.CategoryId = reader.GetGuid(2);
                    model.Price = reader.GetDecimal(3);
                    model.Quantity = reader.GetInt32(4);
                    model.Named = reader.GetString(5);
                    model.IsShoppingCart = reader.GetBoolean(6);
                    model.LastUpdatedDate = reader.GetDateTime(7);
                    cartItems.Add(model);
                }
            }
                
            return cartItems;
        }

        public void SetCartItems(object profileId, ICollection<CartItemInfo> cartItems, bool isShoppingCart)
        {
            string sqlDelete = "DELETE FROM Cart WHERE ProfileId = @ProfileId AND IsShoppingCart = @IsShoppingCart;";

            SqlParameter[] parms = {
                new SqlParameter("@ProfileId", SqlDbType.UniqueIdentifier),
                new SqlParameter("@IsShoppingCart", SqlDbType.Bit)};
            parms[0].Value = Guid.Parse(profileId.ToString());
            parms[1].Value = isShoppingCart;

            if (cartItems.Count > 0)
            {
                string sqlInsert = "INSERT INTO Cart (ProfileId, ProductId, CategoryId, Price, Quantity, Named, IsShoppingCart, LastUpdatedDate) VALUES (@ProfileId, @ProductId, @CategoryId, @Price, @Quantity, @Named, @IsShoppingCart, @LastUpdatedDate);";

                SqlParameter[] parms2 = {
                new SqlParameter("@ProfileId", SqlDbType.UniqueIdentifier),
                new SqlParameter("@IsShoppingCart", SqlDbType.Bit),
                new SqlParameter("@ProductId", SqlDbType.UniqueIdentifier),
                new SqlParameter("@CategoryId", SqlDbType.UniqueIdentifier),
                new SqlParameter("@Price", SqlDbType.Decimal),
                new SqlParameter("@Quantity", SqlDbType.Int),
                new SqlParameter("@Named", SqlDbType.NVarChar,50),
                new SqlParameter("@LastUpdatedDate", SqlDbType.DateTime)
                };
                parms2[0].Value = Guid.Parse(profileId.ToString());
                parms2[1].Value = isShoppingCart;

                SqlConnection conn = new SqlConnection(SqlHelper.HnztcShopDbConnString);
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlDelete, parms);

                    foreach (CartItemInfo cartItem in cartItems)
                    {
                        parms2[2].Value = Guid.Parse(cartItem.ProductId.ToString());
                        parms2[3].Value = Guid.Parse(cartItem.CategoryId == null ? Guid.Empty.ToString() : cartItem.CategoryId.ToString());
                        parms2[4].Value = cartItem.Price;
                        parms2[5].Value = cartItem.Quantity;
                        parms2[6].Value = cartItem.Named;
                        parms2[7].Value = cartItem.LastUpdatedDate;
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlInsert, parms2);
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new ApplicationException(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
                // delete cart
                SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sqlDelete, parms);
        }

        public IList<string> GetInactiveProfiles(int authenticationOption, DateTime userInactiveSinceDate, string appName)
        {
            StringBuilder sqlSelect = new StringBuilder("SELECT Username FROM Profiles WHERE AppName = @AppName AND LastActivityDate <= @LastActivityDate");

            SqlParameter[] parms = {
                new SqlParameter("@AppName", SqlDbType.NVarChar, 50),
                new SqlParameter("@LastActivityDate", SqlDbType.DateTime)};
            parms[0].Value = appName;
            parms[1].Value = userInactiveSinceDate;

            switch (authenticationOption)
            {
                case AUTH_ANONYMOUS:
                    sqlSelect.Append(" AND IsAnonymous = @IsAnonymous");
                    Array.Resize<SqlParameter>(ref parms, parms.Length + 1);
                    parms[2] = new SqlParameter("@IsAnonymous", SqlDbType.Bit);
                    parms[2].Value = true;
                    break;
                case AUTH_AUTHENTICATED:
                    sqlSelect.Append(" AND IsAnonymous = @IsAnonymous");
                    Array.Resize<SqlParameter>(ref parms, parms.Length + 1);
                    parms[2] = new SqlParameter("@IsAnonymous", SqlDbType.Bit);
                    parms[2].Value = false;
                    break;
                default:
                    break;
            }

            IList<string> usernames = new List<string>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sqlSelect.ToString(), parms))
            {
                while (reader.Read())
                {
                    usernames.Add(reader.GetString(0));
                }
            }

            return usernames;
        }

        public IList<ProfilesInfo> GetProfileInfo(int authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, string appName, out int totalRecords)
        {
            StringBuilder sqlSelect1 = new StringBuilder("SELECT COUNT(*) FROM Profiles WHERE AppName = @AppName");
            SqlParameter[] parms1 = {
                new SqlParameter("@AppName", SqlDbType.NVarChar,50)};
            parms1[0].Value = appName;

            StringBuilder sqlSelect2 = new StringBuilder("SELECT Username,IsAnonymous, LastActivityDate, LastUpdatedDate FROM Profiles WHERE AppName = @AppName");
            SqlParameter[] parms2 = { new SqlParameter("@AppName", SqlDbType.NVarChar,50) };
            parms2[0].Value = appName;

            int arraySize;

            if (usernameToMatch != null)
            {
                arraySize = parms1.Length;

                sqlSelect1.Append(" AND Username LIKE @Username ");
                Array.Resize<SqlParameter>(ref parms1, arraySize + 1);
                parms1[arraySize] = new SqlParameter("@Username", SqlDbType.VarChar, 256);
                parms1[arraySize].Value = usernameToMatch;

                sqlSelect2.Append(" AND Username LIKE @Username ");
                Array.Resize<SqlParameter>(ref parms2, arraySize + 1);
                parms2[arraySize] = new SqlParameter("@Username", SqlDbType.VarChar, 256);
                parms2[arraySize].Value = usernameToMatch;
            }

            if (userInactiveSinceDate != null)
            {
                arraySize = parms1.Length;

                sqlSelect1.Append(" AND LastActivityDate >= @LastActivityDate ");
                Array.Resize<SqlParameter>(ref parms1, arraySize + 1);
                parms1[arraySize] = new SqlParameter("@LastActivityDate", SqlDbType.DateTime);
                parms1[arraySize].Value = (DateTime)userInactiveSinceDate;

                sqlSelect2.Append(" AND LastActivityDate >= @LastActivityDate ");
                Array.Resize<SqlParameter>(ref parms2, arraySize + 1);
                parms2[arraySize] = new SqlParameter("@LastActivityDate", SqlDbType.DateTime);
                parms2[arraySize].Value = (DateTime)userInactiveSinceDate;
            }

            if (authenticationOption != AUTH_ALL)
            {
                arraySize = parms1.Length;

                Array.Resize<SqlParameter>(ref parms1, arraySize + 1);
                sqlSelect1.Append(" AND IsAnonymous = @IsAnonymous");
                parms1[arraySize] = new SqlParameter("@IsAnonymous", SqlDbType.Bit);

                Array.Resize<SqlParameter>(ref parms2, arraySize + 1);
                sqlSelect2.Append(" AND IsAnonymous = @IsAnonymous");
                parms2[arraySize] = new SqlParameter("@IsAnonymous", SqlDbType.Bit);

                switch (authenticationOption)
                {
                    case AUTH_ANONYMOUS:
                        parms1[arraySize].Value = true;
                        parms2[arraySize].Value = true;
                        break;
                    case AUTH_AUTHENTICATED:
                        parms1[arraySize].Value = false;
                        parms2[arraySize].Value = false;
                        break;
                    default:
                        break;
                }
            }

            IList<ProfilesInfo> list = new List<ProfilesInfo>();

            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sqlSelect1.ToString(), parms1);

            if (totalRecords <= 0)
                return list;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.HnztcShopDbConnString, CommandType.Text, sqlSelect2.ToString(), parms2))
            {
                while (reader.Read())
                {
                    ProfilesInfo model = new ProfilesInfo();
                    model.Username = reader.GetString(1);
                    model.IsAnonymous = reader.GetBoolean(3);
                    model.LastActivityDate = reader.GetDateTime(4);
                    model.LastUpdatedDate = reader.GetDateTime(5);

                    list.Add(model);
                }
            }

            return list;
        }

        public Guid GetProfileId(string userName, bool isAuthenticated, bool ignoreAuthenticationType, string appName)
        {
            string sqlSelect = "SELECT Id FROM Profiles WHERE Username = @Username AND AppName = @AppName";

            SqlParameter[] parms = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50),
                new SqlParameter("@AppName", SqlDbType.NVarChar, 50)
            };
            parms[0].Value = userName;
            parms[1].Value = appName;

            if (!ignoreAuthenticationType)
            {
                sqlSelect += " AND IsAnonymous = @IsAnonymous";
                Array.Resize<SqlParameter>(ref parms, parms.Length + 1);
                parms[2] = new SqlParameter("@IsAnonymous", SqlDbType.Bit);
                parms[2].Value = !isAuthenticated;
            }

            Guid Id = Guid.Empty;

            object retVal = null;
            retVal = SqlHelper.ExecuteScalar(SqlHelper.HnztcShopDbConnString, CommandType.Text, sqlSelect, parms);

            if (retVal == null)
            {
                Id = Guid.NewGuid();
                CreateProfileForUser(Id, userName, isAuthenticated, appName);
            }
            else
                Id = Guid.Parse(retVal.ToString());
            return Id;
        }

        public void UpdateActivityDates(string userName, bool activityOnly, string appName)
        {
            DateTime activityDate = DateTime.Now;

            string sqlUpdate;
            SqlParameter[] parms;

            if (activityOnly)
            {
                sqlUpdate = "UPDATE Profiles Set LastActivityDate = @LastActivityDate WHERE Username = @Username AND AppName = @AppName;";
                parms = new SqlParameter[]{
                    new SqlParameter("@LastActivityDate", SqlDbType.DateTime),
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50),
                    new SqlParameter("@AppName", SqlDbType.NVarChar, 50)};

                parms[0].Value = activityDate;
                parms[1].Value = userName;
                parms[2].Value = appName;

            }
            else
            {
                sqlUpdate = "UPDATE Profiles Set LastActivityDate = @LastActivityDate, LastUpdatedDate = @LastUpdatedDate WHERE Username = @Username AND AppName = @AppName;";
                parms = new SqlParameter[]{
                    new SqlParameter("@LastActivityDate", SqlDbType.DateTime),
                    new SqlParameter("@LastUpdatedDate", SqlDbType.DateTime),
                    new SqlParameter("@Username", SqlDbType.VarChar, 256),
                    new SqlParameter("@AppName", SqlDbType.VarChar, 256)};

                parms[0].Value = activityDate;
                parms[1].Value = activityDate;
                parms[2].Value = userName;
                parms[3].Value = appName;
            }

            SqlHelper.ExecuteNonQuery(SqlHelper.HnztcShopDbConnString, CommandType.Text, sqlUpdate, parms);
        }
    }
}
