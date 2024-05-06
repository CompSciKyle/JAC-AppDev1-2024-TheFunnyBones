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
        public bool calledDisplayBoard = false;
        public bool calledDisplayBoardByMonth = false;
        public bool calledDisplayBoardByCategory = false;
        public bool calledDisplayBoardByDictionary = false;
        public bool calledShowTypes = false;
        public bool calledUpdateBoard = false;

        public List<CalendarItem> items = new List<CalendarItem>();
        public List<CalendarItemsByCategory> categoryItems = new List<CalendarItemsByCategory>();
        public List<CalendarItemsByMonth> MonthItems = new List<CalendarItemsByMonth>();
        public  List<Dictionary<string, object>> DictionaryItems = new List<Dictionary<string, object>>();
        public List<Category> allCategories = new List<Category>();


        public void DisplayBoard(List<CalendarItem> events)
        {
            calledDisplayBoard = true;
            items = events;

        }

        public void DisplayBoardByCategory(List<CalendarItemsByCategory> events)
        {
            calledDisplayBoardByCategory = true;
            categoryItems = events;
        }

        public void DisplayBoardByMonth(List<CalendarItemsByMonth> events)
        {
            calledDisplayBoardByMonth = true;
            MonthItems = events;
        }

        public void DisplayBoardDictionary(List<Dictionary<string, object>> events)
        {
            calledDisplayBoardByDictionary = true;
            DictionaryItems = events;
        }

        public void DisplayMessage(string message)
        {
            calledDisplayMessage = true;
        }

        public void ShowDbName(string DBName)
        {
            calledShowDBName = true;
            dbName = DBName;
        }

        public void ShowTypes(List<Category> categories)
        {
            calledShowTypes = true;
            allCategories = categories;
        }

        public void UpdateBoard()
        {
            calledUpdateBoard = true;
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

        public void ClosingConfirmation(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdateView : ViewInterfaceForUpdatingEvent
    {
        public bool calledDisplayDB = false;
        public bool calledDisplayMessage = false;
        public bool calledPopulateFields = false;
        public bool ShowDBName = false;
        public bool calledShowTypes = false;

        public void DisplayDB()
        {
            calledDisplayDB = true;
        }

        public void DisplayMessage(string message)
        {
            calledDisplayMessage = true;
        }

        public void PopulateFields(DateTime startDateTime, string startDateHour, string startDateMinute, string startDateSecond, Category category, string durationInMinutes, string details)
        {
            calledDisplayMessage = true;
        }

        public void ShowDbName(string DBName)
        {
            ShowDBName = true;
        }

        public void ShowTypes(List<Category> allCategories)
        {
            calledShowTypes = true;
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

            presenter.RegisterNewView(viewCalendar);

            presenter.DisplayAll();

            Assert.True(viewCalendar.calledDisplayBoard);
        }

        [Fact]
        public void TestDelete_DeleteEvent()
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
        [Fact]
        public void TestPopulateCalendarItems_ValidData()
        {
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

            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);

            // Act
            presenter.PopulateDataGrid("", "", false, mycategories[1], false, false);

            // Assert
            Assert.True(viewCalendar.calledDisplayBoard);
            Assert.Equal(1, viewCalendar.items.Count);
        }

        [Fact]
        public void TestPopulateCalendarItems_InvalidData()
        {
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

            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);

            // Act
            presenter.PopulateDataGrid(null, null, false, mycategories[1], false, false);

            // Assert
            Assert.True(viewCalendar.calledDisplayBoard);
            Assert.Equal(0, viewCalendar.items.Count);
        }


        [Fact]
        public void TestPopulateCalendarItemsByMonth_ValidData()
        {
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
            viewCalendar.calledDisplayBoardByMonth = false;
            viewCalendar.MonthItems.Clear();

            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);

            // Act
            presenter.PopulateDataGrid("", "", false, mycategories[1], true, false);

            // Assert
            Assert.True(viewCalendar.calledDisplayBoardByMonth);
            Assert.Equal(1, viewCalendar.MonthItems.Count);
        }


        [Fact]
        public void TestPopulateCalendarItemsByMonth_InvalidData()
        {
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
            viewCalendar.calledDisplayBoardByMonth = false;
            viewCalendar.MonthItems.Clear();

            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);

            // Act
            presenter.PopulateDataGrid(null, null, false, mycategories[1], true, false);

            // Assert
            Assert.True(viewCalendar.calledDisplayBoardByMonth);
            Assert.Equal(0, viewCalendar.MonthItems.Count);
        }

        [Fact]
        public void TestPopulateCalendarItemsByCategory_ValidData()
        {
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
            viewCalendar.calledDisplayBoardByCategory = false;
            viewCalendar.categoryItems.Clear();

            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);

            // Act
            presenter.PopulateDataGrid("", "", false, mycategories[1], false, true);

            // Assert
            Assert.True(viewCalendar.calledDisplayBoardByCategory);
            Assert.Equal(1, viewCalendar.categoryItems.Count);
        }

        [Fact]
        public void TestPopulateCalendarItemsByCategory_InvalidData()
        {
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
            viewCalendar.calledDisplayBoardByCategory = false;
            viewCalendar.categoryItems.Clear();

            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);

            // Act
            presenter.PopulateDataGrid(null, null, false, mycategories[1], false, true);

            // Assert
            Assert.True(viewCalendar.calledDisplayBoardByCategory);
            Assert.Equal(0, viewCalendar.categoryItems.Count);
        }

        [Fact]
        public void TestPopulateCalendarItemsByCategoryAndMonth_ValidData()
        {
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
            viewCalendar.calledDisplayBoardByDictionary = false;
            viewCalendar.DictionaryItems.Clear();


            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);

            // Act
            presenter.PopulateDataGrid("", "", false, mycategories[1], true, true);

            // Assert
            Assert.True(viewCalendar.calledDisplayBoardByDictionary);
            Assert.Equal(2, viewCalendar.DictionaryItems.Count);
        }

        [Fact]
        public void TestPopulateCalendarItemsByCategoryAndMonth_InvalidData()
        {
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
            viewCalendar.calledDisplayBoardByDictionary = false;
            viewCalendar.DictionaryItems.Clear();


            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);

            // Act
            presenter.PopulateDataGrid(null, null, false, mycategories[1], true, true);

            // Assert
            Assert.True(viewCalendar.calledDisplayBoardByDictionary);
            Assert.Equal(1, viewCalendar.DictionaryItems.Count);
        }


        [Fact]
        public void TestUpdateEvent()
        {

            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            TestViewEvent ev = new TestViewEvent();
            TestViewCalendar viewCalendar = new TestViewCalendar();
            UpdateView updatewindow = new UpdateView();

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

            presenter.RegisterNewView(ev);
            presenter.RegisterNewView(updatewindow);
            presenter.RegisterNewView(viewCalendar);
            
            List<Category> mycategories = presenter.GetAllCategories();
            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);
            presenter.PopulateDataGrid("", "", false, mycategories[1], false, false);

            //Act

            presenter.UpdateEvent(viewCalendar.items[0], new DateTime(1900, 1, 1).ToString(), mycategories[0], "60", "hi");

            //Assert
            Assert.True(updatewindow.calledDisplayDB);
            Assert.True(viewCalendar.calledUpdateBoard);
            Assert.True(viewCalendar.calledDisplayMessage);

        }

        [Fact]
        public void TestUpdateInvalidEvent()
        {

            TestDBView view = new TestDBView();
            Presenter presenter = new Presenter(view);
            TestViewEvent ev = new TestViewEvent();
            TestViewCalendar viewCalendar = new TestViewCalendar();
            UpdateView updatewindow = new UpdateView();

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

            presenter.RegisterNewView(ev);
            presenter.RegisterNewView(updatewindow);
            presenter.RegisterNewView(viewCalendar);

            List<Category> mycategories = presenter.GetAllCategories();
            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);
            presenter.PopulateDataGrid("", "", false, mycategories[1], false, false);

            updatewindow.calledDisplayMessage = false;

            //Act

            presenter.UpdateEvent(viewCalendar.items[0], new DateTime(1900, 1, 1).ToString(), mycategories[2], "-60", "hi");

            //Assert
            Assert.True(updatewindow.calledDisplayMessage);
           

        }


            //Resetting variables
            eventView.calledDisplayDB = false;
            viewCalendar.calledDisplayMessage = false;

            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            // Act
            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);
            presenter.PopulateDataGrid("", "", false, mycategories[1], false, false);

            presenter.DeleteEvent(viewCalendar.items[0]);

            Assert.True(viewCalendar.calledUpdateBoard);

        }
        [Fact]
        public void TestDelete_InvalidDeleteEvent()
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
            viewCalendar.calledUpdateBoard = false;

            //Registering the views
            presenter.RegisterNewView(eventView);
            presenter.RegisterNewView(viewCalendar);

            // Act
            presenter.NewEvent(startDate, mycategories[1], durationInMinutes, details);
            presenter.PopulateDataGrid("", "", false, mycategories[1], false, false);

            CalendarItem calItem = viewCalendar.items[0];
            calItem.EventID = 5;

            viewCalendar.calledUpdateBoard = false;
            presenter.DeleteEvent(calItem);

            Assert.False(viewCalendar.calledUpdateBoard);
            Assert.True(viewCalendar.calledDisplayMessage);
        }
    }
}   