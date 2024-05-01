using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;

namespace CalendarMVP
{
    public interface ViewInterfaceForUpdatingEvent
    {
        void DisplayDB();
        void ShowDbName(string DBName);
        void DisplayMessage(string message);
        void ShowTypes(List<Category> allCategories);
        void PopulateFields(DateTime startDateTime, string startDateHour, string startDateMinute, string startDateSecond, Category category, string durationInMinutes, string details);
    }
}
