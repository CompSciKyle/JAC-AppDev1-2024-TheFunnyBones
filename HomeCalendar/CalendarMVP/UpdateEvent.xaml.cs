using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
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
    /// Interaction logic for UpdateEvent.xaml
    /// </summary>
    public partial class UpdateEvent : Window, ViewInterfaceForUpdatingEvent
    {
        private readonly Presenter presenter;
        private readonly CalendarItem selectedItem;
        public UpdateEvent(Presenter p, CalendarItem calItem)
        {
            InitializeComponent();
            presenter = p;
            presenter.RegisterNewView(this);
            selectedItem = calItem;
            presenter.PopulateUpdateWindow(selectedItem);
        }
        public void DisplayDB()
        {
            this.Hide();
        }

        public void DisplayMessage(string message)
        {
            DisplayMessageBoxes.DisplayMessage(message);
        }


        public void ShowTypes(List<Category> categories)
        {
            Cmb_Categories.ItemsSource = categories;
        }
        public void ShowDbName(string DBName)
        {
            Txb_DBName.Text = DBName;
        }

        public void ClosingConfirmation(object sender, CancelEventArgs e)
        {
            DisplayMessageBoxes.ClosingConfirmation(sender, e);
        }
        public void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        public void Btn_Update(object sender, RoutedEventArgs e)
        {
            string dateTimeString = $"{Dtp_Date.Text} {Txb_Time_Hour.Text}:{Txb_Time_Minutes.Text}:{Txb_Time_Second.Text}";
            presenter.UpdateEvent(selectedItem, dateTimeString, (Category)Cmb_Categories.SelectedItem, Txb_Duration.Text, Txb_Details.Text);
        }
        public void Btn_Delete(object sender, RoutedEventArgs e)
        {
            presenter.DeleteEvent(selectedItem);
            this.Hide();
        }
        void PopulateFields(string startDateTime, string startDateHour, string startDateMinute, string startDateSecond, Category category, string durationInMinutes, string details)
        {

        }

    }
}
