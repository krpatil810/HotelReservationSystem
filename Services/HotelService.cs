using HotelReservationSystem.Utilities;
using Newtonsoft.Json;

class HotelService
{
    private List<Hotel> hotels;
    private List<Booking> bookings;

    
    #region HotelService Constructor
    public HotelService()
    {
        try
        {
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string hotelFilePath = Path.Combine(projectRoot, "Data", "hotels.json");
            string bookingFilePath = Path.Combine(projectRoot, "Data", "bookings.json");

            if (!File.Exists(hotelFilePath) || !File.Exists(bookingFilePath))
            {
                Console.WriteLine("Error: JSON files not found in 'Data/' directory.");
                return;
            }

            hotels = JsonConvert.DeserializeObject<List<Hotel>>(File.ReadAllText(hotelFilePath)) ?? new List<Hotel>();
            bookings = JsonConvert.DeserializeObject<List<Booking>>(File.ReadAllText(bookingFilePath)) ?? new List<Booking>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading JSON files: {ex.Message}");
        }
    }
    #endregion

    #region Check Availability of Rooms of Specified Type on given dates.
    public int CheckAvailability(string hotelId, string dateRange, string roomType)
    {
        int availableRooms = 0;
        try
        {
            var hotel = hotels.FirstOrDefault(h => h.Id == hotelId);
            if (hotel == null)
            {
                Console.WriteLine("\nHotel not found.");
                return availableRooms;
            }

            // Get all rooms of the specified type in the selected hotel
            var roomsOfType = hotel.Rooms.Where(r => r.RoomType == roomType).Select(r => r.RoomId).ToList();
            if (!roomsOfType.Any())
            {
                Console.WriteLine("\nNo rooms of this type available in this hotel.");
                return availableRooms;
            }

            // Parse the date range (single date or start-end range) to datahelper class to return formatted date yyyyMMddd
            var (startDate, endDate) = DateHelper.ParseDateRange(dateRange);

            // Count the number of rooms booked within the given date range
            int bookedRooms = bookings.Count(b =>
                b.HotelId == hotelId &&
                b.RoomType == roomType &&
                DateHelper.DateOverlap(b.Arrival, b.Departure, startDate, endDate));

            // Calculate available rooms
            availableRooms = roomsOfType.Count - bookedRooms;

            // Display message for overbooking case
            if (availableRooms < 0)
            {
                Console.WriteLine($"\nOverbooked {roomType} rooms: {availableRooms} (Hotel has more bookings than available rooms!)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nAn error occurred while checking availability: {ex.Message}");
        }

        return availableRooms;
    }
    #endregion

    #region Find All Future Date Ranges Where a Specific Room Type is Available
    public string SearchAvailability(string hotelId, int days, string roomType)
    {
        List<string> availabilityRanges = new List<string>();
        try
        {
            var hotel = hotels.FirstOrDefault(h => h.Id == hotelId);
            if (hotel == null)
            {
                Console.WriteLine("\nHotel not found.");
                return "";
            }

            // Get all rooms of the specified type in the selected hotel
            var roomsOfType = hotel.Rooms.Where(r => r.RoomType == roomType).Select(r => r.RoomId).ToList();
            if (!roomsOfType.Any())
            {
                Console.WriteLine("\nNo rooms of this type available in this hotel.");
                return "";
            }

            DateTime today = DateTime.Today;
            DateTime? rangeStart = null;
            int availableRooms = 0;
            int previousAvailability = 0;

            // Loop through the next 'days' days
            for (int i = 0; i < days; i++)
            {
                DateTime date = today.AddDays(i);
                int bookedRooms = bookings.Count(b =>
                    b.HotelId == hotelId &&
                    b.RoomType == roomType &&
                    DateHelper.DateOverlap(b.Arrival, b.Departure, date, date));

                int currentAvailability = roomsOfType.Count - bookedRooms;

                if (currentAvailability > 0)
                {
                    if (rangeStart == null)
                    {
                        // Start a new availability range
                        rangeStart = date;
                        availableRooms = currentAvailability;
                    }
                    else
                    {
                        // If availability changes, store the previous range
                        if (currentAvailability != previousAvailability)
                        {
                            availabilityRanges.Add($"({rangeStart:yyyyMMdd}-{date.AddDays(-1):yyyyMMdd}, {previousAvailability})");
                            rangeStart = date;
                            availableRooms = currentAvailability;
                        }
                    }
                }
                else if (rangeStart != null)
                {
                    // End the previous range and reset
                    availabilityRanges.Add($"({rangeStart:yyyyMMdd}-{date.AddDays(-1):yyyyMMdd}, {availableRooms})");
                    rangeStart = null;
                }

                previousAvailability = currentAvailability;
            }

            // Handle last range
            if (rangeStart != null)
            {
                availabilityRanges.Add($"({rangeStart:yyyyMMdd}-{today.AddDays(days - 1):yyyyMMdd}, {availableRooms})");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nAn error occurred while searching availability: {ex.Message}");
        }

        return availabilityRanges.Any() ? string.Join(", ", availabilityRanges) : "";
    } 
    #endregion
}
