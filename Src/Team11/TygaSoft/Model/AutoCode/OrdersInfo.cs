using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class OrdersInfo
    {
        public object Id { get; set; }

        public string OrderNum { get; set; }

        public Guid UserId { get; set; }

        public string Receiver { get; set; }

        public string ProviceCity { get; set; }

        public string Address { get; set; }

        public string Mobilephone { get; set; }

        public string Telephone { get; set; }

        public Decimal TotalPrice { get; set; }

        public string PayOption { get; set; }

        public DateTime PayDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
