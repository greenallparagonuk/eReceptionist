using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace eReceptionist.Services
{
    public class SendGridMessage : ISendGridMessage
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("eReception@example.com", "eReception");
            var to = new EmailAddress(email, email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);                                    
        }
    }
}