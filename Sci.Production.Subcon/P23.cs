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
            :base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Id LIKE 'TB%'");
            lbl_queryfor.Text = "Sub Process";
            this.WorkAlias = "BundleTrack";
            this.GridAlias = "BundleTrack_detail";
            this.GridUniqueKey = "ID,BundleNo";
            this.DefaultOrder = "ID";
            this.ExpressQuery = true;
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
        protected override bool ClickNew()
        {
            var frm = new Sci.Production.Subcon.P23_ImportBarcode();
            frm.ShowDialog(this);
            this.ReloadDatas();
            return true;
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["IssueDate"] = DateTime.Now;
            CurrentMaintain["StartSite"] = Sci.Env.User.Factory;
        }
        protected override bool ClickSaveBefore()
        {
            //DataTable newDt = ((DataTable)detailgridbs.DataSource).Clone();
            //foreach (DataRow dtr in ((DataTable)detailgridbs.DataSource).Rows)
            //{
            //    string[] dtrLocation = dtr["location"].ToString().Split(',');
            //    dtrLocation = dtrLocation.Distinct().ToArray();

            //    if (dtrLocation.Length == 1)
            //    {
            //        DataRow newDr = newDt.NewRow();
            //        newDr.ItemArray = dtr.ItemArray;
            //        newDt.Rows.Add(newDr);
            //    }
            //    else
            //    {
            //        foreach (string location in dtrLocation)
            //        {
            //            DataRow newDr = newDt.NewRow();
            //            newDr.ItemArray = dtr.ItemArray;
            //            newDr["location"] = location;
            //            newDt.Rows.Add(newDr);
            //        }
            //    }
            //}
            //設定ID編碼
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
                if (MyUtility.Check.Seek(dr["BundleNo"].ToString(), "BundleTrack_detail", "BundleNo") == true)
                {
                    MyUtility.Msg.WarningBox(string.Format("Data is Duplicate!!!\r\n {0}", dr["BundleNo"].ToString()));
                    return false;
                }
            }
            return base.ClickSaveBefore();
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
                Select 
                 BundleTrack_detail.BundleNo
                ,BundleTrack_detail.orderid
                ,ss.SubprocessId
                ,Bundle_Detail.Patterncode
                ,Bundle_Detail.PatternDesc
                FROM BundleTrack_detail 
                INNER JOIN Bundle_Detail_Art ON BundleTrack_detail.BundleNo= Bundle_Detail_Art.Bundleno
                INNER JOIN Bundle_Detail ON BundleTrack_detail.BundleNo = Bundle_Detail.BundleNo
                OUTER APPLY(
	                SELECT SubprocessId = STUFF((
		                SELECT SubprocessId + '+' 
                from Bundle_Detail_Art
                where Bundleno=BundleTrack_detail.BundleNo
                FOR XML PATH('')
	                ),1,1,'')
	                )ss
                WHERE 1=1
                  --AND BundleTrack_detail.Id LIKE 'TB%'
                  AND BundleTrack_detail.ID = '{0}'
                ", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
            //DataRow dr;
            ts1.CellValidating += (s, e) =>
            {
               
                if (this.EditMode && e.FormattedValue.ToString() != "")
                {
                    if (MyUtility.Check.Seek(string.Format("select 1 where exists(select * from Bundle_Detail WITH (NOLOCK) where BundleNo = '{0}')", e.FormattedValue), null))
                    {
                       DataTable getdata ;
                        string sqlcmd = string.Format(@"
                        select B.Orderid , Artwork.value , BD.Patterncode , BD.PatternDesc
                        from Bundle_Detail BD WITH (NOLOCK)
                        left join Bundle B on B.ID=BD.Id
                        outer apply (
	                        select value = stuff ((	select '+' + subprocessid
							                        from (
								                        select distinct subprocessid
								                        from Bundle_Detail_Art bda WITH (NOLOCK)
								                        where bd.BundleNo = bda.Bundleno
							                        ) k
							                        for xml path('')
							                        ), 1, 1, '')
                        ) ArtWork
                        where BundleNo='{0}'
                        ", e.FormattedValue);
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
                .Text("BundleNo", header: "Bundle#", width: Widths.AnsiChars(12),settings: ts1, iseditingreadonly: false)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13))
                .Text("SubprocessId", header: "Artwork", width: Widths.AnsiChars(8), iseditingreadonly: false)
                .Text("Patterncode", header: "PTN Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("PatternDesc", header: "PTN Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true);

        }
    }
}
