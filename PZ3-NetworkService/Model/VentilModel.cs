using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//C:\Users\Selenic\Desktop\SelenicPz3\PZ3-NetworkService\PZ3-NetworkService\Images
namespace PZ3_NetworkService.Model
{
    public class VentilModel
    {

    }
    public class Ventil : INotifyPropertyChanged
    {

        private int id;
        private string name;
        private double value;
        private Type typeV;
  
        public Ventil()
        { }
        public Ventil(int id)
        {
            this.id = id;
        }
        public Ventil(Ventil v)
        {
            Id = v.Id;
            Name = v.Name;
            Value = v.Value;
            TypeV = v.TypeV;

        }
        public double Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }
     
        
        public string Name
        {
            get => name;
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public int Id
        {
            get => id;
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }

        public Type TypeV
        {
            get => typeV;
            set
            {
                /*  typeV.Name = value.Name;
                  typeV.ImgSrc = value.ImgSrc;*/
                typeV = value;
                RaisePropertyChanged("TypeV");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
