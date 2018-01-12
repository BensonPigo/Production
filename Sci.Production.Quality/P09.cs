using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win.Tems;
using Sci.Data;

namespace Sci.Production.Quality
{
    public partial class P09 : Sci.Win.Tems.Input6
    {
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        // 撈出表身資料
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmd = string.Format(@"
select fd.id
    ,[Seq] = seq1+'-'+seq2
    ,[RefNo] = RefNo
    ,[Color] = fd.ColorID+'-'+c.Name
    ,[Supp] = fd.SuppID+'-'+s.NameEN
    ,[TestReport] = fd.TestReport
    ,[InspReport] = fd.InspReport
    ,[ContinuityCard] = fd.ContinuityCard
    ,[1stBulkDyelot] = fd.BulkDyelot
from FabricInspDoc_Detail fd WITH (NOLOCK)
left join color c WITH (NOLOCK) on fd.ColorID=c.ID
left join Supp s WITH (NOLOCK) on fd.SuppID=s.ID
where fd.id='{0}' order by seq1,seq2
", masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorCheckBoxColumnSettings chx1st = new DataGridViewGeneratorCheckBoxColumnSettings();

            chx1st.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                foreach (DataRow drt in DetailDatas)
                {
                    // color,supp,refno 如果都相同,就勾選相同的1stBulkDyelot
                    if (dr["color"] == drt["color"] && dr["Supp"] == drt["Supp"] && dr["Refno"] == drt["Refno"])
                    {                        
                        drt["1stBulkDyelot"] = true;
                    }
                }
            };

            Helper.Controls.Grid.Generator(this.detailgrid)
               .Text("Seq", header: "Seq#", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(20), iseditingreadonly: true)
               .Text("Color", header: "Color", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("Supp", header: "Supp", width: Widths.AnsiChars(25), iseditingreadonly: true)
               .CheckBox("TestReport", header: "Test Report", width: Widths.AnsiChars(5))
               .CheckBox("InspReport", header: "Inspection Report", width: Widths.AnsiChars(5))
               .CheckBox("ContinuityCard", header: "Continuity Card", width: Widths.AnsiChars(5))
               .CheckBox("1stBulkDyelot", header: "1stBulk Dyelot", width: Widths.AnsiChars(5), settings: chx1st);
        }

        protected override void ClickNewAfter()
        {
            CurrentMaintain["Status"] = "New";
            this.txtID.ReadOnly = false;
            base.ClickNewAfter();
        }

        private void txtID_Validating(object sender, CancelEventArgs e)
        {
            string spno = this.txtID.Text;
            // 檢查是否存在orders
            if (!MyUtility.Check.Seek($"select 1 from orders where poid='{spno}'"))
            {
                MyUtility.Msg.WarningBox($"{spno} dose not exist!");
                return;
            }

            // 檢查該ID是否存在FabircInspDoc
            if (MyUtility.Check.Seek($"select 1 from FabricInspDoc where id='{spno}'"))
            {
                MyUtility.Msg.WarningBox($"{spno} is already exists!");
                return;
            }

            DataTable dtDetail;
            DBProxy.Current.Select(null, $@"
SELECT DISTINCT 
[Seq] = psd.SEQ1+'-'+psd.SEQ2
,psd.SEQ1,psd.SEQ2
,[Refno] = psd.Refno
,[Color] = psd.colorid
,[Season] = O.seasonID
,[Supp] = ps.suppid
,[BulkDyelot] = ISNULL(OFD.BulkDyelot,0)
FROM PO_Supp_Detail psd
LEFT JOIN po_supp ps ON ps.id=psd.id AND ps.seq1=psd.seq1
LEFT JOIN ORDERS O ON O.ID=PSD.ID
outer apply
(SELECT DISTINCT FD.SeasonID, FD.Refno,FD.ColorID,FD.SuppID,FD.BulkDyelot FROM FabricInspDoc_Detail FD
WHERE FD.Refno=psd.Refno AND FD.seasonID
 =o.seasonID AND FD.colorid=psd.colorid AND  FD.suppid=ps.suppid AND FD.BulkDyelot=1
) 
AS OFD
WHERE  FabricType='F' and psd.id ='{spno}' 
and psd.SEQ1 <'70' and psd.SEQ1<>20 and psd.Junk=0
", out dtDetail);


        }

        protected override DualResult ClickSave()
        {
            this.txtID.ReadOnly = true;
            return base.ClickSave();
        }

        protected override void ClickUndo()
        {
            this.txtID.ReadOnly = true;
            base.ClickUndo();
        }
    }
}
