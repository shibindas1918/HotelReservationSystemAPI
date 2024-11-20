namespace HotelReservationSystemAPI.Models
{
    public class Invoices
    {
        public int InvoiceId { get; set; }
        public int BookingId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public Decimal TotalAmount { get; set; }
        public Decimal DueAmount { get; set; }
    }
}
