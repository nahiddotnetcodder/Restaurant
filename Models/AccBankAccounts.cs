using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class AccBankAccounts
    {
        [Key]
        public int ABAId { get; set; }
        //[ForeignKey("AccChartMasters")]
        public string ACMAccCode { get; set; }
        //public virtual AccChartMaster AccChartMasters { get; set; }
        public ABAAType ABAAType { get; set; }
        [NotMapped]
        public string ABAATypeName
        {
            get
            {
                return EnumUtility.GetDescriptionFromEnumValue((ABAAType)ABAAType);
            }
        }
        [StringLength(100)]
        public  string  ABABAName { get; set; }
        [StringLength(100)]
        public string ABABANumber { get; set; }
        [StringLength(100)]
        public string ABABName { get; set; }
        [StringLength(250)]
        public string ABABAdd { get; set; }
        //[ForeignKey("AccChartMasterss")]
        public string ABABCCode { get; set; }
        //public virtual AccChartMaster AccChartMasterss { get; set; }
        public DateTime ABALRDate { get; set; }
        public double? ABAERBal { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}

namespace RMS.Models
{
    public enum ABAAType
    {
        [Description("Saving Account")] SavingAccount,
        [Description("Current Account")] CurrentAccount,
        [Description("Chequing Account")] ChequingAccount,
        [Description("Credit Account")] CreditAccount
    }
}