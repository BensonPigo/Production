using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System.Linq;
using System.Data.SqlClient;
using Sci.Production.PublicPrg;

namespace Sci.Production.Warehouse
{
    public partial class P08_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        public P08_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.EditMode = true;


            #region Location 右鍵開窗

            Ict.Win.DataGridViewGeneratorTextColumnSettings Location_Setting = new DataGridViewGeneratorTextColumnSettings();
            Location_Setting.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentRow = grid.GetDataRow<DataRow>(e.RowIndex);
                    string currentLocation = currentRow["Location"].ToString();
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation("B", currentLocation);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    currentRow["Location"] = item.GetSelectedString();
                }
            };

            Location_Setting.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow currentRow = grid.GetDataRow<DataRow>(e.RowIndex);
                    currentRow["location"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", currentRow["StockType"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = currentRow["location"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!(location.EqualString("")))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                    }
                    trueLocation.Sort();
                    currentRow["location"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                    currentRow.EndEdit();
                }
            };
            #endregion Location 右鍵開窗

            Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Roll;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Dyelot;

            this.grid.IsEditingReadOnly = false;

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  
                .Text("Refno", header: "Ref#", width: Widths.AnsiChars(9), iseditingreadonly: true)   
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) 
                .Text("ColorID", header: "Color", iseditingreadonly: true)
                .Text("SizeSpec", header: "Size", iseditingreadonly: true)
                .Date("FinalETA", header: "ETA", iseditingreadonly: true)
                .Numeric("InQty", header: "In Qty", width: Widths.AnsiChars(11), decimal_places: 0, integer_places: 10, iseditingreadonly: true)
                .Text("StockUnit", header: "Stock Unit", iseditingreadonly: true)
                .Text("Roll", header: "Roll",  iseditingreadonly: false).Get(out cbb_Roll)
                .Text("Dyelot", header: "Dyelot",  iseditingreadonly: false).Get(out cbb_Dyelot)
                .Numeric("StockQty", header: "Receiving Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)
                .Text("Location", header: "Location", settings: Location_Setting, iseditingreadonly: false)
            ;
            cbb_Roll.MaxLength = 8;
            cbb_Dyelot.MaxLength = 8;
            #endregion 欄位設定
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.InfoBox("SP# can't be empty.");
                return;
            }

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@SP", this.txtSP.Text));
            string poid = MyUtility.GetValue.Lookup("SELECT  POID FROM Orders WHERE ID = @SP", paras);
            string Seq1 = this.txtSeq1.Text;
            string Seq2 = this.txtSeq2.Text;

            // 這邊必須和P08表身一致
            string cmd= $@"
select  [Selected] = 0
        , [ID]=''
        , [POID] = p.id
        , [Seq] = concat(Ltrim(Rtrim(p.seq1)), ' ', p.seq2) 
        , p.Refno   
        , [Description] = dbo.getmtldesc(p.id,p.seq1,p.seq2,2,0)
        , p.ColorID
        , p.SizeSpec 
        , p.FinalETA
        , isnull(m.InQty, 0) as InQty
        , StockUnit = dbo.GetStockUnitBySPSeq (p.id, p.seq1, p.seq2)
        , p.seq1
        , p.seq2
        , p.scirefno
		,[Roll]=''
		,[Dyelot]=''
		,[StockQty]=0
		,[Location]=''
        ,[StockType]='B'
		,[UseQty] = UseQty.Val
from dbo.PO_Supp_Detail p WITH (NOLOCK) 
inner join Orders o on p.id = o.id
inner join Factory f on o.FtyGroup = f.id
left join dbo.mdivisionpodetail m WITH (NOLOCK) on m.poid = p.id and m.seq1 = p.seq1 and m.seq2 = p.seq2
inner join View_unitrate v on v.FROM_U = p.POUnit 
	                          and v.TO_U = dbo.GetStockUnitBySPSeq (p.id, p.seq1, p.seq2)
OUTER APPLY(
	SELECT [LockCount]=COUNT(UKEY)
	FROM FtyInventory
	WHERE POID='{poid}'
	AND Seq1=p.Seq1
	AND Seq2=p.Seq2
	AND Lock = 1
)LockStatus
OUTER APPLY(
	select [Val]=Round(sum(dbo.GetUnitQty(b.POUnit, b.StockUnit, b.Qty)), 2) 
	from PO_Supp_Detail b WITH (NOLOCK) 
	where b.id= p.id and b.seq1 = p.seq1 and b.seq2 = p.seq2
)UseQty

where p.id ='{poid}'
AND left(p.seq1,1) != '7'
and p.Junk = 0
";
            if (!MyUtility.Check.Empty(Seq1))
            {
                cmd += $"AND p.Seq1 = '{Seq1}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(Seq2))
            {
                cmd += $"AND p.Seq2 = '{Seq2}'" + Environment.NewLine;
            }

            DataTable dt;

            DualResult result = DBProxy.Current.Select(null, cmd, out dt);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
            }


            this.listControlBindingSource.DataSource = dt;

        }

        private void BtnImport_Click(object sender, EventArgs e)
        {

            grid.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;


            DataRow[] selectedRows = dtGridBS1.Select("Selected = 1");
            if (selectedRows.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }


            selectedRows = dtGridBS1.Select("StockQty = 0 and Selected = 1");
            if (selectedRows.Length > 0)
            {
                MyUtility.Msg.WarningBox("Receiving Qty of selected row can't be zero!", "Warning");
                return;
            }

            selectedRows = dtGridBS1.Select("StockQty <> 0 and Selected = 1");

            foreach (DataRow tmp in selectedRows)
            {

                DataRow[] findrow = dt_detail.Select($@"POID = '{tmp["POID"].ToString()}'AND Seq1 = '{tmp["Seq1"].ToString()}' AND Seq2 = '{tmp["Seq2"].ToString()}' AND Roll='{tmp["Roll"].ToString()}' AND Dyelot='{tmp["Dyelot"].ToString()}' ");


                if (findrow.Length > 0)
                {
                    findrow[0]["StockQty"] = tmp["StockQty"];
                    findrow[0]["Location"] = tmp["Location"];
                }
                else
                {
                    // 表頭ID
                    tmp["id"] = dr_master["id"];

                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }

            this.Close();

        }
    }
}
