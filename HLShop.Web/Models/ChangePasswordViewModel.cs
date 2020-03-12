using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HLShop.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống!")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải dài tối thiếu 6 ký tự.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được bỏ trống!")]
        [MinLength(6, ErrorMessage = "Xác nhận mật khẩu phải dài tối thiếu 6 ký tự.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu phải trùng nhau!")]
        public string ConfirmPassword { get; set; }
    }
}