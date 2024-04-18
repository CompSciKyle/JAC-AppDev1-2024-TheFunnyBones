using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CalendarMVP
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory : Window, ViewInterfaceForEventsAndCategories
    {
        TextBlock dbNameTxtBlock;
        public NewCategory()
        {
            InitializeComponent();
            dbNameTxtBlock = Txb_DBName;
        }
        private void BtnClickCancel(object sender, RoutedEventArgs e)
        {

            throw new NotImplementedException();
        }

        private void BtnClickSave(object sender, RoutedEventArgs e)
        {

            throw new NotImplementedException();
        }
        public void DisplayDB(string DBName)
        {
            dbNameTxtBlock.Text = DBName; // Get the name of the db and assign it to the Text value of the TextBlock
        }
        public void DisplayMessage(string message)
        {
            string messageBoxCaption = "Error!";
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            MessageBoxImage messageBoxImage = MessageBoxImage.Error;
            MessageBoxResult result;
            result = MessageBox.Show(message, messageBoxCaption, messageBoxButton, messageBoxImage);
            throw new NotImplementedException();
        }
        public void ClosingConfirmation(object sender, CancelEventArgs e)
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
        public void ShowTypes()
        {
            throw new NotImplementedException();
        }
        public void ShowCategory()
        {
            throw new NotImplementedException();
        }
    }
}
