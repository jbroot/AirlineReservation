using System;
using System.Reflection;

namespace AirlineReservation.Main
{
    /// <summary>
    /// Represents rows in the Flight Table
    /// </summary>
    class Flight
    {
        /// <summary>
        /// The flight's primary key
        /// </summary>
        public int flightID;

        /// <summary>
        /// The Flight Number
        /// </summary>
        public int flightNumber;

        /// <summary>
        /// The Aircraft Type
        /// </summary>
        public string aircraftType;

        /// <summary>
        /// Constructs Flight
        /// </summary>
        public Flight(int FlightID, int FlightNum, string AircraftType)
        {
            try
            {
                this.flightID = FlightID;
                this.flightNumber = FlightNum;
                this.aircraftType = AircraftType;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

    }
}
