using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.SysHelper
{
    public class EnumData
    {
        /// <summary>
        /// 设备 平台
        /// </summary>
        public enum Platform : byte { PC, Android, IOS }

        /// <summary>
        /// 原、大、中、小缩略图
        /// </summary>
        public enum PictureType : byte { OriginalPicture, BPicture, MPicture, SPicture }

        /// <summary>
        /// 金币元宝
        /// </summary>
        public enum ErnieItemNumType:byte { 金币, 元宝, 倍数 }

        /// <summary>
        /// 活动操作类型
        /// </summary>
        public enum ActivityType : byte { Vote, SignUp }

        public enum UploadSaveDir : byte { ActivityPhotoPicture }
    }
}
