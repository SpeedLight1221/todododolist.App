using System.Security.Cryptography.X509Certificates;

namespace todododolist;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		List<ukol> ukoly = new List<ukol>();
		ukol test = new ukol("test", new DateTime(2023, 11, 8), "hotovo");
		ukoly.Add(test);
		seznam.ItemsSource = ukoly;
	}

	
}

public class ukol
{
	public string nazev;
	public DateTime termin;
	public string status;

	public ukol(string n, DateTime d, string s)
	{
		nazev = n;
		termin = d;
		status = s;
	}
}
