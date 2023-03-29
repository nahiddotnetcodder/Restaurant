using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class ResFoodType
    {
        [Key]
        [DisplayName("Food Type ID")]
        public int RFTId { get; set; }

        [Required(ErrorMessage = "The F.T Name field is Required")]
        [StringLength(50)]
        [Display(Name="Food Type Name")]
        public string RFTName { get; set; }
        
        [StringLength(250)]
        [Display(Name ="Description")]
        public string RFTDescription { get; set; }
    }
}
