using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;

namespace CalendarMVP
{
    public interface ViewInterfaceForEvents
    {
        void DisplayDB();
        void DisplayMessage(string message);
        void ClosingConfirmation();
        void ShowTypes(List<Category> allCategories);
    }
}
