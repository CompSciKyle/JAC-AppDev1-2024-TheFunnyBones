using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================


namespace Calendar
{
    // ====================================================================
    // CLASS: HomeCalendar
    //        - Combines a Categories Class and an Events Class
    //        - One File defines Category and Events File
    //        - etc
    // ====================================================================
    /// <summary>
    /// Represents a home calendar combining categories and events
    /// </summary>
    /// <remarks>
    /// Categories: <see cref="Categories"/>  Events: <see cref="Events"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// <b>Sample Input For ToDoList.Calendar </b>
    /// ToDoList.cats
    /// ToDoList.evts
    /// <b>Sample Input For ToDoList.Cats</b>
    /// Categories  
    /// Category ID = "8" type="Holiday">Canadian Holidays/Category
    /// Category ID = "9" type= "AllDayEvent" > Vacation  / Category 
    /// Category ID= "10" type= "AllDayEvent" > Wellness days/Category
    /// Category ID = "11" type= "AllDayEvent" > Birthdays  / Category 
    /// Categories
    /// <b>Sample Input for ToDoList.evts</b>
    /// Events
    /// Event ID="4"
    ///   StartDateTime>1/20/2020 11:00:00 AM/StartDateTime
    ///   Details>On call security/Details
    ///   DurationInMinutes>180/DurationInMinutes
    ///   Category>7/Category
    ///  Event
    ///  Event ID = "5"
    ///   StartDateTime > 1 / 11 / 2018 7:30:00 PM/StartDateTime
    ///   Details>staff meeting/Details
    ///   DurationInMinutes>15/DurationInMinutes
    ///  Category>2/Category
    ///  Event
    ///  Event ID = "6"
    ///    StartDateTime > 1 / 1 / 2020 12:00:00 AM/StartDateTime
    ///    Details>New Years/Details
    ///    DurationInMinutes>1440/DurationInMinutes
    ///    Category>8/Category
    ///  Event
    ///  Event ID = "7"
    ///    StartDateTime > 1 / 12 / 2020 12:00:00 AM/StartDateTime
    ///    Details>Wendys birthday/Details
    ///    DurationInMinutes>1440/DurationInMinutes
    ///   Category>11/Category
    ///  Event
    ///  Event ID = "8"
    ///    StartDateTime > 1 / 11 / 2018 10:15:00 AM/StartDateTime
    ///    Details>Sprint retrospective/Details
    ///    DurationInMinutes>60/DurationInMinutes
    ///    Category>2/Category
    ///  Event
    /// Events
    ///</code>
    ///<code>
    /// <b>Typical Usage Of This Class</b>
    /// <![CDATA[
    ///     //Creates a homecalendar with a ToDoList and inputs it into the calendar
    ///     HomeCalendar homeCalendar = new HomeCalendar(./ToDoList.calendar);
    ///
    ///     List<CalendarItem> calendarItems = homeCalendar.GetCalendarItems(null, null, false, 0);
    ///
    ///     //Print all important information about the calendar item
    ///     Console.WriteLine("All Calendar Items");
    ///     Console.WriteLine("-------------------------------------------------------------------------------------------");
    ///     Console.WriteLine($"{"Item",-10} {"StartTime",-20} {"Description",-20} {"Duration",-10} {"Total Time Spent"}");
    ///     foreach (CalendarItem ci in calendarItems)
    ///     {
    ///         Console.WriteLine($"{ci.EventID,-10} {ci.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),-20} {ci.ShortDescription,-20} {ci.DurationInMinutes,-10} {ci.BusyTime}");
    ///     }
    ///    ]]>
    ///    
    /// <b>Sample Output</b>
    /// Shoud display everything.
    /// </code>
    /// </example>
    public class HomeCalendar


    {
        private string? _databaseFile;
        private Categories _categories;
        private Events _events;
        private SQLiteConnection _connection;

        // ====================================================================
        // Properties
        // ===================================================================

        // Properties (location of files etc)

        /// <summary>
        /// Gets the directory name.
        /// </summary>
        /// <value>
        /// The name of the folder.
        /// </value>
        private String? DatabaseFile { get { return _databaseFile; } }


        // Properties (categories and events object)
        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <value>
        /// A bunch of groups that have similar items inside of it.
        /// </value>
        public Categories categories { get { return _categories; } }
        /// <summary>
        /// Gets all the events.
        /// </summary>
        /// <value>
        /// A plan you have for specific amount of time.
        /// </value>
        public Events events { get { return _events; } }

        private SQLiteConnection Connection { get { return _connection; } set { _connection = value; } }

        // -------------------------------------------------------------------
        // Constructor (existing calendar ... must specify file)
        // -------------------------------------------------------------------
        /// <summary>
        /// Initilizing a instance of the HomeCalandar using an existing calendar.
        /// </summary>
        /// <param name="calendarFileName">The file name for the calendar.</param>
        /// <example>
        /// Initilizing a home calendar using a existing home calendar.
        /// <code>
        /// HomeCalendar homeCalendar = new HomeCalendar("CompSciCalendar.txt");
        /// </code>
        /// </example>
        public HomeCalendar(string databaseFile)
        {
            Database.existingDatabase(databaseFile);
            Connection = Database.dbConnection;
            _categories = new Categories(Database.dbConnection, false);
            _events = new Events(Database.dbConnection);
        }



        // ============================================================================
        // Get all events list
        // ============================================================================
        /// <summary>
        /// Gets all calendar items falling within the specified date range, ordered by start date and end date.
        /// </summary>
        /// <param name="Start">The start date of the calendar items you want to get. A null value indicates an open range.</param>
        /// <param name="End">The end date of the calendar items you want to get. A null value indicates an open range.</param>
        /// <param name="FilterFlag">If true, filters by category ID.</param>
        /// <param name="CategoryID">The category ID to filter by.</param>
        /// <returns>A list of calendar items falling within the specified date range, inclusive, ordered by time.</returns>
        /// <example>
        /// Assume all the below examples contain this input.
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		             1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Years                  1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendys birthday            1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60 ''
        /// </code>
        /// <b>Gets all calendar items with default start date and end date</b>
        /// <code>
        /// <![CDATA[
        /// HomeCalendar calendar = new HomeCalendar();
        /// List<CalendarItem> calendarItems = calendar.GetCalendarItems(null, null, false, 0); // Category ID is used to uniquely identify a category
        ///
        ///    // Prints information on each calendar item sorted by the time of the day in ascending order
        ///    
        ///    Console.WriteLine("All Calendar Items");
        ///    Console.WriteLine("-------------------------------------------------------------------------------------------");
        ///    Console.WriteLine($"{"Item",-10} {"StartTime",-20} {"Description",-20} {"Duration",-10} {"Total Time Spent"}");
        ///    foreach (CalendarItem ci in calendarItems)
        ///    {
        ///        Console.WriteLine($"{ci.EventID,-10} {ci.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),-20} {ci.ShortDescription,-20} {ci.DurationInMinutes,-10} {ci.BusyTime}"); // Representing the amount of time spent doing an event as the day goes on.
        ///    }
        ///    ]]>
        ///    
        ///<b>Sample Output</b>
        /// Item       StartTime            Description          Duration   Total Time Spent
        /// 1          2018-Jan.-10-10-00   App Dev Homework     40         40
        /// 8          2018-Jan.-11-10-15   Sprint retrospective 60         100 
        /// 5          2018-Jan.-11-19-30   staff meeting        15         115
        /// 6          2020-Jan.-01-00-00   New Years            1440       1555
        /// 2          2020-Jan.-09-00-00   Honolulu             1440       2995
        /// 3          2020-Jan.-10-00-00   Honolulu             1440       4435
        /// 7          2020-Jan.-12-00-00   Wendys birthday      1440       5875
        /// 4          2020-Jan.-20-11-00   On call security     180        6055
        /// </code>
        ///
        /// <b>Gets all calendar items within a certain date.</b>
        /// <code>
        ///    // Start date and end date are inclusive and will include everything on the end date and start date
        ///    <![CDATA[
        ///    HomeCalendar calendar = new HomeCalendar();
        ///    List<CalendarItem> calendarItems = calendar.GetCalendarItems(DateTime.Now.AddYears(-6), DateTime.Now, false, 0); // Category ID is used to uniquely identify a category
        ///
        ///    // Prints all information within the current day to the next day ordered by the time in ascending order.
        ///    Console.WriteLine("All Calendar Items");
        ///    Console.WriteLine("-------------------------------------------------------------------------------------------");
        ///    Console.WriteLine($"{"Item",-10} {"StartTime",-20} {"Description",-20} {"Duration",-10} {"Total Time Spent"}");
        ///    foreach (CalendarItem ci in calendarItems)
        ///    {
        ///        Console.WriteLine($"{ci.EventID,-10} {ci.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),-20} {ci.ShortDescription,-20} {ci.DurationInMinutes,-10} {ci.BusyTime}"); // Representing the amount of time spent doing an event as the day goes on.
        ///    }
        ///    ]]>
        ///    
        ///<b>Sample output</b>
        /// Item       StartTime            Description          Duration   Total Time Spent
        /// 6          2020-Jan.-01-00-00   New Years            1440       1440
        /// 2          2020-Jan.-09-00-00   Honolulu             1440       2880
        /// 3          2020-Jan.-10-00-00   Honolulu             1440       4320
        /// 7          2020-Jan.-12-00-00   Wendys birthday      1440       5760
        /// 4          2020-Jan.-20-11-00   On call security     180        5940
        /// </code>
        ///
        /// <b>Gets all calendar items sorted by a category ID</b>
        /// <code>
        /// <![CDATA[
        /// HomeCalendar calendar = new HomeCalendar();
        /// List<CalendarItem> calendarItems = calendar.GetCalendarItems(null, null, true, 9); // If the filter tag is set to true, it will retrieve calendar items only with the specific Category ID, which uniquely identifies a category
        ///
        ///    // Prints all information on calendar items that are in the category ID of 9 ordered by the time of day in ascending order.
        ///    Console.WriteLine("All Calendar Items");
        ///    Console.WriteLine("-------------------------------------------------------------------------------------------");
        ///    Console.WriteLine($"{"Item",-10} {"StartTime",-20} {"Description",-20} {"Duration",-10} {"Total Time Spent"}");
        ///    foreach (CalendarItem ci in calendarItems)
        ///    {
        ///        Console.WriteLine($"{ci.EventID,-10} {ci.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),-20} {ci.ShortDescription,-20} {ci.DurationInMinutes,-10} {ci.BusyTime}"); // Representing the amount of time spent doing an event as the day goes on.
        ///    }
        ///    ]]>
        ///<b>Sample output</b>
        /// Item       StartTime            Description          Duration   Total Time Spent
        /// 2          2020-Jan.-09-00-00   Honolulu             1440       1440
        /// 3          2020-Jan.-10-00-00   Honolulu             1440       2880
        /// </code>
        /// </example>


        public List<CalendarItem> GetCalendarItems(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // ------------------------------------------------------------------------
            // return joined list within time frame
            // ------------------------------------------------------------------------
            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            List<CalendarItem> items = new List<CalendarItem>();
            Double totalBusyTime = 0;


            var cmd = new SQLiteCommand("SELECT c.Id as 'CategoryId', e.Id as 'EventId', e.StartDateTime as 'EventStartDateTime', c.Description as 'CategoryDescription', e.Details as 'EventDetails', e.DurationInMinutes as 'EventDurationInMinutes' FROM categories c JOIN events e ON e.CategoryId = c.Id WHERE e.StartDateTime >= @start AND e.StartDateTime <= @end ORDER BY e.StartDateTime;", Connection);
            cmd.Parameters.AddWithValue("@start", Start?.ToString("yyyy-MM-dd H:mm:ss"));
            cmd.Parameters.AddWithValue("@end", End?.ToString("yyyy-MM-dd H:mm:ss"));


            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    //filter out unwanted categories if filter flag is on
                    if (FilterFlag && CategoryID != Convert.ToInt32(reader["CategoryId"]))
                    {
                        continue;
                    }

                    // keep track of running totals while ignoring availability time
                    Category categoryFromId = _categories.GetCategoryFromId(Convert.ToInt32(reader["CategoryId"]));
                    if (categoryFromId.Type != Category.CategoryType.Availability)
                        totalBusyTime = totalBusyTime + Convert.ToDouble(reader["EventDurationInMinutes"]);

                    items.Add(new CalendarItem
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryId"]),
                        EventID = Convert.ToInt32(reader["EventId"]),
                        ShortDescription = Convert.ToString(reader["EventDetails"]),
                        StartDateTime = Convert.ToDateTime(reader["EventStartDateTime"]),
                        DurationInMinutes = Convert.ToDouble(reader["EventDurationInMinutes"]),
                        Category = Convert.ToString(reader["CategoryDescription"]),
                        BusyTime = totalBusyTime
                    });

                }
            }
            return items;
        }

        // ============================================================================
        // Group all events month by month (sorted by year/month)
        // returns a list of CalendarItemsByMonth which is 
        // "year/month", list of calendar items, and totalBusyTime for that month
        // ============================================================================
        /// <summary>
        /// Gets all calendar items based on the start date and end date grouped by month and year.
        /// </summary>
        /// <param name="Start">The start date of the calendar items you want.</param>
        /// <param name="End">The end date of the calendar items you want.</param>
        /// <param name="FilterFlag">If true, it filters it by category ID.</param>
        /// <param name="CategoryID"> The category ID to filter by.</param>
        /// <returns>CalendarItemsByMonth that has been grouped by month with a list of calendar item inside of it.</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Assume all the below examples contain this input
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		             1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    Staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Years                  1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendys birthday            1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60 ''
        ///    ]]>
        /// </code>
        /// <b>Gets all calendar items grouped by month with default start dates and end dates</b>
        /// 
        /// <code>
        ///   <![CDATA[
        ///   //The list is sorted by the begining of the month, the items inside of it are ordered by time in ascending order.
        /// 
        ///     HomeCalendar calendar = new HomeCalendar();
        ///     List<CalendarItemsByMonth> allItemsPerMonth = calendar.GetCalendarItemsByMonth(null, null, false, 0);//If FilterTag is set to false, it doesn't filters by category
        ///
        ///    foreach (CalendarItemsByMonth ci in allItemsPerMonth)
        ///    {
        ///        Console.WriteLine("Month: " + ci.Month);
        ///        Console.WriteLine("All containing items");
        ///
        ///        foreach (CalendarItem items in ci.Items)
        ///        {
        ///            Console.WriteLine($"Description Of Item {items.EventID}: " + items.ShortDescription);
        ///        }
        ///     
        ///       Console.WriteLine($"Total time spent on events per month in minutes: {ci.TotalBusyTime} \n"); //Represents the amount of total time spent on these events throughout the month
        ///   }
        ///   ]]>
        /// <b>Sample output</b>
        /// Month: 2018/01
        /// All containing items
        /// -------------------------------------------------------------------------------------------
        /// Description Of Item 1: App Dev Homework
        /// Description Of Item 8: Sprint retrospective
        /// Description Of Item 5: staff meeting
        /// Total time spent on events per month in minutes: 115
        /// Month: 2020/01
        /// All containing items
        /// -------------------------------------------------------------------------------------------
        /// Description Of Item 6: New Years
        /// Description Of Item 2: Honolulu
        /// Description Of Item 3: Honolulu
        /// Description Of Item 7: Wendys birthday
        /// Description Of Item 4: On call security
        /// Total time spent on events per month in minutes: 5940
        ///   
        /// </code>
        /// <b>Gets all calendar items grouped by month within a range of time</b>
        /// <code>
        ///    <![CDATA[
        ///    HomeCalendar calendar = new HomeCalendar();
        ///    List<CalendarItemsByMonth> allItemsPerMonth = calendar.GetCalendarItemsByMonth(DateTime.Now.AddYears(-6), DateTime.Now, false, 0); //If FilterTag is set to false,  it doesn't filters by category
        ///
        ///    foreach (CalendarItemsByMonth ci in allItemsPerMonth)
        ///    {
        ///        Console.WriteLine("Month: " + ci.Month);
        ///        Console.WriteLine("All containing items");
        ///
        ///        foreach (CalendarItem items in ci.Items)
        ///        {
        ///            Console.WriteLine($"Description Of Item {items.EventID}: " + items.ShortDescription);
        ///        }
        ///     
        ///       Console.WriteLine($"Total time spent on events per month in minutes: {ci.TotalBusyTime} \n"); //Represents the amount of total time spent on these events throughout the month
        ///   }
        ///   ]]>
        ///   
        /// <b>Sample output</b>
        /// Month: 2020/01
        /// All containing items
        /// -------------------------------------------------------------------------------------------
        /// Description Of Item 6: New Years
        /// Description Of Item 2: Honolulu
        /// Description Of Item 3: Honolulu
        /// Description Of Item 7: Wendys birthday
        /// Description Of Item 4: On call security
        /// Total time spent on events per month in minutes: 5940
        ///   
        /// </code>
        /// 
        /// <b>Gets all calendar items grouped by month but filtering it through the catergory ID</b>
        /// <code>
        /// <![CDATA[
        ///     
        ///     HomeCalendar calendar = new HomeCalendar();
        ///     List<CalendarItemsByMonth> allItemsPerMonth = calendar.GetCalendarItemsByMonth(null, null, true, 9); //If FilterTag is set to true, filters by category as well as groups with month
        ///
        ///    foreach (CalendarItemsByMonth ci in allItemsPerMonth)
        ///    {
        ///        Console.WriteLine("Month: " + ci.Month);
        ///        Console.WriteLine("All containing items");
        ///
        ///        foreach (CalendarItem items in ci.Items)
        ///        {
        ///            Console.WriteLine($"Description Of Item {items.EventID}: " + items.ShortDescription);
        ///        }
        ///     
        ///       Console.WriteLine($"Total time spent on events per month in minutes: {ci.TotalBusyTime} \n"); //Represents the amount of total time spent on these events throughout the month
        ///   }
        ///   ]]>
        ///   
        ///  <b>Sample output</b>
        ///  All containing items
        /// -------------------------------------------------------------------------------------------
        /// Description Of Item 2: Honolulu
        /// Description Of Item 3: Honolulu
        /// Total time spent on events per month in minutes: 2880
        /// </code>
        /// 
        /// </example>
        public List<CalendarItemsByMonth> GetCalendarItemsByMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            var cmd = new SQLiteCommand("SELECT STRFTIME('%Y/%m', e.StartDateTime) as Month, c.Id as CategoryId, e.Id, e.StartDateTime, c.Description, e.Details, e.DurationInMinutes as DurationInMinutes FROM categories c JOIN events e ON e.CategoryId = c.Id WHERE e.StartDateTime >= @start AND e.StartDateTime <= @end GROUP BY STRFTIME('%Y/%m', e.StartDateTime) ORDER BY STRFTIME('%Y/%m', e.StartDateTime);", Connection);
            cmd.Parameters.AddWithValue("@start", Start?.ToString("yyyy-MM-dd H:mm:ss"));
            cmd.Parameters.AddWithValue("@end", End?.ToString("yyyy-MM-dd H:mm:ss"));

            //var GroupedByMonth = items.GroupBy(c => c.StartDateTime.Year.ToString("D4") + "/" + c.StartDateTime.Month.ToString("D2"));

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<CalendarItemsByMonth>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // calculate totalBusyTime for this month, and create list of items
                    if (FilterFlag && CategoryID != Convert.ToInt32(reader["CategoryId"]))
                    {
                        continue;
                    }

                    double total = 0;
                    DateTime time = Convert.ToDateTime(reader["Month"]);
                    List<CalendarItem> calendarItems = GetCalendarItems(time, time.AddMonths(1), FilterFlag, CategoryID);
                    foreach (CalendarItem item in calendarItems)
                    {
                        Category categoryFromId = _categories.GetCategoryFromId(item.CategoryID);
                        if (categoryFromId.Type != Category.CategoryType.Availability)
                            total += item.DurationInMinutes;

                    }


                    // Add new CalendarItemsByMonth to our list
                    summary.Add(new CalendarItemsByMonth
                    {
                        Month = Convert.ToString(reader["Month"]),
                        Items = calendarItems,
                        TotalBusyTime = total
                    });
                }
            }

            return summary;
        }

        // ============================================================================
        // Group all events by category (ordered by category name)
        // ============================================================================

        /// <summary>
        /// Gets all calendar items based on their start date and end date grouped by category.
        /// </summary>
        /// <param name="Start">The start date of the items you want to get.</param>
        /// <param name="End">The end date of the item you want to get.</param>
        /// <param name="FilterFlag">If true, filters by category ID.</param>
        /// <param name="CategoryID">The category ID you want it to be filtered by.</param>
        /// <returns>CalendarItemsByCategory that have been grouped by category with a list of calendar items inside of it.</returns>
        /// <example>
        /// Assume all the below examples contain this input.
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		             1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    Staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Years                  1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendys birthday            1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60 '' 
        /// </code>
        /// <code>
        /// <b>Gets all calendarItems grouped by category with default start time and end time</b>
        /// <![CDATA[
        ///  HomeCalendar homeCalendar = new homeCalendar();
        ///  List<CalendarItemsByCategory> allItemsPerCategory = calendar.GeCalendarItemsByCategory(null, null, false, 0);
        ///
        ///    foreach (CalendarItemsByCategory ci in allItemsPerCategory)
        ///   {
        ///        Console.WriteLine("Category: " + ci.Category);
        ///        Console.WriteLine("All containing items");
        ///
        ///        foreach (CalendarItem items in ci.Items)
        ///        {
        ///            Console.WriteLine($"Description Of Item {items.EventID}: " + items.ShortDescription);
        ///        }
        ///     Console.WriteLine($"Total time spent on events per month in minutes: {ci.TotalBusyTime} \n");
        ///    }
        ///   ]]>
        ///    
        /// <b>Sample output</b>
        /// Category: Birthdays
        /// All containing items
        /// Description Of Item 7: Wendys birthday
        /// Total time spent on events per month in minutes: 1440
        /// Category: Canadian Holidays
        /// All containing items
        /// Description Of Item 6: New Years
        /// Total time spent on events per month in minutes: 1440
        /// Category: Fun
        /// All containing items
        /// Description Of Item 1: App Dev Homework
        /// Total time spent on events per month in minutes: 40
        /// Category: On call
        /// All containing items
        /// Description Of Item 4: On call security
        /// Total time spent on events per month in minutes: 180
        /// Category: Vacation
        /// All containing items
        /// Description Of Item 2: Honolulu
        /// Description Of Item 3: Honolulu
        /// Total time spent on events per month in minutes: 2880
        /// Category: Work
        /// All containing items
        /// Description Of Item 8: Sprint retrospective
        /// Description Of Item 5: staff meeting
        /// Total time spent on events per month in minutes: 75
        /// </code>
        /// <code>
        /// <b>Gets all calendar items grouped by category and within the start date and end date</b>
        /// <![CDATA[
        ///  HomeCalendar homeCalendar = new homeCalendar();
        ///  List<CalendarItemsByCategory> allItemsPerCategory = calendar.GeCalendarItemsByCategory(DateTime.Now.AddYears(-6),DateTime.Now, false, 0);
        ///
        ///    foreach (CalendarItemsByCategory ci in allItemsPerCategory)
        ///   {
        ///        Console.WriteLine("Category: " + ci.Category);
        ///        Console.WriteLine("All containing items");
        ///
        ///        foreach (CalendarItem items in ci.Items)
        ///        {
        ///            Console.WriteLine($"Description Of Item {items.EventID}: " + items.ShortDescription);
        ///        }
        ///     Console.WriteLine($"Total time spent on events per month in minutes: {ci.TotalBusyTime} \n");
        ///    }
        ///    ]]>
        ///    <b>Sample output</b>
        /// Category: Birthdays
        /// All containing items
        /// Description Of Item 7: Wendys birthday
        /// Total time spent on events per month in minutes: 1440
        /// Category: Canadian Holidays
        /// All containing items
        /// Description Of Item 6: New Years
        /// Total time spent on events per month in minutes: 1440
        /// Category: On call
        /// All containing items
        /// Description Of Item 4: On call security
        /// Total time spent on events per month in minutes: 180
        /// Category: Vacation
        /// All containing items
        /// Description Of Item 2: Honolulu
        /// Description Of Item 3: Honolulu
        /// Total time spent on events per month in minutes: 2880
        /// </code>
        /// <code>
        /// <b>Gets all calendar items grouped by category and is filtered by category</b>
        /// <![CDATA[
        ///  HomeCalendar homeCalendar = new homeCalendar();
        ///  List<CalendarItemsByCategory> allItemsPerCategory = calendar.GeCalendarItemsByCategory(null, null, true, 9);
        ///
        ///    foreach (CalendarItemsByCategory ci in allItemsPerCategory)
        ///   {
        ///        Console.WriteLine("Category: " + ci.Category);
        ///        Console.WriteLine("All containing items");
        ///
        ///        foreach (CalendarItem items in ci.Items)
        ///        {
        ///            Console.WriteLine($"Description Of Item {items.EventID}: " + items.ShortDescription);
        ///        }
        ///     Console.WriteLine($"Total time spent on events per month in minutes: {ci.TotalBusyTime} \n");
        ///    }
        ///   ]]>
        ///    <b>Sample output</b>
        /// Category: Vacation
        /// All containing items
        /// Description Of Item 2: Honolulu
        /// Description Of Item 3: Honolulu
        /// Total time spent on events per month in minutes: 2880
        /// </code>
        /// </example>
        public List<CalendarItemsByCategory> GetCalendarItemsByCategory(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            var cmd = new SQLiteCommand("SELECT c.Id as CategoryId, e.Id, e.StartDateTime, c.Description as CategoryName, e.Details, e.DurationInMinutes as DurationInMinutes FROM categories c JOIN events e ON e.CategoryId = c.Id WHERE e.StartDateTime >= @start AND e.StartDateTime <= @end GROUP BY c.Description ORDER BY c.Description", Connection);
            cmd.Parameters.AddWithValue("@start", Start?.ToString("yyyy-MM-dd H:mm:ss"));
            cmd.Parameters.AddWithValue("@end", End?.ToString("yyyy-MM-dd H:mm:ss"));

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<CalendarItemsByCategory>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // calculate totalBusyTime for this month, and create list of items
                    if (FilterFlag && CategoryID != Convert.ToInt32(reader["CategoryId"]))
                    {
                        continue;
                    }
                    // calculate totalBusyTime for this category, and create list of items
                    double total = 0;
                    string categoryName = Convert.ToString(reader["CategoryName"]);
                    List<CalendarItem> calendarItems = GetCalendarItems(Start, End, FilterFlag, CategoryID);
                    var items = new List<CalendarItem>();
                    foreach (CalendarItem item in calendarItems)
                    {
                        if (item.Category == categoryName)
                        {
                            Category categoryFromId = _categories.GetCategoryFromId(item.CategoryID);
                            if (categoryFromId.Type != Category.CategoryType.Availability)
                                total += item.DurationInMinutes;
                            items.Add(item);

                        }
                    }

                    // Add new CalendarItemsByCategory to our list
                    summary.Add(new CalendarItemsByCategory
                    {
                        Category = categoryName,
                        Items = items,
                        TotalBusyTime = total
                    });
                }
            }

            return summary;
        }



        // ============================================================================
        // Group all events by category and Month
        // creates a list of Dictionary objects with:
        //          one dictionary object per month,
        //          and one dictionary object for the category total busy times
        // 
        // Each per month dictionary object has the following key value pairs:
        //           "Month", <name of month>
        //           "TotalBusyTime", <the total durations for the month>
        //             for each category for which there is an event in the month:
        //             "items:category", a List<CalendarItem>
        //             "category", the total busy time for that category for this month
        // The one dictionary for the category total busy times has the following key value pairs:
        //             for each category for which there is an event in ANY month:
        //             "category", the total busy time for that category for all the months
        // ============================================================================
        /// <summary>
        /// Gets all calendar items based on start date and end date grouped by month and category.
        /// </summary>
        /// <param name="Start">The start date of the calendar items you want to get.</param>
        /// <param name="End">The end date of the calendar items you want to get.</param>
        /// <param name="FilterFlag">If true, filter by category ID.</param>
        /// <param name="CategoryID">The category ID you want to filter by.</param>
        /// <returns>A list of dictionary objects with one dictionary object for month and one for the category.</returns>
        /// <example>
        /// Assume all the below examples contain this input.
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework        40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		          1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security        180
        ///    2       5      1/11/2018 7:30:00 PM    Staff meeting           15
        ///    8       6      1/1/2020 12:00:00 AM    New Years               1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendys birthday         1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective    60 ''
        /// </code>
        /// <code>
        ///     <b>Getting All calendar items grouped by month and are categorized with a total time spent.</b>
        ///    <![CDATA[
        ///    HomeCalendar calendar = new HomeCalendar();
        ///    List<Dictionary<string, object>> allCalendarItemsByCategoryAndMonth = calendar.GetCalendarDictionaryByCategoryAndMonth(null, null, false, 0);
        ///     
        ///    foreach (Dictionary<string, object> dictionary in allCalendarItemsByCategoryAndMonth)
        ///    {
        ///        Console.WriteLine("------------------------------------------------------------------------------");
        ///        Console.WriteLine($"Month: {dictionary["Month"]}");
        ///
        ///        if (dictionary.ContainsKey("TotalBusyTime"))
        ///        {
        ///            Console.WriteLine($"Time spent: {dictionary["TotalBusyTime"]}");
        ///        }
        ///
        ///        foreach (var item in dictionary)
        ///        {
        ///            if (item.Key.StartsWith("items:"))
        ///            {
        ///                Console.WriteLine($"Category: {item.Key.Replace("items:", "")}"); //Removing items: from the key inside of the Console.WriteLine
        ///
        ///
        ///
        ///                Console.WriteLine($"{"Item",-10} {"StartTime",-20} {"Description",-20} {"Duration",-10} {"Total Time Spent"}");
        ///
        ///                List<CalendarItem> calendarItems = new List<CalendarItem>();
        ///                calendarItems = item.Value as List<CalendarItem>;
        ///
        ///                foreach (CalendarItem ci in calendarItems)
        ///                 {
        ///                    Console.WriteLine($"{ci.EventID,-10} {ci.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),-20} {ci.ShortDescription,-20} {ci.DurationInMinutes,-10} {ci.BusyTime}");
        ///                }
        ///            }
        ///        }
        ///
        ///        if (dictionary["Month"] == "TOTALS")
        ///        {
        ///
        ///            foreach (var total in dictionary)
        ///            {
        ///                 Console.WriteLine($" Category: {total.Key} Total Time Spent: {total.Value}");
        ///            }
        ///        }
        ///
        ///         Console.WriteLine("");
        ///
        ///   }
        ///     ]]>
        ///     <b>Sample Output</b>
        ///     -------------------------------------------------------------------------------------------------
        ///     Month: 2018/01
        ///     Time spent: 115
        ///     Category: Fun
        ///     Item       StartTime            Description          Duration   Total Time Spent
        ///     1          2018-Jan.-10-10-00   App Dev Homework     40         40
        ///     Category: Work
        ///     Item       StartTime            Description          Duration Total Time Spent
        ///     8          2018-Jan.-11-10-15   Sprint retrospective 60         100
        ///     5          2018-Jan.-11-19-30   staff meeting        15         115
        ///     ------------------------------------------------------------------------------------------------
        ///     Month: 2020/01
        ///     Time spent: 5940
        ///     Category: Birthdays
        ///     Item       StartTime            Description          Duration   Total Time Spent
        ///     7          2020-Jan.-12-00-00   Wendys birthday      1440       5875    
        ///     Category: Canadian Holidays
        ///     Item       StartTime            Description          Duration   Total Time Spent
        ///     6          2020-Jan.-01-00-00   New Years            1440       1555
        ///     Category: On call
        ///     Item       StartTime            Description          Duration   Total Time Spent
        ///     4          2020-Jan.-20-11-00   On call security     180        6055
        ///     Category: Vacation
        ///     Item       StartTime            Description          Duration   Total Time Spent
        ///     2          2020-Jan.-09-00-00   Honolulu             1440       2995
        ///     3          2020-Jan.-10-00-00   Honolulu             1440       4435
        ///     ---------------------------------------------------------------------------------------------
        ///     Month: TOTALS
        ///     Category: Month Total Time Spent: TOTALS
        ///     Category: Work Total Time Spent: 75
        ///     Category: Fun Total Time Spent: 40
        ///     Category: On call Total Time Spent: 180
        ///     Category: Canadian Holidays Total Time Spent: 1440
        ///     Category: Vacation Total Time Spent: 2880
        ///     Category: Birthdays Total Time Spent: 1440   
        /// </code>
        /// <code>
        /// <b>Getting All calendar items within the past 6 years and grouped by month and are categorized with a total time spent.</b>
        ///  <![CDATA[
        ///    HomeCalendar calendar = new HomeCalendar();
        ///    List<Dictionary<string, object>> allCalendarItemsByCategoryAndMonth = calendar.GetCalendarDictionaryByCategoryAndMonth((DateTime.Now.AddYears(-6), DateTime.Now, false, 0);
        ///     
        ///    foreach (Dictionary<string, object> dictionary in allCalendarItemsByCategoryAndMonth)
        ///    {
        ///        Console.WriteLine("-----------------------------------------------------------------------------");
        ///        Console.WriteLine($"Month: {dictionary["Month"]}");
        ///
        ///        if (dictionary.ContainsKey("TotalBusyTime"))
        ///        {
        ///            Console.WriteLine($"Time spent: {dictionary["TotalBusyTime"]}");
        ///        }
        ///
        ///        foreach (var item in dictionary)
        ///        {
        ///            if (item.Key.StartsWith("items:"))
        ///            {
        ///                Console.WriteLine($"Category: {item.Key.Replace("items:", "")}"); //Removing items: from the key inside of the Console.WriteLine
        ///
        ///
        ///
        ///                Console.WriteLine($"{"Item",-10} {"StartTime",-20} {"Description",-20} {"Duration",-10} {"Total Time Spent"}");
        ///
        ///                List<CalendarItem> calendarItems = new List<CalendarItem>();
        ///                calendarItems = item.Value as List<CalendarItem>;
        ///
        ///                foreach (CalendarItem ci in calendarItems)
        ///                 {
        ///                    Console.WriteLine($"{ci.EventID,-10} {ci.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),-20} {ci.ShortDescription,-20} {ci.DurationInMinutes,-10} {ci.BusyTime}");
        ///                }
        ///            }
        ///        }
        ///
        ///        if (dictionary["Month"] == "TOTALS")
        ///        {
        ///
        ///            foreach (var total in dictionary)
        ///            {
        ///                 Console.WriteLine($" Category: {total.Key} Total Time Spent: {total.Value}");
        ///            }
        ///        }
        ///
        ///         Console.WriteLine("");
        ///
        ///   }
        ///     ]]>
        /// <b>Sample Output</b>
        /// /// --------------------------------------------------------------------------------------------
        /// Month: 2020/01
        /// Time spent: 5940
        /// Category: Birthdays
        /// Item       StartTime            Description          Duration   Total Time Spent
        /// 7          2020-Jan.-12-00-00   Wendys birthday      1440       5760
        /// Category: Canadian Holidays
        /// Item       StartTime            Description          Duration   Total Time Spent
        /// 6          2020-Jan.-01-00-00   New Years            1440       1440
        /// Category: On call
        /// Item       StartTime            Description          Duration   Total Time Spent
        /// 4          2020-Jan.-20-11-00   On call security     180        5940
        /// Category: Vacation
        /// Item       StartTime            Description          Duration   Total Time Spent
        /// 2          2020-Jan.-09-00-00   Honolulu             1440       2880
        /// 3          2020-Jan.-10-00-00   Honolulu             1440       4320
        /// ----------------------------------------------------------------------------------------------
        /// Month: TOTALS
        ///  Category: Month Total Time Spent: TOTALS
        ///  Category: On call Total Time Spent: 180
        ///  Category: Canadian Holidays Total Time Spent: 1440
        ///  Category: Vacation Total Time Spent: 2880
        ///  Category: Birthdays Total Time Spent: 1440
        /// </code>
        /// <code>
        ///<b>Getting All calendar items categorized by category ID 9 and grouped by month and are categorized with a total time spent.</b>
        ///       ///  <![CDATA[
        ///    HomeCalendar calendar = new HomeCalendar();
        ///    List<Dictionary<string, object>> allCalendarItemsByCategoryAndMonth = calendar.GetCalendarDictionaryByCategoryAndMonth((null,null, true, 9);
        ///     
        ///    foreach (Dictionary<string, object> dictionary in allCalendarItemsByCategoryAndMonth)
        ///    {
        ///        Console.WriteLine("---------------------------------------------------------------------");
        ///        Console.WriteLine($"Month: {dictionary["Month"]}");
        ///
        ///        if (dictionary.ContainsKey("TotalBusyTime"))
        ///        {
        ///            Console.WriteLine($"Time spent: {dictionary["TotalBusyTime"]}");
        ///        }
        ///
        ///        foreach (var item in dictionary)
        ///        {
        ///            if (item.Key.StartsWith("items:"))
        ///            {
        ///                Console.WriteLine($"Category: {item.Key.Replace("items:", "")}"); //Removing items: from the key inside of the Console.WriteLine
        ///
        ///
        ///
        ///                Console.WriteLine($"{"Item",-10} {"StartTime",-20} {"Description",-20} {"Duration",-10} {"Total Time Spent"}");
        ///
        ///                List<CalendarItem> calendarItems = new List<CalendarItem>();
        ///                calendarItems = item.Value as List<CalendarItem>;
        ///
        ///                foreach (CalendarItem ci in calendarItems)
        ///                 {
        ///                    Console.WriteLine($"{ci.EventID,-10} {ci.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),-20} {ci.ShortDescription,-20} {ci.DurationInMinutes,-10} {ci.BusyTime}");
        ///                }
        ///            }
        ///        }
        ///
        ///        if (dictionary["Month"] == "TOTALS")
        ///        {
        ///
        ///            foreach (var total in dictionary)
        ///            {
        ///                 Console.WriteLine($" Category: {total.Key} Total Time Spent: {total.Value}");
        ///            }
        ///        }
        ///
        ///         Console.WriteLine("");
        ///
        ///   }
        ///     ]]>
        /// -------------------------------------------------------------------------------------
        /// Month: 2020/01
        /// Time spent: 2880
        /// Category: Vacation
        /// Item       StartTime            Description          Duration   Total Time Spent
        /// 2          2020-Jan.-09-00-00   Honolulu             1440       1440
        /// 3          2020-Jan.-10-00-00   Honolulu             1440       2880
        ///
        /// -------------------------------------------------------------------------------------
        /// Month: TOTALS
        ///  Category: Month Total Time Spent: TOTALS
        ///  Category: Vacation Total Time Spent: 2880
        ///
        /// </code>
        /// </example>
        public List<Dictionary<string, object>> GetCalendarDictionaryByCategoryAndMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items by month 
            // -----------------------------------------------------------------------
            List<CalendarItemsByMonth> GroupedByMonth = GetCalendarItemsByMonth(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // loop over each month
            // -----------------------------------------------------------------------
            var summary = new List<Dictionary<string, object>>();
            var totalBusyTimePerCategory = new Dictionary<String, Double>();

            foreach (var MonthGroup in GroupedByMonth)
            {
                // create record object for this month
                Dictionary<string, object> record = new Dictionary<string, object>();
                record["Month"] = MonthGroup.Month;
                record["TotalBusyTime"] = MonthGroup.TotalBusyTime;

                // break up the month items into categories
                var GroupedByCategory = MonthGroup.Items.GroupBy(c => c.Category);

                // -----------------------------------------------------------------------
                // loop over each category
                // -----------------------------------------------------------------------
                foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
                {

                    // calculate totals for the cat/month, and create list of items
                    double totalCategoryBusyTimeForThisMonth = 0;
                    var details = new List<CalendarItem>();

                    foreach (var item in CategoryGroup)
                    {
                        totalCategoryBusyTimeForThisMonth = totalCategoryBusyTimeForThisMonth + item.DurationInMinutes;
                        details.Add(item);
                    }

                    // add new properties and values to our record object
                    record["items:" + CategoryGroup.Key] = details;
                    record[CategoryGroup.Key] = totalCategoryBusyTimeForThisMonth;

                    // keep track of totals for each category
                    if (totalBusyTimePerCategory.TryGetValue(CategoryGroup.Key, out Double currentTotalBusyTimeForCategory))
                    {
                        totalBusyTimePerCategory[CategoryGroup.Key] = currentTotalBusyTimeForCategory + totalCategoryBusyTimeForThisMonth;
                    }
                    else
                    {
                        totalBusyTimePerCategory[CategoryGroup.Key] = totalCategoryBusyTimeForThisMonth;
                    }
                }

                // add record to collection
                summary.Add(record);
            }
            // ---------------------------------------------------------------------------
            // add final record which is the totals for each category
            // ---------------------------------------------------------------------------
            Dictionary<string, object> totalsRecord = new Dictionary<string, object>();
            totalsRecord["Month"] = "TOTALS";

            foreach (var cat in categories.List())
            {
                try
                {
                    totalsRecord.Add(cat.Description, totalBusyTimePerCategory[cat.Description]);
                }
                catch { }
            }
            summary.Add(totalsRecord);


            return summary;
        }




    }
}

