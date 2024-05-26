using Calendar;
using System;
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
            LoadCalendarItemColumns();
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
            //this.Close();
            //this.Close();
        }

        private void Btn_Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = Txt_Search.Text.ToLower();
            presenter.SearchCategories(searchText, myDataGrid.ItemsSource as List<CalendarItem>);
        }

        public void HighlightRow(CalendarItem ev)
        {
            myDataGrid.SelectedItem = ev;
        }

        public void ShowDbName(string DBName)
        {
            dbName.Text = DBName;
        }

        public void DisplayMessage(string message)
        {
            DisplayMessageBoxes.DisplayMessage(message);
        }

        public void ClosingConfirmation(object sender, CancelEventArgs e)
        {
            DisplayMessageBoxes.ClosingConfirmation(sender, e);
        }

        public void DisplayBoard(List<CalendarItem> events)
        {
            myDataGrid.ClearValue(ItemsControl.ItemsSourceProperty);
            myDataGrid.Items.Clear();
            myDataGrid.ItemsSource = events;
        }
        public void UpdateBoard()
        {
            FilterEvents();
        }

        public void ShowTypes(List<Category> categories)
        {
            Cmb_All_Categories.ItemsSource = categories;
        }

        private void Item_Click_Edit(object sender, RoutedEventArgs e)
        {
            // the selected object will always be a of the type that was originally put in the ItemsSource
            CalendarItem calItem = myDataGrid.SelectedItem as CalendarItem;
            UpdateEvent window = new UpdateEvent(presenter, calItem);
            window.Show();
            presenter.PopulateUpdateWindow(calItem);

        }
        private void Item_Click_Delete(object sender, RoutedEventArgs e)
        {
            CalendarItem? calItem = myDataGrid.SelectedItem as CalendarItem;
            presenter.DeleteEvent(calItem);
        }
        private void Item_DoubleClick(object sender, RoutedEventArgs e)
        {
            // the selected object will always be a of the type that was originally put in the ItemsSource
            CalendarItem? calItem = myDataGrid.SelectedItem as CalendarItem;
            UpdateEvent window = new UpdateEvent(presenter, calItem);
            window.Show();
            presenter.PopulateUpdateWindow(calItem);

        }
        


        private void startDateChanged(object sender, RoutedEventArgs e)
        {
            FilterEvents();

        }

        private void endDateChanged(object sender, RoutedEventArgs e)
        {
            FilterEvents();
        }

        private void FilterByCategory(object sender, RoutedEventArgs e)
        {
            FilterEvents();
        }

        private void SelectsCategory(object sender, RoutedEventArgs e)
        {
            FilterEvents();
        }

        private void GetAllEventsByMonth(object sender, RoutedEventArgs e)
        {
            FilterEvents();
        }

        private void GetAllEventsByCategory(object sender, RoutedEventArgs e)
        {   
            FilterEvents();
        }

        private void FilterEvents()
        {
            //If they are not checked they are false
            bool filterCategory = Ctb_FilterByCategory.IsChecked ?? false;
            bool eventsByMonth = Ctb_Month.IsChecked ?? false;
            bool eventsByCategory = Ctb_Category.IsChecked ?? false;

            //Checks which filters are checked and loads the columns accordingly
            if ((bool)Ctb_Category.IsChecked && (bool)Ctb_Month.IsChecked)
            {
                LoadCalendarItemsByMonthAndCategory();
            }
            else if ((bool)Ctb_Category.IsChecked)
            {
                LoadCalendarItemsByCategory();
            }
            else if ((bool)Ctb_Month.IsChecked)
            {
                LoadCalendarItemsByMonth();
            }
            else
            {
                LoadCalendarItemColumns();
            }

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
            columnDate.Binding.StringFormat = "dd/MM/yyyy";
            myDataGrid.Columns.Add(columnDate);

            var columnStartTime = new DataGridTextColumn();
            columnStartTime.Header = "StartTime";
            columnStartTime.Binding = new Binding("StartDateTime");
            columnStartTime.Binding.StringFormat = "HH:mm:ss";
            myDataGrid.Columns.Add(columnStartTime);

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

        private void LoadCalendarItemsByMonthAndCategory()
        {
            myDataGrid.Columns.Clear();
            myDataGrid.AutoGenerateColumns = false;

            var columnMonth = new DataGridTextColumn();
            columnMonth.Header = "Month";
            columnMonth.Binding = new Binding("[Month]");
            myDataGrid.Columns.Add(columnMonth);

            var columnBirthdays = new DataGridTextColumn();
            columnBirthdays.Header = "Birthdays";
            columnBirthdays.Binding = new Binding("[Birthdays]");
            myDataGrid.Columns.Add(columnBirthdays);

            var columnCanadianHolidays = new DataGridTextColumn();
            columnCanadianHolidays.Header = "Canadian Holidays";
            columnCanadianHolidays.Binding = new Binding("[Canadian Holidays]");
            myDataGrid.Columns.Add(columnCanadianHolidays);

            var columnFun = new DataGridTextColumn();
            columnFun.Header = "Fun";
            columnFun.Binding = new Binding("[Fun]");
            myDataGrid.Columns.Add(columnFun);

            var columnHomework = new DataGridTextColumn();
            columnHomework.Header = "Homework";
            columnHomework.Binding = new Binding("[Homework]");
            myDataGrid.Columns.Add(columnHomework);

            var columnMedical = new DataGridTextColumn();
            columnMedical.Header = "Medical";
            columnMedical.Binding = new Binding("[Medical]");
            myDataGrid.Columns.Add(columnMedical);

            var columnOnCall = new DataGridTextColumn();
            columnOnCall.Header = "On call";
            columnOnCall.Binding = new Binding("[On call]");
            myDataGrid.Columns.Add(columnOnCall);

            var columnSchool = new DataGridTextColumn();
            columnSchool.Header = "School";
            columnSchool.Binding = new Binding("[School]");
            myDataGrid.Columns.Add(columnSchool);

            var columnSleep = new DataGridTextColumn();
            columnSleep.Header = "Sleep";
            columnSleep.Binding = new Binding("[Sleep]");
            myDataGrid.Columns.Add(columnSleep);

            var columnVacation = new DataGridTextColumn();
            columnVacation.Header = "Vacation";
            columnVacation.Binding = new Binding("[Vacation]");
            myDataGrid.Columns.Add(columnVacation);

            var columnWellnessDays = new DataGridTextColumn();
            columnWellnessDays.Header = "Wellness days";
            columnWellnessDays.Binding = new Binding("[Wellness days]");
            myDataGrid.Columns.Add(columnWellnessDays);

            var columnWork = new DataGridTextColumn();
            columnWork.Header = "Work";
            columnWork.Binding = new Binding("[Work]");
            myDataGrid.Columns.Add(columnWork);

            var columnWorking = new DataGridTextColumn();
            columnWorking.Header = "Working";
            columnWorking.Binding = new Binding("[Working]");
            myDataGrid.Columns.Add(columnWorking);

            var columnTotal = new DataGridTextColumn();
            columnTotal.Header = "Total Busy Time";
            columnTotal.Binding = new Binding("[TotalBusyTime]");
            myDataGrid.Columns.Add(columnTotal);
        }

        private void AddCollumn(string first, string second)
        {
            var column = new DataGridTextColumn();
            column.Header = first;
            column.Binding = new Binding(second);
            myDataGrid.Columns.Add(column);
        }

    }
}
