using System;
using System.Globalization;

namespace TygaSoft.WebHelper
{
    public class CustomsHelper
    {
        /// <summary>
        /// 根据时间创建字符串
        /// </summary>
        /// <returns></returns>
        public static string CreateDateTimeString()
        {
            //确保产生的字符串唯一性，使用线程休眠
            System.Threading.Thread.Sleep(2);
            Random random = new System.Random(); ;
            return DateTime.Now.ToString("yyyyMMddHHmmssffff", DateTimeFormatInfo.InvariantInfo) + random.Next(0, 9999).ToString().PadLeft(4, '0');
        }

        /// <summary>
        /// 根据时间创建字符串
        /// </summary>
        /// <returns></returns>
        public static string GetFormatDateTime()
        {
            //确保产生的字符串唯一性，使用线程休眠
            System.Threading.Thread.Sleep(1);
            return DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
        }
    }
}
