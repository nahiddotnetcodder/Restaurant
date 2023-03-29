using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class ResTable
    {
        [Key]
        public int RTId { get; set; }

        [Required(ErrorMessage = "The Number field is Required")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Table Number")]
        public string RTNumber { get; set; }
        [Required(ErrorMessage = "The Description field is Required")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Table Description")]
        public string RTDescription { get; set; }

        public string CUser { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

    }
}
