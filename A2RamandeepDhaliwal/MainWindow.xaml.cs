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
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Markup;

namespace A2RamandeepDhaliwal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadContinents();
        }

        public string GetConnectionString()
        {
            return "Server=(LocalDB)\\MSSQLLocalDB;Database=WorldDB;Trusted_Connection=Yes;";
        }

        public void loadContinents()
        {
            try
            {
                comboContinents.Items.Clear();
                string cs = GetConnectionString();
                string query = "Select ContinentName FROM Continent";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader continentsReader = cmd.ExecuteReader();

                    while (continentsReader.Read())
                    {
                        string continentName = (string)continentsReader["ContinentName"];
                        MaincomboContinents(continentName);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void MaincomboContinents(string continentName)
        {
            comboContinents.Items.Add(continentName);
        }

        private void comboContinents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refreshLabelDatagrid();
            if (comboContinents.SelectedItem != null)
            {
                List<string> countries = new List<string>();

                String selectedContinent = (string)comboContinents.SelectedItem;
                string cs = GetConnectionString();
                string query1 = "Select ContinentId From Continent Where ContinentName = @ContinentName";
                string query2 = "Select CountryName from Country Where ContinentId = @ContinentID";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd1.Parameters.AddWithValue("ContinentName", selectedContinent);
                    conn.Open();
                    int continentID = (int)cmd1.ExecuteScalar();
                    cmd2.Parameters.AddWithValue("ContinentID", continentID);
                    SqlDataReader countriesReader = cmd2.ExecuteReader();

                    while (countriesReader.Read())
                    {
                        string countryName = (string)countriesReader["CountryName"];
                        countries.Add(countryName);
                    }

                    listBoxCountries.ItemsSource = countries;

                }

            }

        }

        private List<object> LoadCityDetails(string selectedCountry)
        {
            List<object> cities = new List<object>();

            string cs = GetConnectionString();
            string queryCities = "Select CityId, CityName, IsCapital, Population FROM City Where CountryID = @CountryID";
            string queryCountryId = "Select CountryId From Country Where CountryName = @Countrynamee";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd2 = new SqlCommand(queryCities, conn);
                SqlCommand cmd3 = new SqlCommand(queryCountryId, conn);

                cmd3.Parameters.AddWithValue("@Countrynamee", selectedCountry);
                conn.Open();

                int countryID = (int)cmd3.ExecuteScalar();
                cmd2.Parameters.AddWithValue("@CountryID", countryID);

                SqlDataReader cityReader = cmd2.ExecuteReader();
                while (cityReader.Read())
                {
                    int cityId = (int)cityReader["CityId"];
                    string cityName = (string)cityReader["CityName"];
                    bool isCapital = (bool)cityReader["IsCapital"];
                    string population = (string)cityReader["Population"];
                    cities.Add(new
                    {
                        CityId = cityId,
                        CityName = cityName,
                        Population = population,
                        CountryName = selectedCountry,
                        IsCapital = isCapital
                    });
                }
                cityReader.Close();

                return cities;
            }
        }

        private void listBoxCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listBoxCountries.SelectedItem != null)
                {
                    string selectedCountry = (string)listBoxCountries.SelectedItem;

                    string cs = GetConnectionString();
                    string query = "Select Language, Currency From Country Where CountryName = @CountryyName";

                    using (SqlConnection conn = new SqlConnection(cs))
                    {
                        SqlCommand cmd1 = new SqlCommand(query, conn);
                        cmd1.Parameters.AddWithValue("CountryyName", selectedCountry);
                        conn.Open();

                        SqlDataReader LangCurr = cmd1.ExecuteReader();
                        while (LangCurr.Read())
                        {
                            string language = (string)LangCurr["Language"];
                            string currency = (string)LangCurr["Currency"];
                            languageLabel.Content = language;
                            currencyLabel.Content = currency;
                        }
                        LangCurr.Close();

                        List<object> cities = LoadCityDetails(selectedCountry);
                        cityDataGrid.ItemsSource = cities;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        public void refreshLabelDatagrid()
        {
            languageLabel.Content = "";
            currencyLabel.Content = "";
            cityDataGrid.ItemsSource = null;
        }

        private void AddContinentsBtn_Click(object sender, RoutedEventArgs e)
        {

            AddContinents addContinents = new AddContinents(this);
            addContinents.Show();

        }

        private void addCountryBtn_Click(object sender, RoutedEventArgs e)
        {
            AddCountry addCountry = new AddCountry(this);
            addCountry.Show();

        }

        private void addCityBtn_Click(object sender, RoutedEventArgs e)
        {
            AddCities addCities = new AddCities(this);
            addCities.Show();
        }
    }

}

