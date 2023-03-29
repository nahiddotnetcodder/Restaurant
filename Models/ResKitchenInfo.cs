using System.ComponentModel;

namespace RMS.Models
{
    public class ResKitchenInfo
    {
        [Key]
        [DisplayName("Kitchen ID")]
        public int RKId { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Kitchen Name")]
        public string RKitchenName { get; set; }
        [StringLength(250)]
        [DisplayName("Description")]
        public string RKDescription { get; set; }
    }
}
