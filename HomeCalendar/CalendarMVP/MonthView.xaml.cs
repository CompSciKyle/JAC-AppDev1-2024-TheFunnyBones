using Calendar;
using System;
using System.Collections.Generic;
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
    public partial class MonthView : Window
    {
        public MonthView()
        {
            InitializeComponent();
        }
        private void BtnClickNewEvent(object sender, RoutedEventArgs e)
        {
            Event eventView = new Event();
            eventView.Show();   
        }

        private void BtnClickNewCategory(object sender, RoutedEventArgs e)
        {
            NewCategory catView = new NewCategory();
            catView.Show();
        }
    }
}
