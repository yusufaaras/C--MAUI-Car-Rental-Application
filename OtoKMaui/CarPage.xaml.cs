using Microsoft.Data.SqlClient;
using OtoKMaui.Entity;
using System;
using System.Data;

namespace OtoKMaui
{
    public partial class CarPage : ContentPage
    {
        public CarPage()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-S1QPNRR;TrustServerCertificate=true;Initial Catalog=OtoKiralamaDb;Integrated Security=True;Connect Timeout=30");

        DataTable dt = new DataTable();
        private void populate()
        {
            Con.Open();
            string query = "select * from CarTb1";
            SqlDataAdapter da = new SqlDataAdapter(query, Con);
            var dt = new DataTable();
            da.Fill(dt);

            var userList = new List<Car>();
            foreach (DataRow row in dt.Rows)
            {
                var car = new Car()
                {
                    RegNo = row["RegNo"].ToString(),
                    Brand = row["Brand"].ToString(),
                    Model = row["Model"].ToString(),
                    Availability = row["Availability"].ToString(),
                    Price = Convert.ToDecimal(row["Price"])
                };
                userList.Add(car);
            }
            CarsListView.ItemsSource = userList;
            Con.Close();
        }

        private void OnItemTapped(object sender, SelectionChangedEventArgs e)
        {
            if (CarsListView.SelectedItem is DataRowView selectedCar)
            {
                RegNoEntry.Text = selectedCar["RegNo"].ToString();
                BrandEntry.Text = selectedCar["Brand"].ToString();
                ModelEntry.Text = selectedCar["Model"].ToString();
                AvailabPicker.SelectedItem = selectedCar["Availability"].ToString();
                PriceEntry.Text = selectedCar["Price"].ToString();
            }
        }

        private void RefreshClicked(object sender, EventArgs e)
        {
            populate();
        }

        private void AddClicked(object sender, EventArgs e)
        {
            if (RegNoEntry.Text == "" || BrandEntry.Text == "" || ModelEntry.Text == "" || PriceEntry.Text == "")
            {
                DisplayAlert("Error", "Missing information", "OK");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "insert into CarTb1 values('" + RegNoEntry.Text + "','" + BrandEntry.Text + "','" + ModelEntry.Text + "','" + AvailabPicker.SelectedItem.ToString() + "'," + PriceEntry.Text + ")";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    DisplayAlert("Error", "Car Successfully Added", "OK");
                    Con.Close();
                    populate();
                }
                catch (Exception Myex)
                {
                    DisplayAlert("Error", Myex.Message, "OK");
                }
            }
        }

        private void UpdateClicked(object sender, EventArgs e)
        {
            if (RegNoEntry.Text == "" || BrandEntry.Text == "" || ModelEntry.Text == ""||PriceEntry.Text=="")
            {
                DisplayAlert("Error", "Missing information", "OK");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "update CarTb1 set Model='" + ModelEntry.Text + "',Price='" + PriceEntry.Text + "',Availability='"+ AvailabPicker + "',Brand='" + BrandEntry.Text+ "'where RegNo=" + RegNoEntry.Text + ";";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    DisplayAlert("Error", "User Successfully Added", "OK");
                    Con.Close();
                    populate();
                }
                catch (Exception Myex)
                {
                    DisplayAlert("Error", Myex.Message, "OK");
                }

            }
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            if (RegNoEntry.Text == "")
            {
                DisplayAlert("Error", "Missing information", "OK");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = " delete  from CarTb1 where RegNo='" + Convert.ToString(RegNoEntry.Text) + "';";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    DisplayAlert("Error", "Car Deleted Successfully ", "OK");
                    Con.Close();
                    populate();
                }
                catch (Exception Myex)
                {
                    DisplayAlert("Error", Myex.Message, "OK");
                }
                finally
                {
                    Con.Close();
                }
            }
        }
        
        private void BackClicked(object sender, EventArgs e)
        {
            var nextPage = new HomePage();
            var navigationPage = new NavigationPage(nextPage);
            Application.Current.MainPage = navigationPage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            populate();
        }

        private void ClearClicked(object sender, EventArgs e)
        {
            RegNoEntry.Text = "";
            BrandEntry.Text = "";
            ModelEntry.Text = "";
            PriceEntry.Text = "";
            AvailabPicker.SelectedItem = null;
        }
        private void CarListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is DataRowView rowView)
            {
                RegNoEntry.Text = rowView.Row["RegNo"].ToString();
                BrandEntry.Text = rowView.Row["brand"].ToString();
                ModelEntry.Text = rowView.Row["Model"].ToString();
                PriceEntry.Text = rowView.Row["Availability"].ToString();
                AvailabPicker.SelectedItem = rowView.Row["Price"].ToString();

            }
        }
        private void CarListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedItem = e.SelectedItem as Car;

                // Öğenin verilerine erişin
                var itemRegNo = selectedItem.RegNo;
                var itemBrand = selectedItem.Brand;
                var itemModel = selectedItem.Model;
                var itemAvailability = selectedItem.Availability;
                var itemPrice = selectedItem.Price;

                // Verileri metin kutularına yerleştirin
                RegNoEntry.Text = itemRegNo.ToString();
                BrandEntry.Text = itemBrand.ToString();
                ModelEntry.Text = itemModel.ToString();
                AvailabPicker.SelectedItem= itemAvailability.ToString();
                PriceEntry.Text= itemPrice.ToString();  

                // İşlemleri gerçekleştirin
                // ...
            }
        }

        private void SearchPicker_SelectionChanged(object sender, EventArgs e)
        {
            var selectedText = SerachPicker.SelectedItem?.ToString();
            if (selectedText == "Availability")
            {
                // Available seçeneği seçildiğinde yapılması gereken işlemler
                string flag = "Yes";
                Con.Open();
                string query = "select * from CarTb1 where Availability='" + flag + "'";
                SqlDataAdapter da = new SqlDataAdapter(query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                CarsListView.ItemsSource = ds.Tables[0].DefaultView;
                Con.Close();
            }
            else
            {
                // Diğer durumlar için yapılması gereken işlemler
                string flag = "No";
                Con.Open();
                string query = "select * from CarTb1 where Availability='" + flag + "'";
                SqlDataAdapter da = new SqlDataAdapter(query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                CarsListView.ItemsSource = ds.Tables[0].DefaultView;
                Con.Close();
            }
        }
    }
}
