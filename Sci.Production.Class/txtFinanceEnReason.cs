using System.ComponentModel;
using System.Data;
using Sci.Data;

namespace Sci.Production.Class
{
    public partial class txtFinanceEnReason : Sci.Win.UI.ComboBox
    {
        private string type;

        [Category("Custom Properties")]
        public string ReasonTypeID
        {
            get { return this.type; }

            set
            {
                this.type = value;
                if (!Env.DesignTime)
                {
                    string selectCommand = string.Format(
                        @"select ID, rtrim(ID)+'- '+rtrim(Name) as IDName 
                    from dbo.reason WITH (NOLOCK) left join Production.dbo.System a WITH (NOLOCK) on a.ExchangeID=Reason.ID where ReasonTypeID = '{0}' order by a.ExchangeID desc", this.ReasonTypeID);
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
        }

        public txtFinanceEnReason()
        {
        }
    }
}
