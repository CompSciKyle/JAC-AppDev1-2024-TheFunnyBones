using System.Collections.Generic;
using System.Globalization;

namespace Calendar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //            CalendarFiles.VerifyReadFromFileName("./test.calendar", null);
            //            HomeCalendar calendar = new HomeCalendar();

            //            calendar.ReadFromFile("./test.calendar");
            //            MainMenu(calendar);

            //        }

            //        static void MainMenu(HomeCalendar calendar)
            //        {
            //            do
            //            {

            //                Console.WriteLine(@"1.Display All Calendar Items
            //2.Display All Calendar Items By Month And Year
            //3.Display All Calendar Items By Category
            //4.Display All Calendar Items By Category and Month
            //5.Quit");

            //                bool ifValid = int.TryParse(Console.ReadLine(), out int userInput);

            //                switch (userInput)
            //                {
            //                    case 1:
            //                        DispalyAllCalendarItems(calendar);
            //                        Console.ReadKey();
            //                        Console.Clear();
            //                        break;
            //                    case 2:
            //                        DispalyAllMonthlyCalendarItems(calendar);
            //                        Console.ReadKey();
            //                        Console.Clear();
            //                        break;
            //                    case 3:
            //                        DispalyAllCategorizedCalendarItems(calendar);
            //                        Console.ReadKey();
            //                        Console.Clear();
            //                        break;
            //                    case 4:
            //                        DispalyAllMonthlyCategorizedCalendarItems(calendar);
            //                        break;
            //                    case 5:
            //                        return;
            //                    default:
            //                        Console.WriteLine("Must use 1-3 when choosing your choice");
            //                        break;
            //                }

            //            } while (true);
            //        }

            //        #region Calendar Items
            //        static void DispalyAllCalendarItems(HomeCalendar calendar)
            //        {

            //            Console.WriteLine(@"Use Cases:
            //1.Retrieve every calendar item:
            //2.Retrieve items within certain date
            //3.Retrieve items filtered by a category");

            //            bool ifValid = int.TryParse(Console.ReadLine(), out int userInput);

            //            switch (userInput)
            //            {
            //                case 1:
            //                    List<CalendarItem> calendarItems = calendar.GetCalendarItems(null, null, false, 0);
            //                    PrintCalendarItemsWithAllValues(calendarItems);
            //                    break;
            //                case 2:
            //                    List<CalendarItem> calendarItemsSortedByTime = calendar.GetCalendarItems(DateTime.Now.AddYears(-6), DateTime.Now, false, 0);
            //                    PrintCalendarItemsWithAllValues(calendarItemsSortedByTime);
            //                    break;
            //                case 3:
            //                    List<CalendarItem> calendarItemsSortedBycategory = calendar.GetCalendarItems(null, null, true, 9);
            //                    PrintCalendarItemsWithAllValues(calendarItemsSortedBycategory);
            //                    break;
            //                default:
            //                    Console.WriteLine("Must enter a value between 1-3");
            //                    break;
            //            }


            //        }
            //        static void PrintCalendarItemsWithAllValues(List<CalendarItem> calendarItems)
            //        {


            //            // print important information
            //            Console.WriteLine("All Calendar Items");
            //            Console.WriteLine("-------------------------------------------------------------------------------------------");
            //            Console.WriteLine($"{"Item",-10} {"StartTime",-20} {"Description",-20} {"Duration",-10} {"Total Time Spent"}");
            //            foreach (CalendarItem ci in calendarItems)
            //            {
            //                Console.WriteLine($"{ci.EventID,-10} {ci.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),-20} {ci.ShortDescription,-20} {ci.DurationInMinutes,-10} {ci.BusyTime}");
            //            }
            //        }
            //        #endregion

            //        #region MonthlyItems
            //        static void DisplayAllCalendarItemsByMonth(List<CalendarItemsByMonth> allItemsPerMonth)
            //        {
            //            // PER MONTH

            //            foreach (CalendarItemsByMonth ci in allItemsPerMonth)
            //            {
            //                Console.WriteLine("Month: " + ci.Month);
            //                Console.WriteLine("All containing items");
            //                Console.WriteLine("-------------------------------------------------------------------------------------------");

            //                foreach (CalendarItem items in ci.Items)
            //                {
            //                    Console.WriteLine($"Description Of Item {items.EventID}: " + items.ShortDescription);
            //                }
            //                Console.WriteLine($"Total time spent on events per month in minutes: {ci.TotalBusyTime} \n");
            //            }
            //        }

            //        static void DispalyAllMonthlyCalendarItems(HomeCalendar calendar)
            //        {

            //            Console.WriteLine(@"Use Cases:
            //1.Retrieve every monthly calendar items
            //2.Retrieve monthly calendar items within certain date
            //3.Retrieve monthly calendar items filtered by a category");

            //            bool ifValid = int.TryParse(Console.ReadLine(), out int userInput);

            //            switch (userInput)
            //            {
            //                case 1:
            //                    List<CalendarItemsByMonth> allItemsPerMonth = calendar.GetCalendarItemsByMonth(null, null, false, 0);
            //                    DisplayAllCalendarItemsByMonth(allItemsPerMonth);
            //                    break;
            //                case 2:
            //                    List<CalendarItemsByMonth> monthlyCalendarItemsSortedByTime = calendar.GetCalendarItemsByMonth(DateTime.Now.AddYears(-6), DateTime.Now, false, 0);
            //                    DisplayAllCalendarItemsByMonth(monthlyCalendarItemsSortedByTime);
            //                    break;
            //                case 3:
            //                    List<CalendarItemsByMonth> monthlyCalendarItemsSortedBycategory = calendar.GetCalendarItemsByMonth(null, null, true, 9);
            //                    DisplayAllCalendarItemsByMonth(monthlyCalendarItemsSortedBycategory);
            //                    break;
            //                default:
            //                    Console.WriteLine("Must enter a value between 1-3");
            //                    break;
            //            }


            //        }
            //        #endregion

            //        #region Categories
            //        static void DisplayAllCalendarItemsByCategory(List<CalendarItemsByCategory> allItemsPerCategory)
            //        {
            //            //PER CATEGORY


            //            foreach (CalendarItemsByCategory ci in allItemsPerCategory)
            //            {
            //                Console.WriteLine("Category: " + ci.Category);
            //                Console.WriteLine("All containing items");

            //                foreach (CalendarItem items in ci.Items)
            //                {
            //                    Console.WriteLine($"Description Of Item {items.EventID}: " + items.ShortDescription);
            //                }
            //                Console.WriteLine($"Total time spent on events per month in minutes: {ci.TotalBusyTime} \n");
            //            }
            //        }

            //        static void DispalyAllCategorizedCalendarItems(HomeCalendar calendar)
            //        {

            //            Console.WriteLine(@"Use Cases:
            //1.Retrieve every monthly calendar items
            //2.Retrieve monthly calendar items within certain date
            //3.Retrieve monthly calendar items filtered by a category");

            //            bool ifValid = int.TryParse(Console.ReadLine(), out int userInput);

            //            switch (userInput)
            //            {
            //                case 1:
            //                    List<CalendarItemsByCategory> allItemsPerCategory = calendar.GetCalendarItemsByCategory(null, null, false, 0);
            //                    DisplayAllCalendarItemsByCategory(allItemsPerCategory);
            //                    break;
            //                case 2:
            //                    List<CalendarItemsByCategory> categoryCalendarItemsSortedByTime = calendar.GetCalendarItemsByCategory(DateTime.Now.AddYears(-6), DateTime.Now, false, 0);
            //                    DisplayAllCalendarItemsByCategory(categoryCalendarItemsSortedByTime);
            //                    break;
            //                case 3:
            //                    List<CalendarItemsByCategory> categoryCalendarItemsSortedBycategory = calendar.GetCalendarItemsByCategory(null, null, true, 9);
            //                    DisplayAllCalendarItemsByCategory(categoryCalendarItemsSortedBycategory);
            //                    break;
            //                default:
            //                    Console.WriteLine("Must enter a value between 1-3");
            //                    break;
            //            }


            //        }

            //        #endregion

            //        #region Dictionary

            //        static void DispalyAllMonthlyCategorizedCalendarItems(HomeCalendar calendar)
            //        {

            //            Console.WriteLine(@"Use Cases:
            //1.Retrieve every monthly calendar items
            //2.Retrieve monthly calendar items within certain date
            //3.Retrieve monthly calendar items filtered by a category");

            //            bool ifValid = int.TryParse(Console.ReadLine(), out int userInput);

            //            switch (userInput)
            //            {
            //                case 1:
            //                    List<Dictionary<string, object>> allCalendarItemsByCategoryAndMonth = calendar.GetCalendarDictionaryByCategoryAndMonth(null, null, false, 0);
            //                    DisplayAllCalendarItemsByCategoryAndMonth(allCalendarItemsByCategoryAndMonth);
            //                    break;
            //                case 2:
            //                    List<Dictionary<string, object>> allCalendarItemsByCategoryAndMonthFilteredByTime = calendar.GetCalendarDictionaryByCategoryAndMonth(DateTime.Now.AddYears(-6), DateTime.Now, false, 0);
            //                    DisplayAllCalendarItemsByCategoryAndMonth(allCalendarItemsByCategoryAndMonthFilteredByTime);
            //                    break;
            //                case 3:
            //                    List<Dictionary<string, object>> allCalendarItemsByCategoryAndMonthFilteredByCategory = calendar.GetCalendarDictionaryByCategoryAndMonth(null, null, true, 9);
            //                    DisplayAllCalendarItemsByCategoryAndMonth(allCalendarItemsByCategoryAndMonthFilteredByCategory);
            //                    break;
            //                default:
            //                    Console.WriteLine("Must enter a value between 1-3");
            //                    break;
            //            }


            //        }
            //        static void DisplayAllCalendarItemsByCategoryAndMonth(List<Dictionary<string, object>> allCalendarItemsByCategoryAndMonth)
            //        {


            //            foreach (Dictionary<string, object> dictionary in allCalendarItemsByCategoryAndMonth)
            //            {
            //                Console.WriteLine("--------------------------------------------------------------------------------------------------------");
            //                Console.WriteLine($"Month: {dictionary["Month"]}");

            //                if (dictionary.ContainsKey("TotalBusyTime"))
            //                {
            //                    Console.WriteLine($"Time spent: {dictionary["TotalBusyTime"]}");
            //                }

            //                foreach (var item in dictionary)
            //                {
            //                    if (item.Key.StartsWith("items:"))
            //                    {
            //                        Console.WriteLine($"Category: {item.Key.Replace("items:", "")}"); //Removing items: from the key inside of the Console.WriteLine



            //                        Console.WriteLine($"{"Item",-10} {"StartTime",-20} {"Description",-20} {"Duration",-10} {"Total Time Spent"}");

            //                        List<CalendarItem> calendarItems = new List<CalendarItem>();
            //                        calendarItems = item.Value as List<CalendarItem>;

            //                        foreach (CalendarItem ci in calendarItems)
            //                        {
            //                            Console.WriteLine($"{ci.EventID,-10} {ci.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),-20} {ci.ShortDescription,-20} {ci.DurationInMinutes,-10} {ci.BusyTime}");
            //                        }
            //                    }
            //                }

            //                if (dictionary["Month"] == "TOTALS")
            //                {

            //                    foreach (var total in dictionary)
            //                    {
            //                        Console.WriteLine($" Category: {total.Key} Total Time Spent: {total.Value}");
            //                    }
            //                }

            //                Console.WriteLine("");

            //            }




            //        }
            //    }
            //}



            //#endregion
        }
    }
}
