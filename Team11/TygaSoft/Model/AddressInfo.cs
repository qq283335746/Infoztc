using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.Model
{
    [Serializable]
    public class AddressInfo
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string TelPhone { get; set; }
        public string ProvinceCity { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
    }
}
