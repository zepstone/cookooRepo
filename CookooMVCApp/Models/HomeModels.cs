using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CookooMVCApp.ValidationRules;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using CookooOrc.interfaces;
using System.Web.Mvc;
using CookooOrc;

namespace CookooMVCApp.Models
{

    public class BaseRcpModel 
    {
        protected CkRcpDetails m_rcpDetailes;

        public BaseRcpModel(CkRcpDetails a_rcpDetailes) {
            m_rcpDetailes = a_rcpDetailes;
        }

        public long? ID {
            get { return m_rcpDetailes.RtnRecipeBaseID; }
            set { m_rcpDetailes.RtnRecipeBaseID = value; }
        }

        [Required]
        [MaxWordLetters(20)]
        [StringLength(50)]
        public string RecipeName {
            get { return m_rcpDetailes.RecipeName; }
            set { m_rcpDetailes.RecipeName = value; }
        }
        /// <summary>
        /// This list Diplayed in the view.
        /// </summary>

        [MinIntArrayAttribute(1)]
        public int[] SelectedCtgrs { get; set; }

        [MaxWordLetters(20)]
        [DataType(DataType.MultilineText)]
        public string Description {
            get { return m_rcpDetailes.Description; }
            set { m_rcpDetailes.Description = value; }
        }
    }

    public class SndCKRcpModel : BaseRcpModel
    {
        public CkSndRcp CkRcpSnd {get;private set;}
        public SndCKRcpModel():this(new CkSndRcp()){}

        public SndCKRcpModel(CkSndRcp a_CkRcpSnd):base(a_CkRcpSnd)
            
        {
            CkRcpSnd = a_CkRcpSnd;
            Ingredients = CkRcpSnd.Ingredients;
            Instructions = CkRcpSnd.Instructions;
            DoesCount = CkRcpSnd.DoesCount;
        }
      
        public int? PreparationHourTime { get; set; }
        public int? PreparationMinuteTime { get; set; }



        [Required]
        [MaxWordLetters(20)]
        [DataType(DataType.MultilineText)]
        public string Ingredients {
            get { return CkRcpSnd.Ingredients; }
            set { CkRcpSnd.Ingredients = value; }
        }

        [Required]
        [MaxWordLetters(20)]
        [DataType(DataType.MultilineText)]
        public string Instructions {
            get { return CkRcpSnd.Instructions; }
            set { CkRcpSnd.Instructions = value; }
        }

        public int? DoesCount {
            get { return CkRcpSnd.DoesCount; }
            set { CkRcpSnd.DoesCount = value; }
        }

        public FileModel FileModel { get; set; }
    }

    public class SndImgsRcpModel : BaseRcpModel {

        public ImgsSndRcp ImgRcpSnd { get; private set; }

        public SndImgsRcpModel(): this(new ImgsSndRcp()) {}

        public SndImgsRcpModel(ImgsSndRcp a_CkRcpSnd)
            : base(a_CkRcpSnd){
                ImgRcpSnd = a_CkRcpSnd;
        }

        [Required]
        [FilesCountAttribute(5)]
        [FileTypes("jpg,jpeg,png")]
        [FileSizeAttribute(3000000)]
        public HttpPostedFileBase[] files { get; set; }

    }

    public class SndUrlRcpModel : BaseRcpModel {

        public UrlRcp UrlRcpSnd { get; private set; }

        public SndUrlRcpModel() : this(new UrlRcp()) { }

        public SndUrlRcpModel(UrlRcp a_CkRcpSnd)
            : base(a_CkRcpSnd) {
            UrlRcpSnd = a_CkRcpSnd;
        }

        [Required]
        [Display(Name = "Recipe Url")]
        public string Url {   
            get { return UrlRcpSnd.RcpUrl; }
            set { UrlRcpSnd.RcpUrl = value; }
        }

    }

    public class EditCKRcpModel : SndCKRcpModel {
        public EditCKRcpModel() {

        }

        public EditCKRcpModel(CkRtnRcp a_getRcp, CkSndRcp a_sendRcp):base(a_sendRcp) {

            if (a_getRcp.PreparationTimeMin.HasValue) {
                PreparationTime = TimeSpan.FromMinutes(a_getRcp.PreparationTimeMin.Value);
            }

            ImageID = a_getRcp.CKRcpImage;
        }

        public long? ImageID { get; set; }
        public TimeSpan PreparationTime { get; set; }
    }

    public class FileModel{

        [FilesCountAttribute(1)]
        [FileTypes("jpg,jpeg,png")]
        [FileSizeAttribute(3000000)]
        public HttpPostedFileBase[] files { get; set; }
    }

    public class RcpSearchResModel : BaseRcpModel {

        public RcpSearchResModel(CkRcpDetails a_rcp):base(a_rcp) {
            ImageSrc = a_rcp.RcpDetailsImageSrc;
        }

        public string ImageSrc { get; set; }
        public RecipeSrcEnum RecipeSrcEnum { get { return m_rcpDetailes.RecipeSrc; } }
    }

    public class SearchModel 
    {
        public SearchModel() {
            
        }

        public string OwnerName { get; set; }

        public bool SearchInCurrentUserBook { get; set; }

        public int[] SelectedCtgrs { get; set; }

        [StringLength(200)]
        public string SearchText { get; set; }
        public string Discription{ get; set; }
        public long LastIndex { get; set; }

    } 
}