using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserToRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(10, MinimumLength=6, ErrorMessage="Sorry! Invalid Password.")]
        public string  Password { get; set; }
    }
}