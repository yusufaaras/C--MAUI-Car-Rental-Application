using Microsoft.Data.SqlClient;
using System.Data;

namespace OtoKMaui;

public partial class DashBoardPage : ContentPage
{
	public DashBoardPage()
	{
		InitializeComponent();
	}
    SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-S1QPNRR;TrustServerCertificate=true;Initial Catalog=OtoKiralamaDb;Integrated Security=True;Connect Timeout=30");

    DataTable dt = new DataTable();

    protected override void OnAppearing()
    {
        base.OnAppearing();

        string quercar = "Select count(*) from CarTb1";
        SqlDataAdapter sda = new SqlDataAdapter(quercar, Con);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        CarLbl.Text = "Total Car:" + dt.Rows[0][0].ToString();
        string quercust = "Select count(*) from CustomerTb1";
        SqlDataAdapter sda1 = new SqlDataAdapter(quercust, Con);
        DataTable dt1 = new DataTable();
        sda1.Fill(dt1);
        CustLbl.Text = "Total Cust:" + dt1.Rows[0][0].ToString();
        string querusers = "Select count(*) from UserTb1";
        SqlDataAdapter sda2 = new SqlDataAdapter(querusers, Con);
        DataTable dt2 = new DataTable();
        sda2.Fill(dt2);
        UsersLbl.Text = "Total User:"+dt2.Rows[0][0].ToString();
    }
    private void BackClicked(object sender, EventArgs e)
	{
        var nextPage = new HomePage();
        var navigationPage = new NavigationPage(nextPage);
        Application.Current.MainPage = navigationPage;
    }
}