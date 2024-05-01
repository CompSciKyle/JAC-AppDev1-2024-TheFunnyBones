using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;

namespace CalendarMVP
{
    internal class ViewInterfaceForUpdatingEvent
    {
        void DisplayDB();
        void ShowDbName(string DBName);
        void DisplayMessage(string message);
        void ShowTypes(List<Category> allCategories);
    }
}
