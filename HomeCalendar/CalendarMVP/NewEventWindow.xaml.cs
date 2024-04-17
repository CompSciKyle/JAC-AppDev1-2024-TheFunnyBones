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
    /// Interaction logic for NewEventWindow.xaml
    /// </summary>
    public partial class NewEventWindow : Window, ViewInterfaceForEventsAndCategories
    {
        private readonly Presenter presenter;
        public NewEventWindow(Presenter p)
        {
            InitializeComponent();
            presenter = p;
            //presenter.RegisterEventView(this);
            Cmb_Categories.ItemsSource = presenter.GetAllCategories();
        }

        public void ClosingConfirmation()
        {
            throw new NotImplementedException();
        }

        public void DisplayDB()
        {
            throw new NotImplementedException();
        }

        public void DisplayMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowCategory()
        {
            throw new NotImplementedException();
        }

        public void ShowTypes()
        {
            Cmb_Categories.ItemsSource = presenter.GetAllCategories();
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            double duration = double.Parse(Txb_Duration.Text);
            Category category = Cmb_Types.SelectedItem as Category;
            DateTime startDate = Dtp_Date.Text;
            presenter.NewEvent(startDate, category, duration, Txb_Details.Text);
        }
    }
}