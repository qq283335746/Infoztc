using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TygaSoft.CustomExceptions;
using TygaSoft.SysHelper;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Mobile.Shares.Ernie
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated) Response.Redirect("/m/yernie.html");

            if (Request.HttpMethod.ToLower() == "post")
            {
                Bind();
            }
        }

        private void Bind()
        {
            string appId = Request.Form["appID"];
            if (string.IsNullOrWhiteSpace(appId))
            {
                ltrMyData.Text = "<span style=\"color:red;\">非法请求</span>";
                return;
            }
            string userName = Request.Form["loginID"];
            if (string.IsNullOrWhiteSpace(userName))
            {
                ltrMyData.Text = "<span style=\"color:red;\">非法请求</span>";
                return;
            }
            string sessionId = Request.Form["sessionID"];
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                ltrMyData.Text = "<span style=\"color:red;\">非法请求</span>";
                return;
            }

            MembershipUser user = Membership.GetUser(userName);
            if (user == null)
            {
                ltrMyData.Text = "<span style=\"color:red;\">非法请求</span>";
                return;
            }

            if (!IsValid(appId, userName, sessionId))
            {
                ltrMyData.Text = "<span style=\"color:red;\">非法请求</span>";
                return;
            }

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddHours(2),
                true, user.ProviderUserKey.ToString(), FormsAuthentication.FormsCookiePath);
            string encTicket = FormsAuthentication.Encrypt(ticket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            Response.Redirect("/m/yernie.html");
        }

        private bool IsValid(string appId, string loginId, string sessionId)
        {
            try
            {
                string H5AuthenticatedUrl = WebConfigurationManager.AppSettings["H5AuthenticatedUrl"] + "/VerifyLotteryPower";

                object[] postDataArr = { appId, loginId, sessionId };
                string content = string.Format("appID={0}&loginID={1}&sessionID={2}", postDataArr);
                int statusCode = -1;
                string result = "";
                HttpHelper.DoHttpPost(H5AuthenticatedUrl, content, out statusCode, out result);
                if (statusCode != 200)
                {
                    return false;
                }
                result = HttpUtility.HtmlDecode(result);
                XElement root = XElement.Parse(result);
                var q = root.Descendants().FirstOrDefault(x => x.Name.LocalName == "IsOK");
                if (q != null)
                {
                    if (q.Value.ToLower() == "true")
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                new CustomException(string.Format("方法：bool IsValid(string appId, string loginId, string sessionId)，异常：{0}", ex.Message), ex);
            }

            return false;
        }
    }
}