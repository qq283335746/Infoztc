using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.Model;

namespace TygaSoft.BLL
{
    public class UserLevel
    {
        #region UserLevel

        public void SaveUserLevelByEnumSource(object userId, int funCode, int enumSource, int gold, int silver, int integral)
        {
            UserLevelProduce ulpBll = new UserLevelProduce();
            if (ulpBll.IsExist(userId, funCode, enumSource))
            {
                return;
            }

            #region 用户级别来源明细表

            UserLevelProduceInfo ulpModel = new UserLevelProduceInfo();
            ulpModel.UserId = Guid.Parse(userId.ToString());
            ulpModel.FunCode = funCode;
            ulpModel.EnumSource = enumSource;
            ulpModel.TotalGold = gold;
            ulpModel.TotalSilver = silver;
            ulpModel.TotalIntegral = integral;
            ulpModel.LastUpdatedDate = DateTime.Now;

            ulpBll.Insert(ulpModel);

            #endregion

            #region 用户级别来源统计表

            UserLevelView ulvBll = new UserLevelView();

            var ulvModel = ulvBll.GetModel(userId, funCode, enumSource);
            if (ulvModel == null)
            {
                ulvModel = new UserLevelViewInfo();
                ulvModel.UserId = ulpModel.UserId;
                ulvModel.FunCode = ulpModel.FunCode;
                ulvModel.EnumSource = ulpModel.EnumSource;
                ulvModel.TotalGold = gold;
                ulvModel.TotalSilver = silver;
                ulvModel.TotalIntegral = integral;
                ulvModel.LastUpdatedDate = DateTime.Now;

                ulvBll.Insert(ulvModel);
            }
            else
            {
                ulvModel.TotalGold += gold;
                ulvModel.TotalSilver += silver;
                ulvModel.TotalIntegral += integral;
                ulvModel.LastUpdatedDate = DateTime.Now;

                ulvBll.Update(ulvModel);
            }

            #endregion
        }

        public int GetColorLevel(int gold)
        {
            if (gold < 500) return 0;
            if (gold < 5000) return 1;
            if (gold < 30000) return 2;
            if (gold < 60000) return 3;
            if (gold < 120000) return 4;
            return 0;
        }

        public int GetSilverLevel(int silver)
        {
            if (silver < 1) return 0;
            if (silver < 5) return 1;
            if (silver < 10) return 2;
            if (silver < 20) return 3;
            if (silver < 50) return 4;
            if (silver < 100) return 5;
            if (silver < 200) return 6;
            if (silver < 400) return 7;
            if (silver < 800) return 8;
            if (silver < 2000) return 9;
            return 10;
        }

        #endregion
    }
}
