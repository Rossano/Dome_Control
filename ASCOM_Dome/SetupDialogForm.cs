using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM.Arduino;
using System.IO.Ports;

namespace ASCOM.Arduino
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile 
            DomeCOMLabel.Text = Properties.Resources.DomeCOMLabelContent;
            foreach (string s in SerialPort.GetPortNames())
            {
                DomeCOMcomboBox.Items.Add(s);
            }
            chkTrace.Checked = Dome.traceState;
            TelesChooseLabel.Text = Properties.Resources.TelescopeChooserLabelContent;
            TelescopeChooserButton.Text = Properties.Resources.TelescopeChooserButtonContent;
            cmdOK.Text = Properties.Resources.cmdOKLabel;
            cmdCancel.Text = Properties.Resources.cmdCancelLabel;
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here

            if(!CheckTelescopeRegistration())
            {
                throw new Exception("Telescope not chosen");
            }
            using (ASCOM.Utilities.Profile p = new Profile())
            {
                p.DeviceType = "Dome";
                p.WriteValue(Dome.driverID, "comPort", (string)DomeCOMcomboBox.SelectedItem);
            }
            Dispose();
            //if(DomeCOMcomboBox.SelectedItem.ToString().Contains("COM"))
            //{
            //    Dome.comPort=(string)DomeCOMcomboBox.SelectedItem;
            //    try
            //    {
            //        Dome.Connected.set(true);
            //    }
            //    catch(Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            //Dome.comPort = DomeCOMTextBox.Text; // Update the state variables with results from the dialogue
            //Dome.traceState = chkTrace.Checked;
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void TelescopeChooserButton_Click(object sender, EventArgs e)
        {
            try
            {
                Dome._telescope = new ASCOM_Telescope_ns.ASCOM_Telescope();
            }
            catch(Exception ex)
            {
                Dome._telescope=null;
                throw ex;
            }
        }

        private void DomeCOMcomboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            Dome.comPort = (string)DomeCOMcomboBox.SelectedItem;
        }

        private bool CheckTelescopeRegistration()
        {
            using (ASCOM.Utilities.Profile p = new Profile())
            {
                if (p.RegisteredDevices("Telescope").Capacity != 0) return true;
                //foreach (string item in p.RegisteredDevices("Telescope"))
                //{
                //    if (item.Contains("Telescope")) return true;
                //}
            }
            return false;
        }
    }
}