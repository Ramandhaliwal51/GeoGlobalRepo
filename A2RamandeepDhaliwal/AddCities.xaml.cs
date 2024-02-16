using System;
using System.Collections;
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
    /// Interaction logic for AddCities.xaml
    /// </summary>
    public partial class AddCities : Window
    {
        private MainWindow _mw;
        public AddCities(MainWindow mw)
        {
            InitializeComponent();
            _mw = mw;
            loadCountries();
        }

        private void loadCountries()
        {
            try
            {
                countriesCombo.Items.Clear();
                string cs = _mw.GetConnectionString();
                string query = "Select CountryName FROM Country";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader countriesReader = cmd.ExecuteReader();

                    while (countriesReader.Read())
                    {
                        string countryName = (string)countriesReader["CountryName"];
                        countriesCombo.Items.Add(countryName);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((countriesCombo.SelectedItem != null) && (cityTextBox.Text.ToString() != ""))
            {
                string cs = _mw.GetConnectionString();
                string selectedCountry = (string)countriesCombo.SelectedItem;
                string query1 = "Select CountryId from Country Where CountryName = @CountryName";
                string query2 = "Insert into City(CityName, IsCapital, Population, CountryId) Values (@CityName,@IsCapital,@Population,@CountryId)";

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);

                    cmd1.Parameters.AddWithValue("CountryName", selectedCountry);
                    cmd2.Parameters.AddWithValue("CityName", cityTextBox.Text.ToString());
                    cmd2.Parameters.AddWithValue("IsCapital", capitalCheckB.IsChecked);
                    cmd2.Parameters.AddWithValue("Population", popTextBox.Text.ToString());

                    conn.Open();

                    int countryId = (int)cmd1.ExecuteScalar();
                    cmd2.Parameters.AddWithValue("CountryId", countryId);

                    int result = cmd2.ExecuteNonQuery();
                    if (result == 1)
                    {
                        MessageBox.Show("City Information inserted");
                        _mw.loadContinents();
                        //clearTexts();
                    }
                    else
                    {
                        MessageBox.Show("City not inserted");
                    }
                }
            }
            else
            {
                cNameLabel.Content = "field is required.";
                cityNameLabel.Content = "field is required.";
            }
        }

        private void clearTexts()
        {
            popTextBox.Clear();
            capitalCheckB.IsChecked = false;
            cityTextBox.Clear();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
