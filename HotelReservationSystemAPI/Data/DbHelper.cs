using HotelReservationSystemAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace HotelReservationSystemAPI.Data
{
    public class DbHelper
    {
        private readonly string _configuration;
        public DbHelper(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnectionString");

        }
        public DataSet ExecuteQuery(string query, SqlParameter[] sqlparameters)
        {
            using (SqlConnection conn = new SqlConnection(_configuration))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(query, conn);
                if (sqlparameters != null)
                    cmd.Parameters.AddRange(sqlparameters);
                adapter.SelectCommand = cmd;

                DataSet ds = new DataSet();
                conn.Open();
                adapter.Fill(ds);
                return ds;

            }

        }
        

        public List<Customers> GetCustomers()
        {
            List<Customers> customersList = new List<Customers>();
            using (SqlConnection conn = new SqlConnection(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select *from Customers", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Customers customers = new Customers
                    {
                        CustomerID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        PhoneNumber = reader.GetString(3),

                    };
                    customersList.Add(customers);

                }
                return customersList;
            }
        }
      
       

    }

}