using System.ComponentModel;

namespace RMS.Models
{
    public class AccChartClass
    {
        [Key]
        public int ACCId { get; set; }
        public int ClassId { get; set; }
        [StringLength(100)]
        public string ACCName { get; set; }
        public ACCCType ACCCType { get; set; }
        [NotMapped]
        public string ACCCTypeName
        {
            get
            {
                return EnumUtility.GetDescriptionFromEnumValue((ACCCType)ACCCType);
            }
        }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;
    }
}

namespace RMS.Models
{
    public enum ACCCType
    {
        [Description("Assets")] Assets, [Description("Liabilities")] Liabilities, [Description("Equity")] Equity, [Description("Income")] Income, [Description("Cost of Goods Sold")] CostofGoodsSold, [Description("Expense")] Expense
    }
    public enum YesNo
    {
        [Description("No")] No,
        [Description("Yes")] Yes
    }
}