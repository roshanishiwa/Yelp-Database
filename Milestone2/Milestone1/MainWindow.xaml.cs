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
using Npgsql;


namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public string bid { get; set; }
            public string businessname { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string address { get; set; }
            public int  numtips { get; set; }
            public double stars { get; set; }
            public int numcheckins { get; set; }
            public double latitude { get; set; }



        }

        public MainWindow()
        {
            InitializeComponent();
            addState();
            addColumns2Grid();
        }

        private string buildConnectionString()
        {
            return "Host=localhost; Username=postgres; Database=yelpdb; password=heba1996";
        }

        private void addState()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            stateList.Items.Add(reader.GetString(0));
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());

                    }
                    finally
                    {

                        connection.Close();
                    }
                }
            }

        }



        private void addColumns2Grid()
        {


            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("businessname");
            col1.Header = "BusinessName";
            col1.Width = 255;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("address");
            col2.Header = "Address";
            col2.Width = 150;
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 150;
            businessGrid.Columns.Add(col3);


            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("state");
            col4.Header = "State";
            col4.Width = 60;
            businessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("latitude");
            col5.Header = "Distance (miles)";
            col5.Width = 150;
            businessGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("stars");
            col6.Header = "Stars";
            col6.Width = 150;
            businessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Binding = new Binding("numtips");
            col7.Header = "# of Tips";
            col7.Width = 150;
            businessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Binding = new Binding("numcheckins");
            col8.Header = "Total Checkins";
            col8.Width = 150;
            businessGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Binding = new Binding("bid");
            col9.Header = "";
            col9.Width = 0;
            businessGrid.Columns.Add(col9);


        }


        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {

            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            myf(reader);

                    }

                    catch (NpgsqlException ex)
                    {

                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error -" + ex.Message.ToString());

                    }

                    finally
                    {

                        connection.Close();
                    
                    }
                
             
                
                }
            
            
            
            
            
            }
        
        
        
        
        }

        private void addCity(NpgsqlDataReader R)
        {
            citiesList.Items.Add(R.GetString(0));
        }
        private void addZipCode(NpgsqlDataReader R)
        {
            zipcodesList.Items.Add(R.GetString(0));
        }

        private void addCategory(NpgsqlDataReader R)
        {
            categoriesList.Items.Add(R.GetString(0));
        }

        private void Todayhour(NpgsqlDataReader R)
        {
            string hours = "Today (" + System.DateTime.Now.DayOfWeek.ToString() + ") : Opens: "
                + R.GetString(0) + " Close: " + R.GetString(1);


            Todayhours.Text = hours;


        }


        private void stateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            citiesList.Items.Clear();
            if (stateList.SelectedIndex > -1)
            {
                string sqlStr = "SELECT distinct city FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "'ORDER BY city";
                executeQuery(sqlStr, addCity);
         
                

            }

        }



        private void addGridRow(NpgsqlDataReader R)
        {
            businessGrid.Items.Add(new Business() { businessname = R.GetString(0), address = R.GetString(1), city = R.GetString(2), state = R.GetString(3), latitude = R.GetDouble(4), stars= R.GetDouble(5), numtips= R.GetInt32(6), numcheckins= R.GetInt32(7), bid= R.GetString(8) } );
        
        }

      private void cityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            if (citiesList.SelectedIndex > -1)
            {
                string sqlStr = "SELECT businessname, state, city, businessid, address, numtips, stars, numcheckins, latitude FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "'AND city ='" + citiesList.SelectedItem.ToString() + "'ORDER BY city;";
                executeQuery(sqlStr, addGridRow);

               
            }

        }
     
       

        private void businessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    BusinessDetails businessWindow = new BusinessDetails(B.bid.ToString());
                    //businessWindow.Show();
                    SelectedBusin.Text=B.businessname;
                    address.Text = B.address + " , " + B.city + " , " + B.state;
                    Today_hours(B);
                }

            }
        }

        private void citiesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zipcodesList.Items.Clear();
            businessGrid.Items.Clear();
            if (stateList.SelectedIndex > -1 && citiesList.SelectedIndex > -1)
            {
                string sqlStr = "SELECT distinct zip FROM business WHERE state = '"
                    + stateList.SelectedItem.ToString()
                    + "' AND city = '" + citiesList.SelectedItem.ToString() + "' ORDER BY zip";
                
                executeQuery(sqlStr, addZipCode);

                /*
                 string sqlStr1 = "SELECT name, state, city, business_id, address FROM business WHERE state = '"
                    + stateList.SelectedItem.ToString() +
                    "'AND city ='" + citiesList.SelectedItem.ToString() + "'ORDER BY city;";
                executeQuery(sqlStr1, addGridRow);
               */

            }
        }

        private void categoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void zipcodesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoriesList.Items.Clear();
            if (stateList.SelectedIndex > -1 && citiesList.SelectedIndex > -1 && zipcodesList.SelectedIndex > -1)
            {
                string sqlStr = "SELECT distinct bc.categoryname " +
                    "FROM business b LEFT JOIN categories bc ON b.businessid = bc.businessid " +
                    "WHERE b.state = '" + stateList.SelectedItem.ToString() + "' AND b.city = '" + citiesList.SelectedItem.ToString() +
                    "' AND b.zip = '" + zipcodesList.SelectedItem.ToString() + "' " +
                    "ORDER BY bc.categoryname; ";
               
                executeQuery(sqlStr, addCategory);
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            businessGrid.Items.Clear();
            if (stateList.SelectedIndex > -1 && citiesList.SelectedIndex > -1 && zipcodesList.SelectedIndex > -1)
            {
                string catWhere = "";
                if (selectedCategoresList.Items.Count > 0)
                {
                    foreach (var category in selectedCategoresList.Items)
                    {
                        catWhere += "'" + category + "',";
                    }
                    catWhere = catWhere.Remove(catWhere.Length - 1);
                    catWhere = "' AND bc.categoryname IN (" + catWhere + ")";
                }
                string sqlStr = "SELECT businessname, address, city, state, latitude, stars, numtips, numcheckins, b.businessid  " +
                "FROM business b LEFT JOIN categories bc ON b.businessid = bc.businessid " +
                "WHERE b.state = '" + stateList.SelectedItem.ToString() +
                "' AND b.city = '" + citiesList.SelectedItem.ToString() +
                "' AND b.zip = '" + zipcodesList.SelectedItem.ToString() +
                catWhere +
                " ORDER BY businessname; ";
                executeQuery(sqlStr, addGridRow);
            }
        }

        private void addCatBtn_Click(object sender, RoutedEventArgs e)
        {
            if (categoriesList.SelectedIndex > -1)
            {
                selectedCategoresList.Items.Add(categoriesList.SelectedItem.ToString());
            }
        }

        private void removeCatBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCategoresList.SelectedIndex > -1)
            {
                selectedCategoresList.Items.Remove(selectedCategoresList.SelectedItem);
            }
        }

        

        private void address_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Today_hours(Business B)
        {
            
                string sqlStr = "SELECT  open::text, close::text FROM public.hours" + 
                               " WHERE businessid = '" + B.bid + "' AND dayofweek =" +
                               "'"  + System.DateTime.Now.DayOfWeek.ToString() + "'" ; 



                executeQuery(sqlStr, Todayhour);
            
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void addTip_Click(object sender, RoutedEventArgs e)
        {

            if (businessGrid.SelectedIndex > -1)
            {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    string TipText_1 = TipText.Text;
                    string sql_str = "INSERT INTO Tip (userID, businessID, tipDate, tipText, likes)" +
                    " VALUES ('4XChL029mKr5hydo79Ljxg', '" + B.bid + " ' , NOW() , '" + TipText_1 + ", '1'";
                    
                    executeQuery(sql_str, null); 

                }
            }

        }

        private void TipText_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }

}

    