namespace HotelReservationSystemAPI.Models
{
    public class Rooms
    {
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public Decimal PricePerNight { get; set; }
        public bool AvailabilityStatus { get; set; }
    }
}
