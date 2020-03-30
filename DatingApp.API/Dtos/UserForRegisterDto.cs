using System.ComponentModel.DataAnnotations;
namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
       public string UserName { get; set; } 
        [Required]
        [StringLength(8,MinimumLength =4,ErrorMessage ="Your  must specify password between 4-8 chars")]
        public string Password { get; set; } 
    }
}