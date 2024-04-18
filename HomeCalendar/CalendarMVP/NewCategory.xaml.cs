using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Calendar;

namespace CalendarMVP
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory : Window, ViewInterfaceForCategories
    {
        private readonly Presenter presenter;
        public NewCategory(Presenter p)
        {
            InitializeComponent();
            presenter = p;
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
        public void DisplayDB(string DBName)
        {
            Txb_DBName.Text = DBName; // Get the name of the db and assign it to the Text value of the TextBlock
        }
        public void DisplayMessage(string message)
        {
            DisplayMessageBoxes.DisplayMessage(message);
        }
        public void ClosingConfirmation(object sender, CancelEventArgs e)
        {
            DisplayMessageBoxes.ClosingConfirmation(sender, e);
        }
        public void ShowTypes(List<Category.CategoryType> allCategoryTypes)
        {
            Cmb_Types.ItemsSource = allCategoryTypes;
        }
    }
}
