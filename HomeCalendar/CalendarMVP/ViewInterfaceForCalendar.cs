using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarMVP
{
    public interface ViewInterfaceForCalendar
    {
        void ShowDbName(string DBName);
        void DisplayMessage(string message);
        void ShowTypes(List<Categories> categories);
    }
}
