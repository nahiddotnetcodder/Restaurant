using System.Globalization;

namespace RMS.Models
{
    public class AccJournalViewModel
    {
        public int AJId { get; set; }
        public DateTime AJTrDate { get; set; }
        public string AJTrDateString
        {
            get
            {
                return AJTrDate == null ? string.Empty : AJTrDate.ToString("yyyy-MM-dd");
            }
        }
        public DateTime AJDDate { get; set; }
        public string AJDDateString
        {
            get
            {
                return AJDDate == null ? string.Empty : AJDDate.ToString("yyyy-MM-dd");
            }
        }
        public DateTime AJEDate { get; set; }
        public string AJEDateString
        {
            get
            {
                return AJEDate == null ? string.Empty : AJEDate.ToString("yyyy-MM-dd");
            }
        }
        public string JournalType { get; set; }
        public string AJRef { get; set; }
        public string AJSoRef { get; set; }
        public double AJAmount { get; set; }
        public string AmountString
        {
            get
            {
                NumberFormatInfo nfo = new NumberFormatInfo
                {
                    CurrencyGroupSeparator = ",",
                    CurrencyGroupSizes = new int[] { 3, 2 },
                    CurrencySymbol = ""
                };
                return AJAmount.ToString("c0", nfo);
            }
        }
        public string AJMemo { get; set; }
        public string CUser { get; set; }
        public List<AccGlTransViewModel> Items { get; set; }
    }
    public class AccGlTransViewModel
    {
        public int AGTId { get; set; }
        public DateTime AJTrDate { get; set; }
        public string AJTrDateString
        {
            get
            {
                return AJTrDate == null ? string.Empty : AJTrDate.ToString("MM/dd/yyyy");
            }
        }
        public int AJTrNo { get; set; }
        public string AGTAccCode { get; set; }
        public string AGTAccDescription { get; set; }
        public double AGTDebitAccount { get; set; }
        public double AGTCreditAccount { get; set; }
        public string AGTMemo { get; set; }
    }
}
