using Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly ViewInterfaceForCalendar viewForCalendar;
        private readonly ViewInterfaceForEventsAndCategories viewForEventAndCategories;
        private HomeCalendar model;

        public Presenter(ViewInterfaceForDatabaseConnection v)
        {
            viewForDatabase = v;
        }

        public void NewDB(string filePath, string fileName)
        {
            string fullPath = Path.Combine(filePath, fileName);
            if (File.Exists(fullPath))
            {
                model = (new HomeCalendar(fullPath, true));
                viewForDatabase.DisplayDB();
            }
            else
            {
                viewForDatabase.DisplayError("Path does not exist");
            }
        }

        public void ExistingDB(string filePath, string fileName)
        {
            string fullPath = Path.Combine(filePath, fileName);
            if (File.Exists(fullPath))
            {
                model = (new HomeCalendar(fullPath, false));
            }
            else
            {
                viewForDatabase.DisplayError("Path does not exist");
            }
            viewForDatabase.DisplayDB();
        }

        public void NewCategory(Category.CategoryType type, string description)
        {
            bool valid = ValidatingCategoryData(type);
            if (!valid)
            {
                viewForEventAndCategories.DisplayMessage("Invalid type");
            }
            else
            {

                try
                {
                    model.categories.Add(description, type);

                }
                catch (Exception ex)
                {
                    viewForEventAndCategories.DisplayMessage(ex.Message);
                }
                //Close window
                viewForEventAndCategories.DisplayDB();
                viewForCalendar.DisplayMessage("Category has been created");
            }
        }

        public void NewEvent(DateTime startDateTime, int categoryId, double durationInMinutes, string details)
        {
            bool valid = ValidatingEventData(startDateTime, categoryId, durationInMinutes);
            if (!valid)
            {
                viewForEventAndCategories.DisplayMessage("Fields are not valid");
            }
            else
            {

                try
                {
                    model.events.Add(startDateTime, categoryId, durationInMinutes, details);

                }
                catch (Exception ex)
                {
                    viewForEventAndCategories.DisplayMessage(ex.Message);
                }
                viewForEventAndCategories.DisplayDB();
                viewForCalendar.DisplayMessage("Event has been created");
            }
        }

        private bool ValidatingEventData(DateTime startDateTime, int categoryId, double durationInMinutes)
        {
            bool valid = false;

            if (startDateTime < DateTime.Now && durationInMinutes > 0)
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

        private bool ValidatingCategoryData(Category.CategoryType type)
        {

            return Enum.IsDefined(typeof(Category.CategoryType), type);

        }

        public List<Category> GetAllCategories()
        {
            List<Category>  allCategories = new List<Category>();   

            if(model != null)
            {
                allCategories = model.categories.List(); 
            }

            return allCategories;
            
        }

    }

}
