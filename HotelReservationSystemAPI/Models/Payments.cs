namespace HotelReservationSystemAPI.Models
{
    public class Payments
    {
        public int PaymentID { get; set; }
        public int BookingId { get; set; }
        public DateTime PaymentDate { get; set; }
        public Decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
    }
}
