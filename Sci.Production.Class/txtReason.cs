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
    public partial class txtReason : Sci.Win.UI.ComboBox
    {
        private string type;
        [Category("Custom Properties")]
        public string ReasonTypeID
        {
            set
            {
                this.type = value;
                if (!Env.DesignTime)
                {
                    string selectCommand = string.Format(@"select '' ID,'ALL' IDName,-1 as no UNION ALL select ID, rtrim(ID)+'- '+rtrim(Name) as IDName ,No
                    from dbo.reason where ReasonTypeID = '{0}' order by no", this.ReasonTypeID);
                    Ict.DualResult returnResult;
                    DataTable dropDownListTable = new DataTable();
                    if (returnResult = DBProxy.Current.Select("Production", selectCommand, out dropDownListTable))
                    {
                        this.DataSource = dropDownListTable;
                        this.DisplayMember = "IDName";
                        this.ValueMember = "ID";
                    }
                }
            }
            get { return this.type; }
        }

        public txtReason()
        { }
    }
}
