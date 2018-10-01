using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Security;
using System.Configuration;
using System.Text.RegularExpressions;
using TygaSoft.CustomExceptions;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.SysHelper;

namespace TygaSoft.WcfService
{
    public partial class WebSecurityService : IWebSecurity
    {
        public static readonly string WebSiteHost_Team = ConfigurationManager.AppSettings["WebSiteHost_Team"].Trim('/');
        public static readonly string PasswordExpression = ConfigurationManager.AppSettings["PasswordExpression"].Trim('/');

        public string Login(string username, string password)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<LoginInfo>");

                MembershipUser user = Membership.GetUser(username);
                if (!Membership.ValidateUser(username, password))
                {
                    sb.AppendFormat("<IsLogined>{0}</IsLogined>", false);
                    if (user == null)
                    {
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "用户名不存在！");
                    }
                    else
                    {
                        if (user.IsLockedOut)
                        {
                            sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "您的账号已被锁定，请联系管理员先解锁后才能登录！");
                        }
                        else if (!user.IsApproved)
                        {
                            sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "您的帐户尚未获得批准。您无法登录，直到管理员批准您的帐户！");
                        }
                        else
                        {
                            sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "密码不正确，请检查！");
                        }
                    }
                }
                else
                {
                    sb.AppendFormat("<IsLogined>{0}</IsLogined>", true);
                    sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "登录成功！");
                    UserBase ubBll = new UserBase();
                    var ubModel = ubBll.GetModel(user.ProviderUserKey);

                    sb.Append("<UserInfo>");
                    if (ubModel != null)
                    {
                        sb.AppendFormat(@"<Nickname>{0}</Nickname><HeadImg>{1}</HeadImg><Sex>{2}</Sex><MobilePhone>{3}</MobilePhone><TotalGold>{4}</TotalGold><TotalSilver>{5}</TotalSilver><TotalIntegral>{6}</TotalIntegral><SilverLevel>{7}</SilverLevel><ColorLevel>{8}</ColorLevel><IntegralLevel>{9}</IntegralLevel><VIPLevel>{10}</VIPLevel><UserId>{11}</UserId>", 
                            ubModel.Nickname, WebSiteHost_Team + ubModel.HeadPicture, ubModel.Sex, ubModel.MobilePhone, ubModel.TotalGold, ubModel.TotalSilver, ubModel.TotalIntegral, ubModel.SilverLevel, ubModel.ColorLevel,ubModel.IntegralLevel,ubModel.VIPLevel, ubModel.UserId);
                    }
                    else
                    {
                        sb.Append("<Nickname></Nickname><HeadImg></HeadImg><Sex></Sex><MobilePhone></MobilePhone><TotalGold></TotalGold><TotalSilver></TotalSilver><TotalIntegral></TotalIntegral><SilverLevel></SilverLevel><ColorLevel></ColorLevel><IntegralLevel></IntegralLevel><VIPLevel></VIPLevel><UserId></UserId>");
                    }

                    sb.Append("</UserInfo>");
                }

                sb.Append("</LoginInfo>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<LoginInfo>");
                sb.AppendFormat("<IsLogined>{0}</IsLogined>", false);
                sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", ex.Message);
                sb.Append("</LoginInfo>");

                return sb.ToString();
            }
        }

        public string Register(string username, string password, string nickname)
        {
            StringBuilder sb = null;
            try
            {
                if (!Regex.IsMatch(password, PasswordExpression))
                {
                    sb = new StringBuilder();
                    sb.Append("<RegisterInfo>");
                    sb.AppendFormat("<IsRegistered>{0}</IsRegistered>", false);
                    sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "密码必须是由数字或字母组成的字符串，且最小6位，最大30位！");
                    sb.Append("</RegisterInfo>");
                    return sb.ToString();
                }
                if (!Regex.IsMatch(username, @"^(\d+){11}$"))
                {
                    sb = new StringBuilder();
                    sb.Append("<RegisterInfo>");
                    sb.AppendFormat("<IsRegistered>{0}</IsRegistered>", false);
                    sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "注册失败，原因：用户名应是您的手机号！");
                    sb.Append("</RegisterInfo>");
                    return sb.ToString();
                }

                MembershipUser user = null;

                using (TransactionScope scope = new TransactionScope())
                {
                    user = Membership.GetUser(username);
                    if (user != null)
                    {
                        sb = new StringBuilder();
                        sb.Append("<RegisterInfo>");
                        sb.AppendFormat("<IsRegistered>{0}</IsRegistered>", false);
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "注册失败，已存在用户：" + username + "，请换一个账号再重试");
                        sb.Append("</RegisterInfo>");
                        return sb.ToString();
                    }
                    user = Membership.CreateUser(username, password);
                    if (user == null)
                    {
                        sb = new StringBuilder();
                        sb.Append("<RegisterInfo>");
                        sb.AppendFormat("<IsRegistered>{0}</IsRegistered>", false);
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "注册失败，请检查：密码正确格式应由数字或字母组成的字符串，且最小6位，最大30位");
                        sb.Append("</RegisterInfo>");
                        return sb.ToString();
                    }

                    Roles.AddUserToRole(username, "Users");

                    UserBaseInfo ubModel = new UserBaseInfo();
                    ubModel.Nickname = nickname.Trim();
                    ubModel.UserId = Guid.Parse(user.ProviderUserKey.ToString());
                    ubModel.HeadPicture = "";
                    ubModel.Sex = "";
                    ubModel.MobilePhone = username;
                    ubModel.TotalGold = 0;
                    ubModel.TotalSilver = 0;
                    ubModel.TotalSilver = 0;
                    ubModel.SilverLevel = 0;
                    ubModel.ColorLevel = 0;
                    ubModel.IntegralLevel = 0;
                    ubModel.VIPLevel = "V0";
                    UserBase ubBll = new UserBase();
                    ubBll.Insert(ubModel);

                    scope.Complete();
                }

                sb = new StringBuilder();
                sb.Append("<RegisterInfo>");
                sb.AppendFormat("<IsRegistered>{0}</IsRegistered>", true);
                sb.AppendFormat("<UserId>{0}</UserId>", user.ProviderUserKey);
                sb.Append("</RegisterInfo>");

                return sb.ToString();

            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                sb = new StringBuilder();
                sb.Append("<RegisterInfo>");
                sb.AppendFormat("<IsRegistered>{0}</IsRegistered>", false);
                sb.Append("<ErrorMsg> 异常：" + ex.Message + "</ErrorMsg>");
                sb.Append("</RegisterInfo>");
                return sb.ToString();
            }
        }

        public string GetUserInfo(string username)
        {
            try
            {
                MembershipUser user = Membership.GetUser(username);
                if (user == null) return "";
                UserBase bll = new UserBase();
                var model = bll.GetModel(user.ProviderUserKey);
                if (model == null) return "";
                return string.Format(@"<Rsp><Nickname>{0}</Nickname><HeadImg>{1}</HeadImg><Sex>{2}</Sex><MobilePhone>{3}</MobilePhone><TotalGold>{4}</TotalGold><TotalSilver>{5}</TotalSilver><TotalIntegral>{6}</TotalIntegral><SilverLevel>{7}</SilverLevel><ColorLevel>{8}</ColorLevel><IntegralLevel>{9}</IntegralLevel><VIPLevel>{10}</VIPLevel><UserId>{11}</UserId></Rsp>", model.Nickname, WebSiteHost_Team + model.HeadPicture, model.Sex, model.MobilePhone, model.TotalGold, model.TotalSilver, model.TotalIntegral, model.SilverLevel,model.ColorLevel,model.IntegralLevel,model.VIPLevel, model.UserId);
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                return "";
            }
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

        public string GetRandomNumber(string prefix)
        {
            try
            {
                return new OrderCode().GetOrderCode(prefix);

                //RandomGenerator rgBll = new RandomGenerator();
                //string rndCode = rgBll.ReceiveFromQueue(20);
                //if (!string.IsNullOrWhiteSpace(rndCode))
                //{
                //    string randomGeneratorFilePath = ConfigurationManager.AppSettings["RandomGeneratorFilePath"];
                //    rgBll.WriteToReceiveFile(randomGeneratorFilePath, rndCode);
                //}
                //return rgBll.ReceiveFromQueue(20);
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public string ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                Regex r = new Regex(PasswordExpression);
                if (!r.IsMatch(oldPassword) || !r.IsMatch(newPassword))
                {
                    sb.Append("<Rsp>");
                    sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                    sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "密码必须是由数字或字母组成的字符串，且最小6位，最大30位！");
                    sb.Append("</Rsp>");
                    return sb.ToString();
                }

                MembershipUser user = Membership.GetUser(username);
                if (user == null)
                {
                    sb.Append("<Rsp>");
                    sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                    sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "用户名不存在！");
                    sb.Append("</Rsp>");
                    return sb.ToString();
                }
                if (!Membership.ValidateUser(username, oldPassword))
                {
                    sb.Append("<Rsp>");
                    sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                    if (user.IsLockedOut)
                    {
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "您的账号已被锁定，请联系管理员先解锁后才能登录！");
                    }
                    else if (!user.IsApproved)
                    {
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "您的帐户尚未获得批准。您无法登录，直到管理员批准您的帐户！");
                    }
                    else
                    {
                        sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "原密码输入不正确，请检查！");
                    }

                    sb.Append("</Rsp>");
                    return sb.ToString();
                }

                if (!user.ChangePassword(oldPassword, newPassword))
                {
                    sb.Append("<Rsp>");
                    sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                    sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "修改密码失败，请正确输入并重试");
                    sb.Append("</Rsp>");
                    return sb.ToString();
                }

                sb.Append("<Rsp>");
                sb.AppendFormat("<IsOk>{0}</IsOk>", true);
                sb.Append("<ErrorMsg></ErrorMsg>");
                sb.Append("</Rsp>");
                return sb.ToString();

            }
            catch (Exception ex)
            {
                new CustomException(string.Format("服务-接口：string ChangePassword(string username, string oldPassword, string newPassword)：异常：{0}", ex.Message), ex);
                StringBuilder sb = new StringBuilder();
                sb.Append("<Rsp>");
                sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", ex.Message);
                sb.Append("</Rsp>");
                return sb.ToString();
            }
        }

        public string UpdatePassword(string username, string newPassword)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                Regex r = new Regex(PasswordExpression);
                if (!r.IsMatch(newPassword))
                {
                    sb.Append("<Rsp>");
                    sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                    sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "密码必须是由数字或字母组成的字符串，且最小6位，最大30位！");
                    sb.Append("</Rsp>");
                    return sb.ToString();
                }

                MembershipUser user = Membership.GetUser(username);
                if (user == null)
                {
                    sb.Append("<Rsp>");
                    sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                    sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "用户名不存在！");
                    sb.Append("</Rsp>");
                    return sb.ToString();
                }

                string oldPassword = user.ResetPassword();
                if (!user.ChangePassword(oldPassword, newPassword))
                {
                    sb.Append("<Rsp>");
                    sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                    sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", "修改密码失败，请正确输入并重试");
                    sb.Append("</Rsp>");
                    return sb.ToString();
                }

                sb.Append("<Rsp>");
                sb.AppendFormat("<IsOk>{0}</IsOk>", true);
                sb.Append("<ErrorMsg></ErrorMsg>");
                sb.Append("</Rsp>");
                return sb.ToString();
            }
            catch (MembershipPasswordException ex)
            {
                new CustomException(ex.Message, ex);
                StringBuilder sb = new StringBuilder();
                sb.Append("<Rsp>");
                sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", ex.Message);
                sb.Append("</Rsp>");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
                StringBuilder sb = new StringBuilder();
                sb.Append("<Rsp>");
                sb.AppendFormat("<IsOk>{0}</IsOk>", false);
                sb.AppendFormat("<ErrorMsg>{0}</ErrorMsg>", ex.Message);
                sb.Append("</Rsp>");
                return sb.ToString();
            }
        }

    }
}
