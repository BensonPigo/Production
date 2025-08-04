using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using Sci.Production.Class.Commons;
using Sci.Win.Tems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Win
{
    /// <inheritdoc />
    public partial class SubNotification : UserControl
    {
        public string ClassName { get; set; }
        public string Parameter { get; set; }
        public DataTable dt { get; set; }
        private string module;

        /// <inheritdoc />
        public SubNotification()
        {
            this.InitializeComponent();
            new ToolTip().SetToolTip(this.lblCount, "Double click here to show Notification List!");
        }

        /// <summary>
        /// 設定提醒的模組、名稱、筆數
        /// </summary>
        /// <param name="module">模組</param>
        /// <param name="name">提醒名稱</param>
        /// <param name="className">開啟作業ClassName</param>
        /// <param name="parameter">參數</param>
        /// <param name="count">筆數</param>
        public void SetValue(string module, string name, string className, string parameter, string count)
        {
            this.module = module;
            this.lblName.Text = name;
            this.UI_tipName.SetToolTip(this.lblName, this.lblName.Text);
            this.ClassName = className;
            this.Parameter = parameter;
            this.lblCount.Text = count;
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            string id = string.Empty;
            if (this.dt != null && this.dt.Rows.Count > 0)
            {
                string selectColumn = string.Join(",", this.dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName));

                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(this.dt, selectColumn, "15", string.Empty);
                item.Text = this.lblName.Text; // 寫入UserLog的FMCaption
                DialogResult returnResult = item.ShowDialog(this);
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                id = item.GetSelectedString();
            }

            #region 取得MenuItem
            string formName = DBProxy.Current.LookupEx<string>($"select BarPrompt from MenuDetail Where FormName = @ClassName", "ClassName", this.ClassName).ExtendedData;
            var menuControls = this.ParentForm.Controls.Find("menus", true);
            var menuStrip = menuControls.FirstOrDefault() as Sci.Win.UI.MenuStrip;

            ToolStripMenuItem mainMenu = new ToolStripMenuItem();
            ToolStripMenuItem menuItem = new ToolStripMenuItem();

            foreach (var m in menuStrip.Items)
            {
                if (m.GetType() == typeof(ToolStripMenuItem))
                {
                    if (((ToolStripMenuItem)m).Text == this.module)
                    {
                        mainMenu = m as ToolStripMenuItem;
                    }
                }
            }

            foreach (var item in mainMenu.DropDownItems)
            {
                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    if (((ToolStripMenuItem)item).DropDownItems.Count > 0)
                    {
                        foreach (var item2 in ((ToolStripMenuItem)item).DropDownItems)
                        {
                            if (item2.GetType() == typeof(ToolStripMenuItem))
                            {
                                if (((ToolStripMenuItem)item2).Text == formName)
                                {
                                    menuItem = item2 as ToolStripMenuItem;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (((ToolStripMenuItem)item).Text == formName)
                        {
                            menuItem = item as ToolStripMenuItem;
                        }
                    }
                }
            }
            #endregion

            // 打開作業
            object[] arrArg = null;
            arrArg = string.IsNullOrWhiteSpace(this.Parameter) ? new object[1] { menuItem } : new object[2] { menuItem, this.Parameter };

            var dllName = this.ClassName.Substring(0, this.ClassName.ToString().LastIndexOf("."));
            if (dllName == "Sci.Win.UI")
            {
                dllName = "Sci.Proj";
            }
            else if (dllName.StartsWith("Sci.Trade.Report2"))
            {
                dllName = "Sci.Trade.Report2";
            }
            else if (dllName.StartsWith("Sci.Trade.Report"))
            {
                dllName = "Sci.Trade.Report";
            }

            var typeName = this.ClassName + "," + dllName;

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                type = Type.GetType(this.ClassName + ",Sci.Trade.Report2", false);
            }

            if (type != null)
            {
                var frm = (Form)Activator.CreateInstance(type, arrArg);

                Form newf = frm.FindForm();
                Form sameForm = this.ParentForm.MdiChildren.AsEnumerable().Where(f => f.Text.Contains(newf.Text)).FirstOrDefault();

                // 判斷作業是否已開啟,已經開啟的話直接focus到那之作業,並且切換到選擇的ID
                if (sameForm != null)
                {
                    sameForm.Focus();
                    CommonPrg.OpenFormDetail(sameForm, id);
                }
                else
                {
                    frm.MdiParent = this.ParentForm;
                    frm.Show();

                    CommonPrg.OpenFormDetail(frm, id);
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("ClassName setting Error!");
                return;
            }
        }

        // 筆數label雙擊可開啟提醒清單
        private void LblCount_DoubleClick(object sender, EventArgs e)
        {
            string selectColumn = string.Join(",", this.dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName));

            Sci.Win.UI.MsgGridForm form = new Sci.Win.UI.MsgGridForm(this.dt, caption: "Notification List", shownColumns: selectColumn);

            int idx = 0;
            foreach (DataColumn column in this.dt.Columns)
            {
                string columnName = column.ColumnName;

                // 設定欄位寬度
                int columnWidth = 0;
                if (this.dt.AsEnumerable().Where(w => !MyUtility.Check.Empty(w[columnName])).Any())
                {
                    columnWidth = this.dt.AsEnumerable().Where(w => !MyUtility.Check.Empty(w[columnName])).Select(r => r[columnName]).FirstOrDefault().ToString().Length * 11;
                }

                form.grid1.Columns[idx].Width = columnWidth < 60 ? 60 : columnWidth;
                form.grid1.Columns[idx].HeaderText = columnName;
                idx++;
            }

            UIClassPrg.SetGrid_HeaderBorderStyle(form.grid1);
            form.Show(this);
        }
    }
}
