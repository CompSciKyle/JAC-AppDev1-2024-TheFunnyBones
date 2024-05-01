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
        private Presenter presenter;
        private bool updateEvent;
        public NewCategory(Presenter p)
        {
            InitializeComponent();
            presenter = p;
            presenter.RegisterNewView(this);
        }
        public NewCategory(Presenter p, bool fromEvent)
        {
            InitializeComponent();
            presenter = p;
            presenter.RegisterNewView(this);
            updateEvent = fromEvent;
        }
        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            presenter.NewCategory((Category.CategoryType)Cmb_CategoriesTypes.SelectedItem, Txb_Description.Text, updateEvent);
        }

        public void DisplayDB()
        {
            this.Hide();
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
            Cmb_CategoriesTypes.ItemsSource = allCategoryTypes;
        }

        public void ShowDbName(string DBName)
        {
            Txb_DBName.Text = DBName;
        }
    }
}
