using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RMS.Models
{
    public class HREmpAtt
    {
        [Key]
        public int HREAId { get; set; }
        [ForeignKey("HREmpDetails")]
        [Required]
        [Display(Name = "Employee Name:")]
        public int HREDId { get; set; }
        public virtual HREmpDetails HREmpDetails { get;  set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime HREADate { get; set; } = DateTime.Now;

        [Display(Name = "In Time")]
        [DataType(DataType.Time)]
        public DateTime  HREAInTime { get; set; }

        [Display(Name = "Out Time")]
        [DataType(DataType.Time)]
        public DateTime  HREAOutTime { get; set; }

        [Display(Name = "Minute")]
        [Required(ErrorMessage ="The Minute feid is required")]
        public int HREATMinute { get; set; }

        [Required]
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
