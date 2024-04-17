using Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarMVP
{
    class Presenter
    {
        private readonly ViewInterface view;
        private HomeCalendar model;

        public Presenter(ViewInterface v)
        {
            view = v;
        }

        public void NewDB(string filePath, string fileName)
        {
            string fullPath = Path.Combine(filePath, fileName);
            model = new HomeCalendar(fullPath, true);
            view.DisplayDB();
        }

        public void ExsistingDB(string filePath, string fileName)
        {
            string fullPath = Path.Combine(filePath, fileName);
            model = (new HomeCalendar(fileName, false));
            view.DisplayDB();
        }

        public void NewCategory(Category.CategoryType type, string description)
        {
            bool valid = ValidatingCategoryData(type);
            if (!valid)
            {
                view.DisplayMessage("Invalid type");
            }
            else
            {

                try
                {
                    model.categories.Add(description, type);

                }
                catch (Exception ex)
                {
                    view.DisplayMessage(ex.Message);
                }
                view.DisplayMessage("Category has been created");
                view.DisplayDB();
            }
        }

        public void NewEvent(DateTime startDateTime, int categoryId, double durationInMinutes, string details)
        {
            bool valid = ValidatingEventData(startDateTime, categoryId, durationInMinutes);
            if (!valid)
            {
                view.DisplayMessage("Fields are not valid");
            }
            else
            {

                try
                {
                    model.events.Add(startDateTime, categoryId, durationInMinutes, details);

                }
                catch (Exception ex)
                {
                    view.DisplayMessage(ex.Message);
                }
                view.DisplayMessage("Event has been created");
                view.DisplayDB();
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

        public List<Category> GetAllCategoryTypes()
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
