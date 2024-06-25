using LMS.DTOs;
using LMS.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }



       [HttpPost("getDashboradData")]
        public async Task<IActionResult> getDashboradData()
        {
            var httpContext = HttpContext;
            return await _dashboardService.getDashboradData(httpContext);
        }

        [HttpPost("getOverDueList")]
        public async Task<IActionResult> getOverdueList()
        {
            var httpContext = HttpContext;
            return await _dashboardService.getOverdueList(httpContext);
        }

        [HttpPost("getLastWeekReservations")]
        public async Task<List<LastWeekReservations>> getLastWeekReservations()
        {
            return await _dashboardService.getLastWeekReservations();
        }
        [HttpPost("getLastWeekUsers")]
        public async Task<List<LastWeekReservations>> getLastWeekUsers()
        {
            return await _dashboardService.getLastWeekUsers();
        }

        [HttpPost("getLastWeekResourses")]
        public async Task<List<LastWeekReservations>> getLastWeekResourses()
        {
            return await _dashboardService.getLastWeekResourses();
        }
        [HttpPost("getAnouncement")]
        public async Task<IActionResult> getAnouncement()
        {
            var httpContext = HttpContext;
            return await _dashboardService.getAnouncement(httpContext);

        }


    }
}
