using System;
using System.Reflection;

namespace AirlineReservation.AddPassenger
{
    /// <summary>
    /// SQL methods for the Add Passenger Form
    /// </summary>
    class AddPassengerSQL
    {
        /// <summary>
        /// Links to the class that opens the database connection
        /// </summary>
        clsDataAccess clsData;

        /// <summary>
        /// Constructs AddPassengerSQL
        /// </summary>
        public AddPassengerSQL()
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
        /// Adds passenger to Passenger table and returns Passenger_ID.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>Identity Column</returns>
        public int addPassenger(string firstName, string lastName)
        {
            try
            {
                //Add user
                string sql = "INSERT INTO Passenger(First_Name, Last_Name) VALUES('" + firstName + "','" + lastName + "');";

                //2nd parameter tells method to return identity column
                int passengerID = clsData.executeNonQuery(sql, true);
                if (passengerID == -1) throw new Exception("Passenger not added");

                return passengerID;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
    }
}
