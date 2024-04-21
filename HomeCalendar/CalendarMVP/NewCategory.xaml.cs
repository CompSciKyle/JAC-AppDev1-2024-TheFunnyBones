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
        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            presenter.NewCategory((Category.CategoryType)Cmb_CategoriesTypes.SelectedItem, Txb_Description.Text);
        }

        public void DisplayDB()
        {
            this.Hide();
        }

        public void DisplayMessage(string message)
        {
            //string messageBoxCaption = "Error!";
            //MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            //MessageBoxImage messageBoxImage = MessageBoxImage.Error;
            //MessageBoxResult result;
            //result = MessageBox.Show(message, messageBoxCaption, messageBoxButton, messageBoxImage);
            //throw new NotImplementedException();

            MessageBox.Show(message);
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
            Cmb_CategoriesTypes.ItemsSource = allCategoryTypes;
        }

        public void ShowDbName(string DBName)
        {
            Txb_DBName.Text = DBName;
        }
    }
}
