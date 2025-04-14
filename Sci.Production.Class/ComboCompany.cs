using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <summary>
    /// ComboCompany
    /// </summary>
    public partial class ComboCompany : Win.UI.ComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboCompany"/> class.
        /// </summary>
        public ComboCompany()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboCompany"/> class.
        /// </summary>
        /// <param name="container">container</param>
        public ComboCompany(System.ComponentModel.IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
        }

        private bool? isOrderCompany = null;
        private bool? junk = null;
        private bool isAddEmpty = false;

        /// <summary>
        /// 是否為訂單公司別
        /// </summary>
        [Category("Custom Properties")]
        [Description("是否為訂單公司別")]
        public bool? IsOrderCompany
        {
            get
            {
                return this.isOrderCompany;
            }

            set
            {
                this.isOrderCompany = value;
                this.SetSource();
            }
        }

        /// <summary>
        /// isAddEmpty
        /// </summary>
        [Category("Custom Properties")]
        public bool IsAddEmpty
        {
            get
            {
                return this.isAddEmpty;
            }

            set
            {
                this.isAddEmpty = value;
            }
        }

        /// <summary>
        /// Junk
        /// </summary>
        [Category("Custom Properties")]
        public bool? Junk
        {
            get
            {
                return this.junk;
            }

            set
            {
                this.junk = value;
                this.SetSource();
            }
        }

        private void SetSource()
        {
            if (Env.DesignTime)
            {
                return;
            }

            string where = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();
            if (this.isOrderCompany != null)
            {
                where += @" AND IsOrderCompany = @IsOrderCompany";
                listPar.Add(new SqlParameter("@IsOrderCompany", SqlDbType.Bit) { Value = this.IsOrderCompany });
            }

            if (this.junk != null)
            {
                where += @" AND Junk = @Junk";
                listPar.Add(new SqlParameter("@Junk", SqlDbType.Bit) { Value = this.Junk });
            }

            string sqlcmd = $@"
SELECT
    Company.ID
   ,Company.NameEN
FROM Company
WHERE 1 = 1
{where}
ORDER BY ID ASC
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, listPar, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }

            if (this.IsAddEmpty)
            {
                var row = dt.NewRow();
                row["ID"] = 0;
                row["NameEN"] = string.Empty;
                dt.Rows.Add(row);
                dt = dt.AsEnumerable().OrderBy(o => o["ID"]).CopyToDataTable();
            }

            this.DataSource = dt;
            this.DisplayMember = "NameEN";
            this.ValueMember = "ID";

        }
    }
}
