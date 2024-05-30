using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LMS.Data
{  
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }

        
        public DbSet<User> Users { get; set; }
      
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<RequestResource> Requests { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Author> Author { get; set; }
       public DbSet<RemindNotification> RemindNotifications { get; set; }

        public DbSet<UpdateNotifications> UpdateNotifications { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.;Database=LMS;Integrated Security=True;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Reservation>()
             .HasOne(a => a.IssuedBy)
             .WithMany()
             .HasForeignKey(a => a.IssuedByID);
        }



    }

}
