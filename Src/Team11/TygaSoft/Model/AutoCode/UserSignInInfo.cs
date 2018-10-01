using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class UserSignInInfo
    {
        public Guid Id { get; set; }

        public object UserId { get; set; }

        public string SignInXml { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
