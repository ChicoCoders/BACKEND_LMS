using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Author
    {
        [Key]
        public string AuthorName {  get; set; }
        public List<Resource> Resources { get; set; }    
    }
}
