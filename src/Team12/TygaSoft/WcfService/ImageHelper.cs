using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Drawing;
using TygaSoft.SysHelper;

namespace TygaSoft.WcfService
{
    public class ImageHelper
    {
        public void CreateThumbnailImage(string dir, string fromFile, string rndCode)
        {
            string fileEx = Path.GetExtension(fromFile);

            string rndDirFullPath = string.Format("{0}\\{1}", dir, rndCode);
            if (!Directory.Exists(rndDirFullPath))
            {
                Directory.CreateDirectory(rndDirFullPath);
            }
            File.Copy(fromFile, string.Format("{0}\\{1}{2}", rndDirFullPath, rndCode, fileEx), true);

            string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
            foreach (string name in platformNames)
            {
                string platformUrlFullPath = string.Format("{0}\\{1}\\{2}", dir, rndCode, name);
                if (!Directory.Exists(platformUrlFullPath))
                {
                    Directory.CreateDirectory(platformUrlFullPath);
                }
                string sizeAppend = ConfigurationManager.AppSettings[name];
                string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sizeArr.Length; i++)
                {
                    string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, rndCode, i, fileEx);
                    string[] wh = sizeArr[i].Split('*');

                    CreateThumbnailImage(fromFile, bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", fileEx);
                }
            }
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="fromPath">源图路径（物理路径）</param>
        /// <param name="toPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param> 
        /// <param name="fileExtension">生成的文件类型</param> 
        public void CreateThumbnailImage(string fromPath, string toPath, int width, int height, string mode, string fileExtension)
        {
            Image originalImage = Image.FromFile(fromPath);

            int towidth = width;
            int toheight = height;
            fileExtension = fileExtension.ToLower();

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形） 
                    break;
                case "W"://指定宽，高按比例 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形） 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                case "DB"://等比缩放（不变形，如果高大按高，宽大按宽缩放）
                    if (((double)originalImage.Width / (double)towidth) < ((double)originalImage.Height / (double)toheight))
                    {
                        toheight = height;
                        towidth = originalImage.Width * height / originalImage.Height;
                    }
                    else
                    {
                        towidth = width;
                        toheight = originalImage.Height * width / originalImage.Width;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
            new Rectangle(x, y, ow, oh),
            GraphicsUnit.Pixel);

            try
            {
                //保存缩略图
                switch (fileExtension)
                {
                    case ".jpg":
                        bitmap.Save(toPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".bmp":
                        bitmap.Save(toPath, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case ".gif":
                        bitmap.Save(toPath, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case ".png":
                        bitmap.Save(toPath, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        throw new ArgumentException("文件类型【" + fileExtension + "】不合法，请检查");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 使用文件固定字节法验证上传图片是否合法
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileLen"></param>
        /// <returns></returns>
        public bool IsPictureValidated(byte[] bytes)
        {
            FileExtension[] fes = { FileExtension.jpg, FileExtension.gif, FileExtension.bmp, FileExtension.png };

            MemoryStream ms = new MemoryStream(bytes);
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

        public enum FileExtension
        {
            jpg = 255216, gif = 7173, bmp = 6677, png = 13780, xls = 208207, xlsx = 8075, doc = 208207, docx = 8075
            // 255216 jpg;7173 gif;6677 bmp;13780 png; 7790 exe dll; 8297 rar; 6063 xml;6033 html;239187 aspx;117115 cs;119105 js;210187 txt;255254 sql;xls = 208207 
        }
    }
}
