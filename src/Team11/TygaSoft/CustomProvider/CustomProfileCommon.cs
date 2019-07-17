using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Profile;
using TygaSoft.BLL;

namespace TygaSoft.CustomProvider
{
    public class CustomProfileCommon : ProfileBase
    {
        public new void Save()
        {
            HttpContext.Current.Profile.Save();
        }

        [SettingsAllowAnonymous(false)]
        [ProfileProvider("ShoppingCartProvider")]
        public CartItem ShoppingCart
        {
            get { return (CartItem)HttpContext.Current.Profile.GetPropertyValue("ShoppingCart"); }
            set { HttpContext.Current.Profile.SetPropertyValue("ShoppingCart",value); }
        }

        public CustomProfileCommon GetProfile(string userName, bool isAuthenticated)
        {
            return (CustomProfileCommon)ProfileBase.Create(userName, isAuthenticated);
        }

        public string GetUserName()
        {
            if (HttpContext.Current.Profile.IsAnonymous) return HttpContext.Current.Request.AnonymousID;
            else return HttpContext.Current.Profile.UserName;
        }
    }
}
