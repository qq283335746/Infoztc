using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TygaSoft.BLL
{
    public class OrderCode
    {
        #region OrderCode Member

        public string GetOrderCode(string prefix)
        {
            OrderRandom o = new OrderRandom();
            var orderCode = string.Empty;

            Monitor.Enter(o);

            orderCode = o.GetOrderCode(prefix);

            Monitor.Exit(o);

            return orderCode;
        }

        #endregion
    }
}
