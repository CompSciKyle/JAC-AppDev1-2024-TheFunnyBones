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
        public void DisplayDB()
        {
            this.Close();
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

        public void ShowDbName(string DBName)
        {
            Cmb_Types.ItemsSource = allCategoryTypes;
            Txb_DBName.Text = DBName;
        }
    }
}
