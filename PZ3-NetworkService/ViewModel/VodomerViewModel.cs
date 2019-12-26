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
    public class VodomerViewModel : BindableBase
    {
        public static List<string> VodomerTypes { get; set; } = new List<string> { "IndustrijskiVodomer", "TurbinskiVodomer", "Vodomer" };//listavodomera
        private ObservableCollection<Vodomer> FilterVodomer { get; set; } = new ObservableCollection<Vodomer>();
        public static ObservableCollection<Vodomer> Vodomeri { get; set; } = new ObservableCollection<Vodomer>();
        public static ObservableCollection<Vodomer> VodomeriCopy { get; set; } = new ObservableCollection<Vodomer>();

        //za filter
        private string selectedTypeVodomer=string.Empty;
        private int lowOrHigh = -1;
        private int inOrOutValues = -1;
        private int idForFilter = -1;
        private string idForFilterText="";
        private bool filtercan = false;
        //putanja do slike //"name.jpg"
        private string pathFirst = @"c:\Users\Selenic\Desktop\SelenicPz3\PZ3-NetworkService\PZ3-NetworkService\Images\";

        private string path="";


        private Vodomer selectedVodomer;//selektovan u listi 

        private string idText; //za vrednosti polja
        private string nameText;
       // private string valueText;
        private string selectedTypeVodomer2=string.Empty;

        public MyICommand DeleteCommand { get; set; }//kontrola komande
        public MyICommand AddCommand { get; set; }
        public MyICommand FilterCommand { get; set; }
        public MyICommand CancelFilterCommand { get; set; }
        public MyICommand LowValueCommand { get; set; }
        public MyICommand OutOfRangeCommand { get; set; }
        public MyICommand ExpectedValuesCommand { get; set; }

        public MyICommand HighValueCommand { get; set; }

        public VodomerViewModel()
        {
        
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            AddCommand = new MyICommand(OnAdd, CanAdd);
            FilterCommand = new MyICommand(OnFilter, CanFilter);
            LowValueCommand = new MyICommand(OnLow);
            HighValueCommand = new MyICommand(OnHigh);
            CancelFilterCommand = new MyICommand(OnCancel, CanCancel);
            OutOfRangeCommand = new MyICommand(OnOut);
            ExpectedValuesCommand = new MyICommand(OnExpected);
        }

        public Vodomer SelectedVodomer
        {
            get
            {
                return selectedVodomer;
            }
            set
            {
                selectedVodomer = value;
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
       
     

        public string SelectedTypeVodomer2
        {
            get => selectedTypeVodomer2;
            set
            {
                if (selectedTypeVodomer2 != value)
                {
                    selectedTypeVodomer2 = value;
                    Path = pathFirst + value.ToString() + ".jpg";
                    OnPropertyChanged("Path");
                    OnPropertyChanged("SelectedTypeVodomer2");
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public string SelectedTypeVodomer
        {
            get => selectedTypeVodomer;
            set
            {
                if (selectedTypeVodomer != value)
                {
                    selectedTypeVodomer = value;
                    OnPropertyChanged("SelectedTypeVodomer");
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
            return SelectedVodomer != null;
        }
        private void OnDelete()
        {
            Vodomeri.Remove(SelectedVodomer);
        }
        private bool CanAdd()
        {
            if (SelectedTypeVodomer2 != null && IdText != null && NameText != null)
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
            foreach(Vodomer v in Vodomeri)
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
            Model.Type temp = new Model.Type(selectedTypeVodomer2,@"C: \Users\Selenic\Desktop\SelenicPz3\PZ3-NetworkService\PZ3-NetworkService\Images\"+SelectedTypeVodomer2.ToString()+".jpg");
            Vodomeri.Add(new Vodomer { Id = tempID, Name = NameText, Value = 700, TypeV = temp });
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
            if (((LowOrHigh !=-1) || (SelectedTypeVodomer != string.Empty) ||(InOrOutValues!=-1)))
                filter= true;
            else
                filter= false;
            
            return filter;
        }
        private void OnFilter()
        {
            int val;
            FilterVodomer.Clear();
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
            
            
            VodomeriCopy.Clear();
            foreach (Vodomer v in Vodomeri)
            {
                VodomeriCopy.Add(v);
            }
            if (LowOrHigh != -1)
            {
                foreach (Vodomer v in Vodomeri)
                {
                    //filtrirati da li je vece manje od zadatog id-a
                    if (LowOrHigh ==1)
                    {
                        if (val >= v.Id)
                        {
                            FilterVodomer.Add(v);
                        }
                    }
                    else if (LowOrHigh == 2)
                    {
                        if (val <= v.Id)
                        {
                            FilterVodomer.Add(v);
                        }
                    }
                }
                FilterInVodomer();
            }
            
            if (InOrOutValues != -1)
            {
                foreach (Vodomer v in Vodomeri)
                {
                    //filtrirati po opsegu
                    if (InOrOutValues == 2)
                    {
                        if (v.Value>=5 &&v.Value<=16)
                        {
                            FilterVodomer.Add(v);
                        }
                    }
                    else if (InOrOutValues == 1)
                    {
                        if (v.Value < 5 && v.Value > 16)
                        {
                            FilterVodomer.Add(v);
                        }
                    }
                }
                FilterInVodomer();
            }
             if (SelectedTypeVodomer != string.Empty)
             {
                foreach (Vodomer v in Vodomeri)
                {
                    //filtrirati po opsegu
                    if (SelectedTypeVodomer.Equals(v.TypeV.Name))
                    {
                        FilterVodomer.Add(v);
                    }
                }
                FilterInVodomer();
             }
            IdForFilterText = "";
           LowOrHigh = -1;
            idForFilter = -1;
            InOrOutValues = -1;
            SelectedTypeVodomer = string.Empty;
            OnPropertyChanged("SelectedTypeVodomer"); 
            filtercan = true;
            OnPropertyChanged("Vodomeri");
            CancelFilterCommand.RaiseCanExecuteChanged();
        }
        private void FilterInVodomer()
        {
            Vodomeri.Clear();
            foreach (Vodomer v in FilterVodomer)
            {
                Vodomeri.Add(v);
            }
            FilterVodomer.Clear();
        }
        private bool CanCancel()
        {
            return filtercan;

        }
        private void OnCancel()
        {
            Vodomeri.Clear();
            foreach (Vodomer v in VodomeriCopy)
            {
                Vodomeri.Add(v);
            }
            OnPropertyChanged("Vodomeri");
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
