using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PZ3_NetworkService.ViewModel
{
    public class VentilViewModel : BindableBase
    {
        public static List<string> VentilTypes { get; set; } = new List<string> { "Ventil pod pritiskom", "Nepovratni ventil", "Vodeni ventil" };//listaventila
        private ObservableCollection<Ventil> FilterVentils { get; set; } = new ObservableCollection<Ventil>();
        public static ObservableCollection<Ventil> Ventils { get; set; } = new ObservableCollection<Ventil>();
        public static ObservableCollection<Ventil> VentilsCopy { get; set; } = new ObservableCollection<Ventil>();

        //za filter
        private string selectedTypeVent=string.Empty;
        private int lowOrHigh = -1;
        private int inOrOutValues = -1;
        private int idForFilter = -1;
        private string idForFilterText="";
        private bool filtercan = false;
        //putanja do slike //"name.jpg"
        private string pathFirst = @"c:\Users\Selenic\Desktop\SelenicPz3\PZ3-NetworkService\PZ3-NetworkService\Images\";

        private string path="";


        private Ventil selectedVentil;//selektovan u listi 

        private string idText; //za vrednosti polja
        private string nameText;
       // private string valueText;
        private string selectedTypeVent2=string.Empty;

        public MyICommand DeleteCommand { get; set; }//kontrola komande
        public MyICommand AddCommand { get; set; }
        public MyICommand FilterCommand { get; set; }
        public MyICommand CancelFilterCommand { get; set; }
        public MyICommand LowValueCommand { get; set; }
        public MyICommand OutOfRangeCommand { get; set; }
        public MyICommand ExpectedValuesCommand { get; set; }

        public MyICommand HighValueCommand { get; set; }

        public VentilViewModel()
        {
         //   LoadVentils();
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            AddCommand = new MyICommand(OnAdd, CanAdd);
            FilterCommand = new MyICommand(OnFilter, CanFilter);
            LowValueCommand = new MyICommand(OnLow);
            HighValueCommand = new MyICommand(OnHigh);
            CancelFilterCommand = new MyICommand(OnCancel, CanCancel);
            OutOfRangeCommand = new MyICommand(OnOut);
            ExpectedValuesCommand = new MyICommand(OnExpected);
        }

        public Ventil SelectedVentil
        {
            get
            {
                return selectedVentil;
            }
            set
            {
                selectedVentil = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }

        }


        public string IdText
        {
            get
            {
                return idText;
            }
            set
            {
                if (idText != value)
                {
                    idText = value;
                    OnPropertyChanged("IdText");
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public string NameText
        {
            get
            {
                return nameText;
            }
            set
            {
                if (nameText != value)
                {
                    nameText = value;
                    OnPropertyChanged("NameText");
                    AddCommand.RaiseCanExecuteChanged();

                }
            }
        }
       
     

        public string SelectedTypeVent2
        {
            get => selectedTypeVent2;
            set
            {
                if (selectedTypeVent2 != value)
                {
                    selectedTypeVent2 = value;
                    Path = pathFirst + value.ToString() + ".jpg";
                    OnPropertyChanged("Path");
                    OnPropertyChanged("SelectedTypeVentil2");
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public string SelectedTypeVent
        {
            get => selectedTypeVent;
            set
            {
                if (selectedTypeVent != value)
                {
                    selectedTypeVent = value;
                    OnPropertyChanged("SelectedTypeVentil");
                    FilterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int LowOrHigh { get => lowOrHigh; set => lowOrHigh = value; }
        public int IdForFilter { get => idForFilter; set => idForFilter = value; }
        public string IdForFilterText { get => idForFilterText; set => idForFilterText = value; }
        public string Path {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        public int InOrOutValues { get => inOrOutValues; set => inOrOutValues = value; }

        private bool CanDelete()
        {
            return SelectedVentil != null;
        }
        private void OnDelete()
        {
            Ventils.Remove(SelectedVentil);
        }
        private bool CanAdd()
        {
            if (SelectedTypeVent2 != null && IdText != null && NameText != null)
                return true;
            return false;
        }
        private void OnAdd()
        {//validacija
            int tempID = 0;
            try
            {
                tempID = Int32.Parse(IdText);
            }
            catch
            {
                System.Windows.MessageBox.Show("ID must be a number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string tempS = NameText;
            if(string.IsNullOrWhiteSpace(tempS))
            {
                System.Windows.MessageBox.Show("The name must not be a white space!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool exists = false;
            foreach(Ventil v in Ventils)
            {
                if(v.Id==tempID)
                {
                    exists = true;
                }
            }
            if(exists)
            {
                System.Windows.MessageBox.Show("ID must be unique!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Model.Type temp = new Model.Type(selectedTypeVent2,@"C: \Users\Selenic\Desktop\SelenicPz3\PZ3-NetworkService\PZ3-NetworkService\Images\"+SelectedTypeVent2.ToString()+".jpg");
            Ventils.Add(new Ventil { Id = tempID, Name = NameText, Value = 5.5, TypeV = temp });
        }
        //funkcije za postavljanje rezima filtera
        private void OnLow()
        {
            LowOrHigh = 1;
            FilterCommand.RaiseCanExecuteChanged();
        }
        private void OnHigh()
        {
            LowOrHigh = 2;
            FilterCommand.RaiseCanExecuteChanged();
        }
        private bool CanFilter()
        {
            bool filter=false;
            if (((LowOrHigh !=-1) || (SelectedTypeVent != string.Empty) ||(InOrOutValues!=-1)))
                filter= true;
            else
                filter= false;
            
            return filter;
        }
        private void OnFilter()
        {
            int val;
            FilterVentils.Clear();
            if (IdForFilterText == "")
                val = -1;
            else
            {
                try
                {
                    val = Int32.Parse(IdForFilterText);
                }
                catch
                {
                    System.Windows.MessageBox.Show("ID must be a number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            
            
            VentilsCopy.Clear();
            foreach (Ventil v in Ventils)
            {
                VentilsCopy.Add(v);
            }
            if (LowOrHigh != -1)
            {
                foreach (Ventil v in Ventils)
                {
                    //filtrirati da li je vece manje od zadatog id-a
                    if (LowOrHigh ==1)
                    {
                        if (val >= v.Id)
                        {
                            FilterVentils.Add(v);
                        }
                    }
                    else if (LowOrHigh == 2)
                    {
                        if (val <= v.Id)
                        {
                            FilterVentils.Add(v);
                        }
                    }
                }
                FilterInVentils();
            }
            
            if (InOrOutValues != -1)
            {
                foreach (Ventil v in Ventils)
                {
                    //filtrirati po opsegu
                    if (InOrOutValues == 2)
                    {
                        if (v.Value>=5 &&v.Value<=16)
                        {
                            FilterVentils.Add(v);
                        }
                    }
                    else if (InOrOutValues == 1)
                    {
                        if (v.Value < 5 && v.Value > 16)
                        {
                            FilterVentils.Add(v);
                        }
                    }
                }
                FilterInVentils();
            }
             if (SelectedTypeVent != string.Empty)
             {
                foreach (Ventil v in Ventils)
                {
                    //filtrirati po opsegu
                    if (SelectedTypeVent.Equals(v.TypeV.Name))
                    {
                        FilterVentils.Add(v);
                    }
                }
                FilterInVentils();
             }
            IdForFilterText = "";
           LowOrHigh = -1;
            idForFilter = -1;
            InOrOutValues = -1;
            SelectedTypeVent = string.Empty;
            OnPropertyChanged("SelectedTypeVent"); 
            filtercan = true;
            OnPropertyChanged("Ventils");
            CancelFilterCommand.RaiseCanExecuteChanged();
        }
        private void FilterInVentils()
        {
            Ventils.Clear();
            foreach (Ventil v in FilterVentils)
            {
                Ventils.Add(v);
            }
            FilterVentils.Clear();
        }
        private bool CanCancel()
        {
            return filtercan;

        }
        private void OnCancel()
        {
            Ventils.Clear();
            foreach (Ventil v in VentilsCopy)
            {
                Ventils.Add(v);
            }
            OnPropertyChanged("Ventils");
            filtercan = false;
            CancelFilterCommand.RaiseCanExecuteChanged();

        }
        private void OnOut()
        {
            InOrOutValues = 1;
            FilterCommand.RaiseCanExecuteChanged();
        }
        private void OnExpected()
        {
            InOrOutValues = 2;
            FilterCommand.RaiseCanExecuteChanged();
        }
    }
}
