using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using CookooMVCApp.Models;
using System.Globalization;
using CookooOrc;

namespace CookooMVCApp.Controllers {
    public class BaseController : Controller {

        protected int GetCurrentUserID() {
            return WebSecurity.GetUserId(User.Identity.Name);
        }

        protected void ConfigUserLng() {

            var context = new UsersContext();

            if (User != null) {
                var l_userID = WebSecurity.GetUserId(User.Identity.Name);
                var l_user = context.UserProfiles.FirstOrDefault((x) => x.UserId == l_userID);

                CultureInfo l_CultureInfo = new CultureInfo(l_user.Language);
                if (l_CultureInfo.TextInfo.IsRightToLeft) {
                    ViewBag.Direction = "rtl";
                }
                else {
                    ViewBag.Direction = "ltr";
                }

                LanguageEnum l_CuurentLang = DataUtils.GetLanguageEnum(l_user.Language);
                ViewBag.ConstStrings = LngFactory.GetLng(l_CuurentLang);
            }
        }
    }
}