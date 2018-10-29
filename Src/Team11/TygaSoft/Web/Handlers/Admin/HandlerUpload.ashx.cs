﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Transactions;
using System.Web.Configuration;
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
                    case "PictureBrand":
                        OnUploadPictureBrand(context);
                        break;
                    case "PictureCategory":
                        OnUploadPictureCategory(context);
                        break;
                    case "PictureProduct":
                        OnUploadPictureProduct(context);
                        break;
                    case "PictureProductSize":
                        OnUploadPictureProductSize(context);
                        break;
                    case "AdvertisementPicture":
                        OnUploadAdvertisementPicture(context);
                        break;
                    case "PictureServiceItem":
                        OnUploadPictureServiceItem(context);
                        break;
                    case "PictureServiceVote":
                        OnUploadPictureServiceVote(context);
                        break;
                    case "PictureServiceLink":
                        OnUploadPictureServiceLink(context);
                        break;
                    case "PictureServiceContent":
                        OnUploadPictureServiceContent(context);
                        break;
                    case "PictureContent":
                        OnUploadPictureContent(context);
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

        private void OnUploadAdvertisementPicture(HttpContext context)
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
                        int uploadFileSize = int.Parse(TygaSoft.WebHelper.ConfigHelper.GetValueByKey("UploadFileSize"));
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        AdvertisementPicture bll = new AdvertisementPicture();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "AdvertisementPicture");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("adp"));

                        AdvertisementPictureInfo model = new AdvertisementPictureInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        CreateThumbnailImage(context, ih, originalUrl, model.FileDirectory, model.RandomFolder, model.FileExtension);

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

        private void OnUploadPictureBrand(HttpContext context)
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
                        int uploadFileSize = int.Parse(WebConfigurationManager.AppSettings["UploadFileSize"]);
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureBrand bll = new PictureBrand();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "PictureBrand");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("bp"));

                        PictureBrandInfo model = new PictureBrandInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        CreateThumbnailImage(context, ih, originalUrl, model.FileDirectory, model.RandomFolder, model.FileExtension);

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

        private void OnUploadPictureCategory(HttpContext context)
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
                        int uploadFileSize = int.Parse(WebConfigurationManager.AppSettings["UploadFileSize"]);
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureCategory bll = new PictureCategory();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "PictureCategory");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("cp"));

                        PictureCategoryInfo model = new PictureCategoryInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        CreateThumbnailImage(context,ih, originalUrl, model.FileDirectory, model.RandomFolder, model.FileExtension);

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

        private void OnUploadPictureProduct(HttpContext context)
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
                        int uploadFileSize = int.Parse(WebConfigurationManager.AppSettings["UploadFileSize"]);
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureProduct bll = new PictureProduct();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "PictureProduct");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("pp"));

                        PictureProductInfo model = new PictureProductInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        CreateThumbnailImage(context, ih, originalUrl, model.FileDirectory, model.RandomFolder, model.FileExtension);

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

        private void OnUploadPictureProductSize(HttpContext context)
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
                        int uploadFileSize = int.Parse(WebConfigurationManager.AppSettings["UploadFileSize"]);
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureProductSize bll = new PictureProductSize();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "PictureProductSize");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("psp"));

                        PictureProductSizeInfo model = new PictureProductSizeInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        CreateThumbnailImage(context, ih, originalUrl, model.FileDirectory, model.RandomFolder, model.FileExtension);

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

        private void OnUploadPictureServiceItem(HttpContext context)
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
                        int uploadFileSize = int.Parse(WebConfigurationManager.AppSettings["UploadFileSize"]);
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureServiceItem bll = new PictureServiceItem();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "PictureServiceItem");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("sip"));

                        PictureServiceItemInfo model = new PictureServiceItemInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        CreateThumbnailImage(context, ih, originalUrl, model.FileDirectory, model.RandomFolder, model.FileExtension);

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

        private void OnUploadPictureServiceVote(HttpContext context)
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
                        int uploadFileSize = int.Parse(WebConfigurationManager.AppSettings["UploadFileSize"]);
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureServiceVote bll = new PictureServiceVote();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "PictureServiceVote");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("svp"));

                        PictureServiceVoteInfo model = new PictureServiceVoteInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        CreateThumbnailImage(context, ih, originalUrl, model.FileDirectory, model.RandomFolder, model.FileExtension);

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

        private void OnUploadPictureServiceLink(HttpContext context)
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
                        int uploadFileSize = int.Parse(WebConfigurationManager.AppSettings["UploadFileSize"]);
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureServiceLink bll = new PictureServiceLink();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "PictureServiceLink");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("slp"));

                        PictureServiceLinkInfo model = new PictureServiceLinkInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        CreateThumbnailImage(context, ih, originalUrl, model.FileDirectory, model.RandomFolder, model.FileExtension);

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

        private void OnUploadPictureServiceContent(HttpContext context)
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
                        int uploadFileSize = int.Parse(WebConfigurationManager.AppSettings["UploadFileSize"]);
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureServiceContent bll = new PictureServiceContent();
                        if (bll.IsExist(file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "PictureServiceContent");

                        //获取随机生成的文件名代码
                        string randomFolder = string.Format("{0}_{1}", DateTime.Now.ToString("MMdd"), UploadFilesHelper.GetRandomFolder("scp"));

                        PictureServiceContentInfo model = new PictureServiceContentInfo();
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        CreateThumbnailImage(context, ih, originalUrl, model.FileDirectory, model.RandomFolder, model.FileExtension);

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

        private void OnUploadPictureContent(HttpContext context)
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

                object userId = WebCommon.GetUserId();
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
                        int uploadFileSize = int.Parse(WebConfigurationManager.AppSettings["UploadFileSize"]);
                        if (fileSize > uploadFileSize)
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
                        }
                        if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
                        }

                        string fileName = file.FileName;

                        PictureContent bll = new PictureContent();
                        if (bll.IsExist(userId, file.FileName, fileSize))
                        {
                            throw new ArgumentException("文件【" + file.FileName + "】已存在，请勿重复上传！");
                        }

                        string originalUrl = UploadFilesHelper.UploadOriginalFile(file, "PictureContent");

                        //获取随机生成的文件名代码
                        string randomFolder = UploadFilesHelper.GetRandomFolder("cp");

                        PictureContentInfo model = new PictureContentInfo();
                        model.UserId = userId;
                        model.FileName = VirtualPathUtility.GetFileName(originalUrl);
                        model.FileSize = fileSize;
                        model.FileExtension = VirtualPathUtility.GetExtension(originalUrl).ToLower();
                        model.FileDirectory = VirtualPathUtility.GetDirectory(originalUrl.Replace("~", ""));
                        model.RandomFolder = randomFolder;
                        model.LastUpdatedDate = DateTime.Now;

                        bll.Insert(model);

                        CreateThumbnailImage(context, ih, originalUrl, model.FileDirectory, model.RandomFolder, model.FileExtension);

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

        private void CreateThumbnailImage(HttpContext context, ImagesHelper ih, string originalUrl, string fileDirectory, string randomFolder, string fileExtension)
        {
            string rndDirFullPath = context.Server.MapPath(string.Format("~{0}{1}", fileDirectory, randomFolder));
            if (!Directory.Exists(rndDirFullPath))
            {
                Directory.CreateDirectory(rndDirFullPath);
            }
            File.Copy(context.Server.MapPath(originalUrl), string.Format("{0}\\{1}{2}", rndDirFullPath, randomFolder, fileExtension), true);

            string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
            foreach (string name in platformNames)
            {
                string platformUrl = string.Format("{0}/{1}/{2}", fileDirectory, randomFolder, name);
                string platformUrlFullPath = context.Server.MapPath("~" + platformUrl);
                if (!Directory.Exists(platformUrlFullPath))
                {
                    Directory.CreateDirectory(platformUrlFullPath);
                }
                string sizeAppend = WebConfigurationManager.AppSettings[name];
                string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sizeArr.Length; i++)
                {
                    string bmsPicUrl = string.Format("{0}\\{1}_{2}{3}", platformUrlFullPath, randomFolder, i, fileExtension);
                    string[] wh = sizeArr[i].Split('*');

                    ih.CreateThumbnailImage(context.Server.MapPath(originalUrl), bmsPicUrl, int.Parse(wh[0]), int.Parse(wh[1]), "DB", fileExtension);
                }
            }
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