using System;

namespace TygaSoft.Model
{
    public partial class CategoryInfo
    {
        public string PictureUrl { get; set; }

        public string FileDirectory { get; set; }
        public string RandomFolder { get; set; }
        public string FileExtension { get; set; }

        public string OriginalPicture { get; set; }
        public string BPicture { get; set; }
        public string MPicture { get; set; }
        public string SPicture { get; set; }
    }
}
