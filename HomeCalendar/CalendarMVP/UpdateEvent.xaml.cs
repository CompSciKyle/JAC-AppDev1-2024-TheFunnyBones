﻿using System;
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
using Calendar;

namespace CalendarMVP
{
    /// <summary>
    /// Interaction logic for UpdateEvent.xaml
    /// </summary>
    public partial class UpdateEvent : Window, ViewInterfaceForUpdatingEvent
    {
        private readonly Presenter presenter;
        public UpdateEvent(Presenter p, CalendarItem calItem)
        {
            InitializeComponent();
            presenter = p;
            presenter.RegisterNewView(this);
            presenter.PopulateUpdateWindow(calItem);
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
            this.Hide();
        }
        public void Btn_Delete(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        void PopulateFields(string startDateTime, string startDateHour, string startDateMinute, string startDateSecond, Category category, string durationInMinutes, string details)
        {
            Dtp_Date.DataContext = startDateTime;
            Txb_Time_Hour.Text = startDateHour;
            Txb_Time_Minutes.Text = startDateMinute;
            Txb_Time_Second.Text = startDateSecond;
            Txb_Duration.Text = durationInMinutes;
            Txb_Details.Text = details;
            Cmb_Categories.SelectedItem = category;

        }

        void ViewInterfaceForUpdatingEvent.PopulateFields(string startDateTime, string startDateHour, string startDateMinute, string startDateSecond, Category category, string durationInMinutes, string details)
        {
            throw new NotImplementedException();
        }
    }
}
