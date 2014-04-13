using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookooOrc.interfaces;

namespace CookooOrc.Implemented {
    public class heSrings : IConstStrings {

        public heSrings() {
            
            Categories = new Dictionary<CategoryEnum, string>();
            var l_ctgrs = Enum.GetValues(typeof(CategoryEnum));

            foreach (CategoryEnum ctgry in l_ctgrs) {
                switch (ctgry) {
                    case CategoryEnum.Soups:
                    Categories.Add(ctgry, "מרקים");
                    break;
                    case CategoryEnum.Breads:
                    Categories.Add(ctgry, "לחמים");
                    break;
                    case CategoryEnum.Meat:
                    Categories.Add(ctgry, "בשרים");
                    break;
                    case CategoryEnum.Cake:
                    Categories.Add(ctgry, "עוגות");
                    break;
                    case CategoryEnum.Fish:
                    Categories.Add(ctgry, "דגים");
                    break;
                    case CategoryEnum.Kids:
                    Categories.Add(ctgry, "ילדים");
                    break;
                    case CategoryEnum.Salad:
                    Categories.Add(ctgry, "סלטים");
                    break;
                    case CategoryEnum.Main_Course:
                    Categories.Add(ctgry, "עיקריות");
                    break;
                    case CategoryEnum.Dreesing:
                    Categories.Add(ctgry, "תוספות");
                    break;
                    case CategoryEnum.Dessert:
                    Categories.Add(ctgry, "קינוחים");
                    break;
                    case CategoryEnum.Appetizer:
                    Categories.Add(ctgry, "פתיחה");
                    break;
                    case CategoryEnum.Healthy:
                    Categories.Add(ctgry, "בריא");
                    break;
                    case CategoryEnum.Pastry:
                    Categories.Add(ctgry, "מאפים");
                    break;
                    case CategoryEnum.Cookie:
                    Categories.Add(ctgry, "עוגיות");
                    break;
                    case CategoryEnum.Dip:
                    Categories.Add(ctgry, "רטבים");
                    break;
                    case CategoryEnum.Pasta:
                    Categories.Add(ctgry, "פאסטה");
                    break;
                    case CategoryEnum.Dairy:
                    Categories.Add(ctgry, "חלבי");
                    break;
                    case CategoryEnum.Fast:
                    Categories.Add(ctgry, "מהיר");
                    break;
                    case CategoryEnum.Hollydays:
                    Categories.Add(ctgry, "חגים");
                    break;
                    case CategoryEnum.Pickles:
                    Categories.Add(ctgry, "חמוצים");
                    break;
                    case CategoryEnum.Vegetarian:
                    Categories.Add(ctgry, "צמחוני");
                    break;
                    case CategoryEnum.Chicken:
                    Categories.Add(ctgry, "עוף");
                    break;
                    default:
                    Categories.Add(ctgry, ctgry.ToString());
                    break;
                }
            }
        }

        public string Search { get { return "חפש"; } }

        public IDictionary<CategoryEnum, string> Categories { get; private set; }

        public string Add {
            get { return "הוסף"; }
        }

        public string Edit {
            get { return "ערוך"; }
        }

        public string Save {
            get { return "שמור שינויים"; }
        }

        public string SearchInMyBoob {
            get { return "חפש בספר המתכונים שלי"; }
        }

        public string NewRecipeHeader {
            get { return "הוסף מתכון"; }
        }

        public string CKRcpHeader {
            get { return "Cookoo"; }
        }

        public string ImageRcpHeader {
            get { return "סריקה"; }
        }

        public string UrlRcpHeader {
            get { return " לינק "; }
        }

        public string AdvSearchHeader {
            get { return "חיפוש מתקדם"; }
        }

        public string MessagesHeader {
            get { return "הודעות"; }
        }

        public string NewCookooRecipeTitle {
            get { return "מתכון Cookoo חדש"; }
        }

        public string NewImagesRecipeTitle {
            get { return "הוספת מתכון חדש"; }
        }

        public string NewUriRecipeTitle {
            get { return "הוספת מתכון חדש"; }
        }


        public string SearchResultTitle {
            get { return "תוצאות חיפוש"; }
        }

        public string NewRcpRecipeName {
            get { return "*שם המתכון"; }
        }

        public string NewRcpCategories {
            get { return "*בחר קטגוריות"; }
        }

        public string NewRcpDescription {
            get { return "הערות"; }
        }

        public string NewCKIngredients {
            get { return "*מצרכים"; }
        }

        public string NewCKInstructions {
            get { return "*הוראות הכנה"; }
        }

        public string NewCKPreparationTime {
            get { return "זמן הכנה"; }
        }

        public string NewCKHour {
            get { return "שעות.."; }
        }

        public string NewCKMinute {
            get { return "דקות.."; }
        }

        public string NewCKDoesCount {
            get { return "מספר מנות"; }
        }

        public string NewCKAddPic {
            get { return "הוסף תמונה"; }
        }


        public string EditCKAddPic {
            get { return "החלף תמונה"; }
        }

        public string NewImgAddPics {
            get { return "*הוסף סריקות"; }
        }

        public string NewUrlLink {
            get { return "*קישור למתכון"; }
        }


        public string GetCKRcpIngredients {
            get { return "רכיבים"; }
        }

        public string GetCKRcpInstructions {
            get { return "הוראות הכנה"; }
        }

        public string GetCKRcpCategries {
            get { return "קטגוריות"; }
        }


        public string GetRecipeOwner {
            get { return "נוצר ע'י "; }
        }


        public string EditCookooRecipeTitle {
            get { return "ערוך Cookoo"; }
        }

        public string RemoveRecipeUser {
            get { return "מחק מהספר שלי "; }
        }

        public string AddRecipeUser {
            get {return "הוסף לספר שלי"; }
        }


        public string EditImagesRecipeTitle {
            get { return "עריכת מתכון"; }
        }

        public string EditImgAddPic {
            get { return "החלפת תמונות"; }
        }


        public string AdvencedStringTitle {
            get { return "חיפוש מתקדם"; }
        }

        public string SearchByOwnerName {
            get { return "חפש/י לפי יוצר המתכון"; }
        }

        public string SerchByCategories {
            get { return "חפש/י לפי קטגוריות"; }
        }

        public string HelloTitle {
            get { return "היי "; }
        }

        public string HomePageDis1 {
            get { return "השימוש בCookoo הוא חינמי."; }
        }

        public string HomePageDis2 {
            get { return "Cookoo מציע לך איחסון וארגון של המתכונים שלך."; }
        }

        public string HomePageDis3 {
            get { return "תוכל לאחסן שלושה סוגי מתכונים :"; }
        }

        public string HomePageDis4 {
            get { return "תוכלו לשתף חברים במתכונים שלכם."; }
        }

        public string HomePageDis5 {
            get { return "בתאבון!"; }
        }

        public string ConfirmNameSuccessfullyDis {
            get { return "זוהת בהצלחה עם "; }
        }

        public string EnterNameDis {
            get { return "כדי לסיים את ההרשמה לחצ/י בבקשה על כפתור הרישום."; }
        }


        public string LogInQstDis {
            get { return "האם יש לך חשבון באחד מהאתרים הבאים?"; }
        }

        public string LogInAnsDis {
            get { return "לחצ/י על אחד הסמלים בכדי להיכנס."; }
        }


        public string RegisterHeader {
            get { return "רישום"; }
        }

        public string UserNameReg {
            get { return "בחר את שם המשתמש שלך לאתר"; }
        }

        public string UserNameLan {
            get { return "בחר שפה רצויה"; }
        }


        public string NewUrlRecipeTitle {
            get { return "הוסף קישור"; }
        }

        public string EditUrlRecipeTitle {
            get { return "שנה קישור"; }
        }

        public string EditUrlLink {
            get { return "שנה קישור"; }
        }

        public string GetUrlRcpLink {
            get { return "קישור :"; }
        }


        public string NomiCredit {
            get { return "עוצב על ידי "; }
        }


        public string SendRecpToUsersHeader {
            get { return "שתף עם חבריך את המתכון "; }
        }


        public string SendRecpMessage {
            get { return "ההודעה שלך..."; }
        }

        public string sendRecpUsers {
            get { return "בחר חברים..."; }
        }


        public string Share {
            get { return "שתף"; }
        }


        public string ShareMgsTitle {
            get { return " רוצה לשתף איתך את "; }
        }


        public string NewMessage {
            get { return "חדש"; }
        }


        public string FriendsButton {
            get { return"חברים >>"; }
        }

        public string SearchButton {
            get { return"<< חפש"; }
        }


        public string BackButton {
            get { return "חזור >>"; }
        }


        public string ContactUsHeader {
            get { return "צור קשר"; }
        }


        public string Send {
            get { return "שלח"; }
        }

        public string MsgContent {
            get { return "גוף ההודעה"; }
        }

        public string MsgTiltle {
            get { return "*כותרת"; }
        }


        public string ContactUsDis1 {
            get { return "צריך עזרה?? נתקלת בבעיה??"; }
        }

        public string ContactUsDis2 {
            get { return "או שסתם בא לך לשתף אותנו ..."; }
        }

        public string ContactUsDis3 {
            get { return "כתוב לנו בבקשה."; }
        }
    }
}
