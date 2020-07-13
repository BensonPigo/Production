using System.ComponentModel;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P02_FileInfo
    /// </summary>
    public class P02_FileInfo : INotifyPropertyChanged
    {
        private string filename;
        private string fullfilename;

        /// <summary>
        /// P02_FileInfo
        /// </summary>
        /// <param name="filename">filename</param>
        /// <param name="fullfilename">fullfilename</param>
        public P02_FileInfo(string filename, string fullfilename)
        {
            this.filename = filename;
            this.fullfilename = fullfilename;
        }

        /// <summary>
        /// Filename
        /// </summary>
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

        /// <summary>
        /// Fullfilename
        /// </summary>
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

        /// <summary>
        /// PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChangeed(string propname)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
