using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RMS.Models
{
    public class StoreGIssueDetails
    {
        [Key]
        public int GIDId { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Unit{ get; set; }

        [Display(Name = "Quantity")]
        [Required]
        [Range(1, 1000, ErrorMessage = "Quantity should be greater then 0 and 1000")]
        public float GIDQty { get; set; }

        [Range(1, 10000000, ErrorMessage = "Price should be greater than 0")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "smallmoney")]
        [Required]
        [Display(Name = "Unit Price")]
        public float GIDUPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Display(Name = "Total Price")]
        [Required]
        public int GIDTPrice { get; set; }

        [ForeignKey("StoreGIssueMaster")]
        public int GIMId { get; set; }
        public virtual StoreGIssueMaster StoreGIssueMaster { get;  set; }

    }
}
