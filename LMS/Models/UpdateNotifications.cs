namespace LMS.Models
{
    public class UpdateNotifications
    {
        public int id {  get; set; }
        public string Subject { get; set; }
        public bool books {  get; set; }
        public bool ebooks {  get; set; }
        public bool journals {  get; set; }
        public bool others {  get; set; }
    }
}
