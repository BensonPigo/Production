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
    public partial class txtFinanceEnReason : Sci.Win.UI.ComboBox
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
                    string selectCommand = string.Format(@"select ID, rtrim(ID)+'- '+rtrim(Name) as IDName 
                    from dbo.reason left join Production.dbo.System a on a.ExchangeID=Reason.ID where ReasonTypeID = '{0}' order by a.ExchangeID desc", this.ReasonTypeID);
                    Ict.DualResult returnResult;
                    DataTable dropDownListTable = new DataTable();
                    if (returnResult = DBProxy.Current.Select("Finance", selectCommand, out dropDownListTable))
                    {
                        this.DataSource = dropDownListTable;
                        this.DisplayMember = "IDName";
                        this.ValueMember = "ID";
                        
                    }
                }
            }
            get { return this.type; }
        }

        public txtFinanceEnReason()
        { }
    }
}
