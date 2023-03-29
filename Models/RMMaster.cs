namespace RMS.Models
{
    public class RMMaster
    {
        [Key]
        public int RMMId { get; set; }
        [StringLength(50)]
        public string RMItemCode { get; set; }

        [StringLength(250)]
        public string RMItemName { get; set; }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;
        [NotMapped]
        public string Items { get; set; }
        public List<RMDetails> RMDetails { get; set; }
    }
}
