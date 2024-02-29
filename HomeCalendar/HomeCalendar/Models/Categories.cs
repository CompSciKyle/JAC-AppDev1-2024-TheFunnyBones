using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Linq.Expressions;
using System.Data.SQLite;
using System.Configuration;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{
    // ====================================================================
    // CLASS: categories
    //        - A collection of category items,
    //        - Read / write to file
    //        - etc
    // ====================================================================

    /// <summary>
    /// Represents a collection of categories that can be managed, It provides functionality to retrieve categories from reading from a file,
    /// and can create them by writing to a file and save it, setting default ones and adding and deleting them.
    /// </summary>
    public class Categories
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

        public Categories(SQLiteConnection connection, bool existingConnection)
        {
            Connection = connection;

            if (existingConnection)
            {
                SetCategoriesToDefaults();
            }

        }

        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================
        /// <summary>
        /// Gets a specific category from the list of categories by the specified ID.
        /// </summary>
        /// <param name="i">Represents the id of category you want to retrieve.</param>
        /// <returns>The category with the id you inputed.</returns>
        /// <exception cref="Exception">Thrown when the category id cannot be found.</exception>
        /// <example>
        /// Finding a category by ID.
        /// <code>
        /// Categories categories = new Categories();
        /// Category myCategory = categories.GetCategoryFromId(4);
        /// </code>
        /// </example>
        public Category GetCategoryFromId(int i)
        {

            var cmd = new SQLiteCommand(Connection);
            cmd.CommandText = "SELECT Id, Description,TypeId FROM categories WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", i);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    int categoryId = Convert.ToInt32(reader["Id"]);
                    string description = Convert.ToString(reader["Description"]);
                    Category.CategoryType type = (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), Convert.ToString(reader["TypeId"]));
                    return new Category(categoryId, description, type);
                }
                else
                {
                    throw new Exception($"Cannot find category with id {i}");
                }
            }
        }


        // ====================================================================
        // set categories to default
        // ====================================================================
        /// <summary>
        /// Resets the list of categories back to the default values.
        /// </summary>
        /// <example>
        /// Setting the categories back to default.
        /// <code>
        /// Categories categories = new Categories();
        /// categories.Add("Birthdays", Category.CategoryType.Event);
        /// categories.SetCategoriesToDefaults();
        /// </code>
        /// </example>
        public void SetCategoriesToDefaults()
        {

            // ---------------------------------------------------------------
            // Reset the Tables
            // ---------------------------------------------------------------

            var cmd = new SQLiteCommand(Connection);

            cmd.CommandText = "DELETE FROM categories";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM categoryTypes";
            cmd.ExecuteNonQuery();

            // ---------------------------------------------------------------
            // Add Defaults
            // ---------------------------------------------------------------

            foreach (string name in Enum.GetNames(typeof(Category.CategoryType)))
            {
                cmd.CommandText = @"INSERT INTO categoryTypes(Description) VALUES (@desc)";
                cmd.Parameters.AddWithValue("@desc", name);
                cmd.ExecuteNonQuery();
            }

            Add("School", Category.CategoryType.Event);
            Add("Personal", Category.CategoryType.Event);
            Add("VideoGames", Category.CategoryType.Event);
            Add("Medical", Category.CategoryType.Event);
            Add("Sleep", Category.CategoryType.Event);
            Add("Vacation", Category.CategoryType.AllDayEvent);
            Add("Travel days", Category.CategoryType.AllDayEvent);
            Add("Canadian Holidays", Category.CategoryType.Holiday);
            Add("US Holidays", Category.CategoryType.Holiday);
        }

        /// <summary>
        /// Adds a new category to the list of categories
        /// </summary>
        /// <param name="desc">A description of the category being added.</param>
        /// <param name="type">The type of category you want to add.</param>
        /// <example>
        /// Adding a new category to the list of categories.
        /// <code>
        /// Categories categories = new Categories();
        /// categories.Add("Birthdays", Category.CategoryType.Event);
        /// </code>
        /// </example>
        /// 
        public void Add(string desc, Category.CategoryType type)
        {

            var cmd = new SQLiteCommand(Connection);

            cmd.CommandText = "INSERT INTO categories(Description, TypeId) VALUES (@desc, @type)";
            cmd.Parameters.AddWithValue("@desc", desc);
            cmd.Parameters.AddWithValue("@type", (int)type);
            cmd.ExecuteNonQuery();

        }

        public void UpdateProperties(int id, string description, Category.CategoryType type)
        {

            var cmd = new SQLiteCommand(Connection);

            cmd.CommandText = "UPDATE categories SET Description = @Description, TypeId = @TypeId WHERE Id = @Id";

            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@TypeId", (int)type);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();


        }


        // ====================================================================
        // Delete category
        // ====================================================================
        /// <summary>
        /// Allows you to delete the category with the specified Id from the list of catogries.
        /// </summary>
        /// <param name="Id">The Id of that category you want to delete.</param>
        /// <example>
        /// The following example demonstrates how to delete a category with the specified ID.
        /// <code>
        /// Categories categories = new Categories();
        /// categories.Delete(5);
        /// </code>
        /// </example>
        public void Delete(int Id)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);

                cmd.CommandText = "DELETE FROM events WHERE CategoryId = @Id";
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM categories WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================

        /// <summary>
        /// Creates a copy of the list with all the categories.
        /// </summary>
        /// <returns>A duplicated list of all the categories.</returns>
        /// <example>
        /// Duplicating the categories to a new list.
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories();
        /// categories.Add("Birthdays", Category.CategoryType.Event);
        /// List<Category>newList = categories.List();
        /// ]]>
        /// </code>
        /// </example>
        public List<Category> List()
        {

            List<Category> newList = new List<Category>();

            var cmd = new SQLiteCommand("SELECT Id, Description, TypeId FROM categories;", Connection);


            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string description = Convert.ToString(reader["Description"]);
                    Category.CategoryType type = (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), Convert.ToString(reader["TypeId"]));
                    newList.Add(new Category(id, description, type));
                }
            }

            return newList;

        }


    }
}

