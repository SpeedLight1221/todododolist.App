using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace todododolist;

public partial class MainPage : ContentPage
{
	int count = 0;
	public ObservableCollection<Ukol> ukoly { get; set; }
    public MainPage()
	{
		InitializeComponent();
        ukoly = new ObservableCollection<Ukol>();
        Ukol test = new Ukol("test", new DateTime(2023, 11, 8), "hotovo");
		ukoly.Add(test);
		BindingContext = this;
	
	}

	
}

public class Ukol
{
	public string nazev { private set; get; }
	public DateTime termin { private set; get; }
    public string status { private set; get; }

    public Ukol(string n, DateTime d, string s)
	{
		nazev = n;
		termin = d;
		status = s;
	}
}
