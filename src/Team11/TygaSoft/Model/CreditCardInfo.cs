using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.Model
{
    [Serializable]
    public class CreditCardInfo
    {
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
    }
}
