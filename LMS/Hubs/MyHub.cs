
using LMS.DTOs;
using Microsoft.AspNetCore.SignalR;
using MimeKit;

namespace LMS.Hubs
{
    public class MyHub : Hub
    {
        
        public class UserMessage
        {
            public required string Title { get; set; }
            public required string Sender { get; set; }
            public required string Content { get; set; }
            public DateTime SentTime { get; set; }
        }
        public class MessagingHub : Hub
           
        {
            private readonly DataContext _context;

            public MessagingHub(DataContext context)
            {
                _context = context;
            }

            private static readonly List<UserMessage> MessageHistory = new List<UserMessage>();

            public async Task JoinRoom(string roomName)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName!);
            }
            public async Task LeaveRoom(string roomName)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            }
         //   public async Task PostMessage(string content)
           // {
             //   var senderId = Context.ConnectionId;
               // var userMessage = new UserMessage
                //{
                 //   T
                  //  Sender = senderId,
                   // Content = content,
                    //SentTime = DateTime.UtcNow
                //};
                //MessageHistory.Add(userMessage);

                //await Clients.Others.SendAsync("ReceiveMessage", senderId, content, userMessage.SentTime);
           //}

        public async Task SendMessage(NoticeDto notice)
        {
                Console.WriteLine(notice.Subject);
            var senderId = Context.ConnectionId;
            var userMessage = new UserMessage
                {
                    Title = notice.Subject,
                    Sender = senderId,
                    Content = notice.Description,
                    SentTime = DateTime.UtcNow
            };
                // Create a new notification object
                var notification = new Notification
            {
                Title = notice.Subject,
                Description = notice.Description,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Notices",
                time= TimeOnly.FromDateTime(DateTime.Now),
                ToUser = notice.UserName
            };

              _context.Notifications.Add(notification);
               await _context.SaveChangesAsync();


            MessageHistory.Add(userMessage);
           
            await Clients.GroupExcept(notice.UserName,Context.ConnectionId).SendAsync("ReceiveMessage", senderId,userMessage.Title, userMessage.Content, userMessage.SentTime);
            
            }


            public async Task RetrieveMessageHistory()
            {
                await Clients.Caller.SendAsync("MessageHistory", MessageHistory);
            }
            }
    }
}
