using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.SubProcess
{
    /// <summary>
    /// P01
    /// </summary>
    public partial class P01 : Win.Tems.Input8
    {
        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboTeam, 1, 1, "A,B");
            this.DoSubForm = new P01_QAOutput();
            this.DefaultFilter = $"MDivisionID = '{Env.User.Keyword}'";
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select sd.*,o.styleid, a.QAOutput,o.styleUkey,EFF_percent=concat(EFF,'%')
from SubProcessOutput_Detail sd with(nolock)
left join Orders o with(nolock) on o.id = sd.orderid
outer apply(
	select QAOutput = stuff((
		select concat(',' ,t.TEMP)
		from (
			select TEMP = concat(sdd.SizeCode,'*',sdd.QAQty)
			from SubProcessOutput_Detail_Detail SDD WITH (NOLOCK) 
			where SDD.SubprocessOutput_DetailUKey = sd.UKey
		) t 
		for xml path('')
	),1,1,'')
)a
where sd.ID = '{0}'",
                masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string detail_UKey = (e.Detail == null) ? "0" : MyUtility.Convert.GetString(e.Detail["UKey"]);

            this.SubDetailSelectCommand = string.Format(
                @"
;with AllQty as
(
	select sd.ID, [SubProcessOutput_DetailUkey] = sd.UKey, sd.OrderId,oq.Article,oq.SizeCode,[OrderQty] = oq.Qty,
	QAQty = isnull((
		select QAQty
		from SubProcessOutput_Detail_Detail WITH (NOLOCK)
		where SubProcessOutput_DetailUkey = sd.UKey and SizeCode = oq.SizeCode and orderid = sd.OrderId),0),
	AccumQty = isnull((
				select sum(QAQty) 
				from SubProcessOutput_Detail_Detail WITH (NOLOCK) 
				where OrderId = sd.OrderId  and Article = oq.Article and SizeCode = oq.SizeCode and ID != sd.ID),0) 
	from SubProcessOutput_Detail sd WITH (NOLOCK) ,Order_Qty oq WITH (NOLOCK)  
	where sd.OrderId = oq.ID and sd.Article=oq.Article and sd.UKey = '{0}'
	union all

	select sdd.ID, sdd.SubProcessOutput_DetailUkey, sdd.OrderId,sdd.Article,sdd.SizeCode,[OrderQty] = 0,
	sdd.QAQty,
	AccumQty = isnull((
				select sum(QAQty)
				from SubProcessOutput_Detail_Detail WITH (NOLOCK)
				where OrderId = sdd.OrderId and Article = sdd.Article and SizeCode = sdd.SizeCode and ID != sdd.ID),0)
	from SubProcessOutput_Detail_Detail sdd WITH (NOLOCK)
	where SubProcessOutput_DetailUKey = '{0}'
	and not exists (select 1 from Order_Qty WITH (NOLOCK) where ID = sdd.OrderId and Article = sdd.Article and SizeCode = sdd.SizeCode)
)
select  a.*
        , [Variance] = a.OrderQty-a.AccumQty
        , [BalQty] = a.OrderQty-a.AccumQty-a.QAQty
        , [Seq] = isnull(os.Seq,0)
from AllQty a
left join Orders o WITH (NOLOCK) on a.OrderId = o.ID
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID and os.SizeCode = a.SizeCode
order by a.OrderId,os.Seq
",
                detail_UKey);
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            if (this.EditMode && this.DoSubForm.DialogResult == DialogResult.OK)
            {
                StringBuilder qAOutput = new StringBuilder();
                int qAQty = 0;

                foreach (DataRow dr in e.SubDetails.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (MyUtility.Convert.GetString(dr["SubProcessOutput_DetailUKey"]) == MyUtility.Convert.GetString(this.CurrentDetailData["UKey"]) && !MyUtility.Check.Empty(dr["QAQty"]))
                        {
                            qAOutput.Append(string.Format("{0}*{1},", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["QAQty"])));
                            qAQty = qAQty + MyUtility.Convert.GetInt(dr["QAQty"]);
                        }
                    }
                }

                e.Detail["QAOutput"] = qAOutput.Length > 0 ? qAOutput.ToString() : string.Empty; // 組字串 SizeCode*Qty, ...
                e.Detail["QAQty"] = qAQty; // 總計第三層 Qty 填入第二層 QAQty
            }

            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region SubprocessLineID
            DataGridViewGeneratorTextColumnSettings subprocessLineID = new DataGridViewGeneratorTextColumnSettings();
            subprocessLineID.EditingMouseDown += (s, e) =>
            {
                if (!this.EditingMouseDownContinue(s, e))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlCmd = string.Format(
                    @"
Select  Id, Manpower, Description
from SubProcessLine
where type = '{0}' and Junk = 0 and MDivisionID = '{1}'
order by Id",
                    this.CurrentMaintain["TypeID"],
                    Env.User.Keyword);
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "10,10,20", this.CurrentDetailData["subprocessLineID"].ToString());
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["SubprocessLineID"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                }
            };
            subprocessLineID.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                string sqlCmd = string.Format(
                @"
Select  Id, Manpower, Description
from SubProcessLine
where type = '{0}' and Junk = 0 and Id = '{1}' and MDivisionID = '{2}'
order by Id",
                this.CurrentMaintain["TypeID"],
                e.FormattedValue,
                Env.User.Keyword);
                if (!MyUtility.Check.Seek(sqlCmd))
                {
                    MyUtility.Msg.WarningBox(string.Format("<Subprocess Line:{0}> not found !!", e.FormattedValue));

                    this.CurrentDetailData["SubprocessLineID"] = string.Empty;
                    this.CurrentDetailData.EndEdit();
                    return;
                }

                this.CurrentDetailData["subprocessLineID"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();
            };
            #endregion

            #region orderID
            DataGridViewGeneratorTextColumnSettings orderID = new DataGridViewGeneratorTextColumnSettings();
            orderID.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                if (MyUtility.Convert.GetString(e.FormattedValue) == MyUtility.Convert.GetString(this.CurrentDetailData["OrderID"]))
                {
                    return;
                }

                bool flag = false;
                string sqlCmd = string.Format(
                @"select o.ID, o.StyleID, o.styleukey
from Orders o WITH (NOLOCK) 
inner join Factory f on o.FactoryID = f.ID
inner join Style_Feature SF on SF.styleukey =o.styleukey
where o.ID ='{0}' and o.Category in ('B','S') 
and f.IsProduceFty = 1 and SF.Type='PPA' and o.MDivisionID  = '{1}'",
                e.FormattedValue,
                Env.User.Keyword);
                DataRow dr_orderID;
                if (!MyUtility.Check.Seek(sqlCmd, out dr_orderID))
                {
                    MyUtility.Msg.WarningBox(string.Format("{0} data not find. Please check [Style Feature Data] first !!", e.FormattedValue));
                    this.CurrentDetailData["OrderID"] = string.Empty;
                    this.CurrentDetailData["StyleID"] = string.Empty;
                    this.CurrentDetailData["styleukey"] = 0;
                    flag = false;
                }
                else
                {
                    this.CurrentDetailData["OrderID"] = e.FormattedValue;
                    this.CurrentDetailData["StyleID"] = dr_orderID["StyleID"];
                    this.CurrentDetailData["styleukey"] = dr_orderID["styleukey"];
                    flag = true;
                }

                this.CurrentDetailData["Article"] = string.Empty;
                this.CurrentDetailData["Color"] = string.Empty;
                this.CurrentDetailData["QAOutput"] = string.Empty;
                this.CurrentDetailData["QAQty"] = 0;
                this.CurrentDetailData["ProdQty"] = 0;
                this.CurrentDetailData["DefectQty"] = 0;
                this.CurrentDetailData["Feature"] = string.Empty;
                this.CurrentDetailData["SMV"] = 0;
                this.CurrentDetailData["FeatureCPU"] = 0;
                this.CurrentDetailData["Manpower"] = 0;
                this.CurrentDetailData["Workinghours"] = 0;
                this.CurrentDetailData["TTLWorkinghours"] = 0;
                this.CurrentDetailData["EFF"] = 0;
                this.CurrentDetailData["TotalCPU"] = 0;
                this.CurrentDetailData["PPH"] = 0;
                this.CurrentDetailData["EFF_percent"] = string.Empty;
                this.CurrentDetailData["Remark"] = string.Empty;
                this.CurrentDetailData.EndEdit();
                this.DeleteSubDetailData(this.CurrentDetailData);
                if (flag)
                {
                    this.CreateSubDetailDatas(this.CurrentDetailData);
                }

                this.SumQty_QA_Prod_Defect_CPU();
                this.SumWorkinghours_Manpower();
            };
            #endregion

            #region Article
            DataGridViewGeneratorTextColumnSettings article = new DataGridViewGeneratorTextColumnSettings();
            article.EditingMouseDown += (s, e) =>
            {
                if (!this.EditingMouseDownContinue(s, e))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlCmd = string.Format(@"select Article,ColorID from View_OrderFAColor where Id = '{0}'", this.CurrentDetailData["orderid"]);
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "10,10", this.CurrentDetailData["Article"].ToString());
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (item.GetSelecteds().Count > 0)
                    {
                        if (MyUtility.Convert.GetString(item.GetSelecteds()[0]["Article"]) != MyUtility.Convert.GetString(this.CurrentDetailData["Article"])
                        || MyUtility.Convert.GetString(item.GetSelecteds()[0]["Colorid"]) != MyUtility.Convert.GetString(this.CurrentDetailData["Color"]))
                        {
                            this.CurrentDetailData["Article"] = item.GetSelecteds()[0]["Article"];
                            this.CurrentDetailData["Color"] = item.GetSelecteds()[0]["Colorid"];
                            this.CurrentDetailData["QAOutput"] = string.Empty;
                            this.CurrentDetailData["QAQty"] = 0;
                            this.CurrentDetailData["ProdQty"] = 0;
                            this.CurrentDetailData["DefectQty"] = 0;
                            this.CurrentDetailData["Feature"] = string.Empty;
                            this.CurrentDetailData["SMV"] = 0;
                            this.CurrentDetailData["FeatureCPU"] = 0;
                            this.CurrentDetailData["Manpower"] = 0;
                            this.CurrentDetailData["Workinghours"] = 0;
                            this.CurrentDetailData["TTLWorkinghours"] = 0;
                            this.CurrentDetailData["EFF"] = 0;
                            this.CurrentDetailData["TotalCPU"] = 0;
                            this.CurrentDetailData["PPH"] = 0;
                            this.CurrentDetailData["EFF_percent"] = string.Empty;
                            this.CurrentDetailData["Remark"] = string.Empty;
                            this.CurrentDetailData.EndEdit();
                            this.DeleteSubDetailData(this.CurrentDetailData);
                            this.CreateSubDetailDatas(this.CurrentDetailData);
                        }
                    }

                    this.SumQty_QA_Prod_Defect_CPU();
                    this.SumWorkinghours_Manpower();
                }
            };
            article.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                if (MyUtility.Convert.GetString(e.FormattedValue) == MyUtility.Convert.GetString(this.CurrentDetailData["Article"]))
                {
                    return;
                }

                bool flag = false;
                string sqlCmd = string.Format(@"select * from View_OrderFAColor where Id = '{0}' and Article = '{1}'", this.CurrentDetailData["orderid"], e.FormattedValue);
                DataRow dr_article;
                if (!MyUtility.Check.Seek(sqlCmd, out dr_article))
                {
                    this.CurrentDetailData["Article"] = string.Empty;
                    this.CurrentDetailData["Color"] = string.Empty;
                    flag = false;
                }
                else
                {
                    this.CurrentDetailData["Article"] = e.FormattedValue;
                    this.CurrentDetailData["Color"] = dr_article["Colorid"];
                    flag = true;
                }

                this.CurrentDetailData["QAOutput"] = string.Empty;
                this.CurrentDetailData["QAQty"] = 0;
                this.CurrentDetailData["ProdQty"] = 0;
                this.CurrentDetailData["DefectQty"] = 0;
                this.CurrentDetailData["Feature"] = string.Empty;
                this.CurrentDetailData["SMV"] = 0;
                this.CurrentDetailData["FeatureCPU"] = 0;
                this.CurrentDetailData["Manpower"] = 0;
                this.CurrentDetailData["Workinghours"] = 0;
                this.CurrentDetailData["TTLWorkinghours"] = 0;
                this.CurrentDetailData["EFF"] = 0;
                this.CurrentDetailData["TotalCPU"] = 0;
                this.CurrentDetailData["PPH"] = 0;
                this.CurrentDetailData["EFF_percent"] = string.Empty;
                this.CurrentDetailData["Remark"] = string.Empty;
                this.CurrentDetailData.EndEdit();
                this.DeleteSubDetailData(this.CurrentDetailData);

                if (flag)
                {
                    this.CreateSubDetailDatas(this.CurrentDetailData);
                }

                this.SumQty_QA_Prod_Defect_CPU();
                this.SumWorkinghours_Manpower();
            };
            #endregion

            #region qaoutput
            DataGridViewGeneratorTextColumnSettings qaoutput = new DataGridViewGeneratorTextColumnSettings();
            qaoutput.CellMouseDoubleClick += (s, e) =>
             {
                 if (e.Button == MouseButtons.Left)
                 {
                     this.OpenSubDetailPage();
                     this.Caculate_DefectQty();
                     this.Caculate_EFF();
                     this.Caculate_TotalCPU();
                     this.Caculate_PPH();
                     this.CurrentDetailData.EndEdit();
                     this.SumQty_QA_Prod_Defect_CPU();
                     this.SumWorkinghours_Manpower();
                 }
             };
            #endregion

            #region prodQty
            DataGridViewGeneratorNumericColumnSettings prodQty = new DataGridViewGeneratorNumericColumnSettings();
            prodQty.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                this.CurrentDetailData["ProdQty"] = MyUtility.Convert.GetDecimal(e.FormattedValue);
                this.Caculate_DefectQty();
                this.CurrentDetailData.EndEdit();
                this.SumQty_QA_Prod_Defect_CPU();
                this.SumWorkinghours_Manpower();
            };
            #endregion

            #region Feature
            DataGridViewGeneratorTextColumnSettings feature = new DataGridViewGeneratorTextColumnSettings();
            feature.EditingMouseDown += (s, e) =>
            {
                if (!this.EditingMouseDownContinue(s, e))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    if (MyUtility.Check.Empty(this.CurrentDetailData["OrderID"]))
                    {
                        MyUtility.Msg.WarningBox("Please enter SP# first");
                        return;
                    }

                    string sqlCmd = string.Format(
                    @"
Select Feature,smv, remark
From Style_Feature
Where type='{0}'  and styleUkey='{1}'
Order by Feature",
                    this.CurrentMaintain["TypeID"],
                    this.CurrentDetailData["styleukey"]);
                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlCmd, "Feature,SMV,Remark", "12,8,20", MyUtility.Convert.GetString(this.CurrentDetailData["Feature"]), "0,4,0");
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Feature"] = item.GetSelectedString();
                    this.Caculate_SMV();
                    this.CurrentDetailData.EndEdit();
                    this.SumQty_QA_Prod_Defect_CPU();
                    this.SumWorkinghours_Manpower();
                }
            };
            feature.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                if (MyUtility.Check.Empty(this.CurrentDetailData["OrderID"]))
                {
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        MyUtility.Msg.WarningBox("Please enter SP# first");
                    }

                    return;
                }

                string sqlCmd = string.Format(
                    @"
Select Feature,smv, remark
From Style_Feature
Where type='{0}'  and styleUkey='{1}'
Order by Feature",
                    this.CurrentMaintain["TypeID"],
                    this.CurrentDetailData["styleukey"]);

                this.CurrentDetailData["Feature"] = e.FormattedValue;
                DataTable dt;
                DBProxy.Current.Select(null, sqlCmd, out dt);
                string[] getFeature = this.CurrentDetailData["Feature"].ToString().Split(',').Distinct().ToArray();
                bool selectId = true;
                List<string> errFeature = new List<string>();
                List<string> trueFeature = new List<string>();
                foreach (string feature1 in getFeature)
                {
                    if (!dt.AsEnumerable().Any(row => row["Feature"].EqualString(feature1)) && !feature1.EqualString(string.Empty))
                    {
                        selectId &= false;
                        errFeature.Add(feature1);
                    }
                    else if (!feature1.EqualString(string.Empty))
                    {
                        trueFeature.Add(feature1);
                    }
                }

                if (!selectId)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("<Subprocess Feature:" + string.Join(",", errFeature.ToArray()) + " > not found !!", "Data not found");
                }

                trueFeature.Sort();
                this.CurrentDetailData["Feature"] = string.Join(",", trueFeature.ToArray());
                this.Caculate_SMV();
                this.CurrentDetailData.EndEdit();
                this.SumQty_QA_Prod_Defect_CPU();
                this.SumWorkinghours_Manpower();
            };
            #endregion

            #region SMV
            DataGridViewGeneratorNumericColumnSettings sMV = new DataGridViewGeneratorNumericColumnSettings();
            sMV.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                this.CurrentDetailData["SMV"] = e.FormattedValue;
                this.Caculate_FeatureCPU();
                this.Caculate_EFF();
                this.CurrentDetailData.EndEdit();
                this.SumQty_QA_Prod_Defect_CPU();
                this.SumWorkinghours_Manpower();
            };
            #endregion

            #region manpower
            DataGridViewGeneratorNumericColumnSettings manpower = new DataGridViewGeneratorNumericColumnSettings();
            manpower.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                this.CurrentDetailData["manpower"] = e.FormattedValue;
                this.Caculate_TTLWorkinghours();
                this.CurrentDetailData.EndEdit();
                this.SumQty_QA_Prod_Defect_CPU();
                this.SumWorkinghours_Manpower();
            };
            #endregion

            #region workinghours
            DataGridViewGeneratorNumericColumnSettings workinghours = new DataGridViewGeneratorNumericColumnSettings();
            workinghours.CellValidating += (s, e) =>
            {
                if (!this.CellValidatingContinue(s, e))
                {
                    return;
                }

                this.CurrentDetailData["workinghours"] = e.FormattedValue;
                this.Caculate_TTLWorkinghours();
                this.CurrentDetailData.EndEdit();
                this.SumQty_QA_Prod_Defect_CPU();
                this.SumWorkinghours_Manpower();
            };
            #endregion

            #region Grid欄位
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("SubprocessLineID", header: "SubPro.\r\nLine", width: Widths.AnsiChars(6), settings: subprocessLineID)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(11), settings: orderID)
            .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(7), settings: article)
            .Text("QAOutput", header: "QA\r\nOutput", width: Widths.AnsiChars(12), iseditingreadonly: true, settings: qaoutput)
            .Numeric("QAQty", header: "QA Ttl\r\nOutput", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Numeric("ProdQty", header: "Prod.\r\nOutput", width: Widths.AnsiChars(3), settings: prodQty)
            .Numeric("DefectQty", header: "Defect\r\nQ’ty", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("Feature", header: "SubPro.\r\nFeature", width: Widths.AnsiChars(9), settings: feature)
            .Numeric("SMV", header: "SMV\r\n(Sec)", width: Widths.AnsiChars(3), settings: sMV)
            .Numeric("Manpower", header: "Manpower", width: Widths.AnsiChars(3), decimal_places: 2, maximum: 999.99m, settings: manpower)
            .Numeric("Workinghours", header: "Working\r\nhours", width: Widths.AnsiChars(4), decimal_places: 2, maximum: 999.99m, settings: workinghours)
            .Numeric("TTLWorkinghours", header: "Total\r\nW’Hour", width: Widths.AnsiChars(4), decimal_places: 2, iseditingreadonly: true)
            .Numeric("FeatureCPU", header: "Feature\r\n(CPU)", width: Widths.AnsiChars(4), decimal_places: 4, maximum: 99.9999m, iseditingreadonly: true)
            .Text("EFF_percent", header: "EFF(%)", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Numeric("TotalCPU", header: "Total\r\nCPU", width: Widths.AnsiChars(4), decimal_places: 2, iseditingreadonly: true)
            .Numeric("PPH", header: "PPH", width: Widths.AnsiChars(4), decimal_places: 3, iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(5))
            ;

            this.detailgrid.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10);
            this.detailgrid.DefaultCellStyle.Font = new Font("Tahoma", 10);
            #endregion

            #region 顏色設定
            this.detailgrid.Columns["SubprocessLineID"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["OrderID"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Article"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["QAOutput"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ProdQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Feature"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["SMV"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Manpower"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Workinghours"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.SumWorkinghours_Manpower();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 表身需有資料
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                return false;
            }
            #endregion

            #region 表頭的[type][date][shift][team]為必填欄位 /[type]在New就帶入PPA此欄位唯讀, [W/Hours(Day)]不可為0。
            if (MyUtility.Check.Empty(this.CurrentMaintain["Typeid"]))
            {
                this.txtTypeID.Focus();
                MyUtility.Msg.WarningBox("Type can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["OutputDate"]))
            {
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["shift"]))
            {
                this.txtdropdownlistShift.Focus();
                MyUtility.Msg.WarningBox("Shift can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Team"]))
            {
                this.comboTeam.Focus();
                MyUtility.Msg.WarningBox("Team can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["WHour"]))
            {
                this.numWHours.Focus();
                MyUtility.Msg.WarningBox("WHour can't empty!!");
                return false;
            }
            #endregion

            #region 表身的[SP#][QA Output][SubPro. Feature][SMV][Manpower][working hours] 為必填欄位。
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["orderid"]) || MyUtility.Check.Empty(dr["QAOutput"]) || MyUtility.Check.Empty(dr["Feature"])
                    || MyUtility.Check.Empty(dr["SMV"]) || MyUtility.Check.Empty(dr["Manpower"]) || MyUtility.Check.Empty(dr["Workinghours"]))
                {
                    MyUtility.Msg.WarningBox("[SP#],[QA Output],[SubPro. Feature],[SMV],[Manpower],[working hours] can't be empty!");
                    return false;
                }
            }
            #endregion

            #region 新增資料時, 檢查表頭 type,Date, Shift, Team是否已存在
            if (this.IsDetailInserting)
            {
                string check = string.Format(
                    @"select 1 from SubProcessOutput s WITH(NOLOCK) where s.TypeID = '{0}' and s.OutputDate = '{1}' and s.Shift = '{2}' and s.Team = '{3}'",
                    this.CurrentMaintain["TypeID"],
                    ((DateTime)this.CurrentMaintain["OutputDate"]).ToString("d"),
                    this.CurrentMaintain["Shift"],
                    this.CurrentMaintain["Team"],
                    this.CurrentMaintain["id"]);
                if (MyUtility.Check.Seek(check))
                {
                    MyUtility.Msg.WarningBox("This data already exist !!");
                    return false;
                }
            }
            #endregion

            this.SumWorkinghours_Manpower();
            this.SumQty_QA_Prod_Defect_CPU();

            #region GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", Env.User.Factory, "Factory", "ID") + "SU", "SubProcessOutput ", DateTime.Today, 3, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            string checkNonData = string.Format(
                @"
delete sodd
from SubProcessOutput_Detail_Detail sodd
where not exists (select 1 
				  from SubProcessOutput_Detail sod 
				  where sodd.ID = sod.ID
						and sodd.SubProcessOutput_DetailUKey = sod.UKey
						and sodd.OrderId = sod.OrderId
						and sodd.Article = sod.Article)
      and sodd.id = '{0}'",
                this.CurrentMaintain["ID"]);

            DualResult result = DBProxy.Current.Execute(null, checkNonData);
            if (!result)
            {
                return result;
            }

            return base.ClickSavePost();
        }

        private ITableSchema sub_Schema;

        /// <inheritdoc/>
        protected override DualResult ClickSaveSubDetial(SubDetailSaveEventArgs e)
        {
            DualResult result = base.ClickSaveSubDetial(e);
            if (!result)
            {
                return result;
            }

            #region override 新增、刪除第三層資料
            List<DataRow> inserted = new List<DataRow>();
            List<DataRow> deleteList = new List<DataRow>();
            var ok = DBProxy.Current.GetTableSchema(null, this.SubGridAlias, out this.sub_Schema);
            if (!ok)
            {
                return ok;
            }

            foreach (KeyValuePair<DataRow, DataTable> it in e.SubDetails)
            {
                foreach (DataRow dr in it.Value.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (MyUtility.Convert.GetInt(dr["QAQty"]) <= 0)
                    {
                        deleteList.Add(dr);
                    }
                    else
                    {
                        if (dr.RowState == DataRowState.Modified && MyUtility.Convert.GetInt(dr["QAQty"]) > 0 && MyUtility.Convert.GetInt(dr["QAQty", DataRowVersion.Original]) == 0)
                        {
                            dr.AcceptChanges();
                            dr.SetAdded();
                            inserted.Add(dr);
                        }
                    }
                }
            }

            List<DataRow> newDelete = new List<DataRow>();
            if (deleteList.Count > 0)
            {
                var newT = deleteList[0].Table.Clone();
                for (int i = 0; i < deleteList.Count; i++)
                {
                    var newOne = newT.NewRow();
                    newOne.ItemArray = deleteList[i].ItemArray;
                    try
                    {
                        if (deleteList[i].RowState != DataRowState.Added)
                        {
                            newOne["QaQty"] = deleteList[i]["qaqty", DataRowVersion.Original];
                        }
                        else
                        {
                            newOne["QaQty"] = deleteList[i]["qaqty", DataRowVersion.Current];
                        }
                    }
                    catch (Exception ec)
                    {
                        this.ShowErr("Error:", ec);
                    }

                    newDelete.Add(newOne);
                    newT.Rows.Add(newOne);
                }

                newT.AcceptChanges();
                for (int i = 0; i < deleteList.Count; i++)
                {
                    newDelete[i]["QaQty"] = 0;
                }
            }

            ok = DBProxy.Current.Deletes(null, this.sub_Schema, newDelete);
            if (!ok)
            {
                return ok;
            }

            ok = DBProxy.Current.Inserts(null, this.sub_Schema, inserted);
            if (!ok)
            {
                return ok;
            }
            #endregion
            return ok;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["OutputDate"] = DateTime.Today.AddDays(-1);
            this.CurrentMaintain["Shift"] = "D";
            this.CurrentMaintain["Team"] = "A";
            this.CurrentMaintain["status"] = "New";
            this.CurrentMaintain["TypeID"] = "PPA";
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.dateDate.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            #region 更新表頭status
            string sql_updata_status = string.Format(
                @"
update SubProcessOutput 
set status = 'Confirmed'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'",
                Env.User.UserID,
                this.CurrentMaintain["id"]);
            #endregion

            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!(upResult = DBProxy.Current.Execute(null, sql_updata_status)))
                {
                    scope.Dispose();
                    this.ShowErr(upResult);
                    return;
                }

                scope.Complete();
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            #region 更新表頭status
            string sql_updata_status = string.Format(
                @"
update SubProcessOutput 
set status = 'New'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'",
                Env.User.UserID,
                this.CurrentMaintain["id"]);
            #endregion

            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!(upResult = DBProxy.Current.Execute(null, sql_updata_status)))
                {
                    scope.Dispose();
                    this.ShowErr(upResult);
                    return;
                }

                scope.Complete();
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            #region SQL撈取資料
            string ftyName = MyUtility.GetValue.Lookup(string.Format(@"select NameEN from MDivision where ID = '{0}'", Env.User.Keyword));
            string secondrow = "PPA Daily CMP Report, DD." + MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]).Value.Month + "." + MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]).Value.Day;
            string sqlcmd = string.Format(
                @"
select sd.SubprocessLineID, o.styleid,orderid = o.id,o.factoryid
into #tmp
from SubProcessOutput s with(nolock)
inner join SubProcessOutput_Detail sd with(nolock) on s.id=sd.id
left join Orders o with(nolock) on o.id = sd.orderid
where s.id = '{0}'

select t.orderid,t.StyleID,sd.SubprocessLineID,maxOutputDate = max(s.OutputDate),t.factoryid,
	Accudate = [dbo].[GetSubprocessAccudate](t.StyleID,sd.SubprocessLineID,max(s.OutputDate),t.factoryid)
into #tmp2
from #tmp t
inner join SubProcessOutput_Detail sd with(nolock) on sd.SubprocessLineID = t.SubprocessLineID
left join SubProcessOutput s with(nolock) on s.id=sd.id
left join Orders o with(nolock) on o.id = sd.orderid and o.StyleID = t.StyleID
where s.Status='Confirmed'
group by t.orderid,t.StyleID,sd.SubprocessLineID,t.factoryid

select
    empty = null,--Excel從B欄開始
	o.factoryid,
	o.sewline,
	sd.SubprocessLineID,
	o.styleid,
	sd.orderid,
	sd.Feature,
	sd.SMV,
	t.Accudate,
	sd.Manpower,
	sd.Workinghours,
	sd.QAQty,
	sd.FeatureCPU,
	EFF_percent=concat(EFF,'%'),
	sd.TotalCPU,
	sd.PPH
from SubProcessOutput_Detail sd with(nolock)
inner join SubProcessOutput s with(nolock) on s.id =sd.id
left join Orders o with(nolock) on o.id = sd.orderid
inner join #tmp2 t on t.orderid = sd.orderid
outer apply(
	select QAOutput = stuff((
		select concat(',' ,t.TEMP)
		from (
			select TEMP = concat(sdd.SizeCode,'*',sdd.QAQty)
			from SubProcessOutput_Detail_Detail SDD WITH (NOLOCK) 
			where SDD.SubprocessOutput_DetailUKey = sd.UKey
		) t 
		for xml path('')
	),1,1,'')
)a
where sd.ID = '{0}'",
                this.CurrentMaintain["id"]);
            DataTable printData;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out printData);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            sqlcmd = string.Format(
                @"
select ttlEFF = sum(sd.TotalCPU)*1400/3600/sum(sd.TTLworkinghours),ttlpph = sum(sd.TotalCPU)/sum(sd.TTLworkinghours)
from SubProcessOutput s with(nolock)
inner join SubProcessOutput_Detail sd with(nolock) on s.id=sd.id
where s.id = '{0}'
",
                this.CurrentMaintain["id"]);
            DataTable ttl;
            result = DBProxy.Current.Select(null, sqlcmd, out ttl);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }
            #endregion
            if (printData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "Subprocess_P01.xltx");
            Microsoft.Office.Interop.Excel._Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 2] = ftyName;
            worksheet.Cells[2, 2] = secondrow;
            worksheet.Cells[7, 14] = ttl.Rows[0]["ttlEFF"];
            worksheet.Cells[7, 16] = ttl.Rows[0]["ttlpph"];

            for (int i = 2; i < printData.Rows.Count; i++)
            {
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(5)), Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToInsert);
            }

            MyUtility.Excel.CopyToXls(printData, string.Empty, "Subprocess_P01.xltx", 3, false, null, objApp);
            worksheet.get_Range("A1").ColumnWidth = 1.88;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Subprocess_P01");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return base.ClickPrint();
        }

        private void DateDate_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                // 檢查日期不可超過今天
                if (this.dateDate.Value > DateTime.Today)
                {
                    this.dateDate.Value = this.dateDate.OldValue;
                    MyUtility.Msg.WarningBox("The date can not exceed today !!");
                }
            }
        }

        private void NumWHours_Validating(object sender, CancelEventArgs e)
        {
            this.CurrentMaintain["WHour"] = this.numWHours.Value;
            this.SumWorkinghours_Manpower();
        }

        private void DeleteSubDetailData(DataRow dr)
        {
            DataTable subDetailData;
            this.GetSubDetailDatas(dr, out subDetailData);
            foreach (DataRow ddr in subDetailData.Rows)
            {
                ddr["QAQty"] = 0;
            }

            dr["QAQty"] = 0;
            dr["DefectQty"] = 0;

            // dr["InlineQty"] = 0;
        }

        private void CreateSubDetailDatas(DataRow dr)
        {
            if (MyUtility.Check.Empty(dr["OrderID"]) || MyUtility.Check.Empty(dr["Article"]))
            {
                return;
            }

            string sqlCmd = string.Format(
                @"
;with AllQty as (
    select ID = '{0}'
           , SubProcessOutput_DetailUkey = '{1}'
           , OrderId = oq.ID
           , oq.Article
           , oq.SizeCode
           , [OrderQty] = oq.Qty
           , [QAQty] = 0
           , AccumQty = isnull((select sum(QAQty) 
                                from SubProcessOutput_Detail_Detail WITH (NOLOCK) 
                                where OrderId = oq.ID 
                                and Article = oq.Article 
                                and SizeCode = oq.SizeCode
                                and ID != '{0}'), 0) 
    from Order_Qty oq WITH (NOLOCK) 
    where oq.ID = '{2}' and oq.Article = '{3}'
)
select a.*
       , [Variance] = a.OrderQty - a.AccumQty
       , [BalQty] = a.OrderQty - a.AccumQty - a.QAQty
       , [Seq] = isnull(os.Seq,0)
       , OldDetailKey = ''
from AllQty a
left join Orders o WITH (NOLOCK) on a.OrderId = o.ID
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID and os.SizeCode = a.SizeCode
order by a.OrderId,os.Seq
",
                this.CurrentMaintain["ID"],
                this.CurrentDetailData["ukey"],
                this.CurrentDetailData["orderid"],
                this.CurrentDetailData["Article"]);

            DataTable orderQtyData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out orderQtyData);
            if (result && orderQtyData.Rows.Count > 0)
            {
                DataTable subDetailData;
                this.GetSubDetailDatas(dr, out subDetailData);
                foreach (DataRow ddr in orderQtyData.Rows)
                {
                    if (!subDetailData.AsEnumerable().Any(row => row["ID"].EqualString(ddr["ID"])
                                                                && row["SubProcessOutput_DetailUkey"].EqualString(ddr["SubProcessOutput_DetailUkey"])
                                                                && row["OrderID"].EqualString(ddr["OrderID"])
                                                                && row["Article"].EqualString(ddr["Article"])
                                                                && row["SizeCode"].EqualString(ddr["SizeCode"])))
                    {
                        DataRow newDr = subDetailData.NewRow();
                        for (int i = 0; i < subDetailData.Columns.Count; i++)
                        {
                            newDr[subDetailData.Columns[i].ColumnName] = ddr[subDetailData.Columns[i].ColumnName];
                        }

                        subDetailData.Rows.Add(newDr);
                    }
                }
            }
        }

        private void SumQty_QA_Prod_Defect_CPU()
        {
            this.CurrentMaintain["QAQty"] = ((DataTable)this.detailgridbs.DataSource).Compute("sum(QAQty)", string.Empty);
            this.CurrentMaintain["ProdQty"] = ((DataTable)this.detailgridbs.DataSource).Compute("sum(ProdQty)", string.Empty);
            this.CurrentMaintain["DefectQty"] = ((DataTable)this.detailgridbs.DataSource).Compute("sum(DefectQty)", string.Empty);
            this.CurrentMaintain["TotalCPU"] = ((DataTable)this.detailgridbs.DataSource).Compute("sum(TotalCPU)", string.Empty);
        }

        private void SumWorkinghours_Manpower()
        {
            var ttl_Workinghours = ((DataTable)this.detailgridbs.DataSource).Compute("sum(TTLWorkinghours)", string.Empty);
            this.numTTLhours.Value = MyUtility.Convert.GetDecimal(ttl_Workinghours);

            this.CurrentMaintain["Manpower"] = Math.Ceiling(MyUtility.Check.Empty(this.numWHours.Value) ? 0 : MyUtility.Convert.GetDecimal(ttl_Workinghours) / MyUtility.Convert.GetDecimal(this.numWHours.Value));
        }

        private void Caculate_DefectQty()
        {
            this.CurrentDetailData["DefectQty"] = MyUtility.Convert.GetDecimal(this.CurrentDetailData["ProdQty"]) - MyUtility.Convert.GetDecimal(this.CurrentDetailData["QAQty"]);
        }

        private void Caculate_SMV()
        {
            decimal smv = 0;
            string styleUkey = MyUtility.Convert.GetString(this.CurrentDetailData["styleUkey"]);
            string[] getFeature = this.CurrentDetailData["Feature"].ToString().Split(',').Distinct().ToArray();
            foreach (string item in getFeature)
            {
                smv += MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(string.Format("select smv = smv*60  from Style_Feature where styleUkey = '{0}' and type = 'PPA' and Feature ='{1}'", styleUkey, item)));
            }

            this.CurrentDetailData["smv"] = Math.Round(smv, 0);
            this.Caculate_FeatureCPU();
            this.Caculate_EFF();
        }

        private void Caculate_FeatureCPU()
        {
            decimal stdTMS = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("select StdTMS from system"));
            this.CurrentDetailData["FeatureCPU"] = MyUtility.Convert.GetDecimal(this.CurrentDetailData["smv"]) / stdTMS;
            this.Caculate_TotalCPU();
            this.Caculate_PPH();
        }

        private void Caculate_TTLWorkinghours()
        {
            this.CurrentDetailData["TTLWorkinghours"] = Math.Round(MyUtility.Convert.GetDecimal(this.CurrentDetailData["Manpower"]) * MyUtility.Convert.GetDecimal(this.CurrentDetailData["Workinghours"]), 2);
            this.Caculate_EFF();
            this.Caculate_PPH();
        }

        private void Caculate_EFF()
        {
            this.CurrentDetailData["EFF"] = MyUtility.Convert.GetDecimal(this.CurrentDetailData["TTLWorkinghours"]) == 0 ? 0 : Math.Round(MyUtility.Convert.GetDecimal(this.CurrentDetailData["QAQty"]) * MyUtility.Convert.GetDecimal(this.CurrentDetailData["SMV"]) / 3600 / MyUtility.Convert.GetDecimal(this.CurrentDetailData["TTLWorkinghours"]) * 100, 0);
            this.CurrentDetailData["EFF_percent"] = MyUtility.Convert.GetString(this.CurrentDetailData["EFF"]) + "%";
        }

        private void Caculate_TotalCPU()
        {
            this.CurrentDetailData["TotalCPU"] = Math.Round(MyUtility.Convert.GetDecimal(this.CurrentDetailData["QAQty"]) * MyUtility.Convert.GetDecimal(this.CurrentDetailData["FeatureCPU"]), 2);
        }

        private void Caculate_PPH()
        {
            this.CurrentDetailData["PPH"] = MyUtility.Convert.GetDecimal(this.CurrentDetailData["TTLWorkinghours"]) == 0 ? 0 : Math.Round(MyUtility.Convert.GetDecimal(this.CurrentDetailData["QAQty"]) * MyUtility.Convert.GetDecimal(this.CurrentDetailData["FeatureCPU"]) / MyUtility.Convert.GetDecimal(this.CurrentDetailData["TTLWorkinghours"]), 3);
        }

        private bool CellValidatingContinue(object s, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            if (!this.EditMode)
            {
                return false;
            }

            if (e.RowIndex < 0)
            {
                return false;
            }

            return true;
        }

        private bool EditingMouseDownContinue(object s, Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e)
        {
            if (!this.EditMode)
            {
                return false;
            }

            if (e.RowIndex < 0)
            {
                return false;
            }

            return true;
        }
    }
}
