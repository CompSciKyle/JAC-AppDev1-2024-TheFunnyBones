using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarMVP
{
    interface ViewInterfaceForEventsAndCategories
    {
        void DisplayDB();
        void ClosingConfirmation();
        void ShowTypes();
        void ShowCategory();
    }
}
