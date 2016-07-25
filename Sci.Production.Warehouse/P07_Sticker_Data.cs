using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Warehouse
{
    public class P07_Sticker_Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private bool sele;
        public bool selected
        {
            get { return this.sele; }
            set
            {
                this.sele = value;
                NotifyPropertyChanged();
            }
        }
        public string POID { get; set; }
        public string Brand { get; set; }
        public string QTY { get; set; }
        public string Unit { get; set; }
        public string RefNo { get; set; }
        public string Style { get; set; }
        public string Desc { get; set; }
        public string Color { get; set; }
        public string MRName { get; set; }
        public string Season { get; set; }
        public string SEQ { get; set; }
        public string Roll { get; set; }
        public string Dyelot { get; set; }
        public string RcvDate { get; set; }
    }
}
