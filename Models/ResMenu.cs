using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class ResMenu
    {
        [Key]
        public int RMId { get; set; }
        [Required]
        [ForeignKey("ResKInfo")]
        [Display(Name = "Kitchen Type")]
        public int RKId { get; set; }
        public virtual ResKitchenInfo ResKInfo { get; set; }
        [Required]
        [ForeignKey("ResFtype")]
        [Display(Name = "Food Type")]
        public int RFTId { get; set; }
        public virtual ResFoodType ResFtype { get; set; }
        [Required]
        [Display(Name = "Item Code")]
        [StringLength(10)]
        public string RMItemCode { get; set; }
        [Required]
        [Display(Name = "Item Name")]
        [StringLength(250)]
        public string RMItemName { get; set; }
        [Required]
        [Display(Name = "Unit Price")]
        public double RMUPrice { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Created User")]
        public string CUser { get; set; } 
        [Required]
        [Display(Name = "Created Date-Time")]
        public DateTime CreateDate { get; set; }= DateTime.Now;
    }
}
