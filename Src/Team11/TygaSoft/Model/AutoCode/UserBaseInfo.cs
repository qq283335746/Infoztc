using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class UserBaseInfo
    {
        public Guid UserId { get; set; }

        public string Nickname { get; set; }

        public string HeadPicture { get; set; }

        public string Sex { get; set; }

        public string MobilePhone { get; set; }

        public Int32 TotalGold { get; set; }

        public Int32 TotalSilver { get; set; }

        public Int32 TotalIntegral { get; set; }

        public Int32 SilverLevel { get; set; }

        public Int32 ColorLevel { get; set; }

        public Int32 IntegralLevel { get; set; }

        public string VIPLevel { get; set; }
    }
}
