namespace RMS.Models
{
    public class AccJournal
    {
        [Key]
        public int AJId { get; set; }
        public DateTime AJTrDate { get; set; }
        [StringLength(50)]
        public string AJRefPrefix { get; set; }
        [StringLength(50)]
        public string AJRef { get; set; }
        [StringLength(50)]
        public string AJType { get; set; }
        [StringLength(50)]
        public string AJSoRef { get; set; }
        public DateTime AJEDate { get; set; }
        public DateTime AJDDate { get; set; }
        public double AJAmount { get; set; }
        [StringLength(250)]
        public string AJMemo { get; set; }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [NotMapped]
        public string Items { get; set; }
        public List<AccGlTrans> AccGlTrans { get; set; }
    }
}
