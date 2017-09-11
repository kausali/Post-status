using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace wall.Models
{
    public class Image: BaseEntity
    {
        public int imageID { get; set; }
        
        [Required]
        public byte image { get; set; }

        public int like { get; set; }
        public int dislike { get; set; }

        public string comment { get; set; }

        [Required]
        public int UserID { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User User {get;set;}
        
    }
    
}