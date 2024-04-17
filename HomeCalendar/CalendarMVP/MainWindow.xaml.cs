using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.XPath;
using Calendar;
using Microsoft.Win32;

namespace CalendarMVP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterfaceForDatabaseConnection
    {
        public MainWindow()
        {
            InitializeComponent();
            Presenter presenter = new Presenter(this);
        }

        public void Btn_Click_File_Explore(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Database File | *.db";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName; //Gets the file path
                string[] arrayOfPath = filePath.Split('\\');
                int lastIndext = arrayOfPath.Length - 1;
                string fileName = arrayOfPath[lastIndext];
                filePath = "";
                for (int i = 0; i < lastIndext; i++)
                {
                    filePath += arrayOfPath[i] + '\\';
                }
                presenter.ExsistingDB(filePath, fileName);
            }
        }
    }
}