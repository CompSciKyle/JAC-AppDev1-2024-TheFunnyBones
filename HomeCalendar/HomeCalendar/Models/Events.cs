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
    // ====================================================================
    // CLASS: Events
    //        - A collection of Event items,
    //        - Read / write to file
    //        - etc
    // ====================================================================

    /// <summary>
    /// 
    /// </summary>
    public class Events
    {
        private SQLiteConnection _connection;

        // ====================================================================
        // Properties
        // ====================================================================

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

        public Events(SQLiteConnection connection, bool existingConnection)
        {
            Connection = connection;

            if (existingConnection)
            {

            }
        }
        // ====================================================================
        // Add Event
        // ====================================================================
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
        // ====================================================================
        // Return list of Events
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================

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

