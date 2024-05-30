using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly time { get; set; }
        public string Type {  get; set; }
        public string ToUser { get; set; }

    }
}
