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
using System.Windows.Shapes;
using Npgsql;

namespace YelpDB
{
    /// <summary>
    /// Interaction logic for CheckinChart.xaml
    /// </summary>
    public partial class CheckinChart : Window
    {
        public CheckinChart(string businessStr)
        {
            InitializeComponent();
            checkInColumns(businessStr);
        }

        private string buildConnectionString()
        {
            return "Host=localhost; Username=postgres; Database=yelpdb; password=birth161998";
        }

        private void checkInColumns(string businessStr)
        {
            List<KeyValuePair<string, int>> chartData = new List<KeyValuePair<string, int>>();
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {

                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            chartData.Add(new KeyValuePair<string, int>(reader.GetString(0), reader.GetInt32(1)));
                        }
                    }
                }
                connection.Close();
            }
            checkinChart.DataContext = chartData;
        }
    }
}
