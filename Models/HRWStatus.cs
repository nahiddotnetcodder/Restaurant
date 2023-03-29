using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RMS.Models
{
    public class HRWStatus
    {
        [Key]
        public int HRWSId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Status Name")]
        [MaxLength(50)]
        public string HRWSName { get; set; }
        [StringLength(50)]
        [MaxLength(50)]
        [Display(Name = "Description")]
        public string HRWSDes { get; set; }
    }
}
