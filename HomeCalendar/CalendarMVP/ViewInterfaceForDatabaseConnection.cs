using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarMVP
{
    public interface ViewInterfaceForDatabaseConnection
    {
        void DisplayDB();
        void DisplayError(string message);
    }
}
