using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Dome_Control
{
    public partial class MainWindow
    {
        //private COM_Window comWindow;

        //private void COM_Option_Executed(object sender, RoutedEventArgs e)
        //{
        //    comWindow = new COM_Window(this);            
        //    comWindow.ShowDialog();
        //}

        /// <summary>
        /// Execution of the Help event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, @"../../Help/Peltier_GUI_Help_en_US.chm");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GraphOptions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PID_Options_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HelpMenu_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.MessageBox.Show("Inscrisci qui Help Menu", "Informazione", MessageBoxButton.OK, MessageBoxImage.Information);
            Help_Executed(sender, e as ExecutedRoutedEventArgs);
        }

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Graphical User Interface per Controllo Cupola\n\ncopyrigth A.R.A.\nVersione: " +
                string.Format("{0}\n", revisionString); ;
            System.Windows.MessageBox.Show(msg, "Peltier GUI", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
