using Calendar;
using CalendarMVP;
using System.ComponentModel;

namespace PresenterTest
{
    public class TestView : ViewInterfaceForCalendar, ViewInterfaceForDatabaseConnection, ViewInterfaceForCategories, ViewInterfaceForEvents
    {
        public bool calledClosingConfirmation = false;
        public bool calledDisplayDB = false;
        //public bool 
        public bool calledDisplayError = false;
        public bool calledDisplayMessage = false;
        public bool calledShowDBName = false;
        public bool calledShowTypesCategoryTypes = false;
        public bool calledShowTypesCategory = false;
        public void ClosingConfirmation()
        {
            calledClosingConfirmation = true;
        }

        public void ClosingConfirmation(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void DisplayDB()
        {
            calledDisplayDB = true;
        }

        public void DisplayDB(string DBName)
        {
            calledDisplayDB = true;
        }

        public void DisplayError(string message)
        {
            calledDisplayError = true;
        }

        public void DisplayMessage(string message)
        {
            calledDisplayMessage = true;
        }

        public void ShowDbName(string DBName)
        {
            calledShowDBName = true;
        }

        public void ShowTypes(List<Category.CategoryType> allCategoryTypes)
        {
            calledShowTypesCategoryTypes = true;
        }

        public void ShowTypes(List<Category> allCategories)
        {
            calledClosingConfirmation = true;
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }
    }
}