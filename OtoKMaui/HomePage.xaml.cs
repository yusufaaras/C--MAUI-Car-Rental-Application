using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;

namespace OtoKMaui;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}
    private void CarClicked(object sender, EventArgs e)
	{
        var nextPage = new CarPage();
        var navigationPage = new NavigationPage(nextPage);
        Application.Current.MainPage = navigationPage;
    }
    private void CustomerClicked(object sender, EventArgs e)
    {
        var nextPage = new CusomerPage();
        var navigationPage = new NavigationPage(nextPage);
        Application.Current.MainPage = navigationPage;
    }
    private void RentalClicked(object sender, EventArgs e)
    {
        var nextPage = new RentalPage();
        var navigationPage = new NavigationPage(nextPage);
        Application.Current.MainPage = navigationPage;
    }
    private void ReturnClicked(object sender, EventArgs e)
    {
        var nextPage = new ReturnPage();
        var navigationPage = new NavigationPage(nextPage);
        Application.Current.MainPage = navigationPage;
    }
    private void UsersClicked(object sender, EventArgs e)
    {
        var nextPage = new UsersPage();
        var navigationPage = new NavigationPage(nextPage);
        Application.Current.MainPage = navigationPage;
    }
    private void DashBoardClicked(object sender, EventArgs e)
    {
        var nextPage = new DashBoardPage();
        var navigationPage = new NavigationPage(nextPage);
        Application.Current.MainPage = navigationPage;
    }

    [Obsolete]
    private void LogoutClicked(object sender, EventArgs e)
    {
        Environment.Exit(0);
    }

}