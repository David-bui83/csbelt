using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace csbelt.Models
{
    public class Hobby
    {
        [Key]
        public int HobbyId {get;set;}
        public int UserId {get;set;}

        [Required(ErrorMessage="Hobby name is required")]
        [MinLength(2,ErrorMessage="Hobby name must be 2 characters or more.")]
        public string Name {get;set;}
        
        [Required(ErrorMessage="Hobby description is required")]
        [MinLength(2,ErrorMessage="Hobby description must be 2 characters or more.")]
        public string Description {get;set;}
        
        public User creator {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}

        public List<Association> UserJoined {get;set;}
    }
}