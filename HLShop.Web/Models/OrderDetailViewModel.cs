using HLShop.Model.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HLShop.Web.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { get; set; }

        public int ProductID { get; set; }

        [Required]
        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}