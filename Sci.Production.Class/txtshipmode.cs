using System.ComponentModel;
using System.Data;
using Sci.Data;

namespace Sci.Production.Class
{
    public partial class txtshipmode : Win.UI.ComboBox
    {
        private string useFunction;

        [Category("Custom Properties")]
        public string UseFunction
        {
            get
            {
                return this.useFunction;
            }

            set
            {
                this.useFunction = value;
                if (!Env.DesignTime)
                {
                    string sqlCMD;
                    if (MyUtility.Check.Empty(this.useFunction))
                    {
                        sqlCMD = "select ID='',UseFunction='' union all select ID,UseFunction from ShipMode WITH (NOLOCK)";
                    }
                    else
                    {
                        sqlCMD = string.Format("select ID='',UseFunction='' union all select ID,UseFunction from ShipMode WITH (NOLOCK) where UseFunction like '%{0}%'", this.useFunction.Trim());
                    }

                    Ict.DualResult cbResult;
                    DataTable ShipModeTable = new DataTable();
                    if (cbResult = DBProxy.Current.Select(null, sqlCMD, out ShipModeTable))
                    {
                        this.DataSource = ShipModeTable;
                        this.DisplayMember = "ID";
                        this.ValueMember = "ID";
                    }
                }
            }
        }

        public txtshipmode()
        {
            this.Size = new System.Drawing.Size(80, 24);
        }
    }
}
