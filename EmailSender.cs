using Microsoft.AspNetCore.Identity.UI.Services;

namespace BooksSpring2024_sec02
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
           //throw new NotImplementedException();
           //this is a file to take care of the error for now, not implementing an emailsender

            return Task.CompletedTask;
        }
    }
}
