using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.Tools;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P08 : Win.Tems.Input6
    {
        private string fileNameExt;
        private string pathName;

        /// <summary>
        /// Initializes a new instance of the <see cref="P08"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out DataTable queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID='{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("FabricCombo", header: "Fab Combo", width: Widths.Auto(), iseditingreadonly: true)
            .Text("MarkerName", header: "Mark Name", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.Auto(), iseditingreadonly: true)
            .Text("direction", header: "Type of Cutting", width: Widths.Auto(), iseditingreadonly: true)
            .Text("CuttingWidth", header: "Cut Width", width: Widths.Auto(), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.Auto(), integer_places: 5, decimal_places: 2, iseditingreadonly: true)
            .Numeric("ReleaseQty", header: "Release Qty", width: Widths.Auto(), integer_places: 5, decimal_places: 2)
            .Text("Dyelot", header: "Dyelot", width: Widths.Auto())
            .Text("Refno", header: "Refno", width: Widths.Auto(), iseditingreadonly: true)
            .Numeric("ConsPC", header: "Cons", width: Widths.Auto(), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.Auto())
            ;

            this.detailgrid.Columns["ReleaseQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Dyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not modify.");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickNew()
        {
            this.detailgrid.ValidateControl();
            var frm = new P08_Import();
            DialogResult dr = frm.ShowDialog(this);
            this.ReloadDatas();
            if (dr == DialogResult.OK)
            {
                var topID = frm.ImportedIDs[0];
                int newDataIdx = this.gridbs.Find("ID", topID);
                this.gridbs.Position = newDataIdx;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            #region 存在 Issue 不可 Uncomfirm
            string query = string.Format("Select 1 from Issue WITH (NOLOCK) Where Cutplanid ='{0}'", this.CurrentMaintain["ID"]);
            DualResult result = DBProxy.Current.Select(null, query, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count != 0)
            {
                MyUtility.Msg.WarningBox("The record already issued fabric, you can not Unconfirm.");
                return;
            }
            #endregion

            string updSql = $@"update CutTapePlan set status = 'New',[EditDate]=getdate(),[EditName]='{Env.User.UserID}' where id = '{this.CurrentMaintain["ID"]}'";

            if (!(result = DBProxy.Current.Execute(null, updSql)))
            {
                this.ShowErr(result);
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            #region 檢查不能空值  表頭: EstCutDate / 表身:Dyelot, [Release Qty]
            if (MyUtility.Check.Empty(this.CurrentMaintain["EstCutDate"]))
            {
                MyUtility.Msg.WarningBox("Est. Cutting Date can't empty!");
                return;
            }

            if (this.DetailDatas.AsEnumerable().Where(w => MyUtility.Check.Empty(w["Dyelot"]) || MyUtility.Check.Empty(w["ReleaseQty"])).Any())
            {
                MyUtility.Msg.WarningBox("Dyelot or Release Qty can't empty!");
                return;
            }
            #endregion

            string updSql = $@"update CutTapePlan set status = 'Confirmed',[EditDate] = getdate(),[EditName]='{Env.User.UserID}' where id = '{this.CurrentMaintain["ID"]}'";

            DualResult result = DBProxy.Current.Execute(null, updSql);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查表頭 Est. Cut Date 只能輸入今天和未來的日期 , 一定要填
            // Est. Cut Date 一定要填
            if (MyUtility.Check.Empty(this.CurrentMaintain["EstCutDate"]))
            {
                MyUtility.Msg.WarningBox("Est. Cutting Date can't empty!");
                return false;
            }

            // 只能輸入今天和未來的日期
            if (((DateTime)this.CurrentMaintain["EstCutDate"]).Date < DateTime.Today)
            {
                MyUtility.Msg.WarningBox("Est. Cutting Date can't earlier than today!");
                return false;
            }
            #endregion

            #region 檢查表身 Grid 要有資料, Release Qty 要大於 0

            // Grid 要有資料
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!");
                return false;
            }

            // Dyelot 必填 && Release Qty 要大於 0
            if (this.DetailDatas.AsEnumerable().Where(w => MyUtility.Check.Empty(w["dyelot"]) || MyUtility.Convert.GetDecimal(w["ReleaseQty"]) <= 0).Any())
            {
                MyUtility.Msg.WarningBox("Dyelot or Release Qty can't empty!");
                return false;
            }
            #endregion

            #region 檢查 Release Qty, 依據 FabCombo, MarkerName, ColorID 做群組加總, Sum of Release Qty 不能大於該群組的 Qty
            var x = this.DetailDatas.AsEnumerable()
                .GroupBy(g => new
                {
                    FabricCombo = g["FabricCombo"].ToString(),
                    MarkerName = g["MarkerName"].ToString(),
                    ColorID = g["ColorID"].ToString(),
                    Qty = MyUtility.Convert.GetDecimal(g["Qty"]),
                })
                .Select(s => new
                {
                    s.Key.FabricCombo,
                    s.Key.MarkerName,
                    s.Key.ColorID,
                    s.Key.Qty,
                    ReleaseQty = s.Sum(i => MyUtility.Convert.GetDecimal(i["ReleaseQty"])),
                });
            if (x.Where(w => w.ReleaseQty > w.Qty).Any())
            {
                MyUtility.Msg.WarningBox("According to <Fab Combo>, <Mark Name> and <Color> the total <Release Qty> cannot be greater than <Qty>");
                return false;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not delete.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnSendMail.Enabled = this.CurrentMaintain["Status"].ToString() != "New";
            this.detailgrid.AutoResizeColumns();
        }

        private bool ToExcel(bool excelVisivle)
        {
            if (MyUtility.Check.Empty(this.CurrentDetailData))
            {
                MyUtility.Msg.InfoBox("No any data.");
                return false;
            }

            this.ShowWaitMessage("Excel processing...");
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])),
            };
            string cmdsql = $@"
declare @CT varchar(13) = @ID
select 
	[Cut tape plan#] = c.id
	, d.MarkerName
	, [SP#] =o.POID, d.Seq1, d.Seq2
	, [Style#] = o.StyleID
	, [Comb.]= d.FabricCombo
	, [Fab_Code]=e.FabricCode
	, Size.SizeRatio
	, art.Article
	, d.ColorID
	, Cuts.CutQty 
	, [Fab Cons.] = d.ReleaseQty
	, [Fab Desc] = Fabric.DescDetail
	, d.Remark
from dbo.CutTapePlan c
left join dbo.CutTapePlan_Detail d on d.id = c.id
left join dbo.Orders o on o.ID = c.CuttingID
left join dbo.Order_EachCons e on e.Ukey = d.Order_EachConsUkey
left join dbo.Order_BOF bof WITH (NOLOCK) on bof.Id = e.Id and bof.FabricCode = e.FabricCode
left join dbo.Fabric WITH (NOLOCK) on Fabric.SCIRefno = bof.SCIRefno
outer apply (
	select SizeRatio = STUFF((
		Select CONCAT(',', oes.sizecode, '/ ', oes.qty)
		From Order_EachCons_SizeQty oes WITH (NOLOCK) 
		Where  oes.Order_EachConsUkey = d.Order_EachConsUkey 
		For XML path('')
	),1,1,'')
) as Size 
outer apply ( 
	select Article = STUFF((
		Select distinct CONCAT('/ ', tmpa.Article)
		From Order_EachCons_Color tmpB WITH (NOLOCK) 
		left join Order_EachCons_Color_Article tmpA WITH (NOLOCK) on tmpa.Order_EachCons_ColorUkey = tmpb.Ukey
		Where  tmpb.Order_EachConsUkey =d.Order_EachConsUkey and tmpb.ColorID = d.ColorID
		For XML path('')
	),1,2,'')
) art
outer apply(
	select CutQty = STUFF((
		Select concat(', ', tmp.sizecode, '/ ',  isnull(tmp.qty, 0) * isnull(w.Cuts, 0))
		From Order_EachCons_SizeQty tmp WITH (NOLOCK) 
		outer apply(select Cuts = iif(isnull(d.ConsPC, 0) = 0, 0, isnull(d.ReleaseQty, 0) / isnull(d.ConsPC, 0))) w
		Where  tmp.Order_EachConsUkey =d.Order_EachConsUkey                
		For XML path('')
	),1,2,'')
) as Cuts
where c.id = @CT
";
            DualResult result = DBProxy.Current.Select(null, cmdsql, sqlParameters, out DataTable excelTb);
            if (!result)
            {
                this.ShowErr(result);
                this.HideWaitMessage();
                return false;
            }

            string filename = "Cutting_P08.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + filename); // 預先開啟excel app
            bool isexcelWrite = MyUtility.Excel.CopyToXls(excelTb, string.Empty, filename, 5, showExcel: false, excelApp: excelApp);

            if (!isexcelWrite)
            {
                Marshal.ReleaseComObject(excelApp);
                this.HideWaitMessage();
                return false;
            }

            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1]; // 取得工作表
            worksheet.Cells[1, 1] = this.CurrentMaintain["FactoryID"];
            worksheet.Cells[3, 2] = this.dateCuttingDate.Text;
            worksheet.Cells[3, 6] = this.CurrentMaintain["CuttingID"];
            worksheet.Cells[3, 13] = PublicPrg.Prgs.GetAddOrEditBy(Env.User.UserID);
            worksheet.Rows.AutoFit();
            this.pathName = MicrosoftFile.GetName("Cutting_Tape_Plan");
            Excel.Workbook workbook = excelApp.Workbooks[1];
            workbook.SaveAs(this.pathName);
            excelApp.Visible = excelVisivle; // 此excel物件直接顯示，再次產生的excel物件會在不同視窗
            this.fileNameExt = this.pathName.Substring(this.pathName.LastIndexOf("\\") + 1);

            if (!excelVisivle)
            {
                excelApp.Quit(); // 在excelApp物件關閉之後才能刪除檔案，sendmail之後
            }

            Marshal.ReleaseComObject(excelApp);
            this.HideWaitMessage();

            return true;
        }

        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            if (!this.ToExcel(false))
            {
                return;
            }

            if (MyUtility.Check.Seek("select * from mailto WITH (NOLOCK) where Id='022'", out DataRow seekdr))
            {
                string mailFrom = Env.Cfg.MailFrom;
                string mailto = seekdr["ToAddress"].ToString();
                string cc = seekdr["ccAddress"].ToString();
                string content = seekdr["content"].ToString();
                string subject = "<" + this.CurrentMaintain["mDivisionid"].ToString() + ">BulkMarkerRequest#:" + this.CurrentMaintain["ID"].ToString();
                var email = new MailTo(mailFrom, mailto, cc, subject + "-" + this.fileNameExt, this.pathName, content, false, true);
                DialogResult dialogResult = email.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    string sql = $"Update MarkerReq set sendDate = getdate() where id ='{this.CurrentMaintain["ID"]}'";
                    DualResult result;
                    if (!(result = DBProxy.Current.Execute(null, sql)))
                    {
                        this.ShowErr(result);
                    }
                    else
                    {
                        this.RenewData();
                        this.OnDetailEntered();
                    }
                }
            }

            try
            {
                File.Delete(this.pathName);
            }
            catch (IOException ex)
            {
                this.ShowErr(ex);
            }
        }

        private void BtnSetDefaultMail_Click(object sender, EventArgs e)
        {
            P08_MailTo callNextForm = new P08_MailTo(this.IsSupportEdit, null, null, null);
            callNextForm.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            this.ToExcel(true);
            return base.ClickPrint();
        }
    }
}
