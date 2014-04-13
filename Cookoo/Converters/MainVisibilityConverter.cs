using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Cookoo;
using System.Windows.Controls;
using System.Windows;

namespace CookooUI.Converters {
    public class MainVisibilityConverter : IValueConverter {

        private readonly string RecipeEdit = "grdRecipeEdit";
        private readonly string Recipe = "grdRecipe";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {

            if (value != null) {
                MainWinStateEnum state = (MainWinStateEnum)value;
                string controlName = parameter.ToString();

                switch (state) {
                    case MainWinStateEnum.Regular:

                        if (controlName == RecipeEdit) {
                            return Visibility.Collapsed;
                        }
                        else if (controlName == Recipe) {
                            return Visibility.Visible;
                        }

                        break;
                    case MainWinStateEnum.EditRecipe:
                        if (controlName == RecipeEdit) {
                            return Visibility.Visible;
                        }
                        else if (controlName == Recipe) {
                            return Visibility.Collapsed;
                        }
                        break;
                    default:
                        break;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
