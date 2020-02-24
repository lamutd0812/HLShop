using System.ComponentModel.DataAnnotations;

namespace HLShop.Web.Models
{
    public class ContactDetailViewModel
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Tên contact không vượt quá 250 ký tự!")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = "SĐT không vượt quá 50 ký tự!")]
        public string Phone { get; set; }

        [MaxLength(250, ErrorMessage = "Email không vượt quá 250 ký tự!")]
        public string Email { get; set; }

        [MaxLength(250, ErrorMessage = "Tên Website không vượt quá 250 ký tự!")]
        public string Website { get; set; }

        [MaxLength(250, ErrorMessage = "Địa chỉ không vượt quá 250 ký tự!")]
        public string Address { get; set; }

        public string Other { get; set; }

        public double? Lat { get; set; } //vĩ độ

        public double? Lng { get; set; } //kinh độ

        public bool Status { get; set; }
    }
}