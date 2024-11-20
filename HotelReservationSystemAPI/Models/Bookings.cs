namespace HotelReservationSystemAPI.Models
{
    public class Bookings
    {
        public int BookingID { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; }
    }
}
