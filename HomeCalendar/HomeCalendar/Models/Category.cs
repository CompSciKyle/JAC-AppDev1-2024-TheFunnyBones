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
    // CLASS: Category
    //        - An individual category for Calendar program
    //        - Valid category types: Event,AllDayEvent,Holiday
    // ====================================================================
    /// <summary>
    /// Represents a individual category for a calendar items in  a calendar.
    /// </summary>
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets or sets the Id for the category.
        /// </summary>
        /// <value>
        /// A unique identifier for a category.
        /// </value>
        public int Id { get;  }
        /// <summary>
        /// Get or sets the description for the category.
        /// </summary>
        /// <value>
        /// Used to describe a category.
        /// </value>
        public String Description { get; }
        /// <summary>
        /// Get or sets the type of Category.
        /// </summary>
        /// <value>
        /// Identifies the length of the category.
        /// </value>
        public CategoryType Type { get; }
        /// <summary>
        /// Lists all posible types of categories.
        /// </summary>
        public enum CategoryType
        {
            /// <summary>
            /// A occasion throughout a specific amount of time.
            /// </summary>
            Event = 1, // Starts at one because inside of the table its ID is also one
            /// <summary>
            /// A occasion that lasts the whole day.
            /// </summary>
            AllDayEvent,
            /// <summary>
            /// A occasion that can last multiple days or days.
            /// </summary>
            Holiday,
            /// <summary>
            /// When you are free for a certain amount of time.
            /// </summary>
            Availability
        };

        // ====================================================================
        // Constructor
        // ====================================================================
        /// <summary>
        /// Creates a instance of category using ID, description and type.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <param name="description">The description of the category.</param>
        /// <param name="type">The type of the category.</param>
        /// <example>
        /// Initialize a instance of a category using ID, description and type.
        /// <code>
        /// Category birthDates = new Category(3,"Birthdates", CategoryType.Event);
        /// </code>
        /// </example>
        public Category(int id, String description, CategoryType type = CategoryType.Event)
        {
            this.Id = id;
            this.Description = description;
            this.Type = type;
        }

        // ====================================================================
        // Copy Constructor
        // ====================================================================
        /// <summary>
        /// Creates a instance of a category using an existing category.
        /// </summary>
        /// <param name="category">The category you are trying to replicate.</param>
        /// <example>
        /// Initialize a instance of a category using a existing calendar.
        /// <code>
        /// Category birthDates = new Category(3,"Birthdates", CategoryType.Event);
        /// Category newCategory = new Category(birthdates);
        /// </code>
        /// </example>
        public Category(Category category)
        {
            this.Id = category.Id;;
            this.Description = category.Description;
            this.Type = category.Type;
        }
        // ====================================================================
        // String version of object
        // ====================================================================
        /// <summary>
        ///  Returns the description of the category.
        /// </summary>
        /// <returns>The description of the category.</returns>
        /// <example>
        /// Using a ToString for a category.
        /// <code>
        /// Category birthDates = new Category();
        /// birthDates.ToString();
        /// </code>
        /// </example>
        public override string ToString()
        {
            return Description;
        }

    }
}

