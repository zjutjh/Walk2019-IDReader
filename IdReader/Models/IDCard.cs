using System;
using System.ComponentModel;

namespace IdReader.Models
{
    public class IDCard : INotifyPropertyChanged
    {
        private string name1;
        private string sex1;

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        public string ImgString { get; set; }

        public string name { get { return name1; }set { name1 = value; NotifyPropertyChanged(nameof(name)); } }


        public string cnName { get; set; }


        public string number { get; set; }


        public string peopleNation { get; set; }


        public string peopleNationCode { get; set; }

        public string validtermOfStart { get; set; }

        public string birthday { get; set; }


        public string address { get; set; }


        public string validtermOfEnd { get; set; }


        public string signdate { get; set; }

        public string sex { get => sex1; set { sex1 = value; NotifyPropertyChanged(nameof(sex)); } }


        public string samid { get; set; }


        public bool bCivic { get; set; }
        public string certType { get; set; }

        public string strType { get; set; }
        public int nStart { get; set; }

    }
}
