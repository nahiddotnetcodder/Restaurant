using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class HRDesignation
    {
        [Key]
        public int HRDeId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Designation Name")]
        public string HRDeName { get; set; }
        [StringLength(50)]
        [Display(Name = "Description")]
        public string HRDeDes { get; set; }
    }
}
