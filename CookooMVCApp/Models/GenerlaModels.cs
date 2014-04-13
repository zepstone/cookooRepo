using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CookooMVCApp.Models
{
    public class ErrorModel
    {
        public ErrorModel(string a_message)
        {
            Message = a_message;
        }
        public string Message{ get; set; }
    }
}
