using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AirlineReservation.AddPassenger;

namespace AirlineReservation.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region attributes
        /// <summary>
        /// Add Passenger Form
        /// </summary>
        WndAddPassenger wndAddPass;
        /// <summary>
        /// Business Logic class
        /// </summary>
        MainLogic mainLogic;

        /// <summary>
        /// Array of labels for Flight 767
        /// </summary>
        public Label[] Flight767;

        /// <summary>
        /// Array of labels for Flight 380
        /// </summary>
        public Label[] Flight380;

        /// <summary>
        /// Keeps track of which labels are currently being used
        /// </summary>
        Label[] currentFlightLabels;

        /// <summary>
        /// Remembers the previous label used
        /// </summary>
        Label prevLabel;

        /// <summary>
        /// Remembers which flightID is currently being worked with
        /// </summary>
        int currentFlightID;

        /// <summary>
        /// Remembers the list of current passengers being used
        /// </summary>
        List<Passenger> currentPassengers;

        /// <summary>
        /// Key allowing the user to change seats
        /// </summary>
        bool AllowChangeSeat = false;

        #endregion

        /// <summary>
        /// Default constructor for Main Window
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                Flight767 = createSeatArray(16, ref c767_Seats);
                Flight380 = createSeatArray(15, ref cA380_Seats);

                mainLogic = new MainLogic();

                for(int i = 0; i < mainLogic.flightList.Count; i++)
                {
                    ComboBoxItem anotherFlight = new ComboBoxItem();
                    anotherFlight.Content = mainLogic.flightList[i].flightNumber.ToString() + " " + mainLogic.flightList[i].aircraftType;
                    anotherFlight.Tag = mainLogic.flightList[i].flightNumber;
                    cbChooseFlight.Items.Add(anotherFlight);
                }
            }
            catch (Exception ex)
            {
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Creates the seat array
        /// </summary>
        /// <param name="numSeats"></param>
        /// <param name="canvas"></param>
        /// <returns></returns>
        private Label[] createSeatArray(int numSeats, ref Canvas canvas)
        {
            try
            {
                Label[] labels = new Label[numSeats];
                //Place all labels in the array
                foreach (var c in canvas.Children)
                {
                    if (c is Label)
                    {
                        Label thisSeat = (Label)c;
                        string placeString = thisSeat.Content.ToString();
                        int place = Int32.Parse(placeString);
                        thisSeat.MouseLeftButtonUp += seatClick;
                        labels[place - 1] = thisSeat;
                    }
                }
                return labels;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Actions to take when a seat is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seatClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label seat = (Label)sender;
                //if no changes are possible with selection return
                if ((seat.Background == Brushes.Blue && mainLogic.currentPassenger == null) || (seat.Background == Brushes.Blue && !AllowChangeSeat) || (seat.Background == Brushes.Red && AllowChangeSeat)) return;

                //if selection is the same
                if (seat.Background == Brushes.Lime)
                {
                    disableEverythingExceptSeats(true);
                    AllowChangeSeat = false;
                }

                if(mainLogic.currentPassenger != null)
                {
                    //if new passenger
                    if (mainLogic.currentPassenger.seatNum == 0)
                        {
                            if (currentFlightID == 1)
                                addPassenger(ref mainLogic.passengersFlight1, ref seat);
                            else
                                addPassenger(ref mainLogic.passengersFlight2, ref seat);
                            disableEverythingExceptSeats(true);
                            AllowChangeSeat = false;
                        }
                }
                if(seat.Background == Brushes.Blue)
                {
                    changeSeat(ref seat);
                    AllowChangeSeat = false;
                    disableEverythingExceptSeats(true);
                }
                //if red change the current passenger
                else if (seat.Background == Brushes.Red)
                {
                    if (mainLogic.currentPassenger != null)
                    {
                        //if new passenger disallow changing
                        if (mainLogic.currentPassenger.seatNum == 0) return;
                    }
                    int seatNum = Int32.Parse(seat.Content.ToString());
                    changePassengerSelection(seatNum);
                }
                prevLabel = seat;
                
                //remove message that lets user know a seat should be selected
                lblSelectSeatAfterAdd.Visibility = Visibility.Hidden;

                //update seat number
                lblPassengersSeatNumber.Content = mainLogic.currentPassenger.seatNum.ToString();
            }
            catch (Exception ex)
            {
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Done after the user selects a seat after creating a new passenger
        /// </summary>
        /// <param name="oldPassengers"></param>
        /// <param name="seat"></param>
        private void addPassenger(ref List<Passenger> oldPassengers, ref Label seat)
        {
            try
            {
                seat.Background = Brushes.Lime;
                prevLabel = seat;
                int seatNum = Int32.Parse(seat.Content.ToString());
                //add flight_passenger_link
                mainLogic.addPassenger(currentFlightID, mainLogic.currentPassenger.passengerID, seatNum, ref oldPassengers);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Changes to the correct user when a red seat is clicked
        /// </summary>
        /// <param name="seatNum"></param>
        private void changePassengerSelection(int seatNum)
        {
            try
            {
                mainLogic.currentPassenger = mainLogic.getPassenger(seatNum, currentFlightID);
                //update cbchoosepassenger selected item
                for (int i = 0; i < cbChoosePassenger.Items.Count; i++)
                {
                    ComboBoxItem c = (ComboBoxItem)cbChoosePassenger.Items[i];
                    string tag = c.Tag.ToString();
                    if (tag == mainLogic.currentPassenger.passengerID.ToString())
                    {
                        cbChoosePassenger.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Changes the seat by clicking on a blue seat
        /// </summary>
        /// <param name="seat"></param>
        private void changeSeat(ref Label seat)
        {
            try
            {
                //if not allowed to change seat then return
                if (!AllowChangeSeat) return;
                
                //previous seat is now empty
                prevLabel.Background = Brushes.Blue;

                //update passenger
                int seatNum = Int32.Parse(seat.Content.ToString());
                if (mainLogic.updatePassenger(mainLogic.currentPassenger.passengerID, seatNum, currentFlightID) == 0)
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + "Passenger not updated");
                seat.Background = Brushes.Lime;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Updates class attributes to reflect which flight is being used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbChooseFlight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxItem selection = (ComboBoxItem)cbChooseFlight.SelectedItem;
                cbChoosePassenger.IsEnabled = true;
                gPassengerCommands.IsEnabled = true;

                mainLogic.currentPassenger = null;

                if (selection.Tag.ToString() == "412")
                {
                    CanvasA380.Visibility = Visibility.Hidden;
                    Canvas767.Visibility = Visibility.Visible;
                    currentFlightID = 2;
                    currentPassengers = mainLogic.passengersFlight2;
                    currentFlightLabels = Flight767;
                }
                else
                {
                    Canvas767.Visibility = Visibility.Hidden;
                    CanvasA380.Visibility = Visibility.Visible;
                    currentFlightID = 1;
                    currentPassengers = mainLogic.passengersFlight1;
                    currentFlightLabels = Flight380;
                }
                cbChoosePassengerCreateItems();
            }
            catch (Exception ex)
            {
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Updates currentPassenger and label color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbChoosePassenger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //if selection is null
                if(cbChoosePassenger.SelectedIndex == -1)
                {
                    return;
                }

                //enable modification for passenger
                cmdChangeSeat.IsEnabled = cmdDeletePassenger.IsEnabled = true;

                //previous label will always be taken
                if (prevLabel != null) prevLabel.Background = Brushes.Red;
                ComboBoxItem passengerBox = (ComboBoxItem)cbChoosePassenger.SelectedItem;
                int passengerID = Int32.Parse(passengerBox.Tag.ToString());

                //if no seat in database
                if (mainLogic.isSeatInDB(ref passengerID))
                {
                    return;
                }
                int id = Int32.Parse(passengerBox.Tag.ToString());
                mainLogic.currentPassenger = mainLogic.getPassengerFromID(id, currentFlightID);

                //update seat number
                lblPassengersSeatNumber.Content = mainLogic.currentPassenger.seatNum.ToString();

                //update label color
                currentFlightLabels[mainLogic.currentPassenger.seatNum - 1].Background = Brushes.Lime;
                prevLabel = currentFlightLabels[mainLogic.currentPassenger.seatNum - 1];
            }
            catch (Exception ex)
            {
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Creates the combobox items for cbChoosePassenger
        /// </summary>
        private void cbChoosePassengerCreateItems()
        {
            try {
                if (currentFlightID == 1)
                {
                    currentPassengers = mainLogic.passengersFlight1;
                }
                else
                {
                    currentPassengers = mainLogic.passengersFlight2;
                }
                
                if(cbChoosePassenger != null)
                    cbChoosePassenger.Items.Clear();

                //string output = "";
                int iRet = currentPassengers.Count;

                //create cbChoosePassengerItems
                for (int i = 0; i < iRet; i++)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = currentPassengers[i].firstName + " " + currentPassengers[i].lastName;
                    comboBoxItem.Tag = currentPassengers[i].passengerID;
                    cbChoosePassenger.Items.Add(comboBoxItem);
                    int place = currentPassengers[i].seatNum - 1;
                    currentFlightLabels[place].Background = Brushes.Red;
                    //output += passengers[i].seatNum + "," + passengers[i].passengerID + '\n';
                }
                //cbChoosePassenger.SelectedIndex = 1;
                // MessageBox.Show(output);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Begins creating a passenger, submethods wait for a seat to be clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddPassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndAddPass = new WndAddPassenger();
                wndAddPass.ShowDialog();

                //if canceled do nothing
                if (wndAddPass.newPassengerID == -1) return;

                disableEverythingExceptSeats(false);
                mainLogic.currentPassenger = new Passenger(wndAddPass.txtFirstName.Text, wndAddPass.txtLastName.Text, wndAddPass.newPassengerID, 0);

                //add combobox option
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = mainLogic.currentPassenger.firstName + " " + mainLogic.currentPassenger.lastName;
                comboBoxItem.Tag = mainLogic.currentPassenger.passengerID;
                cbChoosePassenger.Items.Add(comboBoxItem);
                cbChoosePassenger.SelectedItem = comboBoxItem;

                //let user know a seat should be selected
                lblSelectSeatAfterAdd.Visibility = Visibility.Visible;

                //blank Passenger's seat number
                lblPassengersSeatNumber.Content = "";

                AllowChangeSeat = true;
            }
            catch (Exception ex)
            {
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the passenger and updates the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeletePassenger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //prevent error from modifying a null passenger
                prevLabel = null;
                cmdChangeSeat.IsEnabled = cmdDeletePassenger.IsEnabled = false;

                //change seat color to blue (empty)
                currentFlightLabels[mainLogic.currentPassenger.seatNum - 1].Background = Brushes.Blue;

                //find and remove passenger from combobox
                foreach (ComboBoxItem item in cbChoosePassenger.Items)
                {
                    if (item.Tag.ToString() == mainLogic.currentPassenger.passengerID.ToString())
                    {
                        cbChoosePassenger.Items.Remove(item);
                        break;
                    }
                }
                cbChoosePassenger.SelectedIndex = -1;
                mainLogic.deletePassenger(ref currentPassengers);

                //update seat number
                lblPassengersSeatNumber.Content = "";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Allows user to change the seat for currentpassenger and disables anything else
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdChangeSeat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if nothing is selected then do nothing
                if (cbChoosePassenger.SelectedIndex == -1) return;
                AllowChangeSeat = true;
                disableEverythingExceptSeats(false);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Buttons and comboboxes isEnabled == enable
        /// </summary>
        /// <param name="enable"></param>
        private void disableEverythingExceptSeats(bool enable)
        {
            try
            {
                gbPassengerInformation.IsEnabled = gPassengerCommands.IsEnabled = enable;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Handles all errors for this class
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void handleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}