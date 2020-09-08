using Ict;
using Ict.Win;
using Sci.Data;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P23 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string querySql = @"select '' union select Id from SubProcess where IsRFIDProcess=1";
            DBProxy.Current.Select(null, querySql, out DataTable queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.lbl_queryfor.Text = "Sub Process";
            this.queryfors.SelectedIndex = 0;
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("StartProcess = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();

            // DataRow dr;
            ts1.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow drr = ((Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                if (e.FormattedValue.ToString() == drr["orderid"].ToString())
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.CurrentDetailData["BundleNo"] = string.Empty;
                    this.CurrentDetailData["OrderID"] = string.Empty;
                    this.CurrentDetailData["SubprocessId"] = string.Empty;
                    this.CurrentDetailData["Patterncode"] = string.Empty;
                    this.CurrentDetailData["PatternDesc"] = string.Empty;
                    return;
                }

                if (e.FormattedValue.ToString() != string.Empty)
                {
                    if (MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from Bundle_Detail WITH (NOLOCK) where BundleNo = '{0}')", e.FormattedValue), null))
                    {
                        string sqlcmd = string.Format(
                            @"
select B.Orderid, Artwork.value, BD.Patterncode, BD.PatternDesc
from Bundle_Detail BD WITH (NOLOCK)
left join Bundle B on B.ID=BD.Id
outer apply (
    select value = stuff ((	
        select concat('+', subprocessid)
	    from (
		    select distinct subprocessid
		    from Bundle_Detail_Art bda WITH (NOLOCK)
		    where bd.BundleNo = bda.Bundleno
	    ) k
	    for xml path('')
    ), 1, 1, '')
) ArtWork
where BundleNo='{0}'",
                            e.FormattedValue);
                        DBProxy.Current.Select(null, sqlcmd, out DataTable getdata);

                        this.CurrentDetailData["BundleNo"] = e.FormattedValue;
                        this.CurrentDetailData["OrderID"] = getdata.Rows[0]["Orderid"].ToString();
                        this.CurrentDetailData["SubprocessId"] = getdata.Rows[0]["value"].ToString();
                        this.CurrentDetailData["Patterncode"] = getdata.Rows[0]["Patterncode"].ToString();
                        this.CurrentDetailData["PatternDesc"] = getdata.Rows[0]["PatternDesc"].ToString();
                    }
                    else
                    {
                        e.Cancel = true;
                        this.CurrentDetailData["BundleNo"] = string.Empty;
                        this.CurrentDetailData["OrderID"] = string.Empty;
                        this.CurrentDetailData["SubprocessId"] = string.Empty;
                        this.CurrentDetailData["Patterncode"] = string.Empty;
                        this.CurrentDetailData["PatternDesc"] = string.Empty;
                        MyUtility.Msg.WarningBox("Bundle# is not exist!!", "Data not found");
                        return;
                    }
                }
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("BundleNo", header: "Bundle#", width: Widths.AnsiChars(12), settings: ts1, iseditingreadonly: false)
                .Text("OrderIDs", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SubprocessId", header: "Artwork", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Patterncode", header: "PTN Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("PatternDesc", header: "PTN Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true);
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select 
    BTD.ID
	,BTD.BundleNo
	,BTD.orderid
    ,OrderIDs = dbo.GetSinglelineSP((select distinct OrderID from Bundle_Detail_Order where BundleNo = bd.BundleNo order by OrderID for XML RAW))
	,S.SubprocessId
	,BD.Patterncode
	,BD.PatternDesc
from BundleTrack_detail BTD 
LEFT JOIN Bundle_Detail BD ON BD.BundleNo = BTD.BundleNo
OUTER APPLY(
	SELECT SubprocessId = STUFF((
		SELECT CONCAT('+',SubprocessId )
		FROM Bundle_Detail_Art BDA
		WHERE BDA.Bundleno = BTD.BundleNo
		FOR XML PATH('')
	),1,1,'')
)S
WHERE BTD.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool ClickNew()
        {
            var frm = new P23_ImportBarcode();
            frm.ShowDialog(this);
            this.ReloadDatas();
            return true;
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            string sqlcmd = string.Format(
                @"
select
	BTD.orderid
	,style.StyleID
	,BD.BundleGroup
	,BTD.BundleNo
	,[Body/cut#] = concat(B.FabricPanelCode , '/' , B.Cutno)
	,bd.SizeCode
	,bd.Qty
	,Color = concat(b.Article,' ',b.Colorid)
	,S.SubprocessId
from BundleTrack_detail BTD 
LEFT JOIN Bundle_Detail BD ON BD.BundleNo = BTD.BundleNo
left join Bundle B on B.ID = BD.Id

outer apply(select StyleID from Orders o where o.ID = BTD.orderid)style
OUTER APPLY(
	SELECT SubprocessId = STUFF((
		SELECT CONCAT('+',SubprocessId )
		FROM Bundle_Detail_Art BDA
		WHERE BDA.Bundleno = BTD.BundleNo
		FOR XML PATH('')
	),1,1,'')
)S
WHERE BTD.ID = '{0}'
order by BTD.orderid", this.CurrentMaintain["ID"].ToString());

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable print);
            if (!result)
            {
                this.ShowErr("Query data fail\r\n" + result.ToString());
                return false;
            }

            if (print.Rows.Count == 0)
            {
                this.ShowErr("Data no found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_P23.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(print, string.Empty, "Subcon_P23.xltx", 3, false, null, objApp); // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 2] = this.CurrentMaintain["ID"].ToString();
            objSheets.Cells[1, 5] = this.CurrentMaintain["StartProcess"].ToString();
            objSheets.Cells[1, 8] = MyUtility.Convert.GetDate(this.CurrentMaintain["IssueDate"]);
            string labb = MyUtility.GetValue.Lookup("abb", this.CurrentMaintain["EndSite"].ToString(), "LocalSupp", "ID");
            objSheets.Cells[2, 2] = this.CurrentMaintain["EndSite"].ToString() + "-" + labb;
            string fabb = MyUtility.GetValue.Lookup("abb", this.CurrentMaintain["StartSite"].ToString(), "Factory", "ID");
            objSheets.Cells[2, 5] = this.CurrentMaintain["StartSite"].ToString() + "-" + fabb;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Subcon_P23");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DataTable dg = (DataTable)this.detailgridbs.DataSource;
            for (int i = dg.Rows.Count; i > 0; i--)
            {
                if (dg.Rows[i - 1].RowState != DataRowState.Deleted)
                {
                    if (MyUtility.Check.Empty(dg.Rows[i - 1]["BundleNo"]))
                    {
                        dg.Rows[i - 1].Delete();
                    }
                }
            }

            return base.ClickSaveBefore();
        }
    }
}
