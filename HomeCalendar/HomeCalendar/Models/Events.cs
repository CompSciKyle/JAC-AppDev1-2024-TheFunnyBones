using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SQLite;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{

    /// <summary>
    /// Represents a collection of events that can be managed, It provides functionality to retrieve events from a database, 
    /// add events, delete and update them as well.
    /// </summary>
    public class Events
    {
        private SQLiteConnection _connection;

        private SQLiteConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        /// <summary>
        /// Initializes an instance of the events class. sets the connection to the database
        /// </summary>
        /// <param name="connection">The connection to the database</param>
        /// <param name="existingConnection">If true ___ and if false connection is set to the database</param>
        public Events(SQLiteConnection connection, bool existingConnection)
        {
            Connection = connection;
        }
        // ====================================================================
        // Add Event
        // ====================================================================
        /// <summary>
        /// Adds a new event to the database.
        /// </summary>
        /// <param name="date">The date of the event</param>
        /// <param name="category">The category id the event is associated woth</param>
        /// <param name="duration">The duration in minutes of the event</param>
        /// <param name="details">The description and details of the event</param>
        public void Add(DateTime date, int category, Double duration, String details)
        {
            var cmd = new SQLiteCommand(Connection);

            cmd.CommandText = "INSERT INTO events(StartDateTime, Details, DurationInMinutes, CategoryId) VALUES (@date, @details, @duration, @categoryid)";
            cmd.Parameters.AddWithValue("@date", date.ToString());
            cmd.Parameters.AddWithValue("@details", details);
            cmd.Parameters.AddWithValue("@duration", duration);
            cmd.Parameters.AddWithValue("@categoryid", category);
            cmd.ExecuteNonQuery();

        }

        // ====================================================================
        // Delete Event
        // ====================================================================
        /// <summary>
        /// Removes an event from the database
        /// </summary>
        /// <param name="Id">The id of the event to delete</param>
        public void Delete(int Id)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);

                cmd.CommandText = "DELETE FROM events WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        /// <summary>
        /// Updates an event from the database
        /// </summary>
        /// <param name="id">The id of the event you want to update</param>
        /// <param name="date">The updated date of the event</param>
        /// <param name="category">The updated category id the event is associated woth</param>
        /// <param name="duration">The updated duration in minutes of the event</param>
        /// <param name="details">The updated description and details of the event</param>
        public void UpdateProperties(int id, DateTime date, int category, Double duration, String details)
        {

            var cmd = new SQLiteCommand(Connection);

            cmd.CommandText = "UPDATE events SET StartDateTime = @date, Details = @details, DurationInMinutes = @duration, CategoryId = @categoryid WHERE Id = @Id";

            cmd.Parameters.AddWithValue("@date", date.ToString());
            cmd.Parameters.AddWithValue("@details", details);
            cmd.Parameters.AddWithValue("@duration", duration);
            cmd.Parameters.AddWithValue("@categoryid", category);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();


        }
        /// <summary>
        /// Creates a list that contains all the events 
        /// </summary>
        /// <returns>A list of all the events</returns>
        public List<Event> List()
        {
            List<Event> newList = new List<Event>();

            var cmd = new SQLiteCommand("SELECT Id, StartDateTime, Details, DurationInMinutes, CategoryId FROM events;", Connection);


            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    DateTime date = Convert.ToDateTime(reader["StartDateTime"]);
                    int category = Convert.ToInt32(reader["CategoryId"]);
                    double duration = Convert.ToDouble(reader["DurationInMinutes"]);
                    string details = Convert.ToString(reader["Details"]);
                    newList.Add(new Event(id, date, category, duration, details));
                }
            }

            return newList;

        }

    }
}

