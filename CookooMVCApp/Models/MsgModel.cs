using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CookooOrc;
using CookooMVCApp.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace CookooMVCApp.Models
{
    public class MsgModel 
    {
        public MsgModel ()
	    {

	    }

        public MsgModel(CKMessage a_CkMsg) {

            using (UsersContext l_Cntx = new UsersContext())
            {
                string l_ownerName = l_Cntx.UserProfiles.FirstOrDefault(x => x.UserId == a_CkMsg.FromUserID).UserName;
                FromUser = l_ownerName;
            }

            Content = a_CkMsg.MsgContent;
            Title = a_CkMsg.MsgTitle;
            IsRead = a_CkMsg.IsRead;
            MsgTime = a_CkMsg.TimeCreated;
        }

        public int[] ToUsersId { get; set; }
        public string FromUser { get; set; }

  
        [MaxWordLetters(20)]
        [StringLength(1000)]
        public string Content { get; set; }

        public bool IsRead { get; set; }

        [Required]
        [MaxWordLetters(20)]
        [StringLength(1000)]
        public string Title { get; set; }
     
        public DateTime MsgTime { get; set; }

    }
}
