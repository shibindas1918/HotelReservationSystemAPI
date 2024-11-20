using HotelReservationSystemAPI.Data;
using HotelReservationSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ApiOperations _api;
        public HotelController(ApiOperations api)
        {
            _api = api;
            
        }
        [HttpGet("/booking/all")]   
        public  ActionResult<List<Rooms>> GetAvailableRooms (DateTime checkin , DateTime checkout)
        {
            var rooms= _api.SearchAvailableRooms(checkin, checkout);
            return Ok(rooms);
        }
        [HttpPost]
        public ActionResult BookRooms(Bookings bookings)
        {
            _api.MakeBooking(bookings);
            return Ok();
        }
        [HttpPost("PAYMENT")]  
        public ActionResult ProcessPayment(Payments payment)
        {
            _api.ProcessPayment(payment);   
            return Ok();
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        [HttpGet("GET INVOICES")]
        public ActionResult GetInvoices(int id)
        {
            _api.GenerateInvoice(id);
            return Ok();
        }
    }
}
