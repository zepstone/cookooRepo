using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookooOrc;
using System.Data.SqlClient;

namespace CookooOrc.interfaces {

    public interface IDataAcess {

        IList<CkCategory> GetCategories();

        long InsertRecipe(CkSndRcp a_recipe);
        long InsertRecipe(ImgsSndRcp a_recipe);
        long InsertRecipe(UrlRcp a_recipe);

        bool AddExsistRcpToUser(long a_rcp, int a_userID);
        bool RemoveExsistRcpToUser(long a_rcp, int a_userID);


        CookooImage GetImageByID(int imageID);

        IList<CkRcpDetails> GetRecipes(int? a_OwnerID, int? a_userID, int[] a_ctgrs, string a_serchText, string a_description, int? MaxRecipesInOnePage, long? minIndex);


        CkRcpDetails GetDetailsRecicpe(long ckRecipeId);
        CkRtnRcp GetCKRecicpe(long ckRecipeId);
        ImgsRtnRcp GetImgRecicpe(long imRecipeId);
        UrlRcp GetUrlRecicpe(long urRecipeId);

        /// <summary>
        /// This method dont changed the owner and image, use for EditImage
        /// </summary>
        /// <param name="a_recipeID"></param>
        /// <param name="a_recipe"></param>
        void EditCkRcp(CkSndRcp a_recipe);
        void EditImageRcp(ImgsSndRcp a_recipe);
        void EditUrlRcp(UrlRcp a_recipe);

        long AddMsg(int a_sendUserID, int a_toUserID,string a_msg, long? a_recipeID, DateTime a_dateTime);

        IList<DBMsg> GetMsgGet(int userID);
        IList<DBMsg> GetMsgSend(int userID);

        IList<string> GetLanguages();

        bool IfUserHaveRecipe(long a_recipe, int a_user);


        void DeleteRecipe(long a_rec, string a_imagePath);

        List<long> SaveMessage(int a_FromUser, List<int> l_ToUsers, string p_title, string l_description);

        List<CKMessage> GetMessages(int p, int MaxMsgInPage, long? a_minIndex);

        int GetMessageDontRead(int a_user);

        void SetMessageReading(bool a_IsReading, List<long> l_ids);
    }

    
}
