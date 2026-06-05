using System.Net;
using System.Net.Mail;

namespace Clean_Pakistan.API.Services
{
    public class EmailService
    {
        public static void SendOtpEmail(string targetEmail, string otp)
        {
            // Replace with your actual development Gmail address
            var senderEmail = "cleanpakistantest@gmail.com";

            // You MUST use an "App Password" from your Google Account settings, NOT your real password!
            var appPassword = "abcd efgh ijkl mnop";

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
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

            client.Send(mailMessage);
        }
    }
} 