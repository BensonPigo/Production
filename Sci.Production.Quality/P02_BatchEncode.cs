using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P02_BatchEncode : Win.Tems.QueryForm
    {
        private readonly string masterID;
        private string defect;
        private bool bolFilter = true;

        /// <inheritdoc/>
        public P02_BatchEncode(string masterID)
        {
            this.InitializeComponent();
            this.masterID = masterID;
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add(string.Empty, string.Empty);
            comboBox1_RowSource.Add("Approval", "Approval");
            comboBox1_RowSource.Add("N/A", "N/A");
            comboBox1_RowSource.Add("Pass", "Pass");
            comboBox1_RowSource.Add("Fail", "Fail");
            this.comboResult.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboResult.ValueMember = "Key";
            this.comboResult.DisplayMember = "Value";

            this.comboBoxResultTop.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboBoxResultTop.ValueMember = "Key";
            this.comboBoxResultTop.DisplayMember = "Value";

            this.dateInspectDt.Value = DateTime.Now;
            this.txtInspector.TextBox1Binding = Env.User.UserID;
            this.EditMode = true;
            this.grid.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region set Grid

            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("select", trueValue: 1, falseValue: 0)
                .Text("SEQ", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: false)
                .Text("ExportID", header: "WKNO", width: Widths.AnsiChars(15), iseditingreadonly: false)
                .Date("whseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(26), iseditingreadonly: false)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(20), iseditingreadonly: false)
                .Text("SuppEn", header: "Supplier", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: false)
                .Text("Size", header: "Size", width: Widths.AnsiChars(15), iseditingreadonly: false)
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(15), iseditingreadonly: false)
                .Numeric("ArriveQty", header: "Arrive Qty", width: Widths.AnsiChars(8), integer_places: 11, decimal_places: 2, iseditingreadonly: false)
                .Numeric("InspQty", header: "Inspected Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Numeric("RejectQty", header: "Rejected Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .Text("Defect", header: "Defect Type", width: Widths.AnsiChars(15), iseditingreadonly: false)
                .Numeric("Rejected", header: "Rejected %", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
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
            string sqlCmd = $@"
Select [select] = 0,a.id,a.poid,SEQ1,SEQ2,a.ReceivingID,a.Refno,a.SCIRefno,Suppid,C.exportid,
ArriveQty,InspDeadline,a.ReplacementReportID,
a.Status,(seq1+seq2) as seq,
f.WeaveTypeID,
c.whseArrival,
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
, f.MtlTypeID, a.RejectQty
,Defect = DefectDesc.ValList
,[Rejected] = convert(float,IIF(a.InspQty = 0,0, Round(a.RejectQty / a.InspQty * 100,2)))
into #tmp
From AIR a WITH (NOLOCK) 
Left join Receiving c WITH (NOLOCK) on c.id = a.receivingid
left join fabric f on f.SCIRefno = a.SCIRefno
outer apply(
	--select * from AccessoryDefect
	select ValList = Stuff((
		select concat('+',val)
		from (
				select 	distinct
					val = d.ID+'-'+d.Description
				from dbo.AccessoryDefect d
				where exists(select * from SplitString(a.Defect,'+') s where s.Data = d.ID)
			) s
		for xml path ('')
	) , 1, 1, '')
)DefectDesc
Where a.poid = '{this.masterID}' --and a.Status <> 'Confirmed' 


select * from #tmp
order by seq1,seq2

select Refno ='All' 
union all 
select distinct Refno from #tmp

select ExportID ='All' 
union all 
select distinct ExportID from #tmp

drop table #tmp
";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable[] encodeData_List);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.grid.DataSource != null)
            {
                var srcDt = ((DataTable)this.grid.DataSource).AsEnumerable().Where(s => (int)s["select"] == 1);
                foreach (DataRow dr in encodeData_List[0].Rows)
                {
                    if (srcDt.Where(s => s["ID"].Equals(dr["ID"]) && s["ReceivingID"].Equals(dr["ReceivingID"])).Any())
                    {
                        dr["select"] = 1;
                    }
                }
            }

            this.grid.DataSource = encodeData_List[0];

            this.bolFilter = false;
            this.comboBoxRefno.DataSource = encodeData_List[1];
            this.comboBoxRefno.DisplayMember = "Refno";
            this.comboBoxRefno.ValueMember = "Refno";

            this.comboBoxWKNo.DataSource = encodeData_List[2];
            this.comboBoxWKNo.DisplayMember = "ExportID";
            this.comboBoxWKNo.ValueMember = "ExportID";

            this.bolFilter = true;
            this.Grid_Filter();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            #region 檢查必輸欄位
            if (this.radioPanelInspected.Value == "1" && MyUtility.Check.Empty(this.numInspectRate.Value))
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
                int inspQty = this.CalInspectedQty(item);
                string inspdate = this.dateInspectDt.Text;
                string inspector = this.txtInspector.TextBox1.Text;
                string result = this.comboResult.Text;
                string remark = this.txtRemark.Text;
                decimal rejectQty = Math.Ceiling(MyUtility.Convert.GetDecimal(inspQty) * MyUtility.Convert.GetDecimal(this.numRejectPercent.Value) / 100);
                updSQL += $@" 
update AIR 
set InspQty= {inspQty}
,Inspdate = '{inspdate}'
,Inspector = '{inspector}'
,Result = '{result}'
,remark = '{remark}'  
,Defect = '{this.defect}'
,RejectQty = {rejectQty}
where ID = '{item["ID"]}'" + Environment.NewLine;
            }

            DualResult upResult;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                if (!(upResult = DBProxy.Current.Execute(null, updSQL)))
                {
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Update Fail!!");
                    return;
                }

                transactionscope.Complete();
            }

            MyUtility.Msg.InfoBox("Successfully");
            this.QueryData();
        }

        private int CalInspectedQty(DataRow dr)
        {
            int inspQty = 0;
            decimal arriveQty = MyUtility.Convert.GetDecimal(dr["ArriveQty"]);
            string sqlcmd;
            switch (this.radioPanelInspected.Value)
            {
                case "1":
                    return MyUtility.Convert.GetInt(Math.Ceiling(arriveQty * this.numInspectRate.Value / 100));
                case "2":
                    sqlcmd = $@"
select         iif ((select SampleSize
                from AcceptableQualityLevels
                where InspectionLevels = '{this.comboDropDownList1.SelectedValue}'
                and AQLType = 1.5
                and {MyUtility.Math.Round(arriveQty)} between LotSize_Start and LotSize_End) is not null
              , (select SampleSize
                from AcceptableQualityLevels
                where InspectionLevels = '{this.comboDropDownList1.SelectedValue}'
                and AQLType = 1.5
                and {MyUtility.Math.Round(arriveQty)} between LotSize_Start and LotSize_End)
              , (select SampleSize
                from AcceptableQualityLevels
                where InspectionLevels = '{this.comboDropDownList1.SelectedValue}'
                and AQLType = 1.5
                      and {MyUtility.Math.Round(arriveQty)} >= LotSize_Start
                      and LotSize_End = -1))
";
                    return MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlcmd));
                case "3":
                    sqlcmd = $@"
select
	case AOS_InspQtyOption
	when 0 then 0
	when 1 then round({arriveQty} * q.InspectedPercentage / 100, 0)
	when 2 then 
        iif ((select SampleSize
                from AcceptableQualityLevels
                where InspectionLevels = q.AQL_InspectionLevels
                and AQLType = 1.5
                and {MyUtility.Math.Round(arriveQty)} between LotSize_Start and LotSize_End) is not null
              , (select SampleSize
                from AcceptableQualityLevels
                where InspectionLevels = q.AQL_InspectionLevels
                and AQLType = 1.5
                and {MyUtility.Math.Round(arriveQty)} between LotSize_Start and LotSize_End)
              , (select SampleSize
                from AcceptableQualityLevels
                where InspectionLevels = q.AQL_InspectionLevels
                and AQLType = 1.5
                      and {MyUtility.Math.Round(arriveQty)} >= LotSize_Start
                      and LotSize_End = -1))
	end
from QAMtlTypeSetting q
where type = 'A'
and id = '{dr["MtlTypeID"]}'
";
                    return MyUtility.Convert.GetInt(MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlcmd)));
            }

            return inspQty;
        }

        private void BtnEncode_Click(object sender, EventArgs e)
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
Lock = 1 , LockName='{Env.User.UserID}' ,LockDate=GETDATE(), F.Remark='Auto Lock by QA_P02.Accessory Inspection'
FROM FtyInventory f 
WHERE f.POID='{item["POID"].ToString().Trim()}' AND f.Seq1='{item["Seq1"].ToString().Trim()}' AND f.Seq2='{item["Seq2"].ToString().Trim()}'";
                            break;

                        case "Pass":

                            string cmd = $@"
SELECT DISTINCT  Result
FROM AIR
WHERE POID='{item["POID"].ToString().Trim()}' AND Seq1='{item["Seq1"].ToString().Trim()} ' AND Seq2='{item["Seq2"].ToString().Trim()}'
AND ID<>'{item["ID"].ToString().Trim()}' AND ReceivingID<>'{item["ReceivingID"].ToString().Trim()}'
";

                            chkresult = DBProxy.Current.Select(null, cmd, out dt);
                            if (!chkresult)
                            {
                                this.ShowErr("Commit transaction error.", chkresult);
                                return;
                            }

                            bool isAllPass = false;

                            // =1表示有相同POID Seq 1 2，且Result只有Pass一種結果
                            if (dt.Rows.Count == 1)
                            {
                                if (dt.Rows[0]["Result"].ToString() == "Pass")
                                {
                                    isAllPass = true;
                                }
                            }

                            // 表示無相同POID Seq 1 2
                            if (dt.Rows.Count == 0)
                            {
                                isAllPass = true;
                            }

                            if (isAllPass)
                            {
                                updSQL += Environment.NewLine + $@"
UPDATE f SET 
Lock = 0 , LockName='{Env.User.UserID}' ,LockDate=GETDATE(), F.Remark='Auto unLock by QA_P02.Accessory Inspection'
FROM FtyInventory f 
WHERE f.POID='{item["POID"].ToString().Trim()}' AND f.Seq1='{item["Seq1"].ToString().Trim()}' AND f.Seq2='{item["Seq2"].ToString().Trim()}'";
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSQL)))
                    {
                        transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Update Fail!!");
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            // 更新PO.FIRInspPercent和AIRInspPercent
            if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'AIR','{this.masterID}';")))
            {
                this.ShowErr(upResult);
            }

            // ISP20200575 Encode全部執行後
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
set Status = 'Preparing'
from #tmp t
inner join AccessoryOrderList a with(nolock) on a.OrderID = t.orderid and a.Status = 'Waiting'
where dbo.GetAirQaRecord(t.orderid) ='PASS'
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

        private void EditDefect_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sqlcmd = "select id,description from AccessoryDefect WITH (NOLOCK) where Junk = 0 ";
                SelectItem2 item = new SelectItem2(sqlcmd, "Code,Description", "10,30", null, null, null, null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                // 用來存進DB
                this.defect = item.GetSelectedString().Replace(",", "+");
                string strEditDefect = string.Empty;

                for (int i = 0; i < item.GetSelectedList().Count; i++)
                {
                    strEditDefect += item.GetSelecteds()[i]["id"].ToString().TrimEnd() + "-" + item.GetSelecteds()[i]["description"].ToString().TrimEnd() + "+";
                }

                // 單純用來顯示
                this.editDefect.Text = strEditDefect.Substring(0, strEditDefect.Length - 1);
            }
        }

        private void ComboBoxRefno_SelectedValueChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        private void ComboBoxResultTop_SelectedValueChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        private void ComboBoxWKNo_SelectedValueChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        private void Grid_Filter()
        {
            string filter = string.Empty;
            DataTable dt = (DataTable)this.grid.DataSource;
            if (dt == null)
            {
                return;
            }

            if (dt.Rows.Count > 0 && this.bolFilter)
            {
                if (MyUtility.Check.Empty(this.grid))
                {
                    return;
                }

                if (this.comboBoxWKNo.SelectedValue.ToString() != "All")
                {
                    filter += $@" exportid='{this.comboBoxWKNo.SelectedValue.ToString()}' and";
                }

                if (!MyUtility.Check.Empty(this.comboBoxResultTop.SelectedValue.ToString()))
                {
                    filter += $@" Result='{this.comboBoxResultTop.SelectedValue.ToString()}' and";
                }

                if (!MyUtility.Check.Empty(this.comboBoxRefno.SelectedValue.ToString()) && this.comboBoxRefno.SelectedValue.ToString() != "All")
                {
                    filter += $@" Refno='{this.comboBoxRefno.SelectedValue.ToString()}' and";
                }

                if (filter.Length > 0)
                {
                    filter = filter.Substring(0, filter.Length - 3);
                }

                ((DataTable)this.grid.DataSource).DefaultView.RowFilter = filter;
            }
        }
    }
}
