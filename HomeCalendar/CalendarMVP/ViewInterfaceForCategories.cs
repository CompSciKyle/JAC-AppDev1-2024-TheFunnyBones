using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendarMVP;
using Calendar;

namespace CalendarMVP
{
    public interface ViewInterfaceForCategories
    { 
    
        void DisplayDB(string DBName);
        void DisplayMessage(string message);
        void ClosingConfirmation();
        void ShowTypes(List<Category.CategoryType> allCategoryTypes);

    }
}
