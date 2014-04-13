using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookooDAL.interfaces;
using CookooOrc;
using System.Data.SqlClient;
using System.Data;

namespace CookooDAL.Implemented {
    public class CookooDBAcess : DBAcsess {

        override public ISqlCommand SqlQueries { get; set; }
        override protected SqlConnection Connection { get; set; } 

        public CookooDBAcess(ISqlCommand a_ISqlQueries)
            : base(a_ISqlQueries) { }


        override protected string ConnectionString {
            get { return "Data Source=cookoodb.clx4dbdnrjxc.us-west-1.rds.amazonaws.com;Initial Catalog=CookooDB;Persist Security Info=True;User ID=Zeps;Password=ZepsAlon;MultipleActiveResultSets=true"; }
        }

        override public IList<ICkRecipeDBData> GetAllIRecipes() {
            string request = SqlQueries.GetAllIRecipes();

            throw new NotImplementedException();
        }

        public override IList<ICategory> GetAllCategories()
        {
            string request = SqlQueries.GetAllICategories();
            SqlCommand cmd = new SqlCommand(request, Connection);
            SqlDataReader myReader = null;
            myReader = cmd.ExecuteReader();

            List<ICategory> rtnCategories = new List<ICategory>();

            while (myReader.Read()) {
                 rtnCategories.Add(new Category(){ID = myReader.GetInt32(0), Name= myReader.GetString(1)} );
            }
            myReader.Close();
           return rtnCategories;
        }

        public override IList<IDoesType> GetAllDoesTypes()
        {
            string request = SqlQueries.GetAllIDoesTypes();
            SqlCommand cmd = new SqlCommand(request, Connection);
            SqlDataReader myReader = null;
            myReader = cmd.ExecuteReader();

            List<IDoesType> rtnDoesTypes = new List<IDoesType>();

            while (myReader.Read())
            {
                rtnDoesTypes.Add(new DoesType() { ID = myReader.GetInt32(0), Name = myReader.GetString(1)} );
            }
            myReader.Close();
            return rtnDoesTypes;
        }

        public override IList<IRecipeType> GetAllRecipeTypes()
        {
            string request = SqlQueries.GetAllRecipeTypes();
            SqlCommand cmd = new SqlCommand(request, Connection);
            SqlDataReader myReader = null;
            myReader = cmd.ExecuteReader();

            List<IRecipeType> rtnRecipeTypes = new List<IRecipeType>();

            while (myReader.Read())
            {
                rtnRecipeTypes.Add(new RecipeType() { ID = myReader.GetInt32(0), Name = myReader.GetString(1)} );
            }
            myReader.Close();
            return rtnRecipeTypes;
        }

        protected override int InsertNewImage(ICookooImage a_image)
        {
            
            string request = SqlQueries.InsertNewImage();
            SqlCommand cmd = new SqlCommand(request, Connection);
            cmd.Parameters.AddWithValue("@0", a_image.FileName);
            cmd.Parameters.AddWithValue("@1", a_image.ImageData);
            cmd.Parameters.AddWithValue("@2", a_image.ContentType);

            var id =cmd.ExecuteScalar();
            return Convert.ToInt32(id);
        }

        private void InsertRecipeToUser(int a_userID,int RecipeID ,string a_description){

            string request = SqlQueries.InsertRecipeToUser();
            SqlCommand cmd = new SqlCommand(request, Connection);
            cmd.Parameters.AddWithValue("@0", a_userID);
            cmd.Parameters.AddWithValue("@1", RecipeID);
            cmd.Parameters.AddWithValue("@2", a_description);
            var id = cmd.ExecuteNonQuery();
        }

        public override int InsertNewRecipe(ICkRecipeUserData a_recipe)
        {
            int? reImageID = null; 
            if (a_recipe.RegularImage != null)
            {
                reImageID = InsertNewImage(a_recipe.RegularImage);
            }

            int? smlImageID = null;
            if (a_recipe.SmallImage != null)
            {
                smlImageID = InsertNewImage(a_recipe.SmallImage);
            }


            string request = SqlQueries.InsertNewRecipe();
            SqlCommand cmd = new SqlCommand(request, Connection);
            cmd.Parameters.AddWithValue("@0", a_recipe.RecipeName);
            cmd.Parameters.AddWithValue("@1", a_recipe.Description);
            cmd.Parameters.AddWithValue("@2", a_recipe.DoesCount);
            // Checking!!!
            cmd.Parameters.AddWithValue("@3", a_recipe.PreparationTime.Value.TotalMinutes);
            cmd.Parameters.AddWithValue("@4", a_recipe.Ingredients);
            cmd.Parameters.AddWithValue("@5", a_recipe.Instructions);
            cmd.Parameters.AddWithValue("@6", a_recipe.DoseType);
            cmd.Parameters.AddWithValue("@7", a_recipe.RecipeType);
            cmd.Parameters.AddWithValue("@8", a_recipe.Category);
            cmd.Parameters.AddWithValue("@9", reImageID);
            cmd.Parameters.AddWithValue("@10", smlImageID);
            cmd.Parameters.AddWithValue("@11", a_recipe.OwnerId);

            foreach (IDataParameter param in cmd.Parameters)
            {
                if (param.Value == null) param.Value = DBNull.Value;
            }

            int id = Convert.ToInt32(cmd.ExecuteScalar());
            InsertRecipeToUser(a_recipe.OwnerId, id, a_recipe.PrivateDescription);
            return id;
        }

        override public ICookooImage GetImageByID(int p)
        {
            lock (this)
            {
                string request = SqlQueries.GetImageByID();
                SqlCommand cmd = new SqlCommand(request, Connection);
                cmd.Parameters.AddWithValue("@0", p);
                SqlDataReader myReader = null;
                myReader = cmd.ExecuteReader();
                CookooImage rtnImg = null;
                while (myReader.Read())
                {
                    byte[] data = (byte[])myReader[2];
                    rtnImg = new CookooImage(myReader.GetString(1), myReader.GetString(3), data);
                }
                myReader.Close();
                return rtnImg; 
            }
        }

        public override IList<ICkRecipeDBData> GetRecipes()
        {
            string request = SqlQueries.GetRecipes();
            SqlCommand cmd = new SqlCommand(request, Connection);

            return GetRecipesCommand(cmd);

        }

        public override IList<ICkRecipeDBData> GetRecipes(int userId, int? a_categoryID, string a_recipeName,
                                                        string a_siscrptionName, int? MaxRecipesInOnePage, long? minIndex)
        {
            SqlCommand cmd = SqlQueries.GetRecipes(userId, a_categoryID, a_recipeName, a_siscrptionName, MaxRecipesInOnePage, minIndex);
            cmd.Connection = Connection;
            return GetRecipesCommand(cmd);
        }

        private IList<ICkRecipeDBData> GetRecipesCommand(SqlCommand cmd)
        {
            SqlDataReader myReader = null;
            myReader = cmd.ExecuteReader();

            List<ICkRecipeDBData> rtnRecipeTypes = new List<ICkRecipeDBData>();

            while (myReader.Read())
            {
                long ID = myReader.GetInt64(0);
                string name = myReader.GetString(1);
                var desDB = myReader[2];
                string des = null;
                if (!(desDB is DBNull))
                {
                    des = Convert.ToString(desDB);
                }

                int l_doesCount = myReader.GetInt32(3);

                TimeSpan time = TimeSpan.FromMinutes(myReader.GetInt32(4));
                string ing = myReader.GetString(5);
                string inst = myReader.GetString(6);
                var doesDBType = myReader[7];
                int? doesType = null;

                if (!(doesDBType is DBNull))
                {
                    doesType = Convert.ToInt32(doesDBType);
                }

                int recipeType = myReader.GetInt32(8);
                int categoryType = myReader.GetInt32(9);

                var regImageId = myReader[10];
                int? regimgID = null;
                if (!(regImageId is DBNull))
                {

                    regimgID = Convert.ToInt32(regImageId);
                }

                var smlImageId = myReader[11];
                int? smlImgID = null;
                if (!(smlImageId is DBNull))
                {

                    smlImgID = Convert.ToInt32(smlImageId);
                }

                int ownerID = myReader.GetInt32(12);

                DBRecipe newRecipe = new DBRecipe(ID, name, des,l_doesCount, time, ing, inst, doesType, recipeType, categoryType, regimgID, smlImgID, ownerID, "");
                rtnRecipeTypes.Add(newRecipe);
            }
            myReader.Close();
            return rtnRecipeTypes;
        }

        public override ICkRecipeDBData GetRecicpe(long recipeId)
        {
            string request = SqlQueries.GetRecipeById();
            SqlCommand cmd = new SqlCommand(request, Connection);
            cmd.Parameters.AddWithValue("@0", recipeId);
            return GetRecipesCommand(cmd)[0];
        }

        public override void EditRecipeDetails(long a_recipeID, ICkRecipBaseData a_recipe)
        {

            string request = SqlQueries.EditRecipeDetails();
            SqlCommand cmd = new SqlCommand(request, Connection);
            cmd.Parameters.AddWithValue("@0", a_recipe.RecipeName);
            cmd.Parameters.AddWithValue("@1", a_recipe.Description);
            cmd.Parameters.AddWithValue("@2", a_recipe.PreparationTime.Value.TotalMinutes);
            cmd.Parameters.AddWithValue("@3", a_recipe.Ingredients);
            cmd.Parameters.AddWithValue("@4", a_recipe.Instructions);
            cmd.Parameters.AddWithValue("@5", a_recipe.DoseType);
            cmd.Parameters.AddWithValue("@6", a_recipe.RecipeType);
            cmd.Parameters.AddWithValue("@7", a_recipe.Category);
            cmd.Parameters.AddWithValue("@8", a_recipeID);

            foreach (IDataParameter param in cmd.Parameters)
            {
                if (param.Value == null) param.Value = DBNull.Value;
            }

            cmd.ExecuteNonQuery();
        }

        public override void EditImage(long a_imageID, ICookooImage a_image)
        {
            string request = SqlQueries.EditImage();
            SqlCommand cmd = new SqlCommand(request, Connection);
            cmd.Parameters.AddWithValue("@0", a_image.FileName);
            cmd.Parameters.AddWithValue("@1", a_image.ImageData);
            cmd.Parameters.AddWithValue("@2", a_image.ContentType);
            cmd.Parameters.AddWithValue("@3", a_imageID);
     
            foreach (IDataParameter param in cmd.Parameters)
            {
                if (param.Value == null) param.Value = DBNull.Value;
            }

            cmd.ExecuteNonQuery();
        }

        private void RemvoeRecipeImages(ICkRecipeDBData a_recipe)
        {
            string editRequest = SqlQueries.EditRecipeImage();
            SqlCommand cmd = new SqlCommand(editRequest, Connection);
            cmd.Parameters.AddWithValue("@0", DBNull.Value);
            cmd.Parameters.AddWithValue("@1", DBNull.Value);
            cmd.Parameters.AddWithValue("@2", a_recipe.RecipeID);
            cmd.ExecuteNonQuery();

            List<int> ids = new List<int>();

            if (a_recipe.RegularImageID.HasValue)
            {
                ids.Add(a_recipe.RegularImageID.Value);
            }
            if (a_recipe.SmallImageID.HasValue)
            {
                ids.Add(a_recipe.SmallImageID.Value);
            }

            cmd = SqlQueries.RemoveImages(ids);
            cmd.Connection = Connection;
            cmd.ExecuteNonQuery();
        }


        private void AddImageToExssistRecpice(int a_recipeID, ICookooImage a_iregIage, ICookooImage a_smlImage)
        {
            string request = SqlQueries.EditRecipeImage();
            int newRegImageID = InsertNewImage(a_smlImage);
            int newSmlImg = InsertNewImage(a_iregIage);
            SqlCommand cmd = new SqlCommand(request, Connection);
            cmd.Parameters.AddWithValue("@0", newRegImageID);
            cmd.Parameters.AddWithValue("@1", newSmlImg);
            cmd.Parameters.AddWithValue("@2", a_recipeID);

            cmd.ExecuteNonQuery();
        }

        public override void EditRecipeImages(long a_recipeID, ICookooImage regImage, ICookooImage smllImage)
        {
            var recipe = GetRecicpe((int)a_recipeID);

            if ((regImage == null || smllImage == null) && recipe.RegularImageID.HasValue)
            {
                RemvoeRecipeImages(recipe);
            }
            else if ((!recipe.RegularImageID.HasValue || !recipe.SmallImageID.HasValue) &&
                      (regImage != null && smllImage != null))
            {
                AddImageToExssistRecpice((int)a_recipeID, regImage, smllImage);
            }
            else if ((recipe.RegularImageID.HasValue && recipe.SmallImageID.HasValue) &&
                     (regImage != null && smllImage != null))
            {
                EditImage((int)recipe.RegularImageID.Value, regImage);
                EditImage((int)recipe.SmallImageID.Value, smllImage);

            }
        }


        public override long AddMsg(int a_sendUserID, int a_toUserID,string a_msg, long? a_recipeID, DateTime a_dateTime)
        {
            SqlCommand cmd = SqlQueries.InsertNewMsg(a_sendUserID, a_toUserID,a_msg, a_recipeID, a_dateTime);
            cmd.Connection = Connection;
            var id = cmd.ExecuteScalar();

            return Convert.ToInt64(id);
        }

        public override IList<IDBMsg> GetMsgGet(int a_userID)
        {
            SqlCommand cmd = SqlQueries.GetMsgsGet(a_userID);
            cmd.Connection = Connection;

            return GetMsgsCommand(cmd);
        }

        public override IList<IDBMsg> GetMsgSend(int a_userID)
        {
            SqlCommand cmd = SqlQueries.GetMsgsSend(a_userID);
            cmd.Connection = Connection;

            return GetMsgsCommand(cmd);
        }

        private IList<IDBMsg> GetMsgsCommand(SqlCommand cmd) {
            List<IDBMsg> rtnMNsgs = new List<IDBMsg>();

            SqlDataReader myReader = null;
            myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                long ID = myReader.GetInt64(0);
                int sendUserID = myReader.GetInt32(1);
                int toUserID = myReader.GetInt32(2);
                string msg = myReader.GetString(3);
                DateTime time = myReader.GetDateTime(4);
                var RecipeIDDB = myReader[5];
                long? recipeID = null;
                if (!(RecipeIDDB is DBNull))
                {
                    recipeID = Convert.ToInt64(RecipeIDDB);
                }

                bool isReding = myReader.GetBoolean(6);

                CookooDBMsg newRecipe = new CookooDBMsg(ID, sendUserID, toUserID, msg, time, recipeID, isReding);
                rtnMNsgs.Add(newRecipe);
            }
            myReader.Close();

            return rtnMNsgs;
        }
 
    }


}
