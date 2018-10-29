using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Web.Configuration;
using TygaSoft.BLL;

namespace TygaSoft.WebHelper
{
    public class UploadFilesHelper
    {
        static readonly string FilesRoot = WebConfigurationManager.AppSettings["FilesRoot"];
        static Random rnd;

        public enum FileExtension
        {
            jpg = 255216, gif = 7173, bmp = 6677, png = 13780, xls = 208207, xlsx = 8075, doc = 208207, docx = 8075
            // 255216 jpg;7173 gif;6677 bmp;13780 png; 7790 exe dll; 8297 rar; 6063 xml;6033 html;239187 aspx;117115 cs;119105 js;210187 txt;255254 sql;xls = 208207 
        }

        /// <summary>
        /// 使用文件固定字节法验证上传图片是否合法
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileLen"></param>
        /// <returns></returns>
        public static bool IsPictureValidated(Stream stream, int fileLen)
        {
            if (fileLen == 0) return false;

            //自定义一个数组，包含所有允许上传的文件扩展名
            FileExtension[] fes = { FileExtension.jpg, FileExtension.gif, FileExtension.bmp, FileExtension.png };

            byte[] imgArray = new byte[fileLen];
            stream.Read(imgArray, 0, fileLen);
            MemoryStream ms = new MemoryStream(imgArray);
            BinaryReader br = new BinaryReader(ms);
            string fileBuffer = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileBuffer = buffer.ToString();
                buffer = br.ReadByte();
                fileBuffer += buffer.ToString();
            }
            catch
            {
            }
            br.Close();
            ms.Close();
            foreach (FileExtension item in fes)
            {
                if (Int32.Parse(fileBuffer) == (int)item)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 使用文件固定字节法验证文件是否合法
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileLen"></param>
        /// <returns></returns>
        public static bool IsFileValidated(Stream stream, int fileLen)
        {
            if (fileLen == 0) return false;

            //自定义一个数组，包含所有允许上传的文件扩展名，这里只定义xls扩展名
            FileExtension[] fes = { FileExtension.gif, FileExtension.bmp, FileExtension.jpg, FileExtension.png, FileExtension.xls, FileExtension.xlsx, FileExtension.doc, FileExtension.docx };

            byte[] imgArray = new byte[fileLen];
            stream.Read(imgArray, 0, fileLen);
            MemoryStream ms = new MemoryStream(imgArray);
            BinaryReader br = new BinaryReader(ms);
            string fileBuffer = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileBuffer = buffer.ToString();
                buffer = br.ReadByte();
                fileBuffer += buffer.ToString();
            }
            catch
            {
            }
            br.Close();
            ms.Close();
            foreach (FileExtension item in fes)
            {
                if (Int32.Parse(fileBuffer) == (int)item)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 指定存储目录key，是否生成缩略图，上传文件
        /// 返回所有文件路径，如果是生成缩略图，则包含缩略图文件路径
        /// </summary>
        /// <param name="file"></param>
        /// <param name="key"></param>
        /// <param name="isCreateThumbnail"></param>
        /// <returns></returns>
        public string[] Upload(HttpPostedFile file, string key,bool isCreateThumbnail)
        {
            if (file == null || file.ContentLength == 0)
            {
                throw new ArgumentException("没有获取到任何上传的文件", "file");
            }
            int size = file.ContentLength;
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!IsFileValidated(file.InputStream, size))
            {
                throw new ArgumentException("上传文件不在规定的上传文件范围内");
            }
            if (isCreateThumbnail)
            {
                if ((fileExtension != ".jpg") || (fileExtension != ".jpg"))
                {
                    throw new ArgumentException("创建缩略图只支持.jpg格式的文件，请检查");
                }
            }
            string dir = ConfigHelper.GetValueByKey(key);
            if (string.IsNullOrWhiteSpace(dir))
            {
                throw new ArgumentException("未找到"+key+"的相关配置，请检查", "key");
            }

            string paths = "";

            dir = VirtualPathUtility.AppendTrailingSlash(dir);
            string rndName = CustomsHelper.GetFormatDateTime();
            string fName = rndName + fileExtension;
            string filePath = dir + rndName.Substring(0, 8) + "/";
            string fullPath = HttpContext.Current.Server.MapPath(filePath);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
            file.SaveAs(fullPath + fName);

            paths += filePath + fName;
            if (isCreateThumbnail)
            {
                ImagesHelper ih = new ImagesHelper();
                string[] whBPicture = ConfigHelper.GetValueByKey("BPicture").Split(',');
                string[] whMPicture = ConfigHelper.GetValueByKey("MPicture").Split(',');
                string[] whSPicture = ConfigHelper.GetValueByKey("SPicture").Split(',');
                string bPicturePath = filePath + rndName + "_b" + fileExtension;
                string mPicturePath = filePath + rndName + "_m" + fileExtension;
                string sPicturePath = filePath + rndName + "_s" + fileExtension;
                ih.CreateThumbnailImage(fullPath + fName, HttpContext.Current.Server.MapPath(bPicturePath), int.Parse(whBPicture[0]), int.Parse(whBPicture[1]));
                ih.CreateThumbnailImage(fullPath + fName, HttpContext.Current.Server.MapPath(mPicturePath), int.Parse(whMPicture[0]), int.Parse(whMPicture[1]));
                ih.CreateThumbnailImage(fullPath + fName, HttpContext.Current.Server.MapPath(sPicturePath), int.Parse(whSPicture[0]), int.Parse(whSPicture[1]));
                paths += "," + bPicturePath;
                paths += "," + mPicturePath;
                paths += "," + sPicturePath;
            }
            else
            {
                paths += "," + filePath + fName;
                paths += "," + filePath + fName;
                paths += "," + filePath + fName;
            }

            return paths.Split(',');
        }

        /// <summary>
        /// 上传文件，并返回文件存储的虚拟路径
        /// </summary>
        /// <param name="file"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string UploadOriginalFile(HttpPostedFile file, string key)
        {
            string fileName = file.FileName;
            string dir = string.Format("{0}/{1}", FilesRoot, key);
            string saveVirtualDir = string.Format("{0}/{1}", dir, DateTime.Now.ToString("yyyyMM"));

            string fullPath = HttpContext.Current.Server.MapPath(saveVirtualDir);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

            file.SaveAs(string.Format("{0}\\{1}", fullPath, fileName));

            return string.Format("{0}/{1}", saveVirtualDir, fileName);
        }

        /// <summary>
        /// 获取唯一随机数
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public static string GetRandomFolder(string prefix)
        {
            WebSecurityClient wsClient = new WebSecurityClient();
            return wsClient.GetRandomNumber(prefix);

            //var n = Directory.GetFiles(Path.GetDirectoryName(HttpContext.Current.Server.MapPath(originalUrl))).Length + 1;
            //if (rnd == null) rnd = new Random();
            //string rndNum = string.Format("{0}{1}", n, (int)(rnd.NextDouble() * int.MaxValue));
            //if (rndNum.Length >= 11) return rndNum.Substring(0, 11);

            //return rndNum.PadRight(11, char.Parse(rnd.Next(10).ToString()));
        }

        #region 已废弃

        ///// <summary>
        ///// 上传文件，并返回存储文件的虚拟路径，该虚拟路径包含根操作符（代字号 [~]）
        ///// </summary>
        ///// <param name="file"></param>
        ///// <param name="dirName"></param>
        ///// <returns></returns>
        //public string Upload(HttpPostedFile file, string dirName)
        //{
        //    if (file == null || file.ContentLength == 0)
        //    {
        //        throw new ArgumentException("没有获取到任何上传的文件", "file");
        //    }
        //    dirName = "" + dirName.Trim('/') + "/";
        //    int size = file.ContentLength;
        //    string fileExtension = Path.GetExtension(file.FileName).ToLower();
        //    if (!IsFileValidated(file.InputStream, size))
        //    {
        //        throw new ArgumentException("上传文件不在规定的上传文件范围内");
        //    }
        //    uploadRoot = VirtualPathUtility.AppendTrailingSlash(uploadRoot);
        //    string fileUrl = string.Empty;
        //    string fName = CustomsHelper.GetFormatDateTime();
        //    string saveVirtualPath = uploadRoot + dirName + fName.Substring(0, 8) + "/";
        //    string fullPath = HttpContext.Current.Server.MapPath(saveVirtualPath);
        //    if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
        //    fullPath += fName + fileExtension;
        //    file.SaveAs(fullPath);

        //    fileUrl = saveVirtualPath + fName + fileExtension;
        //    return fileUrl;
        //}

        ///// <summary>
        ///// 上传文件，并返回存储文件的虚拟路径，该虚拟路径包含根操作符（代字号 [~]）
        ///// </summary>
        ///// <param name="file"></param>
        ///// <param name="fileName"></param>
        ///// <param name="dirName"></param>
        ///// <returns></returns>
        //public string Upload(HttpPostedFile file, string fileName, string dirName)
        //{
        //    if (file == null || file.ContentLength == 0)
        //    {
        //        throw new ArgumentException("没有获取到任何上传的文件", "file");
        //    }
        //    dirName = "" + dirName.Trim('/') + "/";
        //    int size = file.ContentLength;
        //    string fileExtension = Path.GetExtension(file.FileName).ToLower();
        //    if (!IsFileValidated(file.InputStream, size))
        //    {
        //        throw new ArgumentException("上传文件不在规定的上传文件范围内");
        //    }
        //    uploadRoot = VirtualPathUtility.AppendTrailingSlash(uploadRoot);
        //    string fileUrl = string.Empty;
        //    string fName = string.IsNullOrEmpty(fileName) ? CustomsHelper.GetFormatDateTime() : fileName;
        //    string saveVirtualPath = uploadRoot + dirName + fName.Substring(0, 8) + "/";
        //    string fullPath = HttpContext.Current.Server.MapPath(saveVirtualPath);
        //    if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
        //    fullPath += fName + fileExtension;
        //    file.SaveAs(fullPath);

        //    fileUrl = saveVirtualPath + fName + fileExtension;
        //    return fileUrl;
        //}

        ///// <summary>
        ///// 上传文件到临时存储，并返回存储文件的虚拟路径，该虚拟路径包含根操作符（代字号 [~]）
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //public string UploadToTemp(HttpPostedFile file)
        //{
        //    if (file == null || file.ContentLength == 0)
        //    {
        //        throw new ArgumentException("没有获取到任何上传的文件", "file");
        //    }
        //    int size = file.ContentLength;
        //    string fileExtension = Path.GetExtension(file.FileName).ToLower();
        //    if (!IsFileValidated(file.InputStream, size))
        //    {
        //        throw new ArgumentException("上传文件不在规定的上传文件范围内");
        //    }

        //    uploadRoot = VirtualPathUtility.AppendTrailingSlash(uploadRoot);
        //    string dirName = "Temp/";

        //    string[] childDirs = Directory.GetDirectories(HttpContext.Current.Server.MapPath(uploadRoot + dirName));
        //    foreach (string item in childDirs)
        //    {
        //        DirectoryInfo di = new DirectoryInfo(item);
        //        TimeSpan ts = DateTime.Now - di.CreationTime;
        //        if (ts.Days > 2)
        //        {
        //            Directory.Delete(item, true);
        //        }
        //    }

        //    string fName = CustomsHelper.GetFormatDateTime();
        //    string fileUrl = string.Empty;
        //    string saveVirtualPath = uploadRoot + dirName + fName.Substring(0,8) + "";
        //    string fullPath = HttpContext.Current.Server.MapPath(saveVirtualPath);
        //    if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
        //    fullPath += "/" + fName + fileExtension;
        //    file.SaveAs(fullPath);

        //    fileUrl = saveVirtualPath + "/" + fName + fileExtension;
        //    return fileUrl;
        //}

        ///// <summary>
        ///// 将临时存储文件转移到商品文件，并返回存储文件的虚拟路径，该虚拟路径包含根操作符（代字号 [~]）
        ///// </summary>
        ///// <param name="tempVirtualPath"></param>
        ///// <returns></returns>
        //public string FromTempToProduct(string tempVirtualPath)
        //{
        //    uploadRoot = VirtualPathUtility.AppendTrailingSlash(uploadRoot);
        //    string pVirtualPath = uploadRoot + "Product/" + CustomsHelper.CreateDateTimeString().Substring(0, 6) + "/";
        //    return TempFileToUseFile(tempVirtualPath, pVirtualPath);
        //}

        ///// <summary>
        ///// 将临时文件转移到实际应用文件位置存储，并返回存储文件的虚拟路径，该虚拟路径包含根操作符（代字号 [~]）
        ///// </summary>
        ///// <param name="tempVirtualPath"></param>
        ///// <param name="useVirtualPath"></param>
        ///// <returns></returns>
        //private string TempFileToUseFile(string tempVirtualPath, string useVirtualPath)
        //{
        //    string tempPath = HttpContext.Current.Server.MapPath(tempVirtualPath);
        //    if (!File.Exists(tempPath))
        //    {
        //        throw new ArgumentException("文件不存在", "tempVirtualPath");
        //    }
        //    string fileName = VirtualPathUtility.GetFileName(tempVirtualPath);
        //    string usePath = HttpContext.Current.Server.MapPath(useVirtualPath);
        //    string useDir = Path.GetDirectoryName(usePath);
        //    if (!Directory.Exists(useDir))
        //    {
        //        Directory.CreateDirectory(useDir);
        //    }
        //    string fullPath = Path.Combine(useDir, fileName);
        //    File.Move(tempPath, fullPath);

        //    return useVirtualPath.Replace("~","") + fileName;
        //}

        ///// <summary>
        ///// 创建商品缩略图
        ///// 返回值：主图（220*220）、大图（800*800）、中图（350*350）、小图（50*50）
        ///// </summary>
        ///// <param name="virtualPath">商品主图片的路径，该路径是带有“~”符号的路径</param>
        ///// <returns></returns>
        //public string[] GetThumbnailImages(string virtualPath)
        //{
        //    if (string.IsNullOrEmpty(virtualPath))
        //    {
        //        return null;
        //    }
        //    context = HttpContext.Current;
        //    string siteRootPath = context.Server.MapPath("~");
        //    string fullPath = context.Server.MapPath(virtualPath);
        //    string dir = Path.GetDirectoryName(fullPath);
        //    string fName = Path.GetFileNameWithoutExtension(fullPath);
        //    string fExtension = Path.GetExtension(fullPath).ToLower();
        //    string newDir = dir + "\\" + fName;
        //    if (!Directory.Exists(newDir))
        //    {
        //        Directory.CreateDirectory(newDir);
        //    }
        //    ImagesHelper ih = new ImagesHelper();
        //    //创建220*220 主图
        //    string sImages = newDir + "\\" + CustomsHelper.GetFormatDateTime() + fExtension;
        //    ih.CreateThumbnailImage(fullPath, sImages, 220, 220);
        //    sImages = sImages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
        //    //创建800*800 大图
        //    string sLImages = newDir + "\\" + CustomsHelper.GetFormatDateTime() + fExtension;
        //    ih.CreateThumbnailImage(fullPath, sLImages, 800, 800);
        //    sLImages = sLImages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
        //    //创建350*350 中图
        //    string sMimages = newDir + "\\" + CustomsHelper.GetFormatDateTime() + fExtension;
        //    ih.CreateThumbnailImage(fullPath, sMimages, 350, 350);
        //    sMimages = sMimages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
        //    //创建50*50 小图
        //    string sSimages = newDir + "\\" + CustomsHelper.GetFormatDateTime() + fExtension;
        //    ih.CreateThumbnailImage(fullPath, sSimages, 50, 50);
        //    sSimages = sSimages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");

        //    string[] images = {sImages, sLImages, sMimages, sSimages };
        //    return images;
        //}

        ///// <summary>
        ///// 创建商品缩略图
        ///// 返回值：主图（220*220）、大图（800*800）、中图（350*350）、小图（50*50）
        ///// </summary>
        ///// <param name="virtualPath"></param>
        ///// <param name="mainFileName"></param>
        ///// <returns></returns>
        //public string[] GetThumbnailImages(string virtualPath,string mainFileName)
        //{
        //    if (string.IsNullOrEmpty(virtualPath))
        //    {
        //        return null;
        //    }
        //    context = HttpContext.Current;
        //    string siteRootPath = context.Server.MapPath("~");
        //    string fullPath = context.Server.MapPath(virtualPath);
        //    string dir = Path.GetDirectoryName(fullPath);
        //    string fExtension = Path.GetExtension(fullPath).ToLower();
        //    string newDir = dir + mainFileName;
        //    if (!Directory.Exists(newDir))
        //    {
        //        Directory.CreateDirectory(newDir);
        //    }
        //    ImagesHelper ih = new ImagesHelper();
        //    //创建220*220 主图
        //    string sImages = newDir + CustomsHelper.GetFormatDateTime() + fExtension;
        //    ih.CreateThumbnailImage(fullPath, sImages, 220, 220);
        //    sImages = sImages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
        //    //创建800*800 大图
        //    string sLImages = newDir + CustomsHelper.GetFormatDateTime() + fExtension;
        //    ih.CreateThumbnailImage(fullPath, sLImages, 800, 800);
        //    sLImages = sLImages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
        //    //创建350*350 中图
        //    string sMimages = newDir + CustomsHelper.GetFormatDateTime() + fExtension;
        //    ih.CreateThumbnailImage(fullPath, sMimages, 350, 350);
        //    sMimages = sMimages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
        //    //创建50*50 小图
        //    string sSimages = newDir + CustomsHelper.GetFormatDateTime() + fExtension;
        //    ih.CreateThumbnailImage(fullPath, sMimages, 50, 50);
        //    sSimages = sSimages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");

        //    string[] images = {sImages, sLImages, sMimages, sSimages };
        //    return images;
        //}

        ///// <summary>
        ///// 删除当前商品图片,并删除由当前图片创建的缩略图
        ///// </summary>
        ///// <param name="virtualPath"></param>
        //public void DeleteProductImage(string virtualPath)
        //{
        //    context = HttpContext.Current;
        //    if (string.IsNullOrEmpty(virtualPath)) return;
        //    string fullPath = context.Server.MapPath(virtualPath);
        //    string fName = Path.GetFileNameWithoutExtension(fullPath);
        //    string dir = Path.GetDirectoryName(fullPath);
        //    Directory.Delete(dir + "\\" + fName, true);
        //    File.Delete(fullPath);
        //}

        //public string GetProductImgMain(string virtualPath)
        //{
        //    if (string.IsNullOrEmpty(virtualPath)) return string.Empty;
        //    context = HttpContext.Current;
        //    string fullPath = context.Server.MapPath(virtualPath);
        //    string dir = Path.GetDirectoryName(fullPath);
        //    dir = dir + "\\" + Path.GetFileNameWithoutExtension(fullPath);
        //    string[] files = Directory.GetFiles(dir);
        //    foreach (string item in files)
        //    {
        //        Bitmap bmp = new Bitmap(item);
        //        int width = bmp.Width;
        //        int height = bmp.Height;

        //        if (width == 220 && height == 220)
        //        {
        //            return item.Replace(context.Server.MapPath("~"), "/");
        //        }
        //    }
        //    return string.Empty;
        //}

        ///// <summary>
        ///// 将临时文件移到正式使用的位置
        ///// </summary>
        ///// <param name="sourceFileName"></param>
        ///// <param name="destFileName"></param>
        //public string TempFileMoveToUseFile(string tempPath, string replaceTempName)
        //{
        //    if (string.IsNullOrEmpty(tempPath)) return "";
        //    tempPath = HttpContext.Current.Server.MapPath(tempPath);
        //    string fileName = Path.GetFileName(tempPath);
        //    string directoryName = Path.Combine(ConfigHelper.GetFullPath("UploadFilesSavePath"), replaceTempName);
        //    if (!Directory.Exists(directoryName))
        //    {
        //        Directory.CreateDirectory(directoryName);
        //    }
        //    string fileUrl = Path.Combine(directoryName, fileName);
        //    File.Move(tempPath, fileUrl);

        //    return fileUrl.Replace(HttpContext.Current.Server.MapPath("~"), "~/").Trim().Replace(@"\", @"/");
        //}

        ///// <summary>
        ///// 返回临时文件存放位置
        ///// </summary>
        ///// <param name="fileExtension"></param>
        ///// <returns></returns>
        //public string GetFileTempUrl(string fileExtension)
        //{
        //    string sDay = DateTime.Now.Day.ToString();
        //    string dir = ConfigHelper.GetFullPath("UploadFilesSavePath");
        //    dir = Path.Combine(dir, "temp");
        //    string cDir = Path.Combine(dir, sDay);
        //    if (!Directory.Exists(cDir))
        //    {
        //        Directory.CreateDirectory(cDir);
        //    }
        //    string[] childDirs = Directory.GetDirectories(dir);
        //    foreach (string item in childDirs)
        //    {
        //        if (item != (cDir))
        //        {
        //            Directory.Delete(item, true);
        //        }
        //    }
        //    string fileName = CustomsHelper.GetFormatDateTime() + fileExtension.ToLower();
        //    string fileUrl = Path.Combine(dir,sDay,fileName);
        //    return fileUrl.Replace(HttpContext.Current.Server.MapPath("~"), "~/").Trim().Replace(@"\", @"/");
        //}

        #endregion

    }
}
