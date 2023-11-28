using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace todododolist;

public partial class MainPage : ContentPage
{
	int count = 0;
	public ObservableCollection<Ukol> ukoly { get; set; } = new ObservableCollection<Ukol>
	{
		new Ukol("Test",new DateTime(),Prubeh.Rozpracováno)
	};
    public MainPage()
	{   
		InitializeComponent();
        BindingContext = this;
    
		
	
	}

    private void AddTask_Clicked(object sender, EventArgs e)
    {
		if(nazevEntry.Text == "" ||prubehEntry.SelectedItem is null) {
	
			return;
		}

		Ukol u = new Ukol(nazevEntry.Text,terminEntry.Date,(Prubeh)Enum.Parse(typeof(Prubeh),prubehEntry.SelectedItem.ToString()));
		ukoly.Add(u);

		nazevEntry.Text = "";

    }
}

public class Ukol
{
	string nazev;
    public string Nazev { get => nazev; }

	DateTime termin;
    public DateTime Termin { get=>termin; }

	Prubeh status;
	public Prubeh Status { get => status; }

    public Ukol(string n, DateTime d, Prubeh s)
	{
		nazev = n;
		termin = d;
		status = s;

		
	}
}

public enum Prubeh
{
	Zadáno,
	Rozpracováno,
	Hotovo
}

