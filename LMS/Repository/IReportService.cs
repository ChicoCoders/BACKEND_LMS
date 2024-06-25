
using FirebaseAdmin.Auth;
using LMS.DTOs;
using LMS.Models;

namespace LMS.Repository
{
    public interface IReportService
    {

        Task<bool> generateReport();
        //Task<ResoursereportDTO> GetEventCountByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<object[]>> GetEventCountByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<rereservation> GetReservationsCountByDateRangeAsync(DateOnly startDate1, DateOnly endDate1);
        Task<userreport> GetUserCountByDateRangeAsync(DateOnly startDate1, DateOnly endDate1);



    }
}
