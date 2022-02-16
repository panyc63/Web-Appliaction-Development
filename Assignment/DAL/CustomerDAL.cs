using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using Assignment.Models;
using Microsoft.AspNetCore.Razor.Language;

namespace Assignment.DAL
{
    public class CustomerDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CustomerDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("AirFlightConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public bool IsEmailExist(string email, int customerid)
        {
            bool emailFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CustomerID FROM Customer WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            //Open a database connection and excute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != customerid)
                        //The email address is used by another staff
                        emailFound = true;
                }
            }
            else
            { //No record
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return emailFound;
        }

        public List<Customer> GetAllCustomers()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Customer ORDER BY CustomerID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a customer list
            List<Customer> customerList = new List<Customer>();
            while (reader.Read())
            {
                customerList.Add(
                new Customer
                {
                    EmailAddr = reader.GetString(5), //6: 7th column
                    Password = reader.GetString(6), //9: 10th column
                }
                );
            }
            reader.Close();
            conn.Close();
            return customerList;
        }

        public int Add(Customer customer)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Customer (CustomerName, Nationality, BirthDate, TelNo,
                                EmailAddr, Password)
                                OUTPUT INSERTED.CustomerID
                                VALUES(@name, @nationality, @birthdate, @telno,
                                @email, @password)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", customer.CustomerName);
            cmd.Parameters.AddWithValue("@nationality", customer.Nationality);
            cmd.Parameters.AddWithValue("@birthdate", customer.BirthDate);
            cmd.Parameters.AddWithValue("@telno", customer.TelNo);
            cmd.Parameters.AddWithValue("@email", customer.EmailAddr);
            cmd.Parameters.AddWithValue("@password", "p@55Cust");
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            customer.CustomerID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs
            return customer.CustomerID;

        }

        public string GetCustomerName(string userid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT Customer.CustomerName FROM Customer WHERE EmailAddr = @userid";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@userid", userid);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            String CustomerName = "";
            while (reader.Read())
            {
                CustomerName = reader.GetString(0);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return CustomerName;
        }

        public int UpdateCustomerPassword(Customer c)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"UPDATE Customer SET Password = @newpassword
                                WHERE EmailAddr = @loginid";
            cmd.Parameters.AddWithValue("@newpassword", c.NewPassword);
            cmd.Parameters.AddWithValue("@loginid", c.EmailAddr);
            conn.Open();
            int count = (int)cmd.ExecuteNonQuery();
            conn.Close();
            return count;
        }

        public List<CustomerFS> SearchFlightSchedule(string origin, string destination)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT FlightSchedule.ScheduleID,FlightSchedule.FlightNumber,FlightSchedule.DepartureDateTime,FlightRoute.DepartureCountry,FlightRoute.DepartureCity,
                                FlightSchedule.ArrivalDateTime,FlightRoute.ArrivalCountry,FlightRoute.ArrivalCity,
                                FlightSchedule.EconomyClassPrice,FlightSchedule.BusinessClassPrice 
                                FROM FlightSchedule INNER JOIN FlightRoute 
                                ON FlightSchedule.RouteID = FlightRoute.RouteID
                                WHERE FlightRoute.DepartureCountry = @departurecountry
                                AND FlightRoute.ArrivalCountry = @arrivalcountry
                                AND FlightSchedule.Status = 'Opened'
                                AND DATEDIFF(DAY, GETDATE(), FlightSchedule.DepartureDateTime) > 1";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@departurecountry", origin);
            cmd.Parameters.AddWithValue("@arrivalcountry", destination);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<CustomerFS> availableschedules = new List<CustomerFS>();
            while (reader.Read())
            {
                availableschedules.Add(
                    new CustomerFS
                    {
                        ScheduleID = reader.GetInt32(0),
                        FlightNumber = reader.GetString(1),
                        DepartureDateTime = reader.GetDateTime(2),
                        DepartureCountry = reader.GetString(3),
                        DepartureCity = reader.GetString(4),
                        ArrivalDateTime = reader.GetDateTime(5),
                        ArrivalCountry = reader.GetString(6),
                        ArrivalCity = reader.GetString(7),
                        EconomyClassPrice = reader.GetDecimal(8),
                        BusinessClassPrice = reader.GetDecimal(9),
                    }
                    );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return availableschedules;
        }

        public List<Booking> GetSpecificBooking(string userid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT Booking.* FROM Booking INNER JOIN Customer 
                                on Booking.CustomerID = Customer.CustomerID
                                WHERE Customer.EmailAddr = @userid";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@userid", userid);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Booking> BookingList = new List<Booking>();
            while (reader.Read())
            {
                BookingList.Add(
                new Booking
                {
                    BookingID = reader.GetInt32(0),
                    CustomerID = reader.GetInt32(1),
                    ScheduleID = reader.GetInt32(2),
                    PassengerName = reader.GetString(3),
                    PassportNumber = reader.GetString(4),
                    Nationality = reader.GetString(5),
                    SeatClass = reader.GetString(6),
                    AmtPayable = reader.GetDecimal(7),
                    Remarks = !reader.IsDBNull(8) ?
                               reader.GetString(8) : null,
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return BookingList;
        }

        public Overall GetOverallDetails(int id)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT Booking.BookingID,Booking.CustomerID,
                                Booking.ScheduleID,Booking.PassengerName,Booking.PassportNumber,Booking.Nationality,
                                Booking.SeatClass,Booking.Remarks,Booking.DateTimeCreated,
                                FlightSchedule.DepartureDateTime,FlightRoute.DepartureCountry,FlightRoute.DepartureCity,
                                FlightSchedule.ArrivalDateTime,FlightRoute.ArrivalCountry,FlightRoute.ArrivalCity
                                FROM FlightSchedule INNER JOIN FlightRoute ON FlightRoute.RouteID = FlightSchedule.RouteID
                                INNER JOIN Booking ON Booking.ScheduleID = FlightSchedule.ScheduleID
                                WHERE BookingID = @bookingid";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@bookingid", id);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            Overall booking = new Overall() ;
            while (reader.Read())
            {
                booking.BookingID = reader.GetInt32(0);
                booking.CustomerID = reader.GetInt32(1);
                booking.ScheduleID = reader.GetInt32(2);
                booking.PassengerName = reader.GetString(3);
                booking.PassportNumber = reader.GetString(4);
                booking.Nationality = reader.GetString(5);
                booking.SeatClass = reader.GetString(6);
                booking.Remarks = !reader.IsDBNull(7) ?
                                   reader.GetString(7) : null;
                booking.DateTimeCreated = reader.GetDateTime(8);
                booking.DepartureDateTime = reader.GetDateTime(9);
                booking.DepartureCountry = reader.GetString(10);
                booking.DepartureCity = reader.GetString(11);
                booking.ArrivalDateTime = reader.GetDateTime(12);
                booking.ArrivalCountry = reader.GetString(13);
                booking.ArrivalCity = reader.GetString(14);
                
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return booking;
        }

        public decimal FindEconomyPrice(int scheduleid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT EconomyClassPrice FROM FlightSchedule WHERE ScheduleID = @scheduleid";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@scheduleid", scheduleid);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            decimal EconomyPrice = 0;
            while (reader.Read())
            {
                EconomyPrice =  reader.GetDecimal(0);
            }
            reader.Close();
            //Close the database connection
            conn.Close();
            return EconomyPrice;
        }

        public decimal FindBusinessPrice(int scheduleid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT BusinessClassPrice FROM FlightSchedule WHERE ScheduleID = @scheduleid";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@scheduleid", scheduleid);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            decimal BusinessPrice = 0;
            while (reader.Read())
            {
                BusinessPrice = reader.GetDecimal(0);
            }
            reader.Close();
            //Close the database connection
            conn.Close();
            return BusinessPrice;
        }

        public int FindCustomerID(string loginid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT CustomerID FROM Customer WHERE EmailAddr = @loginid";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@loginid", loginid);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            int CustomerID = 0;
            while (reader.Read())
            {
                CustomerID = reader.GetInt32(0);
            }
            reader.Close();
            //Close the database connection
            conn.Close();
            return CustomerID;
        }

        public int BookATicket(Booking booking)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Booking (CustomerID,ScheduleID,PassengerName,
                                PassportNumber,Nationality,SeatClass,AmtPayable,Remarks,DateTimeCreated)
                                OUTPUT INSERTED.BookingID
                                VALUES(@customerid, @scheduleid, @passengername,
                                @passportno, @nationality, @seatclass, @amtpayable, @remarks, @datetimecreated)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@customerid", booking.CustomerID);
            cmd.Parameters.AddWithValue("@scheduleid", booking.ScheduleID);
            cmd.Parameters.AddWithValue("@passengername", booking.PassengerName);
            cmd.Parameters.AddWithValue("@passportno", booking.PassportNumber);
            cmd.Parameters.AddWithValue("@nationality", booking.Nationality);
            cmd.Parameters.AddWithValue("@seatclass", booking.SeatClass);
            cmd.Parameters.AddWithValue("@amtpayable", booking.AmtPayable);
            cmd.Parameters.AddWithValue("@remarks", booking.Remarks);
            cmd.Parameters.AddWithValue("@datetimecreated", booking.DateTimeCreated);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            booking.BookingID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs
            return booking.BookingID;
        }
    }
}
