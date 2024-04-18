using Calendar;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CalendarMVP
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory : Window, ViewInterfaceForCategories
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
            try
            {

            }
            catch (Exception ex)
            {
                DisplayMessage("Failed to create new category: " + ex.Message);
            }
            throw new NotImplementedException();
        }
        public void DisplayDB()
        {
            dbNameTxtBlock.Text = DBName; // Get the name of the db and assign it to the Text value of the TextBlock
        }
        public void DisplayMessage(string message)
        {
            DisplayMessageBoxes.DisplayMessage(message);
        }
        public void ClosingConfirmation(object sender, CancelEventArgs e)
        {
            DisplayMessageBoxes.ClosingConfirmation(sender, e);
        }

        public void ShowDbName(string DBName)
        public void ShowTypes(List<Category.CategoryType> allCategoryTypes)
        {
            Txb_DBName.Text = DBName;
            Cmb_Types.ItemsSource = allCategoryTypes;
        }
    }
}
