using System;
using System.Collections.Generic;
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
using Calendar;


namespace CalendarMVP
{
    /// <summary>
    /// Interaction logic for NewEventWindow.xaml
    /// </summary>
    public partial class NewEventWindow : Window, ViewInterfaceForEventsAndCategories
    {
        private readonly Presenter presenter;
        private MonthView mainWindow;
        public NewEventWindow(Presenter p, MonthView main)
        {
            InitializeComponent();
            presenter = p;
            //presenter.RegisterEventView(this);
            Cmb_Categories.ItemsSource = presenter.GetAllCategories();
            mainWindow = main;
        }

        public void ClosingConfirmation()
        {
            throw new NotImplementedException();
        }

        public void DisplayDB()
        {
            this.Close();
            mainWindow.Show();
        }

        public void DisplayMessage(string message)
        {
           MessageBox.Show(message);
        }


        public void ShowTypes()
        {
            Cmb_Categories.ItemsSource = presenter.GetAllCategories();
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            try
            {

            double duration = double.Parse(Txb_Duration.Text);
            int categoryId = (int)Cmb_Categories.SelectedValue;
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
    }
}