using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CookooOrc.interfaces;

namespace CookooDAL.Implemented {
    public class CookooCommands : ISqlCommand {

        private readonly string ImagesTable = "T_Images";
        private readonly string RecipesTable = "T_Recipes";
        private readonly string MsgTable = "T_Messages";

        public string GetAllIRecipes() {
            throw new NotImplementedException();
        }

        public string GetAllICategories() {
            return "SELECT * FROM T_Categories";
        }

        public string GetAllIDoesTypes()
        {
            return "SELECT * FROM T_DoseTypes ";
        }

        public string GetAllRecipeTypes()
        {
            return "SELECT * FROM T_RecipeTypes";
        }


        public string InsertNewImage()
        {
            return @"insert into T_Images(ImageName,ImageData,ContentType)values(@0, @1, @2) SELECT SCOPE_IDENTITY()";
        }

        public string InsertNewRecipe()
        {
            return @"insert into T_Recipes(Name,Description,DoesCount,PreparationTimeMin,Ingredients,Instructions,DoseTypeID,RecipeTypeID,CategoryID,RegularImageID,SmallImageID,OwnerID)values(@0, @1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11) SELECT SCOPE_IDENTITY()";
        }


        //public string GetRecipes(int userId, int a_categoryID, string a_recipeName, string a_discrption)
        //{
        //    StringBuilder sb = new StringBuilder(@"SELECT * FROM " + RecipesTable);
         
        //    sb.Append(" WHERE ID IN (SELECT RecipeID FROM T_Users_Recipes WHERE UserID=@0)");      
        //    sb.Append(" AND CategoryID=@1");         
        //    sb.Append(" AND Name LIKE '%@2%'");
        //    sb.Append(" AND Description LIKE '%@3%'"); 

        //    return sb.ToString();
        //}
        //public string GetRecipes(int userId, int a_categoryID, string a_recipeName)
        //{
        //    StringBuilder sb = new StringBuilder(@"SELECT * FROM " + RecipesTable);
        
        //    sb.Append(" WHERE ID IN (SELECT RecipeID FROM T_Users_Recipes WHERE UserID=@0)");
     
        //    sb.Append(" AND CategoryID=@1");
        
        //    sb.Append(" AND Name LIKE '%@2%'");


        //    return sb.ToString();
        //}
        //public string GetRecipes(int userId, int? a_categoryID)
        //{
        //    StringBuilder sb = new StringBuilder(@"SELECT * FROM " + RecipesTable);
   
        //    sb.Append(" WHERE ID IN (SELECT RecipeID FROM T_Users_Recipes WHERE UserID=@0)");
       
        //    sb.Append(" AND CategoryID=@1");

        //    return sb.ToString();
        //}
        //public string GetRecipes(int userId)
        //{
        //    StringBuilder sb = new StringBuilder(@"SELECT * FROM " + RecipesTable);
          
        //    sb.Append(" WHERE ID IN (SELECT RecipeID FROM T_Users_Recipes WHERE UserID=@0)");

        //    return sb.ToString();
        //}
        public string GetRecipes()
        {
            StringBuilder sb = new StringBuilder(@"SELECT * FROM " + RecipesTable);
            return sb.ToString();
        }

        public string InsertRecipeToUser()
        {
            return @"insert into T_Users_Recipes(UserID,RecipeID,Description)values(@0, @1, @2)";
        }

        public string GetImageByID()
        {
            return @"SELECT * FROM " + ImagesTable + " WHERE ID = @0";
        }


        public string GetRecipeById()
        {
            return @"SELECT * FROM " + RecipesTable + " WHERE ID = @0";
        }

        /// <summary>
        /// Edit with out imageID and owner.
        /// </summary>
        /// <returns></returns>
        public string EditRecipeDetails()
        {
            return @"Update " + RecipesTable + " SET Name=@0,Description=@1,DoesCount=@2 PreparationTimeMin=@3,Ingredients=@4,Instructions=@5,DoseTypeID=@6,RecipeTypeID=@7,CategoryID=@8 WHERE ID=@9";

        }

        public string EditImage()
        {
            return @"Update " + ImagesTable + " SET ImageName=@0,ImageData=@1,ContentType=@2 WHERE ID=@3";

        }


        public string EditRecipeImage()
        {
            return @"Update " + RecipesTable + " SET RegularImageID=@0,SmallImageID=@1 WHERE ID=@2";
        }


        public string RemoveImage()
        {
            return @"DELETE FROM " + ImagesTable + " WHERE ID=@0";
        }

        SqlCommand ISqlCommand.GetRecipes(int? userId, int? a_categoryID, string a_recipeName, string a_discription, int? MaxRecipesInOnePage, long? minIndex)
        {
            StringBuilder sb = new StringBuilder(@"SELECT ");
            SqlCommand cmd = new SqlCommand();

            if (MaxRecipesInOnePage.HasValue)
            {
                sb.Append(" TOP " + MaxRecipesInOnePage.Value);
            }

            sb.Append( " * FROM " + RecipesTable + " WHERE 1=1");

            if (minIndex.HasValue)
            {
                sb.Append(" AND ID  > @0");
                cmd.Parameters.AddWithValue("@0", minIndex.Value);
            }

            if (userId.HasValue && userId != -1)
            {
                sb.Append(" AND (ID IN (SELECT RecipeID FROM T_Users_Recipes WHERE UserID=@1))");
                cmd.Parameters.AddWithValue("@1", userId.Value);
            }

            if (a_categoryID.HasValue)
            {
                sb.Append(" AND CategoryID=@2");
                cmd.Parameters.AddWithValue("@2", a_categoryID.Value);
            }

            if (!String.IsNullOrEmpty(a_recipeName))
            {
                sb.Append(" AND (Name LIKE '%'+ @3 + '%')");
                cmd.Parameters.AddWithValue("@3", a_recipeName);
            }

            if (!String.IsNullOrEmpty(a_discription))
            {
                sb.Append(" AND Description LIKE '%'+ @4 + '%'");
                 cmd.Parameters.AddWithValue("@4", a_discription);

            }

            cmd.CommandText = sb.ToString();
            return cmd;
        }

        public SqlCommand RemoveImages(List<int> list)
        {
            SqlCommand cmd = new SqlCommand();
            StringBuilder sb = new StringBuilder(@"DELETE FROM " + ImagesTable + " WHERE ID=-1");
            
            for (int i = 0; i < list.Count; i++)
			{
                sb.Append(" OR ID=@" + i);
                cmd.Parameters.AddWithValue("@" + i, list[i]);
			}

            cmd.CommandText = sb.ToString();
            return cmd;
        }



        public SqlCommand InsertNewMsg(int a_sendUserID, int a_toUserID,string a_msg, long? a_recipeID, DateTime a_dateTime)
        {
            string strCmd = @"insert into " + MsgTable + " (FromUserID,ToUserID,Msg,MsgTime,RefernceRecipeID,IsReading) values (@0, @1,@2,@3,@4,@5) SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(strCmd);
            cmd.Parameters.AddWithValue("@0", a_sendUserID);
            cmd.Parameters.AddWithValue("@1", a_toUserID);
            cmd.Parameters.AddWithValue("@2", a_msg);
            cmd.Parameters.AddWithValue("@3", a_dateTime);
            cmd.Parameters.AddWithValue("@4", a_recipeID);
            cmd.Parameters.AddWithValue("@5", false);

            ConverNULLToDBNULL(cmd.Parameters);
            return cmd;
        }

        private void ConverNULLToDBNULL(SqlParameterCollection sqlParameterCollection)
        {
            foreach (IDataParameter param in sqlParameterCollection)
            {
                if (param.Value == null) param.Value = DBNull.Value;
            }
        }

        public SqlCommand GetMsgsGet(int a_user)
        {
            string strCmd = @"SELECT * FROM " + MsgTable + " WHERE ToUserID=@0" ;
            SqlCommand cmd = new SqlCommand(strCmd);
            cmd.Parameters.AddWithValue("@0", a_user);
            return cmd;
        }


        public SqlCommand GetMsgsSend(int a_user)
        {
            string strCmd = @"SELECT * FROM " + MsgTable + " WHERE FromUserID=@0";
            SqlCommand cmd = new SqlCommand(strCmd);
            cmd.Parameters.AddWithValue("@0", a_user);
            return cmd;
        }

    }
}
