using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.CustomExceptions;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.SysHelper;

namespace TygaSoft.WcfService
{
    public partial class HnztcQueueService : IUserBaseQueue,IAccessStatisticQueue
    {
        #region IUserBaseQueue Member

        public void SaveUserLevel(UserLevelInfo userLevelInfo)
        {
            try
            {
                if (userLevelInfo.UserId.Equals(Guid.Empty))
                    throw new ArgumentException("服务-接口：void SaveUserLevel(UserLevelInfo userLevelInfo)，异常：参数UserId【" + userLevelInfo.UserId + "】值不正确");

                UserBase bll = new UserBase();
                var model = bll.GetModel(userLevelInfo.UserId);
                if (model == null)
                    throw new ArgumentException("服务-接口：void SaveUserLevel(UserLevelInfo userLevelInfo)，异常：用户【" + userLevelInfo.UserId + "】基本信息不存在");
                if (userLevelInfo.IsReduce)
                {
                    if (userLevelInfo.TotalGold > model.TotalGold)
                    {
                        throw new ArgumentException("服务-接口：void SaveUserLevel(UserLevelInfo userLevelInfo)，异常：消费的金币值【" + userLevelInfo.TotalGold + "】大于现有值【" + model.TotalGold + "】");
                    }
                    model.TotalGold = model.TotalGold - userLevelInfo.TotalGold;

                    if (userLevelInfo.TotalSilver > model.TotalSilver)
                    {
                        throw new ArgumentException("服务-接口：void SaveUserLevel(UserLevelInfo userLevelInfo)，异常：消费的元宝值【" + userLevelInfo.TotalSilver + "】大于现有值【" + model.TotalSilver + "】");
                    }
                    model.TotalSilver = model.TotalSilver - userLevelInfo.TotalSilver;

                    if (userLevelInfo.TotalIntegral > model.TotalIntegral)
                    {
                        throw new ArgumentException("服务-接口：void SaveUserLevel(UserLevelInfo userLevelInfo)，异常：消费的积分值【" + userLevelInfo.TotalIntegral + "】大于现有值【" + model.TotalIntegral + "】");
                    }
                    model.TotalIntegral = model.TotalIntegral - userLevelInfo.TotalIntegral;
                }
                else
                {
                    model.TotalGold = model.TotalGold + userLevelInfo.TotalGold;
                    model.TotalSilver = model.TotalSilver + userLevelInfo.TotalSilver;
                    model.TotalIntegral = model.TotalIntegral + userLevelInfo.TotalIntegral;
                }

                UserLevel ulBll = new UserLevel();

                if (userLevelInfo.EnumSource == (int)EnumData.UserLevelSource.Encourage)
                {
                    EnumHelper eh = new EnumHelper();
                    if (!eh.IsExistValue(typeof(EnumData.FunCode), userLevelInfo.FunCode)) return;

                    ulBll.SaveUserLevelByEnumSource(userLevelInfo.UserId, userLevelInfo.FunCode, userLevelInfo.EnumSource, userLevelInfo.TotalGold, userLevelInfo.TotalSilver, 
                        userLevelInfo.TotalIntegral);
                }

                model.ColorLevel = ulBll.GetColorLevel(model.TotalGold);
                model.SilverLevel = ulBll.GetSilverLevel(model.TotalSilver);

                bll.Update(model);
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
            }
        }

        #endregion

        #region IAccessStatisticQueue Member

        public void SaveAccessStatistic(AccessStatisticInfo accessStatisticInfo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accessStatisticInfo.TableName))
                {
                    throw new ArgumentException("参数TableName不能为空字符串", "TableName");
                }
                switch (accessStatisticInfo.TableName)
                {
                    case "Advertisement":
                        Advertisement adBll = new Advertisement();
                        adBll.UpdateViewCount(accessStatisticInfo.Id);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
            }
        }

        #endregion
    }
}
