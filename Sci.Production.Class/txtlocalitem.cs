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
using Ict.Win;
using Sci.Win.Tools;


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
                            MyUtility.Msg.WarningBox(string.Format("< Local item> : {0} is Junk!!!", str));
                            this.Text = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Local item> : {0} not found!!!", str));
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
            this.Width = 150;
        }
    }
    public class celllocalitem : DataGridViewGeneratorTextColumnSettings
    {
        public static DataGridViewGeneratorTextColumnSettings GetGridCell(string category,string localSupp=null)
        {
            //pur 為ture 表示需判斷PurchaseFrom
            celllocalitem ts = new celllocalitem();
            string where = "Where junk=0 ";
            if (category != null)
            {
                if (!string.IsNullOrWhiteSpace(category))
                {
                    if (category.ToUpper() == "THREAD")
                    {
                        where = where + string.Format(" and Category like '%{0}%'", category);
                    }
                    else
                    {
                        where = where + string.Format(" and Category = '{0}'", category);
                    }
                }
            }
            if (localSupp != null)
            {
                if (!string.IsNullOrWhiteSpace(localSupp))
                {
                    where = where + string.Format(" and Localsuppid = '{0}'", localSupp);
                }

            }
            ts.EditingMouseDown += (s, e) =>
            {
                // 右鍵彈出功能
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    // Parent form 若是非編輯狀態就 return 
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele;

                    sele = new SelectItem("Select Refno, LocalSuppid, category, description From LocalItem "+where, "23", row["refno"].ToString(), false, ",");

                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }


            };
            // 正確性檢查
            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                // Parent form 若是非編輯狀態就 return 
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                // 右鍵彈出功能
                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                String oldValue = row["refno"].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                string sql;

                sql = string.Format("Select Refno, LocalSuppid, category, description From LocalItem "+where+" and refno ='{0}'", newValue);
                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(sql))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Local item> : {0} not found!!!", newValue));
                        row["refno"] = "";
                        row.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }

            };
            return ts;
        }

    }
}