using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace TygaSoft.PayWebHelper
{
    public class ResResult
    {
        public string ResJsonString(bool isOk, string msg, params object[] data)
        {
            return JsonConvert.SerializeObject(new ResResultInfo { ResCode = isOk ? 1000 : 1001, Msg = msg, Data = data == null ? "" : data[0] });
        }

        public string ResJsonString(ResResultInfo model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
