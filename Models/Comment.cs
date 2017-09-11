using System;
using System.ComponentModel.DataAnnotations;
 
namespace wall.Models
{
    public class Comment: BaseEntity
    {
         public int commentID { get; set; }
        [Required(ErrorMessage="Comment Should be atleast 3 characters long!")]
        [MinLength(3)]
        public string comment { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int MessageID { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User User {get; set;}
        public Message Message {get; set;}
        
    }
    
}