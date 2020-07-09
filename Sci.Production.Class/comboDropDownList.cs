using Sci.Data;
using System.ComponentModel;
using System.Data;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// ComboDropDownList
    /// </summary>
    public partial class ComboDropDownList : Win.UI.ComboBox
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
                    string selectCommand = string.Format(
                        @"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{0}' 
order by Seq", this.Type);
                    DualResult returnResult;
                    DataTable dropDownListTable = new DataTable();
                    if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
                    {
                        this.DataSource = dropDownListTable;
                        this.DisplayMember = "Name";
                        this.ValueMember = "ID";
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboDropDownList"/> class.
        /// </summary>
        public ComboDropDownList()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboDropDownList"/> class.
        /// </summary>
        /// <param name="container">container</param>
        public ComboDropDownList(System.ComponentModel.IContainer container)
        {
            container.Add(this);

            this.InitializeComponent();
        }
    }
}
