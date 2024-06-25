
using FirebaseAdmin.Auth;
using LMS.DTOs;
using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository
{
    public class ReportService : IReportService
    {

        private readonly DataContext _context;

        public ReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> generateReport()
        {

            return true;
        }

        //public async Task<ResoursereportDTO> GetEventCountByDateRangeAsync(DateTime startDate, DateTime endDate)
        //{
        //var Resoursere = new ResoursereportDTO
        //{
        //Total = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate)).Count(),
        // Novels = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate) && e.Type == "Novels").Count(),
        //Journals = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate) && e.Type == "Journals").Count(),
        // Ebooks = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate) && e.Type == "Ebooks").Count(),
        // Rbooks = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate) && e.Type == "Referencesbooks").Count(),
        // Others = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate) && e.Type == "Others").Count(),
        // };
        //return Resoursere;
        //}

        public async Task<List<object[]>> GetEventCountByDateRangeAsync(DateTime startDate, DateTime endDate)

            {
                var bookTypeCounts = await _context.Resources
                    .Where(r => r.AddedOn >= startDate && r.AddedOn <= endDate)
                    .GroupBy(r => r.Type)
                    .Select(g => new object[]
                    {
                    g.Key,
                    g.Count()
                    })
                    .ToListAsync();

            bookTypeCounts.Insert(0, new object[] { "Book Type", "Quantity" });

            return bookTypeCounts;
            }



        public async Task<rereservation> GetReservationsCountByDateRangeAsync(DateOnly startDate1, DateOnly endDate1)
        {
            var Reservations = new rereservation
            {
                Total = _context.Reservations.Where(e => e.IssuedDate >= startDate1 && e.IssuedDate <= endDate1).Count(),
                Due = _context.Reservations.Where(e => (e.IssuedDate >= startDate1 && e.IssuedDate <= endDate1) && e.Status == "Due").Count(),
                Reserved = _context.Reservations.Where(e => (e.IssuedDate >= startDate1 && e.IssuedDate <= endDate1) && e.Status == "Reserved").Count(),
                Borrowed = _context.Reservations.Where(e => (e.IssuedDate >= startDate1 && e.IssuedDate <= endDate1) && e.Status == "Borrowed").Count(),

            };
            return Reservations;
        }

        public async Task<userreport> GetUserCountByDateRangeAsync(DateOnly startDate1, DateOnly endDate1)
        {
            var User = new userreport
            {
                Total = _context.Users.Where(e => e.AddedDate >= startDate1 && e.AddedDate <= endDate1).Count(),
                Admin = _context.Users.Where(e => (e.AddedDate >= startDate1 && e.AddedDate <= endDate1) && e.UserType == "admin").Count(),
                Petron = _context.Users.Where(e => (e.AddedDate >= startDate1 && e.AddedDate <= endDate1) && e.UserType == "petron").Count(),

            };
            return User;
        }
    }
}
