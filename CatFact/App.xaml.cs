

namespace CatFact;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        DataCatFact.InitializeDatabase();
        MainPage = new AppShell();
	}
}
