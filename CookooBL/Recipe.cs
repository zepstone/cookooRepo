using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace CookooBL {

    [Serializable()] 
    public class Recipe {

        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string RecipeCategory { get; set; }
        public RecipeTypeEnum? RecipeType { get; set; }
        public DoseTypeEnum? DoseType { get; set; }
        public string PicPath { get; set; }
    }

    [Serializable()] 
    public enum RecipeTypeEnum {
        חלבי,
        בשרי,
        פרווה,
    }

    [Serializable()] 
    public enum DoseTypeEnum {
        סלטים,
        מנת_פתיחה,
        תוספות,
        מנה_עיקרית,
        מנה_אחרונה
    }
}
