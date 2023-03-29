namespace RMS.Models
{
    public class AccGlTrans
    {
        [Key]
        public int AGTId { get; set; }
        public int AJTrNo { get; set; }
        [ForeignKey("AJTrNo")]
        public virtual AccJournal AccJournal { get; set; }
        [StringLength(20)]
        public string AGTAccCode { get; set; }
        public string AGTAccDescription { get; set; }
        public double? AGTDebitAccount { get; set; }
        public double? AGTCreditAccount { get; set; }
        [StringLength(200)]
        public string AGTMemo { get; set; }
    }
}