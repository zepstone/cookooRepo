using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookooOrc.interfaces;

namespace CookooOrc.Implemented {
    public class enString : IConstStrings {

        public enString() {        

            Categories = new Dictionary<CategoryEnum,string>();
            var l_ctgrs = Enum.GetValues(typeof(CategoryEnum));

            foreach (CategoryEnum ctgry in l_ctgrs) {
                switch (ctgry) {
                    case CategoryEnum.Soups:
                    Categories.Add(ctgry, "Soups");
                    break;
                    case CategoryEnum.Breads:
                    Categories.Add(ctgry, "Breads");
                    break;
                    case CategoryEnum.Meat:
                    Categories.Add(ctgry, "Meat");
                    break;
                    case CategoryEnum.Cake:
                    Categories.Add(ctgry, "Cake");
                    break;
                    case CategoryEnum.Fish:
                    Categories.Add(ctgry, "Fishes");
                    break;
                    case CategoryEnum.Kids:
                    Categories.Add(ctgry, "Kids");
                    break;
                    case CategoryEnum.Salad:
                    Categories.Add(ctgry, "Salad & Dip");
                    break;
                    case CategoryEnum.Main_Course:
                    Categories.Add(ctgry, "Main");
                    break;
                    case CategoryEnum.Dreesing:
                    Categories.Add(ctgry, "Dreesing");
                    break;
                    case CategoryEnum.Dessert:
                    Categories.Add(ctgry, "Dessert");
                    break;
                    case CategoryEnum.Appetizer:
                    Categories.Add(ctgry, "Appetizer");
                    break;
                    case CategoryEnum.Healthy:
                    Categories.Add(ctgry, "Healthy");
                    break;
                    case CategoryEnum.Pastry:
                    Categories.Add(ctgry, "Pastry");
                    break;
                    case CategoryEnum.Cookie:
                    Categories.Add(ctgry, "Cookies");
                    break;
                    case CategoryEnum.Dip:
                    Categories.Add(ctgry, "Dip");
                    break;
                    case CategoryEnum.Pasta:
                    Categories.Add(ctgry, "Pasta");
                    break;
                    case CategoryEnum.Dairy:
                    Categories.Add(ctgry, "Dairy");
                    break;
                    case CategoryEnum.Fast:
                    Categories.Add(ctgry, "Fast");
                    break;
                    case CategoryEnum.Hollydays:
                    Categories.Add(ctgry, "Hollyday");
                    break;
                    case CategoryEnum.Pickles:
                    Categories.Add(ctgry, "Pickles");
                    break;
                    case CategoryEnum.Vegetarian:
                    Categories.Add(ctgry, "Vegetarian");
                    break;
                    case CategoryEnum.Chicken:
                    Categories.Add(ctgry, "Chicken");
                    break;
                    default:
                    Categories.Add(ctgry, ctgry.ToString());
                    break;
                }
            }
        }
 
        public string Search { get { return "Search"; } }
        public IDictionary<CategoryEnum,string> Categories { get; private set; }
        public string Add {
            get { return "Add"; }
        }

        public string Edit {
            get { return "Edit"; }
        }

        public string Save {
            get { return "Save Changes"; }
        }

        public string SearchInMyBoob {
            get { return "Search In My Book"; }      
        }

        public string NewRecipeHeader {
            get { return "Add Recipe"; }
        }

        public string CKRcpHeader {
            get { return "Cookoo"; }
        }

        public string ImageRcpHeader {
            get { return "Scan"; }
        }

        public string UrlRcpHeader {
            get { return " Url "; }
        }

        public string AdvSearchHeader {
            get { return "Advanced Search"; }
        }

        public string MessagesHeader {
            get { return "Messages"; }
        }

        public string NewCookooRecipeTitle {
            get { return "New Cookoo Recipe"; }
        }

        public string NewImagesRecipeTitle {
            get { return "New Images Recipe"; }
        }

        public string NewUriRecipeTitle {
            get { return "New Link REcipe"; }
        }

        public string SearchResultTitle {
            get { return "Search Result"; }
        }

        public string NewRcpRecipeName {
            get { return "*Recipe Name"; }
        }

        public string NewRcpCategories {
            get { return "*Choose Categories..."; }
        }

        public string NewRcpDescription {
            get { return "Description"; }
        }

        public string NewCKIngredients {
            get { return "*Ingredients"; }
        }

        public string NewCKInstructions {
            get { return "*Instructions"; }
        }

        public string NewCKPreparationTime {
            get { return "Preparation Time"; }
        }

        public string NewCKHour {
            get { return "Hours.."; }
        }

        public string NewCKMinute {
            get { return "Minutes.."; }
        }

        public string NewCKDoesCount {
            get { return "Services"; }
        }

        public string NewCKAddPic {
            get { return "Add picture"; }
        }

        public string EditCKAddPic {
            get { return "Replace picture"; }
        }

        public string NewImgAddPics {
            get { return "*Add Images"; }
        }

        public string NewUrlLink {
            get { return "*Link"; }
        }

        public string GetCKRcpIngredients {
            get { return "Ingredients"; }
        }

        public string GetCKRcpInstructions {
            get { return "Instructions"; }
        }

        public string GetCKRcpCategries {
            get { return "Categries"; }
        }


        public string GetRecipeOwner {
            get { return "Create by "; }
        }

        public string EditCookooRecipeTitle {
            get { return "Edit Cookoo"; }
        }


        public string RemoveRecipeUser {
            get { return "Delete from my book"; }
        }

        public string AddRecipeUser {
            get { return "Add to my book"; }
        }


        public string EditImagesRecipeTitle {
            get { return "Edit Recipe"; }
        }

        public string EditImgAddPic {
            get { return "Replace Images"; }
        }


        public string AdvencedStringTitle {
            get { return "Advanced String"; }
        }


        public string SearchByOwnerName {
            get { return "Search recipe created by"; }
        }

        public string SerchByCategories {
            get { return "Search By Categries"; }
        }


        public string HelloTitle {
            get { return "Hi..."; }
        }

        public string HomePageDis1 {
            get { return "The uses with cookoo it's free."; }
        }

        public string HomePageDis2 {
            get { return "Cookoo offers you,to store & orginze your recipes."; }
        }

        public string HomePageDis3 {
            get { return "You can store 3 kinds of recipes :"; }
        }

        public string HomePageDis4 {
            get { return "you can share your recipes with friends."; }
        }

        public string HomePageDis5 {
            get { return "bon appetit!"; }
        }


        public string ConfirmNameSuccessfullyDis {
            get { return "You've successfully authenticated with"; }
        }

        public string EnterNameDis {
            get { return "Please enter a user name for this site below and click the Register button to finish logging in."; }
        }


        public string RegisterHeader {
            get { return "Register"; }
        }

        public string LogInQstDis {
            get { return "Do you already have an account on one of these sites?"; }
        }

        public string LogInAnsDis {
            get { return "Click the logo to log in with it here:"; }
        }


        public string UserNameReg {
            get { return "Enter your username for this site"; }
        }

        public string UserNameLan {
            get { return "Choose language"; }
        }


        public string NewUrlRecipeTitle {
            get { return "New Link"; }
        }

        public string EditUrlRecipeTitle {
            get { return "Edit Link"; }
        }

        public string EditUrlLink {
            get { return "Edit link"; }
        }

        public string GetUrlRcpLink {
            get { return "Link"; }
        }


        public string NomiCredit {
            get { return " Design by "; }
        }


        public string SendRecpToUsersHeader {
            get { return "Share with your firends the recipe "; }
        }


        public string SendRecpMessage {
            get { return "Insert your message"; }
        }

        public string sendRecpUsers {
            get { return "Choose freinds"; }
        }


        public string Share {
            get { return "Share"; }
        }


        public string ShareMgsTitle {
            get { return " want to share with you "; }
        }


        public string NewMessage {
            get {return "new"; }
        }


        public string FriendsButton {
            get { return "<< Frindes"; }
        }


        public string SearchButton {
            get { return "Search >>"; }
        }


        public string BackButton {
            get { return"<< Back"; }
        }


        public string ContactUsHeader {
            get { return "Contact Us"; }
        }


        public string Send {
            get { return "Send"; }
        }

        public string MsgContent {
            get { return "Content"; }
        }

        public string MsgTiltle {
            get { return "*Title"; }
        }


        public string ContactUsDis1 {
            get { return "You need some help??"; }
        }

        public string ContactUsDis2 {
            get { return "Or you just want to tell us something??"; }
        }

        public string ContactUsDis3 {
            get { return "please write us."; }
        }
    }
}
