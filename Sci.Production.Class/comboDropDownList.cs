using Ict.Win;
using Ict.Win.UI;
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

        public comboDropDownList(System.ComponentModel.IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }

    public class cellDropDownList : DataGridViewGeneratorComboBoxColumnSettings
    {
        public static DataGridViewGeneratorComboBoxColumnSettings GetGridCell(string Type)
        {
            cellDropDownList cellcb = new cellDropDownList();
            if (!Env.DesignTime)
            {
                string selectCommand = string.Format(@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{0}' 
order by Seq", Type);
                Ict.DualResult returnResult;
                DataTable dropDownListTable = new DataTable();
                Dictionary<string, string> di_dropdown = new Dictionary<string, string>();
                if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
                {   
                    cellcb.DataSource = dropDownListTable;
                    cellcb.DisplayMember = "Name";
                    cellcb.ValueMember = "ID";                    
                   
                }
            }
            return cellcb;
        }
    }
}
