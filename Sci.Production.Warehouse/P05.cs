using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P05 : Sci.Win.Tems.QueryForm
    {
        private string para_Wkno;
        private string para_Spno;
        private string para_Seq1;
        private string para_Seq2;

        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorTextColumnSettings settingID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings settingPOID = new DataGridViewGeneratorTextColumnSettings();

            #region 雙擊事件

            settingID.CellMouseDoubleClick += (s, e) =>
            {
                // 判斷使用權限
               string canUse = MyUtility.GetValue.Lookup($@"
SELECT DISTINCT 1 FROM Pass1 p1 
INNER JOIN PAss2 p2 ON p1.FKPass0=p2.FKPass0  AND p2.MenuName='Warehouse' AND ( (p2.BarPrompt='P02. Import schedule' AND p2.Used='Y') or (p1.IsAdmin=1 or p1.IsMis =1) )
WHERE p1.ID ='{Sci.Env.User.UserID}' 
");
               if (MyUtility.Check.Empty(canUse))
                {
                    return;
                }

               DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);

               Sci.Production.Warehouse.P02 callP02 = null;

                // 避免重複開啟視窗
               foreach (Form form in Application.OpenForms)
                {
                    if (form is Sci.Production.Warehouse.P02)
                    {
                        form.Activate();
                        Sci.Production.Warehouse.P02 activateForm = (Sci.Production.Warehouse.P02)form;

                        // activateForm.setTxtSPNo(CurrentMaintain["ID"].ToString());
                        // activateForm.Query();
                        return;
                    }
                }

               ToolStripMenuItem P02MenuItem = null;

               foreach (ToolStripMenuItem toolMenuItem in Sci.Env.App.MainMenuStrip.Items)
                {
                    if (toolMenuItem.Text.EqualString("Warehouse"))
                    {
                        foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                        {
                            if (subMenuItem.GetType().Equals(typeof(System.Windows.Forms.ToolStripMenuItem)))
                            {
                                if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P02. Import schedule"))
                                {
                                    P02MenuItem = (ToolStripMenuItem)subMenuItem;
                                    break;
                                }
                            }
                        }
                    }
                }

               callP02 = new P02(dr["ID"].ToString(), null, P02MenuItem);
               callP02.MdiParent = this.MdiParent;
               callP02.Show();
            };

            settingPOID.CellMouseDoubleClick += (s, e) =>
            {
                // 判斷使用權限
                string canUse = MyUtility.GetValue.Lookup($@"
SELECT DISTINCT 1 FROM Pass1 p1 
INNER JOIN PAss2 p2 ON p1.FKPass0=p2.FKPass0  AND p2.MenuName='Warehouse' AND ( (p2.BarPrompt='P02. Import schedule' AND p2.Used='Y') or (p1.IsAdmin=1 or p1.IsMis =1) )
WHERE p1.ID ='{Sci.Env.User.UserID}' 
");
                if (MyUtility.Check.Empty(canUse))
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);

                Sci.Production.Warehouse.P02 callP02 = null;

                // 避免重複開啟視窗
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Sci.Production.Warehouse.P02)
                    {
                        form.Activate();
                        Sci.Production.Warehouse.P02 activateForm = (Sci.Production.Warehouse.P02)form;

                        // activateForm.setTxtSPNo(CurrentMaintain["ID"].ToString());
                        // activateForm.Query();
                        return;
                    }
                }

                ToolStripMenuItem P02MenuItem = null;

                foreach (ToolStripMenuItem toolMenuItem in Sci.Env.App.MainMenuStrip.Items)
                {
                    if (toolMenuItem.Text.EqualString("Warehouse"))
                    {
                        foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                        {
                            if (subMenuItem.GetType().Equals(typeof(System.Windows.Forms.ToolStripMenuItem)))
                            {
                                if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P02. Import schedule"))
                                {
                                    P02MenuItem = (ToolStripMenuItem)subMenuItem;
                                    break;
                                }
                            }
                        }
                    }
                }

                callP02 = new P02(null, dr["POID"].ToString(), P02MenuItem);
                callP02.MdiParent = this.MdiParent;
                callP02.Show();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("ID", header: "WK#", width: Widths.AnsiChars(11), iseditingreadonly: true, settings: settingID)
            .Text("MDivisionID", header: "M", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: settingPOID)
            .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Numeric("BalanceQty", header: "Balance Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Text("StockType", header: "Stock Type", width: Widths.AnsiChars(7), iseditingreadonly: true)
            ;

            for (int i = 0; i <= this.grid.Columns.Count - 1; i++)
            {
                this.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtWkNo.Text))
            {
                this.para_Wkno = $"AND ed.ID = '{this.txtWkNo.Text}'";
            }
            else
            {
                this.para_Wkno = string.Empty;
            }

            if (!MyUtility.Check.Empty(this.txtSpNo.Text))
            {
                this.para_Spno = $"AND ed.POID = '{this.txtSpNo.Text}'";
            }
            else
            {
                this.para_Spno = string.Empty;
            }

            if (!MyUtility.Check.Empty(this.txtSeq1.Text))
            {
                this.para_Seq1 = $"AND ed.seq1 = '{this.txtSeq1.Text}'";
            }
            else
            {
                this.para_Seq1 = string.Empty;
            }

            if (!MyUtility.Check.Empty(this.txtSeq2.Text))
            {
                this.para_Seq2 = $"AND ed.seq2 = '{this.txtSeq2.Text}'";
            }
            else
            {
                this.para_Seq2 = string.Empty;
            }

            this.Query();
        }

        private void Query()
        {
            DataTable dt;
            string sqlCmd = string.Empty;

            sqlCmd += $@"
SELECT 
    ed.ID
    ,ed.PoID
    ,ed.Seq1
    ,ed.Seq2
    ,f.Roll
    ,f.Dyelot
    ,[BalanceQty]=f.InQty-f.OutQty+ f.AdjustQty
    ,[Location]=dbo.Getlocation(f.ukey)
    ,fc.MDivisionID
    ,e.FactoryID
	,[StockType]=dd.Name
FROM Export_Detail ed
INNER JOIN FtyInventory f   ON f.POID= ed.POID
                            AND f.SEQ1= ed.SEQ1
                            AND f.SEQ2= ed.SEQ2
                            AND f.InQty- f.OutQty+ f.AdjustQty>0
INNER JOIN Export e ON e.ID = ed.ID
LEFT JOIN Factory fc ON fc.ID = e.FactoryID
LEFT JOIN DropDownList dd ON dd.ID LIKE '%'+f.stockType+'%'  AND dd.Type='Pms_StockType'
WHERE NOT EXISTS (SELECT 1 FROM Orders o WHERE o.POID=ed.PoID)
AND ed.PoID!='' AND Len(ed.poid) <13
{this.para_Wkno}
{this.para_Spno}
{this.para_Seq1}
{this.para_Seq2}

ORDER BY ed.id DESC, ed.poid , ed.seq1, ed.seq2, f.Roll, f.Dyelot
";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (result)
            {
                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data Not Found.");
                    this.listControlBindingSource1.DataSource = null;
                    return;
                }

                this.listControlBindingSource1.DataSource = dt;
            }
            else
            {
                this.ShowErr(result);
                return;
            }
        }
    }
}
