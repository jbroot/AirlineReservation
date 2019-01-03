using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace AirlineReservation.AddPassenger
{
    /// <summary>
    /// Interaction logic for wndAddPassenger.xaml
    /// </summary>
    public partial class WndAddPassenger : Window
    {
        #region attributes
        /// <summary>
        /// Connects to the class that has SQL methods needed
        /// </summary>
        AddPassengerSQL passengerSQL;

        /// <summary>
        /// Stores Passenger ID of new passenger added
        /// </summary>
        public int newPassengerID;
        #endregion

        /// <summary>
        /// constructor for the add passenger window
        /// </summary>
        public WndAddPassenger()
        {
            try
            {
                InitializeComponent();
                passengerSQL = new AddPassengerSQL();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// only allows letters to be input
        /// </summary>
        /// <param name="sender">sent object</param>
        /// <param name="e">key argument</param>
        private void txtLetterInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Only allow letters to be entered
                if (!(e.Key >= Key.A && e.Key <= Key.Z))
                {
                    //Override newline character as cmdSave_Click
                    if (e.Key == Key.Enter)
                    {
                        cmdSave_Click(sender, e);
                        e.Handled = true;
                    }

                    //Allow the user to use the backspace, delete, tab and enter
                    else if (!(e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab))
                    {
                        //No other keys allowed besides numbers, backspace, delete, tab, and enter
                        e.Handled = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                handleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Updates Passenger table and saves Passenger ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                newPassengerID = passengerSQL.addPassenger(txtFirstName.Text, txtLastName.Text);
                this.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Sets the flag to ignore this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                newPassengerID = -1;
                this.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// exception handler that shows the error
        /// </summary>
        /// <param name="sClass">the class</param>
        /// <param name="sMethod">the method</param>
        /// <param name="sMessage">the error message</param>
        private void handleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}