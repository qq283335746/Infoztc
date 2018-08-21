using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.WebHelper
{
    /// <summary>
    /// 概率百分比
    /// </summary>
    public class GLBfb
    {
        List<string> _sjsList = new List<string>();

        /// <summary>
        /// 百分比
        /// </summary>
        public int Bfb { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        public List<string> SjsList
        {
            get { return _sjsList; }

            set { _sjsList = value; }
        }

        /// <summary>
        /// 比较数字的上限
        /// </summary>
        public int StartNumber { get; set; }

        /// <summary>
        /// 比较数字的下限
        /// </summary>
        public int EndNumber { get; set; }
    }
}
