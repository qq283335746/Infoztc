using System;

namespace TygaSoft.PayWebHelper
{
    [Serializable]
    public class ResResultInfo
    {
        public int ResCode { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }
    }
}
