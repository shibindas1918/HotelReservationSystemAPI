using HotelReservationSystemAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net.Quic;

namespace HotelReservationSystemAPI.Data
{
    public class ApiOperations
    {
        private readonly DbHelper _dbHelper;
        public ApiOperations(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;

        }

        public List<Rooms> SearchAvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            string query = @"SELECT * FROM Rooms WHERE AvailabilityStatus = 1 
                            AND RoomId NOT IN 
                           (SELECT RoomId FROM Bookings WHERE CheckInDate <= @CheckOut AND CheckOutDate >= @CheckIn)";

            SqlParameter[] sqlParams = {
        new SqlParameter("@CheckIn", checkIn),
        new SqlParameter("@CheckOut", checkOut)
    };

            DataSet ds = _dbHelper.ExecuteQuery(query, sqlParams);
            List<Rooms> rooms = new List<Rooms>();
            List<Bookings> bookings = new List<Bookings>(); 

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                bookings.Add(new Bookings
                {
                    BookingID = Convert.ToInt32(row["BookingId"]),
                    CustomerId= Convert.ToInt32(row["customerid"]),
                    RoomId = Convert.ToInt32(row["RoomId"]),
                    BookingDate= Convert.ToDateTime(row["BookingDate"]),
                    CheckInDate = Convert.ToDateTime(row["CheckInDate"]),
                    CheckOutDate = Convert.ToDateTime(row["CheckOutDate"]),
                    Status = Convert.ToString(row["Status"]) 

                });
              
                rooms.Add(new Rooms
                {
                    RoomId = Convert.ToInt32(row["RoomId"]),
                    RoomType = row["RoomType"].ToString(),
                    PricePerNight = Convert.ToDecimal(row["PricePerNight"]),
                    AvailabilityStatus = Convert.ToBoolean(row["AvailabilityStatus"])
                });
            }
            
            return rooms;
        }
        public List<Rooms> ConfirmAvailableRoom(Rooms rooms)
        {
            string confirmQuery = @"";
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@confim", rooms),
            };
            DataSet rs = _dbHelper.ExecuteQuery(confirmQuery, sqlParameters);
            List<Rooms> room = new List<Rooms>();
            List<Bookings> bookings = new List<Bookings>();
            foreach (DataRow row in rs.Tables[0].Rows)
            {
                bookings.Add(new Bookings
                {
                    BookingID = Convert.ToInt32(row["BookingId"]),
                    CustomerId = Convert.ToInt32(row["customerid"]),
                    RoomId = Convert.ToInt32(row["RoomId"]),
                    BookingDate = Convert.ToDateTime(row["BookingDate"]),
                    CheckInDate = Convert.ToDateTime(row["CheckInDate"]),
                    CheckOutDate = Convert.ToDateTime(row["CheckOutDate"]),
                    Status = Convert.ToString(row["Status"])

                });
                room.Add(new Rooms
                {
                    RoomId = Convert.ToInt32(row["RoomId"]),
                    RoomType = row["RoomType"].ToString(),
                    PricePerNight = Convert.ToDecimal(row["PricePerNight"]),
                    AvailabilityStatus = Convert.ToBoolean(row["AvailabilityStatus"])

                });
            }
            return room;    
        }
        


                public void MakeBooking(Bookings booking)
        {
            string query = @"INSERT INTO Bookings (CustomerId, RoomId, BookingDate, CheckInDate, CheckOutDate, Status)
                     VALUES (@CustomerId, @RoomId, @BookingDate, @CheckInDate, @CheckOutDate, @Status)";

            SqlParameter[] sqlParams = {
        new SqlParameter("@CustomerId", booking.CustomerId),
        new SqlParameter("@RoomId", booking.RoomId),
        new SqlParameter("@BookingDate", booking.BookingDate),
        new SqlParameter("@CheckInDate", booking.CheckInDate),
        new SqlParameter("@CheckOutDate", booking.CheckOutDate),
        new SqlParameter("@Status", booking.Status)
    };

            _dbHelper.ExecuteQuery(query, sqlParams);
        }

        public void CreateBooking(Bookings booking)
        {
            string query = @"Insert into bookings (customerid,roomid,bookingdate,checkdate,checkoutdate,status) values(@customerid,@roomid,@bookingdate,@checkdate,@checkoutdate,@status";
            SqlParameter[] sqlParams = {
                new SqlParameter("@customerid", booking.CustomerId),
                new SqlParameter("@roomid", booking.RoomId),
                new SqlParameter("@bookingdate", booking.BookingDate),
                new SqlParameter("@checkdate", booking.CheckInDate),
                new SqlParameter("@checkoutdate", booking.CheckOutDate),
                new SqlParameter("@status", booking.Status)

            };
            _dbHelper.ExecuteQuery(query, sqlParams);
        }
         public void CreateScreen()
        {
            string InputQuery = @"Insert into InsertQuery(customerid,roomid,customerid, checkdate,checkoutdate,status) 
                                  values(@customerid,@roomid,@bookingdate,@checkdate,@checkoutdate,@status,
                                   update set insertquery(customerid=@customerid,roomid=@roomid,
                                    customerid=@customerid,checkoutdate=@checkoutdate,status=@status";

        }
        public void createHome(Bookings booking)
        {
            string query = @"select into bookings(customerid,roomid,bookingdate,checkdate,checkoutdate,status) 
                            values (@customerid,@roomid,@bookingdate,@checkdate,@checkoutdate,@status)";

                                
        }
        public void  createspace( Bookings bookings)
        {
            string query = @"Insert into booking (customerid,roomid,bookingdate,checkdate,checkoutdate,status) values(@customerid,@roomid,@bookingdate,@checkdate,@checkoutdate,@status)";
        }
        public void ProcessPayment(Payments payment)
        {
            string query = @"INSERT INTO Payments (BookingId, PaymentDate, Amount, PaymentStatus)
                     VALUES (@BookingId, @PaymentDate, @Amount, @PaymentStatus)";

            SqlParameter[] sqlParams = {
        new SqlParameter("@BookingId", payment.BookingId),
        new SqlParameter("@PaymentDate", payment.PaymentDate),
        new SqlParameter("@Amount", payment.Amount),
        new SqlParameter("@PaymentStatus", payment.PaymentStatus)
    };

            _dbHelper.ExecuteQuery(query, sqlParams);
        }

        public Invoices GenerateInvoice(int bookingId)
        {
            string query = @"SELECT b.BookingId, c.Name, r.RoomType, r.PricePerNight, 
                            DATEDIFF(day, b.CheckInDate, b.CheckOutDate) AS TotalDays,
                            (DATEDIFF(day, b.CheckInDate, b.CheckOutDate) * r.PricePerNight) AS TotalAmount
                     FROM Bookings b
                     INNER JOIN Customers c ON b.CustomerId = c.CustomerId
                     INNER JOIN Rooms r ON b.RoomId = r.RoomId
                     WHERE b.BookingId = @BookingId";

            SqlParameter[] sqlParams = { new SqlParameter("@BookingId", bookingId) };

            DataSet ds = _dbHelper.ExecuteQuery(query, sqlParams);
            DataRow row = ds.Tables[0].Rows[0];

            return new Invoices
            {
                InvoiceId = bookingId,
                BookingId = Convert.ToInt32(row["Name"]),
                InvoiceDate = Convert.ToDateTime(row["RoomType"]),
                TotalAmount = Convert.ToDecimal(row["TotalDays"]),
                DueAmount = Convert.ToDecimal(row["TotalAmount"])
            };
        }

    }
}
