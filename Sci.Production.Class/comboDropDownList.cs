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
using Sci.Win.UI;
using System.Data.SqlClient;
using System.Windows.Forms;

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

    public partial class txtDropDownList : Sci.Win.UI.TextBox
    {
        private string type;
        [Category("Custom Properties")]
        public string Type
        {
            set
            {
                this.type = value;
               
            }
            get { return this.type; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            #region SQL CMD
            string sqlcmd = $@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{type}' 
order by Seq";
            #endregion
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, "ID,Name", this.Text, false, null);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            #region SQL CMD
            string sqlcmd = $@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{type}' 
and id ='{str}'
order by Seq";
            #endregion
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(sqlcmd) == false)
                {
                    this.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Return TO : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
