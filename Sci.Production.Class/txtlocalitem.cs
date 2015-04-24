using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Sci;
using Ict;
using Sci.Win;


namespace Sci.Production.Class
{
    public partial class txtlocalitem : Sci.Win.UI.TextBox
    {
        private string category = "";
        private string localSupp = "";
        private Control categoryObject;	//欄位.存入要取值的<控制項>
        private Control localSuppObject;
        private string where;
        // 屬性. 利用字串來設定要存取的<控制項>
        [Category("Custom Properties")]
        public Control CategoryObjectName
        {
            set
            {
                categoryObject = value;
            }
            get
            {
                return categoryObject;
            }
        }

        [Category("Custom Properties")]
        public Control LocalSuppObjectName
        {
            set
            {
                localSuppObject = value;
            }
            get
            {
                return localSuppObject;
            }
        }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            where = "Where junk = 0";
            if (categoryObject != null)
            {
                category = categoryObject.Text;
                if (!string.IsNullOrWhiteSpace(category))
                {
                    where = where + string.Format(" and Category = '{0}'", category);
                }
            }
            if (localSuppObject != null)
            {
                localSupp = localSuppObject.Text;
                if (!string.IsNullOrWhiteSpace(localSupp))
                {
                    where = where + string.Format(" and Localsuppid = '{0}'", localSupp);
                }

            }
            string sql = "Select Refno, LocalSuppid, category, description From LocalItem " + where;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "20,8,20,50", this.Text, false, ",");
            //select id from LocalItem where !Junk and LocalSuppid = localSupp and Category = category
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            where = string.Format("Where junk = 0 and Refno = '{0}'",str);
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (categoryObject != null)
                {
                    category = categoryObject.Text;

                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        where = where + " and Category = @Category ";
                    }
                }
                if (localSuppObject != null)
                {
                    localSupp = localSuppObject.Text;
                    if (!string.IsNullOrWhiteSpace(localSupp))
                    {
                        where = where + " and LocalSuppid = @LocalSupp";
                    }
                }

                string sqlcom = "Select Refno,junk from localItem " + where;
                System.Data.SqlClient.SqlParameter categoryIlist = new System.Data.SqlClient.SqlParameter();
                categoryIlist.ParameterName = "@Category";
                categoryIlist.Value = category.PadRight(20);

                System.Data.SqlClient.SqlParameter localsuppIlist = new System.Data.SqlClient.SqlParameter();
                localsuppIlist.ParameterName = "@LocalSupp";
                localsuppIlist.Value = localSupp.PadRight(8);

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(categoryIlist);
                cmds.Add(localsuppIlist);

                DataTable sqlTable;
                DualResult result;
                if (result = DBProxy.Current.Select(null, sqlcom, cmds, out sqlTable))
                {
                    if (sqlTable.Rows.Count > 0)
                    {
                        string aa;
                        aa = (sqlTable.Rows[0])["Junk"].ToString();
                        if (aa == "True")
                        {
                            MessageBox.Show(string.Format("< Local item> : {0} is Junk!!!", str));
                            this.Text = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("< Local item> : {0} not found!!!", str));
                        this.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    throw new Exception(result.ToString());
                }
            }
        }
        public txtlocalitem()
        {
            this.Width = 108;
        }

    }
}