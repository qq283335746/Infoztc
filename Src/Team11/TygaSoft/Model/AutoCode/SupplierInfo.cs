using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class SupplierInfo
    {
        public object Id { get; set; }

        public string SupplierName { get; set; }

        public string Phone { get; set; }

        public string ProvinceCityName { get; set; }

        public string Address { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
