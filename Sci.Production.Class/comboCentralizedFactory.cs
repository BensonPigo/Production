using System.ComponentModel;
using System.Data;
using System.Configuration;

namespace Sci.Production.Class
{
    /// <summary>
    /// ComboFactory
    /// </summary>
    public partial class ComboCentralizedFactory : Win.UI.ComboBox
    {
        /// <summary>
        /// ComboFactory
        /// </summary>
        public ComboCentralizedFactory()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// ComboFactory
        /// </summary>
        /// <param name="container">container</param>
        public ComboCentralizedFactory(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// Set Defalut Index
        /// </summary>
        /// <param name="defalutValue">Defalut Value</param>
        /// <param name="ftygroup">Fty group</param>
        public void SetDefalutIndex(string defalutValue = null, bool ftygroup = false)
        {
            DataTable dtFty = new DataTable();
            DataRow dr;
            dtFty.Columns.Add("Factory", typeof(string));
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            if (ftygroup)
            {
                strSevers = ConfigurationManager.AppSettings["ServerMatchFtyGeoup"].Split(new char[] { ';' });
            }

            foreach (string strServer in strSevers)
            {
                string[] factorys = strServer.Split(new char[] { ':', ',' });
                for (int i = 1; i < factorys.Length; i++)
                {
                    dr = dtFty.NewRow();
                    dr["Factory"] = factorys[i];
                    dtFty.Rows.Add(dr);
                }
            }

            // 第一筆加入空白
            dr = dtFty.NewRow();
            dr["Factory"] = string.Empty;
            dtFty.Rows.Add(dr);
            dtFty.DefaultView.Sort = "Factory";
            this.DataSource = dtFty;
            this.ValueMember = "Factory";
            this.DisplayMember = "Factory";
            this.SelectedValue = (defalutValue == null) ? Env.User.Factory : defalutValue;
        }
    }
}
