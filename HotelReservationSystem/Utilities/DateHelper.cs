namespace HotelReservationSystem.Utilities
{
    public static class DateHelper
    {
        public static (DateTime start, DateTime end) ParseDateRange(string dateRange)
        {
            string[] dates = dateRange.Split('-');
            DateTime startDate = DateTime.ParseExact(dates[0], "yyyyMMdd", null);
            DateTime endDate = dates.Length > 1 ? DateTime.ParseExact(dates[1], "yyyyMMdd", null) : startDate;
            return (startDate, endDate);
        }

        public static bool DateOverlap(string arrival, string departure, DateTime checkStart, DateTime checkEnd)
        {
            DateTime arrivalDate = DateTime.ParseExact(arrival, "yyyyMMdd", null);
            DateTime departureDate = DateTime.ParseExact(departure, "yyyyMMdd", null);
            return checkStart <= departureDate && checkEnd >= arrivalDate;
        }
    }
}
