using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Xml.Linq;
using Calendar;


namespace CalendarMVP
{
    /// <summary>
    /// Interaction logic for NewEventWindow.xaml
    /// </summary>
    public partial class NewEventWindow : Window, ViewInterfaceForEvents
    {
        private readonly Presenter presenter;
        public NewEventWindow(Presenter p)
        {
            InitializeComponent();
            presenter = p;
            presenter.RegisterNewView(this);
        }

        public void DisplayDB()
        {
            this.Close();
        }

        public void DisplayMessage(string message)
        {
           MessageBox.Show(message);
        }


        public void ShowTypes(List<Category> categories)
        {
            Cmb_Categories.ItemsSource = categories;
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                string dateTimeString = $"{Dtp_Date.Text} {Txb_Time_Hour.Text}:{Txb_Time_Minutes.Text}:{Txb_Time_Second.Text}";

                double duration = double.Parse(Txb_Duration.Text);
                int categoryId = (int)Cmb_Categories.SelectedItem;
                string dateTimeString = $"{Dtp_Date.Text} {Txb_Time_Hour.Text}:{Txb_Time_Minutes.Text}:{Txb_Time_Second.Text}";
                DateTime startDate;
                DateTime.TryParseExact(dateTimeString, "yyyy-MM-dd H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                presenter.NewEvent(startDate, categoryId, duration, Txb_Details.Text);
                Console.WriteLine("dateTimeString: " + dateTimeString);

            }catch(Exception ex) 
            {
                DisplayMessage("Failed To create a event: " + ex.Message);
            }
        }

        public void ShowDbName(string DBName)
        {
            Txb_DBName.Text = DBName;
        }

        public void Btn_Close(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Are you sure you would like to exit the window? Any unsaved changes will be lost."; // Create a class that makes this code a static method so anywhere that needs to use it will have access to it. 
            string messageBoxCaption = "Exit Window?";
            MessageBoxButton messageBoxButton = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Exclamation;
            MessageBoxResult result;
            result = MessageBox.Show(messageBoxText, messageBoxCaption, messageBoxButton, messageBoxImage);

            if (result == MessageBoxResult.Yes) 
            { 
                this.Close();
            }
        }
    }
}