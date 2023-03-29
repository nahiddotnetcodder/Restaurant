using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class StoreDClose
    {
        [Key]
        public int SDCId { get; set; }

        [Required]
        [DisplayName("Current Date (MM/DD/YYYY): ")]
        [DataType(DataType.Date)]
        public DateTime SDCDate { get; set; } = DateTime.Now.Date;

        [Required]
        [StringLength(50)]
        [DisplayName("Last Day Closed User:")]
        public string CUser { get; set; } 


    }
}
