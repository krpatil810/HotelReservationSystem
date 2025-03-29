class Hotel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<Room> Rooms { get; set; }
}

class Room
{
    public string RoomType { get; set; }
    public string RoomId { get; set; }
}

class Booking
{
    public string HotelId { get; set; }
    public string Arrival { get; set; }
    public string Departure { get; set; }
    public string RoomType { get; set; }
}
