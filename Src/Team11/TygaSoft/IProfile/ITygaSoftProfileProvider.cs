using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.Model;

namespace TygaSoft.IProfile
{
    public interface ITygaSoftProfileProvider
    {
        #region ITygaSoftProfileProvider Member
 
        IList<CartItemInfo> GetCartItems(string userName, string appName, bool isShoppingCart);

        void SetCartItems(object profileId, ICollection<CartItemInfo> cartItems, bool isShoppingCart);

        void UpdateActivityDates(string userName, bool activityOnly, string appName);

        Guid GetProfileId(string userName, bool isAuthenticated, bool ignoreAuthenticationType, string appName);

        int CreateProfileForUser(Guid profileId, string userName, bool isAuthenticated, string appName);

        IList<string> GetInactiveProfiles(int authenticationOption, DateTime userInactiveSinceDate, string appName);

        bool DeleteProfile(string userName, string appName);

        IList<ProfilesInfo> GetProfileInfo(int authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, string appName, out int totalRecords);

        #endregion
    }
}
