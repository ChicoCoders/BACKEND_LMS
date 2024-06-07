using MimeKit;

namespace LMS.Repository
{
    public interface IEmailService
    {
        Task<string> SendEmail(TextPart text, String To, String Subject);
    }
}
