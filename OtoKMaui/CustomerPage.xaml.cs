
using Microsoft.Data.SqlClient;
using OtoKMaui.Entity;
using System.Data;
using static Azure.Core.HttpHeader;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace OtoKMaui 
{
    public partial class CusomerPage : ContentPage
    {
        public CusomerPage()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-S1QPNRR;TrustServerCertificate=true;Initial Catalog=OtoKiralamaDb;Integrated Security=True;Connect Timeout=30");

        DataTable dt = new DataTable();
        private void populate()
        {
            Con.Open();
            string query = "select * from CustomerTb1";
            SqlDataAdapter da = new SqlDataAdapter(query, Con);
            var dt = new DataTable();
            da.Fill(dt);

            var custList = new List<Customer>();
            foreach (DataRow row in dt.Rows)
            {
                var cust = new Customer
                {
                    CustId = Convert.ToInt32(row["CustId"]),
                    CustName = row["CustName"].ToString(),
                    CustAdd = row["CustAdd"].ToString(),
                    Phone = row["Phone"].ToString()
                };
                custList.Add(cust);
            }

            ListView.ItemsSource = custList;
            Con.Close();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Customer_Load();
        }

        private void Customer_Load()
        {
            populate();

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("Id",typeof(int)),
                new DataColumn("Name", typeof(String)),
                new DataColumn("Address", typeof(String)),
                new DataColumn("Phone", typeof(String)),
                 });
        }
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is DataRowView selectedData)
            {
                IdEntry.Text = selectedData["CustId"].ToString();
                NameEntry.Text = selectedData["CustName"].ToString();
                AddressEntry.Text = selectedData["CustAdd"].ToString();
                PhoneEntry.Text = selectedData["Phone"].ToString();
            }
        }
        private void UsersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedItem = e.SelectedItem as Customer;

                // Öğenin verilerine erişin
                var itemId = selectedItem.CustId;
                var itemName = selectedItem.CustName;
                var itemAdd = selectedItem.CustAdd;
                var itemPhone = selectedItem.Phone;

                // Verileri metin kutularına yerleştirin
                IdEntry.Text = itemId.ToString();
                NameEntry.Text = itemName;
                AddressEntry.Text = itemAdd;
                PhoneEntry.Text = itemPhone;

                // İşlemleri gerçekleştirin
                // ...
            }
        }
        private void AddClicked(object sender, EventArgs e)
        {
            if (NameEntry.Text == "" || AddressEntry.Text == "" || PhoneEntry.Text == "")
            {
                DisplayAlert("Error", "Missing information", "OK");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "INSERT INTO CustomerTb1 (CustName, CustAdd, Phone) VALUES ('" + NameEntry.Text + "','" + AddressEntry.Text + "','" + PhoneEntry.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    DisplayAlert("Success", "User Successfully Added", "OK");
                    Con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }


        private void UpdateClicked(object sender, EventArgs e)
        {
            if (IdEntry.Text == "" || NameEntry.Text == "" || AddressEntry.Text == ""||PhoneEntry.Text=="")
            {
                DisplayAlert("Error", "Missing information", "OK");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "update CustomerTb1 set CustName='" + NameEntry.Text + "',CustAdd='" + AddressEntry.Text + "',Phone='" + PhoneEntry.Text + "'where CustId=" + IdEntry.Text + ";";

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

        //
        private void DeleteClicked(object sender, EventArgs e)
        {
            if (IdEntry.Text == "")
            {
                DisplayAlert("Error", "Missing Information", "OK");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "DELETE FROM CustomerTb1 WHERE CustId = @CustId";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.Parameters.AddWithValue("@CustId", IdEntry.Text);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        DisplayAlert("Success", "User Deleted Successfully", "OK");
                        populate();
                    }
                    else
                    {
                        DisplayAlert("Error", "User not found", "OK");
                    }

                    Con.Close();
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }

        //private void DeleteClicked(object sender, EventArgs e)
        //{
        //    if (IdEntry.Text == "")
        //    {
        //        DisplayAlert("Error", "Missing Information", "OK");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            Con.Open();
        //            string query = "delete from CustomerTb1 where CustId=" + IdEntry.Text + ";";
        //            SqlCommand cmd = new SqlCommand(query, Con);
        //            cmd.ExecuteNonQuery();
        //            DisplayAlert("Error", "User Deleted Successfully", "OK");
        //            Con.Close();
        //            populate();
        //        }
        //        catch (Exception Myex)
        //        {
        //            DisplayAlert("Error", Myex.Message, "OK");
        //        }
        //    }
        //}

        private void BackClicked(object sender, EventArgs e)
        {
            var nextPage = new HomePage();
            var navigationPage = new NavigationPage(nextPage);
            Application.Current.MainPage = navigationPage;
        }
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedCustomer = (Customer)e.SelectedItem;
                IdEntry.Text = selectedCustomer.CustId.ToString();
                NameEntry.Text = selectedCustomer.CustName;
                AddressEntry.Text = selectedCustomer.CustAdd;
                PhoneEntry.Text = selectedCustomer.Phone;
            }
        }

        private void ClearClicked(object sender, EventArgs e)
        {
            IdEntry.Text = "";
            NameEntry.Text = "";
            AddressEntry.Text = "";
            PhoneEntry.Text = "";
        }
    }

}

