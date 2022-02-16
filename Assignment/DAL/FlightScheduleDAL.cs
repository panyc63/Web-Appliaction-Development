using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using Assignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.DAL
{
    public class FlightScheduleDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        public FlightScheduleDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "AirFlightConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<FlightRoute> GetAllRoutes()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM dbo.FlightRoute ORDER BY RouteID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<FlightRoute> routeList = new List<FlightRoute>();
            while (reader.Read())
            {
                routeList.Add(
                new FlightRoute
                {
                    RouteID = reader.GetInt32(0),
                    DepartureCity = reader.GetString(1),
                    DepartureCountry = reader.GetString(2),
                    ArrivalCity = reader.GetString(3),
                    ArrivalCountry = reader.GetString(4),
                    FlightDuration = reader.GetInt32(5)

                }
                ) ; 
            };
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return routeList;
        }

        public FlightRoute getSpecificRoute(int id)
        {
            FlightRoute flightRoute = new FlightRoute();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM dbo.FlightRoute WHERE RouteID = @route";
            //Open a database connection
            cmd.Parameters.AddWithValue("@route", id);
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            while (reader.Read())
            {
                flightRoute.RouteID = reader.GetInt32(0);
                flightRoute.DepartureCity = reader.GetString(1);
                flightRoute.DepartureCountry = reader.GetString(2);
                flightRoute.ArrivalCity = reader.GetString(3);
                flightRoute.ArrivalCountry = reader.GetString(4);
                flightRoute.FlightDuration = reader.GetInt32(5);


            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();


            return flightRoute;
        }

        public List<FlightSchedule> GetFlightSchedules()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM dbo.FlightSchedule ORDER BY ScheduleID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<FlightSchedule> scheduleList = new List<FlightSchedule>();
            while (reader.Read())
            {
                scheduleList.Add(
                new FlightSchedule
                {
                    ScheduleID = reader.GetInt32(0),
                    FlightNumber = reader.GetString(1),
                    RouteID = reader.GetInt32(2),
                    AircraftID = !reader.IsDBNull(3) ? reader.GetInt32(3): (int?) null,
                    DepartureDateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4): (DateTime?)null,
                    ArrivalDateTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                    EconomyClassPrice = reader.GetDecimal(6),
                    BusinessClassPrice = reader.GetDecimal(7),
                    Status = reader.GetString(8)                        

                }
                ); ;
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return scheduleList;
        }


        public FlightSchedule getFlightDetails(int scheduleID)
        {
            FlightSchedule flightSchedule = new FlightSchedule();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM dbo.FlightSchedule WHERE ScheduleID = @selectedScheduleID";
            cmd.Parameters.AddWithValue("@selectedScheduleID", scheduleID);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    flightSchedule.ScheduleID = reader.GetInt32(0);
                    flightSchedule.FlightNumber = reader.GetString(1);
                    flightSchedule.RouteID = reader.GetInt32(2);
                    flightSchedule.AircraftID = !reader.IsDBNull(3) ? reader.GetInt32(3) : (int?)null;
                    flightSchedule.DepartureDateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null;
                    flightSchedule.ArrivalDateTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null;
                    flightSchedule.EconomyClassPrice = reader.GetDecimal(6);
                    flightSchedule.BusinessClassPrice = reader.GetDecimal(7);
                    flightSchedule.Status = reader.GetString(8);

                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return flightSchedule;
        }

        public int AddRoute(FlightRoute flightRoute)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO FlightRoute (DepartureCity, DepartureCountry, ArrivalCity,
            ArrivalCountry, FlightDuration)
            OUTPUT INSERTED.RouteID
            VALUES(@dcity, @dcountry, @acity, @acountry,
            @fduration)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@dcity", flightRoute.DepartureCity);
            cmd.Parameters.AddWithValue("@dcountry", flightRoute.DepartureCountry);
            cmd.Parameters.AddWithValue("@acity", flightRoute.ArrivalCity);
            cmd.Parameters.AddWithValue("@acountry", flightRoute.ArrivalCountry);
            cmd.Parameters.AddWithValue("@fduration", flightRoute.FlightDuration);
        
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            flightRoute.RouteID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return flightRoute.RouteID;
        }
        [HttpPost]
        public int AddSchedule(FlightSchedule flightSchedule)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO FlightSchedule (FlightNumber, RouteID, AircraftID,
            DepartureDateTime, ArrivalDateTime,EconomyClassPrice,BusinessClassPrice,Status)
            OUTPUT INSERTED.ScheduleID
            VALUES(@fNumber,@rID, @aID, @dDateTime, @aDateTime,
            @eClassPrice,@bClassPrice,@status)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@fNumber", flightSchedule.FlightNumber);
            cmd.Parameters.AddWithValue("@rID", flightSchedule.RouteID);

            cmd.Parameters.AddWithValue("@aID", flightSchedule.AircraftID);
            cmd.Parameters.AddWithValue("@dDateTime", flightSchedule.DepartureDateTime);
            cmd.Parameters.AddWithValue("@aDateTime", flightSchedule.ArrivalDateTime);
            cmd.Parameters.AddWithValue("@eClassPrice", flightSchedule.EconomyClassPrice);
            cmd.Parameters.AddWithValue("@bClassPrice", flightSchedule.BusinessClassPrice);
            cmd.Parameters.AddWithValue("@status", flightSchedule.Status);

            System.Diagnostics.Debug.WriteLine(flightSchedule.Status);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            flightSchedule.ScheduleID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return flightSchedule.ScheduleID;
        }

        public int Update(FlightSchedule flightSchedule)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE FlightSchedule 
            SET Status = @status
            WHERE ScheduleID = @selectedScheduleID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@status", flightSchedule.Status);
            cmd.Parameters.AddWithValue("@selectedScheduleID", flightSchedule.ScheduleID);
            //Open a database connection
            conn.Open();

            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = 0;
            count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;

        }

        //Get Staff For Login
        public Staff checkLogin(string Email, string Password)
        {
            SqlCommand cmd = conn.CreateCommand();
            Staff s1 = new Staff();
            cmd.CommandText = @"SELECT * FROM dbo.Staff 
                              WHERE EmailAddr = @email AND
                              Password = @pass";

            cmd.Parameters.AddWithValue("@email", Email);
            cmd.Parameters.AddWithValue("@pass", Password);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    s1.Vocation = reader.GetString(4);
                    s1.EmailAddr = reader.GetString(5);
                    s1.Password = reader.GetString(6);
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            if (s1 == null)
            { return null; }

            return s1;
        }


    }
}
