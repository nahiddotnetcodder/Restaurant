using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RMS.Models
{
    public class StoreGIssueMaster
    {
        [Key]
        public int GIMId { get; set; }

        public DateTime GIMDate { get; set; }
        [NotMapped]
        public string GIMDateString
        {
            get
            {
                return GIMDate == null ? string.Empty : GIMDate.ToString("MM/dd/yyyy");
            }
        }
        [ForeignKey("HRDepart")]
        [Required(ErrorMessage = "Department feild is required")]
        [Display(Name = "Issuing Department")]
        public int HRDId { get; set; }
        public virtual HRDepartment HRDepart { get; set; }
        [NotMapped]
        public string HRDepartName { get; set; }


        [Display(Name = "Remarks")]
        [StringLength(50)]
        [MaxLength(250)]
        public string GIMRemarks { get; set; }

        public string CUser { get; set; } 
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [NotMapped]
        public string Items { get; set; }
        public virtual List<StoreGIssueDetails> StoreGIssueDetails { get; set; }

    }
}
