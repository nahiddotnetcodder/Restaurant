using System.ComponentModel;

namespace RMS.Models
{
    public class HRWeekend
    {
        [Key]
        public int HRWId { get; set; }
        public Weekday Weekday { get; set; }
        [NotMapped]
        public string WeekdayName
        {
            get
            {
                return EnumUtility.GetDescriptionFromEnumValue((Weekday)Weekday);
            }
        }

        [StringLength(50)] 
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;
    }
}

namespace RMS.Models
{
    public enum Weekday
    {
        [Description("Friday")] Friday, 
        [Description("Saturday")] Saturday, 
        [Description("Sunday")] Sunday, 
        [Description("Monday")] Monday, 
        [Description("Tuesday")] Tuesday, 
        [Description("Wednesday")] Wednesday,
        [Description("Thursday")] Thursday
    }
}