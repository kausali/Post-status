using Microsoft.EntityFrameworkCore;
 
namespace wall.Models
{
    public class UserContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    public DbSet<User> users { get; set; }
    public DbSet<Message> messages { get; set; }
    public DbSet<Comment> comments { get; set; }
    public DbSet<Image> images { get; set; }

    //User, Message and Comment refers to our User, Message and Comment class created inside the Models
    //bdset allowss too communicate with our db and allows to get and set stuffs
    // you will need as many dbsets as many tables you have in db
    
    }
}