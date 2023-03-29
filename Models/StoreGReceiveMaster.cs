using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class StoreGReceiveMaster
    {
        [Key]
        public int GRMId { get; set; }


        public DateTime GRMDate { get; set; }
        [NotMapped]
        public string GRMDateString
        {
            get
            {
                return GRMDate == null ? string.Empty : GRMDate.ToString("MM/dd/yyyy");
            }
        }

        [ForeignKey("StoreSuppliers")]
        [Display(Name = "Supplier Name")]
        [Required(ErrorMessage = "Supplier Name feild is required")]
        public int SSId { get; set; }
        public virtual StoreSuppliers StoreSuppliers { get; set; }
        [NotMapped]
        public string StoreSuppliersName { get; set; }

        [MaxLength(250)]
        [Display(Name = "Remarks")]
        public string GRMRemarks { get; set; }

        public string CUser { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
        [NotMapped]
        public string Items { get; set; }
        public virtual List<StoreGReceiveDetails> StoreGReceiveDetails { get; set; } 

    }
}
