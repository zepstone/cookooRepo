using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Origin.Gui;
using Origin.IO;
using CookooBL;

namespace Cookoo {

    public enum MainWinStateEnum { 
        Regular,
        EditRecipe,
    }

    public class RecipeVM : VMBase {
        public List<string> RecipeCategories { get; set; }

        public Recipe Model { get;private set; }

        public RecipeVM(Recipe a_recipe, List<string> a_RecipeCategories) {
            Model = a_recipe;
            RecipeCategories = a_RecipeCategories;
        }

 
        public string Name{
            get { return Model.Name; }
            set { Model.Name = value; OnPropertyChanged("Name"); }
        }


        public string Description {
            get { return Model.Description; }
            set { Model.Description = value; OnPropertyChanged("Description"); }
        }
   
        public TimeSpan PreparationTime {
            get { return Model.PreparationTime; }
            set { Model.PreparationTime = value; OnPropertyChanged("PreparationTime"); }
        }

        public string Ingredients {
            get { return Model.Ingredients; }
            set { Model.Ingredients = value; OnPropertyChanged("Ingredients"); }
        }
  
        public string Instructions {
            get { return Model.Instructions; }
            set { Model.Instructions = value; OnPropertyChanged("Instructions"); }
        }
   
        public string RecipeCategory {
            get { return Model.RecipeCategory; }
            set { Model.RecipeCategory = value; OnPropertyChanged("RecipeCategory"); }
        }

        public RecipeTypeEnum? RecipeType {
            get { return Model.RecipeType; }
            set { Model.RecipeType = value; OnPropertyChanged("RecipeType"); }
        }
    
        public DoseTypeEnum? DoseType {
            get { return Model.DoseType; }
            set { Model.DoseType = value; OnPropertyChanged("DoseType"); }
        }

    }

    public class MainWindowVM : VMBase {

        private MainWinStateEnum m_mainWinState;
        public MainWinStateEnum MainWinState {
            get { return m_mainWinState; }
            set { m_mainWinState = value; OnPropertyChanged("MainWinState"); }
        }

  
        public RecipesBook RecipesBook { get; set; }
       
        private RecipeVM m_recipeEdit;
        public RecipeVM RecipeEdit {
            get { return m_recipeEdit; }
            set { m_recipeEdit = value; OnPropertyChanged("RecipeEdit"); } 
        }

        private Recipe m_currentRecipe;
        public Recipe CurrentRecipe {
            get { return m_currentRecipe; }
            set { m_currentRecipe = value; OnPropertyChanged("CurrentRecipe"); }
        }

        public RealyCommand SaveNewNewRecipeCommand { get; set; }
        public RealyCommand CancelNewNewRecipeCommand { get; set; }
        public RealyCommand AddNewRecipeCommand { get; set; }
        public RealyCommand EditCurrentRecipeCommand { get; set; }
        public RealyCommand DeleteCurrentRecipeCommand { get; set; }

        public ObservableCollection<Recipe> DispalyRecipes { get; set; }

        public MainWindowVM() {
           
            if (!FolderAndFilesCreator.IfFolderAndFilesExsists()) {
                FolderAndFilesCreator.CreateFoldersAndFiles();
            }

            DispalyRecipes = new ObservableCollection<Recipe>();
            RecipesBook = new CookooBL.RecipesBook();
            RecipesBook.Load();
            MainWinState = MainWinStateEnum.Regular;

            AddNewRecipeCommand = new RealyCommand(()=>
            {
                RecipeEdit = new RecipeVM(new Recipe(), RecipesBook.RecipeCategories);
                MainWinState = MainWinStateEnum.EditRecipe;
            });

            SaveNewNewRecipeCommand = new RealyCommand(() => {

                if (!RecipesBook.Recipes.Contains(RecipeEdit.Model)) {
                    RecipesBook.Recipes.Add(RecipeEdit.Model);
                }
           
                RecipesBook.Save();
                UpDateDisplayRecipes();
                MainWinState = MainWinStateEnum.Regular;
            });

            CancelNewNewRecipeCommand = new RealyCommand(() => {
                RecipesBook.Load();    
                UpDateDisplayRecipes();
                MainWinState = MainWinStateEnum.Regular;
            });

            EditCurrentRecipeCommand = new RealyCommand(() => {
                MainWinState = MainWinStateEnum.EditRecipe;
                RecipeEdit = new RecipeVM(CurrentRecipe, RecipesBook.RecipeCategories);

            });

            DeleteCurrentRecipeCommand = new RealyCommand(() => {
                RecipesBook.Recipes.Remove(CurrentRecipe);
                RecipesBook.Save();
                UpDateDisplayRecipes();
                MainWinState = MainWinStateEnum.Regular;

            });

            UpDateDisplayRecipes();
        }

        private void UpDateDisplayRecipes(){
            DispalyRecipes.Clear();
            foreach (Recipe item in RecipesBook.Recipes) {
                DispalyRecipes.Add(item);
            }
        }
    }
}
