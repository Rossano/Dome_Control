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
using System.Windows.Forms;

namespace Dome_Control
{
    /// <summary>
    /// Interaction logic for NewFW_Download_Window.xaml
    /// </summary>
    public partial class NewFW_Download_Window : Window
    {
        #region Members

        /// <summary>
        /// Stores if the Arduino board has an Arduino Bootloader
        /// </summary>
        private bool _isArduinoBootloader;
        /// <summary>
        /// The Firmware filename
        /// </summary>
        private string FWFilename;
        /// <summary>
        /// The bootloader COM^port (it is different from the normal mode COM port)
        /// </summary>
        private string BootloaderCOM;

        #endregion
        
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NewFW_Download_Window"/> class.
        /// </summary>
        /// <param name="isArduinoBootloader">if set to <c>true</c> if it is an [is arduino bootloader]. It is heredited from the Main Windows</param>
        public NewFW_Download_Window(bool isArduinoBootloader)
        {
            InitializeComponent();
            _isArduinoBootloader = isArduinoBootloader;
            //NewFW_Download_Window.TitleProperty.Name = Properties.Resources.FW_Download_Wnd_Title;
            //
            //  Gets the strings from the local resources
            //
            LoadNewFWLabel.Content = Properties.Resources.LoadNewFWLabel;
            NewFWLoadButton.Content = Properties.Resources.LoadNewFWButton;
            BootloaderComLabel.Content = Properties.Resources.BootloaderCOMLabel;
            //  Fills the Bootloader COM ComboBox with COM port name from 3 to 99
            for (int i = 3; i < 99; i++)
            {
                BootloadCOMComboBox.Items.Add("COM" + i.ToString());
            }
            //  Ends last strings from the local resources
            ArduinoBootloader_CheckBox.IsChecked = isArduinoBootloader;
            DownloadButton.Content = Properties.Resources.DownloadButton;
            CancelDownloadButton.Content = Properties.Resources.CancelDownloadButton;
        }

        #endregion

        #region Event Handlers        

        /// <summary>
        /// DownloadButton control Click Event Handler.
        /// If Data are properly filled it launches avrdude to download a new firmware.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            //  Checks if the filename is correct and a COM port is selected, if yes it launches avrdude.
            if (FWFilename != null && FWFilename.Length > 0 && BootloaderCOM.Length > 0)
            {
                System.Windows.MessageBox.Show("Launch AVRDUDE");
            }
            //  Then it closes the window
            this.Close();
        }

        /// <summary>
        /// Cancel Download Button control click event handler.
        /// It simply close the window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CancelDownloadButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// New FW Load Button control click Event Handler.
        /// Open a fileopen dialog box to select a new HEX otr ELF file.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void NewFWLoadButton_Click(object sender, RoutedEventArgs e)
        {
            //  Declare the FileOpen Dialog Box and fill to search for an HEX or ELF firmware file
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Firmware Files (*.hex;*.elf)|*.hex;*.elf" + "|All Files(*.*)|*.*";
            dlg.CheckFileExists = true;
            // Obnly 1 firmware
            dlg.Multiselect = false;
            //  Launch the dialog box and waits to an answer, if it positive store the selected file name into FWFilename
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //  Store the filename into FWFilename and into the window textbox
                FWFilename = dlg.FileName;
                FWFilenameTextBox.Text = FWFilename;
            }
        }

        /// <summary>
        /// Bootloader COM ComboBox control Selectrion Changed Event Handler.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void BootloadCOMComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //  Updates the Bootloader COM port with the new selection
            BootloaderCOM = BootloadCOMComboBox.SelectedItem as string;
        }

        /// <summary>
        /// Arduino Bootloader CheckBox control Click event Handler. 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ArduinoBootloader_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            //  Updates the flag indicating if Arduino Bootloader has to be used.
            if (ArduinoBootloader_CheckBox != null) _isArduinoBootloader = (bool)ArduinoBootloader_CheckBox.IsChecked;
        }

        #endregion

    }
}
