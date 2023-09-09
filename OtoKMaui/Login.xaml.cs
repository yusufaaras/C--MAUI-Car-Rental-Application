
using Microsoft.Data.SqlClient;
using System.Data;

namespace OtoKMaui;

public partial class Login : ContentPage
{
    public Login()
    {
        InitializeComponent();
    }
    SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-S1QPNRR;TrustServerCertificate=true;Initial Catalog=OtoKiralamaDb;Integrated Security=True;Connect Timeout=30");

    DataTable dt = new DataTable();
    private void LoginClicked(object sender, EventArgs e)
    {
        string username = Uname.Text;
        string password = PassTb.Text;

        string connectionString = @"Data Source=.;Initial Catalog=OtoKiralamaDb;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=True;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT COUNT(*) FROM UserTb1 WHERE Uname = @Username AND Upass = @Password";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                int result = (int)command.ExecuteScalar();

                if (result == 1)
                {
                    var nextPage = new HomePage();
                    var navigationPage = new NavigationPage(nextPage);
                    Application.Current.MainPage = navigationPage;
                }
                else
                {
                    DisplayAlert("Error", "Wrong Username or Password", "OK");
                }
            }
        }
    }


    private void SignClicked(object sender, EventArgs e)
    {
        var nextPage = new SignUpPage();
        var navigationPage = new NavigationPage(nextPage);
        Application.Current.MainPage = navigationPage;
    }
        private void ClearClicked(object sender, EventArgs e)
    {
        Uname.Text = "";
        PassTb.Text = "";
    }
}

