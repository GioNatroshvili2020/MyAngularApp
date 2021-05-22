using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(16, MinimumLength=6, ErrorMessage="specify password between 6-16 character")]        
        public string Password { get; set; }
        
    }
}