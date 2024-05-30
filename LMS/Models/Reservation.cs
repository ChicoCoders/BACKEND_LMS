using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Reservation
    {
      
        public int Id { get; set; }
        
        public string ReservationNo { get; set; }
        
        public DateOnly IssuedDate { get; set; }
        
        public DateOnly DueDate {  get; set; }
        
        public DateOnly ReturnDate { get; set; }
        
        public string Status { get; set; }
        
        
        [ForeignKey(nameof(Resource))]
        public string ResourceId {  get; set; }
        
        [ForeignKey(nameof(IssuedBy))]
        public string IssuedByID { get; set; }

       
        public string BorrowerID { get; set; }



        public User Borrower { get; set; }
        public User IssuedBy { get; set; }
        public Resource Resource { get; set; }
        
        


    }
}
