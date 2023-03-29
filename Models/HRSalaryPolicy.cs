using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    public class HRSalaryPolicy
    {
        [Key]
        public int HRSPId { get; set; }
        [StringLength(50)]
        public string HRSPName { get; set; }
        public ADDUC ADDUC { get; set; }
        [NotMapped]
        public string ADDUCName
        {
            get
            {
                return EnumUtility.GetDescriptionFromEnumValue((ADDUC)ADDUC);
            }
        }
        public PerNPer PerNPer { get; set; }
        [NotMapped]
        public string PerNPerName 
        {
            get
            {
                return EnumUtility.GetDescriptionFromEnumValue((PerNPer)PerNPer);
            }
        }
        public float? HRSPPercent { get; set; }

        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;
    }
}

namespace RMS.Models
{
    public enum ADDUC
    {
        Add, Deduct
    }
}

namespace RMS.Models
{
    public enum PerNPer
    {
        Percentage, NonPercentage
    }
}