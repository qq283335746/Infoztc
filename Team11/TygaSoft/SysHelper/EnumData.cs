using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.SysHelper
{
    public class EnumData
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        public enum Platform : byte { PC, Android, IOS }

        /// <summary>
        /// 图片缩略图类型（原，大，中，小）
        /// </summary>
        public enum PictureType : byte { OriginalPicture, BPicture, MPicture, SPicture }

        /// <summary>
        /// 级别类型（元宝，金币，等级，颜色显示）
        /// </summary>
        public enum LevelType : byte { SilverLevel = 1, VIPLevel = 2, GoldLevel = 3, ColorLevel = 4 }

        /// <summary>
        /// 关于站点（关于我们）
        /// </summary>
        public enum AboutSite : byte { AboutUs = 1}

        /// <summary>
        /// 功能模块代码（其他，广告，彩票，服务，商城，资讯，电视，话题，活动）
        /// </summary>
        public enum FunCode : byte { other = 0, sygg = 1, cp = 2, fw = 3, sc = 4, zx = 5, ds = 6, ht = 7, gg = 8, dt = 9, hd = 10 }

        /// <summary>
        /// 用户级别玩法、来源（其他，鼓励：每个功能模块1金币/人/天）
        /// </summary>
        public enum UserLevelSource : byte { other = 0, Encourage = 1  }
    }
}
