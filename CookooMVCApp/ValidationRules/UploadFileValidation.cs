using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CookooMVCApp.ValidationRules
{

    public class ModelClientFileTypesValidationRule : ModelClientValidationRule
    {
        public ModelClientFileTypesValidationRule(string errorMessage, string types)
        {
            ErrorMessage = errorMessage;

            ValidationType = "filetypes";

            ValidationParameters.Add("types", types);
        }

    }

    public class FilesCountAttribute : ValidationAttribute 
    {
        private readonly int _maxCount;
        public FilesCountAttribute(int a_maxCount)
        {
            _maxCount = a_maxCount;
        }

        public override bool IsValid(object value) {
            if (value == null) return true;

            var l_files = value as HttpPostedFileBase[];

            return l_files.Count() <= _maxCount;
        }


        public override string FormatErrorMessage(string name) {
            return string.Format("The files count should  not exceed {0}", _maxCount);
        }
    }

    public class FileSizeAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly int _maxSize;

        public FileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var l_files = value as HttpPostedFileBase[];

            for (int i = 0; i < l_files.Count(); i++)
			{
                if (l_files[i] != null) {
                    if (l_files[0].ContentLength > _maxSize) {
                        return false;
                    }
                }
			}

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("The file size should not exceed {0}", _maxSize);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
                                                                               ControllerContext context) {
            var returnVar = new ModelClientValidationRule {
                ErrorMessage = FormatErrorMessage(string.Empty),
                ValidationType = "maxfilesize",
            };
            returnVar.ValidationParameters.Add("size", _maxSize);
            yield return returnVar;
        }
    }

    public class FileTypesAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly List<string> _types;
        private string _stringTypes; 
        public FileTypesAttribute(string types)
        {
            _stringTypes = types;
            _types = types.Split(',').ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;


            var l_files = value as HttpPostedFileBase[];

            for (int i = 0; i < l_files.Count(); i++) {

                if (l_files[i] != null) {
                    var fileExt = System.IO.Path.GetExtension(l_files[i].FileName).Substring(1);

                    if (!_types.Contains(fileExt, StringComparer.OrdinalIgnoreCase)) {
                        return false;
                    }
                }
           
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Invalid file type. Only the following types {0} are supported.", String.Join(", ", _types));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var returnVar = new ModelClientValidationRule {
                ErrorMessage = FormatErrorMessage(string.Empty),
                ValidationType = "filetypes",
            };
            returnVar.ValidationParameters.Add("types", string.Join(",", _types));
            yield return returnVar;
        }
    }

    public class MinIntArrayAttribute : ValidationAttribute, IClientValidatable {
        private readonly int _minSize;

        public MinIntArrayAttribute(int a_minSize) {
            _minSize = a_minSize;
        }

        public override bool IsValid(object value) {
            if (value == null)
                return false;

            var l_array = value as int[];

            return l_array.Count() >= _minSize;
        }

        public override string FormatErrorMessage(string name) {
            return string.Format("min  list count - {0}", _minSize);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
                                                                               ControllerContext context) {
            var returnVar = new ModelClientValidationRule {
                ErrorMessage = FormatErrorMessage(string.Empty),
                ValidationType = "minintarraysize",
            };
            returnVar.ValidationParameters.Add("size", _minSize);
            yield return returnVar;
        }
    }

    public class MaxWordLetters : ValidationAttribute, IClientValidatable {

         private readonly int _maxSize;

         public MaxWordLetters(int a_minSize) {
            _maxSize = a_minSize;
        }

         public override bool IsValid(object value) {
             if (value == null)
                 return true;

             var l_string = value as string;
             var l_words = l_string.Split(' ','\n','\r');

             for (int i = 0; i < l_words.Length; i++) {
                 if (l_words[i].Count() > _maxSize) {
                     return false;
                 }
             }

             return true;
         }

         public override string FormatErrorMessage(string name) {
             return string.Format("you have a vary vary long word ,max letters ni words is", _maxSize);
         }
         public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context) {
             var returnVar = new ModelClientValidationRule {
                 ErrorMessage = FormatErrorMessage(string.Empty),
                 ValidationType = "maxletters",
             };
             returnVar.ValidationParameters.Add("size", _maxSize);
             yield return returnVar;
         }
    }
}