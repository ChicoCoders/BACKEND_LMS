using FirebaseAdmin.Auth;
using LMS.DTOs;
using LMS.Models;
using LMS.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }



        [HttpPost]
        [Route("generateReport")]
        public async Task<bool> generateReport()
        {
            return await _reportService.generateReport();

        }

      //  [HttpPost("resoursecount")]
     //   public async Task<ResoursereportDTO> GetEventCountByDateRange([FromBody] Reports data)
     //   {
       //     return await _reportService.GetEventCountByDateRangeAsync(data.StartDate, data.EndDate);

     //   }

        [HttpPost("reservationscount")]
        public async Task<rereservation> GetReservationsCountByDateRange([FromBody] Reservationreport data)
        {
            return await _reportService.GetReservationsCountByDateRangeAsync(data.StartDate1, data.EndDate1);
        }

        [HttpPost("usercount")]
        public async Task<userreport> GetUserCountByDateRange([FromBody] Reservationreport data)
        {
            return await _reportService.GetUserCountByDateRangeAsync(data.StartDate1, data.EndDate1);

        }
        [HttpPost("resoursecount")]
        public async Task<List<object[]>> GetEventCountByDateRangeAsync(Reports reports)
        {
            return await _reportService.GetEventCountByDateRangeAsync(reports.StartDate,reports.EndDate);
        }


    }
}
