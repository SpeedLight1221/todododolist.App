
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace todododolist;

public partial class MainPage : ContentPage
{
    int count = 0;

    public static MainPage mp;
    public ObservableCollection<Ukol> ukoly { get; set; } = new ObservableCollection<Ukol>
    {
        new Ukol("Test",new DateTime(),Prubeh.Rozpracováno)
    };

    public ObservableCollection<Ukol> filtred { get; set; } = new ObservableCollection<Ukol>();
    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        mp = this;



    }

    private void AddTask_Clicked(object sender, EventArgs e)
    {
        if (nazevEntry.Text == "" || prubehEntry.SelectedItem is null)
        {

            return;
        }

        Ukol u = new Ukol(nazevEntry.Text, terminEntry.Date, (Prubeh)Enum.Parse(typeof(Prubeh), prubehEntry.SelectedItem.ToString()));
        ukoly.Add(u);

        nazevEntry.Text = "";
        setFilterSource(null, null);
    }

    Ukol selected;
    private void seznam_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        selected = (Ukol)e.SelectedItem;
        if(selected == null) { return; }

        nazevEntry.Text = selected.Nazev;
        prubehEntry.SelectedItem = selected.Status.ToString();
        terminEntry.Date = selected.Termin;
        Prubeh current = selected.Status;


    }


    private void UpdateButton_Clicked(object sender, EventArgs e)
    {
        if (selected == null) { return; }

        selected.Nazev = nazevEntry.Text;
        selected.Status = (Prubeh)Enum.Parse(typeof(Prubeh), prubehEntry.SelectedItem.ToString());
        selected.Termin = terminEntry.Date;
        setFilterSource(null, null);
    }

    private void RemoveButton_Clicked(object sender, EventArgs e)
    {
        if (selected == null) { return; }
        ukoly.Remove(selected);
        selected = null;
        nazevEntry.Text = "";
        terminEntry.Date = DateTime.Now;
        setFilterSource(null,null);

    }

    

   
    static DateTime? DT_from;
    static DateTime? DT_till;




    private void setFilterSource(object? sender, CheckedChangedEventArgs? e)
    {
        filtred = new ObservableCollection<Ukol>(ukoly.Where(ukolFilter));
        seznam.ItemsSource = filtred;
    }


    public bool ukolFilter(Ukol u)
    {
        if(!mp.zadáno.IsChecked && !mp.Hotovo.IsChecked && !mp.rozpracováno.IsChecked)
        {
            return true;
        }

        if (u.Status == Prubeh.Zadáno && mp.zadáno.IsChecked )
        {
            return true;
        }
        else if(u.Status == Prubeh.Zadáno && !mp.zadáno.IsChecked)
        { return false; }

        if (u.Status == Prubeh.Hotovo && mp.Hotovo.IsChecked)
        {
            return true;
        }
        else if (u.Status == Prubeh.Hotovo && !mp.Hotovo.IsChecked)
        { return false; }

        if (u.Status == Prubeh.Rozpracováno && mp.rozpracováno.IsChecked)
        {
            return true;
        }
        else if (u.Status == Prubeh.Rozpracováno && !mp.rozpracováno.IsChecked)
        { 
            return false;
        }

        return false;


    }

    private void dp_from_DateSelected(object sender, DateChangedEventArgs e)
    {
        DT_from = dp_from.Date;
        setFilterSource(null, null);
    }

    private void dp_till_DateSelected(object sender, DateChangedEventArgs e)
    {
        DT_till = dp_till.Date;
        setFilterSource(null, null);
    }

   

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        Load();
    }

    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
        Save();
    }

    private void Load()
    {
        ukoly.Clear();

        try
        {
            using (StreamReader sr = new StreamReader(@"C:\TEMP\Data.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    ukoly.Add(new Ukol(s));

                }

            }
        }
        catch {
        }
    }

    private void Save()
    {
        using (StreamWriter sw = new StreamWriter(@"C:\TEMP\Data.txt"))
        {
            foreach (Ukol u in ukoly)
            {
                sw.WriteLine(u.ToString());
            }
        }
    }




}

public class Ukol : INotifyPropertyChanged
{
    string nazev;
    public string Nazev { get => nazev; set { nazev = value; PropertyChanged.Invoke(Status, new PropertyChangedEventArgs("Nazev")); } }

    DateTime termin;
    public DateTime Termin { get => termin; set { termin = value; PropertyChanged.Invoke(Status, new PropertyChangedEventArgs("Termin")); } }

    Prubeh status;

    public event PropertyChangedEventHandler PropertyChanged;

    public Prubeh Status { get => status; set { status = value; PropertyChanged.Invoke(Status, new PropertyChangedEventArgs("Status")); } }

    public Ukol(string n, DateTime d, Prubeh s)
    {
        nazev = n;
        termin = d;
        status = s;


    }


    public Ukol(string s)
    {
        string[] r = s.Split("\t");
        nazev = r[0];
        termin = Convert.ToDateTime(r[1]);
        status = (Prubeh)Enum.Parse(typeof(Prubeh), r[2]);
    }

    public override string ToString()
    {
        return $"{Nazev}\t{Termin}\t{Status}";
    }
}

public enum Prubeh
{
    Zadáno,
    Rozpracováno,
    Hotovo
}

