using LMS.DTOs;

namespace LMS.Repository
{
    public interface INotificationService
    {
        Task<bool> NewNotice(NewNoticeDto newnotice);
        Task<List<NewNoticeDto>> GetNotification(string username,HttpContext httpContext);
        Task<bool> SetRemind(Reservation reservation);
        Task<bool> IssueNotification(int reservationNo);
        Task<bool> ReturnNotification(int reservationNo);
        Task<bool> RemoveNotification(int reservationNo);
    }
}
