using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookooOrc;
using System.Data.SqlClient;

namespace CookooDAL {
    public interface ISqlCommand {

        string GetAllIRecipes();
        string GetAllICategories();

        string GetAllIDoesTypes();

        string GetAllRecipeTypes();

        string InsertNewImage();

        string InsertNewRecipe();

        //string GetRecipes(int userId, int a_categoryID, string a_recipeName);
        //string GetRecipes(int userId, int? a_categoryID);
        //string GetRecipes(int userId);
       // SqlCommand GetRecipes(int userId);
        string GetRecipes();

        string InsertRecipeToUser();
        string GetImageByID();

        string GetRecipeById();

        string EditRecipeDetails();

        string EditImage();

        string EditRecipeImage();

        SqlCommand RemoveImages(List<int> list);

        SqlCommand GetRecipes(int? userId, int? a_categoryID, string a_recipeName, string a_discription, int? MaxRecipesInOnePage, long? minIndex);

        SqlCommand InsertNewMsg(int a_sendUserID, int a_toUserID,string a_msg, long? a_recipeID, DateTime a_dateTime);

        SqlCommand GetMsgsGet(int a_user);
        SqlCommand GetMsgsSend(int a_user);

    }
}
