using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace RequestSystem.Areas.Customer.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendOtp(string toEmail, string otp)
        {
            var emailSettings = _config.GetSection("EmailSettings");
            string senderEmail = emailSettings["SenderEmail"];
            string senderName = emailSettings["SenderName"];
            string password = emailSettings["Password"];
            string host = emailSettings["Host"];
            int port = int.Parse(emailSettings["Port"]);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail, senderName);
            mail.To.Add(toEmail);
            mail.Subject = "رمز التحقق";
            mail.Body = $"رمز التحقق الخاص بك هو: {otp}";

            SmtpClient smtp = new SmtpClient(host, port);
            smtp.Credentials = new NetworkCredential(senderEmail, password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}