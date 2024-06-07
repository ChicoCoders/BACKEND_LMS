using LMS.DTOs;
using LMS.Hubs;
using static LMS.Hubs.MyHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMS.Helpers;
using LMS.EmailTemplates;
using LMS.Models;

namespace LMS.Repository
{
    // NotificationService implements the INotificationService interface
    public class NotificationService:INotificationService
    {
        private readonly DataContext _Context;// Reference to the database context
        private readonly JWTService _jwtService;// JWT authentication service
        private readonly IEmailService _emailService;// Email service

        // Constructor to initialize dependencies
        public NotificationService(DataContext Context,JWTService jwtservice, IEmailService emailService)
        {
            _Context = Context;
            _jwtService = jwtservice;
            _emailService = emailService;
        }

        // Method to add a new notice
        public async Task<bool> NewNotice(NewNoticeDto newnotice)
        {
            var notice = new Notification
            {
                Title=newnotice.Subject,
                ToUser=newnotice.UserName,
                Description=newnotice.Description,
                Date= DateOnly.FromDateTime(DateTime.Now),
                time= TimeOnly.FromDateTime(DateTime.Now)

            };
            _Context.Notifications.Add(notice); 
            await _Context.SaveChangesAsync();
            return true;
        }

        // Method to get notifications for a user
        public async Task<List<NewNoticeDto>> GetNotification(string username, HttpContext httpContext)
        {
            var notifications = new List<Notification>();
            if (username == "all")
            {
                notifications = _Context.Notifications.Where(e => e.Type != "reservation").ToList();
            }
            else
            {
                // Get the actual username from the JWT token
                username = _jwtService.GetUsername(httpContext);
                var user=await _Context.Users.FirstOrDefaultAsync(e=>e.UserName==username);
                notifications = _Context.Notifications.Where(e => 
                e.ToUser == username||
                e.ToUser=="all"||
                e.ToUser==user.UserType).ToList();
            }
            // Convert notifications to DTOs and return the list
            var notificationlist = new List<NewNoticeDto>();
            if (notifications != null)
            {
                foreach(var x in notifications)
                {
                    var y = new NewNoticeDto
                    {
                        Id = x.Id,
                        UserName=x.ToUser,
                        Subject = x.Title,
                        Date = x.Date,
                        Description = x.Description,

                    };
                    notificationlist.Add(y);
                }
            }
            return notificationlist;
        }

        // Method to set a reminder for a reservation
        public async Task<bool> SetRemind(Reservation reservation)
        {
            // Create a new notification for the reservation reminder
            var newNotification = new Notification
            {
                Title = "Reservation Reminder",
                Description = "The book " + reservation.ResourceId + " is overdue! Please return it by "+reservation.DueDate + " to avoid any fines.",
                ToUser = reservation.BorrowerID,
                Date = DateOnly.FromDateTime(DateTime.Now), 
                time = TimeOnly.FromDateTime(DateTime.Now),
                Type="remind"
            };
            // Get the borrower's email and send a reminder email
            var borrower = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == reservation.BorrowerID);
            var htmlBody = new EmailTemplate().RemindEmail(reservation);
            await _emailService.SendEmail(htmlBody, borrower.Email, "Your Reservation is Overdue" );

            // Add the notification and save changes
            await _Context.Notifications.AddAsync(newNotification);
            await _Context.SaveChangesAsync();
            return true;
        }

        // Method to issue a notification for a reservation
        public async Task<bool> IssueNotification(int reservationNo)
        {
            // Find the reservation in the database
            var reservation = await _Context.Reservations.FirstOrDefaultAsync(e => e.Id == reservationNo);
            if (reservation != null)
            {
                // Create a new notification for the reservation
                var notification = new Notification
                {
                    Title = "About Your ReservationNo : "+reservation.Id,
                    Description = "Reservation No : " + reservation.ReservationNo + ", User Id : " + reservation.BorrowerID + ", Date : " + reservation.IssuedDate + ", Due Date : " + reservation.DueDate,
                    ToUser = reservation.BorrowerID,
                    Date = reservation.IssuedDate,
                    time = new TimeOnly(22,11),
                    Type="reservation"
                    
                };

                // Create a new notice DTO and send a message using the messaging hub
                var notification2 =new NoticeDto
                {
                    UserName = reservation.BorrowerID,
                    Subject = "About Your ReservationNo : " + reservation.Id,
                    Description = "There is a reservation",
                    
                };
                
                  
                 var z = new MessagingHub(_Context);
                 z.SendMessage(notification2);
                // Add the notification and save changes
                _Context.Notifications.Add(notification);
                await _Context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
            
        }
        // Method to remove a notification by its ID
        
        public async Task<bool> ReturnNotification(int reservationNo)
        {
            var reservation = await _Context.Reservations.FirstOrDefaultAsync(e => e.Id == reservationNo);
            if (reservation != null)
            {

                
                var notification = new Notification
                {
                    Title = "Return Resource Successfully.ReservationNo : "+reservation.Id,
                    Description = "Reservation No : " + reservation.ReservationNo + ", User Id : " + reservation.BorrowerID + ", Return Date : " + reservation.ReturnDate + ", Due Date : " + reservation.DueDate,
                    ToUser = reservation.BorrowerID,
                    Date = reservation.IssuedDate
                    
                };
                _Context.Notifications.Add(notification);
                await _Context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> RemoveNotification(int id)
        {
            var notification = await _Context.Notifications.FirstOrDefaultAsync(e => e.Id == id);
            if (notification != null)
            {
                _Context.Notifications.Remove(notification);
                await _Context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
