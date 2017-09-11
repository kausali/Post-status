using System;
using System.Collections.Generic;
 
namespace wall.Models
{
    public class User: BaseEntity
    {
        public int userID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        //Since user can have many messages and comments we create a list of messages and comments.
        //List of type Message and Comment named Messages and Comments
        public List<Message> myMessages { get; set; }
        public List<Comment> myComments { get; set; }
        public List<UserImage> myImages { get; set; }

        public User()
        {
            myMessages = new List<Message>();
            myComments = new List<Comment>();
            myImages = new List<UserImage>();
        }
        
    }
    
}