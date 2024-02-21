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
    // CLASS: Event
    //        - An individual event for calendar program
    // ====================================================================

    /// <summary>
    /// Represents a individual event for the calendar.
    /// </summary>
    public class Event
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// The ID of the event.
        /// </summary>
        /// <value>
        /// A unique identifier for a event.
        /// </value>
        public int Id { get; }
        /// <summary>
        /// The start date of the event.
        /// </summary>
        /// <value>
        /// The date you begin the event.
        /// </value>
        public DateTime StartDateTime { get;  }
        /// <summary>
        /// How long the event will last in minutes.
        /// </summary>
        /// <value>
        /// How long the event takes you in minutes.
        /// </value>
        public Double DurationInMinutes { get; set; }
        /// <summary>
        /// The details about the event.
        /// </summary>
        /// <value>
        /// Describing the event.
        /// </value>
        public String Details { get; set; }
        /// <summary>
        /// The category of where the event belongs in.
        /// </summary>
        /// <value>
        /// Used to group similar events.
        /// </value>
        public int Category { get; set; }

        // ====================================================================
        // Constructor
        //    NB: there is no verification the event category exists in the
        //        categories object
        // ====================================================================
        /// <summary>
        /// Initializes a new instance of the Event class with ID, date, category, duration and details.
        /// </summary>
        /// <param name="id">The unique identifier for the event.</param>
        /// <param name="date">The start date and time of the event.</param>
        /// <param name="category">The category of the event.</param>
        /// <param name="duration">The duration of the event in minutes.</param>
        /// <param name="details">Additional details or description of the event.</param>
        /// <example>
        /// Initializes a new instance of the Event class with ID, date, category, duration and details.
        /// <code>
        /// Event event = new Event(3,DateTime.Now,60.0,"Meeting");
        /// </code>
        /// </example>
        public Event(int id, DateTime date, int category, Double duration, String details)
        {
            this.Id = id;
            this.StartDateTime = date;
            this.Category = category;
            this.DurationInMinutes = duration;
            this.Details = details;
        }

        // ====================================================================
        // Copy constructor - does a deep copy
        // ====================================================================
        /// <summary>
        /// Initializes a new instance of the Event class by using another Event object.
        /// </summary>
        /// <param name="obj">The Event object to copy.</param>
        /// <example>
        /// Intializes a new instance of the event using a existing event.
        /// <code>
        /// Event event = new Event(3,DateTime.Now,60,"Meeting");
        /// Event newEvent = new Event(event);
        /// </code>
        /// </example>
        public Event (Event obj)
        {
            this.Id = obj.Id;
            this.StartDateTime = obj.StartDateTime;
            this.Category = obj.Category;
            this.DurationInMinutes = obj.DurationInMinutes;
            this.Details = obj.Details;
           
        }
    }
}
