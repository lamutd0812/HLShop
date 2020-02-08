using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HLShop.Web.Models
{
    public class PostTagViewModel
    {
        public int PostID { set; get; }

       
        public string TagID { set; get; }

      
        public virtual PostViewModel Post { get; set; }

        
        public virtual TagViewModel Tag { get; set; }
    }
}