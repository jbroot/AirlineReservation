using System;
using System.Data;
using System.Reflection;

namespace AirlineReservation.Main
{
    /// <summary>
    /// SQL statements and connections to the database
    /// </summary>
    class MainSQL
    {
        /// <summary>
        /// Connects to the class that opens the database for manipulation
        /// </summary>
        clsDataAccess clsData;

        /// <summary>
        /// Default constructor for MainSQL
        /// </summary>
        public MainSQL()
        {
            try
            {
                clsData = new clsDataAccess();

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Updates the passenger's seat
        /// </summary>
        /// <param name="Pid"></param>
        /// <param name="seat"></param>
        /// <param name="FlightID"></param>
        /// <returns></returns>
        public int updatePassenger(int Pid, int seat, int FlightID)
        {
            try
            {
                string sql = "UPDATE Flight_Passenger_Link " +
                    "SET Seat_Number = " + seat.ToString() + " " +
                    "WHERE Flight_ID = " + FlightID.ToString() + " " +
                    "AND Passenger_ID = " + Pid.ToString() + ";";
                return clsData.executeNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes the passenger
        /// </summary>
        /// <param name="id"></param>
        public void deletePassenger(int id)
        {
            try
            {
                string sql = "DELETE FROM FLIGHT_PASSENGER_LINK " +
                    "where passenger_ID = " + id.ToString() + ";";
                clsData.executeNonQuery(sql);
                sql = "delete from passenger where passenger_id = " + id.ToString() + ";";
                clsData.executeNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets information for a specific passenger using the seatNum
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="flightID"></param>
        /// <param name="rowsAffected"></param>
        /// <returns></returns>
        public DataSet getPassenger(int seat, int flightID, ref int rowsAffected)
        {
            try
            {
                string sSQL = "SELECT Passenger.Passenger_ID, First_Name, Last_Name, FPL.Seat_Number " +
                 "FROM Passenger, Flight_Passenger_Link FPL " +
                 "WHERE Passenger.Passenger_ID = FPL.Passenger_ID AND " +
                 "Flight_ID = " + flightID.ToString() +
                 " AND FPL.Seat_Number = " + seat.ToString();
                return clsData.executeSQLStatement(sSQL, ref rowsAffected);

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets information for a specific passenger using the passenger ID
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="flightID"></param>
        /// <param name="rowsAffected"></param>
        /// <returns></returns>
        public DataSet getPassengerFromID(int seat, int flightID, ref int rowsAffected)
        {
            try
            {
                string sSQL = "SELECT Passenger.Passenger_ID, First_Name, Last_Name, FPL.Seat_Number " +
                 "FROM Passenger, Flight_Passenger_Link FPL " +
                 "WHERE Passenger.Passenger_ID = FPL.Passenger_ID AND " +
                 "Flight_ID = " + flightID.ToString() +
                 " AND FPL.Seat_Number = " + seat.ToString();
                return clsData.executeSQLStatement(sSQL, ref rowsAffected);

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the Flight Table
        /// </summary>
        /// <param name="rowsAffected"></param>
        /// <returns></returns>
        public DataSet getFlightTable(ref int rowsAffected)
        {
            try
            {
                string sSQL = "SELECT Flight_ID, Flight_Number, Aircraft_Type FROM FLIGHT";
                return clsData.executeSQLStatement(sSQL, ref rowsAffected);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets all the passengers associated with a flightID
        /// </summary>
        /// <param name="flightID"></param>
        /// <param name="rowsAffected"></param>
        /// <returns></returns>
        public DataSet getPassengersFromFlight(int flightID, ref int rowsAffected)
        {
            try
            {
                string sSQL = "SELECT Passenger.Passenger_ID, First_Name, Last_Name, FPL.Seat_Number " +
                              "FROM Passenger, Flight_Passenger_Link FPL " +
                              "WHERE Passenger.Passenger_ID = FPL.Passenger_ID AND " +
                              "Flight_ID = " + flightID.ToString();
                return clsData.executeSQLStatement(sSQL, ref rowsAffected);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the flightID using the Flight_Number
        /// </summary>
        /// <param name="flightNum"></param>
        /// <returns></returns>
        public int getFlightID(int flightNum)
        {
            try
            {
                return Int32.Parse(clsData.executeScalarSQL("SELECT Flight_ID FROM FLIGHT WHERE Flight_Number = " + flightNum.ToString() + ";"));
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Adds the passenger with the seatNum to Flight_Passenger_Link table
        /// </summary>
        /// <param name="flightID"></param>
        /// <param name="passengerID"></param>
        /// <param name="seatNum"></param>
        /// <returns></returns>
        public int addFlightPassengerLink(int flightID, int passengerID, int seatNum)
        {
            try
            {
                string sql = "INSERT INTO Flight_Passenger_Link(Flight_ID, Passenger_ID, Seat_Number) " +
                    "VALUES(" + flightID.ToString() + "," + passengerID.ToString() + "," + seatNum.ToString() + ");";
                return clsData.executeNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Checks if a passengerID is in the Flight_Passenger_Link table
        /// </summary>
        /// <param name="passengerID"></param>
        /// <returns></returns>
        public string checkIfPassengerInLink(int passengerID)
        {
            try
            {
                string sql = "select Seat_Number from Flight_Passenger_Link" +
                    " where Passenger_ID = " + passengerID.ToString() + ";";
                return clsData.executeScalarSQL(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
