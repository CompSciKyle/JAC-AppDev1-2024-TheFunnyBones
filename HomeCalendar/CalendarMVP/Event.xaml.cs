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
    /// Interaction logic for Event.xaml
    /// </summary>
    public partial class Event : Window, ViewInterface
    {
        private readonly Presenter presenter;
        public Event()
        {

            InitializeComponent();
            presenter = new Presenter(this);
            Cmb_Types.ItemsSource = presenter.GetAllCategoryTypes();
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
           presenter.NewEvent(Txb_Date,)
        }
    }
}
