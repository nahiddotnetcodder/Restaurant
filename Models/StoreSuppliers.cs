using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class StoreSuppliers
    {
        [Key]
        public int SSId { get; set; }

        [Required(ErrorMessage = "Supplier Name is required")]
        [Display(Name = "Supplier Name")]
        [MaxLength(100)]
        [StringLength(100)]
        public string SSName { get; set; }

        [Required(ErrorMessage = "Supplier Short Name is required")]
        [Display(Name = "Supplier Short Name")]
        [MaxLength(20)]
        [StringLength(20)]
        public string SSSName { get; set; }

        [MaxLength(250)]
        [Display(Name = "Office Address")]
        public string SSOAdd { get; set; }

        [Required(ErrorMessage = "Contact Person Name is required")]
        [Display(Name = "Contact Person")]
        [MaxLength(150)]
        [StringLength(150)]
        public string SSCPerson { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [Display(Name = "Contact Number")]
        [MaxLength(100)]
        [StringLength(100)]
        public string SSCNumber { get; set; }

        [MaxLength(100)]
        [StringLength(100)]
        [Display(Name = "E-mail (If any)")]
        public string SSEmail { get; set; }

        [MaxLength(100)]
        [StringLength(100)]
        [Display(Name = "Bank Name/Account")]
        public string SSBName { get; set; }

        [MaxLength(250)]
        [Display(Name = "General Notes")]
        public string SSGNotes { get; set; }

        [ForeignKey("AccMaster")]
        [Display(Name = "Payable Account")]
        public int ACMId { get; set; }
        public virtual AccChartMaster AccMaster { get; set; }


        public string CUser { get; set; } 

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
