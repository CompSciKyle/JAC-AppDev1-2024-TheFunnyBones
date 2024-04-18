using Calendar;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace CalendarMVP
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory : Window, ViewInterfaceForCategories
    {
        private Presenter presenter;
        public NewCategory(Presenter p)
        {
            InitializeComponent();
            presenter = p;
            presenter.RegisterNewView(this);
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
            this.Close();
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
        public void ShowTypes(List<Category.CategoryType> allCategoryTypes)
        {
            throw new NotImplementedException();
        }

        public void ShowDbName(string DBName)
        {
            Txb_DBName.Text = DBName;
        }
    }
}
