using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Transactions;
using TygaSoft.SysHelper;
using TygaSoft.WebHelper;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Handlers.Admin
{
    /// <summary>
    /// HandlerUpload 的摘要说明
    /// </summary>
    public class HandlerUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string msg = "";
            try
            {
                string reqName = "";
                switch (context.Request.HttpMethod.ToUpper())
                {
                    case "GET":
                        reqName = context.Request.QueryString["reqName"].Trim();
                        break;
                    case "POST":
                        reqName = context.Request.Form["reqName"].Trim();
                        break;
                    default:
                        break;
                }

                switch (reqName)
                {
                    case "CommunionPicture":
                        OnUploadCommunionPicture(context);
                        break;
                    case "ActivityPhotoPicture":
                        OnUploadActivityPhotoPicture(context);
                        break;
                    case "ActivityPlayerPhotoPicture":
                        OnUploadActivityPlayerPhotoPicture(context);
                        break;
                    case "UserHeadPicture":
                        OnUploadUserHeadPicture(context);
                        break;
                    case "PictureScratchLotto":
                        OnUploadPictureScratchLotto(context);
                        break;
                    case "InformationPicture":
                        OnUploadInformationPicture(context);
                        break;
                    case "PictureAdStartup":
                        OnUploadPictureAdStartup(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (msg != "")
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + msg + "\"}");
            }
        }

        private void OnUploadCommunionPicture(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string errorMsg = "";
            try
            {
                HttpFileCollection files = context.Request.Files;
                if (files.Count == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                int effect = 0;
                UploadFilesHelper ufh = new UploadFilesHelper();
                ImagesHelper ih = new ImagesHelper();

                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (string item in files.AllKeys)
                    {
                        HttpPostedFile file = files[item];
                        if (file == null || file.ContentLength == 0)
                        {
                            continue;
                        }

                        int fileSize = file.ContentLength;
                        int uploadFileSize = int.Parse(ConfigHelper.GetValueByKey("UploadFileSize"));
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        CommunionPicture bll = new CommunionPicture();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "CommunionPicture");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("cmp"));

                        CommunionPictureInfo model = new CommunionPictureInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        string rndDirFullPath = context.Server.MapPath(string.Format("~{0}{1}", model.FileDirectory, model.RandomFolder));
                        if (!Directory.Exists(rndDirFullPath))
                        {
                            Directory.CreateDirectory(rndDirFullPath);
                        }
                        File.Copy(context.Server.MapPath(originalUrl), string.Format("{0}\\{1}{2}", rndDirFullPath, randomFolder, model.FileExtension), true);

                        string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
                        foreach (string name in platformNames)
                        {
                            string platformUrl = string.Format("{0}/{1}/{2}", model.FileDirectory, model.RandomFolder, name);
                            string platformUrlFullPath = context.Server.MapPath("~" + platformUrl);
                            if (!Directory.Exists(platformUrlFullPath))
                            {
                                Directory.CreateDirectory(platformUrlFullPath);
                            }
                            string sizeAppend = ConfigHelper.GetValueByKey(name);
                            string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < sizeArr.Length; i++)
                            {
                                string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, model.RandomFolder, i, model.FileExtension);
                                string[] wh = sizeArr[i].Split('*');

                                ih.CreateThumbnailImage(context.Server.MapPath(originalUrl), bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", model.FileExtension);
                            }
                        }

                        effect++;
                    }

                    scope.Complete();
                }

                if (effect == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"已成功上传文件数：" + effect + "个\"}");

                return;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            context.Response.Write("{\"success\": false,\"message\": \"" + errorMsg + "\"}");
        }

        private void OnUploadUserHeadPicture(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string errorMsg = "";
            try
            {
                HttpFileCollection files = context.Request.Files;
                if (files.Count == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                int effect = 0;
                UploadFilesHelper ufh = new UploadFilesHelper();
                ImagesHelper ih = new ImagesHelper();

                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (string item in files.AllKeys)
                    {
                        HttpPostedFile file = files[item];
                        if (file == null || file.ContentLength == 0)
                        {
                            continue;
                        }

                        int fileSize = file.ContentLength;
                        int uploadFileSize = int.Parse(ConfigHelper.GetValueByKey("UploadFileSize"));
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        UserHeadPicture bll = new UserHeadPicture();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "UserHeadPicture");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("uhp"));

                        UserHeadPictureInfo model = new UserHeadPictureInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        string rndDirFullPath = context.Server.MapPath(string.Format("~{0}{1}", model.FileDirectory, model.RandomFolder));
                        if (!Directory.Exists(rndDirFullPath))
                        {
                            Directory.CreateDirectory(rndDirFullPath);
                        }
                        File.Copy(context.Server.MapPath(originalUrl), string.Format("{0}\\{1}{2}", rndDirFullPath, randomFolder, model.FileExtension), true);

                        string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
                        foreach (string name in platformNames)
                        {
                            string platformUrl = string.Format("{0}/{1}/{2}", model.FileDirectory, model.RandomFolder, name);
                            string platformUrlFullPath = context.Server.MapPath("~" + platformUrl);
                            if (!Directory.Exists(platformUrlFullPath))
                            {
                                Directory.CreateDirectory(platformUrlFullPath);
                            }
                            string sizeAppend = ConfigHelper.GetValueByKey(name);
                            string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < sizeArr.Length; i++)
                            {
                                string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, model.RandomFolder, i, model.FileExtension);
                                string[] wh = sizeArr[i].Split('*');

                                ih.CreateThumbnailImage(context.Server.MapPath(originalUrl), bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", model.FileExtension);
                            }
                        }

                        effect++;
                    }

                    scope.Complete();
                }

                if (effect == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"已成功上传文件数：" + effect + "个\"}");

                return;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            context.Response.Write("{\"success\": false,\"message\": \"" + errorMsg + "\"}");
        }

        private void OnUploadActivityPhotoPicture(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string errorMsg = "";
            try
            {
                HttpFileCollection files = context.Request.Files;
                if (files.Count == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                int effect = 0;
                UploadFilesHelper ufh = new UploadFilesHelper();
                ImagesHelper ih = new ImagesHelper();

                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (string item in files.AllKeys)
                    {
                        HttpPostedFile file = files[item];
                        if (file == null || file.ContentLength == 0)
                        {
                            continue;
                        }

                        int fileSize = file.ContentLength;
                        int uploadFileSize = int.Parse(ConfigHelper.GetValueByKey("UploadFileSize"));
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        ActivityPhotoPicture bll = new ActivityPhotoPicture();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "ActivityPhotoPicture");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("app"));

                        ActivityPhotoPictureInfo model = new ActivityPhotoPictureInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        string rndDirFullPath = context.Server.MapPath(string.Format("~{0}{1}", model.FileDirectory, model.RandomFolder));
                        if (!Directory.Exists(rndDirFullPath))
                        {
                            Directory.CreateDirectory(rndDirFullPath);
                        }
                        File.Copy(context.Server.MapPath(originalUrl), string.Format("{0}\\{1}{2}", rndDirFullPath, randomFolder, model.FileExtension), true);

                        string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
                        foreach (string name in platformNames)
                        {
                            string platformUrl = string.Format("{0}/{1}/{2}", model.FileDirectory, model.RandomFolder, name);
                            string platformUrlFullPath = context.Server.MapPath("~" + platformUrl);
                            if (!Directory.Exists(platformUrlFullPath))
                            {
                                Directory.CreateDirectory(platformUrlFullPath);
                            }
                            string sizeAppend = ConfigHelper.GetValueByKey(name);
                            string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < sizeArr.Length; i++)
                            {
                                string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, model.RandomFolder, i, model.FileExtension);
                                string[] wh = sizeArr[i].Split('*');

                                ih.CreateThumbnailImage(context.Server.MapPath(originalUrl), bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", model.FileExtension);
                            }
                        }

                        effect++;
                    }

                    scope.Complete();
                }

                if (effect == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"已成功上传文件数：" + effect + "个\"}");

                return;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            context.Response.Write("{\"success\": false,\"message\": \"" + errorMsg + "\"}");
        }

        private void OnUploadActivityPlayerPhotoPicture(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string errorMsg = "";
            try
            {
                HttpFileCollection files = context.Request.Files;
                if (files.Count == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                int effect = 0;
                UploadFilesHelper ufh = new UploadFilesHelper();
                ImagesHelper ih = new ImagesHelper();

                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (string item in files.AllKeys)
                    {
                        HttpPostedFile file = files[item];
                        if (file == null || file.ContentLength == 0)
                        {
                            continue;
                        }

                        int fileSize = file.ContentLength;
                        int uploadFileSize = int.Parse(ConfigHelper.GetValueByKey("UploadFileSize"));
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        ActivityPlayerPhotoPicture bll = new ActivityPlayerPhotoPicture();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "ActivityPhotoPicture");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("appp"));

                        ActivityPlayerPhotoPictureInfo model = new ActivityPlayerPhotoPictureInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        string rndDirFullPath = context.Server.MapPath(string.Format("~{0}{1}", model.FileDirectory, model.RandomFolder));
                        if (!Directory.Exists(rndDirFullPath))
                        {
                            Directory.CreateDirectory(rndDirFullPath);
                        }
                        File.Copy(context.Server.MapPath(originalUrl), string.Format("{0}\\{1}{2}", rndDirFullPath, randomFolder, model.FileExtension), true);

                        string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
                        foreach (string name in platformNames)
                        {
                            string platformUrl = string.Format("{0}/{1}/{2}", model.FileDirectory, model.RandomFolder, name);
                            string platformUrlFullPath = context.Server.MapPath("~" + platformUrl);
                            if (!Directory.Exists(platformUrlFullPath))
                            {
                                Directory.CreateDirectory(platformUrlFullPath);
                            }
                            string sizeAppend = ConfigHelper.GetValueByKey(name);
                            string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < sizeArr.Length; i++)
                            {
                                string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, model.RandomFolder, i, model.FileExtension);
                                string[] wh = sizeArr[i].Split('*');

                                ih.CreateThumbnailImage(context.Server.MapPath(originalUrl), bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", model.FileExtension);
                            }
                        }

                        effect++;
                    }

                    scope.Complete();
                }

                if (effect == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"已成功上传文件数：" + effect + "个\"}");

                return;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            context.Response.Write("{\"success\": false,\"message\": \"" + errorMsg + "\"}");
        }

        private void OnUploadPictureScratchLotto(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string errorMsg = "";
            try
            {
                HttpFileCollection files = context.Request.Files;
                if (files.Count == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                int effect = 0;
                UploadFilesHelper ufh = new UploadFilesHelper();
                ImagesHelper ih = new ImagesHelper();

                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (string item in files.AllKeys)
                    {
                        HttpPostedFile file = files[item];
                        if (file == null || file.ContentLength == 0)
                        {
                            continue;
                        }

                        int fileSize = file.ContentLength;
                        int uploadFileSize = int.Parse(ConfigHelper.GetValueByKey("UploadFileSize"));
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureScratchLotto bll = new PictureScratchLotto();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "ActivityPhotoPicture");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("slp"));

                        PictureScratchLottoInfo model = new PictureScratchLottoInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        string rndDirFullPath = context.Server.MapPath(string.Format("~{0}{1}", model.FileDirectory, model.RandomFolder));
                        if (!Directory.Exists(rndDirFullPath))
                        {
                            Directory.CreateDirectory(rndDirFullPath);
                        }
                        File.Copy(context.Server.MapPath(originalUrl), string.Format("{0}\\{1}{2}", rndDirFullPath, randomFolder, model.FileExtension), true);

                        string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
                        foreach (string name in platformNames)
                        {
                            string platformUrl = string.Format("{0}/{1}/{2}", model.FileDirectory, model.RandomFolder, name);
                            string platformUrlFullPath = context.Server.MapPath("~" + platformUrl);
                            if (!Directory.Exists(platformUrlFullPath))
                            {
                                Directory.CreateDirectory(platformUrlFullPath);
                            }
                            string sizeAppend = ConfigHelper.GetValueByKey(name);
                            string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < sizeArr.Length; i++)
                            {
                                string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, model.RandomFolder, i, model.FileExtension);
                                string[] wh = sizeArr[i].Split('*');

                                ih.CreateThumbnailImage(context.Server.MapPath(originalUrl), bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", model.FileExtension);
                            }
                        }

                        effect++;
                    }

                    scope.Complete();
                }

                if (effect == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"已成功上传文件数：" + effect + "个\"}");

                return;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            context.Response.Write("{\"success\": false,\"message\": \"" + errorMsg + "\"}");
        }

        private void OnUploadInformationPicture(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string errorMsg = "";
            try
            {
                HttpFileCollection files = context.Request.Files;
                if (files.Count == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                int effect = 0;
                UploadFilesHelper ufh = new UploadFilesHelper();
                ImagesHelper ih = new ImagesHelper();

                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (string item in files.AllKeys)
                    {
                        HttpPostedFile file = files[item];
                        if (file == null || file.ContentLength == 0)
                        {
                            continue;
                        }

                        int fileSize = file.ContentLength;
                        int uploadFileSize = int.Parse(ConfigHelper.GetValueByKey("UploadFileSize"));
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureInformation bll = new PictureInformation();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "InformationPicture");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("ip"));

                        PictureInformationInfo model = new PictureInformationInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        string rndDirFullPath = context.Server.MapPath(string.Format("~{0}{1}", model.FileDirectory, model.RandomFolder));
                        if (!Directory.Exists(rndDirFullPath))
                        {
                            Directory.CreateDirectory(rndDirFullPath);
                        }
                        File.Copy(context.Server.MapPath(originalUrl), string.Format("{0}\\{1}{2}", rndDirFullPath, randomFolder, model.FileExtension), true);

                        string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
                        foreach (string name in platformNames)
                        {
                            string platformUrl = string.Format("{0}/{1}/{2}", model.FileDirectory, model.RandomFolder, name);
                            string platformUrlFullPath = context.Server.MapPath("~" + platformUrl);
                            if (!Directory.Exists(platformUrlFullPath))
                            {
                                Directory.CreateDirectory(platformUrlFullPath);
                            }
                            string sizeAppend = ConfigHelper.GetValueByKey(name);
                            string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < sizeArr.Length; i++)
                            {
                                string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, model.RandomFolder, i, model.FileExtension);
                                string[] wh = sizeArr[i].Split('*');

                                ih.CreateThumbnailImage(context.Server.MapPath(originalUrl), bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", model.FileExtension);
                            }
                        }

                        effect++;
                    }

                    scope.Complete();
                }

                if (effect == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"已成功上传文件数：" + effect + "个\"}");

                return;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            context.Response.Write("{\"success\": false,\"message\": \"" + errorMsg + "\"}");
        }

        private void OnUploadPictureAdStartup(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string errorMsg = "";
            try
            {
                HttpFileCollection files = context.Request.Files;
                if (files.Count == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                int effect = 0;
                UploadFilesHelper ufh = new UploadFilesHelper();
                ImagesHelper ih = new ImagesHelper();

                foreach (string item in files.AllKeys)
                {
                    HttpPostedFile file = files[item];
                    if (file == null || file.ContentLength == 0)
                    {
                        continue;
                    }

                    int fileSize = file.ContentLength;
                    int uploadFileSize = int.Parse(ConfigHelper.GetValueByKey("UploadFileSize"));
                    if (fileSize > uploadFileSize)
                    {
                        throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                    }
                    if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                    {
                        throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                    }

                    string fileName = file.FileName;

                    var bll = new PictureAdStartup();
                    if (bll.IsExist(file.FileName, fileSize))
                    {
                        throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                    }

                    string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "PictureAdStartup");
                    string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("padsu"));

                    var model = new PictureAdStartupInfo();
                    model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                    model.FileSize = fileSize;
                    model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                    model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                    model.RandomFolder = randomFolder;
                    model.LastUpdatedDate = DateTime.Now;
                    model.UserId = WebCommon.GetUserId();

                    bll.Insert(model);

                    string rndDirFullPath = context.Server.MapPath(string.Format("~{0}{1}", model.FileDirectory, model.RandomFolder));
                    if (!Directory.Exists(rndDirFullPath))
                    {
                        Directory.CreateDirectory(rndDirFullPath);
                    }
                    File.Copy(context.Server.MapPath(originalUrl), string.Format("{0}\\{1}{2}", rndDirFullPath, randomFolder, model.FileExtension), true);

                    string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
                    foreach (string name in platformNames)
                    {
                        string platformUrl = string.Format("{0}/{1}/{2}", model.FileDirectory, model.RandomFolder, name);
                        string platformUrlFullPath = context.Server.MapPath("~" + platformUrl);
                        if (!Directory.Exists(platformUrlFullPath))
                        {
                            Directory.CreateDirectory(platformUrlFullPath);
                        }
                        string sizeAppend = ConfigHelper.GetValueByKey(name);
                        string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < sizeArr.Length; i++)
                        {
                            string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, model.RandomFolder, i, model.FileExtension);
                            string[] wh = sizeArr[i].Split('*');

                            ih.CreateThumbnailImage(context.Server.MapPath(originalUrl), bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", model.FileExtension);
                        }
                    }

                    effect++;
                }

                if (effect == 0)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"未找到任何可上传的文件，请检查！\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"已成功上传文件数：" + effect + "个\"}");

                return;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            context.Response.Write("{\"success\": false,\"message\": \"" + errorMsg + "\"}");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}