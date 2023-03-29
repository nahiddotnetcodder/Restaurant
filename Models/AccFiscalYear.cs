
namespace RMS.Models
{
    public class AccFiscalYear
    {
        [Key]
        public int AFYId { get; set; }
        public DateTime AFYBeginDate { get; set; }
        [NotMapped]
        public string AFYBeginDateString
        {
            get
            {
                return AFYBeginDate == null ? string.Empty : AFYBeginDate.ToString("MM/dd/yyyy");
            }
        }
        public DateTime AFYEndDate { get; set; }
        [NotMapped]
        public string AFYEndDateString
        {
            get
            {
                return AFYEndDate == null ? string.Empty : AFYEndDate.ToString("MM/dd/yyyy");
            }
        }
        public int AFYClosed { get; set; }
        [NotMapped]
        public bool CanClose
        {
            get
            {
                var currentDate = DateTime.Now;
                if (currentDate >= AFYBeginDate && currentDate <= AFYEndDate)
                    return false;
                return true;
            }
        }
        [NotMapped]
        public string AFYClosedName
        {
            get
            {
                return EnumUtility.GetDescriptionFromEnumValue((YesNo)AFYClosed);
            }
        }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}