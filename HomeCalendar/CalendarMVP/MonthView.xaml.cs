using Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            //presenter.DisplayAll();
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
            bool filterCategory = Ctb_FilterByCategory.IsChecked ?? false;   
            bool eventsByMonth = Ctb_Month.IsChecked ?? false;
            bool eventsByCategory = Ctb_Month.IsChecked ?? false;
            presenter.PopulateDataGrid(Dtb_StartDate.Text, Dtb_EndDate.Text, filterCategory, (Category)Cmb_All_Categories.SelectedItem, eventsByMonth, eventsByCategory);
        }

        private void endDateChanged(object sender, RoutedEventArgs e)
        {
            bool filterCategory = Ctb_FilterByCategory.IsChecked ?? false;
            bool eventsByMonth = Ctb_Month.IsChecked ?? false;
            bool eventsByCategory = Ctb_Month.IsChecked ?? false;
            presenter.PopulateDataGrid(Dtb_StartDate.Text, Dtb_EndDate.Text, filterCategory, (Category)Cmb_All_Categories.SelectedItem, eventsByMonth, eventsByCategory);
        }

        private void FilterByCategory(object sender, RoutedEventArgs e)
        {
            bool filterCategory = Ctb_FilterByCategory.IsChecked ?? false;
            bool eventsByMonth = Ctb_Month.IsChecked ?? false;
            bool eventsByCategory = Ctb_Month.IsChecked ?? false;
            presenter.PopulateDataGrid(Dtb_StartDate.Text, Dtb_EndDate.Text, filterCategory, (Category)Cmb_All_Categories.SelectedItem, eventsByMonth, eventsByCategory);
        }

         private void SelectsCategory(object sender, RoutedEventArgs e)
        {
            bool filterCategory = Ctb_FilterByCategory.IsChecked ?? false;
            bool eventsByMonth = Ctb_Month.IsChecked ?? false;
            bool eventsByCategory = Ctb_Month.IsChecked ?? false;
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
    }
}
