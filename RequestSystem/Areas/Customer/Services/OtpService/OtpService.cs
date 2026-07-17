using System.Net;
using System.Net.Mail;

namespace RequestSystem.Areas.Customer.Services
{
    public class OtpService : IOtpService
    {
        public string GenerateOtp()
        {
            return new Random().Next(10000, 99999).ToString();
        }

        public void SaveOtpToSession(ISession session, string otp, string email)
        {
            session.SetString("OTP", otp);
            session.SetString("Email", email);
        }

        public (string? Otp, string? Email) GetOtpFromSession(ISession session)
        {
            var otp = session.GetString("OTP");
            var email = session.GetString("Email");
            return (otp, email);
        }

        public void ClearOtpFromSession(ISession session)
        {
            session.Remove("OTP");
            session.Remove("Email");
        }

        public async Task<bool> SendOtpAsync(string email, string otp)
        {
            try
            {
                var fromAddress = new MailAddress("za265545@gmail.com", "MOH");
                var toAddress = new MailAddress(email);
                const string fromPassword = "imaztatudrxggltb";
                const string subject = "رمز التحقق OTP";
                string body = $"رمز التحقق الخاص بك هو: {otp}";

                using (var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                })
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    await smtp.SendMailAsync(message);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> OtpInputAsync(ISession session, string email, string otp)
        {
            var (savedOtp, savedEmail) = GetOtpFromSession(session);
            bool isValid = savedOtp == otp && savedEmail == email;
            return Task.FromResult(isValid);
        }
    }
}
