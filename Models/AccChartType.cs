
namespace RMS.Models
{
    public class AccChartType
    {
        [Key]
        public int ACTId { get; set; }
        public int ACTClassId { get; set; }
        [StringLength(100)]
        public string ACTName { get; set; }
        [ForeignKey("AccChartClasss")]
        public int? ACCId { get; set; }
        public virtual AccChartClass AccChartClasss { get; set; }
        [NotMapped]
        public string AccChartClassName { get; set; }
        public int? ACTParentId { get; set; }
        public string ACTParentName { get; set; }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
