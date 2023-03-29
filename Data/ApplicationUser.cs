namespace RMS.Data
{
    public class ApplicationUser:IdentityUser
    {
        [Column(TypeName ="nvarchar(50)")]
        public string UserLogin { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string FullName { get; set; }
        public string AccessLevelId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
