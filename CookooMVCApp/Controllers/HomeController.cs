using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CookooMVCApp.Models;
using CookooOrc.interfaces;
using CookooOrc;
using WebMatrix.WebData;
using CookooMVCApp.Filters;
using System.Drawing;
using System.IO;
using CookooDAL.Implemented;
using System.Globalization;
using System.Web.UI;

namespace CookooMVCApp.Controllers.Home
{
    [Authorize(Roles = "User, Admin")]
    public class HomeController : BaseController
   {
        private readonly int regularPicHegiht = 300;
        private readonly int smallPicHegiht = 50;
        private readonly int MaxRecipesInOnePage = 20;
        private readonly string GetPhtoPath = "/Home/GetPhoto?a_ID=";

        private readonly IDataAcess DataAcess;
        private readonly IConstStrings ConstStrings;

        public HomeController() {
            DataAcess = new CookooModel();
        }

        public ActionResult RecipesAutoComplete(string term, bool? a_inCurrentUser) {

            JsonResult sendJson;

            int? l_userHaveRecipe = null;

            if (a_inCurrentUser.HasValue && a_inCurrentUser.Value) {
                l_userHaveRecipe = GetCurrentUserID();
            }

            var l_recpiesFomrDB =
                DataAcess.GetRecipes(null, l_userHaveRecipe, null, term, null, 10, null);

            var l_recipes = from c in l_recpiesFomrDB
                            select c.RecipeName;

            sendJson = Json(l_recipes, JsonRequestBehavior.AllowGet);

            // Return the result set as JSON
            return sendJson;
        }

       public ActionResult UsersAutoComplete(string term){

            JsonResult sendJson;

            using (var ctx = new UsersContext()) {
                
                var l_users =(from c in ctx.UserProfiles
                          where c.UserName.Contains(term)
                         select c.UserName).ToList();

                sendJson = Json(l_users, JsonRequestBehavior.AllowGet);
            }
     
          // Return the result set as JSON
            return sendJson;
        }

        public ActionResult GetPhoto(int? a_ID)
        {
            if (a_ID == null)
                return null;

            var img = DataAcess.GetImageByID(a_ID.Value);

            if (img==null) {
                return null;
            }
            return File(img.ImageData,img.ContentType);
        }

        public ActionResult Index(bool? withLayout)
        {
            ConfigUserLng();
            ViewBag.WithLayout = withLayout;
            return View(); 
        }

        public ActionResult AdvencedSearch() {
            ConfigUserLng();

            ConfigCategorieListItem();
            SearchModel l_model;
            if (Session["SearchCookooModel"] == null) {
                l_model = new SearchModel();
                l_model.SearchInCurrentUserBook = false;
                Session["SearchCookooModel"] = l_model;
            }
           
            l_model = ((SearchModel)Session["SearchCookooModel"]);

            return PartialView("AdvencedSearch", l_model);
        }

        [ChildActionOnly]
        public ActionResult SearchRegion() {

            if (!User.Identity.IsAuthenticated) {
                return null;
            }

            ConfigUserLng();

            SearchModel l_model = new SearchModel();
            l_model.OwnerName = User.Identity.Name;
            
            if (Session["SearchCookooModel"] == null) {
                l_model.SearchInCurrentUserBook = false;

                Session["SearchCookooModel"] = l_model;
            }

            l_model.SearchInCurrentUserBook = ((SearchModel)Session["SearchCookooModel"]).SearchInCurrentUserBook;

            ConfigCategorieListItem();
            return View(l_model);
        }

        public ActionResult AddNewCkRecipe()
        {
            ConfigViewBagToAddCkRcp();

            return PartialView("CkRecipeForm");
        }

        public ActionResult EditCkRecipe(long? a_ID) {

            ConfigViewBagToEditCkRcp();

            CkRtnRcp l_rec = DataAcess.GetCKRecicpe(a_ID.Value);

            if (l_rec == null)
                return View("Error", new HandleErrorInfo(new Exception("recipe is null"), "Home", "GetCKRecipe"));  

            SndCKRcpModel l_sndModel = new SndCKRcpModel();
            l_sndModel.RecipeName = l_rec.RecipeName;
            l_sndModel.Description = l_rec.Description;
            l_sndModel.DoesCount = l_rec.DoesCount;
            l_sndModel.Ingredients = l_rec.Ingredients;
            l_sndModel.Instructions = l_rec.Instructions;

            l_sndModel.ID = a_ID.Value;

            if (l_rec.PreparationTimeMin.HasValue) {
                l_sndModel.PreparationHourTime = l_rec.PreparationTimeMin / 60;
                l_sndModel.PreparationMinuteTime = l_rec.PreparationTimeMin % 60;
            }

            int[] ctgrs = new int[l_rec.Categories.Count];

            for (int i = 0; i < ctgrs.Length; i++) {
                ctgrs[i] = (int)l_rec.Categories[i];
            }

            l_sndModel.SelectedCtgrs = ctgrs;



            return PartialView("CkRecipeForm", l_sndModel);
        }

        [HttpPost]
        public ActionResult SaveCkRecipe(SndCKRcpModel a_model ,bool? a_IsEditMode)
        {
            if (!ModelState.IsValid) {
                if (a_IsEditMode.Value) {
                    ConfigViewBagToEditCkRcp();
                }
                else {
                    ConfigViewBagToAddCkRcp();
                }

                ViewBag.WithLayot = true;
                return View("CkRecipeForm",a_model);
            }

            IDataAcess data = DataAcess;

            Tuple<CookooImage, CookooImage> Images = new Tuple<CookooImage,CookooImage>(null,null);

            if (a_model.FileModel != null && a_model.FileModel.files[0] != null) {
                Images = GetImageFromPost(a_model.FileModel.files[0]);
            }

            if (a_model.PreparationHourTime == null)
                a_model.PreparationHourTime = 0;
            if (a_model.PreparationMinuteTime == null)
                a_model.PreparationMinuteTime = 0;

            var time = a_model.PreparationMinuteTime.Value + (a_model.PreparationHourTime.Value * 60);
            int userId = WebSecurity.GetUserId(User.Identity.Name);

            List<CategoryEnum> l_ctgrs = GetCategoriesFromModel(a_model);

            CkSndRcp ckRcp = a_model.CkRcpSnd;
            ckRcp.Categories = l_ctgrs;
            ckRcp.PreparationTimeMin = time;
            ckRcp.CKRcpDetailsImage = Images.Item1;
            ckRcp.CKRcpImage = Images.Item2;
            ckRcp.RcpDetailsImageSrc = GetPhtoPath ;
            ckRcp.OwnerId = userId;
            ckRcp.RtnRecipeBaseID = a_model.ID;

            long l_newRcp;

            if (a_IsEditMode.Value)
	        {
                 data.EditCkRcp(ckRcp);
                 l_newRcp = ckRcp.RtnRecipeBaseID.Value;
	        }
            else
	        {
                 l_newRcp = data.InsertRecipe(ckRcp);
	        }

            return RedirectToAction("GetCKRecipe", "Home", new { a_ID = l_newRcp, a_WithLayot = true });
        }

        public ActionResult AddNewImgsRecipe() {

            ConfigUserLng();
            ConfigCategorieListItem();
            ViewBag.IsEditMode = false;

            return PartialView("ImgsRecipeForm");
        }

        public ActionResult EditImgsRecipe(long? a_ID) {

            ConfigUserLng();
            ConfigCategorieListItem();
            ViewBag.IsEditMode = true;

            ImgsRtnRcp l_rec = DataAcess.GetImgRecicpe(a_ID.Value);

            if (l_rec == null)
                return View("Error", new HandleErrorInfo(new Exception("recipe is null"), "Home", "GetCKRecipe"));

            SndImgsRcpModel l_sndRes = new SndImgsRcpModel();
            l_sndRes.Description = l_rec.Description;
            l_sndRes.RecipeName = l_rec.RecipeName;
            l_sndRes.ID = a_ID.Value;

            int[] ctgrs = new int[l_rec.Categories.Count];

            for (int i = 0; i < ctgrs.Length; i++) {
                ctgrs[i] = (int)l_rec.Categories[i];
            }

            l_sndRes.SelectedCtgrs = ctgrs;

            return PartialView("ImgsRecipeForm", l_sndRes);
        }

        /// <summary>
        /// This action dont post with ajax becouse i dont know how upload files with ajax.
        /// </summary>
        /// <param name="a_model"></param>
        /// <param name="a_IsEditMode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveImgsRecipe(SndImgsRcpModel a_model,bool? a_IsEditMode) {

            if (!ModelState.IsValid) {

                ConfigUserLng();
                ConfigCategorieListItem();

                if (a_IsEditMode.Value) {
                    ViewBag.IsEditMode = true;
                }
                else {
                    ViewBag.IsEditMode = false;
                }
                ViewBag.WithLayot = true;
                return View("ImgsRecipeForm", a_model);
            }

            IDataAcess data = DataAcess;

            List<CategoryEnum> l_ctgrs = GetCategoriesFromModel(a_model);

            ImgsSndRcp l_sndImgRcp = a_model.ImgRcpSnd;

            l_sndImgRcp.OwnerId = WebSecurity.GetUserId(User.Identity.Name);
            l_sndImgRcp.RcpDetailsImageSrc = GetPhtoPath;
            l_sndImgRcp.RtnRecipeBaseID = a_model.ID;

            if (a_model.files != null && a_model.files.Count() > 0) {

                l_sndImgRcp.RcpImages = new List<CookooImage>();

                for (int i = 0; i < a_model.files.Count(); i++) {

                    if ((a_model.files[i]) != null)
                        l_sndImgRcp.RcpImages.Add(GetImageFromPost(a_model.files[i], 0));

                }

                if (a_model.files[0] != null)
                    l_sndImgRcp.RcpDetailsImage = GetImageFromPost(a_model.files[0], smallPicHegiht);
            }

            l_sndImgRcp.Categories = l_ctgrs;

            long l_newRcp;
            if (a_IsEditMode.Value) {

                data.EditImageRcp(l_sndImgRcp);
                l_newRcp = l_sndImgRcp.RtnRecipeBaseID.Value;
            }
            else {
                l_newRcp = data.InsertRecipe(l_sndImgRcp);
            }

            return RedirectToAction("GetImagesRecipe", "Home", new { a_ID = l_newRcp, a_WithLayot = true });

          
        }

        public ActionResult AddNewUrlRecipe()
        {
            ConfigUserLng();
            ConfigCategorieListItem();
            ViewBag.IsEditMode = false;
            return PartialView("UrlRecipeForm");
        }

        public ActionResult EditUrlRecipe(long? a_ID) {

            ConfigUserLng();
            ConfigCategorieListItem();
            ViewBag.IsEditMode = true;

            UrlRcp l_rec = DataAcess.GetUrlRecicpe(a_ID.Value);

            if (l_rec == null)
                return View("Error", new HandleErrorInfo(new Exception("recipe is null"), "Home", "GetCKRecipe"));

            SndUrlRcpModel l_sndRes = new SndUrlRcpModel();
            l_sndRes.Description = l_rec.Description;
            l_sndRes.RecipeName = l_rec.RecipeName;
            l_sndRes.ID = a_ID.Value;

            int[] ctgrs = new int[l_rec.Categories.Count];

            for (int i = 0; i < ctgrs.Length; i++) {
                ctgrs[i] = (int)l_rec.Categories[i];
            }

            l_sndRes.Url = l_rec.RcpUrl;

            l_sndRes.SelectedCtgrs = ctgrs;

            return PartialView("UrlRecipeForm", l_sndRes);
        }

        [HttpPost]
        public ActionResult SaveUrlRecipe(SndUrlRcpModel model, bool? a_IsEditMode) {

            if (!ModelState.IsValid) {
                ConfigUserLng();
                ConfigCategorieListItem();

                if (a_IsEditMode.Value) {
                    ViewBag.IsEditMode = true;
                }
                else {
                    ViewBag.IsEditMode = false;
                }

                return PartialView("UrlRecipeForm");
            }

            List<CategoryEnum> l_ctgrs = GetCategoriesFromModel(model);

            IDataAcess data = DataAcess;

            UrlRcp l_sndUrlRcp = model.UrlRcpSnd;
            l_sndUrlRcp.OwnerId = WebSecurity.GetUserId(User.Identity.Name);
            l_sndUrlRcp.Categories = l_ctgrs;

            long l_newRcp;

            if (a_IsEditMode.Value) {
                data.EditUrlRcp(l_sndUrlRcp);
                l_newRcp = l_sndUrlRcp.RtnRecipeBaseID.Value;
            }
            else {
                 l_newRcp = data.InsertRecipe(l_sndUrlRcp);
            }


            return RedirectToAction("GetUrlRecipe", "Home", new { a_ID = l_newRcp, a_WithLayot = false});
        }

        [HttpPost]
        public ActionResult GetRecipes(SearchModel a_searchModel)
        {
            Session["SearchCookooModel"] = a_searchModel;

            ConfigUserLng();

            int userId = GetCurrentUserID();

            IDataAcess data = DataAcess;

            IList<CkRcpDetails> list = new List<CkRcpDetails>();

            int? l_userHaveRecipe = null;

            if (a_searchModel.SearchInCurrentUserBook) {
                l_userHaveRecipe = GetCurrentUserID();
            }

            int? l_OwneruserId = null;

            if (a_searchModel.OwnerName != null) {
                l_OwneruserId = WebSecurity.GetUserId(a_searchModel.OwnerName);
            }

            // the 3rd checking,is to Search region if user dont select ctgry so it firts selected by defalut and it  0 by defalut.
            list = data.GetRecipes(l_OwneruserId, l_userHaveRecipe, (a_searchModel.SelectedCtgrs == null || a_searchModel.SelectedCtgrs.Count() == 0 || a_searchModel.SelectedCtgrs[0] == 0) ? null : a_searchModel.SelectedCtgrs,
                                   a_searchModel.SearchText,
                                   a_searchModel.Discription,MaxRecipesInOnePage + 1,a_searchModel.LastIndex);
            ModelState.Clear();

            //If user dont have rcps in DB.
            ViewBag.DontExsistRcps = false;
            if (list.Count == 0 && a_searchModel.LastIndex == 0) {

                ViewBag.DontExsistRcps = true;

                return View(a_searchModel);
            }

            // this mean user have rcps but no more than in MaxRecipesInOnePage.
            ViewBag.ExssitMore = true;
            if (list.Count() <  MaxRecipesInOnePage + 1)
            {
                ViewBag.ExssitMore = false;    
            }
  

            if (list.Count != 0) {

                var displayList = list.Take(MaxRecipesInOnePage);
                a_searchModel.LastIndex = displayList.Last().RtnRecipeBaseID.Value;

                List<RcpSearchResModel> l_resList = new List<RcpSearchResModel>();

                foreach (var item in displayList) {
                    l_resList.Add(new RcpSearchResModel(item));
                }

                ViewBag.Recipes = l_resList;
            }

            ConfigCategorieListItem();
            return PartialView("GetRecipes", a_searchModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_ID"></param>
        /// <param name="a_WithLayot">After saving cookoo recipe, the with loayot is true, becouse i dont find way to upload files with ajax</param>
        /// <returns></returns>
        public ActionResult GetCKRecipe(long? a_ID,bool? a_WithLayot)
        {

            ViewBag.WithLayot = true;
            if (a_WithLayot.HasValue && !a_WithLayot.Value) {
                ViewBag.WithLayot = false;
            }

            ConfigUserLng();
            if (a_ID == null)
            {
                return View("Error", new HandleErrorInfo(new Exception("recipe is null"), "Home", "GetCKRecipe"));              
            }

            var recipe = DataAcess.GetCKRecicpe(a_ID.Value);

            if (recipe == null)
                return View("Error", new HandleErrorInfo(new Exception("recipe is null"), "Home", "GetCKRecipe"));  

            ConfigGetRcp(recipe);

            return View(recipe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_ID"></param>
        /// <param name="a_WithLayot">After saving cookoo recipe, the with loayot is true, becouse i dont find way to upload files with ajax</param>
        /// <returns></returns>
        public ActionResult GetImagesRecipe(long? a_ID,bool? a_WithLayot) {


            ViewBag.WithLayot = true;
            if (a_WithLayot.HasValue && !a_WithLayot.Value) {
                ViewBag.WithLayot = false;
            }

            if (a_ID == null) {
                return View("Error", new HandleErrorInfo(new Exception("recipe is null"), "Home", "GetCKRecipe")); 
            }

            ConfigUserLng();

            var recipe = DataAcess.GetImgRecicpe(a_ID.Value);

            ConfigGetRcp(recipe);


            return View(recipe);
        }

        public ActionResult GetUrlRecipe(long? a_ID, bool? a_WithLayot) {

            ViewBag.WithLayot = true;
            if (a_WithLayot.HasValue && !a_WithLayot.Value) {
                ViewBag.WithLayot = false;
            }

            if (a_ID == null) {
                return View("Error", new HandleErrorInfo(new Exception("recipe is null"), "Home", "GetCKRecipe"));
            }

            ConfigUserLng();

            var recipe = DataAcess.GetUrlRecicpe(a_ID.Value);

            ConfigGetRcp(recipe);

            return View(recipe);
        }

        private void ConfigGetRcp(CkRcpDetails ckRcpDetails) {

            var context = new UsersContext();
            string l_ownerName = context.UserProfiles.FirstOrDefault(x => x.UserId == ckRcpDetails.OwnerId).UserName;
            ViewBag.OwnerName = l_ownerName;

            var l_currentUser = GetCurrentUserID();

            ViewBag.CanEdit = false;
            ViewBag.CanDelete = false;

            if (User.IsInRole("Admin")) {
                ViewBag.CanEdit = true;
                ViewBag.CanDelete = true;
            }
            else if(ckRcpDetails.OwnerId == l_currentUser) {
                ViewBag.CanEdit = true;
            }

            ViewBag.CanRemove = false;
            ViewBag.CanAdd = true;

            if (DataAcess.IfUserHaveRecipe(ckRcpDetails.RtnRecipeBaseID.Value, l_currentUser)) {
                ViewBag.CanRemove = true;
                ViewBag.CanAdd = false;
            }

            ConfigUsersListItem();
        }

        public ActionResult RemoveRecipeUser(long? a_ID) {

            if (a_ID == null) {
                return View("Error", new HandleErrorInfo(new Exception("recipe is null"), "Home", "GetCKRecipe"));
            }


            DataAcess.RemoveExsistRcpToUser(a_ID.Value, GetCurrentUserID());

            return RedirectToAction("Index", "Home");

        }

        public ActionResult AddRecipeUser(CkRcpDetails a_model) {

            if (a_model == null || !a_model.RtnRecipeBaseID.HasValue ) {
                return View("Error", new HandleErrorInfo(new Exception("recipe is null"), "Home", "GetCKRecipe"));
            }

            DataAcess.AddExsistRcpToUser((long)a_model.RtnRecipeBaseID, GetCurrentUserID());

            switch (a_model.RecipeSrc) {
                case RecipeSrcEnum.CooKooRecipe:
                     return RedirectToAction("GetCKRecipe", "Home", new { a_ID = a_model.RtnRecipeBaseID, a_WithLayot = false });
                case RecipeSrcEnum.UrlRecipe:
                     return RedirectToAction("GetUrlRecipe", "Home", new { a_ID = a_model.RtnRecipeBaseID, a_WithLayot = false });
                case RecipeSrcEnum.ImageRecipe:
                     return RedirectToAction("GetImagesRecipe", "Home", new { a_ID = a_model.RtnRecipeBaseID, a_WithLayot = false });
                default:
                     return View("Error", new HandleErrorInfo(new Exception("recipe Action un knowen"), "Home", "AddRecipeUser"));
            }
        }

        public ActionResult DeleteRcp(long? a_ID) {

            if (a_ID == null) {
                return View("Error", new HandleErrorInfo(new Exception("recipe is null"), "Home", "GetCKRecipe"));
            }

            if (User.IsInRole("Admin")) {
                DataAcess.DeleteRecipe(a_ID.Value, GetPhtoPath);
            }

            return RedirectToAction("Index", "Home");
        }

        private void ConfigCategorieListItem() {

            var l_constStrings = ((IConstStrings)ViewBag.ConstStrings).Categories;

            var l_categoriesArray = Enum.GetValues(typeof(CategoryEnum));

            List<SelectListItem> l_ctgrs = new List<SelectListItem>();

            for (int i = 0; i < l_categoriesArray.Length; i++) {

                l_ctgrs.Add(new SelectListItem() { Value = (i + 1).ToString(), Text = l_constStrings[(CategoryEnum)(i + 1)] });
            }

            l_ctgrs.Sort(new CtgryComperer());
            ViewBag.SelectListCtgrs = new SelectList(l_ctgrs, "Value", "Text");
        }

        private void ConfigUsersListItem() {

            List<SelectListItem> l_ctgrs = new List<SelectListItem>();

            List<UserProfile> l_users;

            using (var ctx = new UsersContext())
            {
                l_users = ctx.UserProfiles.ToList();
            }

           

            foreach (var item in l_users) {
                l_ctgrs.Add(new SelectListItem() { Value = item.UserId.ToString(), Text = item.UserName });
            }

            l_ctgrs.Sort(new CtgryComperer());

            l_ctgrs.Insert(0, new SelectListItem { Value = null, Text = null });

            ViewBag.SelectListUsers = new SelectList(l_ctgrs, "Value", "Text");
        }
        private Tuple<CookooImage, CookooImage> GetImageFromPost(HttpPostedFileBase file) {

            return new Tuple<CookooImage, CookooImage>(GetImageFromPost(file, smallPicHegiht),
                                                       GetImageFromPost(file, regularPicHegiht));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="a_maxHegiht">0 to don't limit</param>
        /// <returns></returns>
        private CookooImage GetImageFromPost(HttpPostedFileBase file, int a_maxHegiht) {
            CookooImage l_Image = null;

            if (file != null) {

                Bitmap bmImg = new Bitmap(file.InputStream);
                if (a_maxHegiht!=0 && bmImg.Height > a_maxHegiht) {
                    double dred = bmImg.Height / a_maxHegiht;
                    double fred = bmImg.Width / dred;
                    bmImg = new Bitmap(bmImg, (int)fred, a_maxHegiht);
                }
                ImageConverter imgConv = new ImageConverter();

                var regBytes = (byte[])imgConv.ConvertTo(bmImg, typeof(byte[]));
                l_Image = new CookooImage(file.FileName, file.ContentType, regBytes);
            }

            return l_Image;
        }

        class CtgryComperer : IComparer<SelectListItem> {
            public int Compare(SelectListItem x, SelectListItem y) {
                return x.Text.CompareTo(y.Text);
            }
        }

        private void ConfigViewBagToAddCkRcp() {
            ConfigUserLng();
            ConfigTimeViewBag();
            ConfigCategorieListItem();
            ViewBag.IsEditMode = false;
        }

        private void ConfigViewBagToEditCkRcp() {
            ConfigUserLng();
            ConfigTimeViewBag();
            ConfigCategorieListItem();
            ViewBag.IsEditMode = true;
        }

        private List<CategoryEnum> GetCategoriesFromModel(BaseRcpModel model) {

            List<CategoryEnum> l_rtnList = new List<CategoryEnum>();

            int l_count = model.SelectedCtgrs.Count();

            for (int i = 0; i < l_count; i++) {

                l_rtnList.Add((CategoryEnum)model.SelectedCtgrs[i]);
            }

            return l_rtnList;
        }

        private void ConfigTimeViewBag() {
            List<string> l_minutes = new List<string>();

            for (int i = 0; i < 61; i += 5) {
                l_minutes.Add(i.ToString());
            }

            List<string> l_houers = new List<string>();

            for (int i = 0; i < 13; i++) {
                l_houers.Add(i.ToString());
            }

            ViewBag.Minutes = l_minutes;
            ViewBag.Houers = l_houers;
        }

        [HttpPost]
        public bool SendRcpToUsers(CkRcpDetails a_model) 
        {
            IConstStrings l_constStrings;
            using (var context = new UsersContext()){

                var l_userID = WebSecurity.GetUserId(User.Identity.Name);
                var l_user = context.UserProfiles.FirstOrDefault((x) => x.UserId == l_userID);
                LanguageEnum l_CuurentLang = DataUtils.GetLanguageEnum(l_user.Language);
                l_constStrings = LngFactory.GetLng(l_CuurentLang);
            }

            

             var l_stringUser = Request["UserSendTo"].Split(',');
             List<int> l_Users = new List<int>();
             for (int i = 0; i < l_stringUser.Length; i++) {
                 l_Users.Add(int.Parse(l_stringUser[i]));
             }

            string l_description = Request["DescriptionSendTo"];

             string linkAction = string.Empty;

             switch (a_model.RecipeSrc) {
                 case CookooOrc.RecipeSrcEnum.CooKooRecipe:

                 linkAction = "GetCKRecipe";
                 break;
                 case CookooOrc.RecipeSrcEnum.UrlRecipe:

                 linkAction = "GetUrlRecipe";
                 break;
                 case CookooOrc.RecipeSrcEnum.ImageRecipe:

                 linkAction = "GetImagesRecipe";
                 break;
             }

             string l_recUrl = Url.Action(linkAction, "Home", new { a_ID = a_model.RtnRecipeBaseID, a_WithLayot = true });

          
            StringWriter l_stringWriter = new StringWriter();
            using (HtmlTextWriter l_writer = new HtmlTextWriter(l_stringWriter)) { 

                

                l_writer.RenderBeginTag(HtmlTextWriterTag.Label);

                //strat build title msg

                l_writer.Write(User.Identity.Name + l_constStrings.ShareMgsTitle);

                //Link
                l_writer.AddAttribute(HtmlTextWriterAttribute.Href,l_recUrl);
                l_writer.RenderBeginTag(HtmlTextWriterTag.A);
                l_writer.Write(a_model.RecipeName);
                l_writer.RenderEndTag();

                l_writer.RenderEndTag();

            }


            DataAcess.SaveMessage(GetCurrentUserID(), l_Users, l_stringWriter.ToString(), l_description);
            return true;
        }
    }
}
