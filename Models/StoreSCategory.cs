using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class StoreSCategory
    {
        [Key]
        public int SSCId { get; set; }

       
        [ForeignKey("StoreCat")]
        [Display(Name = "Category Name")]
        public int SCId { get; set; }
        public virtual StoreCategory StoreCat { get; set; }
        
        [NotMapped]
        public string StoreCatName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Sub-Category Name")]
        [MaxLength(50)]
        [StringLength(50)]
        public string SSCName { get; set; }

        public string CUser { get; set; } 

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
