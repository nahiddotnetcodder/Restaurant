namespace RMS.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserLogin { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        public string RoleName { get; set; }
        [Required]
        public string AccessLevelId { get; set; }
        public bool IsActive { get; set; }
    }
}
