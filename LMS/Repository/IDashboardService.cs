using LMS.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Repository
{
    public interface IDashboardService
    {
        Task<IActionResult> getDashboradData(HttpContext httpContext);
        Task<IActionResult> getOverdueList(HttpContext httpContext);
        Task<IActionResult> getAnouncement(HttpContext httpContext);
        Task<List<LastWeekReservations>> getLastWeekReservations();
        Task<List<LastWeekReservations>> getLastWeekUsers();
        Task<List<LastWeekReservations>> getLastWeekResourses();
    }
}
