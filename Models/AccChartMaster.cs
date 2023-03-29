using System.ComponentModel;

namespace RMS.Models
{
    public class AccChartMaster
    {
        [Key]
        public int ACMId { get; set; }
        [Required]
        [StringLength(20)]
        public string ACMAccCode { get; set; }
        [Required]
        [Display(Name = "Payable Account")]
        [StringLength(200)]
        public string ACMAccName { get; set; }
        [Required]
        [ForeignKey("AccChartTypes")]
        public int ACTId { get; set; }
        public virtual AccChartType AccChartTypes { get; set; }
        [Required]
        public ACMAI ACMAI { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;

    }
}

namespace RMS.Models
{
    public enum ACMAI
    {
        Active = 0, Inactive = 1
    }
}