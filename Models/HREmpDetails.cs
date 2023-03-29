using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RMS.Models
{
    public class HREmpDetails
    {
        [Key]
        public int HREDId { get; set; }
        [Required]
        [ForeignKey("HRDepart")]
        [Display(Name = "Department")]
        public int HRDId { get; set; }
        public virtual HRDepartment HRDepart { get; set; }
        [Required]
        [ForeignKey("HRDesig")]
        [Display(Name = "Designation")]
        public int HRDeId { get; set; }
        public virtual HRDesignation HRDesig { get; set; }
        

        [StringLength(10)]
        [Display(Name = "Emp. Id")]
        public string HREDEId { get; set; }
        [Required]
        [StringLength(150)]
        [Display(Name = "Employee Name")]
        public string HREDEName { get; set; }
        [Required]
        [StringLength(150)]
        [Display(Name = "Fathers Name")]
        public string HREDFName { get; set; }
        [Required]
        [StringLength(250)]
        [Display(Name = "Present Address")]
        public string HREDPreAdd { get; set; }
        [Required]
        [StringLength(250)]
        [Display(Name = "Permanent Address")]
        public string HREDParAdd { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Contact No:")]
        public string HREDCont { get; set; }
        [Required]
        [Display(Name = "Blood Group")]
        public HDEDBGroup HREDBGroup { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        [StringLength(50)]
        [Display(Name = "Nationality")]
        public string HREDNat { get; set; }
        [Required]
        [Display(Name = "Religion")]
        public HDEDReligion HREDReligion { get; set; }
        [Required]
        [Display(Name = "Marital Status")]
        public HDEDMStatus HREDMStatus { get; set; }
        [StringLength(150)]
        [Display(Name = "Refer Name")]
        public string HREDRef { get; set; }
        [StringLength(250)]
        [Display(Name = "Refer Address")]
        public string HREDRAdd { get; set; }
        [Required]
        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public DateTime HREDJDate { get; set; }=DateTime.Now;
        [Required]
        [ForeignKey("HRWStatus")]
        [Display(Name = "Work Status")]
        public int HRWSId { get; set; }
        public virtual HRWStatus HRWStatus { get; set; }
        [StringLength(100)]
        [Display(Name = "Email")]
        public string HREDEmail { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Display(Name ="Basic")]
        public float HREDBasic { get; set; }

        [Display(Name = "Is Waiter?")]
        public bool IsWaiter { get; set; }
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }
        [StringLength(250)]
        [Required]
        public string HREDPUrl { get; set; } = "noimage.png";
        [NotMapped]
        [Display(Name = "Employee Photo")]
        public IFormFile HREDPPhoto { get; set; }
        [NotMapped]
        public string HREDBPhotoName { get; set; }
        [Required]
        [StringLength(50)]
        public string CUser { get; set; } 
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}

namespace RMS.Models
{
    public enum HDEDBGroup
    {
        [Display(Name ="A+")] Apo, 
        [Display(Name ="A-")] Aneg, 
        [Display(Name ="AB+")] ABpos, 
        [Display(Name ="AB-")] ABneg, 
        [Display(Name ="B+")] Bpos, 
        [Display(Name ="B-")] Bneg, 
        [Display(Name ="O+")] Opos, 
        [Display(Name ="O-")] Oneg
    }
}

namespace RMS.Models
{
    public enum HDEDReligion
    {
        Muslim, Hindu, Buddist, Christian, Other
    }
}

namespace RMS.Models
{
    public enum HDEDMStatus
    {
        Married, UnMarried, Other
    }
}

namespace RMS.Models
{
    public enum Gender
    {
        Male, Female, Other
    }
}