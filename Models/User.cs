using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace csbelt.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required(ErrorMessage="First name field cannot be empty")]
        [MinLength(2,ErrorMessage="First name must be 2 characters or more.")]
        public string FirstName {get;set;}
        
        [Required(ErrorMessage="Last name field cannot be empty")]
        [MinLength(2,ErrorMessage="Last name must be 2 characters or more.")]
        public string LastName {get;set;}
        
        [Required(ErrorMessage="Username field cannot be empty")]
        [MinLength(3,ErrorMessage="Username must be between 3 and 15 characters")]
        [MaxLength(15,ErrorMessage="Username must be between 3 and 15 characters")]
        public string UserName {get;set;}

        [DataType(DataType.Password)]
        [Required(ErrorMessage="Password cannot be empty")]
        [MinLength(8,ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get;set;}

        public string Proficiency {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        
        [NotMapped]
        [Compare("Password",ErrorMessage="Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}

    
        public List<Association> JoinHobby {get;set;}
    }
    
}