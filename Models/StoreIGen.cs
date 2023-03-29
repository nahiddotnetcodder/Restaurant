using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class StoreIGen
    {
        [Key]
        public int SIGId { get; set; }

      
        [ForeignKey("StoreCategory")]
        [Display(Name = "Category Name")]
        public int SCId { get; set; }
        public virtual StoreCategory StoreCategory { get; set; }

       
        [ForeignKey("StoreSCategory")]
        [Display(Name = "Sub-Category Name")]
        public int? SSCId { get; set; }
        public virtual StoreSCategory StoreSCategory { get; set; }


        [Required(ErrorMessage ="Item Code field is required")]
        [Display(Name = "Item Code")]
        [MaxLength(50)]
        [StringLength(50)]
        public string SIGItemCode { get; set; }


        [Required(ErrorMessage ="Item Name field is required")]
        [Display(Name = "Item Name")]
        [MaxLength(50)]
        [StringLength(50)]
        public string SIGItemName { get; set; }

       
        [ForeignKey("StoreUnits")]
        [Display(Name = "Unit")]
        public int SUId { get; set; }
        public virtual StoreUnit StoreUnits { get; private set; }

        [Required(ErrorMessage = "This Level is required")]
        [Display(Name = "Re-Ordar Level")]
        public int SIGRLevel { get; set; }

        [MaxLength(250)]
        [Display(Name = "Remarks")]
        public string SIGRemarks { get; set; }

        public string CUser { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
