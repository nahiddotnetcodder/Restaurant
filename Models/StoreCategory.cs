using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class StoreCategory
    {
        [Key]
        public int SCId { get; set; }

        [Required(ErrorMessage = "The Name field is Required")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Category Name")]
        public string SCName { get; set; }

        public string CUser { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

    }
}
