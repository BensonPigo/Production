using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
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
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorTextColumnSettings settingID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings settingPOID = new DataGridViewGeneratorTextColumnSettings();


            #region 雙擊事件
           
            settingID.CellMouseDoubleClick += (s, e) =>
            {
                //判斷使用權限
               string canUse= MyUtility.GetValue.Lookup($@"
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

                //避免重複開啟視窗
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Sci.Production.Warehouse.P02)
                    {
                        form.Activate();
                        Sci.Production.Warehouse.P02 activateForm = (Sci.Production.Warehouse.P02)form;
                        //activateForm.setTxtSPNo(CurrentMaintain["ID"].ToString());
                        //activateForm.Query();
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
                                    P02MenuItem = ((ToolStripMenuItem)subMenuItem);
                                    break;
                                }
                            }
                        }
                    }
                }


                callP02 = new P02(dr["ID"].ToString(), null, P02MenuItem);
                callP02.MdiParent = MdiParent;
                callP02.Show();
            };

            settingPOID.CellMouseDoubleClick += (s, e) =>
            {                
                //判斷使用權限
                string canUse = MyUtility.GetValue.Lookup($@"
SELECT DISTINCT 1 FROM Pass1 p1 
INNER JOIN PAss2 p2 ON p1.FKPass0=p2.FKPass0  AND p2.MenuName='Warehouse' AND ( (p2.BarPrompt='P02. Import schedule' AND p2.Used='Y') or (p1.IsAdmin=1 or p1.IsMis =1) )
WHERE p1.ID ='{Sci.Env.User.UserID}' 
");
                if (MyUtility.Check.Empty(canUse))
                {
                    return;
                }
                DataRow dr = grid.GetDataRow<DataRow>(e.RowIndex);

                Sci.Production.Warehouse.P02 callP02 = null;

                //避免重複開啟視窗
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Sci.Production.Warehouse.P02)
                    {
                        form.Activate();
                        Sci.Production.Warehouse.P02 activateForm = (Sci.Production.Warehouse.P02)form;
                        //activateForm.setTxtSPNo(CurrentMaintain["ID"].ToString());
                        //activateForm.Query();
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
                                    P02MenuItem = ((ToolStripMenuItem)subMenuItem);
                                    break;
                                }
                            }
                        }
                    }
                }


                callP02 = new P02(null, dr["POID"].ToString(), P02MenuItem);
                callP02.MdiParent = MdiParent;
                callP02.Show();
            };
            #endregion

            Helper.Controls.Grid.Generator(this.grid)
            .Text("ID", header: "WK#", width: Widths.AnsiChars(11), iseditingreadonly: true, settings:settingID)  //0
            .Text("POID", header: "SP#", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: settingPOID)
            .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(9),iseditingreadonly:true)
            .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(7), iseditingreadonly: true)    //3
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(7), iseditingreadonly: true)    //4
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Numeric("BalanceQty", header: "Balance Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
            ;

            for (int i = 0; i <= this.grid.Columns.Count-1; i++)
            {
                this.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            
        }


        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtWkNo.Text))
                para_Wkno = $"AND ed.ID = '{this.txtWkNo.Text}'";
            else
                para_Wkno = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSpNo.Text))
                para_Spno = $"AND ed.POID = '{this.txtSpNo.Text}'";
            else
                para_Spno = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSeq1.Text))
                para_Seq1 = $"AND ed.seq1 = '{this.txtSeq1.Text}'";
            else
                para_Seq1 = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSeq2.Text))
                para_Seq2 = $"AND ed.seq2 = '{this.txtSeq2.Text}'";
            else
                para_Seq2 = string.Empty;

            Query();

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

FROM Export_Detail ed
INNER JOIN FtyInventory f   ON f.POID= ed.POID
                            AND f.SEQ1= ed.SEQ1
                            AND f.SEQ2= ed.SEQ2
                            AND f.InQty- f.OutQty+ f.AdjustQty>0
WHERE NOT EXISTS (SELECT 1 FROM Orders o WHERE o.POID=ed.PoID)
AND ed.PoID!='' AND Len(ed.poid) <13
{para_Wkno}
{para_Spno}
{para_Seq1}
{para_Seq2}

ORDER BY ed.id DESC, ed.poid , ed.seq1, ed.seq2, f.Roll, f.Dyelot
";

           DualResult result= DBProxy.Current.Select(null, sqlCmd, out dt);
            if (result)
            {
                if (dt.Rows.Count==0)
                {
                    MyUtility.Msg.WarningBox("Data Not Found.");
                    this.listControlBindingSource1.DataSource = null;
                    return;
                }
                this.listControlBindingSource1.DataSource = dt;
            }
            else
            {
                ShowErr(result);
                return;
            }
        }
    }
}
