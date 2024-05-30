using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Cupboard
    {
        [Key]
        public int cupboardID { get; set; }
        public string name { get; set; }
    }
}
