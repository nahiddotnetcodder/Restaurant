using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RMS.Models
{
    public class HRLeavePolicy
    {
        [Key]
        public int HRLPId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Leave Policy Name")]
        [MaxLength(50)]
        [Display(Name = "Leave Policy Name")]
        public string HRLPName { get; set; }

        [Display(Name ="Total Days")]
        public int HRLPTDay { get; set; }

        [StringLength(50)]
        public string CUser { get; set; } 
        public DateTime CreateDate { get; set; }=DateTime.Now;
    }
}
