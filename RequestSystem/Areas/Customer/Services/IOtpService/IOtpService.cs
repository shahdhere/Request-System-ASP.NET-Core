namespace RequestSystem.Areas.Customer.Services
{
    public interface IOtpService
    {
        public string GenerateOtp();
        public void SaveOtpToSession(ISession session, string otp, string email);
        public (string? Otp, string? Email) GetOtpFromSession(ISession session);
        public void ClearOtpFromSession(ISession session);
        Task<bool> SendOtpAsync(string email, string otp);    // لإرسال OTP
        Task<bool> OtpInputAsync(ISession session, string email, string otp); // للتحقق من OTP
    }
}
