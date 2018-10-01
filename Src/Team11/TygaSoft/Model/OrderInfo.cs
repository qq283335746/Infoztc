using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.Model
{
    [Serializable]
    public class OrderInfo
    {
        public object OrderId { get; set; }
        public string OrderNum { get; set; }
        public object UserId { get; set; }
        public CreditCardInfo CreditCard { get; set; }
        public AddressInfo ToAddress { get; set; }
        public AddressInfo FromAddress { get; set; }
    }
}
