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

namespace A2RamandeepDhaliwal
{
    /// <summary>
    /// Interaction logic for AddCountry.xaml
    /// </summary>
    public partial class AddCountry : Window
    {
        private MainWindow _mw;
        public AddCountry(MainWindow mw)
        {
            InitializeComponent();
            _mw = mw;
            loadContinentsCountryWin();
        }

        public void loadContinentsCountryWin()
        {
            ContCombo.Items.Clear();
            string cs = _mw.GetConnectionString();
            string query = "Select ContinentName FROM Continent";
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader continentsReader = cmd.ExecuteReader();

                while (continentsReader.Read())
                {
                    string continentName = (string)continentsReader["ContinentName"];
                    ContCombo.Items.Add(continentName);

                }
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((ContCombo.SelectedItem != null) && (CountryNameTxtBox.Text.ToString() != ""))
            {
                string cs = _mw.GetConnectionString();
                string selectedContinent = (string)ContCombo.SelectedItem;
                string query = "Insert into Country (CountryName, Language, Currency, ContinentId) values (@CountryName, @Language, @Currency, @ContinentId )";
                string query2 = "Select ContinentId from Continent Where ContinentName = @ContinentName";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("ContinentName", selectedContinent);
                    cmd.Parameters.AddWithValue("CountryName", CountryNameTxtBox.Text.ToString());
                    cmd.Parameters.AddWithValue("Language", langTxtBox.Text.ToString());
                    cmd.Parameters.AddWithValue("Currency", currTxtBox.Text.ToString());

                    conn.Open();
                    int continentID = (int)cmd2.ExecuteScalar();
                    cmd.Parameters.AddWithValue("ContinentId", continentID);
                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Country Information inserted");
                        _mw.loadContinents();
                        clearTexts();
                    }
                    else
                    {
                        MessageBox.Show("Country not inserted");
                    }
                }
            }
            else
            {
                conNameLabel.Content = "Field is required.";
                couNameLabel.Content = "Field is required.";
            }
        }

        private void clearTexts()
        {
            CountryNameTxtBox.Clear();
            langTxtBox.Clear();
            currTxtBox.Clear();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
