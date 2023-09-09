using Microsoft.Data.SqlClient;
using OtoKMaui.Entity;
using System.Data;

namespace OtoKMaui;

public partial class ReturnPage : ContentPage
{
    public ReturnPage()
    {
        InitializeComponent();
        ReturnListView.ItemTapped += ReturnedOnItemTapped;

    }

    private object TofeeTb;

    SqlConnection Con = new SqlConnection(@"Data Source=.;TrustServerCertificate=true;Initial Catalog=OtoKiralamaDb;Integrated Security=True;Connect Timeout=30");

    DataTable dt = new DataTable();

    private void Populate()
    {
        Con.Open();
        string query = "select * from RentalTb1";
        SqlDataAdapter da = new SqlDataAdapter(query, Con);
        var dt = new DataTable();
        da.Fill(dt);
        var returnList = new List<RentalTb1>();
        foreach (DataRow row in dt.Rows)
        {
            var ret = new RentalTb1
            {
                RentId = Convert.ToInt32(row["RentId"]),
                CarReg = row["CarReg"].ToString(),
                CustName = row["CustName"].ToString(),
                RentFees = Convert.ToDecimal(row["RentFees"])
            };
            returnList.Add(ret);
        }

        RentListView.ItemsSource = returnList;
        Con.Close();
    }
    private void PopulateRets()
    {
        Con.Open();
        string query = "select * from ReturnTb1";
        SqlDataAdapter da = new SqlDataAdapter(query, Con);
        var dt = new DataTable();
        da.Fill(dt);

        var returnList = new List<RentalItem>();
        foreach (DataRow row in dt.Rows)
        {
            var ret = new RentalItem
            {
                Id = Convert.ToInt32(row["ReturnId"]),
                CarReg = row["CarId"].ToString(),
                Name = row["CustName"].ToString(),
                RentalDate = Convert.ToDateTime(row["ReturnDate"].ToString()),
                Delay = Convert.ToInt32(row["Delay"]),
                Fine = Convert.ToInt32(row["Fine"]),
                TotalFees = Convert.ToInt32(row["TotalFees"])
            };
            returnList.Add(ret);
        }

        RentListView.ItemsSource = returnList;
        Con.Close();
    }
    private void DeleteOnReturn()
    {
        if (RentListView.SelectedItem is RentalTb1 selectedItem)
        {
            int rentId = selectedItem.RentId;
            Con.Open();
            string query = "DELETE FROM RentalTb1 WHERE RentId=" + rentId;
            SqlCommand cmd = new SqlCommand(query, Con);
            cmd.ExecuteNonQuery();
            Con.Close();
            Populate();
        }
    }
    private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Populate();
    }
    private void OnPageLoaded(object sender, EventArgs e)
    {
        base.OnAppearing();
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[2] { new DataColumn("CarIdTb", typeof(string)),
        new DataColumn("CustNameTb", typeof(string))
    });

        RentListView.ItemsSource = dt.DefaultView;
        RentListView.IsVisible = true;

        Populate();
       // PopulateRets();
    }


    private void BackClicked(object sender, EventArgs e)
    {
        var nextPage = new HomePage();
        var navigationPage = new NavigationPage(nextPage);
        Application.Current.MainPage = navigationPage;
    }
    private void ReturnClicked(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(IdEntry.Text) || string.IsNullOrEmpty(NameEntry.Text) ||
        string.IsNullOrEmpty(FineEntry.Text) || string.IsNullOrEmpty(DelayEntry.Text))
        {
            DisplayAlert("Error", "Missing Information.", "OK");
        }
        else
        {
            try
            {
                Con.Open();
                string query = "INSERT INTO ReturnTb1 VALUES (@ReturnId, @CarId, @CustName, @ReturnDate, @Delay, @Fine, @TotalFees)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", int.Parse(IdEntry.Text));
                cmd.Parameters.AddWithValue("@CarId", CarRegEntry.Text);
                cmd.Parameters.AddWithValue("@CustName", NameEntry.Text);
                cmd.Parameters.AddWithValue("@ReturnDate", ReturnDatePicker.Date);
                cmd.Parameters.AddWithValue("@Delay", int.Parse(DelayEntry.Text));
                cmd.Parameters.AddWithValue("@Fine", decimal.Parse(FineEntry.Text));
                cmd.Parameters.AddWithValue("@TotalFees", decimal.Parse(TotalEntry.Text));
                cmd.ExecuteNonQuery();
                DisplayAlert("Error", "Car fully Returned", "OK");
                Con.Close();
                PopulateRets();
                DeleteOnReturn();
            }
            catch (Exception ex)
            {
            }
        }
    }
    private void ReturnListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedItem = e.SelectedItem as RentalItem;

            // Öğenin verilerine erişin
            var itemReturnId = selectedItem.Id;
            var itemCarReg = selectedItem.CarReg;
            var itemName = selectedItem.Name;
            var itemRentalDate = selectedItem.RentalDate;
            var itemDelay = selectedItem.Delay;
            var itemFine = selectedItem.Fine;
            var itemTotalFees = selectedItem.TotalFees;

            // Verileri metin kutularına yerleştirin
            IdEntry.Text = itemReturnId.ToString();
            CarRegEntry.Text = itemCarReg;
            NameEntry.Text = itemName;
            //  ReturnDatePicker.Date =itemReturnId;
            NameEntry.Text = itemName;
            CarRegEntry.Text = itemCarReg;
            NameEntry.Text = itemName;

            // İşlemleri gerçekleştirin
            // ...
        }
    }
    private void ReturnedOnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is DataRowView rowView)
        {
            IdEntry.Text = rowView["ReturnId"].ToString();
            CarRegEntry.Text = rowView["CarId"].ToString();
            NameEntry.Text = rowView["CustName"].ToString();
            ReturnDatePicker.Date = Convert.ToDateTime(rowView["ReturnDate"].ToString());
            DelayEntry.Text = rowView["Delay"].ToString();
            FineEntry.Text = rowView["Fine"].ToString();
            TotalEntry.Text = rowView["TotalFees"].ToString();
        }

        if (e.Item is RentalItem rentalItem)
        {
            ReturnDatePicker.Date = rentalItem.RentalDate;

            DateTime returnDate = ReturnDatePicker.Date;
            TimeSpan rentalDuration = returnDate - rentalItem.RentalDate;
            int delay = (int)rentalDuration.TotalDays;
            decimal fine = delay <= 0 ? 0 : delay * 250;

            DelayEntry.Text = delay.ToString();
            FineEntry.Text = fine.ToString();

            decimal totalFees = rentalItem.TotalFees + fine;
            TotalEntry.Text = totalFees.ToString();

            string nextId = FetchId();
            IdEntry.Text = nextId;
        }

    }
    private void RentListView_ItemSelected (object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedItem = e.SelectedItem as RentalItem;

            // Öğenin verilerine erişin
            var itemReturnId = selectedItem.Id;
            var itemCarReg = selectedItem.CarReg;
            var itemName = selectedItem.Name;
            var itemRentalDate = selectedItem.RentalDate;
            var itemDelay = selectedItem.Delay;
            var itemFine = selectedItem.Fine;
            var itemTotalFees = selectedItem.TotalFees;

            // Verileri metin kutularına yerleştirin
            IdEntry.Text = itemReturnId.ToString();
            CarRegEntry.Text = itemCarReg;
            NameEntry.Text = itemName;
          //  ReturnDatePicker.Date =itemReturnId;
            NameEntry.Text = itemName; 
            CarRegEntry.Text = itemCarReg;
            NameEntry.Text = itemName;

            // İşlemleri gerçekleştirin
            // ...
        }
    }
    private void RentListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is DataRowView rowView)
        {
            IdEntry.Text = rowView.Row["ReturnId"].ToString();
            CarRegEntry.Text = rowView.Row["CarId"].ToString();
            NameEntry.Text = rowView.Row["CustName"].ToString();

        }
        if (e.Item is RentalItem rentalItem)
        {
            ReturnDatePicker.Date = rentalItem.RentalDate;

            DateTime returnDate = ReturnDatePicker.Date;
            TimeSpan rentalDuration = returnDate - rentalItem.RentalDate;
            int Delay = (int)rentalDuration.TotalDays;
            decimal Fine = Delay <= 0 ? 0 : Delay * 250;

            DelayEntry.Text = Delay.ToString();
            FineEntry.Text = Fine.ToString();

            decimal totalFees = rentalItem.TotalFees + Fine;
            TotalEntry.Text = totalFees.ToString();

            string nextId = FetchId();
            IdEntry.Text = nextId;
        }
    }
    string FetchId()
    {
        Con.Open();
        string query = "select ReturnId from ReturnTb1 ORDER BY ReturnId DESC ";
        SqlCommand cmd = new SqlCommand(query, Con);
        cmd.ExecuteNonQuery();
        SqlDataReader rdr = cmd.ExecuteReader();
        string count = "";
        while (rdr.Read())
        {

            int counter = Convert.ToInt32(rdr["ReturnId"].ToString());
            count = Convert.ToString(counter + 1);
            break;
        }
        Con.Close();
        return count;
    }
}