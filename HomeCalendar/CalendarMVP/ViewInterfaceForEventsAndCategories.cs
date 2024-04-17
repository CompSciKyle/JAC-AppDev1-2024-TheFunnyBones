using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarMVP
{
    public interface ViewInterfaceForEventsAndCategories
    {
        void DisplayDB();
        void DisplayMessage(string message);
        void ClosingConfirmation();
        void ShowTypes();
        void ShowCategory();
    }
}
