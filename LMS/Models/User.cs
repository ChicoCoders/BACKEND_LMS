using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LMS.Models
{
    public class User
    {

        [Key]
        public string UserName {  get; set; }
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateOnly DOB {  get; set; }
        [Required]
        public string Address {  get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NIC {  get; set; }
        public string Status {  get; set; }

        [Required]
        public string UserType {  get; set; }
        [Required]
        public string AddedById { get; set; }
        [Required]
        public User AddedBy { get; set; }

        
        public List<Reservation> Reservations { get; set; }

        public List<RequestResource> requests { get; set; }
        

    }

    

   
}


