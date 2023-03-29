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
    public class HREmpRoaster
    {
        [Key]
        public int HRERId { get; set; }

        [ForeignKey("HREmpDetails")]
        public int? HREDId { get; set; }
        public virtual HREmpDetails HREmpDetails { get; set; }
        //public virtual List<HREmpDetails> HREDEName { get; set; } = new List<HREmpDetails>();
        [NotMapped]
        public string HREmpDetailsName { get; set; }

        
        public DateTime HRERFDate { get; set; }

        [NotMapped]
        public string HRERFDateString
        {
            get
            {
                return HRERFDate == null? string.Empty: HRERFDate.ToString("MM/dd/yyyy");
            }
        }

        public DateTime HRERTDate { get; set; }
        [NotMapped]
        public string HRERTDateString
        {
            get
            {
                return HRERTDate == null ? string.Empty : HRERTDate.ToString("MM/dd/yyyy");
            }
        }

        public ShiftType ShiftType { get; set; }

        [NotMapped]
        public string ShiftTypeName
        {
            get
            {
                return EnumUtility.GetDescriptionFromEnumValue((ShiftType)ShiftType);
            }
        }

        public string CUser { get; set; } 
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
namespace RMS.Models
{
    public enum ShiftType
    {
        [Description("Shift 1")] Shift1,
        [Description("Shift 2")] Shift2,
        [Description("Shift 3")] Shift3,
    }
}
