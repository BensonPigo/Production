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

        private bool isOrderCompany = false;
        private bool junk = false;

        /// <summary>
        /// 是否為訂單公司別
        /// </summary>
        [Category("Custom Properties")]
        [Description("是否為訂單公司別")]
        public bool IsOrderCompany
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
        /// Junk
        /// </summary>
        [Category("Custom Properties")]
        public bool Junk
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

            string sqlcmd = $@"
SELECT ID = 0, NameEN = ''
UNION ALL
SELECT
    Company.ID
   ,Company.NameEN
FROM Company
WHERE 1 = 1
AND IsOrderCompany = @IsOrderCompany
AND Junk = @Junk
ORDER BY ID ASC
";
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@IsOrderCompany", SqlDbType.Bit) { Value = this.IsOrderCompany },
                new SqlParameter("@Junk", SqlDbType.Bit) { Value = this.Junk },
            };

            DualResult result = DBProxy.Current.Select(null, sqlcmd, listPar, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }

            this.DataSource = dt;
            this.DisplayMember = "NameEN";
            this.ValueMember = "ID";
        }
    }
}
