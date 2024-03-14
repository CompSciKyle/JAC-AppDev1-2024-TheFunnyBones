using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Calendar;
using System.Data.SQLite;

namespace CalendarCodeTests
{
    public class TestEvents
    {
        int numberOfEventsInFile = TestConstants.numberOfEventsInFile;
        String testInputFile = TestConstants.testEventsInputFile;
        int maxIDInEventFile = TestConstants.maxIDInEventFile;
        Event firstEventInFile = TestConstants.firstEventInFile;


        // ========================================================================

        [Fact]
        public void EventsObject_New()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            string newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            // Act
            Events events = new Events(conn);

            // Assert 
            Assert.IsType<Events>(events);

        }   

        // ========================================================================

        [Fact]
        public void EventsMethod_ReadFromDatabase_ValidateCorrectDataWasRead()
        {

            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String existingDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(existingDB);
            SQLiteConnection conn = Database.dbConnection;

            // Act
            Events events = new Events(conn);
            List<Event> list = events.List();
            Event firstEvent = list[0];

            // Assert
            Assert.Equal(numberOfEventsInFile, list.Count);
            Assert.Equal(firstEventInFile.Id, firstEvent.Id);
            Assert.Equal(firstEventInFile.StartDateTime, firstEvent.StartDateTime);
            Assert.Equal(firstEventInFile.DurationInMinutes, firstEvent.DurationInMinutes);
            Assert.Equal(firstEventInFile.Category, firstEvent.Category);
            Assert.Equal(firstEventInFile.Details, firstEvent.Details);


        }

        // ========================================================================

        [Fact]
        public void EventsMethod_List_ReturnsListOfEvents()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn);

            // Act
            List<Event> list = events.List();

            // Assert
            Assert.Equal(numberOfEventsInFile, list.Count);

        }

        // ========================================================================
        [Fact]
        public void EventsMethod_List_ModifyListDoesNotModifyEventsInstance()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn);
            List<Event> list = events.List();

            // Act
            list[0].DurationInMinutes = list[0].DurationInMinutes + 21.03; 

            // Assert
            Assert.NotEqual(list[0].DurationInMinutes, events.List()[0].DurationInMinutes);

        }
        // ========================================================================

        [Fact]
        public void EventsMethod_Add()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn);
            string details = "New Event";
            int category = 3;
            double durationInMinutes = 98.1;


            // Act
            events.Add(DateTime.Now, category, durationInMinutes, details);
            List<Event> eventsList = events.List();
            int sizeOfList = events.List().Count;
            
            // Assert
            Assert.Equal(numberOfEventsInFile + 1, sizeOfList);
            Assert.Equal(maxIDInEventFile + 1, eventsList[sizeOfList - 1].Id);
            Assert.Equal(durationInMinutes, eventsList[sizeOfList - 1].DurationInMinutes);
        }

        // ========================================================================

        [Fact]
        public void EventsMethod_Delete()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn);
            int IdToDelete = 3;

            // Act
            events.Delete(IdToDelete);
            List<Event> eventsList = events.List();
            int sizeOfList = eventsList.Count;

            // Assert
            Assert.Equal(numberOfEventsInFile - 1, sizeOfList);
            Assert.False(eventsList.Exists(e => e.Id == IdToDelete), "correct Event item deleted");

        }

        // ========================================================================

        [Fact]
        public void EventsMethod_Delete_InvalidIDDoesntCrash()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messyDB";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn);
            int IdToDelete = 9999;
            int sizeOfList = events.List().Count;

            // Act
            try
            {
                events.Delete(IdToDelete);
                Assert.Equal(sizeOfList, events.List().Count);
            }
            // Assert
            catch
            {
                Assert.True(false, "Invalid ID causes Delete to break");
            }
        }

        // ========================================================================

        [Fact]
        public void EventsMethod_UpdateCategory()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn);
            string details = "Shopping with DavyDav";
            int id = 9;

            // Act
            events.UpdateProperties(id, DateTime.Now, 3, 30.0, details);
            Event chosenEvent = GetEventFromId(events, id);

            // Assert 
            Assert.Equal(details, chosenEvent.Details);
            Assert.Equal(DateTime.Now, chosenEvent.StartDateTime)
            Assert.Equal(3, chosenEvent.Category);
            Assert.Equal(30.0, chosenEvent.DurationInMinutes);

        }

        private Event GetEventFromId(Events events, int id)
        {
            List<Event> listOfEvents = events.List();
            foreach(Event ev in listOfEvents)
            {
                if(ev.Id == id)
                {
                    return ev;
                }

            }

            throw (new Exception("Cannot find event with that id"));

        }
    }
}

