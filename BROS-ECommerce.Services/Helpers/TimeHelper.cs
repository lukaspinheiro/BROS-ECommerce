namespace BROS_ECommerce.Services.Helpers
{
    public static class TimeHelper
    {
        public static DateTime AgoraPortoVelho()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
        }
    }

}
