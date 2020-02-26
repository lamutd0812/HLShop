using System;
using System.ComponentModel.DataAnnotations;

namespace HLShop.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Phải nhập tên!")]
        public string Fullname { get; set; }

        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Phải nhập tên đăng nhập!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Phải nhập mật khẩu!")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải dài ít nhất 6 ký tự!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phải nhập email!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Phải nhập số điện thoại!")]
        public string PhoneNumber { get; set; }
    }
}