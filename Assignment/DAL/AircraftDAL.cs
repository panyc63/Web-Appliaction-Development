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
    public class AircraftDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public AircraftDAL()
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
        public List<Aircraft> GetAllAircraft()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Aircraft ORDER BY AircraftID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Aircraft> aircraftList = new List<Aircraft>();
            while (reader.Read())
            {
                aircraftList.Add(
                new Aircraft
                {
                    AircraftID = reader.GetInt32(0), //0: 1st column
                    MakeModel = reader.GetString(1), //1: 2nd column
                                 //Get the first character of a string
                    NumEconomySeat = !reader.IsDBNull(2) ?
                                        reader.GetInt32(2) : (int?)null, //2: 3rd column
                    NumBusinessSeat = !reader.IsDBNull(3) ?
                                        reader.GetInt32(3) : (int?)null, //3: 4th column
                    DateLastMaintenance = !reader.IsDBNull(4) ?
	                              reader.GetDateTime(4) : (DateTime?)null, //5: 6th column
                    Status = reader.GetString(5), //6: 7th column
    
                }
                );
            }//Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return aircraftList;
        }
        public int Add(Aircraft aircraft)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Aircraft (MakeModel, NumEconomySeat, NumBusinessSeat, DateLastMaintenance,
 Status)
OUTPUT INSERTED.AircraftID
VALUES(@makemodel, @numeconomy, @numbusiness, @datelastmaintenance, @status)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@makemodel", aircraft.MakeModel);
            cmd.Parameters.AddWithValue("@numeconomy", aircraft.NumEconomySeat);
            cmd.Parameters.AddWithValue("@numbusiness", aircraft.NumBusinessSeat);
            if (aircraft.DateLastMaintenance != null)
	        cmd.Parameters.AddWithValue("@datelastmaintenance",
	                                                            aircraft.DateLastMaintenance.Value);
	else
	        cmd.Parameters.AddWithValue("@datelastmaintenance",
	                                                               DBNull.Value);
            cmd.Parameters.AddWithValue("@status", aircraft.Status);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            aircraft.AircraftID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return aircraft.AircraftID;
        }
        public Aircraft GetDetails(int aircraftID)
        {
            Aircraft aircraft = new Aircraft();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Aircraft
 WHERE AircraftID = @selectedAircraftID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedAircraftID", aircraftID);
            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    aircraft.AircraftID = aircraftID;
                    aircraft.MakeModel = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    // (char) 0 - ASCII Code 0 - null value
                    aircraft.NumEconomySeat = !reader.IsDBNull(2) ?
                    reader.GetInt32(2) : (int?)null;

                    aircraft.NumBusinessSeat = !reader.IsDBNull(3) ?
                    reader.GetInt32(3) : (int?)null;

                    aircraft.DateLastMaintenance = !reader.IsDBNull(4) ?
                    reader.GetDateTime(4) : (DateTime?)null;

                    aircraft.Status = !reader.IsDBNull(5) ? reader.GetString(5) : null;

                    
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return aircraft;
        }
        public int Update(Aircraft aircraft)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Aircraft SET MakeModel=@makemodel,
 Status=@status, NumEconomySeat = @Noecon, NumBusinessSeat=@NoBus
WHERE AircraftID = @selectedAircraftID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@makemodel", aircraft.MakeModel);
            cmd.Parameters.AddWithValue("@status", aircraft.Status);
            cmd.Parameters.AddWithValue("@Noecon", aircraft.NumEconomySeat);
            cmd.Parameters.AddWithValue("@status", aircraft.NumBusinessSeat);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }


    }
}
