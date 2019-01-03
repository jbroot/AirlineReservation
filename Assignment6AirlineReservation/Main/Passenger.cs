using System;
using System.Reflection;

namespace AirlineReservation.Main
{
    /// <summary>
    /// Abstraction of the Passenger and Flight_Passenger_Link tables
    /// </summary>
    class Passenger
    {
        #region attributes
        /// <summary>
        /// Passenger's first name
        /// </summary>
        public string firstName;

        /// <summary>
        /// Passenger's last name
        /// </summary>
        public string lastName;

        /// <summary>
        /// Passenger's ID
        /// </summary>
        public int passengerID;

        /// <summary>
        /// Passenger's seat number
        /// </summary>
        public int seatNum;
        #endregion

        /// <summary>
        /// Constructor for the passenger
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="pID"></param>
        /// <param name="SeatNum"></param>
        public Passenger(string fName, string lName, int pID, int SeatNum)
        {
            try
            {
                this.firstName = fName;
                this.lastName = lName;
                this.passengerID = pID;
                this.seatNum = SeatNum;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
