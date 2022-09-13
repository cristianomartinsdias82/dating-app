namespace DatingApp.Api.Extensions
{
    public static class DateTimeExtensions
    {
        public static int HowManyYearsItsBeen(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
                return 0;

            var today = DateTime.Now;
            var age = today.Year - dateTime.Year;

            if (today.Month * 100 + today.Day < dateTime.Month * 100 + dateTime.Day)
                --age;

            return age;
        }
    }
}