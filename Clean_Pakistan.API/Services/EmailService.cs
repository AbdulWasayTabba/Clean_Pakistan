using System.Net;
using System.Net.Mail;

namespace Clean_Pakistan.API.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendOtpEmail(string targetEmail, string otp)
        {
            // Replace with your actual development Gmail address
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var appPassword = _config["EmailSettings:AppPassword"];

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, appPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "Clean Pakistan Admin"),
                Subject = "Your Verification Code",
                Body = $"<h2>Welcome to Clean Pakistan!</h2><p>Your official verification code is: <strong>{otp}</strong></p><p>This code will expire in 5 minutes.</p>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(targetEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
} 