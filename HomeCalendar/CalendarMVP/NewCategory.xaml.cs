using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalendarMVP
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory : Window, ViewInterfaceForEventsAndCategories
    {
        public NewCategory()
        {
            InitializeComponent();
        }

        public void test()
        {
            Txb_Description.Text = string.Empty;
        }
        private void BtnClickCancel(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnClickSave(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        public void DisplayDB()
        {
            TextBlock dbNameTxtBlock = this.Txb_DBName;
            dbNameTxtBlock.Text = string.Empty; // Get the name of the db and assign it to the Text value of the TextBlock
            throw new NotImplementedException();
        }
        public void DisplayMessage(string message)
        {
            throw new NotImplementedException();
        }
        public void ClosingConfirmation()
        {
            string messageBoxText = "Are you sure you would like to exit the window? Any unsaved changes will be lost."; // Create a class that makes this code a static method so anywhere that needs to use it will have access to it. 
            string messageBoxCaption = "Exit Window?";
            MessageBoxButton messageBoxButton = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Exclamation;
            MessageBoxResult result;
            result = MessageBox.Show(messageBoxText, messageBoxCaption, messageBoxButton, messageBoxImage);
            if (result == MessageBoxResult.Yes)
            {

            }
            else
            {

            }
            throw new NotImplementedException();
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
