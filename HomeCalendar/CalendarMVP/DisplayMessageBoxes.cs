using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CalendarMVP
{
    internal static class DisplayMessageBoxes
    {
        public static void DisplayMessage(string message)
        {
            string messageBoxCaption = "Error!";
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            MessageBoxImage messageBoxImage = MessageBoxImage.Error;
            MessageBoxResult result;
            result = MessageBox.Show(message, messageBoxCaption, messageBoxButton, messageBoxImage);
        }
        public static void ClosingConfirmation(object sender, CancelEventArgs e)
        {
            string messageBoxText = "Are you sure you would like to exit the window? Any unsaved changes will be lost."; // Create a class that makes this code a static method so anywhere that needs to use it will have access to it. 
            string messageBoxCaption = "Exit Window?";
            MessageBoxButton messageBoxButton = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Exclamation;
            MessageBoxResult result;
            result = MessageBox.Show(messageBoxText, messageBoxCaption, messageBoxButton, messageBoxImage);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; 
            }
        }
    }
}
