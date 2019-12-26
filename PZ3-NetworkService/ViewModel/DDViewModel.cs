using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PZ3_NetworkService.ViewModel
{
    public class DDViewModel:BindableBase
    {
        public static Vodomer draggItm = null;
        private bool dragging = false;
        private static bool exists = false;
        private int selectedIndex = 0;

        private ListView lv;
        public BindingList<Vodomer> Items { get; set; }
        //komande 
        public MyICommand<ListView> MLBUCommand { get; set; }
        public MyICommand<Vodomer> SCCommand { get; set; }
        public MyICommand<Canvas> DCommand { get; set; }//on drop
        public MyICommand<Canvas> FreeCommand { get; set; }//on free
        public MyICommand<Canvas> DOCommand { get; set; }//drag over
        public MyICommand<Canvas> DLCommand { get; set; }//drag over leavew
        public MyICommand<Canvas> LCommand { get; set; }//on load
        public MyICommand<ListView> LLWCommand { get; set; }
        public List<Canvas> CanvasList { get; set; } = new List<Canvas>();
        
        //obj koji su dodati u canvas
        public static Dictionary<string, Vodomer> CanvasObj { get; set; } = new Dictionary<string, Vodomer>();
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
                    
            }
        }
        //napraviti komande  

        public DDViewModel()
        {
            Items = new BindingList<Vodomer>();
                   
                foreach(Vodomer ven in VodomerViewModel.Vodomeri)
                {
                     exists = false;
                    foreach(Vodomer Vodmer in CanvasObj.Values)
                    {
                        if(ven.Id==Vodmer.Id)
                        {
                        exists = true;
                            break;
                        }
                       
                    }
                if (exists == false)
                    Items.Add(new Vodomer(ven));
            }
        
                        //komande 
            MLBUCommand = new MyICommand<ListView>(OnMLBU);
            SCCommand = new MyICommand<Vodomer>(SelectionChange);
            DCommand = new MyICommand<Canvas>(OnDrop);
            FreeCommand = new MyICommand<Canvas>(OnFree);
            DOCommand = new MyICommand<Canvas>(OnDragOver);
            DLCommand = new MyICommand<Canvas>(OnDragLeave);
            LCommand = new MyICommand<Canvas>(OnLoad);
            LLWCommand = new MyICommand<ListView>(OnLLW);

        }
        public void OnLLW(ListView listview)
        {
            //lv dobija vrednost ListView-a gde su obj
            lv = listview;
        }
        public void OnLoad(Canvas c)
        {
            bool exist = false;
            foreach(Vodomer v in VodomerViewModel.Vodomeri)
            {
                if(CanvasObj.ContainsKey(c.Name))
                if (CanvasObj[c.Name].Id == v.Id)
                    exist = true;
            }
            if(exist)
            {
                if (CanvasObj.ContainsKey(c.Name))
                {
                    BitmapImage logo = new BitmapImage();//za jednostavniji prikaz slike
                    logo.BeginInit();//signal za pocetak inicijalizacije
                    string temp = CanvasObj[c.Name].TypeV.Name.ToString() + ".jpg";
                    logo.UriSource = new Uri(temp, UriKind.Relative);
                    logo.EndInit();
                    c.Background = new ImageBrush(logo);
                    ((TextBlock)(c).Children[1]).Text = "";
                    ((TextBlock)(c).Children[3]).Text = CanvasObj[c.Name].Id + ". " + CanvasObj[c.Name].Name;
                    c.Resources.Add("taken", true);
                    CheckValue(c);

                }
                if (!CanvasList.Contains(c))
                {
                    CanvasList.Add(c);
                }
            }
            else
            {
                c.Background = Brushes.White;
                foreach (Vodomer v in VodomerViewModel.Vodomeri)
             
                c.Resources.Remove("taken");
                ((TextBlock)(c).Children[3]).Text = "";
                ((TextBlock)(c).Children[3]).Background = Brushes.Transparent;

                CanvasObj.Remove(c.Name);
            }
           
        }

        private void CheckValue(Canvas c)
        {
            Dictionary<int, Vodomer> temp = new Dictionary<int, Vodomer>();
            foreach(var v in VodomerViewModel.Vodomeri)
            {
                temp.Add(v.Id, v);
            }
           
            Task.Delay(1000).ContinueWith(_ =>
              {
                  Application.Current.Dispatcher.Invoke(() =>
                      {
                          if (CanvasObj.Count != 0)
                          {
                              if (CanvasObj.ContainsKey(c.Name))
                              {
                                  //try i onfs ga izbaciti izbaciti sa kanvasa
                                  if (temp[CanvasObj[c.Name].Id].Value < 670 || temp[CanvasObj[c.Name].Id].Value >730)
                                  {
                                      ((Border)(c).Children[0]).BorderBrush = Brushes.Red;
                                  }
                                  else
                                  {
                                      ((Border)(c).Children[0]).BorderBrush = Brushes.Transparent;
                                  }
                              }
                              else
                              {
                                  //c.Background = Brushes.White;
                      
                                  //c.Resources.Remove("taken");
                                  //((TextBlock)(c).Children[3]).Text = "";
                                  //((TextBlock)(c).Children[3]).Background = Brushes.Transparent;

                                  //CanvasObj.Remove(c.Name);
                                  //((Border)(c).Children[0]).BorderBrush = Brushes.Transparent;
                              }
                          }
                  });
                  CheckValue(c);
            });
        }
        public void OnDragLeave(Canvas c)
        {//kad se skloni slika sa kanvasa koja se prevlaci
            if (((TextBlock)(c).Children[1]).Text == "!")
            {
                ((TextBlock)(c).Children[1]).Text = "";
                ((TextBlock)(c).Children[1]).Background = Brushes.Transparent;
      
            }
        }
        public void OnDragOver(Canvas c)
        {//prilikom prelaska preko zauzete povrsine
            if (c.Resources["taken"] != null)
            {
                ((TextBlock)(c).Children[1]).Text = "!";
                ((TextBlock)(c).Children[1]).Background = Brushes.Salmon;
            }
        }
        public void OnFree(Canvas c)
        {
            try
            {
                if(c.Resources["taken"]!=null)
                {
                    //ako je
                    c.Background = Brushes.White;
                    foreach (Vodomer v in VodomerViewModel.Vodomeri)
                    {
                        if (!Items.Contains(v) && CanvasObj[c.Name].Id == v.Id)
                            Items.Add(new Vodomer(v));
                    }
                    c.Resources.Remove("taken");
                    ((TextBlock)(c).Children[3]).Text = "";
                    ((TextBlock)(c).Children[3]).Background = Brushes.Transparent;
      
                    CanvasObj.Remove(c.Name);
                }
            }
            catch (Exception) { }
            
        }
        public void OnDrop(Canvas c)
        {
            if (((TextBlock)(c).Children[1]).Text == "!")
            {
                ((TextBlock)(c).Children[1]).Text = "";
                ((TextBlock)(c).Children[1]).Foreground = Brushes.White;
            }
            if(draggItm!=null)
            {
                if(c.Resources["taken"]==null)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    string temp = draggItm.TypeV.Name.ToString() + ".jpg";
                    logo.UriSource = new Uri(temp,UriKind.Relative);
                    logo.EndInit();
                    c.Background = new ImageBrush(logo);
                    CanvasObj[c.Name] = draggItm;
                    c.Resources.Add("taken", true);
                    ((TextBlock)(c).Children[3]).Text = draggItm.Id+". "+ draggItm.Name;
                    ((TextBlock)(c).Children[3]).Background = Brushes.Transparent;
                    Items.Remove(Items.Single(x => x.Id == draggItm.Id));
                    SelectedIndex = 0;
                    CheckValue(c);
                }
                dragging = false;
            }
        }
        public void OnMLBU(ListView lw)
        {//podizanje klika, sve stavi na null/false ->ponisti sve
            draggItm = null;
            lw.SelectedItem = null;
            dragging = false;
        }
        public void SelectionChange(Vodomer v)
        {
            if (!dragging)
            {//izvrsi promenu ako nema pomeranja
                try {
                    dragging = true;
                    draggItm = new Vodomer(v);
                    DragDrop.DoDragDrop(lv, draggItm, DragDropEffects.Move);
                }catch
                {

                }
                }
        }
    }
}
