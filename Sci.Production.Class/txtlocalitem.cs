using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtlocalitem
    /// </summary>
    public partial class Txtlocalitem : Win.UI.TextBox
    {
        private string category = string.Empty;
        private string localSupp = string.Empty;
        private string where;

        /// <summary>
        /// Initializes a new instance of the <see cref="Txtlocalitem"/> class.
        /// </summary>
        public Txtlocalitem()
        {
            this.Width = 150;
        }

        /// <summary>
        /// 屬性. 利用字串來設定要存取的<控制項>
        /// </summary>
        [Category("Custom Properties")]
        public Control CategoryObjectName { get; set; }

        /// <summary>
        /// Custom Properties
        /// </summary>
        [Category("Custom Properties")]
        public Control LocalSuppObjectName { get; set; }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            this.where = "Where junk = 0";
            if (this.CategoryObjectName != null)
            {
                this.category = this.CategoryObjectName.Text;
                if (!string.IsNullOrWhiteSpace(this.category))
                {
                    if (this.category.ToUpper() == "THREAD")
                    {
                        this.where = this.where + string.Format(" and Category like '%{0}%'", this.category);
                    }
                    else
                    {
                        this.where = this.where + string.Format(" and Category = '{0}'", this.category);
                    }
                }
            }

            if (this.LocalSuppObjectName != null)
            {
                this.localSupp = this.LocalSuppObjectName.Text;
                if (!string.IsNullOrWhiteSpace(this.localSupp))
                {
                    this.where = this.where + string.Format(" and Localsuppid = '{0}'", this.localSupp);
                }
            }

            string sql = "Select Refno, LocalSuppid, category, description From LocalItem WITH (NOLOCK) " + this.where;
            SelectItem item = new SelectItem(sql, "20,8,20,50", this.Text, false, ",");

            // select id from LocalItem where !Junk and LocalSuppid = localSupp and Category = category
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            this.where = string.Format("Where junk = 0 and Refno = '{0}'", str);
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (this.CategoryObjectName != null)
                {
                    this.category = this.CategoryObjectName.Text;

                    if (!string.IsNullOrWhiteSpace(this.category))
                    {
                        if (this.category.ToUpper() == "THREAD")
                        {
                            this.where = this.where + string.Format(" and Category like '%{0}%'", this.category);
                        }
                        else
                        {
                            this.where = this.where + string.Format(" and Category = '{0}'", this.category);
                        }
                    }
                }

                if (this.LocalSuppObjectName != null)
                {
                    this.localSupp = this.LocalSuppObjectName.Text;
                    if (!string.IsNullOrWhiteSpace(this.localSupp))
                    {
                        this.where = this.where + " and LocalSuppid = @LocalSupp";
                    }
                }

                string sqlcom = "Select Refno,junk from localItem WITH (NOLOCK) " + this.where;
                System.Data.SqlClient.SqlParameter categoryIlist = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@Category",
                    Value = this.category.PadRight(20),
                };

                System.Data.SqlClient.SqlParameter localsuppIlist = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@LocalSupp",
                    Value = this.localSupp.PadRight(8),
                };

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
                        aa = sqlTable.Rows[0]["Junk"].ToString();
                        if (aa == "True")
                        {
                            this.Text = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Local item: {0} >is Junk!!!", str));
                            return;
                        }
                    }
                    else
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Local item: {0}>not found!!!", str));
                        return;
                    }
                }
                else
                {
                    throw new Exception(result.ToString());
                }
            }
        }

        /// <summary>
        /// Celllocalitem
        /// </summary>
        public class Celllocalitem : DataGridViewGeneratorTextColumnSettings
        {
            /// <summary>
            /// SupportPopup
            /// </summary>
            public bool SupportPopup { get; set; } = true;

            /// <summary>
            /// GetGridCell
            /// </summary>
            /// <param name="category">LocalItem Category</param>
            /// <param name="localSupp">LocalItem Localsupp ID</param>
            /// <param name="setColumnname">Column Name</param>
            /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
            public static DataGridViewGeneratorTextColumnSettings GetGridCell(string category, string localSupp = null, string setColumnname = null)
            {
                // pur 為ture 表示需判斷PurchaseFrom
                Celllocalitem ts = new Celllocalitem();
                string where = "Where LI.junk=0 ";
                if (category != null)
                {
                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        if (category.ToUpper() == "THREAD")
                        {
                            where = where + string.Format(" and LI.Category like '%{0}%'", category);
                        }
                        else
                        {
                            where = where + string.Format(" and LI.Category = '{0}'", category);
                        }
                    }
                }

                if (localSupp != null)
                {
                    if (!string.IsNullOrWhiteSpace(localSupp))
                    {
                        where = where + string.Format(" and LI.Localsuppid = '{0}'", localSupp);
                    }
                }

                ts.EditingMouseDown += (s, e) =>
                {
                    // 右鍵彈出功能
                    if (e.Button == MouseButtons.Right)
                    {
                        if (!ts.SupportPopup)
                        {
                            return;
                        }

                        DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                        // Parent form 若是非編輯狀態就 return
                        if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                        {
                            return;
                        }

                        DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                        SelectItem sele;

                        sele = new SelectItem("Select LI.Refno,LI.LocalSuppid, LI.LocalSuppid+'-'+LS.Name as supp, LI.category, LI.description ,LI.ThreadTex ,LI.ThreadTypeID,LI.MeterToCone,LI.Weight,LI.AxleWeight From LocalItem LI WITH (NOLOCK) left join LocalSupp LS WITH (NOLOCK) on LI.LocalSuppid=LS.ID " + where, "23", row["refno"].ToString(), false, ",");
                        DialogResult result = sele.ShowDialog();
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        var sellist = sele.GetSelecteds();
                        if (setColumnname != null)
                        {
                            int count = 1;
                            var columnnamelist = setColumnname.Split(',');
                            foreach (var columnname in columnnamelist)
                            {
                                if (columnname != string.Empty)
                                {
                                    row[columnname] = sellist[0][count];
                                }

                                count++;
                            }

                            // if (Columnname[0] != "") row[Columnname[0]] = sellist[0][1];    //LocalSuppid
                            // if (Columnname[1] != "") row[Columnname[1]] = sellist[0][2];    //supp
                            // if (Columnname[2] != "") row[Columnname[2]] = sellist[0][3];    //category
                            // if (Columnname[3] != "") row[Columnname[3]] = sellist[0][4];    //description
                            // if (Columnname[4] != "") row[Columnname[4]] = sellist[0][5];    //ThreadTex
                            // if (Columnname[5] != "") row[Columnname[5]] = sellist[0][6];    //ThreadTypeID
                            // if (Columnname[6] != "") row[Columnname[6]] = sellist[0][7];    //MeterToCone
                            // if (Columnname[7] != "") row[Columnname[7]] = sellist[0][8];    //Weight
                            // if (Columnname[8] != "") row[Columnname[8]] = sellist[0][9];    //AxleWeight
                        }

                        e.EditingControl.Text = sele.GetSelectedString();       // Refno
                    }
                };

                // 正確性檢查
                ts.CellValidating += (s, e) =>
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    // 右鍵彈出功能
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = row["refno"].ToString();
                    string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                    string sql;

                    sql = string.Format("Select Refno, LocalSuppid, category, description From LocalItem LI WITH (NOLOCK) " + where + " and refno ='{0}'", newValue);
                    if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                    {
                        if (!MyUtility.Check.Seek(sql))
                        {
                            row["refno"] = string.Empty;
                            row.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Local item: {0}> not found.", newValue));
                            return;
                        }
                    }
                };
                return ts;
            }
        }
    }
}