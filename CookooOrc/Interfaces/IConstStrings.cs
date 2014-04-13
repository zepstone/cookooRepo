using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CookooOrc.interfaces {

    public interface IConstStrings {

         string Add { get; }
         string Edit { get;  }
         string Save { get; }
         string Send { get; }

         IDictionary<CategoryEnum, string> Categories { get; }
         string Search { get; }
         string SearchInMyBoob { get; }
         string NomiCredit { get; }
         string ShareMgsTitle { get; }
         string NewMessage { get; }
         string FriendsButton { get; }
         string SearchButton { get; }
         string BackButton { get; }

         #region Headers
         // Header
         string NewRecipeHeader { get;}
         string CKRcpHeader { get; }
         string ImageRcpHeader { get; }
         string UrlRcpHeader { get; }
         string AdvSearchHeader { get;}
         string MessagesHeader { get; }
         string RegisterHeader { get; }
         string ContactUsHeader { get; }
         #endregion

         #region Titles
         string SearchResultTitle { get; }
         string NewCookooRecipeTitle { get; }
         string NewImagesRecipeTitle { get; }
         string EditImagesRecipeTitle { get; }

         string NewUrlRecipeTitle { get; }
         string EditUrlRecipeTitle { get; }

         string NewUriRecipeTitle { get;  }
         string EditCookooRecipeTitle { get; }
         string AdvencedStringTitle { get; }
         string HelloTitle { get; }
         #endregion

         #region PageDiscription
         string HomePageDis1 { get; }
         string HomePageDis2 { get; }
         string HomePageDis3 { get; }
         string HomePageDis4 { get; }
         string HomePageDis5 { get; }

         string LogInQstDis { get; }
         string LogInAnsDis { get; }

         string ConfirmNameSuccessfullyDis { get; }
         string EnterNameDis { get; }

         #endregion

         #region Save recipe
         // new recipes
        string NewRcpRecipeName { get;  }
        string NewRcpCategories { get;  }
        string NewRcpDescription { get;  }

        // new cookoo
        string NewCKIngredients { get; }
        string NewCKInstructions { get;  }
        string NewCKPreparationTime { get;  }
        string NewCKHour { get;  }
        string NewCKMinute { get; }
        string NewCKDoesCount { get;  }

        string NewCKAddPic { get;  }
        string EditCKAddPic { get; }

       

        //new image
        string NewImgAddPics { get; }
        string EditImgAddPic { get; }

        // new URL
        string NewUrlLink { get; }
        string EditUrlLink { get; }


        #endregion 

         #region Get Recipe

        string GetCKRcpIngredients { get; }
        string GetCKRcpInstructions { get; }
        string GetCKRcpCategries { get; }

        string GetUrlRcpLink { get; }

        string GetRecipeOwner { get; }
        string RemoveRecipeUser { get; }
        string AddRecipeUser { get; }

        string SendRecpToUsersHeader { get; }
        string SendRecpMessage { get; }
        string sendRecpUsers { get; }
        string Share { get; }
        #endregion

         #region Search

        string SearchByOwnerName { get;}
        string SerchByCategories { get; }

        #endregion

         #region Reg

        string UserNameReg{ get; }
        string UserNameLan { get; }


        #endregion

        #region ContacktUs

        string ContactUsDis1 { get; }
        string ContactUsDis2 { get; }
        string ContactUsDis3 { get; }


        string MsgContent { get; }

        string MsgTiltle { get; }

        #endregion




      
    }
}
