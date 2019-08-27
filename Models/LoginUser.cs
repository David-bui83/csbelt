using System.ComponentModel.DataAnnotations;
namespace csbelt.Models
{
    public class LoginUser
    {
        [Required]
        public string LoginUserName {get;set;}
        [Required]
        public string LoginPassword {get;set;}
    }
}