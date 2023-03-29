using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class StoreUnit
    {
        [Key]
        public int SUId { get; set; }

        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "The Name field is required")]
        [Display(Name = "Unit Name")]
        public string SUName { get; set; }

        public string CUser { get; set; } 

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
