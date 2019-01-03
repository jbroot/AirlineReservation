using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace AirlineReservation.Main
{
    /// <summary>
    /// Business logic for the Main Window
    /// </summary>
    class MainLogic
    {
        #region attributes
        /// <summary>
        /// List of all flights
        /// </summary>
        public List<Flight> flightList;

        /// <summary>
        /// Remembers the currentPassenger
        /// </summary>
        public Passenger currentPassenger;

        /// <summary>
        /// List of passengers for flight1
        /// </summary>
        public List<Passenger> passengersFlight1;

        /// <summary>
        /// List of passengers for flight2
        /// </summary>
        public List<Passenger> passengersFlight2;

        /// <summary>
        /// Link to the SQL class
        /// </summary>
        MainSQL dbLink;
        #endregion

        /// <summary>
        /// Constructor for MainLogic
        /// </summary>
        public MainLogic()
        {
            try
            {
                flightList = new List<Flight>();
                dbLink = new MainSQL();
                buildFlightList();
                passengersFlight1 = buildPassengerList(1);
                passengersFlight2 = buildPassengerList(2);

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Builds the flightList
        /// </summary>
        void buildFlightList()
        {
            try
            {
                int flightListLength = 0;
                DataSet ds = dbLink.getFlightTable(ref flightListLength);

                for (int i = 0; i < flightListLength; i++)
                {
                    Flight newFlight = new Flight((int)ds.Tables[0].Rows[i][0],
                        Int32.Parse((string)ds.Tables[0].Rows[i][1]),
                        (string)ds.Tables[0].Rows[i][2]);
                    flightList.Add(newFlight);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Builds the passenger lists
        /// </summary>
        /// <param name="flightID"></param>
        /// <param name="rowsAffected"></param>
        /// <returns></returns>
        public List<Passenger> buildPassengerList(int flightID)
        {
            try
            {
                int rowsAffected = 0;
                //ds is Passenger.Passenger_ID, First_Name, Last_Name, FPL.Seat_Number
                DataSet ds = dbLink.getPassengersFromFlight(flightID, ref rowsAffected);
                List<Passenger> passengers = new List<Passenger>();

                for (int i = 0; i < rowsAffected; i++)
                {
                    string firstName = (string)ds.Tables[0].Rows[i][1];
                    string lastName = (string)ds.Tables[0].Rows[i][2];
                    int passengerID = (int)ds.Tables[0].Rows[i][0];
                    int seatNum = Int32.Parse((string)ds.Tables[0].Rows[i][3]);
                    Passenger newPassenger = new Passenger(firstName, lastName, passengerID, seatNum);
                    passengers.Add(newPassenger);
                }
                return passengers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets a passenger using the seat and flightID
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="flightID"></param>
        /// <returns></returns>
        public Passenger getPassenger(int seat, int flightID)
        {
            try
            {
                List<Passenger> passengers;
                if (flightID == 1) passengers = passengersFlight1;
                else passengers = passengersFlight2;
                foreach (Passenger p in passengers)
                {
                    if (p.seatNum == seat) return p;
                }
                throw new Exception("Passenger seatNum not found in Passenger List");
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets a passenger using the Passenger ID and FlightID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flightID"></param>
        /// <returns></returns>
        public Passenger getPassengerFromID(int id, int flightID)
        {
            try
            {
                List<Passenger> passengers;
                if (flightID == 1) passengers = passengersFlight1;
                else passengers = passengersFlight2;
                foreach (Passenger p in passengers)
                {
                    if (p.passengerID == id) return p;
                }
                throw new Exception("Passenger ID not found in Passenger List");
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
        /// <param name="flightID"></param>
        /// <returns></returns>
        public int updatePassenger(int Pid, int seat, int flightID)
        {
            try
            {
                List<Passenger> passengers;
                if (flightID == 1) passengers = passengersFlight1;
                else passengers = passengersFlight2;
                currentPassenger.seatNum = seat;
                for (int i = 0; i < passengers.Count; i++)
                {
                    if (passengers[i].passengerID == Pid)
                    {
                        passengers[i] = currentPassenger;
                        break;
                    }
                }
                if (flightID == 1) passengersFlight1 = passengers;
                else passengersFlight2 = passengers;
                return dbLink.updatePassenger(currentPassenger.passengerID, currentPassenger.seatNum, flightID);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Adds a passenger to the passengerList and database
        /// </summary>
        /// <param name="flightID"></param>
        /// <param name="passengerID"></param>
        /// <param name="seatNum"></param>
        /// <param name="oldPassengers"></param>
        public void addPassenger(int flightID, int passengerID, int seatNum, ref List<Passenger> oldPassengers)
        {
            try
            {
                currentPassenger.seatNum = seatNum;
                oldPassengers.Add(currentPassenger);

                //update database
                if (dbLink.addFlightPassengerLink(flightID, passengerID, seatNum) == 0)
                    throw new Exception("FlightPassengerLink not added");
            }

            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes a passenger from the passengerList and database
        /// </summary>
        /// <param name="passengers"></param>
        public void deletePassenger(ref List<Passenger> passengers)
        {
            try
            {
                //update list
                foreach (Passenger p in passengers)
                {
                    if (p.passengerID == currentPassenger.passengerID)
                    {
                        passengers.Remove(p);
                        break;
                    }
                }
                //update database
                dbLink.deletePassenger(currentPassenger.passengerID);

                currentPassenger = null;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public bool isSeatInDB(ref int passengerID)
        {
            return dbLink.checkIfPassengerInLink(passengerID) == "";
        }
    }
}
