using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// ToolStripMenuItem
    /// </summary>
    public partial class P01_MTLImport : Win.Subs.Base
    {
        private DataRow masterData;

        /// <summary>
        /// ToolStripMenuItem
        /// </summary>
        /// <param name="masterData">DataRow masterData</param>
        public P01_MTLImport(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable mTLImport;
            #region 組撈資料Sql
            string sqlCmd = string.Format(
                @"
declare @POID varchar(13), @OrderID varchar(13)
set @POID = iif('{0}'='',null,'{0}');
set @OrderID = '{1}';
with ExportSeq1 as(select distinct ID,Seq1 from Export_Detail WITH (NOLOCK) where PoID = @POID)
,SeqSum as(select distinct ID, (select Seq1+',' from ExportSeq1 where ID = e.ID for xml path('')) as Seq from ExportSeq1 e)
,ExportData as
(
    select distinct 'Import Schedule' as Type, e.ID,e.Eta,e.WhseArrival,e.Consignee,e.ShipMark,e.Vessel,
           CYCFS =(select cycfs from Export as a1 WITH (NOLOCK) where a1.ID =  e.MainExportID)
    from Export e WITH (NOLOCK) , Export_Detail ed WITH (NOLOCK) 
    where e.ID = ed.ID
    and ed.PoID = @POID
),tmpExport as(select ed.*,left(ss.Seq,len(ss.Seq)-1) as Seq from ExportData ed left join SeqSum ss on ed.ID = ss.ID)
,ReceiveSeq1 as(select distinct ID,Seq1 from Receiving_Detail WITH (NOLOCK) where PoID = @POID)
,RecSeqSum as(select distinct ID, (select Seq1+',' from ReceiveSeq1 where ID = e.ID for xml path('')) as Seq from ReceiveSeq1 e)
,ReceiveData as
(
    select distinct 'Material Receiving' as Type, r.ID,null as ETA,r.WhseArrival,'' as Consignee,'' as ShipMark,'' as Vessel,'' as CYCFS
    from Receiving r WITH (NOLOCK) , Receiving_Detail rd WITH (NOLOCK) 
    where r.ID = rd.ID
    and rd.PoID = @POID
    and r.Type = 'B'
),tmpReceive as(select rd.*,left(rss.Seq,len(rss.Seq)-1) as Seq from ReceiveData rd left join RecSeqSum rss on rd.ID = rss.ID)
,tmpLocalReceiving as
(
    select distinct 'Local Purchase  Receiving' as Type, l.Id, null as ETA, l.IssueDate as WhseArrival, '' as Consignee,'' as ShipMark,'' as Vessel,'' as CYCFS, '' as Seq
    from LocalReceiving l WITH (NOLOCK) , LocalReceiving_Detail ld WITH (NOLOCK) 
    where l.Id = ld.Id
    and ld.OrderId = @OrderID
)
select * from tmpExport
union all
select * from tmpReceive
union all
select * from tmpLocalReceiving
order by ID",
                this.masterData["POID"].ToString(),
                this.masterData["ID"].ToString());
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out mTLImport);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!!" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = mTLImport;

            // 設定Grid的顯示欄位
            this.gridImport.IsEditingReadOnly = true;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .Text("Type", header: "Type", width: Widths.AnsiChars(20))
                .Text("Id", header: "Working No.", width: Widths.AnsiChars(15))
                .Date("ETA", header: "ETA", width: Widths.AnsiChars(12))
                .Date("WhseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(12))
                .Text("Seq", header: "Main SEQ", width: Widths.AnsiChars(15))
                .Text("Consignee", header: "Consignee", width: Widths.AnsiChars(8))
                .Text("ShipMark", header: "Shipping Mark", width: Widths.AnsiChars(10))
                .Text("Vessel", header: "Vessel Name", width: Widths.AnsiChars(25))
                .Text("CYCFS", header: "Container Type", width: Widths.AnsiChars(3));
        }
    }
}
