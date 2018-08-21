using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Security;

namespace TygaSoft.WcfSecurityService
{
    public partial class WebSecurityService : IWebSecurity
    {
        public string Login(string username, string password)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<LoginInfo>");
            string errorMsg = string.Empty;
            try
            {
                MembershipUser userInfo = Membership.GetUser(username);
                if (!Membership.ValidateUser(username, password))
                {
                    sb.AppendFormat("<IsLogined>{0}</IsLogined>", false);
                    if (userInfo == null)
                    {
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "用户名不存在！");
                    }
                    if (userInfo.IsLockedOut)
                    {
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "您的账号已被锁定，请联系管理员先解锁后才能登录！");
                    }
                    if (!userInfo.IsApproved)
                    {
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "您的帐户尚未获得批准。您无法登录，直到管理员批准您的帐户！");
                    }
                    else
                    {
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "密码不正确，请检查！");
                    }
                    sb.Append("<LoginData></LoginData>");
                }
                else
                {
                    sb.AppendFormat("<IsLogined>{0}</IsLogined>", true);
                    sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "登录成功！");
                    sb.Append("<UserInfo>");
                    sb.AppendFormat("<UserName>{0}</UserName><HeadImg></HeadImg><NickName></NickName><Sex></Sex><MobilePhone></MobilePhone><VipLevel></VipLevel>", userInfo.UserName);
                    sb.Append("</UserInfo>");
                }
            }
            catch (Exception ex)
            {
                sb.AppendFormat("<IsLogined>{0}</IsLogined>", false);
                sb.AppendFormat("<LoginedRtMsg>{0}</LoginedRtMsg>", ex.Message);
                sb.Append("<LoginData></LoginData>");
            }

            sb.Append("</LoginInfo>");

            return sb.ToString();
        }

        public string Register(string username, string password)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<RegisterInfo>");
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    MembershipUser user = Membership.GetUser(username);
                    if (user != null)
                    {
                        sb.AppendFormat("<IsRegistered>{0}</IsRegistered>", false);
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "注册失败，已存在用户：" + username + "，请换一个账号再重试");
                    }
                    user = Membership.CreateUser(username, password);
                    if (user == null)
                    {
                        sb.AppendFormat("<IsRegistered>{0}</IsRegistered>", false);
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "注册失败，请检查：密码正确格式应由数字或字母组成的字符串，且最小6位，最大30位");
                    }
                    else
                    {
                        Roles.AddUserToRole(username, "Users");
                        sb.AppendFormat("<IsRegistered>{0}</IsRegistered>", true);
                    }

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                sb.Append("<ErrorMsg> 异常：" + ex.Message + "</ErrorMsg>");
            }
            sb.Append("</RegisterInfo>");

            return sb.ToString();
        }

        public object GetUserId(string username)
        {
            try
            {
                MembershipUser user = Membership.GetUser(username);
                if (user == null) return null;
                return user.ProviderUserKey;
            }
            catch
            {
                return null;
            }
        }

        public bool ValidateUser(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }
    }
}
