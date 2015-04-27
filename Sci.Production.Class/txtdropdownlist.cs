using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Class
{
    public partial class txtdropdownlist : Sci.Win.UI.ComboBox
    {
        private string type;
        [Category("Custom Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Type
        {
            set
            { 
                this.type = value;
                string selectCommand = string.Format("select ID, rtrim(ID)+'- '+rtrim(Name) as IDName from DropDownList where Type = '{0}' order by ID", this.Type);
                Ict.DualResult returnResult;
                DataTable DropDownListTable = new DataTable();
                if (returnResult = DBProxy.Current.Select(null, selectCommand, out DropDownListTable))
                {
                    this.DataSource = DropDownListTable;
                    this.DisplayMember = "IDName";
                    this.ValueMember = "ID";
                }
            }
            get { return this.type; }
        }

        public txtdropdownlist()
        {
            this.Size = new System.Drawing.Size(100, 24);
        }
    }
}
