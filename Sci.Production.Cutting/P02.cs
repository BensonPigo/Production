using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Linq;
using Sci.Utility;
using Sci.Win.Tems;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sci.Production.Automation;
using Sci.Production.Prg;

namespace Sci.Production.Cutting
{
    public partial class P02 : Sci.Win.Tems.Input8
    {
        #region
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;

        private DataTable sizeratioTb, layersTb, distqtyTb, qtybreakTb, sizeGroup, spTb, artTb, PatternPanelTb, PatternPanelTb_Copy;
        private DataTable chksize;
        private DataRow drTEMP;  //紀錄目前表身選擇的資料，避免按列印時模組會重LOAD資料，導致永遠只能印到第一筆資料
        private string Poid;
        private Sci.Win.UI.BindingSource2 bindingSource2 = new Win.UI.BindingSource2();

        Ict.Win.UI.DataGridViewTextBoxColumn col_Markername;
        Ict.Win.UI.DataGridViewTextBoxColumn col_sp;
        Ict.Win.UI.DataGridViewTextBoxColumn col_seq1;
        Ict.Win.UI.DataGridViewTextBoxColumn col_seq2;
        Ict.Win.UI.DataGridViewTextBoxColumn col_cutcell;
        Ict.Win.UI.DataGridViewTextBoxColumn col_SpreadingNoID;
        Ict.Win.UI.DataGridViewTextBoxColumn col_cutno;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_layer;
        Ict.Win.UI.DataGridViewDateBoxColumn col_wketa;        
        Ict.Win.UI.DataGridViewDateBoxColumn col_estcutdate;
        Ict.Win.UI.DataGridViewTextBoxColumn col_cutref;
        Ict.Win.UI.DataGridViewTextBoxColumn col_sizeRatio_size;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_sizeRatio_qty;
        Ict.Win.UI.DataGridViewTextBoxColumn col_dist_size;
        Ict.Win.UI.DataGridViewTextBoxColumn col_dist_article;
        Ict.Win.UI.DataGridViewTextBoxColumn col_dist_sp;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_dist_qty;
        Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_ActCuttingPerimeterNew;
        Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_StraightLengthNew;
        Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_CurvedLengthNew;
        Ict.Win.UI.DataGridViewTextBoxColumn col_shift;
        #endregion

        public P02(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            InitializeComponent();
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("FabricPanelCode", "Pattern Panel");
            comboBox1_RowSource.Add("SP", "SP");
            comboBox1_RowSource.Add("Cut#", "Cut#");
            comboBox1_RowSource.Add("Ref#", "Ref#");
            comboBox1_RowSource.Add("Cutplan#", "Cutplan#");
            comboBox1_RowSource.Add("MarkerName", "MarkerName");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            txtCutCell.MDivisionID = Sci.Env.User.Keyword;

            DoSubForm = new P02_PatternPanel();
            /*
             *設定Binding Source for Text
            */
            this.displayMarkerName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerName", true));
            this.displayColor.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "colorid", true));
            this.numUnitCons.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSource2, "Conspc", true));
            this.txtCutCell.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "CutCellid", true));
            this.numCons.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSource2, "Cons", true));
            this.txtFabricCombo.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "FabricCombo", true));
            this.txtFabricPanelCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "FabricPanelCode", true));
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "Description", true));
            this.displayFabricType_Refno.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MtlTypeID_SCIRefno", true));

            this.displayCutplanNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "Cutplanid", true));
            this.displayTotalCutQty.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "CutQty", true));
            this.displayTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "SandCTime", true));
            this.numMarkerLengthY.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerLengthY", true));
            this.txtMarkerLengthE.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerLengthE", true));
            this.txtMarkerLength.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerLength", true));
            this.txtPatternPanel.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "PatternPanel", true));
            this.lbshc.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "shc", true));
            this.txtBoxMarkerNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerNo", true));
            
            sizeratioMenuStrip.Enabled = this.EditMode;
            distributeMenuStrip.Enabled = this.EditMode;

            if (history == "0")
            {
                this.Text = "P02.Cutting Work Order";
                this.IsSupportEdit = true;
                this.DefaultFilter = string.Format("mDivisionid = '{0}' and WorkType is not null and WorkType != '' and Finished = 0", keyWord);
            }
            else
            {
                this.Text = "P02.Cutting Work Order(History)";
                this.IsSupportEdit = false;
                this.DefaultFilter = string.Format("mDivisionid = '{0}' and WorkType is not null and WorkType != '' and Finished = 1", keyWord);
            }
            detailgrid.Click += detailgrid_Click;
        }

        void detailgrid_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgrid.CurrentCell)) return;
            detailgrid.CurrentCell = detailgrid[detailgrid.CurrentCell.ColumnIndex, detailgrid.CurrentCell.RowIndex];
            detailgrid.BeginEdit(true);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable queryDT;
            string querySql = string.Format(@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory  WITH (NOLOCK)
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
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
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", queryfors.SelectedValue);
                        break;
                }
                this.ReloadDatas();
            };

            this.detailgrid.SelectionChanged += Detailgrid_SelectionChanged;
        }

        private void Detailgrid_SelectionChanged(object sender, EventArgs e)
        {
            if (this.detailgrid.GetSelectedRowIndex() <= 0)
            {
                this.btnAdditionalrevisedmarker.Enabled = false;
            }
            else
            {
                this.btnAdditionalrevisedmarker.Enabled = true;
            }

        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            #region 主Table 左邊grid
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmdsql = string.Format(@"
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
    ,MtlTypeID_SCIRefno = concat(c.WeaveTypeID, ' / ' , a.SCIRefno)
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
					iif(iif(isnull(fi.avgInQty,0)=0,1,round(iif(isnull(a.CutRef,'')='',a.Cons,sum(a.Cons)over(partition by a.CutRef,a.MDivisionId))/fi.avgInQty,0))<1,1,
                        iif(isnull(fi.avgInQty,0)=0,1,round(iif(isnull(a.CutRef,'')='',a.Cons,sum(a.Cons)over(partition by a.CutRef,a.MDivisionId))/fi.avgInQty,0))
                        ),
					iif(isnull(a.CutRef,'')='',a.Layer,sum(a.Layer)over(partition by a.CutRef,a.MDivisionId)),
					iif(isnull(a.CutRef,'')='',a.Cons,sum(a.Cons)over(partition by a.CutRef,a.MDivisionId)),
					1
				)/60.0,0),2)as float),' mins, '
		
		,'Cut:',cast(round(isnull(
			dbo.GetCuttingTime(
					round(dbo.GetActualPerimeter(iif(a.ActCuttingPerimeter not like '%yd%','0',a.ActCuttingPerimeter)),4),
					a.CutCellid,
					iif(isnull(a.CutRef,'')='',a.Layer,sum(a.Layer)over(partition by a.CutRef,a.MDivisionId)),
					c.WeaveTypeID,
					iif(isnull(a.CutRef,'')='',a.Cons,sum(a.Cons)over(partition by a.CutRef,a.MDivisionId))
				)/60.0,0),2)as float),' mins'
	)--同裁次若ActCuttingPerimeter週長若不一樣就是有問題, 所以ActCuttingPerimeter,直接用當前這筆

    ,isbyAdditionalRevisedMarker = cast(0 as int)
    ,fromukey = a.ukey
    ,CuttingLayer = iif(isnull(cs.CuttingLayer,0) = 0, 100 ,cs.CuttingLayer)
from Workorder a WITH (NOLOCK)
left join fabric c WITH (NOLOCK) on c.SCIRefno = a.SCIRefno
left join Construction cs WITH (NOLOCK) on cs.ID = ConstructionID
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
where a.id = '{0}'            
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            #endregion

            #region SizeRatio
            cmdsql = string.Format("Select *,0 as newKey from Workorder_SizeRatio WITH (NOLOCK) where id = '{0}'", masterID);
            DualResult dr = DBProxy.Current.Select(null, cmdsql, out sizeratioTb);
            if (!dr) ShowErr(cmdsql, dr);
            #endregion

            #region layer
            cmdsql = string.Format(@"
Select a.MarkerName,a.Colorid,a.Order_EachconsUkey
	,layer = isnull(sum(a.layer),0)
    ,TotallayerUkey =             
    (
        Select isnull(sum(c.layer),0) as TL
	    from Order_EachCons b WITH (NOLOCK) 
		inner join Order_EachCons_Color c WITH (NOLOCK) on c.id = b.id and c.Order_EachConsUkey = b.ukey
	    where b.id = a.id and b.Markername = a.MarkerName and c.Colorid = a.Colorid and b.Ukey = a.Order_EachconsUkey 
    )
    ,TotallayerMarker =
    (
        Select isnull(sum(c.layer),0) as TL2
	    from Order_EachCons b WITH (NOLOCK) 
		inner join Order_EachCons_Color c WITH (NOLOCK) on b.id = c.id and c.Order_EachConsUkey = b.ukey
	    where b.id = a.id and b.Markername = a.MarkerName and c.Colorid = a.Colorid
    )
From WorkOrder a WITH (NOLOCK) 
Where a.id = '{0}' 
group by a.MarkerName,a.Colorid,a.Order_EachconsUkey,a.id 
Order by a.MarkerName,a.Colorid,a.Order_EachconsUkey
                ", masterID);
            dr = DBProxy.Current.Select(null, cmdsql, out layersTb);
            if (!dr) ShowErr(cmdsql, dr);
            #endregion

            #region distqtyTb / PatternPanelTb / PatternPanelTb_Copy
            cmdsql = string.Format(@"Select *,0 as newKey From Workorder_distribute WITH (NOLOCK) Where id='{0}'", masterID);
            dr = DBProxy.Current.Select(null, cmdsql, out distqtyTb);
            if (!dr) ShowErr(cmdsql, dr);

            //cmdsql = string.Format(@"Select *,0 as newKey From Workorder_PatternPanel WITH (NOLOCK) Where id='{0}'", masterID);
            //dr = DBProxy.Current.Select(null, cmdsql, out PatternPanelTb);
            cmdsql = string.Format(@"Select *,0 as newKey From Workorder_PatternPanel WITH (NOLOCK) Where id='{0}'", masterID);
            dr = DBProxy.Current.Select(null, cmdsql, out PatternPanelTb);
            if (!dr) ShowErr(cmdsql, dr);

            cmdsql = @"Select *,0 as newKey From Workorder_PatternPanel WITH (NOLOCK) Where 1=0";
            dr = DBProxy.Current.Select(null, cmdsql, out PatternPanelTb_Copy);

            if (!dr) ShowErr(cmdsql, dr);
            #endregion

            #region 建立要使用右鍵開窗Grid
            string settbsql = string.Format(@"
select wo.id,FabricCombo,wd.article,wd.SizeCode,wd.OrderID,sum(qty) qty --min(qty) as minQty
into #tmp
from Workorder wo WITH (NOLOCK) 
inner join workorder_Distribute wd WITH (NOLOCK) on wo.ukey = wd.workorderukey 
inner join Order_fabriccode ofb WITH (NOLOCK) on wo.Id  = ofb.id and wo.FabricPanelCode = ofb.FabricPanelCode 
Where wo.id = '{0}'
group by wo.id,FabricCombo,wd.article,wd.SizeCode,wd.OrderID

Select a.id, a.article, a.sizecode, a.qty,  isnull(balc.minQty-a.qty,0) as balance, c.workorder_Distribute_Qty
From Order_Qty a WITH (NOLOCK)
inner join orders b WITH (NOLOCK) on a.id = b.id 
outer apply
(
    Select workorder_Distribute_Qty = Sum(Qty) 
    from workorder_Distribute WD WITH (NOLOCK) 
    where WD.ID='{0}' and WD.OrderID=a.id and WD.Article=a.Article and WD.SizeCode=a.SizeCode
) c
outer apply (
select min(qty) as minQty from (
	select qty --min(qty) as minQty
	from #tmp t
	Where t.id = b.cuttingsp and t.article =a.Article and t.SizeCode=a.SizeCode and t.OrderID=a.ID
	) a
) balc
Where b.cuttingsp ='{0}'
order by id,article,sizecode
drop table #tmp"
                , masterID);
            DualResult gridResult = DBProxy.Current.Select(null, settbsql, out qtybreakTb);
            sizeGroup = qtybreakTb.DefaultView.ToTable(true, "sizecode");
            artTb = qtybreakTb.DefaultView.ToTable(true, new string[] { "article" ,"ID"});
            spTb = qtybreakTb.DefaultView.ToTable(true, "id");

            //// 若訂單數量超過裁切分配數量，則更新balance
            //foreach (DataRow dr2 in qtybreakTb.Rows)
            //{
            //    if (MyUtility.Convert.GetDecimal(dr2["qty"]) > MyUtility.Convert.GetDecimal(dr2["workorder_Distribute_Qty"]))
            //    {
            //        dr2["balance"] = MyUtility.Convert.GetDecimal(dr2["qty"]) - MyUtility.Convert.GetDecimal(dr2["workorder_Distribute_Qty"]);
            //    }
            //}
            //用來檢查size是否存在
            string sqlsizechk = string.Format(@"
select distinct w.SizeCode
from Workorder_SizeRatio w  WITH (NOLOCK) 
where w.ID = '{0}'", masterID);
            DBProxy.Current.Select(null, sqlsizechk, out chksize);
            #endregion

            return base.OnDetailSelectCommandPrepare(e);
        }

        // 撈第三層資料
        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "0" : MyUtility.Convert.GetString(e.Detail["UKey"]);
            this.SubDetailSelectCommand = string.Format(@"
select * from WorkOrder_PatternPanel  WITH (NOLOCK)
where WorkOrderUkey={0}", masterID);

            return base.OnSubDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            gridSizeRatio.DataSource = sizeratiobs;
            sizeratiobs.DataSource = sizeratioTb;
            distributebs.DataSource = distqtyTb;
            gridDistributetoSPNo.DataSource = distributebs;
            qtybreakds.DataSource = qtybreakTb;
            gridQtyBreakdown.DataSource = qtybreakds;

            sizeratioTb.DefaultView.RowFilter = "";
            qtybreakTb.DefaultView.RowFilter = "";
            OnDetailGridRowChanged();

            DataRow orderdr;
            MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) where id='{0}'", CurrentMaintain["ID"]), out orderdr);

            txtStyle.Text = orderdr == null ? "" : orderdr["Styleid"].ToString();
            txtLine.Text = orderdr == null ? "" : orderdr["SewLine"].ToString();
            string maxcutrefCmd = string.Format("Select Max(Cutref) from workorder WITH (NOLOCK) where mDivisionid = '{0}'", keyWord);
            textbox_LastCutRef.Text = MyUtility.GetValue.Lookup(maxcutrefCmd);
            comboBox1.Enabled = !EditMode;  //Sorting於編輯模式時不可選取

            foreach (DataRow dr in DetailDatas) dr["Article"] = dr["Article"].ToString().TrimEnd('/');
            sorting(comboBox1.Text);
            this.detailgrid.SelectRowTo(0);
            this.detailgrid.AutoResizeColumns();

            col_shift.Width = 66;
            col_wketa.Width = 77;
            btnQuantityBreakdown.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;

            #region 檢查MarkerNo ,MarkerVersion ,MarkerDownloadID是否與Order_Eachcons不同
            if (this.DetailDatas.Where(s => !s["MarkerNo"].Equals(s["EachconsMarkerNo"]) ||
                                           !s["MarkerVersion"].Equals(s["EachconsMarkerVersion"]) ||
                                           !s["MarkerDownloadID"].Equals(s["EachconsMarkerDownloadID"])).Count() > 0)
                downloadid_Text.Visible = true;
            else
                downloadid_Text.Visible = false;
            #endregion

            this.Poid = MyUtility.GetValue.Lookup(string.Format("Select poid from orders WITH (NOLOCK) where id ='{0}'", CurrentMaintain["ID"]));
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings cutno = new DataGridViewGeneratorTextColumnSettings();
            cutno.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                Regex NumberPattern = new Regex("^[0-9]{1,6}$");
                if (!NumberPattern.IsMatch(e.FormattedValue.ToString())) { dr["Cutno"] = DBNull.Value; }
            };
            DataGridViewGeneratorDateColumnSettings EstCutDate = new DataGridViewGeneratorDateColumnSettings();
            EstCutDate.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                if (!(MyUtility.Check.Empty(e.FormattedValue)))
                {
                    DataRow dr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (e.FormattedValue.ToString() == dr["estcutdate"].ToString()) { return; }
                    if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(e.FormattedValue)) > 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("[Est. Cut Date] can not be passed !!");
                    }
                }
            };
            DataGridViewGeneratorDateColumnSettings WKETA = new DataGridViewGeneratorDateColumnSettings();
            WKETA.EditingMouseDown += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    P02_WKETA item = new P02_WKETA(dr);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    if (result == DialogResult.No) { dr["WKETA"] = DBNull.Value; }
                    if (result == DialogResult.Yes) { dr["WKETA"] = itemx.WKETA; }
                    dr.EndEdit();
                }
            };


            DataGridViewGeneratorNumericColumnSettings breakqty = new DataGridViewGeneratorNumericColumnSettings();
            breakqty.EditingMouseDoubleClick += (s, e) =>
            {
                gridValid();
                detailgrid.ValidateControl();
                Sci.Production.Cutting.P01_Cutpartchecksummary callNextForm = new Sci.Production.Cutting.P01_Cutpartchecksummary(CurrentMaintain["ID"].ToString());
                callNextForm.ShowDialog(this);
            };

            #region ActCuttingPerimeter,StraightLength,CurvedLength 處理遮罩字串, 存檔字串要包含遮罩字元
            DataGridViewGeneratorMaskedTextColumnSettings ActCuttingPerimeter = new DataGridViewGeneratorMaskedTextColumnSettings();
            ActCuttingPerimeter.CellValidating += (s, e) =>
            {
                if (!EditMode) return;
                if (e.RowIndex == -1) return;

                setMaskString(e.FormattedValue.ToString().Replace(" ", "0"), "ActCuttingPerimeter");
            };
            DataGridViewGeneratorMaskedTextColumnSettings StraightLength = new DataGridViewGeneratorMaskedTextColumnSettings();
            StraightLength.CellValidating += (s, e) =>
            {
                if (!EditMode) return;
                if (e.RowIndex == -1) return;

                setMaskString(e.FormattedValue.ToString().Replace(" ", "0"), "StraightLength");
            };
            DataGridViewGeneratorMaskedTextColumnSettings CurvedLength = new DataGridViewGeneratorMaskedTextColumnSettings();
            CurvedLength.CellValidating += (s, e) =>
            {
                if (!EditMode) return;
                if (e.RowIndex == -1) return;

                setMaskString(e.FormattedValue.ToString().Replace(" ", "0"), "CurvedLength");
            };
            #endregion

            cellDropDownList dropdown = (cellDropDownList)cellDropDownList.GetGridCell("Pms_WorkOrderShift");
            DataGridViewGeneratorTextColumnSettings col_Shift = cellTextDropDownList.GetGridCell("Pms_WorkOrderShift");

            #region set grid
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6)).Get(out col_cutref)
                .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(5), settings: cutno).Get(out col_cutno)
                .Text("MarkerName", header: "Marker\r\nName", width: Widths.AnsiChars(5)).Get(out col_Markername)
                .Text("Fabriccombo", header: "Fabric\r\nCombo", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("FabricPanelCode", header: "Fab_Panel\r\nCode", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, maximum: 99999).Get(out col_layer)
                .Text("CutQty", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(13)).Get(out col_sp)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3)).Get(out col_seq1)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2)).Get(out col_seq2)
                .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("WKETA", header: "WK ETA", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: WKETA).Get(out col_wketa)
                .Date("estcutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10), settings: EstCutDate).Get(out col_estcutdate)
                .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SpreadingNoID", header: "Spreading No", width: Widths.AnsiChars(2)).Get(out col_SpreadingNoID)
                .Text("Cutcellid", header: "Cut Cell", width: Widths.AnsiChars(2)).Get(out col_cutcell)
                .Text("Shift", header: "Shift", width: Widths.AnsiChars(20), settings: col_Shift).Get(out col_shift)
                .Text("Cutplanid", header: "Cutplan#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("actcutdate", header: "Act. Cut Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Edituser", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Adduser", header: "Add Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UKey", header: "Key", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MarkerNo", header: "Apply #", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MarkerVersion", header: "Apply ver", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("MarkerDownloadID", header: "Download ID", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("EachconsMarkerNo", header: "EachCons Apply #", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("EachconsMarkerVersion", header: "EachCons Apply ver", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("EachconsMarkerDownloadID", header: "EachCons Download ID", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .MaskedText("ActCuttingPerimeterNew", "000Yd00\"00", "ActCutting Perimeter", width: Widths.AnsiChars(16), settings: ActCuttingPerimeter).Get(out col_ActCuttingPerimeterNew)
                .MaskedText("StraightLengthNew", "000Yd00\"00", "StraightLength", width: Widths.AnsiChars(16), settings: StraightLength).Get(out col_StraightLengthNew)
                .MaskedText("CurvedLengthNew", "000Yd00\"00", "CurvedLength", width: Widths.AnsiChars(16), settings: CurvedLength).Get(out col_CurvedLengthNew)
                ;
            Helper.Controls.Grid.Generator(this.gridSizeRatio)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(5)).Get(out col_sizeRatio_size)
                .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(5), integer_places: 6, maximum: 999999, minimum: 0).Get(out col_sizeRatio_qty);

            Helper.Controls.Grid.Generator(this.gridDistributetoSPNo)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(15)).Get(out col_dist_sp)
                .Text("article", header: "Article", width: Widths.AnsiChars(8)).Get(out col_dist_article)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(4)).Get(out col_dist_size)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(3), integer_places: 6, maximum: 999999, minimum: 0).Get(out col_dist_qty);

            Helper.Controls.Grid.Generator(this.gridQtyBreakdown)
                .Text("id", header: "SP#", width: Widths.AnsiChars(13))
                .Text("article", header: "Article", width: Widths.AnsiChars(7))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(3))
                .Numeric("Qty", header: "Order \nQty", width: Widths.AnsiChars(3), integer_places: 6, maximum: 999999, minimum: 0)
                .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(5), integer_places: 6, maximum: 999999, minimum: 0, settings: breakqty);

            this.detailgrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8F);
            #endregion

            changeeditable();
        }

        private void setMaskString(string eventString, string colName)
        {
            eventString = eventString.PadRight(7, '0');
            eventString = eventString.Substring(0, 3) + "Yd" + eventString.Substring(3, 2) + "\"" + eventString.Substring(5, 2);
            this.CurrentDetailData[colName] = eventString.TrimStart('0');
            this.CurrentDetailData[colName+"New"] = eventString;
        }

        private void changeeditable()// Grid Cell 物件設定
        {
            #region maingrid
            #region cutref
            col_cutref.EditingControlShowing += (s, e) =>
            {
                ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
            };

            col_cutref.EditingKeyDown += (s, e) =>
            {
                if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) && MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]))
                {
                    e.EditingControl.Text = "";
                }

            };
            #endregion
            #region cutno
            col_cutno.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_cutno.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            }; 

            #endregion
            #region markname
            col_Markername.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_Markername.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            #endregion
            #region Layers
            col_layer.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;

            };
            col_layer.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_layer.CellValidating += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1 || e.FormattedValue == null)
                    return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["layer"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                CurrentDetailData["layer"] = newvalue;

                int sumlayer = 0;
                if (MyUtility.Check.Empty(CurrentDetailData["Order_EachConsUkey"]))
                {
                    object O_sumLayer = ((DataTable)detailgridbs.DataSource).Compute("sum(layer)", string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]));
                    if (!O_sumLayer.Empty()) sumlayer = Convert.ToInt32(O_sumLayer);

                    DataRow[] drar = layersTb.Select(string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]));
                    if (drar.Length != 0) numBalanceLayer.Value = sumlayer - Convert.ToInt32(drar[0]["TotalLayerMarker"]);
                }
                else
                {
                    object O_sumLayer = ((DataTable)detailgridbs.DataSource).Compute("sum(layer)", string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", CurrentDetailData["Order_EachConsUkey"], CurrentDetailData["Colorid"]));
                    if (!O_sumLayer.Empty()) sumlayer = Convert.ToInt32(O_sumLayer);

                    DataRow[] drar = layersTb.Select(string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", CurrentDetailData["Order_EachConsUkey"], CurrentDetailData["Colorid"]));
                    if (drar.Length != 0) numBalanceLayer.Value = sumlayer - Convert.ToInt32(drar[0]["TotalLayerUkey"]);
                }
                cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);

                int newsumQty = 0;
                if (sizeratiobs.DataSource != null)
                {
                    object Sq = ((DataTable)sizeratiobs.DataSource).DefaultView.ToTable().Compute("SUM(Qty)", "");
                    if (!Sq.Empty()) newsumQty = MyUtility.Convert.GetInt(CurrentDetailData["layer"]) * Convert.ToInt32(Sq);
                }

                //重算DistributeqQty
                int oldttlqty = (int)numTotalDistributionQty.Value;
                int diff = newsumQty - oldttlqty;
                numTotalDistributionQty.Value = newsumQty;

                if (diff > 0)
                {
                    DataRow[] sizeR = sizeratioTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = '{1}'", CurrentDetailData["Ukey"].ToString(), CurrentDetailData["NewKey"].ToString()));
                    foreach (DataRow drr in sizeR)
                    {
                        updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), drr["SizeCode"].ToString());
                    }
                }
                if (diff < 0)
                {
                    string sizetmp = "";
                    DataRow[] dist = distqtyTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = '{1}' ", CurrentDetailData["Ukey"].ToString(), CurrentDetailData["NewKey"].ToString()));

                    foreach (DataRow drr in dist)
                    {
                        DataRow[] sizeR = sizeratioTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = '{1}' and SizeCode = '{2}'", CurrentDetailData["Ukey"].ToString(), CurrentDetailData["NewKey"].ToString(), drr["SizeCode"].ToString()));
                        if (sizetmp == drr["SizeCode"].ToString())//size是否和前一筆相同，判斷是否有重複的size
                        {
                            drr["Qty"] = 0;
                        }
                        else
                        {
                            drr["Qty"] = MyUtility.Convert.GetInt(sizeR[0]["Qty"]) * MyUtility.Convert.GetInt(newvalue);
                        }
                        sizetmp = drr["SizeCode"].ToString();
                        if (drr["OrderID"].ToString() == "EXCESS")
                        {
                            drr["Qty"] = 0;
                        }
                    }
                }
                cal_Cons(true, true);
            };
            #endregion
            #region SP
            col_sp.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode && CurrentMaintain["WorkType"].ToString() != "1") ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_sp.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode || CurrentMaintain["WorkType"].ToString() == "1")
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_sp.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {

                    if (CurrentMaintain["WorkType"].ToString() == "1" || !MyUtility.Check.Empty(CurrentDetailData["Cutplanid"])) return;
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    SelectItem sele;

                    sele = new SelectItem(spTb, "ID", "15@300,400", dr["OrderID"].ToString(), columndecimals: "50");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_sp.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["orderid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                DataRow[] seledr = spTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["orderid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SP> : {0} data not found!", newvalue));
                    return;
                }

                dr["orderid"] = newvalue;
                dr.EndEdit();
            };
            #endregion
            #region SEQ1
            col_seq1.EditingMouseDown += (s, e) =>
            {
                if (!MyUtility.Check.Empty(CurrentDetailData["Cutplanid"])) return;
                P02_PublicFunction.Seq1EditingMouseDown(s, e, this, this.detailgrid, this.Poid);
            };
            col_seq1.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
            };
            col_seq1.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_seq1.CellValidating += (s, e) =>
            {
                P02_PublicFunction.Seq1CellValidating(s, e, this, this.detailgrid, this.Poid);
            };
            #endregion
            #region SEQ2
            col_seq2.EditingMouseDown += (s, e) =>
            {
                if (!MyUtility.Check.Empty(CurrentDetailData["Cutplanid"])) return;
                P02_PublicFunction.Seq2EditingMouseDown(s, e, this, this.detailgrid, this.Poid);
            };
            col_seq2.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_seq2.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_seq2.CellValidating += (s, e) =>
            {
                P02_PublicFunction.Seq2CellValidating(s, e, this, this.detailgrid, this.Poid);
            };
            #endregion
            #region estcutdate
            col_estcutdate.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.DateBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true;

            };
            col_estcutdate.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            #endregion
            #region col_wketa
            col_wketa.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                { ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true; ((Ict.Win.UI.DateBox)e.Control).Enabled = true; }
                else { ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true; ((Ict.Win.UI.DateBox)e.Control).Enabled = false; }

            };
            col_wketa.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            #endregion
            #region cutcell
            col_cutcell.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_cutcell.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            bool cellchk = true;
            col_cutcell.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    // 若 cutref != empty 則不可編輯
                    if (!MyUtility.Check.Empty(dr["Cutplanid"])) return;
                    SelectItem sele;
                    DataTable cellTb;
                    DBProxy.Current.Select(null, string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", keyWord), out cellTb);
                    sele = new SelectItem(cellTb, "ID", "10@300,300", dr["CutCellid"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["cutCellid"] = sele.GetSelectedString();
                    dr.EndEdit();

                    checkCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                    cellchk = false;
                }
            };
            col_cutcell.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty()) return;

                string oldvalue = dr["cutcellid"].ToString();
                string newvalue = e.FormattedValue.ToString();

                if (oldvalue == newvalue) return;

                DataTable cellTb;
                DBProxy.Current.Select(null, string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", keyWord), out cellTb);

                DataRow[] seledr = cellTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["cutCellid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Cell> : {0} data not found!", newvalue));
                    return;
                }

                dr["cutCellid"] = newvalue;
                if (!cellchk)
                {
                    cellchk = true;
                }
                else
                {
                    checkCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                }
                dr.EndEdit();
            };
            #endregion
            #region Shift
            col_shift.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_shift.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            #endregion
            #region col_SpreadingNoID
            col_SpreadingNoID.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_SpreadingNoID.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            bool col_SpreadingNoIDchk = true;
            col_SpreadingNoID.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    // 若 cutref != empty 則不可編輯
                    if (!MyUtility.Check.Empty(dr["Cutplanid"])) return;
                    SelectItem sele;
                    DataTable SpreadingNoIDTb;
                    DBProxy.Current.Select(null, $"Select id,CutCell = CutCellID from SpreadingNo WITH (NOLOCK) where mDivisionid = '{keyWord}' and junk=0", out SpreadingNoIDTb);

                    sele = new SelectItem(SpreadingNoIDTb, "ID,CutCell", "10@400,300", dr["SpreadingNoID"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["SpreadingNoID"] = sele.GetSelectedString();
                    if (!MyUtility.Check.Empty(sele.GetSelecteds()[0]["CutCell"]))
                    {
                        dr["cutCellid"] = sele.GetSelecteds()[0]["CutCell"];
                        checkCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                    }
                    dr.EndEdit();

                    col_SpreadingNoIDchk = false;
                }
            };
            col_SpreadingNoID.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return; 
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty()) return;
                string oldvalue = dr["SpreadingNoID"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                DataRow SpreadingNodr;
                string sqlSpreading = $"Select CutCellID from SpreadingNo WITH (NOLOCK) where mDivisionid = '{keyWord}' and  id = '{newvalue}' and junk=0";
                if (!MyUtility.Check.Seek(sqlSpreading, out SpreadingNodr))
                {
                    dr["SpreadingNoID"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SpreadingNo> : {0} data not found!", newvalue));
                    return;
                }

                dr["SpreadingNoID"] = newvalue;

                if (!MyUtility.Check.Empty(SpreadingNodr["CutCellID"]))
                    dr["cutCellid"] = SpreadingNodr["CutCellID"];

                if (!col_SpreadingNoIDchk)
                {
                    col_SpreadingNoIDchk = true;
                }
                else
                {
                    checkCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                }
                dr.EndEdit();
            };
            #endregion
            #region col_ActCuttingPerimeterNew
            col_ActCuttingPerimeterNew.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = true;

            };
            col_ActCuttingPerimeterNew.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            #endregion
            #region col_StraightLengthNew
            col_StraightLengthNew.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = true;

            };
            col_StraightLengthNew.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            #endregion
            #region col_CurvedLengthNew
            col_CurvedLengthNew.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = true;

            };
            col_CurvedLengthNew.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            #endregion
            #endregion
            #region SizeRatio
            col_sizeRatio_size.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode || CurrentDetailData["Cutplanid"].ToString() != "") { return; }
                    DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
                    SelectItem sele;

                    string oldvalue = dr["SizeCode"].ToString();

                    sele = new SelectItem(sizeGroup, "SizeCode", "15@300,300", dr["SizeCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                    string newvalue = sele.GetSelectedString();
                    dr["SizeCode"] = newvalue;
                    dr.EndEdit();

                    redetailsize(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                    cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);
                    totalDisQty();
                    DataRow[] distdrs = distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' ", Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), oldvalue));
                    foreach (DataRow disdr in distdrs)
                    {
                        disdr["SizeCode"] = newvalue;
                    }

                }
            };
            col_sizeRatio_size.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_sizeRatio_size.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
                string oldvalue = dr["SizeCode"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow[] chkrow = chksize.Select(string.Format("SizeCode = '{0}'", newvalue));
                if (chkrow.Length == 0)
                {
                    e.Cancel = true;
                    this.ShowInfo(string.Format("Size <{0}> not found", newvalue));
                    gridSizeRatio.EditingControl.Select();
                    return;
                }
                dr["SizeCode"] = newvalue;
                dr.EndEdit();

                redetailsize(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);
                totalDisQty();
                DataRow[] distdrs = distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' ", Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), oldvalue));
                foreach (DataRow disdr in distdrs)
                {
                    disdr["SizeCode"] = newvalue;
                }
            };
            col_sizeRatio_qty.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;

            };
            col_sizeRatio_qty.CellValidating += (s, e) =>
            {
                // Parent form 若是非編輯狀態就 return 
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
                int oldvalue = Convert.ToInt32(dr["Qty"]);
                int newvalue = Convert.ToInt32(e.FormattedValue);
                if (oldvalue == newvalue) return;
                dr["Qty"] = newvalue;
                dr.EndEdit();
                cal_Cons(true, false);
                //cal_TotalCutQty(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                redetailsize(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);

                updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());
                totalDisQty();
            };
            #endregion
            #region Distribute
            col_dist_sp.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    if (CurrentDetailData == null) return;
                    DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString().ToUpper() == "EXCESS" || CurrentDetailData["Cutplanid"].ToString() != "") return;
                    sele = new SelectItem(spTb, "ID", "15@300,400", dr["OrderID"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_dist_sp.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS") ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_dist_sp.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["orderid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue || newvalue.ToUpper() == "EXCESS") return;

                DataRow[] seledr = spTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["orderid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SP> : {0} data not found!", newvalue));
                    return;
                }
                if (!MyUtility.Check.Empty(dr["SizeCode"]) && !MyUtility.Check.Empty(dr["Article"]))
                {
                    seledr = qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", newvalue, dr["SizeCode"], dr["Article"]));
                    if (seledr.Length == 0)
                    {
                        dr["OrderID"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2}", dr["OrderID"], newvalue, dr["Article"]));
                        return;
                    }
                }
                dr["orderid"] = newvalue;
                dr.EndEdit();
                totalDisQty();

            };

            col_dist_size.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    if (CurrentDetailData == null) return;
                    DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString().ToUpper() == "EXCESS" || CurrentDetailData["Cutplanid"].ToString() != "") return;
                    DataTable srdt = ((DataTable)sizeratiobs.DataSource).DefaultView.ToTable();
                    DataTable sizeGroup2;
                    MyUtility.Tool.ProcessWithDatatable(srdt, "sizecode", "Select distinct SizeCode from #tmp", out sizeGroup2);
                    sele = new SelectItem(sizeGroup2, "SizeCode", "15@300,300", dr["SizeCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_dist_size.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS")
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_dist_size.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["SizeCode"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow[] seledr = sizeGroup.Select(string.Format("SizeCode='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["SizeCode"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Size> : {0} data not found!", newvalue));
                    return;
                }
                if (!MyUtility.Check.Empty(dr["OrderID"]) && !MyUtility.Check.Empty(dr["Article"]))
                {
                    seledr = qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", dr["OrderID"], newvalue, dr["Article"]));
                    if (seledr.Length == 0)
                    {
                        dr["SizeCode"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2} data not found", dr["OrderID"], dr["Article"], newvalue));
                        return;
                    }
                }
                dr["SizeCode"] = newvalue;
                dr.EndEdit();

                updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());
                totalDisQty();

            };
            col_dist_article.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    if (CurrentDetailData == null) return;
                    DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString().ToUpper() == "EXCESS" || CurrentDetailData["Cutplanid"].ToString() != "") return;
                    sele = new SelectItem(artTb, "article", "15@300,300", dr["Article"].ToString(), false, "," , gridFilter: $"ID = '{dr["OrderID"].ToString()}'");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_dist_article.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS") ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_dist_article.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能

                if (e.RowIndex == -1) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["Article"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow[] seledr = artTb.Select(string.Format("Article='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["Article"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Article> : {0} data not found!", newvalue));
                    return;
                }
                if (!MyUtility.Check.Empty(dr["OrderID"]) && !MyUtility.Check.Empty(dr["SizeCode"]))
                {
                    seledr = qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", dr["OrderID"], dr["SizeCode"], newvalue));
                    if (seledr.Length == 0)
                    {
                        dr["Article"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2}", dr["OrderID"], newvalue, dr["SizeCode"]));
                        return;
                    }
                }
                dr["Article"] = newvalue;
                dr.EndEdit();
            };
            //依據Cutplanid&OrderID來設定是否能修改
            col_dist_qty.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS") ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;
            };
            //重算qty
            col_dist_qty.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["Qty"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                dr["Qty"] = newvalue;
                dr.EndEdit();

                updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());
                //計算完EXCESS正確後再Total計算
                totalDisQty();
            };
            #endregion
        }

        //重組detailgrid的size
        private void redetailsize(int workorderukey, int newkey)
        {
            DataRow[] dsr = sizeratioTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1}", workorderukey, newkey));
            string sizeqty = "";
            foreach (DataRow dsrr in dsr)
            {
                sizeqty += ", " + dsrr["SizeCode"] + "/ " + dsrr["Qty"];
            }
            sizeqty = sizeqty.Substring(1);
            DataRow[] dr = ((DataTable)detailgridbs.DataSource).Select(string.Format("Ukey={0} and NewKey = {1}", workorderukey, newkey));
            dr[0]["SizeCode"] = sizeqty;
        }

        //計算Excess
        private void updateExcess(int workorderukey, int newkey, string sizecode)
        {
            //gridValid();
            DataRow[] sizeview = sizeratioTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode = '{2}'", workorderukey, newkey, sizecode));
            foreach (DataRow dr in sizeview)
            {
                int now_distqty, org_distqty;
                object comput = distqtyTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode = '{2}'", workorderukey, newkey, dr["SizeCode"]));
                if (comput == DBNull.Value) now_distqty = 0;
                else org_distqty = Convert.ToInt32(comput);

                now_distqty = Convert.ToInt32(dr["Qty"]) * Convert.ToInt32(MyUtility.Check.Empty(CurrentDetailData["Layer"]) ? 0 : CurrentDetailData["Layer"]);
                DataRow[] distdr = distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' ", workorderukey, newkey, dr["SizeCode"]));
                if (distdr.Length == 0)
                {
                    DataRow ndr = distqtyTb.NewRow();
                    ndr["WorkOrderUKey"] = workorderukey;
                    ndr["NewKey"] = newkey;
                    ndr["OrderID"] = "EXCESS";
                    ndr["SizeCode"] = dr["SizeCode"];
                    ndr["Qty"] = now_distqty;
                    distqtyTb.Rows.Add(ndr);
                }
                else
                {
                    foreach (DataRow dr2 in distdr)
                    {
                        if (dr2["OrderID"].ToString().Trim().ToUpper() != "EXCESS")
                        {
                            now_distqty = now_distqty - Convert.ToInt32(dr2["Qty"]);
                        }
                    }
                    DataRow[] exdr = distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' and OrderID ='EXCESS' ", workorderukey, newkey, dr["SizeCode"]));
                    if (exdr.Length == 0 && now_distqty > 0)
                    {
                        DataRow ndr = distqtyTb.NewRow();
                        ndr["WorkOrderUKey"] = workorderukey;
                        ndr["NewKey"] = newkey;
                        ndr["OrderID"] = "EXCESS";
                        ndr["SizeCode"] = dr["SizeCode"];
                        ndr["Qty"] = now_distqty;
                        distqtyTb.Rows.Add(ndr);
                    }
                    else if(exdr.Length > 0)
                    {
                        exdr[0]["Qty"] = now_distqty < 0 ? 0 : now_distqty;
                    }
                    this.gridDistributetoSPNo.EndEdit();
                }
            }
        }

        private void totalDisQty()
        {
            if (distributebs.DataSource != null)
            {
                object Sq = ((DataTable)distributebs.DataSource).DefaultView.ToTable().Compute("SUM(Qty)", "");
                if (!Sq.Empty()) numTotalDistributionQty.Value = Convert.ToInt32(Sq);
            }
        }

        private void gridValid()
        {
            gridSizeRatio.ValidateControl();
            gridDistributetoSPNo.ValidateControl();
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (sizeratioMenuStrip != null) sizeratioMenuStrip.Enabled = this.EditMode;
            if (distributeMenuStrip != null) distributeMenuStrip.Enabled = this.EditMode;
        }

        //1394: CUTTING_P02_Cutting Work Order。KEEP當前的資料。
        protected override void DoPrint()
        {
            drTEMP = this.CurrentDetailData;
            base.DoPrint();
        }

        protected override void OnDetailGridRowChanged()
        {
            gridValid();
            base.OnDetailGridRowChanged();
            //Binding 資料來源
            if (CurrentDetailData == null) return;
            bindingSource2.SetRow(this.CurrentDetailData);
            DataRow fabdr;

            #region 根據左邊Grid Filter 右邊資訊
            if (!MyUtility.Check.Empty(CurrentDetailData["Ukey"]))
            {

                sizeratioTb.DefaultView.RowFilter = string.Format("Workorderukey = '{0}'", CurrentDetailData["Ukey"]);
                distqtyTb.DefaultView.RowFilter = string.Format("Workorderukey = '{0}' ", CurrentDetailData["Ukey"]);

                gridDistributetoSPNo.SelectRowTo(0);
                for (int i = 0; i < gridDistributetoSPNo.Rows.Count; i++)
                {
                    if (gridDistributetoSPNo.Rows[i].Cells["OrderID"].Value.Equals(CurrentDetailData["OrderID"]))
                    {
                        gridDistributetoSPNo.SelectRowTo(i);

                        break;
                    }
                }



            }
            if (MyUtility.Convert.GetString(CurrentDetailData["Ukey"]) == "0")
            {
                string ukey = CurrentDetailData["Ukey"].ToString();
                string newkey = CurrentDetailData["newkey"].ToString();
                sizeratioTb.DefaultView.RowFilter = string.Format("Workorderukey = {0} and newkey = {1}", ukey, newkey);
                distqtyTb.DefaultView.RowFilter = string.Format("Workorderukey = {0} and newkey = {1}", ukey, newkey);
            }
            #endregion

            #region Total Dist
            totalDisQty();
            #endregion

            int sumlayer = 0;
            int sumlayer2 = 0;

            if (MyUtility.Check.Empty(CurrentDetailData["Order_EachConsUkey"]))
            {
                DataRow[] AA = ((DataTable)detailgridbs.DataSource).Select(string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]));
                DataRow[] B = layersTb.Select(string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]));
                foreach (DataRow l in AA)
                {
                    sumlayer += MyUtility.Convert.GetInt(l["layer"]);
                }
                foreach (DataRow l in B)
                {
                    sumlayer2 += MyUtility.Convert.GetInt(l["layer"]);
                }
            }
            else
            {
                DataRow[] AA = ((DataTable)detailgridbs.DataSource).Select(string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", CurrentDetailData["Order_EachConsUkey"], CurrentDetailData["Colorid"]));
                DataRow[] B = layersTb.Select(string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", CurrentDetailData["Order_EachConsUkey"], CurrentDetailData["Colorid"]));
                foreach (DataRow l in AA)
                {
                    sumlayer += MyUtility.Convert.GetInt(l["layer"]);
                }
                foreach (DataRow l in B)
                {
                    sumlayer2 += MyUtility.Convert.GetInt(l["layer"]);
                }
            }

            int order_EachConsTemp;
            if (CurrentDetailData["Order_EachConsUkey"] == DBNull.Value)
            {//old rule
                order_EachConsTemp = 0;
                string selectcondition = string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]);
                DataRow[] laydr = layersTb.Select(selectcondition);
                if (laydr.Length == 0)
                {
                    numTotalLayer.Value = 0;
                    numBalanceLayer.Value = 0;
                }
                else
                {
                    numTotalLayer.Value = (decimal)laydr[0]["TotalLayerMarker"];
                    numBalanceLayer.Value = sumlayer - (decimal)laydr[0]["TotalLayerMarker"];
                }
            }
            else
            { //New rule
                order_EachConsTemp = Convert.ToInt32(CurrentDetailData["Order_EachConsUkey"]);
                string selectcondition = string.Format("Order_EachConsUkey = {0} and  Colorid = '{1}'", order_EachConsTemp, CurrentDetailData["Colorid"]);
                DataRow[] laydr = layersTb.Select(selectcondition);
                if (laydr.Length == 0)
                {
                    numTotalLayer.Value = 0;
                    numBalanceLayer.Value = 0;
                }
                else
                {
                    numTotalLayer.Value = (decimal)laydr[0]["TotalLayerUkey"];
                    numBalanceLayer.Value = sumlayer - (decimal)laydr[0]["TotalLayerUkey"];
                }
            }

            #region 判斷可否開放修改
            if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode)
            {
                numMarkerLengthY.ReadOnly = false;
                txtMarkerLengthE.ReadOnly = false;
                numUnitCons.ReadOnly = false;
                txtCutCell.ReadOnly = false;
                txtFabricCombo.ReadOnly = false;
                txtFabricPanelCode.ReadOnly = false;
                sizeratioMenuStrip.Enabled = true;
                distributeMenuStrip.Enabled = true;
                txtBoxMarkerNo.IsSupportEditMode = true;
                txtBoxMarkerNo.ReadOnly = false;
            }
            else
            {
                numMarkerLengthY.ReadOnly = true;
                txtMarkerLengthE.ReadOnly = true;
                numUnitCons.ReadOnly = true;
                txtCutCell.ReadOnly = true;
                txtFabricCombo.ReadOnly = true;
                txtFabricPanelCode.ReadOnly = true;
                sizeratioMenuStrip.Enabled = false;
                distributeMenuStrip.Enabled = false;
                txtBoxMarkerNo.IsSupportEditMode = false;
                txtBoxMarkerNo.ReadOnly = true;
            }
            #endregion
            totalDisQty();
            this.gridSizeRatio.AutoResizeColumns();
            this.gridQtyBreakdown.AutoResizeColumns();
            //抓到當前編輯的cell
            if (MyUtility.Check.Empty(detailgrid.CurrentCell)) return;
            detailgrid.CurrentCell = detailgrid[detailgrid.CurrentCell.ColumnIndex, detailgrid.CurrentCell.RowIndex];
            detailgrid.BeginEdit(true);
        }

        //程式產生的BindingSource 必須自行Dispose, 以節省資源
        protected override void OnFormDispose()
        {
            base.OnFormDispose();
            bindingSource2.Dispose();
        }
        
        private void sorting(string sort)
        {
            detailgrid.ValidateControl();
            if (CurrentDetailData == null) return;
            DataView dv = ((DataTable)detailgridbs.DataSource).DefaultView;
            switch (sort)
            {
                case "SP":
                    dv.Sort = "SORT_NUM,Orderid,FabricCombo,Ukey";
                    break;
                case "Cut#":
                    dv.Sort = "SORT_NUM,cutno,FabricCombo,Ukey";
                    break;
                case "Ref#":
                    dv.Sort = "SORT_NUM,cutref,Ukey";
                    break;
                case "Cutplan#":
                    dv.Sort = "SORT_NUM,Cutplanid,Ukey";
                    break;
                case "MarkerName":
                    dv.Sort = "SORT_NUM,FabricCombo,Cutno,Markername,estcutdate,Ukey";
                    break;
                default:
                    dv.Sort = "SORT_NUM,PatternPanel,multisize DESC,Article,Order_SizeCode_Seq DESC,MarkerName,Ukey";
                    break;
            }
        }

        private void AutoRef_Click(object sender, EventArgs e)
        {
            gridValid();
            detailgrid.ValidateControl();
            #region 變更先將同d,Cutref, FabricPanelCode, CutNo, MarkerName, estcutdate 且有cutref,Cuno無cutplanid 的cutref值找出來Group by→cutref 會相同
            string cmdsql = string.Format(@"
            SELECT isnull(Cutref,'') as cutref, isnull(FabricCombo,'') as FabricCombo, CutNo,
            isnull(MarkerName,'') as MarkerName, estcutdate
            FROM Workorder WITH (NOLOCK) 
            WHERE (cutplanid is null or cutplanid ='') AND (CutNo is not null )
                    AND (cutref is not null and cutref !='') and id = '{0}' and mDivisionid = '{1}'
            GROUP BY Cutref, FabricCombo, CutNo, MarkerName, estcutdate", CurrentMaintain["ID"], keyWord);
            DataTable cutreftb;
            DualResult cutrefresult = DBProxy.Current.Select(null, cmdsql, out cutreftb);
            if (!cutrefresult)
            {
                ShowErr(cmdsql, cutrefresult);
                return;
            }
            #endregion
            DataTable workordertmp;
            cmdsql = string.Format(
                @"Select * 
                  From workorder WITH (NOLOCK) 
                  Where (CutNo is not null ) and (cutref is null or cutref ='') 
                    and (estcutdate is not null and estcutdate !='' )
                    and (CutCellid is not null and CutCellid !='' )
                    and id = '{0}' and mDivisionid = '{1}'
                order by FabricCombo,cutno", CurrentMaintain["ID"], keyWord);//找出空的cutref
            cutrefresult = DBProxy.Current.Select(null, cmdsql, out workordertmp);
            if (!cutrefresult)
            {
                ShowErr(cmdsql, cutrefresult);
                return;
            }
            DataTable workorderdt = ((DataTable)detailgridbs.DataSource);
            string maxref = MyUtility.GetValue.Lookup("Select isnull(Max(cutref),'000000') from Workorder WITH (NOLOCK)"); //找最大Cutref
            if (MyUtility.Check.Empty(maxref))
            {
                maxref = "000000";
            }
            string updatecutref = "", newcutref = "";
            updatecutref = @"

DECLARE @chk tinyint
SET @chk = 0
Begin Transaction [Trans_Name] -- Trans_Name 
";
            foreach (DataRow dr in workordertmp.Rows) //寫入空的Cutref
            {
                DataRow[] findrow = cutreftb.Select(string.Format(@"MarkerName = '{0}' and FabricCombo = '{1}' and Cutno = {2} and estcutdate = '{3}' ", dr["MarkerName"], dr["FabricCombo"], dr["Cutno"], dr["estcutdate"]));
                if (findrow.Length != 0) //若有找到同馬克同部位同Cutno同裁剪日就寫入同cutref
                {
                    newcutref = findrow[0]["cutref"].ToString();
                }
                else
                {
                    maxref = MyUtility.GetValue.GetNextValue(maxref, 0);
                    DataRow newdr = cutreftb.NewRow();
                    newdr["MarkerName"] = dr["MarkerName"] == null ? string.Empty : dr["MarkerName"];
                    newdr["FabricCombo"] = dr["FabricCombo"] == null ? string.Empty : dr["FabricCombo"];
                    newdr["Cutno"] = dr["Cutno"];
                    newdr["estcutdate"] = dr["estcutdate"] == null ? string.Empty : dr["estcutdate"];
                    newdr["cutref"] = maxref;
                    cutreftb.Rows.Add(newdr);
                    newcutref = maxref;
                }
                updatecutref = updatecutref + string.Format($@"
    if (select COUNT(1) from Workorder WITH (NOLOCK) where cutref = '{newcutref}' and mDivisionid = '{keyWord}' and id != '{CurrentMaintain["id"]}')>0
	begin
		RAISERROR ('Duplicate Cutref. Please redo Auto Ref#',12, 1) 
		Rollback Transaction [Trans_Name] -- 復原所有操作所造成的變更
	end
    Update Workorder set cutref = '{newcutref}' where ukey = '{dr["ukey"]}';");
            }
            updatecutref = updatecutref + @"
    IF @@Error <> 0 BEGIN SET @chk = 1 END
IF @chk <> 0 BEGIN -- 若是新增資料發生錯誤
    Rollback Transaction [Trans_Name] -- 復原所有操作所造成的變更
END
ELSE BEGIN
    Commit Transaction [Trans_Name] -- 提交所有操作所造成的變更
END";

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                if (!MyUtility.Check.Empty(updatecutref))
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatecutref)))
                    {
                        if (upResult.ToString().Contains("Duplicate Cutref. Please redo Auto Ref#"))
                        {
                            MyUtility.Msg.WarningBox("Duplicate Cutref. Please redo Auto Ref#");
                        }
                        else
                        {
                            ShowErr(upResult);
                        }
                    }
                    else
                    {
                        _transactionscope.Complete();
                    }
                }
            }
            this.RenewData();
            sorting(comboBox1.Text);  //避免順序亂掉
            this.OnDetailEntered();
        }

        private void AutoCut_Click(object sender, EventArgs e)
        {
            // 2020/03/20 ISP20200430調整編碼規則
            // 增加合併CutNo規則
            // 無預計裁剪日不處理
            // 相同[Fabric Combo]為編碼組合
            // 合併CutNo組合, [Fabric Combo]+[Fab_Panel Code]+[Marker No]+[Marker Name]+[Est. Cut Date]再加Size Ratio通常Size Ratio會一樣, 但可以手改
            // 合併CutNo組合的6欄一樣的資料,算裁剪總和,判斷有沒有超過最大裁剪數Construction.CuttingLayer, 若沒超過則CutNo編碼一樣

            gridValid();
            detailgrid.ValidateControl();

            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["cutno"]) && !MyUtility.Check.Empty(dr["estcutdate"]))
                {
                    string estcutdate = ((DateTime)dr["estcutdate"]).ToString("d");
                    DataTable wk = (DataTable)detailgridbs.DataSource;

                    // 編碼組合找出最大 + 1
                    decimal maxNo = 1 + MyUtility.Convert.GetDecimal(wk.Compute("Max(cutno)", $"FabricCombo ='{dr["FabricCombo"]}'"));

                    // 找合併組合相同資料, 且還沒產生Cutref
                    DataRow[] sdr = wk.Select($"FabricCombo ='{dr["FabricCombo"]}' and FabricPanelCode ='{dr["FabricPanelCode"]}' and MarkerNo ='{dr["MarkerNo"]}' and Markername ='{dr["Markername"]}' and estcutdate ='{estcutdate}' and SizeCode ='{dr["SizeCode"]}' and isnull(CutRef,'') = ''");

                    decimal sumLayer = MyUtility.Convert.GetDecimal(sdr.CopyToDataTable().Compute("sum(Layer)", ""));

                    if (sumLayer >　MyUtility.Convert.GetDecimal(dr["CuttingLayer"])) // 最大裁剪數看其中一筆即可
                    {
                        dr["cutno"] = maxNo;
                    }
                    else
                    {
                        decimal hm = sdr.AsEnumerable().Max(m => MyUtility.Convert.GetDecimal(m["cutno"]));
                        if (hm != 0)
                        {
                            foreach (var item in sdr.AsEnumerable().Where(w => MyUtility.Check.Empty(w["cutno"])))
                            {
                                item["cutno"] = hm;
                            }
                        }
                        else
                        {
                            foreach (var item in sdr)
                            {
                                item["cutno"] = maxNo;
                            }
                        }
                    }
                }
            }
        }

        private void Packing_Click(object sender, EventArgs e)
        {
            gridValid();
            detailgrid.ValidateControl();
            var dr = this.CurrentMaintain; if (null == dr) return;
            var frm = new Sci.Production.Cutting.P02_PackingMethod(false, CurrentMaintain["id"].ToString(), null, null);
            frm.ShowDialog(this);
            this.RenewData();
            sorting(comboBox1.Text);  //避免順序亂掉
            this.OnDetailEntered();
        }

        private void btnBatchAssignCellEstCutDate_Click(object sender, EventArgs e)
        {
            gridValid();
            detailgrid.ValidateControl();
            var frm = new Sci.Production.Cutting.P02_BatchAssignCellCutDate((DataTable)(detailgridbs.DataSource));
            frm.ShowDialog(this);
        }


        /// <summary>
        /// 產生SubDetail(PatternPanel)資料
        /// </summary>
        /// <param name="dr"></param>
        private void CreateSubDetailDatas(DataRow dr)
        {
            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@ukey", dr["UKey"]);
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            if (PatternPanelTb_Copy.Rows.Count > 0)
            {
                DataTable SubDetailData;
                GetSubDetailDatas(dr, out SubDetailData);
                foreach (DataRow ddr in PatternPanelTb_Copy.Rows)
                {
                    if (!SubDetailData.AsEnumerable().Any(row => row["ID"].EqualString(ddr["id"])
                    && row["WorkOrderUkey"].Equals(ddr["WorkOrderUkey"])
                    && row["PatternPanel"].Equals(ddr["PatternPanel"])
                    && row["FabricPanelCode"].Equals(ddr["FabricPanelCode"])))
                    {
                        DataRow newDr = SubDetailData.NewRow();
                        for (int i = 0; i < SubDetailData.Columns.Count; i++)
                        {
                            newDr[SubDetailData.Columns[i].ColumnName] = ddr[SubDetailData.Columns[i].ColumnName];
                        }
                        SubDetailData.Rows.Add(newDr);
                    }
                }
            }

        }

        //grid新增一筆的btn
        bool flag = false;
        bool isAdditionalrevisedmarker = false;
        protected override void OnDetailGridAppendClick()
        {
            flag = true;
            base.OnDetailGridAppendClick();
            this.detailgrid.SelectRowTo(0);
        }

        //grid插入的btn, override成複製功能
        protected override void OnDetailGridInsert(int index = -1)
        {
            DataTable table = (DataTable)this.detailgridbs.DataSource;
            DataRow newRow = table.NewRow();
            DataRow OldRow = CurrentDetailData == null ? newRow : CurrentDetailData;  //將游標停駐處的該筆資料複製起來
            if (isAdditionalrevisedmarker) CurrentDetailData["isbyAdditionalRevisedMarker"] = 1;
            //base.OnDetailGridInsert(index); //先給一個NewKey
            int maxkey;
            object comput = ((DataTable)detailgridbs.DataSource).Compute("Max(newkey)", "");
            if (comput == DBNull.Value) maxkey = 0;
            else maxkey = Convert.ToInt32(comput);
            maxkey = maxkey + 1;

            DataTable detailtmp = (DataTable)detailgridbs.DataSource;
            int TEMP = ((DataTable)detailgridbs.DataSource).Rows.Count;
            // 除Cutref, Cutno, Addname, AddDate, EditName, EditDate以外的所有欄位
            newRow["Newkey"] = maxkey;
            newRow["ID"] = OldRow["ID"];
            newRow["Type"] = OldRow["Type"];
            newRow["MDivisionId"] = OldRow["MDivisionId"];
            newRow["FactoryID"] = OldRow["FactoryID"];
            newRow["UKey"] = 0;
            newRow["SCIRefno"] = OldRow["SCIRefno"];
            newRow["Refno"] = OldRow["Refno"];
            newRow["FabricCombo"] = OldRow["FabricCombo"];
            newRow["FabricPanelCode"] = OldRow["FabricPanelCode"];
            newRow["Cutno"] = DBNull.Value;
            if (isAdditionalrevisedmarker) newRow["isbyAdditionalRevisedMarker"] = 2;
            else newRow["isbyAdditionalRevisedMarker"] = 0;

            //因按下新增也會進來這,但新增的btn不要複製全部
            if (flag || ((DataTable)this.detailgridbs.DataSource).Rows.Count <= 0)
            {
                //base.OnDetailGridInsert(index);
                if (OldRow["Type"].ToString() == "1")
                {
                    newRow["OrderID"] = OldRow["OrderID"];
                }
                else
                {
                    newRow["OrderID"] = OldRow["ID"];
                }
                if (index == -1) index = TEMP;

                // add PatternPanel 第三層資料
                PatternPanelTb_Copy.Clear();
                DataRow drNEW = PatternPanelTb_Copy.NewRow();
                drNEW["id"] = CurrentMaintain["ID"];
                drNEW["WorkOrderUkey"] = 0;  //新增WorkOrderUkey塞0
                drNEW["PatternPanel"] = "";
                drNEW["FabricPanelCode"] = "";
                PatternPanelTb_Copy.Rows.Add(drNEW);

                OldRow.Table.Rows.InsertAt(newRow, index);
                //flag = false;
            }
            else
            {
                newRow["OrderID"] = OldRow["OrderID"];
                newRow["SEQ1"] = OldRow["SEQ1"];
                newRow["SEQ2"] = OldRow["SEQ2"];
                //CutRef
                //newRow["Cutplanid"] = OldRow["Cutplanid"];
                //Cutno
                newRow["Layer"] = OldRow["Layer"];
                newRow["Colorid"] = OldRow["Colorid"];
                newRow["Markername"] = OldRow["Markername"];
                newRow["EstCutDate"] = OldRow["EstCutDate"];
                newRow["Cutcellid"] = OldRow["Cutcellid"];
                newRow["MarkerLength"] = OldRow["MarkerLength"];
                newRow["ConsPC"] = OldRow["ConsPC"];
                newRow["Cons"] = OldRow["Cons"];
                newRow["Refno"] = OldRow["Refno"];
                newRow["MarkerNo"] = OldRow["MarkerNo"];
                newRow["MarkerVersion"] = OldRow["MarkerVersion"];
                //newRow["Addname"] = Sci.Env.User.UserName;
                //newRow["AddDate"] = DateTime.Now;
                //EditName
                //EditDate
                newRow["MarkerDownLoadID"] = OldRow["MarkerDownLoadID"];
                newRow["FabricCode"] = OldRow["FabricCode"];
                newRow["Order_EachconsUKey"] = OldRow["Order_EachconsUKey"];
                newRow["Article"] = OldRow["Article"];
                newRow["SizeCode"] = OldRow["SizeCode"];
                newRow["CutQty"] = OldRow["CutQty"];
                newRow["FabricPanelCode1"] = OldRow["FabricPanelCode1"];
                newRow["PatternPanel"] = OldRow["PatternPanel"];
                newRow["Fabeta"] = OldRow["Fabeta"];
                newRow["sewinline"] = OldRow["sewinline"];
                newRow["actcutdate"] = OldRow["actcutdate"];
                newRow["Adduser"] = loginID;
                newRow["edituser"] = OldRow["edituser"];
                newRow["totallayer"] = OldRow["totallayer"];
                newRow["multisize"] = OldRow["multisize"];
                newRow["Order_SizeCode_Seq"] = OldRow["Order_SizeCode_Seq"];
                newRow["SORT_NUM"] = OldRow["SORT_NUM"];
                newRow["WeaveTypeID"] = OldRow["WeaveTypeID"];
                newRow["DescDetail"] = OldRow["DescDetail"];
                newRow["MarkerLengthY"] = OldRow["MarkerLengthY"];
                newRow["MarkerLengthE"] = OldRow["MarkerLengthE"];
                newRow["MtlTypeID_SCIRefno"] = OldRow["MtlTypeID_SCIRefno"];
                newRow["Description"] = OldRow["Description"];
                newRow["ActCuttingPerimeterNew"] = OldRow["ActCuttingPerimeterNew"];
                newRow["StraightLengthNew"] = OldRow["StraightLengthNew"];
                newRow["CurvedLengthNew"] = OldRow["CurvedLengthNew"];
                newRow["ActCuttingPerimeter"] = OldRow["ActCuttingPerimeterNew"];
                newRow["StraightLength"] = OldRow["StraightLengthNew"];
                newRow["CurvedLength"] = OldRow["CurvedLengthNew"];
                newRow["fromukey"] = OldRow["fromukey"];
                if (index == -1) index = TEMP;
                OldRow.Table.Rows.InsertAt(newRow, index);
                DataRow[] drTEMPS = sizeratioTb.Select(string.Format("WorkOrderUkey='{0}'", OldRow["ukey"].ToString()));
                foreach (DataRow drTEMP in drTEMPS)
                {
                    DataRow drNEW = sizeratioTb.NewRow();
                    drNEW["WorkOrderUkey"] = 0;  //新增WorkOrderUkey塞0
                    drNEW["ID"] = drTEMP["ID"];
                    drNEW["SizeCode"] = drTEMP["SizeCode"];
                    drNEW["Qty"] = drTEMP["Qty"];
                    drNEW["newkey"] = maxkey;
                    sizeratioTb.Rows.Add(drNEW);
                }

                DataRow[] drTEMPdists = distqtyTb.Select(string.Format("WorkOrderUkey='{0}'", OldRow["ukey"].ToString()));
                foreach (DataRow drTEMP in drTEMPdists)
                {
                    DataRow drNEW = distqtyTb.NewRow();
                    drNEW["WorkOrderUkey"] = 0;  //新增WorkOrderUkey塞0
                    drNEW["ID"] = drTEMP["ID"];
                    drNEW["OrderID"] = drTEMP["OrderID"];
                    drNEW["Article"] = drTEMP["Article"];
                    drNEW["SizeCode"] = drTEMP["SizeCode"];
                    drNEW["Qty"] = drTEMP["Qty"];
                    drNEW["newkey"] = maxkey;
                    distqtyTb.Rows.Add(drNEW);
                }
                DataRow[] drTEMPPP = PatternPanelTb.Select(string.Format("WorkOrderUkey='{0}'", OldRow["ukey"].ToString()));
                PatternPanelTb_Copy.Clear();
                foreach (DataRow drTEMP in drTEMPPP)
                {
                    DataRow drNEW = PatternPanelTb_Copy.NewRow();
                    drNEW["id"] = CurrentMaintain["ID"];
                    drNEW["WorkOrderUkey"] = 0;  //新增WorkOrderUkey塞0
                    drNEW["PatternPanel"] = drTEMP["PatternPanel"];
                    drNEW["FabricPanelCode"] = drTEMP["FabricPanelCode"];
                    PatternPanelTb_Copy.Rows.Add(drNEW);
                }
            }

            if (flag)
            {
                CreateSubDetailDatas(DetailDatas[0]);
            }
            else
            {
                CreateSubDetailDatas(this.detailgrid.GetDataRow(this.detailgridbs.Position - 1));
            }            

            flag = false;
        }

        protected override void OnDetailGridDelete()
        {
            if (CurrentDetailData == null)
            {
                return;
            }
            // 判斷有 CutPlanID不能刪除
            if (!string.IsNullOrEmpty(CurrentDetailData["Cutplanid"].ToString()))
            {
                MyUtility.Msg.WarningBox($"it's scheduled in P04. Cutting Daily Plan : {CurrentDetailData["Cutplanid"]}, can't be deleted.");
                return;
            }

            if (CurrentDetailData.RowState != DataRowState.Added)
            {
                string sqlchkOutput = $@"select id from CuttingOutput_Detail where WorkOrderUkey = '{CurrentDetailData["uKey"]}'";
                DataRow dataRow;
                if (MyUtility.Check.Seek(sqlchkOutput, out dataRow))
                {
                    MyUtility.Msg.WarningBox($"Already create output <{dataRow["id"]}>, cann't be deleted");
                    return;
                }
            }

            string ukey = CurrentDetailData["Ukey"].ToString() == "" ? "0" : CurrentDetailData["Ukey"].ToString();
            int NewKey = Convert.ToInt32(CurrentDetailData["NewKey"]);
            DataRow[] drar = sizeratioTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = {1}", ukey, NewKey));
            for (int i = drar.Count() - 1; i >= 0; i--)
            {
                drar[i].Delete();
            }
            drar = distqtyTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = {1}", ukey, NewKey));
            for (int i = drar.Count() - 1; i >= 0; i--)
            {
                drar[i].Delete();
            }
            drar = PatternPanelTb.Select(string.Format("WorkOrderUkey = {0} and NewKey = {1}", ukey, NewKey));
            for (int i = drar.Count() - 1; i >= 0; i--)
            {
                drar[i].Delete();
            }
            base.OnDetailGridDelete();
        }

        private void insertSizeRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow ndr = sizeratioTb.NewRow();
            ndr["newkey"] = CurrentDetailData["newkey"];
            ndr["WorkorderUkey"] = CurrentDetailData["Ukey"];
            ndr["Qty"] = 0;
            sizeratioTb.Rows.Add(ndr);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow selectDr = ((DataRowView)gridSizeRatio.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();

            cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);
        }

        #region MarkerLengt驗證/UnitCons/Cons計算
        private void numMarkerLengthY_MarkerLengthY_Validated(object sender, EventArgs e)
        {
            if (numMarkerLengthY.Text.Trim() == "") return;
            if (numMarkerLengthY.OldValue == numMarkerLengthY.Text) return;

            int y;
            y = int.Parse(numMarkerLengthY.Text);
            CurrentDetailData["MarkerLengthY"] = y.ToString("D2");
            cal_Cons(true, true);
        }

        private void txtMarkerLengthE_MarkerLengthE_Validating(object sender, CancelEventArgs e)
        {
            if (txtMarkerLengthE.OldValue == txtMarkerLengthE.Text) return;

            CurrentDetailData["MarkerLengthE"] = txtMarkerLengthE.Text;
            cal_Cons(true, true);
        }

        private void numUnitCons_UnitCons_Validated(object sender, EventArgs e)
        {
            //與marklength變更規則不一樣
            decimal cp = MyUtility.Convert.GetDecimal(CurrentDetailData["Conspc"]);
            decimal la = MyUtility.Convert.GetDecimal(CurrentDetailData["Layer"]);
            decimal ttsr = MyUtility.Convert.GetDecimal(sizeratioTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey = '{0}' and newkey = '{1}'", CurrentDetailData["Ukey"], CurrentDetailData["newkey"])));
            CurrentDetailData["Cons"] = cp * la * ttsr;
        }

        private void cal_Cons(bool updateConsPC, bool updateCons) //update Cons
        {
            gridValid();
            if (numMarkerLengthY.Text.Trim() == "") return;

            int sizeRatioQty;
            object comput;
            comput = sizeratioTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey = '{0}' and newkey = '{1}'", CurrentDetailData["Ukey"], CurrentDetailData["newkey"]));
            if (comput == DBNull.Value) sizeRatioQty = 0;
            else sizeRatioQty = Convert.ToInt32(comput);

            decimal MarkerLengthNum, Conspc;
            string MarkerLengthstr, lenY, lenE;
            if (MyUtility.Check.Empty(CurrentDetailData["MarkerLengthY"])) lenY = "0";
            else lenY = CurrentDetailData["MarkerLengthY"].ToString();
            if (MyUtility.Check.Empty(CurrentDetailData["MarkerLengthE"])) lenE = "0-0/0+0\"";
            else lenE = CurrentDetailData["MarkerLengthE"].ToString();
            MarkerLengthstr = lenY + "Y" + lenE;
            MarkerLengthNum = Convert.ToDecimal(MyUtility.GetValue.Lookup(string.Format("Select dbo.MarkerLengthToYDS('{0}')", MarkerLengthstr)));
            if (sizeRatioQty == 0) Conspc = 0;
            else Conspc = MarkerLengthNum / sizeRatioQty;//Conspc = MarkerLength / SizeRatio Qty
            if (updateConsPC)
                CurrentDetailData["Conspc"] = Conspc;
            if (updateCons)
            {
                if (MyUtility.Check.Empty(CurrentDetailData["Layer"]))
                    CurrentDetailData["Cons"] = MarkerLengthNum * 0;
                else
                    CurrentDetailData["Cons"] = MarkerLengthNum * Convert.ToInt32(CurrentDetailData["Layer"]);
            }
            this.txtMarkerLength.Text = MarkerLengthstr;
            this.txtMarkerLength.ValidateControl();
        }
        #endregion

        private void cal_TotalCutQty(object workorderukey, object newkey)
        {
            gridValid();
            string TotalCutQtystr;
            TotalCutQtystr = "";
            DataRow[] sizeview = sizeratioTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} ", Convert.ToInt32(workorderukey), Convert.ToInt32(newkey)));

            foreach (DataRow dr in sizeview)
            {
                if (TotalCutQtystr == "")
                {
                    TotalCutQtystr = TotalCutQtystr + dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(MyUtility.Check.Empty(CurrentDetailData["Layer"]) ? 0 : CurrentDetailData["Layer"])).ToString();
                }
                else
                {
                    TotalCutQtystr = TotalCutQtystr + "," + dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(MyUtility.Check.Empty(CurrentDetailData["Layer"]) ? 0 : CurrentDetailData["Layer"])).ToString();
                }
            }
            CurrentDetailData["CutQty"] = TotalCutQtystr;
        }

        private void insertNewRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow ndr = distqtyTb.NewRow();
            ndr["newkey"] = CurrentDetailData["newkey"];
            ndr["WorkorderUkey"] = CurrentDetailData["Ukey"];
            ndr["Qty"] = 0;
            distqtyTb.Rows.Add(ndr);
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataRow selectDr = ((DataRowView)gridDistributetoSPNo.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();

            totalDisQty();
        }

        #region Save Before Post After
        protected override bool ClickSaveBefore()
        {
            gridValid();

            int index = 0;
            foreach(DataRow row in DetailDatas.Where(x => MyUtility.Check.Empty(x["MarkerNo"].ToString())))
            { 
                index = DetailDatas.IndexOf(row);
                this.detailgrid.SelectRowTo(index);
                MyUtility.Msg.WarningBox("Marker No cannot be empty.");
                return false;
            }

            foreach (DataRow row in DetailDatas.Where(x => MyUtility.Check.Empty(x["FabricPanelCode"].ToString())))
            {
                index = DetailDatas.IndexOf(row);
                this.detailgrid.SelectRowTo(index);
                MyUtility.Msg.WarningBox("Fab_Panel Code cannot be empty.");
                return false;
            }

            foreach (DataRow row in DetailDatas.Where(x => MyUtility.Check.Empty(x["CutRef"].ToString()) && !MyUtility.Check.Empty(x["CutNo"].ToString())))
            {
                //與該筆相同 FabricCombo、Cut# 的資料
                List<DataRow> SameDatas = DetailDatas.Where(o =>
                                                                o["CutRef"].ToString() == string.Empty
                                                                && o["CutNo"].ToString() == row["CutNo"].ToString()
                                                                && o["FabricCombo"].ToString() == row["FabricCombo"].ToString()
                                                                && o["Ukey"].ToString() != row["Ukey"].ToString()).ToList();

                if (SameDatas.Count > 0)
                {
                    var SigngleData = SameDatas.Where(x => x["MarkerName"].ToString() != row["MarkerName"].ToString() ||
                                                           x["MarkerNo"].ToString() != row["MarkerNo"].ToString()
                                               ).ToList();
                    if (SigngleData.Count > 0)
                    {
                        index = DetailDatas.IndexOf(row);
                        this.detailgrid.SelectRowTo(index);
                        MyUtility.Msg.WarningBox("In the same fabric combo, different 'Marker Name' and 'Marker No' cannot cut in one time which means cannot set the same cut#.");
                        return false;
                    } 
                } 
            }

            var query = DetailDatas.Where(x => x.RowState != DataRowState.Deleted &&
                                    (MyUtility.Check.Empty(x["MarkerName"]) ||
                                    MyUtility.Check.Empty(x["Layer"]) ||
                                    MyUtility.Check.Empty(x["SEQ1"]) ||
                                    MyUtility.Check.Empty(x["SEQ2"]))).ToList();
            if (query.Count > 0)
            {
                MyUtility.Msg.ErrorBox(string.Format("MarkerName,Layer,SEQ1,SEQ2 can't be empty"));
                return false;
            } 

            #region 刪除Qty 為0 
            DataRow[] deledr;
            DataRow[] sizeratioTbrow = sizeratioTb.AsEnumerable().Where(
                                x => x.RowState != DataRowState.Deleted &&
                                (MyUtility.Convert.GetInt(x["Qty"]) == 0 || MyUtility.Check.Empty(x["SizeCode"]))).ToArray();
            for (int i = sizeratioTbrow.Count() - 1 ; i >=  0 ; i--)
            {
                sizeratioTbrow[i].Delete();
            }

            DataRow[] distqtyTbrow = distqtyTb.AsEnumerable().Where(
                                x => this.DistributeToSPSaveBeforeCheck(x)).ToArray();
            for (int i = distqtyTbrow.Count() - 1 ; i >= 0 ; i--)
            {
                distqtyTbrow[i].Delete();
            }
            #endregion

            DataTable dt;
            string msg1 = "", msg2 = "";
            MyUtility.Tool.ProcessWithDatatable(sizeratioTb, "SizeCode,WorkOrderUkey,NewKey", "Select SizeCode,WorkOrderUkey,NewKey,Count() as countN from #tmp having countN >1 Group by SizeCode,WorkOrderUkey,NewKey", out dt);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    msg1 = msg1 + dr["WorkOrderUkey"].ToString() + "\n";
                }
            }
            if (!MyUtility.Check.Empty(msg1))
            {
                MyUtility.Msg.WarningBox("The SizeRatio duplicate ,Please see below <Ukey> \n" + msg1);
                return false;
            }

            DualResult result = MyUtility.Tool.ProcessWithDatatable(distqtyTb, string.Empty, @"
Select OrderID,Article,SizeCode,WorkOrderUkey,NewKey,Count(1) as countN
from #tmp
Group by OrderID,Article,SizeCode,WorkOrderUkey,NewKey
having Count(1) >1", out dt);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            foreach (DataRow dr in dt.Rows)
            {
                msg2 = msg2 + dr["WorkOrderUkey"].ToString() + "\n";
            }

            if (!MyUtility.Check.Empty(msg2))
            {
                MyUtility.Msg.WarningBox("The Distribute Qty data duplicate ,Please see below <Ukey> \n" + msg2);
                return false;
            }

            #region 檢查每一筆 Total distributionQty是否大於TotalCutQty總和
            foreach (DataRow dr_d in DetailDatas.Where(x => x.RowState != DataRowState.Deleted))
            {
                decimal ttlcutqty = 0, ttldisqty = 0;
                DataRow[] sizedr = sizeratioTb.Select(string.Format("newkey = '{0}' and workorderUkey= '{1}'", dr_d["newkey"].ToString(), dr_d["Ukey"].ToString()));
                DataRow[] distdr = distqtyTb.Select(string.Format("newkey = '{0}' and workorderUkey= '{1}'", dr_d["newkey"].ToString(), dr_d["Ukey"].ToString()));
                ttlcutqty = sizedr.Sum(x => x.Field<decimal>("Qty")) * MyUtility.Convert.GetDecimal(dr_d["Layer"]);
                ttldisqty = distdr.Sum(x => x.Field<decimal>("Qty"));
                if (ttlcutqty < ttldisqty)
                {
                    ShowErr(string.Format("Key:{0} Distribution Qty can not exceed total Cut qty", dr_d["Ukey"].ToString()));
                    return false;
                }
            }
            #endregion
            CurrentMaintain["cutinline"] = ((DataTable)detailgridbs.DataSource).Compute("Min(estcutdate)", null);
            CurrentMaintain["CutOffLine"] = ((DataTable)detailgridbs.DataSource).Compute("MAX(estcutdate)", null);

            return base.ClickSaveBefore();
        }

        private bool DistributeToSPSaveBeforeCheck(DataRow distDr)
        { 
            if(distDr.RowState != DataRowState.Deleted &&
                                (MyUtility.Convert.GetInt(distDr["Qty"]) == 0 || MyUtility.Check.Empty(distDr["SizeCode"]) || MyUtility.Check.Empty(distDr["Article"])) &&
                               MyUtility.Convert.GetString(distDr["OrderID"]).ToUpper() != "EXCESS")
                {
                return true;
            }

            if (distDr.RowState != DataRowState.Deleted && MyUtility.Convert.GetInt(distDr["Qty"]) == 0 && MyUtility.Convert.GetString(distDr["OrderID"]).ToUpper() == "EXCESS")
            {
                return true;
            }

            return false;
        }

        protected override DualResult ClickSave()
        {
            #region RevisedMarkerOriginalData 非AdditionalRevisedMarker功能增加的資料 isbyAdditionalRevisedMarker == 0
            string sqlInsertRevisedMarkerOriginalData = string.Empty;
            sqlInsertRevisedMarkerOriginalData += "declare @ID bigint";
            foreach (DataRow dr in DetailDatas.Where(w => (w.RowState == DataRowState.Modified) &&
                            MyUtility.Convert.GetString(w["MarkerName", DataRowVersion.Original]) != MyUtility.Convert.GetString(w["MarkerName"]) &&
                            MyUtility.Convert.GetInt(w["isbyAdditionalRevisedMarker"]) == 0))
            {
                string sqlchk = $@"select 1 from WorkOrderRevisedMarkerOriginalData_Detail where WorkOrderUkey = ('{dr["ukey"]}')  ";
                if (!MyUtility.Check.Seek(sqlchk))
                {
                    sqlInsertRevisedMarkerOriginalData += $@"
INSERT INTO [dbo].[WorkOrderRevisedMarkerOriginalData]
([ID],[FactoryID],[MDivisionId],[SEQ1],[SEQ2],[CutRef],[OrderID],[CutplanID],[Cutno],[Layer],[Colorid],[Markername],[EstCutDate],[CutCellid],[MarkerLength]
,[ConsPC],[Cons],[Refno],[SCIRefno],[MarkerNo],[MarkerVersion],[Type],[AddName],[AddDate],[EditName],[EditDate],[FabricCombo],[MarkerDownLoadId]
,[FabricCode],[FabricPanelCode],[Order_EachconsUkey],[OldFabricUkey],[OldFabricVer],[ActCuttingPerimeter],[StraightLength],[CurvedLength],[SpreadingNoID])
select [ID],[FactoryID],[MDivisionId],[SEQ1],[SEQ2],[CutRef],[OrderID],[CutplanID],[Cutno],[Layer],[Colorid],[Markername],[EstCutDate],[CutCellid]
,[MarkerLength],[ConsPC],[Cons],[Refno],[SCIRefno],[MarkerNo],[MarkerVersion],[Type],[AddName],[AddDate],[EditName],[EditDate],[FabricCombo]
,[MarkerDownLoadId],[FabricCode],[FabricPanelCode],[Order_EachconsUkey],[OldFabricUkey],[OldFabricVer],[ActCuttingPerimeter],[StraightLength]
,[CurvedLength],[SpreadingNoID]
from WorkOrder where Ukey ={dr["ukey"]}

set @ID = (select @@IDENTITY)

INSERT INTO WorkOrderRevisedMarkerOriginalData_Detail(WorkorderUkeyRevisedMarkerOriginalUkey,WorkorderUkey)
            values(@ID,{dr["ukey"]})

INSERT INTO [dbo].[WorkOrder_DistributeRevisedMarkerOriginalData]([WorkOrderRevisedMarkerOriginalDataUkey],[ID],[OrderID],[Article],[SizeCode],[Qty])
select @ID,[ID],[OrderID],[Article],[SizeCode],[Qty] from WorkOrder_Distribute where WorkOrderUkey = {dr["ukey"]}

INSERT INTO [dbo].[WorkOrder_PatternPanelRevisedMarkerOriginalData]([ID],[WorkOrderRevisedMarkerOriginalDataUkey],[PatternPanel],[FabricPanelCode])
select [ID],@ID,[PatternPanel],[FabricPanelCode] from [dbo].[WorkOrder_PatternPanel] where WorkOrderUkey = {dr["ukey"]}

INSERT INTO [dbo].[WorkOrder_SizeRatioRevisedMarkerOriginalData]([WorkOrderRevisedMarkerOriginalDataUkey],[ID],[SizeCode],[Qty])
select @ID,[ID],[SizeCode],[Qty] from [dbo].[WorkOrder_SizeRatio] where WorkOrderUkey = {dr["ukey"]}
";
                }
            }
            #endregion
            #region RevisedMarkerOriginalData AdditionalRevisedMarker功能處理的資料, 原本那筆 isbyAdditionalRevisedMarker = 1, 增加的那筆 = 2
            sqlInsertRevisedMarkerOriginalData += " declare @ID2 bigint";
            foreach (DataRow dr in DetailDatas.Where(w => (w.RowState == DataRowState.Modified) &&
                        MyUtility.Convert.GetInt(w["isbyAdditionalRevisedMarker"]) == 1))
            {
                string sqlchk = $@"select 1 from WorkOrderRevisedMarkerOriginalData_Detail where WorkOrderUkey = ('{dr["ukey"]}')  ";
                if (!MyUtility.Check.Seek(sqlchk))
                {
                    sqlInsertRevisedMarkerOriginalData += $@"
INSERT INTO [dbo].[WorkOrderRevisedMarkerOriginalData]
([ID],[FactoryID],[MDivisionId],[SEQ1],[SEQ2],[CutRef],[OrderID],[CutplanID],[Cutno],[Layer],[Colorid],[Markername],[EstCutDate],[CutCellid],[MarkerLength]
,[ConsPC],[Cons],[Refno],[SCIRefno],[MarkerNo],[MarkerVersion],[Type],[AddName],[AddDate],[EditName],[EditDate],[FabricCombo],[MarkerDownLoadId]
,[FabricCode],[FabricPanelCode],[Order_EachconsUkey],[OldFabricUkey],[OldFabricVer],[ActCuttingPerimeter],[StraightLength],[CurvedLength],[SpreadingNoID])
select [ID],[FactoryID],[MDivisionId],[SEQ1],[SEQ2],[CutRef],[OrderID],[CutplanID],[Cutno],[Layer],[Colorid],[Markername],[EstCutDate],[CutCellid]
,[MarkerLength],[ConsPC],[Cons],[Refno],[SCIRefno],[MarkerNo],[MarkerVersion],[Type],[AddName],[AddDate],[EditName],[EditDate],[FabricCombo]
,[MarkerDownLoadId],[FabricCode],[FabricPanelCode],[Order_EachconsUkey],[OldFabricUkey],[OldFabricVer],[ActCuttingPerimeter],[StraightLength]
,[CurvedLength],[SpreadingNoID]
from WorkOrder where Ukey ={dr["ukey"]}

set @ID2 = (select @@IDENTITY)

INSERT INTO WorkOrderRevisedMarkerOriginalData_Detail(WorkorderUkeyRevisedMarkerOriginalUkey,WorkorderUkey)
            values(@ID2,{dr["ukey"]})

INSERT INTO [dbo].[WorkOrder_DistributeRevisedMarkerOriginalData]([WorkOrderRevisedMarkerOriginalDataUkey],[ID],[OrderID],[Article],[SizeCode],[Qty])
select @ID2,[ID],[OrderID],[Article],[SizeCode],[Qty] from WorkOrder_Distribute where WorkOrderUkey = {dr["ukey"]}

INSERT INTO [dbo].[WorkOrder_PatternPanelRevisedMarkerOriginalData]([ID],[WorkOrderRevisedMarkerOriginalDataUkey],[PatternPanel],[FabricPanelCode])
select [ID],@ID2,[PatternPanel],[FabricPanelCode] from [dbo].[WorkOrder_PatternPanel] where WorkOrderUkey = {dr["ukey"]}

INSERT INTO [dbo].[WorkOrder_SizeRatioRevisedMarkerOriginalData]([WorkOrderRevisedMarkerOriginalDataUkey],[ID],[SizeCode],[Qty])
select @ID2,[ID],[SizeCode],[Qty] from [dbo].[WorkOrder_SizeRatio] where WorkOrderUkey = {dr["ukey"]}
";
                }
            }
            #endregion

            DualResult upResult;
            if (!(upResult = DBProxy.Current.Execute(null, sqlInsertRevisedMarkerOriginalData)))
            {
                return upResult;
            }
            return base.ClickSave();
        }

        protected override DualResult ClickSavePost()
        {
            #region RevisedMarkerOriginalData AdditionalRevisedMarker功能處理的資料, 在此取拆出來資料的ukey,處理刪除的資料
            string sqlUpdateRevisedMarkerOriginalData = string.Empty;
            var listAdditionalRevisedMarkerSeparate = DetailDatas.Where(w => (w.RowState == DataRowState.Modified || w.RowState == DataRowState.Added) &&
                       MyUtility.Convert.GetInt(w["isbyAdditionalRevisedMarker"]) == 2);
            foreach (DataRow dr in listAdditionalRevisedMarkerSeparate)
            {
                sqlUpdateRevisedMarkerOriginalData += $@"
                Insert into WorkOrderRevisedMarkerOriginalData_Detail(WorkorderUkeyRevisedMarkerOriginalUkey,WorkorderUkey)
                            select WorkorderUkeyRevisedMarkerOriginalUkey,{dr["Ukey"]}
                            from WorkOrderRevisedMarkerOriginalData_Detail where WorkorderUkey = {dr["fromukey"]}
";
            }

            var listDeleteRevisedMarkerSeparate = this.CurrentDetailData.Table.AsEnumerable().Where(w => w.RowState == DataRowState.Deleted);
            foreach (DataRow dr in listDeleteRevisedMarkerSeparate)
            {
                sqlUpdateRevisedMarkerOriginalData += $@"
                delete WorkOrderRevisedMarkerOriginalData_Detail where WorkorderUkey = {dr["Ukey",DataRowVersion.Original]}
";
            }

            // 刪除WorkOrderRevisedMarkerOriginalData 沒有detail的資料
            if (listDeleteRevisedMarkerSeparate.Any())
            {
                sqlUpdateRevisedMarkerOriginalData += $@"
                      delete  w
                        from WorkOrderRevisedMarkerOriginalData w
                        where not exists (select 1 from WorkOrderRevisedMarkerOriginalData_Detail wd 
                                    where wd.WorkorderUkeyRevisedMarkerOriginalUkey = w.Ukey)";
            }
            #endregion
            int ukey, newkey;
            DataRow[] dray;
            foreach (DataRow dr in DetailDatas.Where(w => w.RowState != DataRowState.Deleted))
            {
                ukey = Convert.ToInt32(dr["Ukey"]);
                newkey = Convert.ToInt32(dr["Newkey"]);

                dray = sizeratioTb.Select(string.Format("newkey={0} and workorderUkey= 0", newkey)); //0表示新增
                foreach (DataRow dr2 in dray)
                {
                    dr2["WorkOrderUkey"] = ukey;
                }

                dray = distqtyTb.Select(string.Format("newkey={0} and workorderUkey= 0", newkey)); //0表示新增
                foreach (DataRow dr2 in dray)
                {
                    dr2["WorkOrderUkey"] = ukey;
                }
            }
            string delsql = "", updatesql = "", insertsql = "";
            string cId = CurrentMaintain["ID"].ToString();
            #region SizeRatio 修改
            #region 刪除
            foreach (DataRow dr in sizeratioTb.AsEnumerable().Where(x => x.RowState == DataRowState.Deleted))
            {
                delsql = delsql + string.Format("Delete From WorkOrder_SizeRatio Where WorkOrderUkey={0} and SizeCode ='{1}' and ID ='{2}';", dr["WorkOrderUkey", DataRowVersion.Original], dr["SizeCode", DataRowVersion.Original], cId);
            }
            #endregion
            #region 修改
            foreach (DataRow dr in sizeratioTb.AsEnumerable().Where(x => x.RowState == DataRowState.Modified))
            {
                updatesql = updatesql + string.Format("Update WorkOrder_SizeRatio set Qty = {0},SizeCode = '{4}' where WorkOrderUkey ={1} and SizeCode = '{2}' and id ='{3}';", dr["Qty"], dr["WorkOrderUkey"], dr["SizeCode", DataRowVersion.Original], cId, dr["SizeCode"]);
            }
            #endregion
            #region 新增
            foreach (DataRow dr in sizeratioTb.AsEnumerable().Where(x => x.RowState == DataRowState.Added))
            {
                insertsql = insertsql + string.Format("Insert into WorkOrder_SizeRatio(WorkOrderUkey,SizeCode,Qty,ID) values({0},'{1}',{2},'{3}'); ", dr["WorkOrderUkey"], dr["SizeCode"], dr["Qty"], cId);
            }
            #endregion
            #endregion
            #region Distribute 修改
            #region 刪除
            foreach (DataRow dr in distqtyTb.AsEnumerable().Where(x => x.RowState== DataRowState.Deleted))
            {                
                 delsql = delsql + string.Format("Delete From WorkOrder_distribute Where WorkOrderUkey={0} and SizeCode ='{1}' and Article = '{2}' and OrderID = '{3}' and id='{4}';", dr["WorkOrderUkey", DataRowVersion.Original], dr["SizeCode", DataRowVersion.Original], dr["Article", DataRowVersion.Original], dr["Orderid", DataRowVersion.Original], cId);
            }
            #endregion
            #region 修改
            foreach (DataRow dr in distqtyTb.AsEnumerable().Where(x => x.RowState == DataRowState.Modified)) 
            {
                updatesql += $@"
Update WorkOrder_distribute
set Qty = {dr["Qty"]},SizeCode = '{dr["SizeCode"]}',Article = '{dr["Article"]}',OrderID = '{dr["OrderID"]}'
where WorkOrderUkey ={dr["WorkOrderUkey"]} 
and SizeCode = '{dr["SizeCode", DataRowVersion.Original]}'
and Article = '{ dr["Article", DataRowVersion.Original]}'
and OrderID = '{ dr["OrderID", DataRowVersion.Original]}'
and ID ='{dr["ID", DataRowVersion.Original]}'; ";

            }
            #endregion
            #region 新增
            foreach (DataRow dr in distqtyTb.AsEnumerable().Where(x => x.RowState == DataRowState.Added))
            {                
                insertsql = insertsql + string.Format("Insert into WorkOrder_distribute(WorkOrderUkey,SizeCode,Qty,Article,OrderID,ID) values({0},'{1}',{2},'{3}','{4}','{5}'); ", dr["WorkOrderUkey"], dr["SizeCode"], dr["Qty"], dr["Article"], dr["OrderID"], cId);
            }
            #endregion
            #endregion
            #region PatternPanel 修改
            //foreach (DataRow dr in PatternPanelTb.Rows)
            //{
            //    #region 新增
            //    if (dr.RowState == DataRowState.Added)
            //    {
            //        insertsql = insertsql + string.Format("Insert into Workorder_PatternPanel(WorkOrderUkey,PatternPanel,FabricPanelCode,ID) values({0},'{1}','{2}','{3}');", dr["WorkOrderUkey"], dr["PatternPanel"], dr["FabricPanelCode"], cId);
            //    }
            //    #endregion
            //}
            #endregion

            #region 回寫orders CutInLine,CutOffLine
            string _CutInLine, _CutOffLine;
            DateTime aa;

            //aa = Convert.ToDateTime(((DataTable)detailgridbs.DataSource).Compute("Min(estcutdate)", null));
            _CutInLine = ((DataTable)detailgridbs.DataSource).Compute("Min(estcutdate)", null) == DBNull.Value ? "" : Convert.ToDateTime(((DataTable)detailgridbs.DataSource).Compute("Min(estcutdate)", null)).ToString("yyyy-MM-dd HH:mm:ss");
            _CutOffLine = ((DataTable)detailgridbs.DataSource).Compute("Max(estcutdate)", null) == DBNull.Value ? "" : Convert.ToDateTime(((DataTable)detailgridbs.DataSource).Compute("Max(estcutdate)", null)).ToString("yyyy-MM-dd HH:mm:ss");
            updatesql = updatesql + string.Format("Update orders set CutInLine = iif('{0}' = '',null,'{0}'),CutOffLine =  iif('{1}' = '',null,'{1}') where POID = '{2}';", _CutInLine, _CutOffLine, CurrentMaintain["ID"]);

            #endregion


            DualResult upResult;
            if (!MyUtility.Check.Empty(sqlUpdateRevisedMarkerOriginalData))
            {
                if (!(upResult = DBProxy.Current.Execute(null, sqlUpdateRevisedMarkerOriginalData)))
                {
                    return upResult;
                }
            }
            if (!MyUtility.Check.Empty(delsql))
            {
                if (!(upResult = DBProxy.Current.Execute(null, delsql)))
                {
                    return upResult;
                }
            }
            if (!MyUtility.Check.Empty(updatesql))
            {
                if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                {
                    return upResult;
                }
            }
            if (!MyUtility.Check.Empty(insertsql))
            {
                if (!(upResult = DBProxy.Current.Execute(null, insertsql)))
                {
                    return upResult;
                }
            }

            #region sent data to GZ WebAPI
            string compareCol = "CutRef,EstCutDate,ID,OrderID,CutCellID";
            var listChangedDetail = ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                .Where(s =>
                {
                    return s.RowState == DataRowState.Added || (s.RowState == DataRowState.Modified && s.CompareDataRowVersionValue(compareCol));
                });

            if (listChangedDetail.Any())
            {
                DataTable dtWorkOrder = listChangedDetail.CopyToDataTable();
                Task.Run(() => new Guozi_AGV().SentWorkOrderToAGV(dtWorkOrder))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
            #endregion

            return base.ClickSavePost();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            foreach (DataRow dr in DetailDatas) dr["SORT_NUM"] = 0;  //編輯後存檔，將[SORT_NUM]歸零
            OnDetailEntered();
        }
        #endregion

        protected override bool ClickPrint()
        {
            Sci.Production.Cutting.P02_Print callNextForm;
            if (drTEMP != null)
            {
                callNextForm = new P02_Print(drTEMP, CurrentMaintain["ID"].ToString(), MyUtility.Convert.GetInt(CurrentMaintain["WorkType"]));
                callNextForm.ShowDialog(this);
            }
            else if (drTEMP == null && CurrentDetailData != null)
            {
                callNextForm = new P02_Print(CurrentDetailData, CurrentMaintain["ID"].ToString(), MyUtility.Convert.GetInt(CurrentMaintain["WorkType"]));
                callNextForm.ShowDialog(this);
            }
            else
            {
                MyUtility.Msg.InfoBox("No datas");
                return false;
            }

            return base.ClickPrint();
        }

        //編輯時，將[SORT_NUM]賦予流水號
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            int serial = 1;
            this.detailgridbs.SuspendBinding();
            foreach (DataRow dr in DetailDatas)
            {
                dr["SORT_NUM"] = serial;
                serial++;
            }
            this.detailgridbs.ResumeBinding();
            this.detailgrid.SelectRowTo(0);
        }

        private void txtBoxMarkerNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode)
            {
                string sqlCmd = string.Format(@"
select distinct a.MarkerNo from Order_EachCons a
inner join orders b on a.id = b.ID
where b.poid = '{0}'
", CurrentMaintain["ID"]);
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "20", txtBoxMarkerNo.Text);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }
                txtBoxMarkerNo.Text = item.GetSelectedString();
            }

        }

        private void txtBoxMarkerNo_Validating(object sender, CancelEventArgs e)
        {                       
            if (this.EditMode)
            {
                if (!MyUtility.Check.Seek(string.Format(@"
    select 1 from Order_EachCons a
    inner join orders b on a.id = b.ID
    where b.poid = '{0}' and a.MarkerNo='{1}'
    ", CurrentMaintain["ID"], this.txtBoxMarkerNo.Text)))
                {
                    MyUtility.Msg.WarningBox(string.Format("<MarkerNO: {0} > is not found!", this.txtBoxMarkerNo.Text));
                    CurrentDetailData["MarkerNo"] = string.Empty;
                    e.Cancel = true;
                    return;
                }
            }
            if (MyUtility.Check.Empty(txtBoxMarkerNo.Text))
            {
                MyUtility.Msg.WarningBox(string.Format("<MarkerNO > cannot be null"));
                CurrentDetailData["MarkerNo"] = string.Empty;
                e.Cancel = true;
                return ;
            }
            return;
        }
        
        private void txtFabricPanelCode_Validating(object sender, CancelEventArgs e)
        {
            DataRow dr;
            string new_FabricPanelCode = txtFabricPanelCode.Text;
            string sqlcmd = string.Format(@"select ob.SCIRefno,f.Description ,f.WeaveTypeID
                            from Order_BoF ob 
                            left join Fabric f on ob.SCIRefno = f.SCIRefno
                             where 
                             exists (select id from Order_FabricCode ofa where ofa.id = '{0}' and ofa.FabricPanelCode = '{1}'
                             and ofa.id = ob.id and ofa.FabricCode = ob.FabricCode)", CurrentMaintain["ID"], new_FabricPanelCode);

            if (MyUtility.Check.Seek(sqlcmd, out dr))
            {
                CurrentDetailData["SCIRefno"] = dr["SCIRefno"].ToString();
                CurrentDetailData["MtlTypeID_SCIRefno"] = dr["WeaveTypeID"].ToString() + " / " + dr["SCIRefno"].ToString();
                CurrentDetailData["Description"] = dr["Description"].ToString();
                CurrentDetailData["FabricPanelCode"] = new_FabricPanelCode;
            }
            else
            {
                MyUtility.Msg.WarningBox(string.Format("This FabricPanelCode<{0}> is wrong", txtFabricPanelCode.Text));
                CurrentDetailData["FabricPanelCode"] = string.Empty;
                e.Cancel = true;
                return;
            };
        }
    
        private void displayTime_DoubleClick(object sender, EventArgs e)
        {
            if (CurrentDetailData == null) return;
            var frm = new Sci.Production.Cutting.P02_OriginalData(CurrentDetailData);
            frm.ShowDialog(this);
        }

        private void btnAdditionalrevisedmarker_Click(object sender, EventArgs e)
        {
            isAdditionalrevisedmarker = true;
            this.OnDetailGridInsert(-1);
            isAdditionalrevisedmarker = false;
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                btnCutplanChangeHistory.Enabled = true;
            }
            else
            {
                btnCutplanChangeHistory.Enabled = false;
            }
        }

        private void btnCutplanChangeHistory_Click(object sender, EventArgs e)
        {
            if (callP07 != null && callP07.Visible == true)
            {
                callP07.P07Data(CurrentMaintain["ID"].ToString());
                callP07.Activate();
            }
            else
            {
                P07FormOpen();
            }
        }

        //Quantity Breakdown
        private void Qtybreak_Click(object sender, EventArgs e)
        {
            DataRow dr;
            MyUtility.Check.Seek(string.Format("select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList from Orders o WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"]), out dr);
            Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["ID"]), dr["PoList"].ToString());
            callNextForm.ShowDialog(this);
        }

        //PatternPanel
        private void btnPatternPanel_Click(object sender, EventArgs e)
        {
            OpenSubDetailPage();

            DataTable sudt;
            GetSubDetailDatas(this.CurrentDetailData, out sudt);
            var x = sudt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["PatternPanel"])).ToList();
            this.CurrentDetailData["PatternPanel"] = string.Join("+", x);
        }

        private void distribute_grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            this.RenewData();
            sorting(comboBox1.Text);  //避免順序亂掉
            this.OnDetailEntered(); ;
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            RenewData();
            OnDetailEntered();
        }


        private void gridDistributetoSPNo_SelectionChanged(object sender, EventArgs e)
        {
            //更換qtybreakdown index
            DataRow SpNoRow = gridDistributetoSPNo.GetDataRow(gridDistributetoSPNo.GetSelectedRowIndex());
            if (MyUtility.Check.Empty(SpNoRow)) return;
            string Article = SpNoRow["Article"].ToString();
            string SizeCode = SpNoRow["SizeCode"].ToString();
            string SpNo = SpNoRow["Orderid"].ToString();
            int rowIndex = 0;

            if (!MyUtility.Check.Empty(distqtyTb) || distqtyTb.Rows.Count > 1)
            {
                for (int rIdx = 0; rIdx < gridQtyBreakdown.Rows.Count; rIdx++)
                {
                    DataGridViewRow dvr = gridQtyBreakdown.Rows[rIdx];
                    DataRow row = ((DataRowView)dvr.DataBoundItem).Row;
                    if (row["article"].ToString() == Article && row["SizeCode"].ToString() == SizeCode && row["id"].ToString() == SpNo)
                    {
                        rowIndex = rIdx;
                        break;
                    }
                }
                gridQtyBreakdown.SelectRowTo(rowIndex);
            }
        }

        /// <summary>
        /// 確認【布】寬是否會超過【裁桌】的寬度
        /// 若是 CutCell 取得寬度為 0 ，則不顯示訊息
        /// </summary>
        /// <param name="strCutCellID">CutCellID</param>
        /// <param name="strSCIRefno">SciRefno</param>
        private void checkCuttingWidth(string strCutCellID, string strSCIRefno)
        {
            string chkwidth = MyUtility.GetValue.Lookup(string.Format(@"
select width_cm = width*2.54 
from Fabric 
where SCIRefno = '{0}'", strSCIRefno));
            string strCuttingWidth = MyUtility.GetValue.Lookup(string.Format(@"
select cuttingWidth = isnull (cuttingWidth, 0) 
from CutCell 
where   id = '{0}'
        and MDivisionID = '{1}'", strCutCellID, Sci.Env.User.Keyword));
            if (!chkwidth.Empty() && !strCuttingWidth.Empty())
            {
                decimal width_CM = decimal.Parse(chkwidth);
                decimal decCuttingWidth = decimal.Parse(strCuttingWidth);
                if (decCuttingWidth == 0)
                {
                    return;
                }

                if (width_CM > decCuttingWidth)
                {
                    MyUtility.Msg.WarningBox(string.Format("fab width greater than cutting cell {0}, please check it.", strCutCellID));
                }
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            gridValid();
            detailgrid.ValidateControl();
            sorting(comboBox1.Text);
        }
        
        Sci.Production.Cutting.P07 callP07 = null;
        private void P07FormOpen()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Sci.Production.Cutting.P07)
                {
                    form.Activate();
                    Sci.Production.Cutting.P07 activateForm = (Sci.Production.Cutting.P07)form;
                    activateForm.setTxtSPNo(CurrentMaintain["ID"].ToString());
                    activateForm.Queryable();
                    return;
                }
            }

            ToolStripMenuItem P07MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Sci.Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Cutting"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(System.Windows.Forms.ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P07. Query for Change Est. Cut Date Record"))
                            {
                                P07MenuItem = ((ToolStripMenuItem)subMenuItem);
                                break;
                            }
                        }
                    }
                }
            }

            callP07 = new P07(P07MenuItem);
            callP07.MdiParent = MdiParent;
            callP07.Show();
            callP07.P07Data(CurrentMaintain["ID"].ToString());
        }
    }
}
