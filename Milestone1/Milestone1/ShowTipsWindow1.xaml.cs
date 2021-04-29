using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for ShowTipsWindow1.xaml
    /// </summary>
    public partial class ShowTipsWindow1 : Window
    {

        private string bid = "";

        public class Tip
        {
            //TipDate, username, likes, Tiptext
            public string username { get; set; }
            public string userid { get; set; }
            public string TipDate { get; set; }
            public string likes { get; set; }
            public string Tiptext { get; set; }
            //public string Tbid { get; set; }



        }

        private void addColumns2Grid()
        {

            //TipDate, username, likes, Tiptext

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("TipDate");
            col1.Header = "tipdate";
            col1.Width = 255;
            businessTipsGrid.Columns.Add(col1);

            //friendsTipsGrid.Columns.Add(col1);


            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("username");
            col2.Header = "username";
            col2.Width = 150;
            businessTipsGrid.Columns.Add(col2);
           // friendsTipsGrid.Columns.Add(col2);


            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("likes");
            col3.Header = "like";
            col3.Width = 150;
            businessTipsGrid.Columns.Add(col3);
            //friendsTipsGrid.Columns.Add(col3);



            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("Tiptext");
            col4.Header = "tiptext";
            col4.Width = 60;
            businessTipsGrid.Columns.Add(col4);
           // friendsTipsGrid.Columns.Add(col4);







        }

        private void addColumns2friendsGrid()
        {

            //TipDate, username, likes, Tiptext

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("TipDate");
            col1.Header = "tipdate";
            col1.Width = 255;
            //businessTipsGrid.Columns.Add(col1);

            friendsTipsGrid.Columns.Add(col1);


            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("username");
            col2.Header = "username";
            col2.Width = 150;
            //businessTipsGrid.Columns.Add(col2);
            friendsTipsGrid.Columns.Add(col2);


            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("likes");
            col3.Header = "like";
            col3.Width = 150;
            //businessTipsGrid.Columns.Add(col3);
            friendsTipsGrid.Columns.Add(col3);



            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("Tiptext");
            col4.Header = "tiptext";
            col4.Width = 60;
           /// businessTipsGrid.Columns.Add(col4);
            friendsTipsGrid.Columns.Add(col4);







        }


        public ShowTipsWindow1(string bid)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);
            loadBusinessTips();
            addColumns2Grid();
            addColumns2friendsGrid();
            loadfriendsTips();
        }

        private void setBusinessTips(NpgsqlDataReader R)
        {
            //TipDate, username, likes, Tiptext
            businessTipsGrid.Items.Add(new Tip() { username = R.GetString(1), TipDate = R.GetString(0), likes = R.GetString(2), Tiptext = R.GetString(3), userid = R.GetString(4) });

        }

        private void setfriendsTips(NpgsqlDataReader R)
        {
            //TipDate, username, likes, Tiptext
           friendsTipsGrid.Items.Add(new Tip() { username = R.GetString(1), TipDate = R.GetString(0), likes = R.GetString(2), Tiptext = R.GetString(3), userid = R.GetString(4) });

        }

        private string buildConnectionString()
        {
            return "Host=localhost; Username=postgres; Database=yelpdb ; password=heba1996";
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

        private void loadBusinessTips()
        {
            businessTipsGrid.Items.Clear();
            string sqlStr1 = "SELECT TipDate::text, username, likes::text, Tiptext, t.userid " +
               "FROM Tip t JOIN users u ON t.userid = u.userid " +
               "WHERE t.businessid = '"
                + this.bid + "' " + 
            "ORDER By TipDate desc ";
            executeQuery(sqlStr1, setBusinessTips);


        }

        private void loadfriendsTips()
        {

            friendsTipsGrid.Items.Clear();
            string sqlStr1 = "SELECT TipDate::text, username, likes::text, Tiptext, t.userid " +
               "FROM Tip t JOIN users u ON t.userid = u.userid " +
               "WHERE t.businessid = '"
              
                + this.bid + "' " +
                " AND t.userid in ( " +
                " SELECT friendofid FROM friend WHERE friendforid = '4XChL029mKr5hydo79Ljxg' " +
                " UNION " +
                "SELECT friendforid FROM friend WHERE  friendofid= '4XChL029mKr5hydo79Ljxg' )" +

                "ORDER By TipDate desc ";
            executeQuery(sqlStr1, setfriendsTips);



        }


        private void businessTipsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddTip_Click(object sender, RoutedEventArgs e)
        {
            string TipText_1 = usertip.Text;
            string sql_str = "INSERT INTO Tip (userID, businessID, tipDate, tipText, likes)" +
             " VALUES ('4XChL029mKr5hydo79Ljxg', '" + this.bid + "' , NOW() , '" + TipText_1 + "', 0 )";


            executeQuery(sql_str, null);

            loadBusinessTips();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Tip T = businessTipsGrid.Items[businessTipsGrid.SelectedIndex] as Tip;
            if ((T.likes != null) && (T.likes.ToString().CompareTo("") != 0))
            {
                int likes = int.Parse(T.likes);

                string sql_str = "UPDATE Tip " +
                        "SET likes = " + (likes + 1) +
                        " WHERE userID = '" + T.userid + "' AND businessID ='" + bid + "'" +
                        " AND tipText = '" + T.Tiptext + "'"; 


           executeQuery(sql_str, null);
           loadBusinessTips();
                
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

