using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarMVP
{
    interface ViewInterfaceForEventsAndCategories
    {
        void DisplayDB(string DBName);
        void DisplayMessage(string message);
        void ClosingConfirmation(object sender, CancelEventArgs e);
        void ShowTypes();
        void ShowCategory();
    }
}
