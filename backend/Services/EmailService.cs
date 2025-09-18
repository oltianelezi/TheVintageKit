using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace backend.Services
{
    public class EmailService
    {
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService()
        {
            // Read the environment variables
            _smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
            _smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");

            if (string.IsNullOrEmpty(_smtpUser) || string.IsNullOrEmpty(_smtpPass))
                throw new Exception("SMTP_USER or SMTP_PASS environment variable is not set.");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("My Store", _smtpUser));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
