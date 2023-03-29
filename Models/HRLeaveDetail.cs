using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class HRLeaveDetail
    {
        [Key]
        public int HRLDId { get; set; }

        [ForeignKey("HREmpDetails")]
        [Display(Name = "Employee Name")]
        public int HREDId { get; set; }
        public virtual HREmpDetails HREmpDetails { get; private set; }
        public virtual List<HREmpDetails> HREDEName { get; set; } = new List<HREmpDetails>();

        [ForeignKey("HRLeavePolicy")]
        [Display(Name = "Leave Policy Name")]
        public int HRLPId { get; set; }
        public virtual HRLeavePolicy HRLeavePolicy { get; private set; }
        public virtual List<HRLeavePolicy> HRLPName { get; set; } = new List<HRLeavePolicy>();

        [Required(ErrorMessage = "The field is required")]
        [StringLength(50)]
        [Display(Name ="Leave App. Ref. No.")]
        [MaxLength(50)]
        public String HRLDAppSl { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="Leave App. Date")]
        public DateTime HRLDAppDate { get; set; }=DateTime.Now;

        [DataType(DataType.Date)]
        [Display(Name = "Leave Start Date")]
        public DateTime HRLDLeaveSDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [Display(Name = "Leave End Date")]
        public DateTime HRLDLeaveEDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage ="The field is required")]
        [StringLength(250)]
        [MaxLength(50)]
        [Display(Name ="Leave Reason")]
        public string HRLDReason { get; set; }

        [Display(Name = "Remaining Day")]
        public int HREDIdSu { get; set; }

        [Display(Name ="Total Day")]
        public int HRLDTDay { get; set; }


        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
