using Microsoft.Data.SqlClient;
using OtoKMaui.Entity;
using System.Data;

namespace OtoKMaui;

public partial class RentalPage : ContentPage
{
	public RentalPage()
	{
		InitializeComponent();
    }
    SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-S1QPNRR;TrustServerCertificate=true;Initial Catalog=OtoKiralamaDb;Integrated Security=True;Connect Timeout=30");

    DataTable dt = new DataTable();

    string connectionString = "Data Source=DESKTOP-S1QPNRR;TrustServerCertificate=true;Initial Catalog=OtoKiralamaDb;Integrated Security=True";
    private void FillCombo()
    {
        Con.Open();
        string query = "select RegNo from CarTb1 where Availability='" + "Yes" + "';";
        SqlCommand cmd = new SqlCommand(query, Con);
        SqlDataReader rdr;
        rdr = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Columns.Add("RegNo", typeof(string));
        dt.Load(rdr); 
        RentalListView.SelectedItem = "RegNo";
        RentalListView.SelectedItem = dt;
        Con.Close();
    }
    private void FillCustomer()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "SELECT CustId FROM CustomerTb1";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    List<string> custIds = new List<string>();
                    while (rdr.Read())
                    {
                        custIds.Add(rdr.GetInt32(0).ToString());
                    }
                    CustIdPicker.ItemsSource = custIds;
                }
            }
        }
    }

    private void FetchCustName()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "SELECT * FROM CustomerTb1 WHERE CustId = @CustId";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@CustId", CustIdPicker.SelectedItem.ToString());

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    NameEntry.Text = dr["CustName"].ToString();
                }
            }
        }
    }
    private void Populate()
    {
        Con.Open();
        string query = "select * from RentalTb1";
        SqlDataAdapter da = new SqlDataAdapter(query, Con);
        var dt = new DataTable();
        da.Fill(dt);

        var RentalList = new List<RentalTb1>();
        foreach (DataRow row in dt.Rows)
        {
            var rental = new RentalTb1
            {
                RentId = Convert.ToInt32(row["RentId"]),
                CarReg = row["CarReg"].ToString(),
                CustName = row["CustName"].ToString(),
                RentFees = Convert.ToDecimal(row["RentFees"])
            };
            RentalList.Add(rental);
        }

        RentalListView.ItemsSource = RentalList;
        Con.Close();
    }
    private async void UpdateonRentDelete()
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var query = $"update CarTb1 set Available = 'Yes' where RegNo = '{CarRegPicker.SelectedItem.ToString()}';";

        await using var command = new SqlCommand(query, connection);
        await command.ExecuteNonQueryAsync();

        await DisplayAlert("Success", "Car Successfully Updated", "OK");

        connection.Close();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        FillCombo();
        FillCustomer();
        Populate();

        //DataTable dt = new DataTable();
        //dt.Columns.AddRange(new DataColumn[]
        //{
        //new DataColumn("RentId", typeof(int)),
        //new DataColumn("CarReg", typeof(string)),
        //new DataColumn("CustName", typeof(string)),
        //new DataColumn("Name", typeof(string)),
        //new DataColumn("RentFees", typeof(string))
        //});

        //RentalListView.ItemsSource = dt.DefaultView;
    }

    private void AddClicked(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(IdEntry.Text) || string.IsNullOrEmpty(NameEntry.Text) || string.IsNullOrEmpty(FeesEntry.Text))
        {
            DisplayAlert("Error", "Car Successfully Rented", "OK");
        }
        else
        {
            try
            {
                Con.Open();
                string query = "INSERT INTO RentalTb1 (CarReg, CustName, RentFees) VALUES (@CarReg, @CustName, @RentFees)";

                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@RentId", Convert.ToInt32(IdEntry.Text));
                cmd.Parameters.AddWithValue("@CarReg", CarRegPicker.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@CustName", NameEntry.Text);
                cmd.Parameters.AddWithValue("@RentFees", Convert.ToDecimal(FeesEntry.Text));
                cmd.ExecuteNonQuery();
                DisplayAlert("Error", "Car Successfully Rented", "OK");
                Con.Close();
                Populate();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message , "OK");
            }
        }
    }

    private void UpdateClicked(object sender, EventArgs e)
{
    if (IdEntry.Text == "" || NameEntry.Text == "" || FeesEntry.Text == "")
    {
        DisplayAlert("Error", "Missing information", "OK");
    }
    else
    {
        try
        {
            Con.Open();
            string query = "UPDATE RentalTb1 SET CarReg = @CarReg, CustName = @CustName, RentFees = @RentFees WHERE RentId = @RentId";

            SqlCommand cmd = new SqlCommand(query, Con);
            cmd.Parameters.AddWithValue("@CarReg", CarRegPicker.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@CustName", NameEntry.Text);
            cmd.Parameters.AddWithValue("@RentFees", Convert.ToDecimal(FeesEntry.Text));
            cmd.Parameters.AddWithValue("@RentId", Convert.ToInt32(IdEntry.Text));

            cmd.ExecuteNonQuery();
            DisplayAlert("Error", "Car Successfully Updated", "OK");
            Populate();
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

    private void DeleteClicked(object sender, EventArgs e)
    {
        if (IdEntry.Text == "")
        {
            DisplayAlert("Error", "Missing information", "OK");
        }
        else
        {
            try
            {
                Con.Open();
                string query = "DELETE FROM RentalTb1 WHERE RentId = " + IdEntry.Text + ";";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                DisplayAlert("Error", "Rental Deleted Successfully", "OK");
                Con.Close();
                Populate();
                UpdateonRentDelete();
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
    private void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is DataRowView rowView)
        {
            IdEntry.Text = rowView.Row["RentId"].ToString();
            NameEntry.Text = rowView.Row["CustName"].ToString();
            FeesEntry.Text = rowView.Row["RentFees"].ToString();
            CarRegPicker.SelectedItem= rowView.Row["CarReg"].ToString();
        }
    }
    private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        //if (RentalListView.SelectedItem != null)
        //{
        //    var selectedItem = RentalListView.SelectedItem as RentalTb1;

        //    var itemId = selectedItem.RentId;
        //    var itemName = selectedItem.CustName;
        //    var itemCarReg = selectedItem.CarReg;
        //    var itemRentFees = selectedItem.RentFees;

        //    IdEntry.Text = itemId.ToString();
        //    NameEntry.Text = itemName;
        //    CarRegPicker.SelectedItem = itemCarReg;
        //    FeesEntry.Text = itemRentFees.ToString();
        //}
    }
    private void ClearClicked(object sender, EventArgs e)
    {
        IdEntry.Text = "";
        NameEntry.Text = "";
        CarRegPicker.SelectedItem = null;
        FeesEntry.Text = "";
    }
    private void BackClicked(object sender, EventArgs e)
    {
        var nextPage = new HomePage();
        var navigationPage = new NavigationPage(nextPage);
        Application.Current.MainPage = navigationPage;
    }
    private void ListView (object sender, EventArgs e)
    {

    }
    

}