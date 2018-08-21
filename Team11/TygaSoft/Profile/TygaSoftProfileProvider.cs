using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Profile;
using TygaSoft.IProfile;
using TygaSoft.ProfileDALFactory;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Profile
{
    public sealed class TygaSoftProfileProvider : ProfileProvider
    {
        private static readonly ITygaSoftProfileProvider dal = DataAccess.CreateProfileProvider();

        private const string ERR_INVALID_PARAMETER = "未支持的参数：";
        private const string PROFILE_SHOPPINGCART = "ShoppingCart";
        private const string PROFILE_WISHLIST = "WishList";
        private static string applicationName = "HnztcWeb";

        public override string ApplicationName
        {
            get{
                return applicationName;
            }
            set{
                applicationName = value;
            }
        }

		public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Hnztc Custom Profile Provider");
            }

            if (string.IsNullOrEmpty(name))
                name = "TygaSoftProfileProvider";

            if (config["applicationName"] != null && !string.IsNullOrEmpty(config["applicationName"].Trim()))
                applicationName = config["applicationName"];

            base.Initialize(name, config);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            string username = (string)context["UserName"];
            bool isAuthenticated = (bool)context["IsAuthenticated"];

            SettingsPropertyValueCollection svc = new SettingsPropertyValueCollection();

            foreach (SettingsProperty prop in collection)
            {
                SettingsPropertyValue pv = new SettingsPropertyValue(prop);

                switch (pv.Property.Name)
                {
                    case PROFILE_SHOPPINGCART:
                        pv.PropertyValue = GetCartItems(username, true);
                        break;
                    default:
                        throw new ApplicationException(ERR_INVALID_PARAMETER + " name.");
                }

                svc.Add(pv);
            }
            return svc;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            string username = (string)context["UserName"];
            CheckUserName(username);
            bool isAuthenticated = (bool)context["IsAuthenticated"];
            Guid profileId = dal.GetProfileId(username, isAuthenticated, false, ApplicationName);
            if (profileId.Equals(Guid.Empty))
            {
                profileId = Guid.NewGuid();
                dal.CreateProfileForUser(profileId,username, isAuthenticated, ApplicationName);
            }

            foreach (SettingsPropertyValue pv in collection)
            {
                if (pv.PropertyValue != null)
                {
                    switch (pv.Property.Name)
                    {
                        case PROFILE_SHOPPINGCART:
                            SetCartItems(profileId, (CartItem)pv.PropertyValue, true);
                            break;
                        default:
                            throw new ApplicationException(ERR_INVALID_PARAMETER + " name.");
                    }
                }
            }

            UpdateActivityDates(username, false);
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        private static bool DeleteProfile(string username)
        {
            CheckUserName(username);
            return dal.DeleteProfile(username, applicationName);
        }

        public override int DeleteProfiles(string[] usernames)
        {
            int deleteCount = 0;

            foreach (string user in usernames)
                if (DeleteProfile(user))
                    deleteCount++;

            return deleteCount;
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            int deleteCount = 0;

            foreach (ProfileInfo p in profiles)
                if (DeleteProfile(p.UserName))
                    deleteCount++;

            return deleteCount;
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        #region 私有方法

        private static CartItem GetCartItems(string username, bool isShoppingCart)
        {
            CartItem cart = new CartItem();
            foreach (CartItemInfo cartItem in dal.GetCartItems(username, applicationName, isShoppingCart))
            {
                cart.Add(cartItem);
            }
            return cart;
        }

        private static void SetCartItems(object profileId, CartItem cart, bool isShoppingCart)
        {
            dal.SetCartItems(profileId, cart.CartItems, isShoppingCart);
        }

        private static void UpdateActivityDates(string username, bool activityOnly)
        {
            dal.UpdateActivityDates(username, activityOnly, applicationName);
        }

        private static void CheckUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName) || userName.Length > 256 || userName.IndexOf(",") > 0)
                throw new ApplicationException(ERR_INVALID_PARAMETER + " user name.");
        }

        #endregion
    }
}
