using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class StoreGReceiveDetails
    {
        [Key]
        public int GRDId { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public string Unit { get; set; }

        [Display(Name = "Quantity")]
        [Required]
        [Range(1, 1000, ErrorMessage = "Quantity should be greater then 0 and 1000")]
        public float GRDQty { get; set; }


        [Range(1, 10000000, ErrorMessage = "Price should be greater than 0")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "smallmoney")]
        [Required]
        [Display(Name = "Unit Price")]
        public float GRDUPrice { get; set; }


        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Display(Name = "Total Price")]
        [Required]
        public int GRDTPrice { get; set; }

        [ForeignKey("StoreGReceiveMaster")]
        public int GRMId { get; set; }
        public virtual StoreGReceiveMaster StoreGReceiveMaster { get;  set; }


    }
}
