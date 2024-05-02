using Calendar;
using CalendarMVP;
using System.ComponentModel;

namespace PresenterTest
{
    public class TestDBView : ViewInterfaceForDatabaseConnection
    {
        public bool calledDisplayDB = false;
        public bool calledDisplayError = false;
        public string displayedMessage = "";


        public void DisplayDB()
        {
            calledDisplayDB = true;
        }

        public void DisplayError(string message)
        {
            calledDisplayError = true;
            displayedMessage = message;

        }
    }

    public class TestViewCalendar : ViewInterfaceForCalendar
    {
        public bool calledDisplayMessage = false;
        public bool calledShowDBName = false;
        public string dbName = "";

        public void DisplayMessage(string message)
        {
            calledDisplayMessage = true;
        }

        public void ShowDbName(string DBName)
        {
            calledShowDBName = true;
            dbName = DBName;
        }
    }

    public class TestViewEvent : ViewInterfaceForEvents
    {
        public bool calledDisplayMessage = false;
        public bool calledShowDBName = false;
        public bool calledDisplayDB = false;
        public bool calledShowTypesCategory = false;
        public int countOfAllCategories = 0;


        public void DisplayMessage(string message)
        {
            calledDisplayMessage = true;
        }

        public void ShowDbName(string DBName)
        {
            calledShowDBName = true;
        }

        public void DisplayDB()
        {
            calledDisplayDB = true;
        }

        public void ShowTypes(List<Category> allCategories)
        {
            calledShowTypesCategory = true;
            countOfAllCategories = allCategories.Count;
        }

        public void ClosingConfirmation()
        {
            throw new NotImplementedException();
        }
    }

    public class TestViewCategory : ViewInterfaceForCategories
    {
        public bool calledDisplayMessage = false;
        public bool calledShowDBName = false;
        public bool calledDisplayDB = false;
        public bool calledShowTypesCategoryTypes = false;
        public int countOfCategoriesTypes = 0;


        public void DisplayMessage(string message)
        {
            calledDisplayMessage = true;
        }

        public void ShowDbName(string DBName)
        {
            calledShowDBName = true;
        }

        public void DisplayDB()
        {
            calledDisplayDB = true;
        }

        public void ShowTypes(List<Category.CategoryType> allCategoryTypes)
        {
            calledShowTypesCategoryTypes = true;
            countOfCategoriesTypes = allCategoryTypes.Count;
        }
    }


    public class UnitTest1
    {
        [Fact]
        public void TestConnectToDB_NewPath()
        {
            // Arrange
            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            string filePath = Path.GetTempPath(); //Creates a unique temporary file name and returns the full path to that file.
            string fileName = "databaseTest.db";
            bool newDB = true;
            view.calledDisplayDB = false;

            // Act
            presenter.ConnectToDB(filePath, fileName, newDB);

            // Assert
            Assert.True(view.calledDisplayDB);
        }

        [Fact]
        public void TestConnectToDB_NewPath_DoesntExist_Error()
        {
            // Arrange
            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            string filePath = "C:\\NonExistent\\Path\\Im\\Hoping\\That\\You\\Dont\\Have";
            string fileName = "databaseTest.db";
            bool newDB = true;
            view.calledDisplayError = false;
            view.displayedMessage = string.Empty;
            // Act
            presenter.ConnectToDB(filePath, fileName, newDB);

            // Assert
            Assert.True(view.calledDisplayError);
            Assert.Equal("Path does not exist", view.displayedMessage);

        }

        [Fact]
        public void TestConnectToDB_ExistingPath()
        {
            // Arrange
            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            var projectDirectory = Directory.GetCurrentDirectory(); //Gets the current directory of the project
            string filePath = projectDirectory;
            string fileName = "testDBInput.db";
            bool newDB = true;
            view.calledDisplayDB = false;

            // Act
            presenter.ConnectToDB(filePath, fileName, newDB);

            // Assert
            Assert.True(view.calledDisplayDB);
        }

        [Fact]
        public void TestRegisterNewView_ForCalendar()
        {
            // Arrange
            TestDBView dbView = new TestDBView();
            TestViewCalendar calendarView = new TestViewCalendar();
            var presenter = new Presenter(dbView);
            var projectDirectory = Directory.GetCurrentDirectory(); //Gets the current directory of the project
            string filePath = projectDirectory;
            string fileName = "testDBInput.db";
            bool newDB = true;
            presenter.ConnectToDB(filePath, fileName, newDB);
            calendarView.calledShowDBName = false;
            calendarView.dbName = string.Empty;

            // Act
            presenter.RegisterNewView(calendarView);

            // Assert
            Assert.True(calendarView.calledShowDBName);
            Assert.Equal("testDBInput", calendarView.dbName);

        }

        [Fact]
        public void TestRegisterNewView_ForEvents()
        {
            // Arrange
            TestDBView dbView = new TestDBView();
            TestViewEvent eventView = new TestViewEvent();
            var presenter = new Presenter(dbView);
            var projectDirectory = Directory.GetCurrentDirectory(); //Gets the current directory of the project
            string filePath = projectDirectory;
            string fileName = "testDBInput.db";
            bool newDB = true;
            presenter.ConnectToDB(filePath, fileName, newDB);
            eventView.calledShowDBName = false;
            eventView.countOfAllCategories = 0;

            // Act
            presenter.RegisterNewView(eventView);

            // Assert
            Assert.True(eventView.calledShowDBName);
            Assert.Equal(12, eventView.countOfAllCategories);

        }

        [Fact]
        public void TestRegisterNewView_ForCategory()
        {
            // Arrange
            TestDBView dbView = new TestDBView();
            TestViewCategory categoryView = new TestViewCategory();
            var presenter = new Presenter(dbView);
            var projectDirectory = Directory.GetCurrentDirectory(); //Gets the current directory of the project
            string filePath = projectDirectory;
            string fileName = "testDBInput.db";
            bool newDB = true;
            presenter.ConnectToDB(filePath, fileName, newDB);
            categoryView.calledShowDBName = false;

            // Act
            presenter.RegisterNewView(categoryView);

            // Assert
            Assert.True(categoryView.calledShowDBName);
            Assert.Equal(4, categoryView.countOfCategoriesTypes);

        }


        [Fact]
        public void TestNewCategory_ValidType()
        {
            // Arrange

            //Setting up Views and Presenter
            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            TestViewCategory categoryView = new TestViewCategory();
            TestViewCalendar viewCalendar = new TestViewCalendar();

            //Creates a new DB
            var projectDirectory = Directory.GetCurrentDirectory(); //Gets the current directory of the project
            string filePath = projectDirectory;
            string fileName = "databaseTest.db";
            bool newDB = true;
            presenter.ConnectToDB(filePath, fileName, newDB);

            //Input fields for category
            Category.CategoryType type = Category.CategoryType.Event;
            var description = "Some description";

            //Resetting variables
            categoryView.calledDisplayDB = false;
            viewCalendar.calledDisplayMessage = false;

            //Registering the new view
            presenter.RegisterNewView(categoryView);
            presenter.RegisterNewView(viewCalendar);

            // Act
            presenter.NewCategory(type, description, false);

            // Assert
            Assert.True(categoryView.calledDisplayDB);


            Assert.True(viewCalendar.calledDisplayMessage);
        }

        [Fact]
        public void TestNewEvent_ValidData()
        {
            // Arrange
            //Setting up Views and Presenter
            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            TestViewEvent eventView = new TestViewEvent();
            TestViewCalendar viewCalendar = new TestViewCalendar();

            //Create new db
            string filePath = Path.GetTempPath(); //Creates a unique temporary file name and returns the full path to that file.
            string fileName = "databaseTest.db";
            bool newDB = true;
            presenter.ConnectToDB(filePath, fileName, newDB);

            //Input Fields for event
            var startDateTime = DateTime.Now.AddMonths(1);
            string startDate = startDateTime.ToString();
            string durationInMinutes = "60";
            var details = "Some event details";
            List<Category> mycategories = presenter.GetAllCategories();



            //Resetting variables
            eventView.calledDisplayDB = false;
            viewCalendar.calledDisplayMessage = false;

            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            // Act
            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);

            // Assert
            Assert.True(eventView.calledDisplayDB);
            Assert.True(viewCalendar.calledDisplayMessage);
        }

        [Fact]
        public void TestNewEvent_Invalid_DurationData()
        {
            // Arrange
            //Setting up Views and Presenter
            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            TestViewEvent eventView = new TestViewEvent();
            TestViewCalendar viewCalendar = new TestViewCalendar();

            //Create new db
            string filePath = Path.GetTempPath(); //Creates a unique temporary file name and returns the full path to that file.
            string fileName = "databaseTest.db";
            bool newDB = true;
            presenter.ConnectToDB(filePath, fileName, newDB);

            //Input Fields for event
            var startDateTime = DateTime.Now;
            string startDate = startDateTime.ToString();
            string durationInMinutes = "-60";
            var details = "Some event details";
            Category categoryId = new Category(20, "here");

            //Resetting variables
            eventView.calledDisplayDB = false;
            viewCalendar.calledDisplayMessage = false;
            eventView.calledDisplayMessage = false;

            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            // Act
            presenter.NewEvent(startDate, categoryId, durationInMinutes, details);

            // Assert
            Assert.False(eventView.calledDisplayDB);
            Assert.False(viewCalendar.calledDisplayMessage);
            Assert.True(eventView.calledDisplayMessage);
        }

        [Fact]
        public void TestNewEvent_Invalid_Date_Error()
        {
            // Arrange
            //Setting up Views and Presenter
            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            TestViewEvent eventView = new TestViewEvent();
            TestViewCalendar viewCalendar = new TestViewCalendar();

            //Create new db
            string filePath = Path.GetTempPath(); //Creates a unique temporary file name and returns the full path to that file.
            string fileName = "databaseTest.db";
            bool newDB = true;
            presenter.ConnectToDB(filePath, fileName, newDB);

            //Input Fields for event
            var startDateTime = DateTime.Now.AddMonths(-100);
            string startDate = startDateTime.ToString();
            Category categoryId = new Category(20, "shouldn't be here");


            var durationInMinutes = "60";
            var details = "Some event details";

            //Resetting variables
            eventView.calledDisplayDB = false;
            viewCalendar.calledDisplayMessage = false;
            eventView.calledDisplayMessage = false;

            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            // Act
            presenter.NewEvent(startDate, categoryId, durationInMinutes, details);

            // Assert
            Assert.False(eventView.calledDisplayDB);
            Assert.False(viewCalendar.calledDisplayMessage);
            Assert.True(eventView.calledDisplayMessage);
        }

        [Fact]
        public void TestNewEvent_Invalid_Category_Error()
        {
            // Arrange
            //Setting up Views and Presenter
            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            TestViewEvent eventView = new TestViewEvent();
            TestViewCalendar viewCalendar = new TestViewCalendar();

            //Create new db
            string filePath = Path.GetTempPath(); //Creates a unique temporary file name and returns the full path to that file.
            string fileName = "databaseTest.db";
            bool newDB = true;
            presenter.ConnectToDB(filePath, fileName, newDB);

            //Input Fields for event
            var startDateTime = DateTime.Now.AddMonths(-100);
            string newDate = startDateTime.ToString();
            Category.CategoryType nonExistent = new Category.CategoryType();
            Category categoryId = new Category(20, "shouldn't be here", nonExistent);

            string durationInMinutes = "60";
            var details = "Some event details";

            //Resetting variables
            eventView.calledDisplayDB = false;
            viewCalendar.calledDisplayMessage = false;
            eventView.calledDisplayMessage = false;

            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            // Act
            presenter.NewEvent(newDate, categoryId, durationInMinutes, details);

            // Assert
            Assert.False(eventView.calledDisplayDB);
            Assert.False(viewCalendar.calledDisplayMessage);
            Assert.True(eventView.calledDisplayMessage);
        }

        [Fact]
        public void TestMonthView_DisplayAll()
        {
            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            TestViewCalendar viewCalendar = new TestViewCalendar();

            string filePath = Path.GetTempPath(); //Creates a unique temporary file name and returns the full path to that file.
            string fileName = "databaseTest.db";
            bool newDB = true;
            presenter.ConnectToDB(filePath, fileName, newDB);

            Assert.True(viewCalendar.calledDisplayMessage);
        }

        [Fact]
        public void 
    }
}