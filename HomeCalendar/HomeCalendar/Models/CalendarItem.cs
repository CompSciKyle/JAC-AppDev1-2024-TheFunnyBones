using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{
    // ====================================================================
    // CLASS: CalendarItem
    //        A single calendar item, includes a Category and an Event
    // ====================================================================
    
    /// <summary>
    /// Represents a item for a calendar.
    /// </summary>
    public class CalendarItem
    {
        /// <summary>
        /// Gets or sets the ID for the category for the calendar item.
        /// </summary>
        /// <value>
        /// To identify a category.
        /// </value>
        public int CategoryID { get; set; }
        /// <summary>
        /// Gets or sets the ID for the event for the calendar item.
        /// </summary>
        /// <value>
        /// To identify a event.
        /// </value>
        public int EventID { get; set; }
        /// <summary>
        /// Gets or sets the start date for the item calendar item.
        /// </summary>
        /// <value>
        /// A date to begin a item.
        /// </value>
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// Gets or sets  the category for the calendar item.
        /// </summary>
        /// <value>
        /// A group of similar items.
        /// </value>
        public String? Category { get; set; }
        /// <summary>
        /// Gets or sets the description for the calendar item.
        /// </summary>
        /// <value>
        /// Used to describe a item.
        /// </value>
        public String? ShortDescription { get; set; }
        /// <summary>
        /// Gets or sets duration of the calendar item.
        /// </summary>
        /// <value>
        /// How long the time will take you in minutes.
        /// </value>
        public Double DurationInMinutes { get; set; }
        /// <summary>
        /// Gets or sets the total duration of time that are in the same day.
        /// </summary>
        /// <value>
        /// Accumulation of time spent on events throughout the day.
        /// </value>
        public Double BusyTime { get; set; }

    }

    /// <summary>
    /// Represents monthly calendar items.
    /// </summary>
    public class CalendarItemsByMonth
    {
        /// <summary>
        /// Gets or sets the month of the calendar items.
        /// </summary>
        /// <value>
        /// Each division of the year.
        /// </value>
        public String? Month { get; set; }
        /// <summary>
        /// Gets or sets the the calendar items per month.
        /// </summary>
        /// <value>
        /// A list of events and categories grouped by a month.
        /// </value>
        public List<CalendarItem>? Items { get; set; }
        /// <summary>
        /// Get or sets the total duration of time of the calendar items that are in the same month.
        /// </summary>
        /// <value>
        /// The total duration of time spent per month on events.
        /// </value>
        public Double TotalBusyTime { get; set; }
    }

    /// <summary>
    /// Represents items that are grouped by catagories.
    /// </summary>
    public class CalendarItemsByCategory
    {
        /// <summary>
        /// Gets or sets the category for the collection of items.
        /// </summary>
        /// <value>
        /// A group of similar items.
        /// </value>
        public String? Category { get; set; }
        /// <summary>
        /// Gets or sets the items for the category.
        /// </summary>
        /// <value>
        /// A list of events and a categories.
        /// </value>
        public List<CalendarItem>? Items { get; set; }
        /// <summary>
        /// Gets or sets the total duration of time for the calendar items that are in the same category.
        /// </summary>
        /// <value>
        /// Total duration of time spent on the list of category items.
        /// </value>
        public Double TotalBusyTime { get; set; }

    }


}
