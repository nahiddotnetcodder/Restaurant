using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class ResInfo
    {
        [Key]
        [DisplayName("Restaurant ID:")]
        public int ResId { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("Restaurant Name:")]
        public string RName { get; set; }
        [Required]
        [StringLength(250)]
        [DisplayName("Restaurant Address:")]
        public string RAddress { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("City/Area:")]
        public string RCity { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Contact Details:")]
        public string RPhone { get; set; }
        [StringLength(50)]
        [DisplayName("Email Address:")]
        public string REmail { get; set; }
        [Required]
        [DisplayName("Service Charge (%)")]
        public int RSCharge { get; set; }
        [Required]
        [DisplayName("Govt. Tax (%)")]
        public int RTax { get; set; }
        [StringLength(250)]
        [Required]
        public string RCLogoUrl { get; set; } = "noimage.png";
        [NotMapped]
        [Display(Name = "Company Logo")]
        public IFormFile RCLogo { get; set; }
        [NotMapped]
        public string RCLogoName { get; set; }
    }
}
