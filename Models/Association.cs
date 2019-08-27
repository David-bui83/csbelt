using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csbelt.Models
{
    public class Association
    {
        [Key]
        public int AssociationId {get;set;}
        public int UserId {get;set;}
        public int HobbyId {get;set;}
        
        [Required(ErrorMessage="Proficiency level is required")]
        public string Proficiency {get;set;}

        public User User {get;set;}
        public Hobby Hobby {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}