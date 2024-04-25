using Calendar;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CalendarMVP
{
    /// <summary>
    /// Interaction logic for MonthView.xaml
    /// </summary>
    public partial class MonthView : Window, ViewInterfaceForCalendar
    {
        private readonly Presenter presenter;
        public MonthView(Presenter p)
        {
            InitializeComponent();
            presenter = p;
            presenter.RegisterNewView(this);
            presenter.DisplayAll();
        }
        private void BtnClickNewEvent(object sender, RoutedEventArgs e)
        {
            NewEventWindow eventView = new NewEventWindow(presenter);
            eventView.Show();
        }

        private void BtnClickNewCategory(object sender, RoutedEventArgs e)
        {
            NewCategory newCategory = new NewCategory(presenter);
            newCategory.Show();
        }
        public void ShowDbName(string DBName)
        {
            dbName.Text = DBName;
        }

        public void DisplayMessage(string message)
        {
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

        public void DisplayBoard(List<CalendarItem> events)
        {
            myDataGrid.ClearValue(ItemsControl.ItemsSourceProperty);
            myDataGrid.Items.Clear();
            myDataGrid.ItemsSource = events;

        }

        public void ShowTypes(List<Category> categories)
        {
            Cmb_All_Categories.ItemsSource = categories;
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            // the selected object will always be a of the type that was originally put in the ItemsSource
            string hi = "hi";
        }

        private void Item_DoubleClick(object sender, RoutedEventArgs e)
        {
            // the selected object will always be a of the type that was originally put in the ItemsSource
            string hi = "hi";
        }

        private void startDateChanged(object sender, RoutedEventArgs e)
        {
            LoadCalendarItemColumns();
            FilterEvents();

        }

        private void endDateChanged(object sender, RoutedEventArgs e)
        {
            LoadCalendarItemColumns();
            FilterEvents();
        }

        private void FilterByCategory(object sender, RoutedEventArgs e)
        {
            LoadCalendarItemColumns();
            FilterEvents();
        }

        private void SelectsCategory(object sender, RoutedEventArgs e)
        {
            FilterEvents();
        }

        private void GetAllEventsByMonth(object sender, RoutedEventArgs e)
        {

            LoadCalendarItemsByMonth();
            FilterEvents();

        }

        private void GetAllEventsByCategory(object sender, RoutedEventArgs e)
        {

            LoadCalendarItemsByCategory();
            FilterEvents();
        }

        private void FilterEvents()
        {
            bool filterCategory = Ctb_FilterByCategory.IsChecked ?? false;
            bool eventsByMonth = Ctb_Month.IsChecked ?? false;
            bool eventsByCategory = Ctb_Category.IsChecked ?? false;
            presenter.PopulateDataGrid(Dtb_StartDate.Text, Dtb_EndDate.Text, filterCategory, (Category)Cmb_All_Categories.SelectedItem, eventsByMonth, eventsByCategory);
        }

        public void DisplayBoardDictionary(List<Dictionary<string, object>> events)
        {
            myDataGrid.ClearValue(ItemsControl.ItemsSourceProperty);
            myDataGrid.Items.Clear();
            myDataGrid.ItemsSource = events;
        }

        public void DisplayBoardByMonth(List<CalendarItemsByMonth> events)
        {

            myDataGrid.ClearValue(ItemsControl.ItemsSourceProperty);
            myDataGrid.Items.Clear();
            myDataGrid.ItemsSource = events;

        }

        public void DisplayBoardByCategory(List<CalendarItemsByCategory> events)
        {
            myDataGrid.ClearValue(ItemsControl.ItemsSourceProperty);
            myDataGrid.Items.Clear();
            myDataGrid.ItemsSource = events;
        }

        private void LoadCalendarItemColumns()
        {
            myDataGrid.Columns.Clear();
            myDataGrid.AutoGenerateColumns = false;
            var columnDate = new DataGridTextColumn();
            columnDate.Header = "StartDate";
            columnDate.Binding = new Binding("StartDateTime");
            myDataGrid.Columns.Add(columnDate);

            //Need to add StartTime
            //
            //
            //
            //

            var columnCategory = new DataGridTextColumn();
            columnCategory.Header = "Category";
            columnCategory.Binding = new Binding("Category");
            myDataGrid.Columns.Add(columnCategory);

            var columnDescription = new DataGridTextColumn();
            columnDescription.Header = "Description";
            columnDescription.Binding = new Binding("ShortDescription");
            myDataGrid.Columns.Add(columnDescription);

            var columnDuration = new DataGridTextColumn();
            columnDuration.Header = "Duration";
            columnDuration.Binding = new Binding("DurationInMinutes");
            myDataGrid.Columns.Add(columnDuration);


            var columnBusyTime = new DataGridTextColumn();
            columnBusyTime.Header = "BusyTime";
            columnBusyTime.Binding = new Binding("BusyTime");
            myDataGrid.Columns.Add(columnBusyTime);
        }

        private void LoadCalendarItemsByMonth()
        {
            myDataGrid.Columns.Clear();
            myDataGrid.AutoGenerateColumns = false;
            var columnMonth = new DataGridTextColumn();
            columnMonth.Header = "Month";
            columnMonth.Binding = new Binding("Month");
            myDataGrid.Columns.Add(columnMonth);

            var columnTotal = new DataGridTextColumn();
            columnTotal.Header = "Totals";
            columnTotal.Binding = new Binding("TotalBusyTime");
            myDataGrid.Columns.Add(columnTotal);
        }

        private void LoadCalendarItemsByCategory()
        {
            myDataGrid.Columns.Clear();
            myDataGrid.AutoGenerateColumns = false;
            var columnCategory = new DataGridTextColumn();
            columnCategory.Header = "Category";
            columnCategory.Binding = new Binding("Category");
            myDataGrid.Columns.Add(columnCategory);

            var columnTotal = new DataGridTextColumn();
            columnTotal.Header = "Totals";
            columnTotal.Binding = new Binding("TotalBusyTime");
            myDataGrid.Columns.Add(columnTotal);
        }
    }
}
