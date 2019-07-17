using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace TygaSoft.WebHelper
{
    public class PictureHelper
    {
        public string UploadImageFileByBase64(string userID, string imageBase64, string format, string relativePath)
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings["UploadFilePath"].ToString();
            string error = null;
            MemoryStream inputStream = null;

            #region 验证

            if (String.IsNullOrWhiteSpace(imageBase64))
            {
                error = "图片不能为空";
            }
            else
            {
                byte[] arr = Convert.FromBase64String(imageBase64);
                inputStream = new MemoryStream(arr);

                string validExtensions = ".jpg|.jpeg|.gif|.bmp|.png|";

                if (String.IsNullOrWhiteSpace(format))
                {
                    error = "图片名称无效";
                }
                if (validExtensions.IndexOf(format + "|") < 0)
                {
                    error = "图片类型无效";
                }
                if (inputStream.Length > 5 * 1024 * 1024 || inputStream.Length <= 0)
                {
                    error = "请上传小于5M的图片";
                }
            }
            if (error != null) throw new Exception(error);

            #endregion

            relativePath = relativePath + DateTime.Now.ToString("yyyyMMdd") + "\\";
            string path = filePath + relativePath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            relativePath = relativePath + userID + Guid.NewGuid().ToString() + ORIGINAL_IMAGE_NAMEPART + format;
            string savepath = filePath + relativePath;

            System.Drawing.Image img = System.Drawing.Image.FromStream(inputStream);

            MemoryStream ms = new MemoryStream();
            img.Save(ms, Path.GetExtension(savepath).ToLower() == ".gif" ? System.Drawing.Imaging.ImageFormat.Gif : System.Drawing.Imaging.ImageFormat.Jpeg);
            ms.Flush();

            #region 生成大中小图

            MakeSmallImg(ms, ConvertImageNamePart(savepath, ImageNamePartType.Original), img.Width, img.Height);
            MakeSmallImg(ms, ConvertImageNamePart(savepath, ImageNamePartType.Big), 350, 350);
            MakeSmallImg(ms, ConvertImageNamePart(savepath, ImageNamePartType.Mid), 150, 150);
            MakeSmallImg(ms, ConvertImageNamePart(savepath, ImageNamePartType.Small), 60, 60);

            #endregion

            return ConvertImageNamePart(relativePath, ImageNamePartType.Small);

        }

        public string UploadImageFileByBase64(string imageBase64, string format, string relativePath, string fileName, Dictionary<string, TemplateWH> dicTemplate)
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings["UploadFilePath"].ToString();
            string error = null;
            MemoryStream inputStream = null;

            #region 验证

            if (String.IsNullOrWhiteSpace(imageBase64))
            {
                error = "-100";
            }
            else
            {
                byte[] arr = Convert.FromBase64String(imageBase64);
                inputStream = new MemoryStream(arr);

                string validExtensions = ".jpg|.jpeg|.gif|.bmp|.png|";

                if (String.IsNullOrWhiteSpace(format))
                {
                    error = "-101"; //名称错误
                }
                if (validExtensions.IndexOf(format + "|") < 0)
                {
                    error = "-102"; //类型错误
                }
                if (inputStream.Length > 5 * 1024 * 1024 || inputStream.Length <= 0)
                {
                    error = "-103"; //大小超过限制
                }
            }
            if (error != null) throw new Exception(error);
            #endregion

            relativePath = relativePath + DateTime.Now.ToString("yyyyMMdd") + "\\";
            string path = filePath + relativePath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            relativePath = relativePath + fileName + Guid.NewGuid().ToString() + ORIGINAL_IMAGE_NAMEPART + format;
            string savepath = filePath + relativePath;

            System.Drawing.Image img = System.Drawing.Image.FromStream(inputStream);

            MemoryStream ms = new MemoryStream();
            img.Save(ms, Path.GetExtension(savepath).ToLower() == ".gif" ? System.Drawing.Imaging.ImageFormat.Gif : System.Drawing.Imaging.ImageFormat.Jpeg);
            ms.Flush();

            #region 生成大中小图

            MakeSmallImg(ms, ConvertImageNamePart(savepath, ImageNamePartType.Original), img.Width, img.Height);
            MakeSmallImg(ms, ConvertImageNamePart(savepath, ImageNamePartType.Big), dicTemplate["Big"].templateWidth, dicTemplate["Big"].templateHeight);
            MakeSmallImg(ms, ConvertImageNamePart(savepath, ImageNamePartType.Mid), dicTemplate["Mid"].templateWidth, dicTemplate["Mid"].templateHeight);
            MakeSmallImg(ms, ConvertImageNamePart(savepath, ImageNamePartType.Small), dicTemplate["Small"].templateWidth, dicTemplate["Small"].templateHeight);
            #endregion

            return ConvertImageNamePart(relativePath, ImageNamePartType.Small);

        }

        #region 各种图名称及其转换

        //原图名
        public const string ORIGINAL_IMAGE_NAMEPART = "__original__head";
        //大图名
        public const string BIG_IMAGE_NAMEPART = "__big__head";
        //中图名
        public const string MID_IMAGE_NAMEPART = "__mid__head";
        //小图名
        public const string SMALL_IMAGE_NAMEPART = "__small__head";

        private static string namePartPattern = @"__(?:original|big|mid|small)__head(?=\.[a-zA-Z]+$)";

        /// <summary>
        /// 将图片路径中的图片名称转为目标图片规格的名称
        /// </summary>
        /// <param name="path">图片路径（带图片名称）</param>
        /// <param name="toType">要转为的目标图片规格类型，其为枚举值</param>
        /// <returns>转化后的图片路径</returns>
        public static string ConvertImageNamePart(string path, ImageNamePartType toType)
        {
            if (String.IsNullOrWhiteSpace(path)) return path;

            string toNamePart = ORIGINAL_IMAGE_NAMEPART;
            switch (toType)
            {
                case ImageNamePartType.Original:
                    toNamePart = ORIGINAL_IMAGE_NAMEPART;
                    break;
                case ImageNamePartType.Big:
                    toNamePart = BIG_IMAGE_NAMEPART;
                    break;
                case ImageNamePartType.Mid:
                    toNamePart = MID_IMAGE_NAMEPART;
                    break;
                case ImageNamePartType.Small:
                    toNamePart = SMALL_IMAGE_NAMEPART;
                    break;
                default:
                    break;
            }

            return Regex.Replace(path, namePartPattern, toNamePart);
        }

        #endregion

        /// <summary>
        /// 获取图片中的各帧
        /// </summary>
        /// <param name="pPath">图片路径</param>
        /// <param name="pSavePath">保存路径</param>
        public void GetFrames(string pPath, string pSavedPath)
        {
            Image gif = Image.FromFile(pPath);
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);

            //获取帧数(gif图片可能包含多帧，其它格式图片一般仅一帧)
            int count = gif.GetFrameCount(fd);

            //以Jpeg格式保存各帧
            for (int i = 0; i < count; i++)
            {
                gif.SelectActiveFrame(fd, i);
                gif.Save(pSavedPath + "\\frame_" + i + ".jpg", ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// 获取图片缩略图
        /// </summary>
        /// <param name="pPath">图片路径</param>
        /// <param name="pSavePath">保存路径</param>
        /// <param name="pWidth">缩略图宽度</param>
        /// <param name="pHeight">缩略图高度</param>
        /// <param name="pFormat">保存格式，通常可以是jpeg</param>
        public void GetSmaller(string pPath, string pSavedPath, int pWidth, int pHeight)
        {
            string fileSaveUrl = pSavedPath;// +"\\smaller.jpg";

            using (FileStream fs = new FileStream(pPath, FileMode.Open))
            {

                MakeSmallImg(fs, fileSaveUrl, pWidth, pHeight);
            }

        }

        //按模版比例生成缩略图（以流的方式获取源文件）  
        //生成缩略图函数  
        //顺序参数：源图文件流、缩略图存放地址、模版宽、模版高  
        //注：缩略图大小控制在模版区域内  

        public static void MakeSmallImg(Stream fromFileStream, string fileSaveUrl, Double templateWidth, Double templateHeight, Image waterImg = null)
        {
            //从文件取得图片对象，并使用流中嵌入的颜色管理信息  
            Image myImage = Image.FromStream(fromFileStream, true);
            Graphics g = Graphics.FromImage(myImage);
            Image bitmap = myImage;
            if (myImage.Width != templateWidth || myImage.Height != templateHeight)
            {
                //缩略图宽、高  
                Double newWidth = myImage.Width, newHeight = myImage.Height;
                //宽大于模版的横图  
                if (templateHeight <= 0 || myImage.Width > myImage.Height || myImage.Width == myImage.Height)
                {
                    if (myImage.Width > templateWidth)
                    {
                        //宽按模版，高按比例缩放  
                        newWidth = templateWidth;
                        newHeight = Math.Max(myImage.Height * (newWidth / myImage.Width), 1);
                    }
                }
                //高大于模版的竖图  
                else
                {
                    if (myImage.Height > templateHeight)
                    {
                        //高按模版，宽按比例缩放  
                        newHeight = templateHeight;
                        newWidth = Math.Max(myImage.Width * (newHeight / myImage.Height), 1);
                    }
                }

                //取得图片大小  
                Size mySize = new Size((int)newWidth, (int)newHeight);
                //新建一个bmp图片  
                bitmap = new Bitmap(mySize.Width, mySize.Height);
                //新建一个画板  
                g = Graphics.FromImage(bitmap);
                //设置高质量插值法  
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度  
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //清空一下画布  
                g.Clear(Color.White);
                //在指定位置画图  
                g.DrawImage(myImage, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                new Rectangle(0, 0, myImage.Width, myImage.Height),
                GraphicsUnit.Pixel);
            }

            if (waterImg != null)
            {
                ///图片水印  
                #region 添加图片水印 暂时不处理gif

                if (Path.GetExtension(fileSaveUrl).ToLower() != ".gif")
                {
                    #region 当图片高度小于水印高度的两倍 或者 宽度小于水印宽度的两倍时，水印按比例缩小

                    int waterWidth = waterImg.Width;
                    int waterHeight = waterImg.Height;

                    //原图横向较水印图宽
                    bool isH = (bitmap.Width / waterImg.Width) > (bitmap.Height / waterImg.Height);

                    if (isH && bitmap.Height < waterImg.Height * 2)
                    {
                        waterHeight = bitmap.Height / 2;
                        waterWidth = (int)Math.Ceiling((waterImg.Width * 1.0 / waterImg.Height) * waterHeight);
                    }

                    if (!isH && bitmap.Width < waterImg.Width * 2)
                    {
                        waterWidth = bitmap.Width / 2;
                        waterHeight = (int)Math.Ceiling((waterImg.Height * 1.0 / waterImg.Width) * waterWidth);
                    }

                    #endregion

                    #region 水印透明度

                    float[][] nArray ={ new float[] {1, 0, 0, 0, 0},
                                        new float[] {0, 1, 0, 0, 0},
                                        new float[] {0, 0, 1, 0, 0},
                                        new float[] {0, 0, 0, 0.5f, 0},
                                        new float[] {0, 0, 0, 0, 1}
                                      };
                    ColorMatrix matrix = new ColorMatrix(nArray);
                    ImageAttributes attributes = new ImageAttributes();
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    #endregion

                    g.DrawImage(waterImg, new Rectangle(bitmap.Width - waterWidth, bitmap.Height - waterHeight, waterWidth, waterHeight), 0, 0, waterImg.Width, waterImg.Height, GraphicsUnit.Pixel, attributes);
                }

                #endregion
            }

            //保存缩略图  
            if (File.Exists(fileSaveUrl))
            {
                File.SetAttributes(fileSaveUrl, FileAttributes.Normal);
                File.Delete(fileSaveUrl);
            }

            bitmap.Save(fileSaveUrl, Path.GetExtension(fileSaveUrl).ToLower() == ".gif" ? System.Drawing.Imaging.ImageFormat.Gif : System.Drawing.Imaging.ImageFormat.Jpeg);

            g.Dispose();
            myImage.Dispose();
            bitmap.Dispose();
        }

        /// <summary>
        /// 获取图片指定部分
        /// </summary>
        /// <param name="pPath">图片路径</param>
        /// <param name="pSavePath">保存路径</param>
        /// <param name="pPartStartPointX">目标图片开始绘制处的坐标X值(通常为)</param>
        /// <param name="pPartStartPointY">目标图片开始绘制处的坐标Y值(通常为)</param>
        /// <param name="pPartWidth">目标图片的宽度</param>
        /// <param name="pPartHeight">目标图片的高度</param>
        /// <param name="pOrigStartPointX">原始图片开始截取处的坐标X值</param>
        /// <param name="pOrigStartPointY">原始图片开始截取处的坐标Y值</param>
        /// <param name="pFormat">保存格式，通常可以是jpeg</param>
        public void GetPart(string pPath, string pSavedPath, int pPartStartPointX, int pPartStartPointY, int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY)
        {
            string normalJpgPath = pSavedPath;// +"\\normal.jpg";

            using (Image originalImg = Image.FromFile(pPath))
            {
                Bitmap partImg = new Bitmap(pPartWidth, pPartHeight);
                Graphics graphics = Graphics.FromImage(partImg);
                Rectangle destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY), new Size(pPartWidth, pPartHeight));//目标位置
                Rectangle origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY), new Size(pPartWidth, pPartHeight));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）


                ///文字水印  
                System.Drawing.Graphics G = System.Drawing.Graphics.FromImage(partImg);
                System.Drawing.Font f = new Font("Lucida Grande", 12);
                System.Drawing.Brush b = new SolidBrush(Color.Gray);
                G.Clear(Color.White);
                graphics.DrawImage(originalImg, destRect, origRect, GraphicsUnit.Pixel);
                G.DrawString("", f, b, 0, 0);
                G.Dispose();

                originalImg.Dispose();
                if (File.Exists(normalJpgPath))
                {
                    File.SetAttributes(normalJpgPath, FileAttributes.Normal);
                    File.Delete(normalJpgPath);
                }
                partImg.Save(normalJpgPath, ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// 获取按比例缩放的图片指定部分
        /// </summary>
        /// <param name="pPath">图片路径</param>
        /// <param name="pSavePath">保存路径</param>
        /// <param name="pPartStartPointX">目标图片开始绘制处的坐标X值(通常为)</param>
        /// <param name="pPartStartPointY">目标图片开始绘制处的坐标Y值(通常为)</param>
        /// <param name="pPartWidth">目标图片的宽度</param>
        /// <param name="pPartHeight">目标图片的高度</param>
        /// <param name="pOrigStartPointX">原始图片开始截取处的坐标X值</param>
        /// <param name="pOrigStartPointY">原始图片开始截取处的坐标Y值</param>
        /// <param name="imageWidth">缩放后的宽度</param>
        /// <param name="imageHeight">缩放后的高度</param>
        public void GetPart(string pPath, string pSavedPath, int pPartStartPointX, int pPartStartPointY, int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY, int imageWidth, int imageHeight)
        {
            string normalJpgPath = pSavedPath;// +"\\normal.jpg";
            using (Image originalImg = Image.FromFile(pPath))
            {
                if (originalImg.Width == imageWidth && originalImg.Height == imageHeight)
                {
                    GetPart(pPath, pSavedPath, pPartStartPointX, pPartStartPointY, pPartWidth, pPartHeight, pOrigStartPointX, pOrigStartPointY);
                    return;
                }

                Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Image zoomImg = originalImg.GetThumbnailImage(imageWidth, imageHeight, callback, IntPtr.Zero);//缩放
                Bitmap partImg = new Bitmap(pPartWidth, pPartHeight);

                Graphics graphics = Graphics.FromImage(partImg);
                Rectangle destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY), new Size(pPartWidth, pPartHeight));//目标位置
                Rectangle origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY), new Size(pPartWidth, pPartHeight));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）

                ///文字水印  
                System.Drawing.Graphics G = System.Drawing.Graphics.FromImage(partImg);
                System.Drawing.Font f = new Font("Lucida Grande", 12);
                System.Drawing.Brush b = new SolidBrush(Color.Gray);
                G.Clear(Color.White);

                graphics.DrawImage(zoomImg, destRect, origRect, GraphicsUnit.Pixel);
                G.DrawString("", f, b, 0, 0);
                G.Dispose();

                originalImg.Dispose();
                if (File.Exists(normalJpgPath))
                {
                    File.SetAttributes(normalJpgPath, FileAttributes.Normal);
                    File.Delete(normalJpgPath);
                }
                partImg.Save(normalJpgPath, ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// 获得图像高宽信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ImageInformation GetImageInfo(string path)
        {
            using (Image image = Image.FromFile(path))
            {
                ImageInformation imageInfo = new ImageInformation();
                imageInfo.Width = image.Width;
                imageInfo.Height = image.Height;
                return imageInfo;
            }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }
    }

    /// <summary>
    /// 图片基本信息封装类
    /// </summary>
    public struct ImageInformation
    {
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
    }

    public enum ImageNamePartType : short
    {
        Original = 0,
        Big,
        Mid,
        Small
    }

    public class TemplateWH
    {
        public int templateWidth { get; set; }

        public int templateHeight { get; set; }
    }
}
