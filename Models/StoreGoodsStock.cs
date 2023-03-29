using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class StoreGoodsStock
    {
        [Key]
        public int SGSId { get; set; }

        [Display(Name ="Item Code")]
        [StringLength(100)]
        public string ItemCode { get; set; }
        [Display(Name = "Item Name")]
        [StringLength(100)]
        public string ItemName { get; set; }
        public string Unit { get; set; }
        [Display(Name = "Quantity")]
        public float SGSQty { get; set; }
        [Display(Name = "Price")]
        public float SGSUPrice { get; set; }
        [Display(Name = "Total Price")]
        public int SGSTPrice { get; set; }
    }
}
