using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.IO;
using Accessibility;

namespace A2RamandeepDhaliwal
{
    /// <summary>
    /// Interaction logic for AddContinents.xaml
    /// </summary>
    public partial class AddContinents : Window
    {
        private MainWindow _mw;
        public AddContinents(MainWindow mw)
        {
            InitializeComponent();
            _mw = mw;
        }


        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (conNametxtBox.Text != "")
            {
                AddContinent(conNametxtBox.Text.ToString());

            }
            else
            {
                continenetLabel.Content = "This field is required.";
            }
        }

        public void AddContinent(string continent)
        {
            if (conNametxtBox.Text.ToString() != "")
            {

                string cs = _mw.GetConnectionString();
                string query = "Insert into Continent (ContinentName) values (@ContinentName)";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("ContinentName", continent);

                    conn.Open();

                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Continent inserted");
                        _mw.loadContinents();
                        conNametxtBox.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Continent not inserted");
                    }
                }
            }

        }
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {

            this.Close();

        }
    }
}
