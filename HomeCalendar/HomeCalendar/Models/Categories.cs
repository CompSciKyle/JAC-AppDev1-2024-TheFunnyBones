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
        private static String DefaultFileName = "calendarCategories.txt";
        private List<Category> _Categories = new List<Category>();
        private string? _FileName;
        private string? _DirName;
        private SQLiteConnection _connection;

        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets the name of the file associated with the categories.
        /// </summary>
        /// <value>
        /// A name to describe a file.
        /// </value>
        public String? FileName { get { return _FileName; } }

        /// <summary>
        /// Gets the name of the directory associated with the categories.
        /// </summary>
        /// <value>
        /// A name to describe a folder.
        /// </value>
        public String? DirName { get { return _DirName; } }

        public SQLiteConnection Connection
        {
            get
            {
                return _connection;
            }
            private set
            {
                _connection = value;
            }
        }

        // ====================================================================
        // Constructor
        // ====================================================================
        /// <summary>
        /// Creates a instance of categories with default values set inside of it.
        /// </summary>
        public Categories()
        {
            SetCategoriesToDefaults();
        }
        public Categories(SQLiteConnection connection, bool existingConnection)
        {
            if (existingConnection)
            {
                Connection = connection;

            }
            else
            {
                Connection = connection;
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
            //Category? c = _Categories.Find(x => x.Id == i);
            //if (c == null)
            //{
            //    throw new Exception("Cannot find category with id " + i.ToString());
            //}
            //return c;

            var cmd = new SQLiteCommand(Connection);
            cmd.CommandText = "SELECT * FROM categories WHERE Id = @Id";
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
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================

        /// <summary>
        /// Resets all current categories and reads the new ones from the file path provided.
        /// </summary>
        /// <param name="filepath">The path of the file you want to read from.</param>
        /// <example>
        /// Reading the categories from a file.
        /// <code>
        /// Categories categories = new Categories();
        /// categories.ReadFromFile("C:\Users\JohnDoe\Documents\MyNewCategories.txt");
        /// </code>
        /// </example>
        public void ReadFromFile(String? filepath = null)
        {

            // ---------------------------------------------------------------
            // reading from file resets all the current categories,
            // ---------------------------------------------------------------
            _Categories.Clear();

            // ---------------------------------------------------------------
            // reset default dir/filename to null 
            // ... filepath may not be valid, 
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = CalendarFiles.VerifyReadFromFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // If file exists, read it
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        /// <summary>
        /// Writes to the file all the existing categories with the file path that was specified saves the filename info for later.
        /// </summary>
        /// <param name="filepath"> The file path of the file you want to be saved.</param>
        /// <example>
        /// Writing the categories to file.
        /// <code>
        /// Categories categories = new Categories();
        /// categories.SaveToFile("C:\Users\JohnDoe\Documents\MyNewCategories.txt");
        /// </code>
        /// </example>
        public void SaveToFile(String? filepath = null)
        {
            // ---------------------------------------------------------------
            // if file path not specified, set to last read file
            // ---------------------------------------------------------------
            if (filepath == null && DirName != null && FileName != null)
            {
                filepath = DirName + "\\" + FileName;
            }

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = CalendarFiles.VerifyWriteToFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // save as XML
            // ---------------------------------------------------------------
            _WriteXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
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
            // reset any current categories,
            // ---------------------------------------------------------------
            _Categories.Clear();

            // ---------------------------------------------------------------
            // Add Defaults
            // ---------------------------------------------------------------

            /*      
             *  |   Id(PK)   |    Description    |    CategoryType(FK)    |
             */
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

        // ====================================================================
        // Add category
        // ====================================================================
        private void Add(Category category)
        {
            _Categories.Add(category);
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

        //Format = |   Id(PK)   |    Description    |    CategoryType(FK)    |
        public void Add(string desc, Category.CategoryType type)
        {
            int new_num = 1;
            if (_Categories.Count > 0)
            {
                new_num = (from c in _Categories select c.Id).Max();
                new_num++;
            }
            _Categories.Add(new Category(new_num, desc, type));

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
                int i = _Categories.FindIndex(x => x.Id == Id);
                if (i == -1)
                {
                    throw new Exception("Error, ID cannot be found");
                }
                _Categories.RemoveAt(i);

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
            // OLD METHOD
            //List<Category> newList = new List<Category>();

            //foreach (Category category in _Categories)
            //{
            //    newList.Add(new Category(category));
            //}
            //return newList;


            //NEW METHOD

            List<Category> newList = new List<Category>();

            // Open a connection to the database
            //_connection.Open();

            // Create a command to select all categories from the database
            var cmd = new SQLiteCommand("SELECT * FROM categories;", Connection);


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

        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {

            // ---------------------------------------------------------------
            // read the categories from the xml file, and add to this instance
            // ---------------------------------------------------------------
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                foreach (XmlNode category in doc.DocumentElement.ChildNodes)
                {
                    String id = (((XmlElement)category).GetAttributeNode("ID")).InnerText;
                    String typestring = (((XmlElement)category).GetAttributeNode("type")).InnerText;
                    String desc = ((XmlElement)category).InnerText;

                    Category.CategoryType type;
                    switch (typestring.ToLower())
                    {
                        case "event":
                            type = Category.CategoryType.Event;
                            break;
                        case "alldayevent":
                            type = Category.CategoryType.AllDayEvent;
                            break;
                        case "holiday":
                            type = Category.CategoryType.Holiday;
                            break;
                        case "availability":
                            type = Category.CategoryType.Availability;
                            break;
                        default:
                            type = Category.CategoryType.Event;
                            break;
                    }
                    this.Add(new Category(int.Parse(id), desc, type));
                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadXMLFile: Reading XML " + e.Message);
            }

        }


        // ====================================================================
        // write all categories in our list to XML file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            try
            {
                // create top level element of categories
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Categories></Categories>");

                // foreach Category, create an new xml element
                foreach (Category cat in _Categories)
                {
                    XmlElement ele = doc.CreateElement("Category");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = cat.Id.ToString();
                    ele.SetAttributeNode(attr);
                    XmlAttribute type = doc.CreateAttribute("type");
                    type.Value = cat.Type.ToString();
                    ele.SetAttributeNode(type);

                    XmlText text = doc.CreateTextNode(cat.Description);
                    doc.DocumentElement.AppendChild(ele);
                    doc.DocumentElement.LastChild.AppendChild(text);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("_WriteXMLFile: Reading XML " + e.Message);
            }

        }

    }
}

