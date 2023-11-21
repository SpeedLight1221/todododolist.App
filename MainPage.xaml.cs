using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace todododolist;

public partial class MainPage : ContentPage
{
	int count = 0;
	public ObservableCollection<Ukol> ukoly { get; set; } = new ObservableCollection<Ukol>
	{
		new Ukol("Test",new DateTime(),"Zadáno")
	};
    public MainPage()
	{
		InitializeComponent();
        BindingContext = this;
    
		
	
	}

    private void AddTask_Clicked(object sender, EventArgs e)
    {
		if(nazevEntry.Text is null||prubehEntry.SelectedItem is null) {
	
			return;
		}

		Ukol u = new Ukol(nazevEntry.Text,terminEntry.Date,prubehEntry.SelectedItem.ToString());
		ukoly.Add(u);

		nazevEntry.Text = "";

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

