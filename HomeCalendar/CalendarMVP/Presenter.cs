using Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CalendarMVP
{
    public class Presenter
    {
        private readonly ViewInterfaceForDatabaseConnection viewForDatabase;
        private ViewInterfaceForCalendar viewForCalendar;
        private ViewInterfaceForEvents viewForEvent;
        private ViewInterfaceForCategories viewForCategory;
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
            if (File.Exists(fullPath) || NewDB)
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

        public void NewCategory(Category.CategoryType type, string description)
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
                    model.events.Add(startDateTime, categoryId, durationInMinutes, details);
                }
                catch (Exception ex)
                {
                    viewForEvent.DisplayMessage(ex.Message);
                }
                viewForEvent.DisplayDB();
                viewForCalendar.DisplayMessage("Event has been created");
            }
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


        private List<Category> GetAllCategories()
        {
            List<Category>  allCategories = new List<Category>();   

            if(model != null)
            {
                allCategories = model.categories.List(); 
            }

            return allCategories;
            
        }

        private List<Category.CategoryType> GetAllCategoryTypes() 
        {
            List<Category.CategoryType> allCategoryTypes = new List<Category.CategoryType>();

            if(model != null)
            {
                foreach (Category.CategoryType categoryType in Enum.GetValues(typeof(Category.CategoryType)))
                {
                    allCategoryTypes.Add(categoryType);
                }
            }

            return allCategoryTypes;
        }

    }

}
