using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Class
{
    public partial class comboDropDownList : Sci.Win.UI.ComboBox
    {
        private string type;
        [Category("Custom Properties")]
        public string Type
        {
            set
            {
                this.type = value;
                if (!Env.DesignTime)
                {
                    string selectCommand = string.Format(@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{0}' 
order by Seq", this.Type);
                    Ict.DualResult returnResult;
                    DataTable dropDownListTable = new DataTable();
                    if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
                    {
                        this.DataSource = dropDownListTable;
                        this.DisplayMember = "Name";
                        this.ValueMember = "ID";
                    }
                }
            }
            get { return this.type; }
        }

        public comboDropDownList()
        {
            InitializeComponent();
        }

        public comboDropDownList(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
