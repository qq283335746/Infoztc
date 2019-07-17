using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.SysHelper
{
    public class EnumHelper
    {
        public bool IsExistValue(Type enumType, int value)
        {
            return !string.IsNullOrEmpty(Enum.GetName(enumType, value));
        }

        public int GetValue(Type enumType,string name,int defaultValue)
        {
            var names = Enum.GetNames(enumType);
            foreach (var s in names)
            {
                if (s == name)
                {
                    return int.Parse(((byte)Enum.Parse(enumType, name,true)).ToString());
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 内容类型
        /// </summary>
        public enum ContentType 
        {
            Announcement,Notice,AdvertisementCategory,AdvertisementPosition
        }

        /// <summary>
        /// 广告作用类型
        /// </summary>
        public enum AdvertisementCategory 
        {
            LinkToProduct, LinkToOutSite,ImageText
        }

        /// <summary>
        /// 广告放置位置
        /// </summary>
        public enum AdvertisementPosition
        {
            InTodaySpecial, InTenPrice
        }

        /// <summary>
        /// 商城菜单根编号
        /// </summary>
        public enum ShopMenu
        {
            CustomMenu
        }
    }
}
