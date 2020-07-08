using System.ComponentModel;
using System.Data;
using System.Configuration;

namespace Sci.Production.Class
{
    /// <summary>
    /// ComboFactory
    /// </summary>
    public partial class comboCentralizedM : Sci.Win.UI.ComboBox
    {
        /// <summary>
        /// ComboFactory
        /// </summary>
        public comboCentralizedM()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// ComboFactory
        /// </summary>
        /// <param name="container">container</param>
        public comboCentralizedM(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// SetDefalutIndex
        /// </summary>
        /// <param name="defalutValue">defalutValue</param>
        public void SetDefalutIndex(string defalutValue = null)
        {
           DataTable dtM = new DataTable();
           DataRow dr;
           dtM.Columns.Add("M", typeof(string));
           string[] strSevers = ConfigurationManager.AppSettings["ServerMatchMdvision"].Split(new char[] { ';' });
           foreach (string strServer in strSevers)
            {
                string[] MDvisions = strServer.Split(new char[] { ':', ',' });
                for (int i = 1; i < MDvisions.Length; i++)
                {
                    dr = dtM.NewRow();
                    dr["M"] = MDvisions[i];
                    dtM.Rows.Add(dr);
                }
            }

            // 第一筆加入空白
           dr = dtM.NewRow();
           dr["M"] = string.Empty;
           dtM.Rows.Add(dr);
           dtM.DefaultView.Sort = "M";
           this.DataSource = dtM;
           this.ValueMember = "M";
           this.DisplayMember = "M";

           this.SelectedValue = (defalutValue == null) ? Sci.Env.User.Factory : defalutValue;
        }
    }
}
