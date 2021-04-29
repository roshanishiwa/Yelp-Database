﻿using System;
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


namespace YelpDB
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
            public int numtips { get; set; }
            public double stars { get; set; }
            public int numcheckins { get; set; }
            public double distance { get; set; }
            //public double latitude { get; set; }
            //public double longitude { get; set; }

        }

        public class Users
        {
            //public string userID { get; set; }
            public string username { get; set; }
            //public int fans { get; set; }
            public double averageStars { get; set; }
            //public int tipCount { get; set; }
            //public int funny { get; set; }
            //public int cool { get; set; }
            //public int helpful { get; set; }
            public string yelpSince { get; set; }
            public int totalLikes { get; set; }
            //public float latitude { get; set; }
            //public float longitude { get; set; }

        }

        /// <summary>
        /// A class to store information for friend tips table
        /// </summary>
        private class FriendTips
        {
            public string username { get; set; }
            public string businessName { get; set; }
            public string city { get; set; }
            public string text { get; set; }
            public string date { get; set; }
            public int likes { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            addColumnsFriendGrid();
            addColumnsFriendTipGrid();
            addState();
            addColumns2Grid();
        }

        private string buildConnectionString()
        {
            return "Host=localhost; Username=postgres; Database=yelpdb; password=birth161998";
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
            col5.Binding = new Binding("distance");
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

        private double distance(double lat1, double lon1, double lat2, double lon2)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                return (dist);
            }
        }

        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        private void addGridRow(NpgsqlDataReader R)
        {
            double bLat = R.GetDouble(4);
            double bLong = R.GetDouble(5);
            double uLat = 0;
            double uLong = 0;
            try
            {
                uLat = Convert.ToDouble(latBox.Text);
                uLong = Convert.ToDouble(longBox.Text);
            }
            catch
            {
                uLat = 0;
                uLong = 0;
            }
            
            double dis = distance(bLat, bLong, uLat, uLong);

            //businessGrid.Items.Add(new Business() { businessname = R.GetString(0), address = R.GetString(4), city = R.GetString(2), state = R.GetString(1), latitude = R.GetDouble(8), stars = R.GetDouble(6), numtips = R.GetInt32(5), numcheckins = R.GetInt32(7), bid = R.GetString(3) });
            businessGrid.Items.Add(new Business() { businessname = R.GetString(0), address = R.GetString(1), city = R.GetString(2), state = R.GetString(3), distance = dis, stars = R.GetDouble(6), numtips = R.GetInt32(7), numcheckins = R.GetInt32(8), bid = R.GetString(9) });
        }


        private void cityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            if (citiesList.SelectedIndex > -1)
            {
                string sqlStr = "SELECT businessname, state, city, businessid, address, numtips, stars, numcheckins, latitude, longitude FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "'AND city ='" + citiesList.SelectedItem.ToString() + "'ORDER BY city;";
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
                    SelectedBusin.Text = B.businessname;
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

        /*
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
        */

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
                        //  catWhere += "'" + category + "',";
                        catWhere += " AND  b.businessid IN(SELECT c.businessid FROM categories c where categoryname = '" + category + "')";
                    }

                    //catWhere = catWhere.Remove(catWhere.Length - 1);
                    //catWhere = "' AND bc.categoryname IN (" + catWhere + ")";
                }
                string sqlStr = "SELECT businessname, address, city, state, latitude, longitude, stars, numtips, numcheckins, b.businessid  " +
                "FROM business b LEFT JOIN categories bc ON b.businessid = bc.businessid " +
                "WHERE b.state = '" + stateList.SelectedItem.ToString() +
                "' AND b.city = '" + citiesList.SelectedItem.ToString() +
                "' AND b.zip = '" + zipcodesList.SelectedItem.ToString() + "'" +
                catWhere +
                " Group By businessname, address, city, state, latitude, stars, numtips, numcheckins, b.businessid " +
                " ORDER BY businessname; ";
                Console.WriteLine(sqlStr);
                executeQuery(sqlStr, addGridRow);
                //  .. businessid IN (SELECT businessid FROM category where category = '+ cat1 +') 

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
                           "'" + System.DateTime.Now.DayOfWeek.ToString() + "'";

            executeQuery(sqlStr, Todayhour);

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (businessGrid.SelectedIndex > -1)
            {


                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    ShowTipsWindow1 wind = new ShowTipsWindow1(B.bid);
                    wind.Show();

                    //string TipText_1 = TipText.Text;
                    // string sql_str = "INSERT INTO Tip (userID, businessID, tipDate, tipText, likes)" +
                    //  " VALUES ('4XChL029mKr5hydo79Ljxg', '" + B.bid + " ' , NOW() , '" + TipText_1 + ", '1'";

                    // executeQuery(sql_str, null);

                }

            }

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            businessGrid.Items.Clear();

            // filtering categories back in
            String categories = "";
            for (int i = selectedCategoresList.Items.Count; i > 0; i--)//builds category string
            {
                categories += (" AND categoryName = '" + selectedCategoresList.Items[i - 1] + "'");
            }

            // now need to filter attributes
            String attributes = "";
            if (creditCards.IsChecked == true)
            {
                attributes += (" AND (A.attributeName = 'BusinessAcceptsCreditCards' AND A.value = 'true')");
            }
            if (reservations.IsChecked == true)
            {
                attributes += (" AND (A.attributeName = 'RestrauntsReservations' AND A.value = 'true')");
            }
            if (wheelchair.IsChecked == true)
            {
                attributes += (" AND (A.attributeName = 'WheelchairAccessible' AND A.value = 'true')");
            }
            if (outdoorSeat.IsChecked == true)
            {
                attributes += (" AND (A.attributeName = 'OutdoorSeating' AND A.value = 'true')");
            }
            if (kidFriendly.IsChecked == true)
            {
                attributes += (" AND (A.attributeName = 'GoodForKids' AND A.value = 'true')");
            }
            if (groupFriendly.IsChecked == true)
            {
                attributes += (" AND (A.attributeName = 'RestaurantsGoodForGroups' AND A.value = 'true')");
            }
            if (delivery.IsChecked == true)
            {
                attributes += (" AND (A.attributeName = 'RestaurantsDelivery' AND A.value = 'true')");
            }
            if (takeout.IsChecked == true)
            {
                attributes += (" AND (A.attributeName = 'RestaurantsTakeOut' AND A.value = 'true')");
            }
            if (wifi.IsChecked == true)
            {
                attributes += (" AND (A.attributeName = 'WiFi' AND A.value = 'true')");
            }
            if (bikeParking.IsChecked == true)
            {
                attributes += (" AND (A.attributeName = 'BikeParking' AND A.value = 'true')");
            }

            // filter through menu attributes
            String menu = "";
            if (breakfast.IsChecked == true)
            {
                menu += (" AND (A.attributeName = 'breakfast' AND A.value = 'true')");
            }
            if (Lunch.IsChecked == true)
            {
                menu += (" AND (A.attributeName = 'lunch' AND A.value = 'true')");
            }
            if (brunch.IsChecked == true)
            {
                menu += (" AND (A.attributeName = 'brunch' AND A.value = 'true')");
            }
            if (dinner.IsChecked == true)
            {
                menu += (" AND (A.attributeName = 'dinner' AND A.value = 'true')");
            }
            if (dessert.IsChecked == true)
            {
                menu += (" AND (A.attributeName = 'dessert' AND A.value = 'true')");
            }
            if (lateNight.IsChecked == true)
            {
                menu += (" AND (A.attributeName = 'latenight' AND A.value = 'true')");
            }

            // filter prices
            String prices = "";

            if (!(p1.IsChecked == false && p2.IsChecked == false && p3.IsChecked == false && p4.IsChecked == false))
            {
                prices += (" AND (A.attributeName = 'RestaurantsPriceRange2' AND (");

                if (p1.IsChecked == true && p2.IsChecked == true && p3.IsChecked == true && p4.IsChecked == true)
                {
                    prices += ("A.value = '1' OR A.value = '2' OR A.value = '3' OR A.value = '4'))");
                }
                else if (p1.IsChecked == true && p2.IsChecked == true && p3.IsChecked == true && p4.IsChecked == false)
                {
                    prices += ("A.value = '1' OR A.value = '2' OR A.value = '3'))");
                }
                else if (p1.IsChecked == true && p2.IsChecked == true && p3.IsChecked == false && p4.IsChecked == false)
                {
                    prices += ("A.value = '1' OR A.value = '2'))");
                }
                else if (p1.IsChecked == true && p2.IsChecked == false && p3.IsChecked == false && p4.IsChecked == false)
                {
                    prices += ("A.value = '1'))");
                }
                else if (p1.IsChecked == false && p2.IsChecked == true && p3.IsChecked == true && p4.IsChecked == true)
                {
                    prices += ("A.value = '2' OR A.value = '3' OR A.value = '4'))");
                }
                else if (p1.IsChecked == false && p2.IsChecked == true && p3.IsChecked == true && p4.IsChecked == false)
                {
                    prices += ("A.value = '2' OR A.value = '3'))");
                }
                else if (p1.IsChecked == false && p2.IsChecked == true && p3.IsChecked == false && p4.IsChecked == false)
                {
                    prices += ("A.value = '2'))");
                }
                else if (p1.IsChecked == false && p2.IsChecked == false && p3.IsChecked == true && p4.IsChecked == true)
                {
                    prices += ("A.value = '3' OR A.value = '4'))");
                }
                else if (p1.IsChecked == false && p2.IsChecked == false && p3.IsChecked == true && p4.IsChecked == false)
                {
                    prices += ("A.value = '3'))");
                }
                else if (p1.IsChecked == false && p2.IsChecked == false && p3.IsChecked == false && p4.IsChecked == true)
                {
                    prices += ("A.value = '4'))");
                }
            }

            // append the rest of the query
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    String sqlStr = ("SELECT b.businessname, b.state, b.city, b.businessid, b.address, b.numtips, b.stars, b.numcheckins, b.latitude FROM Business as b ");
                    if (categories.Length > 0)
                    {
                        sqlStr += ("JOIN Categories as c ON b.businessID=c.businessID ");
                    }

                    if (attributes.Length > 0 || menu.Length > 0 || prices.Length > 0)
                    {
                        sqlStr += ("JOIN Attributes as A ON B.businessid=A.businessid ");
                    }

                    sqlStr += ("WHERE state ='" + stateList.SelectedItem.ToString() + "' AND city ='" + citiesList.SelectedItem.ToString() + "' AND zip ='" + zipcodesList.SelectedItem.ToString() + "'");
                    if (categories.Length > 0)
                    {
                        sqlStr += (" " + categories.ToString());
                    }
                    if (attributes.Length > 0)
                    {
                        sqlStr += (" " + attributes.ToString());
                    }
                    if (menu.Length > 0)
                    {
                        sqlStr += (" " + menu.ToString());
                    }
                    if (prices.Length > 0)
                    {
                        sqlStr += (" " + prices.ToString());
                    }

                    if (sortby.SelectedIndex != 5 && sortby.SelectedIndex != -1)
                    {
                        sqlStr += (" ORDER BY ");
                        switch (sortby.SelectedIndex)
                        {
                            case 0:
                                {
                                    sqlStr += ("b.businessname");
                                    break;
                                }
                            case 1:
                                {
                                    sqlStr += ("b.stars DESC");
                                    break;
                                }
                            case 2:
                                {
                                    sqlStr += ("b.numtips DESC");
                                    break;
                                }
                            case 3:
                                {
                                    sqlStr += ("b.numcheckins DESC");
                                    break;
                                }
                            case 4:
                                {
                                    sqlStr += ("b.latitude DESC");
                                    break;
                                }
                        }
                    }
                    sqlStr += (";");
                    command.CommandText = sqlStr.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Business newBusiness = new Business();
                            newBusiness.bid = reader.GetString(3);
                            newBusiness.businessname = reader.GetString(0);
                            newBusiness.state = reader.GetString(1);
                            newBusiness.city = reader.GetString(2);
                            newBusiness.address = reader.GetString(4);
                            newBusiness.numtips = reader.GetInt32(5);
                            newBusiness.stars = reader.GetDouble(6);
                            newBusiness.numcheckins = reader.GetInt32(7);
                            newBusiness.distance = reader.GetDouble(8);
                            //newBusiness.longitude = reader.GetDouble(9);
                            //newBusiness.distance = temp.calc_dist(usrlat, usrlong);//needs user long and lat to calculate
                            businessGrid.Items.Add(newBusiness);//adds record to display
                        }
                    }
                }

                connection.Close();
            }
        }

        private void checkInsButton_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                CheckinChart checkIns = new CheckinChart(((Business)businessGrid.SelectedItem).bid);
                checkIns.ShowDialog();
            }
        }

        // ----------------- Hailey's codes -----------------
        /// <summary>
        /// Enter username in the text box to search for all userID that has the same username
        /// ************* Very slow ***************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Users_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            string username = null;
            username = this.Enter_Users_Name.Text;
            idBox.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnectionString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT userID FROM Users WHERE username ='" + username + "';";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            idBox.Items.Add(reader.GetString(0));
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        // System.Windows.MessageBox.Show("SQL Error -" + ex.Message.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }


        /// <summary>
        /// set up for displaying information
        /// </summary>
        /// <param></param>

        private void getName(NpgsqlDataReader R)
        {
            nameBox.Text = R.GetString(0);
        }

        private void getStars(NpgsqlDataReader R)
        {
            starsBox.Text = R.GetDouble(0).ToString();
        }

        private void getFans(NpgsqlDataReader R)
        {
            fanBox.Text = R.GetInt32(0).ToString();
        }

        private void getYelp(NpgsqlDataReader R)
        {
            yelpBox.Text = R.GetTimeStamp(0).ToString();
        }

        private void getFun(NpgsqlDataReader R)
        {
            funBox.Text = R.GetInt32(0).ToString();
        }

        private void getCool(NpgsqlDataReader R)
        {
            coolBox.Text = R.GetInt32(0).ToString();
        }

        private void getHelp(NpgsqlDataReader R)
        {
            helpBox.Text = R.GetInt32(0).ToString();
        }

        private void getTotalLikes(NpgsqlDataReader R)
        {
            try
            {
                totalLikesBox.Text = R.GetInt32(0).ToString();
            }
            catch
            {
                Console.WriteLine("null");
            }
        }

        private void getLat(NpgsqlDataReader R)
        {
            try
            {
                latBox.Text = R.GetDouble(0).ToString();
            }
            catch
            {
                latBox.Text = "0.0";
            }
        }

        private void getLong(NpgsqlDataReader R)
        {
            try
            {
                longBox.Text = R.GetDouble(0).ToString();
            }
            catch
            {
                longBox.Text = "0.0";
            }
        }

        private void getTipCount(NpgsqlDataReader R)
        {
            tipCountBox.Text = R.GetInt32(0).ToString();
        }

        /// <summary>
        /// Update likes for the selected user
        /// </summary>
        /// <param name="userid"></param>
        private void updateTotalLikes(string userid)
        {
            using (var conn = new NpgsqlConnection(buildConnectionString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Users SET totalLikes = Temp.likeCount FROM " +
                        "(SELECT Tip.userID, SUM(likes) AS likeCount FROM Tip GROUP BY Tip.userID) AS Temp " +
                        "WHERE Users.userID = Temp.userID AND Users.userID ='" + userid + "'; ";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        // System.Windows.MessageBox.Show("SQL Error -" + ex.Message.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }

        }

        //private void updateTotalLikes()
        //{
        //    using (var conn = new NpgsqlConnection(buildConnectionString()))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand())
        //        {
        //            cmd.Connection = conn;
        //            cmd.CommandText = "UPDATE Users SET totalLikes = Temp.likeCount FROM " +
        //                "(SELECT Tip.userID, SUM(likes) AS likeCount FROM Tip GROUP BY Tip.userID) AS Temp " +
        //                "WHERE Users.userID = Temp.userID; ";
        //            try
        //            {
        //                var reader = cmd.ExecuteReader();
        //            }
        //            catch (NpgsqlException ex)
        //            {
        //                Console.WriteLine(ex.Message.ToString());
        //                // System.Windows.MessageBox.Show("SQL Error -" + ex.Message.ToString());
        //            }
        //            finally
        //            {
        //                conn.Close();
        //            }

        //        }
        //    }

        //}


        /// <summary>
        /// Adding Columns into Friend Table
        /// </summary>
        private void addColumnsFriendGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("username");
            col1.Header = "Name";
            col1.Width = 100;
            friendGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("totalLikes");
            col2.Header = "Total Likes";
            col2.Width = 60;
            friendGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("averageStars");
            col3.Header = "Avg Stars";
            col3.Width = 60;
            friendGrid.Columns.Add(col3);


            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("yelpSince");
            col4.Header = "Yelp Since";
            col4.Width = 160;
            friendGrid.Columns.Add(col4);

        }

        /// <summary>
        /// add row helper
        /// </summary>
        /// <param name="R"></param>
        private void addGridRowToFriendHelper(NpgsqlDataReader R)
        {
            friendGrid.Items.Add(new Users() { username = R.GetString(0), totalLikes = R.GetInt32(1), averageStars = R.GetDouble(2), yelpSince = R.GetTimeStamp(3).ToString() });
        }
        /// <summary>
        /// Add new rows for each friend the selected user has
        /// </summary>
        /// <param name="R"></param>
        private void addGridRowToFriendTable(NpgsqlDataReader R)
        {
            // friendGrid.Items.Add(new Users() { username = R.GetString(0), totalLikes = R.GetInt32(1), averageStars = R.GetDouble(2), yelpSince = R.GetTimeStamp(3).ToString() });
            string sqlString = "SELECT username, totalLikes, average_stars, yelpSince FROM Users," +
                "(SELECT Tip.userID, SUM(likes) AS likeCount FROM Tip GROUP BY Tip.userID) AS Temp " +
                "WHERE Users.userID = Temp.userID AND totalLikes = likeCount AND Users.userID = '" + R.GetString(0) + "'; ";
            executeQuery(sqlString, addGridRowToFriendHelper);
        }

        private void addGridRowToFriendTipHelper(NpgsqlDataReader R)
        {
            friendTipGrid.Items.Add(new FriendTips() { username = R.GetString(0), businessName = R.GetString(1), city = R.GetString(2), text = R.GetString(3), date = R.GetTimeStamp(4).ToString(), likes = R.GetInt32(5) });
        }

        private void addGridRowToFriendTipTable(NpgsqlDataReader R)
        {
            // friendGrid.Items.Add(new Users() { username = R.GetString(0), totalLikes = R.GetInt32(1), averageStars = R.GetDouble(2), yelpSince = R.GetTimeStamp(3).ToString() });
            string sqlString = "SELECT username, businessName, city, tipText, tipDate, likes FROM Tip, Business, Users " +
                "INNER JOIN(SELECT userID, max(tipDate) AS maxDate FROM Tip GROUP BY userID) AS T " +
                "ON T.userID = Users.userID WHERE Tip.businessID = Business.businessID AND Tip.userID = Users.userID AND tipDate = maxDate AND Tip.userID = '" + R.GetString(0) + "'; ";
            executeQuery(sqlString, addGridRowToFriendTipHelper);
        }

        /// <summary>
        /// Set up columns for friend tips table
        /// </summary>
        private void addColumnsFriendTipGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("username");
            col1.Header = "Name";
            col1.Width = 100;
            friendTipGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("businessName");
            col2.Header = "Business";
            col2.Width = 120;
            friendTipGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 80;
            friendTipGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("text");
            col4.Header = "Tip Text";
            col4.Width = 160;
            friendTipGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("date");
            col5.Header = "Date";
            col5.Width = 60;
            friendTipGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("likes");
            col6.Header = "Likes";
            col6.Width = 50;
            friendTipGrid.Columns.Add(col6);
        }

        /// <summary>
        /// if an item in the list is selected, clear old info and fetch new into boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void idBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            nameBox.Clear();
            starsBox.Clear();
            fanBox.Clear();
            yelpBox.Clear();
            funBox.Clear();
            coolBox.Clear();
            helpBox.Clear();
            totalLikesBox.Clear();
            tipCountBox.Clear();
            longBox.Clear();
            latBox.Clear();
            friendGrid.Items.Clear();
            friendTipGrid.Items.Clear();
            if (idBox.SelectedIndex > -1)
            {
                string name = "SELECT username FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(name, getName);
                string stars = "SELECT average_stars FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(stars, getStars);
                string fan = "SELECT fans FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(fan, getFans);
                string tipCount = "SELECT tipCount FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(tipCount, getTipCount);
                string funny = "SELECT funny FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(funny, getFun);
                string cool = "SELECT cool FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(cool, getCool);
                string helpful = "SELECT useful FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(helpful, getHelp);
                string yelpSince = "SELECT yelpSince FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(yelpSince, getYelp);
                updateTotalLikes(idBox.SelectedItem.ToString());
                string totalLikes = "SELECT totalLikes FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(totalLikes, getTotalLikes);
                string friend = "SELECT userid FROM Users WHERE userID IN (SELECT friendOfID FROM Friend WHERE friendForID = '" + idBox.SelectedItem.ToString() + "');";
                executeQuery(friend, addGridRowToFriendTable);
                executeQuery(friend, addGridRowToFriendTipTable);
                string lat = "SELECT user_latitude FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(lat, getLat);
                string longit = "SELECT user_longitude FROM Users WHERE userID = '" + idBox.SelectedItem.ToString() + "';";
                executeQuery(longit, getLong);
            }
        }

        private void updateLat(string userid)
        {
            using (var conn = new NpgsqlConnection(buildConnectionString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Users SET user_latitude = " + latBox.Text +
                        "WHERE Users.userID ='" + userid + "'; ";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        // System.Windows.MessageBox.Show("SQL Error -" + ex.Message.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }

        }

        private void updateLong(string userid)
        {
            using (var conn = new NpgsqlConnection(buildConnectionString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Users SET user_longitude = " + longBox.Text +
                        "WHERE Users.userID ='" + userid + "'; ";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        // System.Windows.MessageBox.Show("SQL Error -" + ex.Message.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }

        }


        /// <summary>
        /// Edit button clicked => make latbox and longbox editable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            latBox.IsReadOnly = false;
            longBox.IsReadOnly = false;
        }

        /// <summary>
        /// Lat and long can be entered by the user to update their location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            updateLat(idBox.SelectedItem.ToString());
            updateLong(idBox.SelectedItem.ToString());
            longBox.IsReadOnly = true;
            latBox.IsReadOnly = true;
        }
    }
}

