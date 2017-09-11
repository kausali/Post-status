using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 
namespace wall.Models
{
    public class Message: BaseEntity
    {
        public int messageID { get; set; }

        
        [Required]
        public int UserID { get; set; }
        [Required]
        [MinLength(3)]
        public string message { get; set; }
        public int like { get; set; }
        public int dislike { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User User {get;set;}
        
        public List<Comment> Comments { get; set; }

        public Message()
        {
            Comments = new List<Comment>();
        }
    }
    
}