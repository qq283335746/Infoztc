﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Admin.AboutSite
{
    public partial class AddAdvertisement : System.Web.UI.Page
    {
        Guid Id;
        string myDataAppend;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    Guid.TryParse(Request.QueryString["Id"], out Id);
                }

                BindCbbActionType();
                Bind();

                ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + myDataAppend + "</div>";
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑广告";

                Advertisement bll = new Advertisement();

                var model = bll.GetModelByJoin(Id);
                if (model != null)
                {
                    hId.Value = Id.ToString();
                    txtTitle.Value = model.Title;
                    txtTimeout.Value = model.Timeout.ToString();
                    txtaDescr.Value = model.Descr;
                    txtContent.Value = model.ContentText;

                    string imgContentPictureHtml = "";
                    AdvertisementLink alBll = new AdvertisementLink();
                    var picList = alBll.GetListByAdId(Id);
                    if (picList != null && picList.Count > 0)
                    {
                        string adTemplateText = File.ReadAllText(Server.MapPath("~/Templates/PartialAdvertisement.txt"));
                        foreach (var adlModel in picList)
                        {
                            string currTemplateText = adTemplateText;
                            imgContentPictureHtml += string.Format(currTemplateText, adlModel.MPicture, adlModel.ContentPictureId, adlModel.ActionTypeId, adlModel.Url, adlModel.Sort, adlModel.IsDisable,adlModel.Id);
                        }

                        ltrImgItem.Text = imgContentPictureHtml;
                    }

                    myDataAppend += "<div id=\"myDataForModel\">[{\"SiteFunId\":\"" + model.SiteFunId + "\",\"LayoutPositionId\":\"" + model.LayoutPositionId + "\"}]</div>";
                }
            }
        }

        /// <summary>
        /// 绑定广告作用类别
        /// </summary>
        private void BindCbbActionType()
        {
            string cbbJson = "";
            ContentType ctBll = new ContentType();
            var dic = ctBll.GetKeyValueByParent("AdvertisementCategory");
            foreach (var kvp in dic)
            {
                cbbJson += "{\"id\":\"" + kvp.Key + "\",\"text\":\"" + kvp.Value + "\"},";
            }
            cbbJson = "[" + cbbJson.Trim(',') + "]";

            myDataAppend += "<div id=\"myDataForActionType\">" + cbbJson + "</div>";
        }
    }
}