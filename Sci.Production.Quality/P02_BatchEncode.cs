using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P02_BatchEncode : Sci.Win.Tems.QueryForm
    {
        private string masterID;
        public P02_BatchEncode(string MasterID)
        {
            InitializeComponent();
            this.masterID = MasterID;
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("", "");
            comboBox1_RowSource.Add("Approval", "Approval");
            comboBox1_RowSource.Add("N/A", "N/A");
            comboBox1_RowSource.Add("Pass", "Pass");
            comboBox1_RowSource.Add("Fail", "Fail");
            comboResult.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboResult.ValueMember = "Key";
            comboResult.DisplayMember = "Value";

            this.dateInspectDt.Value = DateTime.Now;
            this.txtInspector.TextBox1Binding = Env.User.UserID;
            this.EditMode = true;
            this.grid.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            
            #region set Grid

            Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("select",trueValue: 1, falseValue: 0)
                .Text("SEQ", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: false)
                .Text("ExportID", header: "WKNO", width: Widths.AnsiChars(13), iseditingreadonly: false)
                .Date("whseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(26), iseditingreadonly: false)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(20), iseditingreadonly: false)
                .Text("SuppEn", header: "Supplier", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: false)
                .Text("Size", header: "Size", width: Widths.AnsiChars(15), iseditingreadonly: false)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(8), integer_places: 11, decimal_places: 2, iseditingreadonly: false)
                .Numeric("InspQty", header: "Inspected Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Inspdate", header: "Insp. Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Inspector2", header: "Inspector", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("unit", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Date("InspDeadline", header: "Insp. Deadline", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Text("ReplacementID", header: "1st ReplacementID", width: Widths.AnsiChars(15), iseditingreadonly: false)
                .Text("ReceivingID", header: "Receiving ID", width: Widths.AnsiChars(15), iseditingreadonly: false);
            #endregion

            this.QueryData();
        }

        private void QueryData()
        {
            DataTable encodeData;
                
            string sqlCmd = $@"
Select [select] = 0,a.id,a.poid,SEQ1,SEQ2,Receivingid,Refno,SCIRefno,Suppid,C.exportid,
                ArriveQty,InspDeadline,a.ReplacementReportID,
                a.Status,ReplacementReportID,(seq1+seq2) as seq,
                (
                    Select weavetypeid from Fabric b WITH (NOLOCK) where b.SCIRefno =a.SCIrefno
                ) as weavetypeid,
                c.ID AS ReceivingID,c.whseArrival,
                (
                    dbo.GetColorMultipleID((select top 1 o.BrandID from orders o where o.POID =a.poid) ,(Select d.colorid from PO_Supp_Detail d WITH (NOLOCK) Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2))
                ) as Colorid,
                (
                    Select d.SizeSpec from PO_Supp_Detail d WITH (NOLOCK) Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
                ) as Size,
                (
                    Select d.StockUnit from PO_Supp_Detail d WITH (NOLOCK) Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
                ) as unit,
                (
                    Select AbbEn From Supp WITH (NOLOCK) Where a.suppid = supp.id
                ) as SuppEn,InspQty,Inspdate,(
				    select Pass1.Name from Pass1 WITH (NOLOCK) where a.Inspector = pass1.id
				) AS Inspector2,Inspector,Result
	                From AIR a WITH (NOLOCK) 
                    Left join Receiving c WITH (NOLOCK) on c.id = a.receivingid
                Where a.poid='{this.masterID}' --and a.Status <> 'Confirmed' 
                order by seq1,seq2
";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out encodeData);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.grid.DataSource != null)
            {
                var srcDt = ((DataTable)this.grid.DataSource).AsEnumerable().Where(s => (int)s["select"] == 1);
                foreach (DataRow dr in encodeData.Rows)
                {
                    if (srcDt.Where(s => s["ID"].Equals(dr["ID"]) && s["ReceivingID"].Equals(dr["ReceivingID"])).Any())
                    {
                        dr["select"] = 1;
                    }
                }
            }
            
            this.grid.DataSource = encodeData;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.numInspectRate.Value))
            {
                MyUtility.Msg.WarningBox("<Inspected %> can not be empty or 0");
                return;
            }

            if (MyUtility.Check.Empty(this.dateInspectDt.Value))
            {
                MyUtility.Msg.WarningBox("<Inspect Date> can not be empty");
                return;
            }

            if (MyUtility.Check.Empty(this.txtInspector.TextBox1.Text))
            {
                MyUtility.Msg.WarningBox("<Inspector> can not be empty");
                return;
            }

            if (MyUtility.Check.Empty(this.comboResult.Text))
            {
                MyUtility.Msg.WarningBox("<Result> can not be empty");
                return;
            }
            #endregion

            DataTable gridDt = (DataTable)this.grid.DataSource;
            var selectedData = gridDt.AsEnumerable().Where(s => (int)s["select"] == 1);
            if (!selectedData.Any())
            {
                MyUtility.Msg.WarningBox("Please select at least one");
                return;
            }

            string updSQL = string.Empty;
            foreach (var item in selectedData)
            {
                int InspQty = (int)Math.Ceiling(((decimal)item["ArriveQty"]) * this.numInspectRate.Value / 100);
                string Inspdate = this.dateInspectDt.Text;
                string Inspector = this.txtInspector.TextBox1.Text;
                string Result = this.comboResult.Text;
                string remark = this.txtRemark.Text;
                updSQL += $" update AIR set InspQty= {InspQty},Inspdate = '{Inspdate}',Inspector = '{Inspector}',Result = '{Result}',remark = '{remark}'  where ID = '{item["ID"]}'" + Environment.NewLine;
            }

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSQL)))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Update Fail!!");
                        return;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");

                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            this.QueryData();
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            DataTable gridDt = (DataTable)this.grid.DataSource;

            DualResult chkresult;
            DataTable dt;

            var selectedData = gridDt.AsEnumerable().Where(s => (int)s["select"] == 1);

            if (!selectedData.Any())
            {
                MyUtility.Msg.WarningBox("Please select at least one");
                return;
            }

            // 檢查是否可做encode
            var checkResult = selectedData
                                .Where(s => MyUtility.Check.Empty(s["InspQty"]) ||
                                            MyUtility.Check.Empty(s["Result"]) ||
                                            MyUtility.Check.Empty(s["Inspdate"]) ||
                                            MyUtility.Check.Empty(s["Inspector"]));
            if (checkResult.Count() > 0)
            {
                MyUtility.Msg.WarningBox("<Inspected Qty>,<Result>,<Inspdate>,<Inspector> can not be null");
                return;
            }
            
            checkResult = selectedData
                                .Where(s => s["Result"].Equals("Approval"));
            if (checkResult.Count() > 0)
            {
                MyUtility.Msg.WarningBox("<Result> Can not be Approval.");
                return;
            }

            string strInspAutoLockAcc = MyUtility.GetValue.Lookup("SELECT InspAutoLockAcc FROM System");

            string updSQL = string.Empty;
            foreach (var item in selectedData)
            {
                updSQL += $" update AIR set Status = 'Confirmed',EditDate= GETDATE()  where ID = '{item["ID"]}'" + Environment.NewLine;

                if (MyUtility.Convert.GetBool(strInspAutoLockAcc))
                {
                    switch (this.comboResult.Text.ToString())
                    {
                        case "Fail":
                            updSQL += Environment.NewLine + $@"
UPDATE f SET 
Lock = 1 , LockName='{Sci.Env.User.UserID}' ,LockDate=GETDATE(), F.Remark='Auto Lock by QA_P02.Accessory Inspection'
FROM FtyInventory f 
INNER JOIN Receiving_Detail rd ON rd.PoId=f.POID AND rd.Seq1=f.seq1 AND rd.seq2=f.Seq2 AND rd.StockType=f.StockType 
WHERE f.POID='{item["POID"].ToString().Trim()}' AND f.Seq1='{item["Seq1"].ToString().Trim()}' AND f.Seq2='{item["Seq2"].ToString().Trim()}'";
                            break;

                        case "Pass":

                            chkresult = DBProxy.Current.Select(null, $@"
SELECT DISTINCT  Result
FROM AIR
WHERE POID='{item["POID"].ToString().Trim()}' AND Seq1='{item["Seq1"].ToString().Trim()} ' AND Seq2='{item["Seq2"].ToString().Trim()}'
AND ID<>'{item["ID"].ToString().Trim()}' AND ReceivingID<>'{item["ReceivingID"].ToString().Trim()}'
", out dt);
                            if (!chkresult)
                            {
                                ShowErr("Commit transaction error.", chkresult);
                                return;
                            }

                            bool isAllPass = false;


                            //=1表示有相同POID Seq 1 2，且Result只有Pass一種結果
                            if (dt.Rows.Count == 1)
                            {
                                if (dt.Rows[0]["Result"].ToString() == "Pass")
                                {
                                    isAllPass = true;
                                }
                            }
                            //表示無相同POID Seq 1 2
                            if (dt.Rows.Count == 0)
                            {
                                isAllPass = true;
                            }

                            if (isAllPass)
                            {
                                updSQL += Environment.NewLine + $@"
UPDATE f SET 
Lock = 0 , LockName='{Sci.Env.User.UserID}' ,LockDate=GETDATE(), F.Remark='Auto unLock by QA_P02.Accessory Inspection'
FROM FtyInventory f 
INNER JOIN Receiving_Detail rd ON rd.PoId=f.POID AND rd.Seq1=f.seq1 AND rd.seq2=f.Seq2 AND rd.StockType=f.StockType 
WHERE f.POID='{item["POID"].ToString().Trim()}' AND f.Seq1='{item["Seq1"].ToString().Trim()}' AND f.Seq2='{item["Seq2"].ToString().Trim()}'";
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSQL)))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Update Fail!!");
                        return;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");

                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            //更新PO.FIRInspPercent和AIRInspPercent
            if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'AIR','{masterID}';")))
            {
                ShowErr(upResult);
            }


            //ISP20200575 Encode全部執行後
            string sqlcmd = $@"select distinct orderid=o.ID from Orders o with(nolock) inner join #tmp t on t.POID = o.POID";
            DataTable dtid = selectedData.CopyToDataTable();
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtid, "poid", sqlcmd, out dtid);
            if (!result)
            {
                this.ShowErr(result);
            }
            else
            {
                string sqlup = $@"
update a 
set Status = iif(dbo.GetAirQaRecord(t.orderid) ='PASS','Preparing',a.Status)
from #tmp t
inner join AccessoryOrderList a with(nolock) on a.OrderID = t.orderid and a.Status = 'Waiting'
";
                SqlConnection sqlConn = null;
                DBProxy.Current.OpenConnection("ManufacturingExecution", out sqlConn);
                result = MyUtility.Tool.ProcessWithDatatable(dtid, string.Empty, sqlup, out dtid, "#tmp", sqlConn);
                if (!result)
                {
                    this.ShowErr(result);
                }
            }

            this.QueryData();
        }

        private void numInspectRate_ValueChanged(object sender, EventArgs e)
        {
            this.numInspectRate.Text = this.numInspectRate.Value.ToString() + "%";
        }
    }
}
