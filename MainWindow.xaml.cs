using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFAppWithDatabaseTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string name="";
        int age=0;
        string gender="";
        string mysqlconnectionstring = null;
        MySqlConnection conn;
        string query=null;

        public MainWindow()
        {
            InitializeComponent();
            if (checkDBConn())
            {
                dropdownID(); 
            }
        }

        private bool checkDBConn()
        {
            mysqlconnectionstring = "server=localhost;database=sample_database;uid=root;pwd=;";
            using (conn = new MySqlConnection(mysqlconnectionstring))
            {
                try
                {
                    conn.Open();
                    //MessageBox.Show("Connection to database is OK!");
                    conn.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to connect to the database");
                    return false;
                    throw;                    
                }
            }
            return true;
        }

        private void buttonCreateUser_Click(object sender, RoutedEventArgs e)
        {
            //To do: Grey click box if db is not connected

            if (!string.IsNullOrEmpty(formName.Text))
                name = formName.Text;
            else
                name = "No Name";

            if (!string.IsNullOrEmpty(formGender.Text))
                gender = formGender.Text;
            else
                gender = "Genderless";
            
            age = parseThisToInt(formAge.Text);

            if (name == "No Name" || gender == "Genderless" || age == 0)
                MessageBox.Show("Please enter your name, age and gender first before submitting");
            else
            {
                query = "INSERT INTO list_people(NAME,AGE,GENDER) " +
                "VALUES ('" + name + "','" + age + "','" + gender + "');";
                createUserToDB(mysqlconnectionstring, conn, query);                
            }

            dropdownID();

        }
                
        private int parseThisToInt(String textBoxString)
        {
            int parseResult;
            bool isParsable = Int32.TryParse(textBoxString, out parseResult);
            if (isParsable)
                return parseResult;
            else
                return 0;
        }

        private void createUserToDB(string mysqlconnectionstring, MySqlConnection conn, string query)
        {
            mysqlconnectionstring = "server=localhost;database=sample_database;uid=root;pwd=;";
            using (conn = new MySqlConnection(mysqlconnectionstring))
            {
                using (MySqlCommand executeQuery = new MySqlCommand(query, conn))
                {
                    MySqlDataReader Reader;
                    try
                    {
                        conn.Open();
                        Reader = executeQuery.ExecuteReader();
                        conn.Close();
                        MessageBox.Show("The data has been successfully inserted to the database");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Cannot open connection");
                        throw;
                    }
                }
            }

        }

        private void fetchNamesButton_Click(object sender, RoutedEventArgs e)
        {
            //List<names> listOfNames = new List<names>();
            string listHolder = "";
            mysqlconnectionstring = "server=localhost;database=sample_database;uid=root;pwd=;";
            query = "SELECT * FROM list_people";
            nameListBox.Items.Clear();
            using (conn = new MySqlConnection(mysqlconnectionstring))
            {
                using (MySqlCommand executeQuery = new MySqlCommand(query, conn))
                {                    
                    try
                    {
                        conn.Open();
                        MySqlDataReader Reader = executeQuery.ExecuteReader();
                        if (Reader.HasRows)
                        {
                            while (Reader.Read())
                            {
                                //MessageBox.Show(Reader.GetString(1));
                                listHolder = Reader.GetString(0) + "--"
                                    + Reader.GetString(1) + " || " 
                                    + Reader.GetString(2) + " || " 
                                    + Reader.GetString(3);
                                nameListBox.Items.Add(listHolder);
                            }
                            Reader.Close();
                        }
                        MessageBox.Show("Successfully fetched the latest list of names");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error in fetching names. Check database connection");
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            //getNameList(mysqlconnectionstring, conn);

        }

        private void checkDBButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkDBConn())
            {
                MessageBox.Show("Connection to database is OK!");
            }
        }

        private void dropdownID()
        {
            mysqlconnectionstring = "server=localhost;database=sample_database;uid=root;pwd=;";
            query = "SELECT * FROM list_people";
            using (conn = new MySqlConnection(mysqlconnectionstring))
            {
                using (MySqlCommand executeQuery = new MySqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        if (listIDEdit.Items.Count >= 1)
                        {
                            listIDEdit.Items.Clear();
                        }
                        MySqlDataReader Reader = executeQuery.ExecuteReader();
                        if (Reader.HasRows)
                        {
                            while (Reader.Read())
                            {
                                //MessageBox.Show(Reader.GetString(0)); //Did this even happened?
                                listIDEdit.Items.Add(Reader.GetString(0));
                            }
                            Reader.Close();
                        }
                        //MessageBox.Show("Successfully fetched the latest list of names");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error in fetching names. Check database connection");
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void listIDEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void formName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void formAge_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void formGender_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            string appendToQuery = "set ";

            if (!string.IsNullOrEmpty(formNameEdit.Text))
            {
                name = formNameEdit.Text; 
                appendToQuery += $"NAME = '{name}'";
            }                 
            else
            {
                name = "No Name"; 
            }

            if (!string.IsNullOrEmpty(formGenderEdit.Text))
            {
                gender = formGenderEdit.Text;
                appendToQuery += $", GENDER = '{gender}'";
            }
            else
                gender = "Genderless";

            if (!string.IsNullOrEmpty(formAgeEdit.Text))
            {
                age = parseThisToInt(formAgeEdit.Text);
                appendToQuery += $", AGE = '{age}'";    //sampai sini
            }

            query = "update list_people";

            mysqlconnectionstring = "server=localhost;database=sample_database;uid=root;pwd=;";
            using (conn = new MySqlConnection(mysqlconnectionstring))
            {
                using (MySqlCommand executeQuery = new MySqlCommand(query, conn))
                {
                    MySqlDataReader Reader;
                    try
                    {
                        conn.Open();
                        Reader = executeQuery.ExecuteReader();
                        conn.Close();
                        MessageBox.Show("The data has been successfully inserted to the database");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Cannot open connection");
                        throw;
                    }
                }
            }
        }
    }
}
