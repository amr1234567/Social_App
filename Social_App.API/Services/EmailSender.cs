using System;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Social_App.API.Interfaces;
using Social_App.API.Models.Helpers;
namespace Social_App.API.Services
{
    public class EmailSender(IOptions<EmailDetails> emailInfo, ILogger<EmailSender> logger) : IEmailSender
    {
        private readonly EmailDetails _emailDetails = emailInfo.Value;

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailToSend = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_emailDetails.Email),
                Subject = subject,
            };
            emailToSend.To.Add(MailboxAddress.Parse(email));
            var body = new BodyBuilder
            {
                HtmlBody = message
            };
            emailToSend.Body = body.ToMessageBody();
            emailToSend.From.Add(new MailboxAddress(_emailDetails.DisplayName, _emailDetails.Email));

            using var smtp = new SmtpClient();

            // Connect asynchronously to Office365 SMTP server
            await smtp.ConnectAsync("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

            // Authenticate asynchronously
            await smtp.AuthenticateAsync(_emailDetails.Email, _emailDetails.Password);

            // Send the email asynchronously
            await smtp.SendAsync(emailToSend);

            // Disconnect after sending
            await smtp.DisconnectAsync(true);
        }
    }
}
