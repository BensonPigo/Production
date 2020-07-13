using System.ComponentModel;
using System.Data;
using Sci.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtDropdownlist
    /// </summary>
    public partial class Txtdropdownlist : Win.UI.ComboBox
    {
        private string type;

        /// <summary>
        /// Type
        /// </summary>
        [Category("Custom Properties")]
        public string Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                if (!Env.DesignTime)
                {
                    string selectCommand = string.Format("select ID, rtrim(ID)+'- '+rtrim(Name) as IDName from DropDownList WITH (NOLOCK) where Type = '{0}' order by Seq", this.Type);
                    Ict.DualResult returnResult;
                    DataTable dropDownListTable = new DataTable();
                    if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
                    {
                        this.DataSource = dropDownListTable;
                        this.DisplayMember = "IDName";
                        this.ValueMember = "ID";
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Txtdropdownlist"/> class.
        /// </summary>
        public Txtdropdownlist()
        {
        }
    }
}
