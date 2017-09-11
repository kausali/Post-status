using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace wall.Models
{
    public class UserImage: BaseEntity
    {
         public int imageID { get; set; }
        [Required]
        public Blob image { get; set; }

        public int like { get; set; }
        public int dislike { get; set; }

        [Required]
        public string comment { get; set; }

        [Required]
        public int UserID { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User User {get;set;}
        
        public List<User> myUsers { get; set; }

        public UserImage()
        {
            myUsers = new List<User>();
        }
    }
    
}