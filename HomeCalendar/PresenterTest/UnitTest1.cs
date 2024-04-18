using CalendarMVP;
namespace PresenterTest
{
    public class TestView : ViewInterfaceForCalendar, ViewInterfaceForDatabaseConnection, ViewInterfaceForEventsAndCategories
    {
        public bool calledClosingConfirmation = false;
        public bool calledDisplayDB = false;
        public bool calledDisplayError = false;
        public bool calledDisplayMessage = false;
        public bool calledShowDBName = false;
        public bool calledShowTypes = false;
        public bool calledShowTypesCategory = false;
        public bool calledShowTypesCategoryTypes = false;
        public void ClosingConfirmation()
        {
            throw new NotImplementedException();
        }

        public void DisplayDB()
        {
            throw new NotImplementedException();
        }

        public void DisplayError(string message)
        {
            throw new NotImplementedException();
        }

        public void DisplayMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowDbName(string DBName)
        {
            throw new NotImplementedException();
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