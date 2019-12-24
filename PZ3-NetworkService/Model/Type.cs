using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    public class Type: INotifyPropertyChanged
    {
        private string name;
        private string imgSrc;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }

        }
        public string ImgSrc { get => imgSrc; set
            {
                imgSrc = value;
                RaisePropertyChanged("ImgSrc");
            }
        }
        public Type(string namee, string src)
        {
            Name = namee;
            ImgSrc = src;
        }
        public Type()
        {
            name = string.Empty;
            imgSrc = string.Empty;
        }
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
