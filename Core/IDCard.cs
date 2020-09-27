using System;
using System.ComponentModel;

namespace Shared
{
    public class IDCard 
    {
        private string name1;
        private string sex1;

        public string ImgString { get; set; }

        public string name { get; set; }

        public string cnName { get; set; }

        public string number { get; set; }

        public string peopleNation { get; set; }

        public string peopleNationCode { get; set; }

        public string validtermOfStart { get; set; }

        public string birthday { get; set; }

        public string address { get; set; }

        public string validtermOfEnd { get; set; }

        public string signdate { get; set; }

        public string sex { get; set; }

        public string samid { get; set; }

        public bool bCivic { get; set; }
        public string certType { get; set; }

        public string strType { get; set; }
        public int nStart { get; set; }
    }
}
