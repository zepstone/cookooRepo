using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookooOrc.interfaces;
using CookooOrc.Implemented;

namespace CookooOrc {
    public class LngFactory {

        public static IConstStrings GetLng(LanguageEnum a_lan) {

            switch (a_lan) {
                case LanguageEnum.English:
                      return new enString();
                case LanguageEnum.עברית:
                      return new heSrings();
                default:
                      return new enString();
            }
        }
    }
}
