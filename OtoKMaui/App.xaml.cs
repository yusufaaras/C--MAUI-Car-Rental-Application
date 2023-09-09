namespace OtoKMaui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new Login();
	}
}
