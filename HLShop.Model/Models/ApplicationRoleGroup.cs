﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLShop.Model.Models
{
    [Table("ApplicationRoleGroups")]
    public class ApplicationRoleGroup
    {
        [Key]
        [Column(Order = 1)]
        public int GroupId { get; set; }

        [Key]
        [Column(Order = 2)]
        [MaxLength(128)]
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole ApplicationRole { get; set; }

        [ForeignKey("GroupId")]
        public ApplicationGroup ApplicationGroup { get; set; }
    }
}
