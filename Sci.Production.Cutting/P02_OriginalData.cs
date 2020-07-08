using Ict;
using Ict.Win;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Cutting
{
    public partial class P02_OriginalData : Win.Tems.QueryForm
    {
        DataRow CurrDataRow;

        public P02_OriginalData(DataRow currDataRow)
        {
            this.InitializeComponent();
            this.CurrDataRow = currDataRow;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region set grid
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6))
            .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), integer_places: 3)
            .Text("MarkerName", header: "Marker\r\nName", width: Widths.AnsiChars(5))
            .Text("Fabriccombo", header: "Fabric\r\nCombo", width: Widths.AnsiChars(2))
            .Text("FabricPanelCode", header: "Fab_Panel\r\nCode", width: Widths.AnsiChars(2))
            .Text("Article", header: "Article", width: Widths.AnsiChars(10))
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6))
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10))
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5)
            .Text("CutQty", header: "Total CutQty", width: Widths.AnsiChars(10))
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13))
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3))
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2))
            .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10))
            .Date("estcutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10))
            .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10))
            .Text("SpreadingNoID", header: "Spreading No", width: Widths.AnsiChars(2))
            .Text("Cutcellid", header: "Cut Cell", width: Widths.AnsiChars(2))
            .Text("Cutplanid", header: "Cutplan#", width: Widths.AnsiChars(13))
            .Date("actcutdate", header: "Act. Cut Date", width: Widths.AnsiChars(10))
            .Text("Edituser", header: "Edit Name", width: Widths.AnsiChars(10))
            .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(10))
            .Text("Adduser", header: "Add Name", width: Widths.AnsiChars(10))
            .DateTime("AddDate", header: "Add Date", width: Widths.AnsiChars(10))
            .Text("MarkerNo", header: "Apply #", width: Widths.AnsiChars(10))
            .Text("MarkerVersion", header: "Apply ver", width: Widths.AnsiChars(3))
            .Text("MarkerDownloadID", header: "Download ID", width: Widths.AnsiChars(25))
            .Text("EachconsMarkerNo", header: "EachCons Apply #", width: Widths.AnsiChars(10))
            .Text("EachconsMarkerVersion", header: "EachCons Apply ver", width: Widths.AnsiChars(3))
            .Text("EachconsMarkerDownloadID", header: "EachCons Download ID", width: Widths.AnsiChars(25))
            .MaskedText("ActCuttingPerimeterNew", "000Yd00\"00", "ActCutting Perimeter", width: Widths.AnsiChars(16))
            .MaskedText("StraightLengthNew", "000Yd00\"00", "StraightLength", width: Widths.AnsiChars(16))
            .MaskedText("CurvedLengthNew", "000Yd00\"00", "CurvedLength", width: Widths.AnsiChars(16))
            ;

            this.Helper.Controls.Grid.Generator(this.grid2)
            .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6))
            .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), integer_places: 3)
            .Text("MarkerName", header: "Marker\r\nName", width: Widths.AnsiChars(5))
            .Text("Fabriccombo", header: "Fabric\r\nCombo", width: Widths.AnsiChars(2))
            .Text("FabricPanelCode", header: "Fab_Panel\r\nCode", width: Widths.AnsiChars(2))
            .Text("Article", header: "Article", width: Widths.AnsiChars(10))
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6))
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10))
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5)
            .Text("CutQty", header: "Total CutQty", width: Widths.AnsiChars(10))
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13))
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3))
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2))
            .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10))
            .Date("estcutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10))
            .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10))
            .Text("SpreadingNoID", header: "Spreading No", width: Widths.AnsiChars(2))
            .Text("Cutcellid", header: "Cut Cell", width: Widths.AnsiChars(2))
            .Text("Cutplanid", header: "Cutplan#", width: Widths.AnsiChars(13))
            .Date("actcutdate", header: "Act. Cut Date", width: Widths.AnsiChars(10))
            .Text("Edituser", header: "Edit Name", width: Widths.AnsiChars(10))
            .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(10))
            .Text("Adduser", header: "Add Name", width: Widths.AnsiChars(10))
            .DateTime("AddDate", header: "Add Date", width: Widths.AnsiChars(10))
            .Text("MarkerNo", header: "Apply #", width: Widths.AnsiChars(10))
            .Text("MarkerVersion", header: "Apply ver", width: Widths.AnsiChars(3))
            .Text("MarkerDownloadID", header: "Download ID", width: Widths.AnsiChars(25))
            .Text("EachconsMarkerNo", header: "EachCons Apply #", width: Widths.AnsiChars(10))
            .Text("EachconsMarkerVersion", header: "EachCons Apply ver", width: Widths.AnsiChars(3))
            .Text("EachconsMarkerDownloadID", header: "EachCons Download ID", width: Widths.AnsiChars(25))
            .MaskedText("ActCuttingPerimeterNew", "000Yd00\"00", "ActCutting Perimeter", width: Widths.AnsiChars(16))
            .MaskedText("StraightLengthNew", "000Yd00\"00", "StraightLength", width: Widths.AnsiChars(16))
            .MaskedText("CurvedLengthNew", "000Yd00\"00", "CurvedLength", width: Widths.AnsiChars(16))
            ;

            this.Helper.Controls.Grid.Generator(this.grid3)
            .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(2))
            .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.AnsiChars(2))
            ;

            this.Helper.Controls.Grid.Generator(this.grid4)
            .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(2))
            .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.AnsiChars(2))
            ;

            this.Helper.Controls.Grid.Generator(this.grid5)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(5))
            .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(5), integer_places: 6)
            ;

            this.Helper.Controls.Grid.Generator(this.grid6)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(5))
            .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(5), integer_places: 6)
            ;

            this.Helper.Controls.Grid.Generator(this.grid7)
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(15))
            .Text("article", header: "article", width: Widths.AnsiChars(8))
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(4))
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(3), integer_places: 6)
            ;

            this.Helper.Controls.Grid.Generator(this.grid8)
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(15))
            .Text("article", header: "article", width: Widths.AnsiChars(8))
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(4))
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(3), integer_places: 6)
            ;

            #endregion

            this.Query();
        }

        private void Query()
        {
            DataTable wdtO;
            DataTable wdtN;
            DataTable pdtO;
            DataTable pdtN;
            DataTable sdtO;
            DataTable sdtN;
            DataTable ddtO;
            DataTable ddtN;
            DualResult result;
            string sqlcmd;
            #region WorkOrder Ori
            sqlcmd = $@"
Select
	a.*
	,article.article
	,SizeCode.SizeCode
	,CutQty.CutQty
	,FabricPanelCode.FabricPanelCode
	,PatternPanel.PatternPanel
	,fabeta.fabeta
	,Sewinline.Sewinline
	,actcutdate.actcutdate
	,adduser.adduser
	,edituser.edituser
	,totallayer =
	(
		Select sum(layer)
		From Order_EachCons ea WITH (NOLOCK) 
		inner join Order_EachCons_color ea_b WITH (NOLOCK) on ea.ukey = ea_b.order_eachConsUkey 
		Where ea.id = a.ID and ea.Markername = a.markername and ea_b.colorid = a.colorid
	)
	,multisize.multisize
	,Order_SizeCode_Seq.Order_SizeCode_Seq
	,SORT_NUM =0
    ,c.WeaveTypeID
    ,WeaveTypeID_SCIRefno = concat(c.WeaveTypeID, ' / ' , a.SCIRefno)
	,c.DescDetail
    ,c.Description
	,newkey = 0
	,MarkerLengthY = iif(CHARINDEX('Y',a.MarkerLength)>0, RIGHT(REPLICATE('0', 2) + CAST(substring(a.MarkerLength,1,CHARINDEX('Y',a.MarkerLength)-1) as VARCHAR), 2),'')
	,MarkerLengthE = substring(a.MarkerLength,CHARINDEX('Y',a.MarkerLength)+1,15) 
    ,shc = iif(isnull(shc.RefNo,'')='','','Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension')
    ,EachconsMarkerNo = e.markerNo
    ,EachconsMarkerDownloadID = e.MarkerDownloadID 
    ,EachconsMarkerVersion = e.MarkerVersion
    ,ActCuttingPerimeterNew = iif(CHARINDEX('Yd',a.ActCuttingPerimeter)<4,RIGHT(REPLICATE('0', 10) + a.ActCuttingPerimeter, 10),a.ActCuttingPerimeter)
	,StraightLengthNew = iif(CHARINDEX('Yd',a.StraightLength)<4,RIGHT(REPLICATE('0', 10) + a.StraightLength, 10),a.StraightLength)
	,CurvedLengthNew = iif(CHARINDEX('Yd',a.CurvedLength)<4,RIGHT(REPLICATE('0', 10) + a.CurvedLength, 10),a.CurvedLength)
	
	,SandCTime = concat(
		'Spr.:',cast(round(isnull(
			dbo.GetSpreadingTime(
					c.WeaveTypeID,
					a.Refno,
					iif(iif(isnull(fi.avgInQty,0)=0,1,round(a.Cons/fi.avgInQty,0))<1,1,iif(isnull(fi.avgInQty,0)=0,1,round(a.Cons/fi.avgInQty,0))),
					a.Layer,
					a.Cons,
					1
				)/60.0,0),2)as float),' mins,'
		,'Cut:',cast(round(isnull(
			dbo.GetCuttingTime(
					round(dbo.GetActualPerimeter(a.ActCuttingPerimeter),4),
					a.CutCellid,
					a.Layer,
					c.WeaveTypeID,
					a.cons
				)/60.0,0),2)as float),' mins,'
        ,'ActCuttingPerimeter: ' ,a.ActCuttingPerimeter
	)
    ,[NowWorkOrderUkey] = (SELECT Stuff((select concat( ',',WorkorderUkey)   from WorkOrderRevisedMarkerOriginalData_Detail where WorkorderUkeyRevisedMarkerOriginalUkey = a.Ukey FOR XML PATH('')),1,1,'') )
from WorkOrderRevisedMarkerOriginalData a WITH (NOLOCK)
inner join WorkOrderRevisedMarkerOriginalData_Detail wdd with (nolock) on a.Ukey = wdd.WorkorderUkeyRevisedMarkerOriginalUkey
left join fabric c WITH (NOLOCK) on c.SCIRefno = a.SCIRefno
left join dbo.order_Eachcons e WITH (NOLOCK) on e.Ukey = a.Order_EachconsUkey 
outer apply(select RefNo from ShrinkageConcern WITH (NOLOCK) where RefNo=a.RefNo and Junk=0) shc
outer apply
(
	select article = stuff(
	(
		Select distinct concat('/' ,Article)
		From dbo.WorkOrder_DistributeRevisedMarkerOriginalData b WITH (NOLOCK) 
		Where b.WorkOrderRevisedMarkerOriginalDataUkey = a.Ukey and b.article!=''
		For XML path('')
	),1,1,'')
) as article
outer apply
(
	select SizeCode = stuff(
	(
		Select concat(', ' , c.sizecode, '/ ', c.qty)
		From WorkOrder_SizeRatioRevisedMarkerOriginalData c WITH (NOLOCK) 
		Where c.WorkOrderRevisedMarkerOriginalDataUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as SizeCode
outer apply
(
	select CutQty = stuff(
	(
		Select concat(', ', c.sizecode, '/ ', c.qty * a.layer)
		From WorkOrder_SizeRatioRevisedMarkerOriginalData c WITH (NOLOCK) 
		Where  c.WorkOrderRevisedMarkerOriginalDataUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as CutQty
outer apply
(
	select FabricPanelCode = stuff(
	(
		Select concat('+ ', FabricPanelCode)
		From WorkOrder_PatternPanelRevisedMarkerOriginalData c WITH (NOLOCK) 
		Where c.WorkOrderRevisedMarkerOriginalDataUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as FabricPanelCode
outer apply
(
	select PatternPanel = stuff(
	(
		Select concat('+ ', PatternPanel)
		From WorkOrder_PatternPanelRevisedMarkerOriginalData c WITH (NOLOCK) 
		Where c.WorkOrderRevisedMarkerOriginalDataUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as PatternPanel
outer apply
(
	Select fabeta = iif(e.Complete=1, e.FinalETA, iif(e.Eta is not null, e.eta, iif(e.shipeta is not null, e.shipeta,e.finaletd)))
	From PO_Supp_Detail e WITH (NOLOCK) 
	Where e.id = (Select distinct poid from orders WITH (NOLOCK) where orders.cuttingsp = a.ID) and e.seq1 = a.seq1 and e.seq2 = a.seq2
) as fabeta
outer apply
(
	Select Sewinline = Min(sew.Inline)
	From SewingSchedule sew WITH (NOLOCK)
	inner join SewingSchedule_detail sew_b WITH (NOLOCK) on sew_b.id = sew.id
	inner join WorkOrder_DistributeRevisedMarkerOriginalData h WITH (NOLOCK) on h.orderid = sew_b.OrderID and h.Article = sew_b.Article and h.orderid = sew.orderid
	Where h.WorkOrderRevisedMarkerOriginalDataUkey = a.ukey 
) as Sewinline
outer apply
(
	Select actcutdate = iif(sum(cut_b.Layer) = a.Layer, Max(cut.cdate),null)
	From cuttingoutput cut WITH (NOLOCK) 
	inner join cuttingoutput_detail cut_b WITH (NOLOCK) on cut.id = cut_b.id
	Where cut_b.WorkOrderUkey = a.Ukey and cut.Status != 'New' 
)  as actcutdate
outer apply(Select adduser = Name From Pass1 ps WITH (NOLOCK) Where ps.id = a.addName) as adduser
outer apply(Select edituser = Name From Pass1 ps WITH (NOLOCK) Where ps.id = a.editName) as edituser
outer apply
(
	Select multisize = iif(count(size.sizecode)>1,2,1) 
	From WorkOrder_SizeRatioRevisedMarkerOriginalData size WITH (NOLOCK) 
	Where a.ukey = size.WorkOrderRevisedMarkerOriginalDataUkey
) as multisize
outer apply
(
	select Order_SizeCode_Seq = max(c.Seq)
	from WorkOrder_SizeRatioRevisedMarkerOriginalData b WITH (NOLOCK)
	left join Order_SizeCode c WITH (NOLOCK) on c.Id = b.ID and c.SizeCode = b.SizeCode
	where b.WorkOrderRevisedMarkerOriginalDataUkey = a.Ukey
) as Order_SizeCode_Seq
outer apply(
	select avgInQty = avg(fi.InQty)
	from PO_Supp_Detail psd with(nolock)
	left join FtyInventory fi with(nolock) on fi.POID = psd.ID and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
	where psd.ID = a.id and psd.SCIRefno = a.SCIRefno
	and fi.InQty is not null
) as fi
where wdd.workorderukey = '{this.CurrDataRow["ukey"]}'
";
            result = DBProxy.Current.Select(null, sqlcmd, out wdtO);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource1.DataSource = wdtO;
            if (wdtO.Rows.Count > 0)
            {
                this.displayTime.Text = MyUtility.Convert.GetString(wdtO.Rows[0]["SandCTime"]);
            }
            #endregion
            #region WorkOrder Now
            string inUkey = "''";
            if (wdtO.Rows.Count > 0)
            {
                inUkey = "'" + string.Join("','", MyUtility.Convert.GetString(wdtO.Rows[0]["NowWorkOrderUkey"]).Split(',')) + "'";
            }
            else
            {
                inUkey = $"'{this.CurrDataRow["ukey"]}'";
            }

            sqlcmd = $@"
Select
	a.*
	,article.article
	,SizeCode.SizeCode
	,CutQty.CutQty
	,FabricPanelCode.FabricPanelCode
	,PatternPanel.PatternPanel
	,fabeta.fabeta
	,Sewinline.Sewinline
	,actcutdate.actcutdate
	,adduser.adduser
	,edituser.edituser
	,totallayer =
	(
		Select sum(layer)
		From Order_EachCons ea WITH (NOLOCK) 
		inner join Order_EachCons_color ea_b WITH (NOLOCK) on ea.ukey = ea_b.order_eachConsUkey 
		Where ea.id = a.ID and ea.Markername = a.markername and ea_b.colorid = a.colorid
	)
	,multisize.multisize
	,Order_SizeCode_Seq.Order_SizeCode_Seq
	,SORT_NUM =0
    ,c.WeaveTypeID
    ,WeaveTypeID_SCIRefno = concat(c.WeaveTypeID, ' / ' , a.SCIRefno)
	,c.DescDetail
    ,c.Description
	,newkey = 0
	,MarkerLengthY = iif(CHARINDEX('Y',a.MarkerLength)>0, RIGHT(REPLICATE('0', 2) + CAST(substring(a.MarkerLength,1,CHARINDEX('Y',a.MarkerLength)-1) as VARCHAR), 2),'')
	,MarkerLengthE = substring(a.MarkerLength,CHARINDEX('Y',a.MarkerLength)+1,15) 
    ,shc = iif(isnull(shc.RefNo,'')='','','Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension')
    ,EachconsMarkerNo = e.markerNo
    ,EachconsMarkerDownloadID = e.MarkerDownloadID 
    ,EachconsMarkerVersion = e.MarkerVersion
    ,ActCuttingPerimeterNew = iif(CHARINDEX('Yd',a.ActCuttingPerimeter)<4,RIGHT(REPLICATE('0', 10) + a.ActCuttingPerimeter, 10),a.ActCuttingPerimeter)
	,StraightLengthNew = iif(CHARINDEX('Yd',a.StraightLength)<4,RIGHT(REPLICATE('0', 10) + a.StraightLength, 10),a.StraightLength)
	,CurvedLengthNew = iif(CHARINDEX('Yd',a.CurvedLength)<4,RIGHT(REPLICATE('0', 10) + a.CurvedLength, 10),a.CurvedLength)
	
	,SandCTime = concat(
		'Spr.:',cast(round(isnull(
			dbo.GetSpreadingTime(
					c.WeaveTypeID,
					a.Refno,
					iif(iif(isnull(fi.avgInQty,0)=0,1,round(a.Cons/fi.avgInQty,0))<1,1,iif(isnull(fi.avgInQty,0)=0,1,round(a.Cons/fi.avgInQty,0))),
					a.Layer,
					a.Cons,
					1
				)/60.0,0),2)as float),' mins,'
		,'Cut:',cast(round(isnull(
			dbo.GetCuttingTime(
					round(dbo.GetActualPerimeter(a.ActCuttingPerimeter),4),
					a.CutCellid,
					a.Layer,
					c.WeaveTypeID,
					a.cons
				)/60.0,0),2)as float),' mins,'
        ,'ActCuttingPerimeter: ',a.ActCuttingPerimeter
	)
from Workorder a WITH (NOLOCK)
left join fabric c WITH (NOLOCK) on c.SCIRefno = a.SCIRefno
left join dbo.order_Eachcons e WITH (NOLOCK) on e.Ukey = a.Order_EachconsUkey 
outer apply(select RefNo from ShrinkageConcern WITH (NOLOCK) where RefNo=a.RefNo and Junk=0) shc
outer apply
(
	select article = stuff(
	(
		Select distinct concat('/' ,Article)
		From dbo.WorkOrder_Distribute b WITH (NOLOCK) 
		Where b.workorderukey = a.Ukey and b.article!=''
		For XML path('')
	),1,1,'')
) as article
outer apply
(
	select SizeCode = stuff(
	(
		Select concat(', ' , c.sizecode, '/ ', c.qty)
		From WorkOrder_SizeRatio c WITH (NOLOCK) 
		Where c.WorkOrderUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as SizeCode
outer apply
(
	select CutQty = stuff(
	(
		Select concat(', ', c.sizecode, '/ ', c.qty * a.layer)
		From WorkOrder_SizeRatio c WITH (NOLOCK) 
		Where  c.WorkOrderUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as CutQty
outer apply
(
	select FabricPanelCode = stuff(
	(
		Select concat('+ ', FabricPanelCode)
		From WorkOrder_PatternPanel c WITH (NOLOCK) 
		Where c.WorkOrderUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as FabricPanelCode
outer apply
(
	select PatternPanel = stuff(
	(
		Select concat('+ ', PatternPanel)
		From WorkOrder_PatternPanel c WITH (NOLOCK) 
		Where c.WorkOrderUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as PatternPanel
outer apply
(
	Select fabeta = iif(e.Complete=1, e.FinalETA, iif(e.Eta is not null, e.eta, iif(e.shipeta is not null, e.shipeta,e.finaletd)))
	From PO_Supp_Detail e WITH (NOLOCK) 
	Where e.id = (Select distinct poid from orders WITH (NOLOCK) where orders.cuttingsp = a.ID) and e.seq1 = a.seq1 and e.seq2 = a.seq2
) as fabeta
outer apply
(
	Select Sewinline = Min(sew.Inline)
	From SewingSchedule sew WITH (NOLOCK)
	inner join SewingSchedule_detail sew_b WITH (NOLOCK) on sew_b.id = sew.id
	inner join WorkOrder_Distribute h WITH (NOLOCK) on h.orderid = sew_b.OrderID and h.Article = sew_b.Article and h.orderid = sew.orderid
	Where h.WorkOrderUkey = a.ukey 
) as Sewinline
outer apply
(
	Select actcutdate = iif(sum(cut_b.Layer) = a.Layer, Max(cut.cdate),null)
	From cuttingoutput cut WITH (NOLOCK) 
	inner join cuttingoutput_detail cut_b WITH (NOLOCK) on cut.id = cut_b.id
	Where cut_b.workorderukey = a.Ukey and cut.Status != 'New' 
)  as actcutdate
outer apply(Select adduser = Name From Pass1 ps WITH (NOLOCK) Where ps.id = a.addName) as adduser
outer apply(Select edituser = Name From Pass1 ps WITH (NOLOCK) Where ps.id = a.editName) as edituser
outer apply
(
	Select multisize = iif(count(size.sizecode)>1,2,1) 
	From WorkOrder_SizeRatio size WITH (NOLOCK) 
	Where a.ukey = size.WorkOrderUkey
) as multisize
outer apply
(
	select Order_SizeCode_Seq = max(c.Seq)
	from WorkOrder_SizeRatio b WITH (NOLOCK)
	left join Order_SizeCode c WITH (NOLOCK) on c.Id = b.ID and c.SizeCode = b.SizeCode
	where b.WorkOrderUkey = a.Ukey
) as Order_SizeCode_Seq
outer apply(
	select avgInQty = avg(fi.InQty)
	from PO_Supp_Detail psd with(nolock)
	left join FtyInventory fi with(nolock) on fi.POID = psd.ID and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
	where psd.ID = a.id and psd.SCIRefno = a.SCIRefno
	and fi.InQty is not null
) as fi
where a.ukey in ({inUkey})
";
            result = DBProxy.Current.Select(null, sqlcmd, out wdtN);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource2.DataSource = wdtN;
            #endregion

            #region PatternPanel
            sqlcmd = $@"
select p.* from WorkOrder_PatternPanelRevisedMarkerOriginalData p  WITH (NOLOCK)
inner join WorkOrderRevisedMarkerOriginalData_Detail w WITH (NOLOCK) on w.WorkorderUkeyRevisedMarkerOriginalUkey = p.WorkOrderRevisedMarkerOriginalDataUkey
where w.workorderukey = '{this.CurrDataRow["ukey"]}' ";
            result = DBProxy.Current.Select(null, sqlcmd, out pdtO);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource3.DataSource = pdtO;

            sqlcmd = $@"
select * from WorkOrder_PatternPanel  WITH (NOLOCK)
where WorkOrderUkey in ({inUkey}) ";
            result = DBProxy.Current.Select(null, sqlcmd, out pdtN);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource4.DataSource = pdtN;
            #endregion

            #region SizeRatio
            sqlcmd = $@"
Select * 
from WorkOrder_SizeRatioRevisedMarkerOriginalData s WITH (NOLOCK) 
inner join WorkOrderRevisedMarkerOriginalData_Detail w WITH (NOLOCK) on w.WorkorderUkeyRevisedMarkerOriginalUkey = s.WorkOrderRevisedMarkerOriginalDataUkey
where w.workorderukey = '{this.CurrDataRow["ukey"]}' ";
            result = DBProxy.Current.Select(null, sqlcmd, out sdtO);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource5.DataSource = sdtO;

            sqlcmd = $@"Select * from Workorder_SizeRatio WITH (NOLOCK) where WorkOrderUkey in ({inUkey})";
            result = DBProxy.Current.Select(null, sqlcmd, out sdtN);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource6.DataSource = sdtN;
            #endregion

            #region Distribute
            sqlcmd = $@"
Select * 
from WorkOrder_DistributeRevisedMarkerOriginalData d WITH (NOLOCK) 
inner join WorkOrderRevisedMarkerOriginalData_Detail w WITH (NOLOCK) on w.WorkorderUkeyRevisedMarkerOriginalUkey = d.WorkOrderRevisedMarkerOriginalDataUkey
where w.workorderukey = '{this.CurrDataRow["ukey"]}'";
            result = DBProxy.Current.Select(null, sqlcmd, out ddtO);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource7.DataSource = ddtO;

            sqlcmd = $@"Select * From Workorder_distribute WITH (NOLOCK) where WorkOrderUkey in ({inUkey}) ";
            result = DBProxy.Current.Select(null, sqlcmd, out ddtN);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource8.DataSource = ddtN;
            #endregion

            #region Relations
            DataSet dataSet = new DataSet("All");
            wdtN.TableName = "Master";
            pdtN.TableName = "P";
            sdtN.TableName = "S";
            ddtN.TableName = "D";
            dataSet.Tables.Add(wdtN);
            dataSet.Tables.Add(pdtN);
            dataSet.Tables.Add(sdtN);
            dataSet.Tables.Add(ddtN);
            DataRelation relation = new DataRelation(
                "rel1",
                new DataColumn[] { wdtN.Columns["ukey"] },
                new DataColumn[] { pdtN.Columns["WorkOrderUkey"] });
            dataSet.Relations.Add(relation);

            DataRelation relation2 = new DataRelation(
                "rel2",
                new DataColumn[] { wdtN.Columns["ukey"] },
                new DataColumn[] { sdtN.Columns["WorkOrderUkey"] });
            dataSet.Relations.Add(relation2);

            DataRelation relation3 = new DataRelation(
                "rel3",
                new DataColumn[] { wdtN.Columns["ukey"] },
                new DataColumn[] { ddtN.Columns["WorkOrderUkey"] });
            dataSet.Relations.Add(relation3);

            this.listControlBindingSource2.DataSource = dataSet;
            this.listControlBindingSource2.DataMember = "Master";
            this.listControlBindingSource4.DataSource = this.listControlBindingSource2;
            this.listControlBindingSource4.DataMember = "rel1";
            this.listControlBindingSource6.DataSource = this.listControlBindingSource2;
            this.listControlBindingSource6.DataMember = "rel2";
            this.listControlBindingSource8.DataSource = this.listControlBindingSource2;
            this.listControlBindingSource8.DataMember = "rel3";

            this.displayTimeN.DataBindings.Add("Text", this.listControlBindingSource2, "SandCTime", true, DataSourceUpdateMode.OnPropertyChanged);
            #endregion
        }
    }
}
