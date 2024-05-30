using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class RemindNotification
    {
       [Key]
       public int id { get; set; }
       public string Content { get; set; }
       public string Subject { get; set; }
    }
}
