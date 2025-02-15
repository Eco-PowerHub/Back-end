using EcoPowerHub.Models;
using EcoPowerHub.Repositories.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;

namespace EcoPowerHub.Repositories.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(_emailSettings.Email));
            mail.To.Add(MailboxAddress.Parse(toEmail));
            mail.Subject = subject;
            var builder = new BodyBuilder
            {
                HtmlBody = message
            };
            mail.Body = builder.ToMessageBody();
            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
            smtp.Send(mail);
            smtp.DisconnectAsync(true);
        }
    }
}
