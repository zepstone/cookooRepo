using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Origin.IO;
using System.IO;
using CookooDAL.Implemented;

namespace CookooBL {

    [Serializable()] 
    public class RecipesBook {

        string RecipeCategoriePath = "RecipeCategorie.txt";
        private readonly string RecipesPath = "Recipes";

        public List<string> RecipeCategories { get; set; }

        private IOHelper<List<Recipe>> RecipeBookSerlizer;
        public List<Recipe> Recipes { get; set; }

        public RecipesBook() {

            RecipeBookSerlizer = new IOHelper<List<Recipe>>(IOTypeEnum.Binary);
        }

        private void LoadRecipeCategories() {

            IOHelper<List<string>> RecipeCategorieSerlizer;
            RecipeCategorieSerlizer = new IOHelper<List<string>>(IOTypeEnum.Text);
            RecipeCategories = RecipeCategorieSerlizer.Load(RecipeCategoriePath, FilesType.Configuration);

        }

        public void Load() {

            LoadRecipeCategories();
            Recipes = RecipeBookSerlizer.Load(RecipesPath, FilesType.Data);
        }

        public void Save() {
            RecipeBookSerlizer.Save(RecipesPath,FilesType.Data,Recipes);
        }
    }
}
