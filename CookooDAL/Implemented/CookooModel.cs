using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using CookooOrc.interfaces;
using System.IO;
using System.Data.Objects.DataClasses;
using CookooOrc;
using System.Data.Objects;

namespace CookooDAL.Implemented
{
    public class CookooModel : IDataAcess
    {
        private CookooDBEntities Ctx { get; set; }

        public CookooModel()
        {

            Ctx = new CookooDBEntities();
    
            // Delete all rec
            //var ol_rec = Ctx.RecipeDetails.Where((x) => x.RcpDetailsID != 56).ToList();
            //foreach (var item in ol_rec) {
            //    DeleteRecipe(item.RcpDetailsID, "/Home/GetPhoto?a_ID=");
                
            //}
            //Ctx.SaveChanges();

            // Deletae all mesga
            //var l_msg = Ctx.Messages.ToList();
            //foreach (var item in l_msg) {
            //    Ctx.Messages.DeleteObject(item);
            //}
            //Ctx.SaveChanges();

        }

        public IList<CkCategory> GetCategories() {
            CkCategory dsakldsa = new CkCategory();
            List<CkCategory> retList = new List<CkCategory>();
            var stud = (from s in Ctx.Categories
                        select s
                            );
            foreach (var item in stud) {
                CkCategory ctg = new CkCategory() { ID = item.CtgryID, Name = item.CategoryName };
                retList.Add(ctg);
            }
            return retList; 
        }

        public long InsertRecipe(CkSndRcp a_recipe) {
           
            Image detImg = null;
            if (a_recipe.CKRcpDetailsImage != null) {
                detImg = new Image();
                detImg.ImageData = a_recipe.CKRcpDetailsImage.ImageData;
                detImg.ImageName = a_recipe.CKRcpDetailsImage.FileName;
                detImg.ContentType = a_recipe.CKRcpDetailsImage.ContentType;
                Ctx.Images.AddObject(detImg);
                Ctx.SaveChanges();
            }

            CooKooRecipe ckRcp = new CooKooRecipe();
            ckRcp.DoesCount = a_recipe.DoesCount;
            ckRcp.Ingredients = a_recipe.Ingredients;
            ckRcp.Instructions = a_recipe.Instructions;
            ckRcp.PreparationTimeMin = a_recipe.PreparationTimeMin;

            if (a_recipe.CKRcpImage != null) {
                Image rgImage = new Image();
                rgImage.ImageData = a_recipe.CKRcpImage.ImageData;
                rgImage.ImageName = a_recipe.CKRcpImage.FileName;
                rgImage.ContentType = a_recipe.CKRcpImage.ContentType;
                ckRcp.Image = rgImage;
            }

            string l_baseImgUrl = null;
            if (detImg != null) {
                l_baseImgUrl = a_recipe.RcpDetailsImageSrc + detImg.ImageID.ToString();
            }

            RecipeDetail rcpBase = ConvertType(a_recipe, l_baseImgUrl,RecipeSrcEnum.CooKooRecipe);

            rcpBase.CooKooRecipe = ckRcp;
           
            AddExsistRcpToUser(rcpBase, a_recipe.OwnerId);

            Ctx.SaveChanges();

            return rcpBase.RcpDetailsID;      
        }

        public long InsertRecipe(ImgsSndRcp a_recipe) {

            EntityCollection<ImageRecipe> l_imgsRcp = new EntityCollection<ImageRecipe>();

            foreach (var item in a_recipe.RcpImages) {
                Image Img = new Image();

                Img.ImageData = item.ImageData;
                Img.ImageName = item.FileName;
                Img.ContentType = item.ContentType;

                ImageRecipe imgRcp = new ImageRecipe();
                imgRcp.Image = Img;

                l_imgsRcp.Add(imgRcp);
            }

            Image detImg = new Image();

            detImg.ImageData = a_recipe.RcpDetailsImage.ImageData;
            detImg.ImageName = a_recipe.RcpDetailsImage.FileName;
            detImg.ContentType = a_recipe.RcpDetailsImage.ContentType;
            Ctx.Images.AddObject(detImg);
            Ctx.SaveChanges();

            string l_baseImgUrl = a_recipe.RcpDetailsImageSrc + detImg.ImageID.ToString();

            RecipeDetail rcpBase = ConvertType(a_recipe, l_baseImgUrl, RecipeSrcEnum.ImageRecipe);

            rcpBase.ImageRecipes = l_imgsRcp;

            AddExsistRcpToUser(rcpBase, a_recipe.OwnerId);
            Ctx.SaveChanges();
            return rcpBase.RcpDetailsID;
        }

        public long InsertRecipe(UrlRcp a_recipe) {
            UrlRecipe l_rcp = new UrlRecipe();
            l_rcp.Url = a_recipe.RcpUrl;

            RecipeDetail rcpBase = ConvertType(a_recipe, null, RecipeSrcEnum.UrlRecipe);
            rcpBase.UrlRecipe = l_rcp;

            AddExsistRcpToUser(rcpBase, a_recipe.OwnerId);

            Ctx.SaveChanges();
            return rcpBase.RcpDetailsID;
        }

        private bool AddExsistRcpToUser(RecipeDetail a_rcp, int a_userID) {

            User_Recipe userRcp = new User_Recipe();
            userRcp.UserID = a_userID;
            userRcp.RecipeDetail = a_rcp;

            Ctx.User_Recipe.AddObject(userRcp);

            return true;
        }

        public CookooImage GetImageByID(int imageID) {

            CookooImage l_rtnImage = null;

            var l_dbImage = Ctx.Images.FirstOrDefault(o => o.ImageID == imageID);

            if (l_dbImage != null) {
                l_rtnImage = new CookooImage(l_dbImage.ImageName, l_dbImage.ContentType, l_dbImage.ImageData);
            }

            return l_rtnImage;
        }

        public IList<CkRcpDetails> GetRecipes(int? a_OwnerID, int? userId, int[] a_ctgrs, string a_SearchText, string a_discription, int? MaxRecipesInOnePage, long? minIndex) {
            StringBuilder sb = new StringBuilder(@"SELECT ");
            string RecipesTable = "RecipeDetails";
            List<SqlParameter> prms = new List<SqlParameter>();
            if (MaxRecipesInOnePage.HasValue) {
                sb.Append(" TOP " + MaxRecipesInOnePage.Value);
            }

            sb.Append(" * FROM " + RecipesTable + " WHERE 1=1");

            if (minIndex.HasValue) {
                sb.Append(" AND RcpDetailsID  > @0");
                prms.Add(new SqlParameter("@0", minIndex.Value));
            }

            if (a_OwnerID.HasValue && a_OwnerID != -1) {
                 sb.Append(" AND (OwnerID=@6)");
                prms.Add(new SqlParameter("@6", a_OwnerID.Value));
            }

            if (userId.HasValue && userId != -1) {
                sb.Append(" AND (RcpDetailsID IN (SELECT RecipeID FROM User_Recipe WHERE UserID=@1))");
                prms.Add(new SqlParameter("@1", userId));
            }

            if (!String.IsNullOrEmpty(a_discription)) {
                sb.Append(" AND Description LIKE '%'+ @2 + '%'");
                prms.Add(new SqlParameter("@2", a_discription));
            }

            if (a_ctgrs != null) {

                string ctgrsQu = string.Empty;

                for (int i = 0; i < a_ctgrs.Length; i++) {
                    
                    ctgrsQu += " CtgryID = " + (int)a_ctgrs[i];

                    if (i != a_ctgrs.Length - 1)
	                {
                        ctgrsQu += " OR ";
	                }
                }

                sb.Append(" AND (RcpDetailsID IN (SELECT RcpDetailsID FROM Rcp_Category WHERE " + ctgrsQu + "))");
            }

            if (!String.IsNullOrEmpty(a_SearchText)) {
                sb.Append(" AND ((Name LIKE '%'+ @4 + '%') OR Description LIKE '%'+ @5 + '%')");
                prms.Add(new SqlParameter("@4", a_SearchText));
                prms.Add(new SqlParameter("@5", a_SearchText));

            }


            var baseRcps = Ctx.ExecuteStoreQuery<RecipeDetail>(sb.ToString(), prms.ToArray());

            List<CkRcpDetails> rtnList = new List<CkRcpDetails>();
            foreach (RecipeDetail item in baseRcps) {
                
                CkRcpDetails rcp = ConvertType(item);
                rtnList.Add(rcp);
            }

            return rtnList;
        }

        public CkRcpDetails GetDetailsRecicpe(long ckRecipeId) {
            var l_rcp = Ctx.RecipeDetails.FirstOrDefault(o=> o.RcpDetailsID == ckRecipeId);
            return ConvertType(l_rcp);
        }

        public CkRtnRcp GetCKRecicpe(long ckRecipeId) {

            var l_CKRcpID = Ctx.CooKooRecipes.FirstOrDefault(o => o.CKRcpID == ckRecipeId);

            if (l_CKRcpID == null)
                return null;
            return ConvertType(l_CKRcpID);
        }

      

        public ImgsRtnRcp GetImgRecicpe(long imRecipeId) {
            var l_imgsRcp = from imgRcp in Ctx.ImageRecipes
                            where imgRcp.ImgRcp == imRecipeId
                            select imgRcp;

            return ConvertType(l_imgsRcp.ToList());
        }

        public UrlRcp GetUrlRecicpe(long urRecipeId) {
            var l_imgRcpID = Ctx.UrlRecipes.FirstOrDefault(o => o.UrlRcpID == urRecipeId);
            return ConvertType(l_imgRcpID);
        }


        public void EditCkRcp(CkSndRcp a_recipe) {

            var l_recp = Ctx.CooKooRecipes.FirstOrDefault(x => x.CKRcpID == a_recipe.RtnRecipeBaseID.Value);
            l_recp.Ingredients = a_recipe.Ingredients;
            l_recp.Instructions = a_recipe.Instructions;
            l_recp.PreparationTimeMin = a_recipe.PreparationTimeMin;
            l_recp.DoesCount = a_recipe.DoesCount;

            l_recp.RecipeDetail.Name = a_recipe.RecipeName;
            l_recp.RecipeDetail.Description = a_recipe.Description;

            var Rcps_Categorys = l_recp.RecipeDetail.Rcp_Category.ToList();

            foreach (var item in Rcps_Categorys)
	        {
                Ctx.Rcp_Category.DeleteObject(item);
	        }

            a_recipe.Categories.ForEach(x => {

                Rcp_Category l_ctgry = new Rcp_Category();
                l_ctgry.CtgryID = (int)x;
                l_recp.RecipeDetail.Rcp_Category.Add(l_ctgry);
            });

            Image detImg = null;
            if (a_recipe.CKRcpDetailsImage != null) {
                detImg = new Image();
                detImg.ImageData = a_recipe.CKRcpDetailsImage.ImageData;
                detImg.ImageName = a_recipe.CKRcpDetailsImage.FileName;
                detImg.ContentType = a_recipe.CKRcpDetailsImage.ContentType;
                Ctx.Images.AddObject(detImg);
                Ctx.SaveChanges();


                if (l_recp.RecipeDetail.ImageUrl != null) {
                    string l_strImgid = l_recp.RecipeDetail.ImageUrl.Remove(0, a_recipe.RcpDetailsImageSrc.Count());
                    int imageId = int.Parse(l_strImgid);
                    var image = Ctx.Images.FirstOrDefault(x => x.ImageID == imageId);
                    Ctx.Images.DeleteObject(image);
                }
             

                l_recp.RecipeDetail.ImageUrl = a_recipe.RcpDetailsImageSrc + detImg.ImageID.ToString();
            }

            Image l_deleteImg = null;

            if (a_recipe.CKRcpImage != null) {
                Image rgImage = new Image();
                rgImage.ImageData = a_recipe.CKRcpImage.ImageData;
                rgImage.ImageName = a_recipe.CKRcpImage.FileName;
                rgImage.ContentType = a_recipe.CKRcpImage.ContentType;
                l_deleteImg = l_recp.Image;
                l_recp.Image = rgImage;
            }
            Ctx.SaveChanges();

            if (l_deleteImg!= null)
                Ctx.Images.DeleteObject(l_deleteImg);

            Ctx.SaveChanges();
        }

        public void EditImageRcp(ImgsSndRcp a_recipe) {
            var l_recp = Ctx.RecipeDetails.FirstOrDefault(x => x.RcpDetailsID == a_recipe.RtnRecipeBaseID.Value);
            l_recp.Name = a_recipe.RecipeName;
            l_recp.Description = a_recipe.Description;

            // Replace Images
            if (a_recipe.RcpImages.Count > 0) {

                var l_deltaeImgs = new List<Image>();

                var l_imgRecps = l_recp.ImageRecipes;
                foreach (var item in l_imgRecps)
	            {
                    l_deltaeImgs.Add(item.Image);
	            }

                // Chenge Catgrs
                var Rcps_Categorys = l_recp.Rcp_Category.ToList();

                foreach (var item in Rcps_Categorys) {
                    Ctx.Rcp_Category.DeleteObject(item);
                }

                a_recipe.Categories.ForEach(x => {

                    Rcp_Category l_ctgry = new Rcp_Category();
                    l_ctgry.CtgryID = (int)x;
                    l_recp.Rcp_Category.Add(l_ctgry);
                });

                // all recipes should be ImageUrl but maby thair is problem...
                if (l_recp.ImageUrl != null) {
                    string l_strImgid = l_recp.ImageUrl.Remove(0, a_recipe.RcpDetailsImageSrc.Count());
                    int imageId = int.Parse(l_strImgid);
                    var image = Ctx.Images.FirstOrDefault(x => x.ImageID == imageId);

                    if (image != null) {
                        Ctx.Images.DeleteObject(image); 
                    }
                }

                foreach (var item in a_recipe.RcpImages) {
                    Image Img = new Image();

                    Img.ImageData = item.ImageData;
                    Img.ImageName = item.FileName;
                    Img.ContentType = item.ContentType;

                    ImageRecipe imgRcp = new ImageRecipe();
                    imgRcp.Image = Img;

                    l_recp.ImageRecipes.Add(imgRcp);
                }


                Image detImg = new Image();

                detImg.ImageData = a_recipe.RcpDetailsImage.ImageData;
                detImg.ImageName = a_recipe.RcpDetailsImage.FileName;
                detImg.ContentType = a_recipe.RcpDetailsImage.ContentType;
                Ctx.Images.AddObject(detImg);
                Ctx.SaveChanges();

                string l_baseImgUrl = a_recipe.RcpDetailsImageSrc + detImg.ImageID.ToString();
                l_recp.ImageUrl = l_baseImgUrl;
                l_deltaeImgs.ForEach(x => Ctx.Images.DeleteObject(x));
            }

            Ctx.SaveChanges();
        }

        public void EditUrlRcp(UrlRcp a_recipe) {
            var l_recp = Ctx.RecipeDetails.FirstOrDefault(x => x.RcpDetailsID == a_recipe.RtnRecipeBaseID.Value);
            l_recp.Name = a_recipe.RecipeName;
            l_recp.Description = a_recipe.Description;

            // Chenge Catgrs
            var Rcps_Categorys = l_recp.Rcp_Category.ToList();

            foreach (var item in Rcps_Categorys) {
                Ctx.Rcp_Category.DeleteObject(item);
            }

            a_recipe.Categories.ForEach(x => {

                Rcp_Category l_ctgry = new Rcp_Category();
                l_ctgry.CtgryID = (int)x;
                l_recp.Rcp_Category.Add(l_ctgry);
            });

            l_recp.UrlRecipe.Url = a_recipe.RcpUrl;
            Ctx.SaveChanges();
        }

        public long AddMsg(int a_sendUserID, int a_toUserID, string a_msg, long? a_recipeID, DateTime a_dateTime) {
            throw new NotImplementedException();
        }

        public IList<DBMsg> GetMsgGet(int userID) {
            throw new NotImplementedException();
        }

        public IList<DBMsg> GetMsgSend(int userID) {
            throw new NotImplementedException();
        }

        private CkRcpDetails ConvertType(RecipeDetail a_RecipeDetail) {

            List<CategoryEnum> l_ctgrs = new List<CategoryEnum>();

            foreach (var item in a_RecipeDetail.Rcp_Category) {
                l_ctgrs.Add((CategoryEnum)item.CtgryID);
            }

            return new CkRcpDetails(a_RecipeDetail.RcpDetailsID, a_RecipeDetail.Name,
                a_RecipeDetail.Description,
                l_ctgrs, a_RecipeDetail.ImageUrl,
                a_RecipeDetail.OwnerID, (RecipeSrcEnum)a_RecipeDetail.RcpSrcTypeID);
        }

        private CkRtnRcp ConvertType(CooKooRecipe a_CKRcpID) {

            CkRcpDetails l_detailsRcp =  GetDetailsRecicpe(a_CKRcpID.CKRcpID);

            CkRtnRcp l_rcp = new CkRtnRcp(l_detailsRcp, a_CKRcpID.DoesCount, a_CKRcpID.Ingredients,
                a_CKRcpID.Instructions, a_CKRcpID.PreparationTimeMin, a_CKRcpID.RegularImageID);

            return l_rcp;
        }

        private ImgsRtnRcp ConvertType(List<ImageRecipe> a_imgsRcp) {

            CkRcpDetails l_detailsRcp =  GetDetailsRecicpe(a_imgsRcp.First().ImgRcp);

            List<long> l_imags = new List<long>();

             a_imgsRcp.ForEach((x)=> l_imags.Add(x.ImgID));

             ImgsRtnRcp l_rcp = new ImgsRtnRcp(l_detailsRcp, l_imags);
            return l_rcp;
        }

        private UrlRcp ConvertType(UrlRecipe a_urlRcp) {
            CkRcpDetails l_detailsRcp = GetDetailsRecicpe(a_urlRcp.UrlRcpID);
            UrlRcp l_rcp = new UrlRcp(l_detailsRcp, a_urlRcp.Url);

            return l_rcp;
        }

        private RecipeDetail ConvertType(CkRcpDetails a_ckRrcDtls, string a_imageUrl, RecipeSrcEnum a_RecipeSrcEnum) {

            RecipeDetail rcpBase = new RecipeDetail();
            rcpBase.Description = a_ckRrcDtls.Description;

            a_ckRrcDtls.Categories.ForEach(x => {

                Rcp_Category l_ctgry = new Rcp_Category();
                l_ctgry.CtgryID = (int)x;
                rcpBase.Rcp_Category.Add(l_ctgry);
            });

            rcpBase.Name = a_ckRrcDtls.RecipeName;
            rcpBase.OwnerID = a_ckRrcDtls.OwnerId;
            rcpBase.RcpSrcTypeID = (int)a_RecipeSrcEnum;
            rcpBase.ImageUrl = a_imageUrl;

            return rcpBase;

        }

        private CKMessage ConvertType(Message item) {
            return new CKMessage(item.MsgID, item.FromUserID, item.ToUserID, 
                item.MsgTitle, item.MsgContent, item.TimeCreated, item.IsRead);
        }

        public bool AddExsistRcpToUser(long a_rcp, int a_userID) {

            var l_recipe = Ctx.RecipeDetails.FirstOrDefault(x => x.RcpDetailsID == a_rcp);
            var l_user = Ctx.UserProfiles.FirstOrDefault(x => x.UserId == a_userID);

            if (l_user == null || l_recipe == null)
	        {
                return false;
	        }

            User_Recipe userRcp = new User_Recipe();
            userRcp.UserID = a_userID;
            userRcp.RecipeID = a_rcp;

            Ctx.User_Recipe.AddObject(userRcp);
            Ctx.SaveChanges();

            return true;
        }

        public bool RemoveExsistRcpToUser(long a_rcp, int a_userID) {
            var l_recipe = Ctx.RecipeDetails.FirstOrDefault(x => x.RcpDetailsID == a_rcp);
            var l_user = Ctx.UserProfiles.FirstOrDefault(x => x.UserId == a_userID);

            var l_user_rcp = Ctx.User_Recipe.FirstOrDefault(x => (x.RecipeID == a_rcp && x.UserID == a_userID));

            if (l_user_rcp == null) {
                return false;
            }

            Ctx.User_Recipe.DeleteObject(l_user_rcp);

            Ctx.SaveChanges();

            return true;
        }

        public IList<string> GetLanguages() {
            var l_result = from lng in Ctx.Languages
                         select lng.Language1;

            return l_result.ToList();
        }

        public bool IfUserHaveRecipe(long a_recipe, int a_user) {
            var l_result = Ctx.User_Recipe.FirstOrDefault(x => (x.UserID == a_user && x.RecipeID == a_recipe));
            return l_result != null;
        }





        public void DeleteRecipe(long a_rec, string a_imagePath) {
            var l_recp = Ctx.RecipeDetails.FirstOrDefault(x => x.RcpDetailsID == a_rec);

            if (l_recp.ImageUrl != null) {
                string l_strImgid = l_recp.ImageUrl.Remove(0, a_imagePath.Count());
                int imageId = int.Parse(l_strImgid);
                var image = Ctx.Images.FirstOrDefault(x => x.ImageID == imageId);

                if (image != null) {
                    Ctx.Images.DeleteObject(image);
                }
            }

            // If imgs 
            if (l_recp.ImageRecipes.Count != 0) {

                var l_imgs = l_recp.ImageRecipes.ToArray();

                foreach (var item in l_imgs)
	            {
                    Ctx.Images.DeleteObject(item.Image);
	            }
            }
            else if (l_recp.CooKooRecipe != null && l_recp.CooKooRecipe.Image != null) {
                Ctx.Images.DeleteObject(l_recp.CooKooRecipe.Image);
            }

            Ctx.RecipeDetails.DeleteObject(l_recp);

            Ctx.SaveChanges();
        }


        public List<long> SaveMessage(int a_FromUser, List<int> l_ToUsers, string p_title, string l_description) {

           
            List<Message> l_entMes = new List<Message>();

            foreach (var item in l_ToUsers) {
                Message l_mes = new Message();
                l_mes.FromUserID = a_FromUser;
                l_mes.IsRead = false;
                l_mes.MsgTitle = p_title;
                l_mes.MsgContent = l_description;
                l_mes.TimeCreated = DateTime.Now;
                l_mes.ToUserID = item;

                Ctx.Messages.AddObject(l_mes);
                l_entMes.Add(l_mes);
            }
           
            Ctx.SaveChanges();
            List<long> l_msg = new List<long>();

            l_entMes.ForEach(x => l_msg.Add(x.MsgID));
            return l_msg;
        }


        public List<CKMessage> GetMessages(int a_toUser, int a_maxMsg, long? a_minIndex) {

            string l_MessafesTableName = "Message";

            StringBuilder sb = new StringBuilder(@"SELECT TOP " + a_maxMsg + " * From " + l_MessafesTableName);

            sb.Append(" WHERE ToUserID=" + a_toUser);
            if (a_minIndex.HasValue) {
                sb.Append(" AND MsgID < " + a_minIndex.Value);
            }
            sb.Append(" Order By TimeCreated DESC");

            var l_msgs = Ctx.ExecuteStoreQuery<Message>(sb.ToString());

            List<CKMessage> l_retenList = new List<CKMessage>();

            foreach (Message item in l_msgs) {

                l_retenList.Add(ConvertType(item));
            }

            return l_retenList;
        }

        public int GetMessageDontRead(int a_user) {

            var l_res = (from l_messge in Ctx.Messages
                         where l_messge.ToUserID == a_user && !l_messge.IsRead
                         select l_messge).Count();

            return l_res;
        }


        public void SetMessageReading(bool a_IsReading, List<long> l_ids) {

            foreach (var item in l_ids) {
                var l_msg = Ctx.Messages.FirstOrDefault(x => x.MsgID == item);
                if (l_msg != null) {
                    l_msg.IsRead = a_IsReading;
                }
            }

            Ctx.SaveChanges();
        }
    }
}
