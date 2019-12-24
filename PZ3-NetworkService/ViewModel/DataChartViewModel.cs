using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.ViewModel
{

    public class DataChartViewModel : BindableBase
    {
        public static List<string> VentilsBar { get; set; } = new List<string>();
        private List<string> values = new List<string>();
        private List<string> time = new List<string>();
        private string selectedId = "";
        //za graf u procentima
        private int[] types = new int[] { 0, 0, 0 };
        private List<string> lengthOfOneType = new List<string>();
        private List<string> positionOnCanvasBottom = new List<string>();

        public MyICommand ShowBarsCommand { get; set; }
        private string path = @"C:\Users\Selenic\Desktop\SelenicPz3\PZ3-NetworkService\LogFile.txt";
        public string SelectedId {
            get => selectedId;
            set
            {
                selectedId = value;
                OnPropertyChanged("SelectedId");
                ShowBarsCommand.RaiseCanExecuteChanged();
            }
        }
        public List<string> Values { get => values; set
            {
                values = value;
                OnPropertyChanged("Values");
            }
        }
        public List<string> Time { get => time; set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }

        public List<string> LengthOfOneType { get => lengthOfOneType;
            set
            {
                lengthOfOneType = value;
                OnPropertyChanged("LengthOfOneType");
            }
        }
        public List<string> PositionOnCanvasBottom { get => positionOnCanvasBottom;
            set
            {
                positionOnCanvasBottom = value;
                OnPropertyChanged("PositionOnCanvasBottom");
            }
        }

        public DataChartViewModel()
        {
            VentilsBar.Clear();
          for (int i=0; i<7;i++)
          {
               
                Values.Add("0");
                Time.Add( "Not defined");
          }

            foreach(Ventil v in VentilViewModel.Ventils)
            {
                VentilsBar.Add(v.Id.ToString());
                if(v.TypeV.Name== "Nepovratni ventil")
                {
                    types[0]++;
                }
                if (v.TypeV.Name == "Vodeni ventil")
                {
                    types[1]++;
                }
                if (v.TypeV.Name == "Ventil pod pritiskom")
                {
                    types[2]++;
                }
            }
            DravGraf();
            OnPropertyChanged("PositionOnCanvasBottom");
            OnPropertyChanged("LengthOfOneType");
            ShowBarsCommand = new MyICommand(OnShow, CanShow);
        }
        public void OnShow()
        {
            DateTime todayIs;
            List<List<string>> FileData = new List<List<string>>();
            List<List<string>> FileDataForSelectedID = new List<List<string>>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    String txt = sr.ReadToEnd();
                    string[] str_split = txt.Split('\t', ' ', '\n', '_');
                    int length = str_split.Length;
                    todayIs = DateTime.Today;
                    for (int i = 2; i < length; i = i + 8)
                    {
                        if (str_split[i].Equals(todayIs.ToString("d")))
                            FileData.Add(new List<string> { str_split[i], str_split[i + 1], str_split[i + 3], str_split[i + 5] });
                    }
                }

            }
            catch
            {

            }
            int j = 0;
            foreach (List<string> ls in FileData)
            {
                if(SelectedId==ls[2])
                {
                    if (j < 7) {
                        FileDataForSelectedID.Add(ls);
                        j++;
                    }
                    else
                    {
                        FileDataForSelectedID.RemoveAt(0);
                        FileDataForSelectedID.Add(ls);
                    }
                }
            }
            j = 0;
            foreach (List<string> ls in FileDataForSelectedID)
            {
                double temp = double.Parse(ls[3]);
               temp = temp*15 ;
                int temp2 = (int)temp;
                Values[j] = temp2.ToString();
                Time[j] = ls[1];
                OnPropertyChanged("Values");
                OnPropertyChanged("Time");
                j++;
            }
            for(;j<7; j++)
            {

                Values[j]="0";
                Time[j]="Not defined";
                OnPropertyChanged("Values");
                OnPropertyChanged("Time");
            }


        }
        public bool  CanShow()
        {
            if (SelectedId.Equals( ""))
                return false;
            else
                return true;
        }
        private void DravGraf()
        {
            PositionOnCanvasBottom.Clear();
            LengthOfOneType.Clear();
            double ukupno = types[0] + types[1] + types[2];
            double duzinaJednog = 200 / ukupno;
            int[] temp2 = new int[] { 0, 0, 0 };
            for(int i = 0; i < 3; i++)
            {
                double temp = duzinaJednog * types[i];
                 temp2[i] = (int)temp;
                LengthOfOneType.Add(temp2[i].ToString());
            }
            int temp3 = 90;
            PositionOnCanvasBottom.Add(temp3.ToString());
            temp3 = temp3 + temp2[0];
            PositionOnCanvasBottom.Add(temp3.ToString());
            temp3 = temp3 + temp2[1];
            PositionOnCanvasBottom.Add(temp3.ToString());

        }
    }
}
