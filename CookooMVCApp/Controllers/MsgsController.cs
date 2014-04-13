using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using CookooOrc.interfaces;
using CookooDAL.Implemented;
using WebMatrix.WebData;
using CookooMVCApp.Models;
using CookooOrc;
using System.Globalization;


namespace CookooMVCApp.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "User, Admin")]
    public class MsgsController : BaseController
    {

        private readonly IDataAcess DataAcess;
        private readonly int MAX_MSG_PER_PAGE = 30;
       

        public MsgsController() {
            DataAcess = new CookooModel();

    
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetMessagesByTime() 
        {
            ConfigUserLng();

            long? l_lastIndex = null;
            if (Request["lastIndex"] != null)
	        {
		         l_lastIndex =  long.Parse(Request["lastIndex"]);
	        } 

            var l_msg = DataAcess.GetMessages(GetCurrentUserID(), MAX_MSG_PER_PAGE + 1, l_lastIndex);


            // this mean user have rcps but no more than in MaxRecipesInOnePage.
            ViewBag.ExssitMore = true;
            if (l_msg.Count() < MAX_MSG_PER_PAGE + 1) {
                ViewBag.ExssitMore = false;
            }

            List<MsgModel> l_rtrn = new List<MsgModel>();

            if (l_msg.Count > 0) {

                var l_helpList = l_msg.Take(MAX_MSG_PER_PAGE).ToList();

                ViewBag.LastMsgIndex = l_helpList.Last().MsgID;

                List<long> l_ids = new List<long>();

                l_helpList.ForEach(x =>{ 
                    l_rtrn.Add(new MsgModel(x));
                    l_ids.Add(x.MsgID);
                });
                

                DataAcess.SetMessageReading(true,l_ids);
            }

            return PartialView(l_rtrn);
        }

        public ActionResult ContactUs() 
        {
            ConfigUserLng();
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ContactUs(MsgModel a_model) {


            if (!ModelState.IsValid) {
                return View("ContactUs", a_model);
            }

            DataAcess.SaveMessage(GetCurrentUserID(), new List<int>{59,61}, a_model.Title, a_model.Content);

            return RedirectToAction("Index", "Home", new { withLayout = false });
        }

        [ChildActionOnly]
        public string GetNotReadingCount() 
        {
            int l_res = DataAcess.GetMessageDontRead(GetCurrentUserID());

            return l_res.ToString();
        }

    }
}
