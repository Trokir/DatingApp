using System;
using System.ComponentModel.DataAnnotations;
namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Your  must specify password between 4-8 chars")]
        public string Password { get; set; }
         [Required]
        public string Gender { get; set; }
         [Required]
        public string KnownAs { get; set; }
         [Required]
        public DateTime DateOfBirth { get; set; }
         [Required]
        public string City { get; set; }
         [Required]
        public string Country { get; set; }
         public DateTime Created { get; set; }
        public DateTime Lastactive { get; set; }
        public UserForRegisterDto()
        {
           Created  =DateTime.Now; 
           Lastactive =DateTime.Now; 
        }
    }
}