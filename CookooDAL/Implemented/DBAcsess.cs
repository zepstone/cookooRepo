using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookooDAL.interfaces;
using System.Data.SqlClient;
using CookooOrc;

namespace CookooDAL.Implemented
{
    abstract public class DBAcsess : IDataAcess
    {

        abstract protected SqlConnection Connection { get; set; }
        abstract protected string ConnectionString { get; }
        abstract public ISqlCommand SqlQueries { get; set; }

        public DBAcsess(ISqlCommand a_ISqlQueries)
        {
            SqlQueries = a_ISqlQueries;
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Connection.StateChange += new System.Data.StateChangeEventHandler(Connection_StateChange);
        }

        void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            throw new NotImplementedException();
        }

        abstract public IList<ICkRecipeDBData> GetAllIRecipes();
        abstract public IList<ICategory> GetAllCategories();
        abstract public IList<IDoesType> GetAllDoesTypes();
        abstract public IList<IRecipeType> GetAllRecipeTypes();
        abstract protected int InsertNewImage(ICookooImage a_image);
        abstract public ICookooImage GetImageByID(int p);
        abstract public int InsertNewRecipe(ICkRecipeUserData a_recipe);

        abstract public IList<ICkRecipeDBData> GetRecipes();
        //abstract public IList<IRecipeDBData> GetRecipes(int userId);
        //abstract public IList<IRecipeDBData> GetRecipes(int userId, int a_categoryID);
        //abstract public IList<IRecipeDBData> GetRecipes(int userId, int a_categoryID, string a_recipeName);
        abstract public IList<ICkRecipeDBData> GetRecipes(int p, int? nullable, string p_2, string p_3, int? MaxRecipesInOnePage, long? minIndex);


        abstract public ICkRecipeDBData GetRecicpe(long recipeId);

        abstract public void EditRecipeDetails(long a_recipeID, ICkRecipBaseData a_recipe);
        abstract public void EditImage(long a_imageID, ICookooImage a_recipe);


        abstract public void EditRecipeImages(long a_recipeID, ICookooImage regImage, ICookooImage smllImage);


        abstract public long AddMsg(int a_sendUserID, int a_toUserID, string a_msg, long? a_recipeID, DateTime a_dateTime);


        abstract public IList<IDBMsg> GetMsgGet(int userID);


        abstract public IList<IDBMsg> GetMsgSend(int userID);
    }
}
