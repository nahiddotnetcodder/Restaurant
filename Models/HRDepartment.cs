using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class HRDepartment
    {
        [Key]
        public int HRDId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Department Name")]
        public string HRDName { get; set; }
        [StringLength(250)]
        [Display(Name = "Description")]
        public string HRDDes { get; set; }

    }
}

