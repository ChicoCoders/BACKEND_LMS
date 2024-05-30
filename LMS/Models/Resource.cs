using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Resource
    {
        
        
        [Key]
        public string ISBN { get; set; }
        public string Title { get; set; }
        [ForeignKey(nameof(Author))]
        public string AuthorName {  get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
      //  public int year { get; set; }
        public int Borrowed { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int PageCount { get; set; }
        public DateTime AddedOn { get; set; }
        public string ImageURL { get; set; }
        public string AddedByID { get; set; }

        [ForeignKey(nameof(Location))]
        public string BookLocation { get; set; }
        public User AddedBy { get; set; }
        public Location Location { get; set; }
        public Author Author { get; set; }
        
    }
}
