using System.ComponentModel;
using System.Data;
using Sci.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtshipmode
    /// </summary>
    public partial class Txtshipmode : Win.UI.ComboBox
    {
        private string useFunction;

        /// <summary>
        /// ShipMode.UseFunction
        /// </summary>
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
                    DataTable shipModeTable = new DataTable();
                    if (cbResult = DBProxy.Current.Select(null, sqlCMD, out shipModeTable))
                    {
                        this.DataSource = shipModeTable;
                        this.DisplayMember = "ID";
                        this.ValueMember = "ID";
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Txtshipmode"/> class.
        /// </summary>
        public Txtshipmode()
        {
            this.Size = new System.Drawing.Size(80, 24);
        }
    }
}
