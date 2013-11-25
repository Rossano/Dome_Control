using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dome_Control
{
    /// <summary>
    /// Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window, IDisposable
    {
        #region Fields

        /// <summary>
        /// Gets or sets the RX buffer.
        /// </summary>
        /// <value>
        /// The buffer of received chars.
        /// </value>
        public string RXbuffer { set; get; }
        /// <summary>
        /// Gets or sets the command string.
        /// </summary>
        /// <value>
        /// The command string.
        /// </value>
        public string command { get; set; }
        
        #endregion

        #region Members

       // private string command;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugWindow"/> class.
        /// </summary>
        /// <param name="mainWindow">The main window object.</param>
        public DebugWindow(Window mainWindow)
        {
            //  Sets the Ownership of this window to the main window
            this.Owner = mainWindow;
            //  Initialize the command string buffer
            command = string.Empty;
            //  Initialize the xaml objects
            InitializeComponent();
            //  Sets the windos dimensions
            this.Height = 400;
            this.Width = 500;            
        }

        #endregion

        #region Event Handlers

        //private void DebugOUT_TextBox_DragEnter(object sender, DragEventArgs e)
        //{
        //    command = DebugOUT_TextBox.Text;
        //}

        /// <summary>
        /// Handles the Closed event of the Debug Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            //this.Close();            
            //  Throws the Debug Window to tge garbage collector
            Dispose();
        }

        /// <summary>
        /// DebugOUT_TextBox control TextChanged Event Handler.
        /// If the DebugOUT_TextBox ends with CR gets all the lines of that control and send the last one to the Peltier object.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void DebugOUT_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 
            //  Checks if the TextBox controls ends with \n, if yes splits the TextBox text into its lines
            //  and send the last one to the Arduino controller (the Peltier object).
            //
            if (DebugOUT_TextBox.Text.EndsWith("\n"))
            {
                //
                //  New Line found, send the command to the Arduino
                //  If an error is found display the Error Dialog
                //
                try
                {
                    //  Splits the TextBox text into its composing lines
                    char[] delim = { '\r', '\n' };
                    string[] lines = DebugOUT_TextBox.Text.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    //  Send last line to the Arduino
                    command = lines.Last<string>();
                    ((App)(Application.Current))._Dome_uC.SendCommand(command);
                }
                catch (NullReferenceException ex)
                {
                    ErrDlg("Peltier Obect not yet referenced, please connect to the controller", ex);
                }
                catch (Exception ex)
                {
                    ErrDlg(ex.Message, ex);
                }
                finally
                {
                    //  In any case empty the command buffer string
                    command = string.Empty;
                }
            }
            //else command = DebugOUT_TextBox.Text;
        }

        #endregion

        #region Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
           
        }

        /// <summary>
        /// Generic function to display an Error on a Message Box.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="ex">The ex.</param>
        private void ErrDlg(string str, Exception ex)
        {
            System.Windows.MessageBox.Show(str + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        #endregion
    }
}
