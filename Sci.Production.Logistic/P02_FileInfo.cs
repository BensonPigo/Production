using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Sci.Production.Logistic
{
    public class P02_FileInfo : INotifyPropertyChanged
    {
        private string filename;
        private string fullfilename;

        public P02_FileInfo(string filename, string fullfilename)
        {
            this.filename = filename;
            this.fullfilename = fullfilename;
        }

        public string Filename
        {
            set { this.filename = value; OnPropertyChangeed("Filename"); }
            get { return this.filename; }
        }

        public string Fullfilename
        {
            set { this.fullfilename = value; OnPropertyChangeed("Fullfilename"); }
            get { return this.fullfilename; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChangeed(string propname)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new System.ComponentModel.PropertyChangedEventArgs(propname));
        }
    }
}
