using Ict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Drawing;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P02_Detail : Win.Subs.Input6A
    {
        #region 改版原因事項
        /*
         使用Inpute6A理由: 可以使用切換上下筆的功能
         *但是Inpute6A需要直接存進DB,只好手刻存檔以及Encode功能
         *Inpute6A原始按鈕save and Undo功能不使用理由:1.更多可調整性,避免被底層綁死
         *注意! P02_Detail WorkAlias沒有特別作用,所有的繫結資料(mtbs - )來源都是上一層的CurrentDetailData(detail GridView)
         */
        #endregion
        private readonly string loginID = Env.User.UserID;
        private readonly bool canedit;
        private string id;
        private string receivingID;
        private string poid;
        private string seq1;
        private string seq2;
        private string defect;

        /// <inheritdoc/>
        public P02_Detail(bool canEdit, string airID, string spNo)
        {
            this.InitializeComponent();
            this.id = airID;
            this.canedit = canEdit;
            this.Btn_status(this.id);
            this.Button_enable(this.canedit);

            string air_cmd = $@"

select MtlTypeID = isnull(f.MtlTypeID,'')
,[DefectDesc] = DefectDesc.ValList
,a.* 
,[% of Inspection] = iif(isnull(a.InspQty,0) = 0 ,0 ,round((a.InspQty / a.ArriveQty)*100 ,2))
from air a WITH (NOLOCK) 
left join Fabric f on a.SCIRefno = f.SCIRefno
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
where a.id='{this.id}'
";
            DataRow dr;
            if (MyUtility.Check.Seek(air_cmd, out dr))
            {
                this.txtSEQ.Text = dr["SEQ1"].ToString() + " - " + dr["SEQ2"].ToString();
                this.seq1 = dr["SEQ1"].ToString();
                this.seq2 = dr["SEQ2"].ToString();
                this.receivingID = dr["ReceivingID"].ToString();
                this.poid = dr["Poid"].ToString();
                this.txtMaterialType.Text = dr["MtlTypeID"].ToString();
                this.defect = dr["Defect"].ToString();
                this.editDefect.Text = dr["DefectDesc"].ToString();
                this.numeric_ofInspection.Value = (decimal)dr["% of Inspection"];
            }
            else
            {
                this.txtSEQ.Text = string.Empty;
                this.receivingID = string.Empty;
                this.seq1 = string.Empty;
                this.seq2 = string.Empty;
                this.poid = string.Empty;
                this.txtMaterialType.Text = string.Empty;
                this.defect = string.Empty;
                this.editDefect.Text = string.Empty;
            }

            this.CalculateReject();
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("Approval", "Approval");
            comboBox1_RowSource.Add("N/A", "N/A");
            comboBox1_RowSource.Add("Pass", "Pass");
            comboBox1_RowSource.Add("Fail", "Fail");
            this.comboResult.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboResult.ValueMember = "Key";
            this.comboResult.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Button_enable(this.canedit);
            this.CalculateReject();
            this.LoadPicture();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnAmend_Click(object sender, EventArgs e)
        {
            string updatesql = string.Empty;
            string updatesql1 = string.Empty;
            DualResult chkresult;
            DataTable dt;

            if (this.btnAmend.Text == "Amend")
            {
                DialogResult btnAmend = MyUtility.Msg.QuestionBox("Are you sure want to <Amend> this data?", "Question", MessageBoxButtons.YesNo);
                if (btnAmend == DialogResult.No)
                {
                    return;
                }
                else
                {
                    #region  寫入實體Table Amend
                    updatesql1 = string.Format(
                    "Update Air set Status = 'New',EditDate=CONVERT(VARCHAR(20), GETDATE(), 120),EditName='{0}' where id ='{1}'",
                    this.loginID, this.id);

                    DualResult upResult1;
                    TransactionScope transactionscope1 = new TransactionScope();
                    using (transactionscope1)
                    {
                        try
                        {
                            if (!(upResult1 = DBProxy.Current.Execute(null, updatesql1)))
                            {
                                transactionscope1.Dispose();

                                return;
                            }

                            transactionscope1.Complete();
                            transactionscope1.Dispose();
                            this.Btn_status(this.id);
                            this.btnEdit.Text = "Edit";
                            this.btnAmend.Text = "Encode";
                            this.btnEdit.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            transactionscope1.Dispose();
                            this.ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }

                    #endregion
                }
            }
            else
            {
                if (MyUtility.Check.Empty(this.txtInspectedQty) || MyUtility.Check.Empty(this.comboResult.Text) || MyUtility.Check.Empty(this.txtInspector) || MyUtility.Check.Empty(this.dateInspectDate))
                {
                    if (MyUtility.Check.Empty(this.txtInspectedQty.Text))
                    {
                        this.txtInspectedQty.Focus();
                        MyUtility.Msg.InfoBox("<Inspected> can not be null");
                        return;
                    }
                    else if (MyUtility.Check.Empty(this.comboResult.SelectedValue))
                    {
                        this.comboResult.Focus();
                        MyUtility.Msg.InfoBox("<Result> can not be null");
                        return;
                    }
                    else if (MyUtility.Check.Empty(this.dateInspectDate))
                    {
                        this.dateInspectDate.Focus();
                        MyUtility.Msg.InfoBox("<Inspdate> can not be null");
                        return;
                    }
                    else if (MyUtility.Check.Empty(this.txtInspector))
                    {
                        this.txtInspector.Focus();
                        MyUtility.Msg.InfoBox("<Inspector> can not be null");
                        return;
                    }
                }
                #region  寫入實體Table Encode
                if (this.comboResult.SelectedValue.ToString() == "Approval")
                {
                    MyUtility.Msg.InfoBox("<Result> Can not be Approval.");
                    return;
                }

                updatesql = string.Format(
                "Update Air set Status = 'Confirmed',EditDate=CONVERT(VARCHAR(20), GETDATE(), 120),EditName='{0}' where id ='{1}'",
                this.loginID, this.id);

                string strInspAutoLockAcc = MyUtility.GetValue.Lookup("SELECT InspAutoLockAcc FROM System");

                if (MyUtility.Convert.GetBool(strInspAutoLockAcc))
                {
                    switch (this.comboResult.Text.ToString())
                    {
                        case "Fail":
                            updatesql += Environment.NewLine + $@"
UPDATE f SET 
Lock = 1 , LockName='{Env.User.UserID}' ,LockDate=GETDATE(), F.Remark='Auto Lock by QA_P02.Accessory Inspection'
FROM FtyInventory f 
WHERE f.POID='{this.poid}' AND f.Seq1='{this.seq1}' AND f.Seq2='{this.seq2}'
and exists(
    select 1 from Receiving_Detail r where r.ID = '{this.receivingID}' 
    and r.Poid = f.POID
    and r.Seq1 = f.Seq1
    and r.Seq2 = f.Seq2
    and r.StockType = f.StockType
)
";
                            break;

                        case "Pass":
                            string cmd = $@"
SELECT DISTINCT  Result
FROM AIR
WHERE POID='{this.poid}' AND Seq1='{this.seq1} ' AND Seq2='{this.seq2}'
AND ID<>'{this.id}' AND ReceivingID<>'{this.receivingID}'
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
                                updatesql += Environment.NewLine + $@"
UPDATE f SET 
Lock = 0 , LockName='{Env.User.UserID}' ,LockDate=GETDATE(), F.Remark='Auto unLock by QA_P02.Accessory Inspection'
FROM FtyInventory f 
WHERE f.POID='{this.poid}' AND f.Seq1='{this.seq1}' AND f.Seq2='{this.seq2}'
and exists(
    select 1 from Receiving_Detail r where r.ID = '{this.receivingID}' 
    and r.Poid = f.POID
    and r.Seq1 = f.Seq1
    and r.Seq2 = f.Seq2
    and r.StockType = f.StockType
)
";
                            }

                            break;
                        default:
                            break;
                    }
                }

                DualResult upResult;
                TransactionScope transactionscope = new TransactionScope();
                using (transactionscope)
                {
                    try
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                        {
                            transactionscope.Dispose();

                            return;
                        }

                        transactionscope.Complete();
                        transactionscope.Dispose();
                        MyUtility.Msg.InfoBox("Successfully");
                        this.btnAmend.Text = "Amend";
                        this.btnEdit.Text = "Edit";
                        this.btnEdit.Enabled = false;

                        this.Btn_status(this.id);
                    }
                    catch (Exception ex)
                    {
                        transactionscope.Dispose();
                        this.ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
                #endregion

                // ISP20200575 Encode全部執行後
                string sqlcmd = $@"select distinct orderid=o.ID from Orders o with(nolock) where o.poid = '{this.poid}'";
                DataTable dtid;
                DualResult result1 = DBProxy.Current.Select(string.Empty, sqlcmd, out dtid);
                if (!result1)
                {
                    this.ShowErr(result1);
                }
                else
                {
                    string sqlup = $@"
alter table #Tmp alter column OrderID varchar(13)

update a 
set Status = 'Preparing'
from #tmp t
inner join AccessoryOrderList a with(nolock) on a.OrderID = t.orderid and a.Status = 'Waiting'
where dbo.GetAirQaRecord(t.orderid) ='PASS'
";
                    SqlConnection sqlConn = null;
                    DBProxy.Current.OpenConnection("ManufacturingExecution", out sqlConn);
                    result1 = MyUtility.Tool.ProcessWithDatatable(dtid, string.Empty, sqlup, out dtid, "#tmp", sqlConn);
                    if (!result1)
                    {
                        this.ShowErr(result1);
                    }
                }
            }

            // 更新PO.FIRInspPercent和AIRInspPercent
            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'AIR','{this.poid}';")))
            {
                this.ShowErr(result);
            }
        }

        // Save and Edit
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            string strSqlcmd = string.Empty;
            this.btnAmend.Enabled = false;

            DualResult result;
            DataTable dt;
            strSqlcmd = string.Format("Select * from AIR WITH (NOLOCK) where ID='{0}' ", this.id);
            if (result = DBProxy.Current.Select(null, strSqlcmd, null, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    if (this.btnEdit.Text == "Save")
                    {
                        if (MyUtility.Check.Empty(this.txtInspectedQty.Text))
                        {
                            this.txtInspectedQty.Focus();
                            MyUtility.Msg.InfoBox("<Inspected> can not be null");
                            return;
                        }

                        if (MyUtility.Check.Empty(this.comboResult.SelectedValue))
                        {
                            this.comboResult.Focus();
                            MyUtility.Msg.InfoBox("<Result> can not be null");
                            return;
                        }

                        if ((this.txtRejectedQty.Text != "0.00" || this.txtRejectedQty.Text != string.Empty) && MyUtility.Check.Empty(this.editDefect))
                        {
                            this.editDefect.Focus();
                            MyUtility.Msg.InfoBox("When <Rejected Qty> has any value then <Defect> can not be empty !");
                            return;
                        }

                        if ((!MyUtility.Check.Empty(this.editDefect.Text)) && (this.txtRejectedQty.Text == "0.00"))
                        {
                            this.txtRejectedQty.Focus();
                            MyUtility.Msg.InfoBox("<Rejected Qty> can not be empty ,when <Defect> has not empty ! ");
                            return;
                        }

                        if ((this.comboResult.Text.ToString() == "Fail") && (MyUtility.Convert.GetDecimal(this.txtRejectedQty.Text) == 0 || MyUtility.Check.Empty(this.editDefect)))
                        {
                            this.txtRejectedQty.Focus();
                            MyUtility.Msg.InfoBox("When <Result> is Fail then <Rejected Qty> can not be empty !");
                            return;
                        }

                        string updatesql = string.Empty;
                        #region  寫入實體Table Encode
                        string inspDate = MyUtility.Check.Empty(this.dateInspectDate.Value) ? "Null" : "'" + string.Format("{0:yyyy-MM-dd}", this.dateInspectDate.Value) + "'";
                        updatesql = string.Format(
                        "Update Air set InspQty= '{0}',RejectQty='{1}',Inspdate = {2},Inspector = '{3}',Result= '{4}',Defect='{5}',Remark='{6}' where id ='{7}'",
                        this.txtInspectedQty.Value, this.txtRejectedQty.Value,
                        inspDate, this.txtInspector.TextBox1.Text, this.comboResult.Text, this.defect, this.txtRemark.Text, this.id);

                        DualResult upResult;
                        TransactionScope transactionscope = new TransactionScope();
                        using (transactionscope)
                        {
                            try
                            {
                                if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(upResult);
                                    return;
                                }

                                this.SavePricture(transactionscope);
                                transactionscope.Complete();
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }

                        this.LoadPicture();
                        MyUtility.Msg.InfoBox("Successfully");
                        this.btnAmend.Text = "Encode";
                        this.btnEdit.Text = "Edit";
                        this.btnAmend.Enabled = true;
                        #endregion
                        this.txtInspectedQty.ReadOnly = true;
                        this.txtRejectedQty.ReadOnly = true;
                        this.dateInspectDate.ReadOnly = true;
                        this.txtInspector.TextBox1.ReadOnly = true;
                        this.txtsupplier.TextBox1.ReadOnly = true;
                        this.comboResult.ReadOnly = true;
                        this.txtRemark.ReadOnly = true;
                        this.editDefect.ReadOnly = true;
                        this.btnClose.Text = "Close";
                        this.left.Enabled = true;
                        this.right.Enabled = true;
                        this.EditMode = false; // 因為從上一層進來是false,導致popup功能無法使用,所以才改變EditMode
                        return;
                    }
                    else
                    {
                        this.EditMode = true; // 因為從上一層進來是false,導致popup功能無法使用,所以才改變EditMode
                        if (MyUtility.Check.Empty(this.dateInspectDate.Value))
                        {
                            this.dateInspectDate.Value = DateTime.Today;
                            this.CurrentData["InspDate"] = string.Format("{0:yyyy-MM-dd}", this.dateInspectDate.Value);
                        }

                        if (MyUtility.Check.Empty(this.txtInspector.TextBox1.Text))
                        {
                            this.txtInspector.TextBox1.Text = Env.User.UserID;
                            this.CurrentData["Inspector"] = Env.User.UserID;
                        }

                        this.EditMode = true; // 因為從上一層進來是false,導致popup功能無法使用,所以才改變EditMode
                        if (MyUtility.Check.Empty(this.dateInspectDate.Value))
                        {
                            this.dateInspectDate.Value = DateTime.Today;
                            this.CurrentData["InspDate"] = string.Format("{0:yyyy-MM-dd}", this.dateInspectDate.Value);
                        }

                        if (MyUtility.Check.Empty(this.txtInspector.TextBox1.Text))
                        {
                            this.txtInspector.TextBox1.Text = Env.User.UserID;
                            this.CurrentData["Inspector"] = Env.User.UserID;
                        }

                        if (dt.Rows[0]["Status"].ToString().Trim() == "Confirmed")
                        {
                            this.btnAmend.Enabled = true;
                            MyUtility.Msg.InfoBox("It's already Confirmed");
                            return;
                        }

                        if (dt.Rows[0]["Status"].ToString().Trim() != "Confirmed")
                        {
                            this.txtInspectedQty.ReadOnly = false;
                            this.txtRejectedQty.ReadOnly = false;
                            this.dateInspectDate.ReadOnly = false;
                            this.txtInspector.TextBox1.ReadOnly = false;
                            this.txtsupplier.TextBox1.ReadOnly = true;
                            this.comboResult.ReadOnly = false;
                            this.txtRemark.ReadOnly = false;
                            this.editDefect.ReadOnly = true;
                            this.btnEdit.Text = "Save";
                            this.btnClose.Text = "Undo";
                            this.left.Enabled = false;
                            this.right.Enabled = false;
                            return;
                        }
                    }
                }
            }
        }

        private void EditDefect_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.btnEdit.Text != "Save")
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                string sqlcmd = "select id,description from AccessoryDefect WITH (NOLOCK) where Junk = 0 ";
                SelectItem2 item = new SelectItem2(sqlcmd, "Code,Description", "10,30", null, null, null, null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentData["Defect"] = item.GetSelectedString().Replace(",", "+");
                this.defect = item.GetSelectedString().Replace(",", "+");
                string strEditDefect = string.Empty;

                if (item.GetSelectedList().Count > 0)
                {
                    for (int i = 0; i < item.GetSelectedList().Count; i++)
                    {
                        strEditDefect += item.GetSelecteds()[i]["id"].ToString().TrimEnd() + "-" + item.GetSelecteds()[i]["description"].ToString().TrimEnd() + "+";
                    }

                    this.editDefect.Text = strEditDefect.Substring(0, strEditDefect.Length - 1);
                }
                else
                {
                    this.editDefect.Text = string.Empty;
                }
            }
        }

        private void Button_enable(bool canedit)
        {
            // Visable
            this.btnEdit.Visible = (bool)canedit;
            this.btnAmend.Visible = (bool)canedit;

            // Enable
            this.btnEdit.Enabled = this.btnAmend.Text.ToString() == "Encode" ? true : false;
        }

        private void Btn_status(object id)
        {
            string air_cmd = string.Format("select * from air WITH (NOLOCK) where id='{0}'", id);
            DataTable dt;
            DualResult result;
            if (result = DBProxy.Current.Select(null, air_cmd, out dt))
            {
                if (dt.Rows[0]["Status"].ToString().Trim() == "Confirmed")
                {
                    this.btnAmend.Text = "Amend";
                }
                else
                {
                    this.btnAmend.Text = "Encode";
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (this.btnClose.Text.ToString().ToUpper() == "UNDO")
            {
                DialogResult buttonQ = MyUtility.Msg.QuestionBox("Ensure undo?", "Question", MessageBoxButtons.OKCancel);
                if (buttonQ == DialogResult.OK)
                {
                    this.LoadPicture();
                    this.txtInspectedQty.ReadOnly = true;
                    this.txtRejectedQty.ReadOnly = true;
                    this.dateInspectDate.ReadOnly = true;
                    this.txtInspector.TextBox1.ReadOnly = true;
                    this.comboResult.ReadOnly = true;
                    this.txtRemark.ReadOnly = true;
                    this.editDefect.ReadOnly = true;
                    this.btnClose.Text = "Close";
                    this.btnEdit.Text = "Edit";
                    this.left.Enabled = true;
                    this.right.Enabled = true;
                    this.EditMode = false;
                    this.OnAttached(this.CurrentData);
                    this.ReBindingData();
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 將CurrentData 返回原來的值
        /// 特別針對Undo去處理
        /// </summary>
        /// <param name="data">data</param>
        protected override void OnAttached(DataRow data)
        {
            data.RejectChanges();
            base.OnAttached(data);
        }

        private void ChangeData()
        {
            this.id = this.CurrentData["Id"].ToString();
            string air_cmd = $@"

select MtlTypeID = isnull(f.MtlTypeID,'')
,[DefectDesc] = DefectDesc.ValList
,a.* 
from air a WITH (NOLOCK) 
left join Fabric f on a.SCIRefno = f.SCIRefno
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
where a.id='{this.id}'
";

            DataTable dt;
            DBProxy.Current.Select(null, air_cmd, out dt);
            if (!MyUtility.Check.Empty(dt) || dt.Rows.Count > 0)
            {
                this.txtSEQ.Text = dt.Rows[0]["SEQ1"].ToString() + " - " + dt.Rows[0]["SEQ2"].ToString();
                this.receivingID = dt.Rows[0]["ReceivingID"].ToString();
                this.seq1 = dt.Rows[0]["SEQ1"].ToString();
                this.seq2 = dt.Rows[0]["SEQ2"].ToString();
                this.txtMaterialType.Text = dt.Rows[0]["MtlTypeID"].ToString();
                this.defect = dt.Rows[0]["Defect"].ToString();
                this.editDefect.Text = dt.Rows[0]["DefectDesc"].ToString();
                if (dt.Rows[0]["Status"].ToString().Trim() == "Confirmed")
                {
                    this.btnAmend.Text = "Amend";
                }
                else
                {
                    this.btnAmend.Text = "Encode";
                }
            }
            else
            {
                this.txtSEQ.Text = string.Empty;
                this.receivingID = string.Empty;
                this.seq1 = string.Empty;
                this.seq2 = string.Empty;
                this.txtMaterialType.Text = string.Empty;
                this.defect = string.Empty;
                this.editDefect.Text = string.Empty;
            }

            this.Button_enable(this.canedit);
        }

        private void CalculateReject()
        {
            decimal InspQty = MyUtility.Convert.GetDecimal(this.txtInspectedQty.Value);
            decimal RejectQty = MyUtility.Convert.GetDecimal(this.txtRejectedQty.Value);
            if (MyUtility.Check.Empty(InspQty) || MyUtility.Check.Empty(RejectQty))
            {
                this.txtRejectPercent.Value = 0;
            }
            else
            {
                this.txtRejectPercent.Value = Math.Round(((decimal)RejectQty / InspQty) * 100, 2);
            }
        }

        private void Right_Click(object sender, EventArgs e)
        {
            this.ChangeData();
            this.LoadPicture();
            this.CalculateReject();
        }

        private void Left_Click(object sender, EventArgs e)
        {
            this.ChangeData();
            this.LoadPicture();
            this.CalculateReject();
        }

        /// <summary>
        /// 重新ReloadData
        /// 特別針對Undo去處理
        /// </summary>
        private void ReBindingData()
        {
            this.txtInspectedQty.Value = MyUtility.Convert.GetDecimal(this.CurrentData["InspQty"]);
            this.txtRejectedQty.Value = MyUtility.Convert.GetDecimal(this.CurrentData["RejectQty"]);
            this.dateInspectDate.Value = MyUtility.Convert.GetDate(this.CurrentData["InspDate"]);
            this.txtInspector.TextBox1.Text = this.CurrentData["Inspector"].ToString();
            this.comboResult.SelectedValue = this.CurrentData["Result1"].ToString();
        }

        private void BtnUploadDefectPicture_Click(object sender, EventArgs e)
        {
            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog
            {
                InitialDirectory = "c:\\", // 預設路徑
                Filter = "Image Files(*.PNG;*.JPG)|*.PNG;*.JPG", // 使用檔名
                FilterIndex = 1,
                RestoreDirectory = true,
                Multiselect = true,
            };
            if (file.ShowDialog() == DialogResult.OK)
            {
                int virtualSeqnum = this.ListDefectImg.Count == 0 ? 1 : this.ListDefectImg.Max(s => MyUtility.Convert.GetInt(s.VirtualSeq)) + 1;
                foreach (string fileName in file.FileNames)
                {
                    string virtualSeq = virtualSeqnum.ToString().PadLeft(3, '0');
                    this.ListDefectImg.Add(new DefectImg { VirtualSeq = virtualSeq, Img = File.ReadAllBytes(fileName), UpdType = DefectImgUpdType.Insert });
                    virtualSeqnum++;
                }
            }

            this.SetPicCombox();
        }

        private void SavePricture(TransactionScope transactionscope)
        {
            foreach (var item in this.ListDefectImg.Where(w => w.Img != null && w.UpdType != DefectImgUpdType.None))
            {
                List<SqlParameter> paras = new List<SqlParameter>();
                string sqlcmd = string.Empty;

                if (item.UpdType == DefectImgUpdType.Insert)
                {
                    paras = new List<SqlParameter> { new SqlParameter($"@Image", item.Img) };
                    sqlcmd = $@"
set XACT_ABORT on
INSERT INTO SciPMSFile_AIR_DefectImage ([AIRID],[ReceivingID],[Image])VALUES('{this.id}','{this.receivingID}',@Image)
";
                }
                else if (item.UpdType == DefectImgUpdType.Remove && item.Ukey > 0)
                {
                    paras = new List<SqlParameter> { new SqlParameter($"@Ukey", item.Ukey) };
                    sqlcmd = $@"
set XACT_ABORT on
delete SciPMSFile_AIR_DefectImage where Ukey = @Ukey
";
                }
                else
                {
                    continue;
                }

                DualResult result = DBProxy.Current.Execute(null, sqlcmd, paras);
                if (!result)
                {
                    transactionscope.Dispose();
                    this.ShowErr(result);
                    return;
                }
            }
        }

        private void LoadPicture()
        {
            string sqlcmd = $@"select * from SciPMSFile_AIR_DefectImage where AIRID = '{this.id}' and ReceivingID = '{this.receivingID}' order by ukey";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.ListDefectImg.Clear();
            int virtualSeqnum = 1;
            foreach (DataRow dr in dt.Rows)
            {
                string virtualSeq = virtualSeqnum.ToString().PadLeft(3, '0');
                this.ListDefectImg.Add(new DefectImg { VirtualSeq = virtualSeq, Img = (byte[])dr["Image"], UpdType = DefectImgUpdType.None, Ukey = MyUtility.Convert.GetLong(dr["Ukey"]) });
                virtualSeqnum++;
            }

            this.SetPicCombox();
        }

        private void SetPicCombox()
        {
            MyUtility.Tool.SetupCombox(this.cmbDefectPicture, 1, 1, this.ListDefectImg.Where(s => s.UpdType != DefectImgUpdType.Remove).Select(s => s.VirtualSeq).JoinToString(","));
            this.cmbDefectPicture.SelectedIndex = -1;
            this.cmbDefectPicture.SelectedIndex = 0;
        }

        private void CmbDefectPicture_SelectedIndexChanged(object sender, EventArgs e)
        {
            DefectImg defectoicture = this.ListDefectImg.Where(w => w.VirtualSeq == this.cmbDefectPicture.Text).FirstOrDefault();
            if (defectoicture != null && !MyUtility.Check.Empty(defectoicture.Img))
            {
                using (MemoryStream ms = new MemoryStream(defectoicture.Img))
                {
                    this.pictureBox1.Image = Image.FromStream(ms);
                }
            }
            else
            {
                this.pictureBox1.Image = null;
            }
        }

        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            string sqlcmd = $@"select EMail from Pass1 p inner join Orders o on o.MCHandle = p.id where o.id='{this.poid}'";
            string mailto = MyUtility.GetValue.Lookup(sqlcmd);
            if (MyUtility.Check.Empty(mailto))
            {
                sqlcmd = $@"select EMail from TPEPass1 p inner join Orders o on o.MCHandle = p.id where o.id='{this.poid}'";
                mailto = MyUtility.GetValue.Lookup(sqlcmd);
            }

            string ccAddress = string.Empty;
            string subject = $@"Accessory Inspection SP#: {this.poid}, Ref#:{this.txtRefno.Text}, Color#: {this.txtColor.Text}";
            string content = $@"<Information>
[SP#]: {this.poid}
[SEQ]: {this.seq1} {this.seq2}
[WK#]: {this.txtWKNO.Text}
[Ref#]: {this.txtRefno.Text}
[Color#]: {this.txtColor.Text}
[Arrive Qty]: {this.txtArriveQty.Text}
[Unit]: {this.txtUnit.Text}
[Size]: {this.txtSize.Text}

<Inspection Result>
[Inspected Qty]: {this.txtInspectedQty.Text}
[Reject Qty]: {this.txtRejectedQty}
[Inspected Date]: {this.dateInspectDate.Text}
[Inspector]: {this.txtInspector.TextBox1.Text}-{this.txtInspector.DisplayBox1.Text}
[Remark]: {this.txtRemark.Text}
[Result]: {this.comboResult.Text}";

            List<string> attachment = new List<string>();
            foreach (var item in this.ListDefectImg)
            {
                Image img = Image.FromStream(new MemoryStream(item.Img));
                string imageName = this.poid + "_" + this.seq1 + this.seq2 + "_" + this.editDefect.Text + item.VirtualSeq + ".jpg";
                string imgPath = Path.Combine(Env.Cfg.ReportTempDir, imageName);
                img.Save(imgPath);
                attachment.Add(imgPath);
            }

            var email = new MailTo(Env.Cfg.MailFrom, mailto, ccAddress, subject, content, attachment, false, true);
            email.ShowDialog(this);

            foreach (var item in attachment)
            {
                try
                {
                    File.Delete(item);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private List<DefectImg> ListDefectImg = new List<DefectImg>();

        private enum DefectImgUpdType
        {
            None,
            Insert,
            Remove,
        }

        private class DefectImg
        {
            public string VirtualSeq { get; set; }

            public byte[] Img { get; set; }

            public DefectImgUpdType UpdType { get; set; }

            public long Ukey { get; set; } = -1;
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            var targetDefectImage = this.ListDefectImg.Where(s => s.VirtualSeq == this.cmbDefectPicture.Text);

            if (targetDefectImage.Any())
            {
                foreach (var defectImg in targetDefectImage)
                {
                    defectImg.UpdType = DefectImgUpdType.Remove;
                }

                this.SetPicCombox();
            }
        }

        private void TxtInspectedQty_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtInspectedQty.Value))
            {
                this.CalculateReject();
            }
        }

        private void TxtRejectedQty_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtRejectedQty.Value))
            {
                this.CalculateReject();
            }
        }
    }
}
