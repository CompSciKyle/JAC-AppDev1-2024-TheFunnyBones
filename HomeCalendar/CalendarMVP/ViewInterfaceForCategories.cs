using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendarMVP;
using Calendar;
using System.ComponentModel;

namespace CalendarMVP
{
    public interface ViewInterfaceForCategories
    { 
    
        void DisplayDB();
        void ShowDbName(string DBName);
        void DisplayMessage(string message);
        void ClosingConfirmation(object sender, CancelEventArgs e);
        void ShowTypes(List<Category.CategoryType> allCategoryTypes);

    }
}
