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
            get
            {
                return this.filename;
            }

            set
            {
                this.filename = value;
                this.OnPropertyChangeed("Filename");
            }
        }

        public string Fullfilename
        {
            get
            {
                return this.fullfilename;
            }

            set
            {
                this.fullfilename = value;
                this.OnPropertyChangeed("Fullfilename");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChangeed(string propname)
        {
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propname));
        }
    }
}
