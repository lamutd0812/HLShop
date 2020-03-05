using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HLShop.Model.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace HLShop.Model.Models
{
    [Table("Orders")]
    public class Order : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerEmail { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerMobile { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerMessage { get; set; }

        [MaxLength(256)]
        public string PaymentMethod { get; set; }

        [MaxLength(256)]
        public string PaymentStatus { get; set; }

        [MaxLength(128)]
        [Column(TypeName = "nvarchar")]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual ApplicationUser User { get; set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}