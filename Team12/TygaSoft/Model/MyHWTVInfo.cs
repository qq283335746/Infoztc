using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class MyHWTVInfo
    {
        public object Id { get; set; }

        public string HWTVName { get; set; }

        public object HWTVIcon { get; set; }

        public string ProgramAddress { get; set; }

        public Boolean IsTurnTo { get; set; }

        public Int32 Sort { get; set; }

        public string FileName { get; set; }

        public Int32 FileSize { get; set; }

        public string FileExtension { get; set; }

        public string FileDirectory { get; set; }

        public string RandomFolder { get; set; }

        public string OriginalPicture { get; set; }
        public string BPicture { get; set; }
        public string MPicture { get; set; }
        public string SPicture { get; set; }
    }
}
