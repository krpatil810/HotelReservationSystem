class CommandHandler
{
    private readonly HotelService _hotelService;

    public CommandHandler(HotelService hotelService)
    {
        _hotelService = hotelService;
    }

    public void ProcessCommand(string input)
    {
        if (input.StartsWith("Availability", StringComparison.OrdinalIgnoreCase))
        {
            ProcessAvailability(input);
        }
        else if (input.StartsWith("Search", StringComparison.OrdinalIgnoreCase))
        {
            ProcessSearch(input);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("\n❌ Invalid command. Use 'Availability' or 'Search'.");
        }
    }

    private void ProcessAvailability(string input)
    {
        try
        {
            var parameters = ExtractParameters(input);
            if (parameters.Length < 3)
            {
                Console.WriteLine("\n❌ Invalid Availability command format. Example: Availability(H1, 20240901, SGL)");
                return;
            }

            string hotelId = parameters[0].Trim();
            string dateRange = parameters[1].Trim();
            string roomType = parameters[2].Trim();

            int availability = _hotelService.CheckAvailability(hotelId, dateRange, roomType);
            Console.WriteLine($"\n✅ Availability of {roomType} rooms in Hotel {hotelId} from {dateRange}: {availability} rooms available.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error processing availability: {ex.Message}");
        }
    }

    private void ProcessSearch(string input)
    {
        try
        {
            var parameters = ExtractParameters(input);
            if (parameters.Length < 3)
            {
                Console.WriteLine("❌ Invalid Search command format. Example: Search(H1, 365, SGL)");
                return;
            }

            string hotelId = parameters[0].Trim();
            int days = int.Parse(parameters[1].Trim());
            string roomType = parameters[2].Trim();

            string result = _hotelService.SearchAvailability(hotelId, days, roomType);
            Console.WriteLine($"\n✅ Available dates for {roomType} in {hotelId}: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error processing search: {ex.Message}");
        }
    }

    private string[] ExtractParameters(string input)
    {
        int startIndex = input.IndexOf('(');
        int endIndex = input.IndexOf(')');
        if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
        {
            throw new ArgumentException("\n❌ Invalid command format.");
        }

        string paramString = input.Substring(startIndex + 1, endIndex - startIndex - 1);
        return paramString.Split(',');
    }
}
