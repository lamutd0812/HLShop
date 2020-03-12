using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HLShop.Web.Models
{
    public class RecoverViewModel
    {
        [Required(ErrorMessage = "Email không được bỏ trống!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}