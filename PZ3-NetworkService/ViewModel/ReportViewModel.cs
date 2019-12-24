using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.ViewModel
{
    public class ReportViewModel : BindableBase
    {
        private string report;
          private string path = @"C:\Users\Selenic\Desktop\SelenicPz3\PZ3-NetworkService\LogFile.txt";
        public MyICommand ShowCommand { get; set; }

        public string Report {
            get => report;
            set
            {
                report = value;
                OnPropertyChanged("Report");
            }
        }

      //  public IEnumerable<Ventil> Ventils { get; private set; }

        public void LoadReport()
        {
            string temp = "Nema izvestaja";
            report = temp;

        }
        public ReportViewModel()
        {
            LoadReport();
            ShowCommand = new MyICommand(OnShow);
        }
        private void OnShow()
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    String txt = sr.ReadToEnd();
                    string[] str_split = txt.Split('\t', ' ', '\n', '_');
                    int length = str_split.Length;
                   List<List<string>> FileData = new List<List<string>>();
                    DateTime todayIs = new DateTime();
                    todayIs = DateTime.Today;
                    Report = "";
                    for (int i=2; i<length;i=i+8)
                    {
                        if (str_split[i].Equals(todayIs.ToString("d")))
                             FileData.Add(new List<string> { str_split[i], str_split[i + 1], str_split[i + 3], str_split[i + 5] });
                    }
                    Report = "Dan " + todayIs.ToString("dd.MM.yyyy.")+ "\nPromene vrednosti  \n";
                    foreach (var v in VentilViewModel.Ventils)
                    {
                        string y = "Object " + v.Id.ToString() + ":\n";
                        string x = "";
                        foreach (List<string> ls in FileData)
                        {
                            int tempID = Int32.Parse(ls[2]);
                            string str = todayIs.ToString("dd.MM.yyyyy.");
                            if (!ls[0].Equals(str) &&(v.Id==tempID))
                            {
                                x =x+ "\tTime: " + ls[1] + " Value: " + ls[3] + "\n";
                            }
                        }
                        if (x.Equals(""))
                        {
                            Report = Report + y + "\tNema promena vrednosti\n";
                            OnPropertyChanged("Report");
                        }
                        else
                        {
                            Report = Report + y + x;
                            OnPropertyChanged("Report");
                        }
                        
                    }
                   
                }

            }
            catch
            {

            }
        }
    }
}
