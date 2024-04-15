using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarMVP
{
    interface ViewInterface
    {
        void DisplayDB();
        //string GetFileName();
        //string GetFolderName();
        void DisplayMessage(string message);
        void ClosingConfirmation();
        void ShowTypes();
        void ShowCategory();
    }
}
