using Calendar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using static System.Net.WebRequestMethods;

namespace CalendarMVP
{
    public class Presenter
    {
        private readonly ViewInterfaceForDatabaseConnection viewForDatabase;
        private ViewInterfaceForCalendar viewForCalendar;
        private ViewInterfaceForEvents viewForEvent;
        private ViewInterfaceForCategories viewForCategory;
        private ViewInterfaceForUpdatingEvent viewForUpdate;
        private HomeCalendar model;
        private string _dbName;

        public Presenter(ViewInterfaceForDatabaseConnection v)
        {
            viewForDatabase = v;
        }

        public void ConnectToDB(string filePath, string fileName, bool NewDB)
        {
            _dbName = fileName;
            string fullPath = Path.Combine(filePath, fileName);
            if (System.IO.File.Exists(fullPath) || NewDB)
            {
                model = (new HomeCalendar(fullPath, NewDB));
                viewForDatabase.DisplayDB();
            }
            else
            {
                viewForDatabase.DisplayError("Path does not exist");
            }
        }

        public void RegisterNewView(ViewInterfaceForCalendar v)
        {
            viewForCalendar = v;
            List<Category> allCategories = GetAllCategories();
            viewForCalendar.ShowTypes(allCategories);
            viewForCalendar.ShowDbName(_dbName.Substring(0, _dbName.Length - 3));
        }

        public void RegisterNewView(ViewInterfaceForEvents v)
        {
            viewForEvent = v;
            List<Category> allCategories = GetAllCategories();
            viewForEvent.ShowTypes(allCategories);
            viewForEvent.ShowDbName(_dbName.Substring(0, _dbName.Length - 3));
        }

        public void RegisterNewView(ViewInterfaceForCategories v)
        {
            viewForCategory = v;
            List<Category.CategoryType> allCategoryTypes = GetAllCategoryTypes();
            viewForCategory.ShowTypes(allCategoryTypes);
            viewForCategory.ShowDbName(_dbName.Substring(0, _dbName.Length - 3));
        }
        public void RegisterNewView(ViewInterfaceForUpdatingEvent v)
        {
            viewForUpdate = v;
            List<Category> allCategories = GetAllCategories();
            viewForUpdate.ShowTypes(allCategories);
            viewForUpdate.ShowDbName(_dbName.Substring(0, _dbName.Length - 3));
        }

        public void NewCategory(Category.CategoryType type, string description, bool updateEvent)
        {
            bool valid = ValidatingCategoryTypeData(type);
            if (!valid)
            {
                viewForCategory.DisplayMessage("Invalid type");
            }
            else
            {
                try
                {
                    model.categories.Add(description, type);
                }
                catch (Exception ex)
                {
                    viewForCategory.DisplayMessage(ex.Message);
                }
                //Close window
                viewForCategory.DisplayDB();
                viewForCalendar.DisplayMessage("Category has been created");
                if (updateEvent)
                {
                    List<Category> allCategories = GetAllCategories();
                    viewForEvent.ShowTypes(allCategories);
                }
            }

        }

        public void NewEvent(string startDateTime, Category category, string durationInMinutes, string details)
        {
            double durationInMinutesDouble = Convert.ToDouble(durationInMinutes);

            DateTime startDateTimeToParse = Convert.ToDateTime(startDateTime);

            bool valid = ValidatingEventData(startDateTimeToParse, category.Id, durationInMinutesDouble);
            if (!valid)
            {
                viewForEvent.DisplayMessage("Fields are not valid");
            }
            else
            {

                try
                {
                    model.events.Add(startDateTimeToParse, category.Id, durationInMinutesDouble, details);
                }
                catch (Exception ex)
                {
                    viewForEvent.DisplayMessage(ex.Message);
                }
                viewForEvent.DisplayDB();
                viewForCalendar.UpdateBoard();
                viewForCalendar.DisplayMessage("Event has been created");
            }
        }

        public void PopulateDataGrid(string? startDateTime, string? endDateTime, bool FilterFlag, Category? category, bool monthChecked, bool categoryChecked)
        {
            int CategoryID = 1;
            if (category != null)
            {
                CategoryID = category.Id;
            }
            DateTime Start = new DateTime(1900, 1, 1);
            DateTime End = new DateTime(2500, 1, 1);
            if (startDateTime != "")
            {
                Start = Convert.ToDateTime(startDateTime);
            }
            if (endDateTime != "")
            {
                End = Convert.ToDateTime(endDateTime);
            }


            if (categoryChecked && monthChecked)
            {
                CalendarItemsByMonthAndCategory(Start, End, FilterFlag, CategoryID);
            }
            else if (monthChecked)
            {
                CalendarItemsByMonth(Start, End, FilterFlag, CategoryID);
            }
            else if (categoryChecked)
            {
                CalendarItemsByCategory(Start, End, FilterFlag, CategoryID);
            }
            else
            {
                CalendarItems(Start, End, FilterFlag, CategoryID);
            }
        }
        public void DisplayAll()
        {
            List<CalendarItem> events = model.GetCalendarItems(null, null, false, 1);
            viewForCalendar.DisplayBoard(events);
        }

        private void CalendarItems(DateTime Start, DateTime End, bool FilterFlag, int CategoryID)
        {
            List<CalendarItem> events = model.GetCalendarItems(Start, End, FilterFlag, CategoryID);
            viewForCalendar.DisplayBoard(events);
        }

        private void CalendarItemsByMonth(DateTime Start, DateTime End, bool FilterFlag, int CategoryID)
        {
            List<CalendarItemsByMonth> events = model.GetCalendarItemsByMonth(Start, End, FilterFlag, CategoryID);
            viewForCalendar.DisplayBoardByMonth(events);
        }
        private void CalendarItemsByCategory(DateTime Start, DateTime End, bool FilterFlag, int CategoryID)
        {
            List<CalendarItemsByCategory> events = model.GetCalendarItemsByCategory(Start, End, FilterFlag, CategoryID);
            viewForCalendar.DisplayBoardByCategory(events);
        }
        private void CalendarItemsByMonthAndCategory(DateTime Start, DateTime End, bool FilterFlag, int CategoryID)
        {
            List<Dictionary<string, object>> events = model.GetCalendarDictionaryByCategoryAndMonth(Start, End, FilterFlag, CategoryID);
            viewForCalendar.DisplayBoardDictionary(events);
        }

        private bool ValidatingEventData(DateTime startDateTime, int categoryId, double durationInMinutes)
        {
            bool valid = false;

            if (startDateTime > DateTime.Now && durationInMinutes > 0)
            {
                try
                {
                    model.categories.GetCategoryFromId(categoryId);
                    valid = true;

                }
                catch (Exception ex)
                {
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }

            return valid;

        }

        private bool ValidatingCategoryTypeData(Category.CategoryType type)
        {
            return Enum.IsDefined(typeof(Category.CategoryType), type);
        }


        public List<Category> GetAllCategories()
        {
            List<Category> allCategories = new List<Category>();

            if (model != null)
            {
                allCategories = model.categories.List();
            }

            return allCategories;

        }

        private List<Category.CategoryType> GetAllCategoryTypes()
        {
            List<Category.CategoryType> allCategoryTypes = new List<Category.CategoryType>();

            if (model != null)
            {
                foreach (Category.CategoryType categoryType in Enum.GetValues(typeof(Category.CategoryType)))
                {
                    allCategoryTypes.Add(categoryType);
                }
            }

            return allCategoryTypes;
        }

        public void DeleteEvent(CalendarItem calItem)
        {
            if(calItem.EventID != null)
            {
               model.events.Delete(calItem.EventID);
            }
            else
            {
                viewForCalendar.DisplayMessage("Event not found");
            }
           
        }
        public void UpdateEvent(CalendarItem calItem)
        {
            
        }
        public void PopulateUpdateWindow(CalendarItem calItem)
        {
            string startDateTime = calItem.StartDateTime.ToString("yyyy-MM-dd");
            string startDateHour = calItem.StartDateTime.ToString("H");
            string startDateMinute = calItem.StartDateTime.ToString("mm");
            string startDateSecond = calItem.StartDateTime.ToString("ss");
            Category category = model.categories.GetCategoryFromId(calItem.CategoryID);
            string durationInMinutes = Convert.ToString(calItem.DurationInMinutes);
            string details = calItem.ShortDescription;

            viewForUpdate.PopulateFields(startDateTime, startDateHour, startDateMinute, startDateSecond, category, durationInMinutes, details);
        }

    }

}
