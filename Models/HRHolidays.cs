using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class HRHolidays
    {
        [Key]
        public int HRHId { get; set; }
        public HolidayType HolidayType { get; set; }

        [NotMapped]
        public string HolidayTypeName
        {
            get
            {
                return EnumUtility.GetDescriptionFromEnumValue((HolidayType)HolidayType);
            }
        }

        public DateTime HRHStartDate { get; set; }
        [NotMapped]
        public string HRHStartDateString 
        { 
            get
            {
                return HRHStartDate == null ? string.Empty : HRHStartDate.ToString("MM/dd/yyyy");
            }
        }

        public DateTime HRHEndDate { get; set; }
        [NotMapped]
        public string HRHEndDateString
        {
            get
            {
                return HRHEndDate == null ? string.Empty : HRHEndDate.ToString("MM/dd/yyyy");
            }
        }

        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}

namespace RMS.Models
{
    public enum HolidayType
    {
        [Description("Weekend")] Weekend,
        [Description("Govt. Holiday")] GovtHoliday,
        [Description("Special Holiday")] SpecialHolida,
        [Description("Other Holiday")] OtherHoliday,
    }
}