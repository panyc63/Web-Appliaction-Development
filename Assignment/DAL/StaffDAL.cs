using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using Assignment.Models;

namespace Assignment.DAL
{


    public class StaffDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public StaffDAL()
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

        public List<Staff> GetAllStaff()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff ORDER BY StaffID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data 
            List<Staff> staffList = new List<Staff>();
            while (reader.Read())
            {
                staffList.Add(
                new Staff
                {
                    StaffID = reader.GetInt32(0), //0: 1st column
                    StaffName = reader.GetString(1), //1: 2nd column
                                                     //Get the first character of a string
                    Gender = !reader.IsDBNull(2) ? reader.GetString(2)[0] : (char?)null, //2: 3rd column
                    DateEmployed = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null, //3: 4th column
                    Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : (string?)null, //4: 5th column
                    EmailAddr = reader.GetString(5), //5: 6th column
                    Status = reader.GetString(7), //7: 8th column
                }
              );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return staffList;
        }

        public List<Staff> GetAllPilotStaff()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff WHERE Vocation ='Pilot' AND Status='Active' ORDER BY StaffID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data 
            List<Staff> staffList = new List<Staff>();
            while (reader.Read())
            {
                staffList.Add(
                new Staff
                {
                    StaffID = reader.GetInt32(0), //0: 1st column
                    StaffName = reader.GetString(1), //1: 2nd column
                                                     //Get the first character of a string
                    Gender = !reader.IsDBNull(2) ? reader.GetString(2)[0] : (char?)null, //2: 3rd column
                    DateEmployed = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null, //3: 4th column
                    Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : (string?)null, //4: 5th column
                    EmailAddr = reader.GetString(5), //5: 6th column
                    Status = reader.GetString(7), //7: 8th column
                }
              );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return staffList;
        }

        public List<Staff> GetAllAttendantStaff()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff WHERE Vocation ='Flight Attendant' AND Status='Active' ORDER BY StaffID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data 
            List<Staff> staffList = new List<Staff>();
            while (reader.Read())
            {
                staffList.Add(
                new Staff
                {
                    StaffID = reader.GetInt32(0), //0: 1st column
                    StaffName = reader.GetString(1), //1: 2nd column
                                                     //Get the first character of a string
                    Gender = !reader.IsDBNull(2) ? reader.GetString(2)[0] : (char?)null, //2: 3rd column
                    DateEmployed = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null, //3: 4th column
                    Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : (string?)null, //4: 5th column
                    EmailAddr = reader.GetString(5), //5: 6th column
                    Status = reader.GetString(7), //7: 8th column
                }
              );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return staffList;
        }

        public Staff GetDetails(int staffId)
        {
            Staff staff = new Staff();

            //crteate a sqlcommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //specify the select sql statment that retrives all attribute of a staff record
            cmd.CommandText = @"SELECT * FROM Staff WHERE StaffID = @selectedStaffID";

            //Define the parameter used in sql statement, value for the parameter is retrived from the mehod parameter "StaffId"
            cmd.Parameters.AddWithValue("@selectedStaffID", staffId);

            //open db connection
            conn.Open();

            //execute SELECT sql through datareader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    staff.StaffID = staffId;
                    staff.StaffName = reader.GetString(1);
                    // (char) 0 - ASCII Code 0 - null value
                    staff.Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : (string?)null;
                    staff.EmailAddr = reader.GetString(5);
                    staff.Status = reader.GetString(7);
                }
            }

            reader.Close();
            conn.Close();

            return staff;
        }

        public int Update(Staff staff)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Staff SET Vocation=@vocation,
                                EmailAddr =@emailaddr, Status = @status
                                WHERE StaffID = @selectedStaffID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@vocation", staff.Vocation);
            cmd.Parameters.AddWithValue("@emailaddr", staff.EmailAddr);
            cmd.Parameters.AddWithValue("@status", staff.Status);

            cmd.Parameters.AddWithValue("@selectedStaffID", staff.StaffID);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        //updates status
        public int Updatestatus(FlightCrew crew)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Staff SET Status = @status
                                WHERE StaffID = @selectedStaffID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@status", "Inactive");

            cmd.Parameters.AddWithValue("@selectedStaffID", crew.StaffID);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        //adds into db
        public int Add(Staff staff)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Staff (StaffName, Gender, DateEmployed, Vocation,
                                EmailAddr, Password, Status)
                                OUTPUT INSERTED.StaffID
                                VALUES(@name, @gender, @dateemployed, @vocation,
                                @email, @password, @status)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", staff.StaffName);
            cmd.Parameters.AddWithValue("@gender", staff.Gender);
            cmd.Parameters.AddWithValue("@dateemployed", staff.DateEmployed);
            cmd.Parameters.AddWithValue("@vocation", staff.Vocation);
            cmd.Parameters.AddWithValue("@email", staff.EmailAddr);
            cmd.Parameters.AddWithValue("@password", staff.Password);
            cmd.Parameters.AddWithValue("@status", "Active");
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            staff.StaffID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return staff.StaffID;
        }

        public int AddSchedule(FlightCrew crew)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            cmd.CommandText = @"INSERT INTO FlightCrew (ScheduleID, StaffID, Role)
                                VALUES(@scheduleid, @staffid,@role)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@scheduleid", crew.ScheduleID);
            cmd.Parameters.AddWithValue("@staffid", crew.StaffID);
            cmd.Parameters.AddWithValue("@role", crew.Role);
            //A connection to database must be opened before any operations made.
            conn.Open();

            crew.ScheduleID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return crew.ScheduleID;
        }

    }
}
