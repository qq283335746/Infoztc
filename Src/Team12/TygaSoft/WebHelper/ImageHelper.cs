using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace TygaSoft.WebHelper
{
    public class ImageHelper
    {
        public Image resourceImage;
        private int imageWidth;
        private int imageHeight;
        public string errMessage;

        /// <summary>
        /// 类的构造函数
        /// </summary>
        /// <param name="imageFileName">图片文件的全路径名称</param>
        public ImageHelper(string imageFileName)
        {
            try
            {
                resourceImage = Image.FromFile(imageFileName);
                errMessage = "";
            }
            catch
            {
                //   ResourceImage = Image.FromFile(Application.StartupPath + "\\null.jpg");
            }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// 生成缩略图重载方法1，返回缩略图的Image对象
        /// </summary>
        /// <param name="width">缩略图的宽度</param>
        /// <param name="height">缩略图的高度</param>
        /// <returns>缩略图的Image对象</returns>
        public Image GetReducedImage(int width, int height)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                ReducedImage = resourceImage.GetThumbnailImage(width, height, callb, IntPtr.Zero);
                resourceImage.Dispose();

                return ReducedImage;
            }
            catch (Exception e)
            {
                errMessage = e.Message;

                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法2，将缩略图文件保存到指定的路径
        /// </summary>
        /// <param name="width">缩略图的宽度</param>
        /// <param name="height">缩略图的高度</param>
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:Images ilename.jpg</param>
        /// <returns>成功返回true，否则返回false</returns>
        public bool GetReducedImage(int width, int height, string targetFilePath)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                ReducedImage = resourceImage.GetThumbnailImage(width, height, callb, IntPtr.Zero);
                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);
                ReducedImage.Dispose();

                return true;
            }
            catch (Exception e)
            {
                errMessage = e.Message;

                return false;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法3，返回缩略图的Image对象
        /// </summary>
        /// <param name="percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>
        /// <returns>缩略图的Image对象</returns>
        public Image GetReducedImage(double percent)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                imageWidth = Convert.ToInt32(resourceImage.Width * percent);
                imageHeight = Convert.ToInt32(resourceImage.Width * percent);
                ReducedImage = resourceImage.GetThumbnailImage(imageWidth, imageHeight, callb, IntPtr.Zero);

                return ReducedImage;
            }
            catch (Exception e)
            {
                errMessage = e.Message;

                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法4，返回缩略图的Image对象
        /// </summary>
        /// <param name="percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:Images ilename.jpg</param>
        /// <returns>成功返回true,否则返回false</returns>
        public bool GetReducedImage(double percent, string targetFilePath)
        {
            try
            {
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                imageWidth = Convert.ToInt32(resourceImage.Width * percent);
                imageHeight = Convert.ToInt32(resourceImage.Width * percent);
                ReducedImage = resourceImage.GetThumbnailImage(imageWidth, imageHeight, callb, IntPtr.Zero);
                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);
                ReducedImage.Dispose();

                return true;
            }
            catch (Exception e)
            {
                errMessage = e.Message;

                return false;
            }
        }

        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="fromPath">原服务器图片路径</param>
        /// <param name="toPath">生成的带文字水印的图片路径</param>
        protected void AddShuiYinWord(string fromPath, string toPath)
        {
            string addText = "测试水印";
            System.Drawing.Image image = System.Drawing.Image.FromFile(fromPath);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 16);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);

            g.DrawString(addText, f, b, 15, 15);
            g.Dispose();

            image.Save(toPath);
            image.Dispose();
        }

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="fromPath">原服务器图片路径</param>
        /// <param name="toPath">生成的带图片水印的图片路径</param>
        /// <param name="fromPic">水印图片路径</param>
        protected void AddShuiYinPic(string fromPath, string toPath, string fromPic)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(fromPath);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(fromPic);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(toPath);
            image.Dispose();
        }

        public byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Gif);

            return ms.ToArray();
        }

        public string ConvertImageToBase64String(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                var base64 = Convert.ToBase64String(buffer);

                return base64;
            }
        }

        public Image ByteArrayToImage(byte[] fileBytes)
        {
            using (MemoryStream fileStream = new MemoryStream(fileBytes))
            {
                return Image.FromStream(fileStream);
            }
        }

    }
}
