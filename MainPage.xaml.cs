using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

	Ukol selected;
    private async void seznam_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
		selected = (Ukol)e.SelectedItem;
		nazevEntry.Text = selected.Nazev;
		prubehEntry.SelectedItem = selected.Status.ToString();
		terminEntry.Date = selected.Termin;
        Prubeh current = selected.Status;
		

    }

	private void CB_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
        

    }

    private void UpdateButton_Clicked(object sender, EventArgs e)
    {
		if(selected == null) { return; }

		selected.Nazev = nazevEntry.Text;
		selected.Status = (Prubeh)Enum.Parse(typeof(Prubeh), prubehEntry.SelectedItem.ToString());
		selected.Termin = terminEntry.Date;
    }
}

public class Ukol : INotifyPropertyChanged
{
	string nazev;
    public string Nazev { get => nazev; set { nazev = value; PropertyChanged.Invoke(Status, new PropertyChangedEventArgs("Nazev")); } }

	DateTime termin;
    public DateTime Termin { get=>termin; set {termin = value; PropertyChanged.Invoke(Status, new PropertyChangedEventArgs("Termin")); } }

	Prubeh status;

    public event PropertyChangedEventHandler PropertyChanged;

    public Prubeh Status { get => status; set { status = value; PropertyChanged.Invoke(Status, new PropertyChangedEventArgs("Status")); } }

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

