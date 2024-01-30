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

    public ObservableCollection<Ukol> filtred { get; set; } = new ObservableCollection<Ukol>();
    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;



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

    }

    Ukol selected;
    private void seznam_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        selected = (Ukol)e.SelectedItem;
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
    }

    private void RemoveButton_Clicked(object sender, EventArgs e)
    {
        if (selected == null) { return; }
        ukoly.Remove(selected);
        selected = null;
        nazevEntry.Text = "";
        terminEntry.Date = DateTime.Now;

    }

    private void zadáno_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Filter(Prubeh.Zadáno, e.Value);
    }

    private void rozpracováno_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Filter(Prubeh.Rozpracováno, e.Value);
    }

    private void Hotovo_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Filter(Prubeh.Hotovo, e.Value);
    }

    Predicate<Ukol> filterList = ukolFilter;

    static DateTime? DT_from;
    static DateTime? DT_till;
    static Prubeh? filtr;




    private void Filter(Prubeh p, bool c)
    {
        if (c == false)
        {
            filtr = null;
            setFilterSource();
            return;
        }


        if (p != Prubeh.Hotovo) { Hotovo.IsChecked = false; }
        if (p != Prubeh.Zadáno) { zadáno.IsChecked = false; }
        if (p != Prubeh.Rozpracováno) { rozpracováno.IsChecked = false; }

        setFilterSource();
    }

    private void setFilterSource()
    {
        filtred = new ObservableCollection<Ukol>(ukoly.Where(ukolFilter));
        seznam.ItemsSource = filtred;
    }


    public static bool ukolFilter(Ukol u)
    {
        if (DT_from != null && u.Termin < DT_from)
        {
            return false;
        }

        if (DT_till != null && u.Termin > DT_till)
        {
            return false;
        }

        if (u.Status != filtr)
        {
            return false;
        }

        return true;

    }

    private void dp_from_DateSelected(object sender, DateChangedEventArgs e)
    {
        DT_from = dp_from.Date;
        setFilterSource();
    }

    private void dp_till_DateSelected(object sender, DateChangedEventArgs e)
    {
        DT_till = dp_till.Date;
        setFilterSource();
    }

    private void Read_Clicked(object sender, EventArgs e)
    {
        ukoly.Clear();

        using (StreamReader sr = new StreamReader(@"C:\TEMP\Data.txt"))
        {
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                ukoly.Add(new Ukol(s));

            }

        }
    }
    private void Write_Clicked(object sender, EventArgs e)
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

