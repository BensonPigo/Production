using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class P23 : Sci.Win.Tems.Input6
    {
        public P23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable queryDT;
            string querySql = @"select '' union select Id from SubProcess where IsRFIDProcess=1";
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(queryfors, 1, queryDT);
            queryfors.SelectedIndex = 0;
            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "";
                        break;
                    default:
                        this.DefaultWhere = string.Format("StartProcess = '{0}'", queryfors.SelectedValue);
                        break;
                }
                this.ReloadDatas();
            };
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
            //DataRow dr;
            ts1.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                DataRow drr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                if (e.FormattedValue.ToString() == drr["orderid"].ToString()) return;
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    CurrentDetailData["BundleNo"] = "";
                    CurrentDetailData["OrderID"] = "";
                    CurrentDetailData["SubprocessId"] = "";
                    CurrentDetailData["Patterncode"] = "";
                    CurrentDetailData["PatternDesc"] = "";
                    return;
                }

                if (e.FormattedValue.ToString() != "")
                {
                    if (MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from Bundle_Detail WITH (NOLOCK) where BundleNo = '{0}')", e.FormattedValue), null))
                    {
                        DataTable getdata;
                        string sqlcmd = string.Format(@"
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
where BundleNo='{0}'"
                            , e.FormattedValue);
                        DBProxy.Current.Select(null, sqlcmd, out getdata);

                        CurrentDetailData["BundleNo"] = e.FormattedValue;
                        CurrentDetailData["OrderID"] = getdata.Rows[0]["Orderid"].ToString();
                        CurrentDetailData["SubprocessId"] = getdata.Rows[0]["value"].ToString();
                        CurrentDetailData["Patterncode"] = getdata.Rows[0]["Patterncode"].ToString();
                        CurrentDetailData["PatternDesc"] = getdata.Rows[0]["PatternDesc"].ToString();

                    }
                    else
                    {
                        e.Cancel = true;
                        CurrentDetailData["BundleNo"] = "";
                        CurrentDetailData["OrderID"] = "";
                        CurrentDetailData["SubprocessId"] = "";
                        CurrentDetailData["Patterncode"] = "";
                        CurrentDetailData["PatternDesc"] = "";
                        MyUtility.Msg.WarningBox("Bundle# is not exist!!", "Data not found");
                        return;
                    }
                }
            };

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("BundleNo", header: "Bundle#", width: Widths.AnsiChars(12), settings: ts1, iseditingreadonly: false)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13))
                .Text("SubprocessId", header: "Artwork", width: Widths.AnsiChars(8), iseditingreadonly: false)
                .Text("Patterncode", header: "PTN Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("PatternDesc", header: "PTN Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true);
        }

        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
select 
	BTD.BundleNo
	,BTD.orderid
	,S.SubprocessId
	,BD.Patterncode
	,BD.PatternDesc
from BundleTrack_detail BTD 
LEFT JOIN Bundle_Detail BD ON BD.BundleNo = BTD.BundleNo
OUTER APPLY(
	SELECT SubprocessId = STUFF((
		SELECT CONCAT(',',SubprocessId )
		FROM Bundle_Detail_Art BDA
		WHERE BDA.Bundleno = BTD.BundleNo
		FOR XML PATH('')
	),1,1,'')
)S
WHERE BTD.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override bool ClickNew()
        {
            var frm = new Sci.Production.Subcon.P23_ImportBarcode();
            frm.ShowDialog(this);
            this.ReloadDatas();
            return true;
        }

        protected override bool ClickSaveBefore()
        {            
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                string getID = MyUtility.GetValue.GetID("TB", "BundleTrack", DateTime.Today, 5, "ID", null);
                if (MyUtility.Check.Empty(getID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = getID;
            }
            //檢核BundleTrack_detail.BundleNo是否已存在
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Seek(dr["BundleNo"].ToString(), "BundleTrack_detail", "BundleNo"))
                {
                    MyUtility.Msg.WarningBox(string.Format("Data is Duplicate!!!\r\n {0}", dr["BundleNo"].ToString()));
                    return false;
                }
            }
            return base.ClickSaveBefore();
        }

        protected override bool ClickPrint()
        {
            string sqlcmd = string.Format(@"
select
	BTD.orderid
	,style.StyleID
	,BD.BundleGroup
	,BTD.BundleNo
	,[Body/cut#] = concat(B.FabricPanelCode , '/' , B.Cutno)
	,bd.SizeCode
	,bd.Qty
	,Color = concat(b.Article,'',b.Colorid)
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
order by BTD.orderid", CurrentMaintain["ID"].ToString());

            DataTable print;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out print);
            if (!result)
            {
                ShowErr("Query data fail\r\n" + result.ToString());
                return false;
            }
            if (print.Rows.Count == 0)
            {
                ShowErr("Data no found!");
                return false;
            }
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_P23.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(print, "", "Subcon_P23.xltx", 3, true, null, objApp);// 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 2] = CurrentMaintain["ID"].ToString();
            objSheets.Cells[1, 5] = CurrentMaintain["StartProcess"].ToString();
            objSheets.Cells[1, 8] = MyUtility.Convert.GetDate(CurrentMaintain["IssueDate"]);
            string labb = MyUtility.GetValue.Lookup("abb", CurrentMaintain["EndSite"].ToString(), "LocalSupp", "ID");
            objSheets.Cells[2, 2] = CurrentMaintain["EndSite"].ToString() + "-" + labb;
            string fabb = MyUtility.GetValue.Lookup("abb", CurrentMaintain["StartSite"].ToString(), "Factory", "ID");
            objSheets.Cells[2, 5] = CurrentMaintain["StartSite"].ToString() + "-" + fabb;
            return true;
        }
    }
}
