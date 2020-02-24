using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLShop.Model.Models
{
    [Table("ContextDetails")]
    public class ContactDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [MaxLength(250)]
        public string Email { get; set; }

        [MaxLength(250)]
        public string Website { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        public string Other { get; set; }

        public double? Lat { get; set; } //vĩ độ

        public double? Lng { get; set; } //kinh độ

        public bool Status { get; set; }
    }
}
