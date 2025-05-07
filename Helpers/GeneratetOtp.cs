namespace EcoPowerHub.Helpers
{
    public static class GeneratetOtp
    {
        public static string GenerateOTP()
        {
            var randomNum = new Random();
            return randomNum.Next(100000,999999).ToString();
        }
            
    }
}
