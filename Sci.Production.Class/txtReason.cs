using System.ComponentModel;
using System.Data;
using Sci.Data;

namespace Sci.Production.Class
{
    public partial class txtReason : Sci.Win.UI.ComboBox
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
                        @"select '' ID,'ALL' IDName,-1 as no UNION ALL select ID, rtrim(ID)+'- '+rtrim(Name) as IDName ,No
                    from dbo.reason WITH (NOLOCK) where ReasonTypeID = '{0}' order by no", this.ReasonTypeID);
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
        }

        public txtReason()
        {
        }
    }
}
