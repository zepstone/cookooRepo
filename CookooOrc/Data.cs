using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Policy;

namespace CookooOrc
{

    public class DataUtils {
        public static string GetLanguage(LanguageEnum languageEnum) {
            switch (languageEnum) {
                case LanguageEnum.English:
                return "en";
                case LanguageEnum.עברית:
                return "he";
                default:
                return "en";
            }
        }

        public static LanguageEnum GetLanguageEnum(string l_lang) {
            switch (l_lang) {
                case ("he"):
                return LanguageEnum.עברית;
                default:
                return LanguageEnum.English;
            }
        }
    }



    public enum LanguageEnum {
        English = 1,
        עברית = 2,
    }

    public enum RecipeSrcEnum {
        CooKooRecipe = 1,
        UrlRecipe = 2,
        ImageRecipe = 3
    }

    public enum CategoryEnum {
        Soups = 1,
        Breads = 2,
        Meat = 3,
        Cake = 4,
        Fish = 5,
        Kids = 6,
        Salad = 7,
        Main_Course = 8,
        Dreesing = 9,
        Dessert = 10,
        Appetizer = 11,
        Healthy = 12,      
        Pastry = 13,
        Cookie = 14,
        Dip = 15,
        Pasta = 16,
        Dairy = 17,
        Pickles =18,
        Chicken =19,
        Vegetarian = 20,
        Fast = 21,
        Hollydays =22
    }

    public class CkCategory
    {
        public int ID { get;  set; }
        public string Name { get;  set; }
    }

    /// <summary>
    /// Hold recipe base data .
    /// </summary>
    public class CkRcpDetails  {

        public CkRcpDetails() { Categories = new List<CategoryEnum>(); }

        public CkRcpDetails(CkRcpDetails a_rcp) :
            this(a_rcp.RtnRecipeBaseID, a_rcp.RecipeName, a_rcp.Description,
            a_rcp.Categories, a_rcp.RcpDetailsImageSrc, a_rcp.OwnerId,a_rcp.RecipeSrc) { }

        public CkRcpDetails(long? a_RtnRecipeBaseID, string a_RecipeName, string a_Description,
                          List<CategoryEnum> a_ctg, string a_BaseImage, int a_Owner, RecipeSrcEnum a_RecipeSrc)
        {
            RtnRecipeBaseID = a_RtnRecipeBaseID;
            RecipeName = a_RecipeName;
            Description = a_Description;
            Categories = a_ctg;
            OwnerId = a_Owner;
            RcpDetailsImageSrc = a_BaseImage;
            RecipeSrc = a_RecipeSrc;
 
        }

        public long? RtnRecipeBaseID { get;  set; }
        public string RecipeName { get;  set; }
        public string Description { get;  set; }
        public List<CategoryEnum> Categories { get;  set; }
        public int OwnerId { get;  set; }
        public string RcpDetailsImageSrc { get; set; }
        public RecipeSrcEnum RecipeSrc { get; set; }
    }

    public abstract class CkBaseRc : CkRcpDetails {

        public CkBaseRc() {

        }

        public CkBaseRc(CkRcpDetails a_RcpDetails, int? a_DoesCount,
                        string a_Ingredients, string a_Instructions,
                        int? a_PreparationTimeMin)
            : base(a_RcpDetails) {

                DoesCount = a_DoesCount;
                Ingredients = a_Ingredients;
                Instructions = a_Instructions;
                PreparationTimeMin = a_PreparationTimeMin;
        }

        public int? DoesCount { get;  set; }
        public string Ingredients { get;  set; }
        public string Instructions { get;  set; }
        public int? PreparationTimeMin { get;  set; }
    }

    /// <summary>
    /// Hold Cookoo recipe data that send to DAL.
    /// </summary>
    public class CkSndRcp : CkBaseRc {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_RcpDetails">In a_BaseImage in a_RcpDetails
        ///  fill the base path, The Dal will add the image ID</param>
        public CkSndRcp() {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_RcpDetails">In a_BaseImage in a_RcpDetails
        ///  fill the base path, The Dal will add the image ID</param>
        public CkSndRcp(CkRcpDetails a_RcpDetails, int? a_DoesCount,
                        string a_Ingredients, string a_Instructions,
                        int? a_PreparationTimeMin, CookooImage a_CKRcpImage, CookooImage a_CKRcpDetailsImage)
            : base(a_RcpDetails, a_DoesCount, a_Ingredients, a_Instructions, a_PreparationTimeMin) {
                CKRcpImage = a_CKRcpImage;
                CKRcpDetailsImage = a_CKRcpDetailsImage;
        }

        public CookooImage CKRcpImage { get;  set; }
        public CookooImage CKRcpDetailsImage { get;  set; }

    }

    /// <summary>
    /// Hold Cookoo recipe data that return from DAL.
    /// </summary>
    public class CkRtnRcp : CkBaseRc {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_RcpDetails">In a_BaseImage in a_RcpDetails
        ///  fill the base path, The Dal will add the image ID</param>
        public CkRtnRcp(CkRcpDetails a_RcpDetails, int? a_DoesCount,
                        string a_Ingredients, string a_Instructions,
                        int? a_PreparationTimeMin, long? a_CKRcpImage)
            : base(a_RcpDetails, a_DoesCount, a_Ingredients, a_Instructions, a_PreparationTimeMin) {
            CKRcpImage = a_CKRcpImage;
        }

     //   public long CkSndRcpID { get; private set; }
        public long? CKRcpImage { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a_RcpDetails">In a_BaseImage in a_RcpDetails
    ///  fill the base path, The Dal will add the image ID</param>
    public class ImgsSndRcp : CkRcpDetails {

        public ImgsSndRcp() {
            RcpImages = new List<CookooImage>();
        }

        public ImgsSndRcp(CkRcpDetails a_RcpDetails, List<CookooImage> a_RcpImages)
            : base(a_RcpDetails) {
                RcpImages = a_RcpImages;
        }

        public List<CookooImage> RcpImages { get; set; }
        public CookooImage RcpDetailsImage { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ImgsRtnRcp : CkRcpDetails {
        public ImgsRtnRcp(CkRcpDetails a_RcpDetails, IList<long> a_RcpImages)
            : base(a_RcpDetails) {
            RcpImages = a_RcpImages;
        }

        public IList<long> RcpImages { get; private set; }
    }

    public class UrlRcp : CkRcpDetails {

        public UrlRcp() {}

        public UrlRcp(CkRcpDetails a_RcpDetails, string a_RcpUrl)
            : base(a_RcpDetails) {
                RcpUrl = a_RcpUrl;
        }

        public string RcpUrl { get; set; }
    }

    //public interface ICkRecipBaseData
    //{
    //    string RecipeName { get; }
    //    string Description { get; }
    //    string PrivateDescription { get; }
    //    int? DoesCount { get; }
    //    string Ingredients { get; }
    //    string Instructions { get; }
    //    int? DoseType { get; }
    //    int RecipeType { get; }
    //    int Category { get; }
    //    int OwnerId { get; }
    //    TimeSpan? PreparationTime { get; }
    //}
    //public interface ICkRecipeUserData: ICkRecipBaseData
    //{
    //    ICookooImage RegularImage { get; }
    //    ICookooImage SmallImage { get; }
    //}

    //public interface ICkRecipeDBData : ICkRecipBaseData
    //{
    //    long RecipeID { get; }
    //    int? RegularImageID { get; }
    //    int? SmallImageID { get; }
    //}

    public class CookooImage {

        public CookooImage() {

        }

        public CookooImage(string a_fileName, string a_ContentType, byte[] a_imageData) {
            FileName = a_fileName;
            ContentType = a_ContentType;
            ImageData = a_imageData;
        }

        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public byte[] ImageData { get; private set; }
    }


    public class CKMessage 
    {
        public long MsgID {get;set;}
        public int FromUserID { get; set; }
        public int ToUserID { get; set; }
        public string MsgTitle { get; set; }
        public string MsgContent { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool IsRead { get; set; }

        public CKMessage(long p, int p_2, int p_3, string p_4, string p_5, DateTime dateTime, bool p_6) {

            MsgID = p;
            this.FromUserID = p_2;
            this.ToUserID = p_3;
            this.MsgTitle = p_4;
            this.MsgContent = p_5;
            this.TimeCreated = dateTime;
            this.IsRead = p_6;
        } 
    }

    public class DBMsg
    {

        public long ID { get; private set; }

        public int SendUserID { get; private set; }

        public int ToUserID { get; private set; }

        public string Msg { get; private set; }

        public long? RecipeID { get; private set; }
        public DateTime time { get; private set; }
        public bool isReding { get; private set; }
    }
}
