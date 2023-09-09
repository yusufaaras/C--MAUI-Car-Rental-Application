using Microsoft.Data.SqlClient;
using System.Data;

namespace OtoKMaui;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}
    SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-S1QPNRR;TrustServerCertificate=true;Initial Catalog=OtoKiralamaDb;Integrated Security=True;Connect Timeout=30");

    DataTable dt = new DataTable();
    private void populate()
    {
        Con.Open();
        string query = "select * from UserTb1";
        SqlDataAdapter da = new SqlDataAdapter(query, Con);
        SqlCommandBuilder builder = new SqlCommandBuilder(da);
        var ds = new DataSet();
        da.Fill(ds);
        Con.Close();
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
                var nextPage = new Login();
                var navigationPage = new NavigationPage(nextPage);
                Application.Current.MainPage = navigationPage;
            }
            catch (Exception Myex)
            {
                DisplayAlert("Error", Myex.Message, "OK");
            }
        }
    }
    private void BackClicked(object sender, EventArgs e)
	{
        var nextPage = new Login();
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