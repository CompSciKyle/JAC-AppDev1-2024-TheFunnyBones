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
    }
}
