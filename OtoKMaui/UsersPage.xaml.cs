using Microsoft.Data.SqlClient;
using OtoKMaui.Entity;
using System;
using System.Data;

namespace OtoKMaui
{
    public partial class UsersPage : ContentPage
    {
        public UsersPage()
        {
            InitializeComponent();
            UsersListView.ItemSelected += UsersListView_ItemSelected;

        }
        SqlConnection Con = new SqlConnection(@"Data Source=.;TrustServerCertificate=true;Initial Catalog=OtoKiralamaDb;Integrated Security=True;Connect Timeout=30");

        DataTable dt = new DataTable();
        private void populate()
        {
            Con.Open();
            string query = "select * from UserTb1";
            SqlDataAdapter da = new SqlDataAdapter(query, Con);
            var dt = new DataTable();
            da.Fill(dt);

            var userList = new List<UserTb1>();
            foreach (DataRow row in dt.Rows)
            {
                var user = new UserTb1
                {
                    UserId = Convert.ToInt32(row["UserId"]),
                    Uname = row["Uname"].ToString(),
                    Upass = row["Upass"].ToString()
                };
                userList.Add(user);
            }

            UsersListView.ItemsSource = userList;
            Con.Close();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            populate();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("Id",typeof(int)),
                new DataColumn("Name", typeof(String)),
                new DataColumn("Password", typeof(String))
             });
        }

        private void AddClicked(object sender, EventArgs e)
        {
            if (NameEntry.Text == "" || PassEntry.Text == "")
            {
                DisplayAlert("Error", "Missing information", "OK");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "insert into UserTb1 (Uname, Upass) values ('" + NameEntry.Text + "', '" + PassEntry.Text + "')";
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

        private void UpdateClicked(object sender, EventArgs e)
        {
            if (IdEntry.Text == "" || NameEntry.Text == "" || PassEntry.Text == "")
            {
                DisplayAlert("Error", "Missing information", "OK");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "update UserTb1 set Uname='" + NameEntry.Text + "',Upass='" + PassEntry.Text + "'where UserId=" + IdEntry.Text + ";";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    DisplayAlert("Error", "User Successfully Update", "OK");
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
             if (IdEntry.Text == "")
            {
                DisplayAlert("Error", "Missing Information", "OK");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "delete from UserTb1 where UserId=" + IdEntry.Text + ";";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    DisplayAlert("Error", "User Deleted Successfully", "OK");
                    Con.Close();
                    populate();
                }
                catch (Exception Myex)
                {
                    DisplayAlert("Error", Myex.Message, "OK");
                }
            }
        }

        private void UsersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is DataRowView rowView)
            {
                IdEntry.Text = rowView.Row["Id"].ToString();
                NameEntry.Text = rowView.Row["Name"].ToString();
                PassEntry.Text = rowView.Row["Password"].ToString();
            }
        }
        private void UsersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedItem = e.SelectedItem as UserTb1;

                // Öğenin verilerine erişin
                var itemId = selectedItem.UserId;
                var itemName = selectedItem.Uname;
                var itemPassword = selectedItem.Upass;

                // Verileri metin kutularına yerleştirin
                IdEntry.Text = itemId.ToString();
                NameEntry.Text = itemName;
                PassEntry.Text = itemPassword;

                // İşlemleri gerçekleştirin
                // ...
            }
        }
        private void BackClicked(object sender, EventArgs e)
        {
            var nextPage = new HomePage();
            var navigationPage = new NavigationPage(nextPage);
            Application.Current.MainPage = navigationPage;
        }

        private void ClearClicked(object sender, EventArgs e)
        {
            IdEntry.Text = "";
            NameEntry.Text = "";
            PassEntry.Text = "";
        }
    }
}
