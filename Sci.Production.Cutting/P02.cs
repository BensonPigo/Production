using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Class;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02 : Win.Tems.Input8
    {
        #region
        private readonly string LoginID = Sci.Env.User.UserID;
        private readonly string KeyWord = Sci.Env.User.Keyword;
        private readonly Win.UI.BindingSource2 bindingSource2 = new Win.UI.BindingSource2();
        private DataTable PatternPanelTb;
        private DataTable sizeratioTb;
        private DataTable layersTb;
        private DataTable distqtyTb;
        private DataTable qtybreakTb;
        private DataTable sizeGroup;
        private DataTable spTb;
        private DataTable artTb;
        private DataTable chksize;
        private DataRow drTEMP;  // 紀錄目前表身選擇的資料，避免按列印時模組會重LOAD資料，導致永遠只能印到第一筆資料
        private string Poid;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Markername;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_sp;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_seq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_seq2;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_cutcell;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SpreadingNoID;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_cutno;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_layer;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_wketa;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_estcutdate;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_cutref;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_sizeRatio_size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_sizeRatio_qty;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_dist_size;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_dist_article;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_dist_sp;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_dist_qty;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_ActCuttingPerimeterNew;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_StraightLengthNew;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_CurvedLengthNew;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_shift;
        #endregion

        /// <inheritdoc/>
        public P02(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            this.InitializeComponent();
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>
            {
                { "FabricPanelCode", "Pattern Panel" },
                { "SP", "SP" },
                { "Cut#", "Cut#" },
                { "Ref#", "Ref#" },
                { "Cutplan#", "Cutplan#" },
                { "MarkerName", "MarkerName" },
            };
            this.comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboBox1.ValueMember = "Key";
            this.comboBox1.DisplayMember = "Value";
            this.txtCutCell.MDivisionID = Sci.Env.User.Keyword;

            /*
             *設定Binding Source for Text
            */
            this.displayMarkerName.DataBindings.Add(new Binding("Text", this.bindingSource2, "MarkerName", true));
            this.displayColor.DataBindings.Add(new Binding("Text", this.bindingSource2, "colorid", true));
            this.numUnitCons.DataBindings.Add(new Binding("Value", this.bindingSource2, "Conspc", true));
            this.txtCutCell.DataBindings.Add(new Binding("Text", this.bindingSource2, "CutCellid", true));
            this.numCons.DataBindings.Add(new Binding("Value", this.bindingSource2, "Cons", true));
            this.txtFabricCombo.DataBindings.Add(new Binding("Text", this.bindingSource2, "FabricCombo", true));
            this.txtFabricPanelCode.DataBindings.Add(new Binding("Text", this.bindingSource2, "FabricPanelCode", true));
            this.editDescription.DataBindings.Add(new Binding("Text", this.bindingSource2, "Description", true));
            this.displayFabricType_Refno.DataBindings.Add(new Binding("Text", this.bindingSource2, "MtlTypeID_SCIRefno", true));
            this.displayCutplanNo.DataBindings.Add(new Binding("Text", this.bindingSource2, "Cutplanid", true));
            this.displayTotalCutQty.DataBindings.Add(new Binding("Text", this.bindingSource2, "CutQty", true));
            this.displayTime.DataBindings.Add(new Binding("Text", this.bindingSource2, "SandCTime", true));
            this.numMarkerLengthY.DataBindings.Add(new Binding("Text", this.bindingSource2, "MarkerLengthY", true));
            this.txtMarkerLengthE.DataBindings.Add(new Binding("Text", this.bindingSource2, "MarkerLengthE", true));
            this.txtMarkerLength.DataBindings.Add(new Binding("Text", this.bindingSource2, "MarkerLength", true));
            this.txtPatternPanel.DataBindings.Add(new Binding("Text", this.bindingSource2, "PatternPanel", true));
            this.lbshc.DataBindings.Add(new Binding("Text", this.bindingSource2, "shc", true));
            this.txtBoxMarkerNo.DataBindings.Add(new Binding("Text", this.bindingSource2, "MarkerNo", true));

            this.sizeratioMenuStrip.Enabled = this.EditMode;
            this.distributeMenuStrip.Enabled = this.EditMode;

            if (history == "0")
            {
                this.Text = "P02.Cutting Work Order";
                this.IsSupportEdit = true;
                this.DefaultFilter = string.Format("mDivisionid = '{0}' and WorkType is not null and WorkType != '' and Finished = 0", this.KeyWord);
            }
            else
            {
                this.Text = "P02.Cutting Work Order(History)";
                this.IsSupportEdit = false;
                this.DefaultFilter = string.Format("mDivisionid = '{0}' and WorkType is not null and WorkType != '' and Finished = 1", this.KeyWord);
            }

            this.detailgrid.Click += this.Detailgrid_Click;
        }

        private void Detailgrid_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgrid.CurrentCell))
            {
                return;
            }

            this.detailgrid.CurrentCell = this.detailgrid[this.detailgrid.CurrentCell.ColumnIndex, this.detailgrid.CurrentCell.RowIndex];
            this.detailgrid.BeginEdit(true);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridSizeRatio.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.Default;
            string querySql = $@"
select '' FTYGroup
union 
select distinct FTYGroup 
from Factory  WITH (NOLOCK)
where MDivisionID = '{this.KeyWord}'";
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
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };

            this.detailgrid.SelectionChanged += this.Detailgrid_SelectionChanged;
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

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            #region 主Table 左邊grid
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmdsql = $@"
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
    ,ImportML = cast(0 as bit)
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
where a.id = '{masterID}'            
";
            this.DetailSelectCommand = cmdsql;
            #endregion

            #region SizeRatio
            cmdsql = string.Format("Select *,0 as newKey from Workorder_SizeRatio WITH (NOLOCK) where id = '{0}'", masterID);
            DualResult dr = DBProxy.Current.Select(null, cmdsql, out this.sizeratioTb);
            if (!dr)
            {
                this.ShowErr(cmdsql, dr);
            }
            #endregion

            #region distqtyTb
            cmdsql = string.Format(@"Select *,0 as newKey From Workorder_distribute WITH (NOLOCK) Where id='{0}'", masterID);
            dr = DBProxy.Current.Select(null, cmdsql, out this.distqtyTb);
            if (!dr)
            {
                this.ShowErr(cmdsql, dr);
            }
            #endregion

            #region PatternPanelTb
            cmdsql = string.Format(@"Select *,0 as newKey From WorkOrder_PatternPanel WITH (NOLOCK) Where id='{0}'", masterID);
            dr = DBProxy.Current.Select(null, cmdsql, out this.PatternPanelTb);
            if (!dr)
            {
                this.ShowErr(cmdsql, dr);
            }
            #endregion
            #region layer
            cmdsql = $@"
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
Where a.id = '{masterID}' 
group by a.MarkerName,a.Colorid,a.Order_EachconsUkey,a.id 
Order by a.MarkerName,a.Colorid,a.Order_EachconsUkey
";
            dr = DBProxy.Current.Select(null, cmdsql, out this.layersTb);
            if (!dr)
            {
                this.ShowErr(cmdsql, dr);
            }
            #endregion

            #region 建立要使用右鍵開窗Grid
            string settbsql = $@"
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
Where b.cuttingsp ='{masterID}'
order by id,article,sizecode
drop table #tmp
";
            DualResult result = DBProxy.Current.Select(null, settbsql, out this.qtybreakTb);
            if (!result)
            {
                this.ShowErr(result);
            }
            else
            {
                this.sizeGroup = this.qtybreakTb.DefaultView.ToTable(true, "sizecode");
                this.artTb = this.qtybreakTb.DefaultView.ToTable(true, new string[] { "article", "ID" });
                this.spTb = this.qtybreakTb.DefaultView.ToTable(true, "id");
            }

            // 用來檢查size是否存在
            string sqlsizechk = $@"select distinct w.SizeCode from Workorder_SizeRatio w  WITH (NOLOCK) where w.ID = '{masterID}'";
            DBProxy.Current.Select(null, sqlsizechk, out this.chksize);
            #endregion

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "0" : MyUtility.Convert.GetString(e.Detail["UKey"]);
            this.SubDetailSelectCommand = string.Format(
                @"
select * from WorkOrder_PatternPanel  WITH (NOLOCK)
where WorkOrderUkey={0}", masterID);

            return base.OnSubDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.gridSizeRatio.DataSource = this.sizeratiobs;
            this.sizeratiobs.DataSource = this.sizeratioTb;
            this.distributebs.DataSource = this.distqtyTb;
            this.gridDistributetoSPNo.DataSource = this.distributebs;
            this.qtybreakds.DataSource = this.qtybreakTb;
            this.gridQtyBreakdown.DataSource = this.qtybreakds;

            this.sizeratioTb.DefaultView.RowFilter = string.Empty;
            this.qtybreakTb.DefaultView.RowFilter = string.Empty;
            this.OnDetailGridRowChanged();

            MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["ID"]), out DataRow orderdr);

            this.txtStyle.Text = orderdr == null ? string.Empty : orderdr["Styleid"].ToString();
            this.txtLine.Text = orderdr == null ? string.Empty : orderdr["SewLine"].ToString();
            string maxcutrefCmd = string.Format("Select Max(Cutref) from workorder WITH (NOLOCK) where mDivisionid = '{0}'", this.KeyWord);
            this.textbox_LastCutRef.Text = MyUtility.GetValue.Lookup(maxcutrefCmd);
            this.comboBox1.Enabled = !this.EditMode;  // Sorting於編輯模式時不可選取
            this.BtnImportMarker.Enabled = this.EditMode;

            foreach (DataRow dr in this.DetailDatas)
            {
                dr["Article"] = dr["Article"].ToString().TrimEnd('/');
            }

            this.Sorting(this.comboBox1.Text);
            this.detailgrid.SelectRowTo(0);
            this.detailgrid.AutoResizeColumns();

            this.col_shift.Width = 66;
            this.col_wketa.Width = 77;
            this.btnQuantityBreakdown.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;

            #region 取得 LeadTime 和 Subprocess
            var orders = this.distqtyTb.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["OrderID"])).Distinct().ToList();
            var leadTimeList = Prgs.GetLeadTimeList(orders, out string annotationStr);
            this.dispSubprocess.Text = annotationStr;
            this.numLeadTime.Value = leadTimeList.Count > 0 ? leadTimeList[0].LeadTimeDay : 0;
            #endregion

            #region 檢查MarkerNo ,MarkerVersion ,MarkerDownloadID是否與Order_Eachcons不同
            if (this.DetailDatas.Where(s => !s["MarkerNo"].Equals(s["EachconsMarkerNo"]) ||
                                           !s["MarkerVersion"].Equals(s["EachconsMarkerVersion"]) ||
                                           !s["MarkerDownloadID"].Equals(s["EachconsMarkerDownloadID"])).Count() > 0)
            {
                this.downloadid_Text.Visible = true;
            }
            else
            {
                this.downloadid_Text.Visible = false;
            }
            #endregion

            this.Poid = MyUtility.GetValue.Lookup(string.Format("Select poid from orders WITH (NOLOCK) where id ='{0}'", this.CurrentMaintain["ID"]));
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings cutno = new DataGridViewGeneratorTextColumnSettings();
            cutno.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                Regex numberPattern = new Regex("^[0-9]{1,6}$");
                if (!numberPattern.IsMatch(e.FormattedValue.ToString()))
                {
                    dr["Cutno"] = DBNull.Value;
                }
            };
            DataGridViewGeneratorDateColumnSettings estCutDate = new DataGridViewGeneratorDateColumnSettings();
            estCutDate.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow dr = ((Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (e.FormattedValue.ToString() == dr["estcutdate"].ToString())
                    {
                        return;
                    }

                    if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(e.FormattedValue)) > 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("[Est. Cut Date] can not be passed !!");
                    }
                }
            };
            DataGridViewGeneratorDateColumnSettings wKETA = new DataGridViewGeneratorDateColumnSettings();
            wKETA.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    P02_WKETA item = new P02_WKETA(dr);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (result == DialogResult.No)
                    {
                        dr["WKETA"] = DBNull.Value;
                    }

                    if (result == DialogResult.Yes)
                    {
                        dr["WKETA"] = itemx.WKETA;
                    }

                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorNumericColumnSettings breakqty = new DataGridViewGeneratorNumericColumnSettings();
            breakqty.EditingMouseDoubleClick += (s, e) =>
            {
                this.GridValid();
                this.detailgrid.ValidateControl();
                P01_Cutpartchecksummary callNextForm = new P01_Cutpartchecksummary(this.CurrentMaintain["ID"].ToString());
                callNextForm.ShowDialog(this);
            };

            #region ActCuttingPerimeter,StraightLength,CurvedLength 處理遮罩字串, 存檔字串要包含遮罩字元
            DataGridViewGeneratorMaskedTextColumnSettings actCuttingPerimeter = new DataGridViewGeneratorMaskedTextColumnSettings();
            actCuttingPerimeter.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                this.SetMaskString(e.FormattedValue.ToString().Replace(" ", "0"), "ActCuttingPerimeter");
            };
            DataGridViewGeneratorMaskedTextColumnSettings straightLength = new DataGridViewGeneratorMaskedTextColumnSettings();
            straightLength.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                this.SetMaskString(e.FormattedValue.ToString().Replace(" ", "0"), "StraightLength");
            };
            DataGridViewGeneratorMaskedTextColumnSettings curvedLength = new DataGridViewGeneratorMaskedTextColumnSettings();
            curvedLength.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                this.SetMaskString(e.FormattedValue.ToString().Replace(" ", "0"), "CurvedLength");
            };
            #endregion

            CellDropDownList dropdown = (CellDropDownList)CellDropDownList.GetGridCell("Pms_WorkOrderShift");
            DataGridViewGeneratorTextColumnSettings col_Shift = CellTextDropDownList.GetGridCell("Pms_WorkOrderShift");

            #region set grid
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6)).Get(out this.col_cutref)
                .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(5), settings: cutno).Get(out this.col_cutno)
                .Text("MarkerName", header: "Marker\r\nName", width: Widths.AnsiChars(5)).Get(out this.col_Markername)
                .Text("Fabriccombo", header: "Fabric\r\nCombo", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("FabricPanelCode", header: "Fab_Panel\r\nCode", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, maximum: 99999).Get(out this.col_layer)
                .Text("CutQty", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(13)).Get(out this.col_sp)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3)).Get(out this.col_seq1)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2)).Get(out this.col_seq2)
                .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("WKETA", header: "WK ETA", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: wKETA).Get(out this.col_wketa)
                .Date("estcutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10), settings: estCutDate).Get(out this.col_estcutdate)
                .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SpreadingNoID", header: "Spreading No", width: Widths.AnsiChars(2)).Get(out this.col_SpreadingNoID)
                .Text("Cutcellid", header: "Cut Cell", width: Widths.AnsiChars(2)).Get(out this.col_cutcell)
                .Text("Shift", header: "Shift", width: Widths.AnsiChars(20), settings: col_Shift).Get(out this.col_shift)
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
                .MaskedText("ActCuttingPerimeterNew", "000Yd00\"00", "ActCutting Perimeter", width: Widths.AnsiChars(16), settings: actCuttingPerimeter).Get(out this.col_ActCuttingPerimeterNew)
                .MaskedText("StraightLengthNew", "000Yd00\"00", "StraightLength", width: Widths.AnsiChars(16), settings: straightLength).Get(out this.col_StraightLengthNew)
                .MaskedText("CurvedLengthNew", "000Yd00\"00", "CurvedLength", width: Widths.AnsiChars(16), settings: curvedLength).Get(out this.col_CurvedLengthNew)
                ;
            this.Helper.Controls.Grid.Generator(this.gridSizeRatio)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(5)).Get(out this.col_sizeRatio_size)
                .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(5), integer_places: 6, maximum: 999999, minimum: 0).Get(out this.col_sizeRatio_qty);

            this.Helper.Controls.Grid.Generator(this.gridDistributetoSPNo)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(15)).Get(out this.col_dist_sp)
                .Text("article", header: "Article", width: Widths.AnsiChars(8)).Get(out this.col_dist_article)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(4)).Get(out this.col_dist_size)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(3), integer_places: 6, maximum: 999999, minimum: 0).Get(out this.col_dist_qty);

            this.Helper.Controls.Grid.Generator(this.gridQtyBreakdown)
                .Text("id", header: "SP#", width: Widths.AnsiChars(13))
                .Text("article", header: "Article", width: Widths.AnsiChars(7))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(3))
                .Numeric("Qty", header: "Order \nQty", width: Widths.AnsiChars(3), integer_places: 6, maximum: 999999, minimum: 0)
                .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(5), integer_places: 6, maximum: 999999, minimum: 0, settings: breakqty);

            this.detailgrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8F);
            #endregion

            this.Changeeditable();
        }

        private void SetMaskString(string eventString, string colName)
        {
            eventString = eventString.PadRight(7, '0');
            eventString = eventString.Substring(0, 3) + "Yd" + eventString.Substring(3, 2) + "\"" + eventString.Substring(5, 2);
            this.CurrentDetailData[colName] = eventString.TrimStart('0');
            this.CurrentDetailData[colName + "New"] = eventString;
        }

        private void Changeeditable() // Grid Cell 物件設定
        {
            #region maingrid
            #region cutref
            this.col_cutref.EditingControlShowing += (s, e) =>
            {
                ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
            };

            this.col_cutref.EditingKeyDown += (s, e) =>
            {
                if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) && MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]))
                {
                    e.EditingControl.Text = string.Empty;
                }
            };
            #endregion
            #region cutno
            this.col_cutno.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_cutno.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_Markername.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_Markername.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_layer.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;
                }
            };
            this.col_layer.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_layer.CellValidating += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1 || e.FormattedValue == null)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["layer"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                this.CurrentDetailData["layer"] = newvalue;

                int sumlayer = 0;
                if (MyUtility.Check.Empty(this.CurrentDetailData["Order_EachConsUkey"]))
                {
                    object o_sumLayer = ((DataTable)this.detailgridbs.DataSource).Compute("sum(layer)", string.Format("MarkerName = '{0}' and Colorid = '{1}'", this.CurrentDetailData["MarkerName"], this.CurrentDetailData["Colorid"]));
                    if (!o_sumLayer.Empty())
                    {
                        sumlayer = Convert.ToInt32(o_sumLayer);
                    }

                    DataRow[] drar = this.layersTb.Select(string.Format("MarkerName = '{0}' and Colorid = '{1}'", this.CurrentDetailData["MarkerName"], this.CurrentDetailData["Colorid"]));
                    if (drar.Length != 0)
                    {
                        this.numBalanceLayer.Value = sumlayer - Convert.ToInt32(drar[0]["TotalLayerMarker"]);
                    }
                }
                else
                {
                    object o_sumLayer = ((DataTable)this.detailgridbs.DataSource).Compute("sum(layer)", string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", this.CurrentDetailData["Order_EachConsUkey"], this.CurrentDetailData["Colorid"]));
                    if (!o_sumLayer.Empty())
                    {
                        sumlayer = Convert.ToInt32(o_sumLayer);
                    }

                    DataRow[] drar = this.layersTb.Select(string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", this.CurrentDetailData["Order_EachConsUkey"], this.CurrentDetailData["Colorid"]));
                    if (drar.Length != 0)
                    {
                        this.numBalanceLayer.Value = sumlayer - Convert.ToInt32(drar[0]["TotalLayerUkey"]);
                    }
                }

                this.Cal_TotalCutQty(this.CurrentDetailData["Ukey"], this.CurrentDetailData["NewKey"]);

                int newsumQty = 0;
                if (this.sizeratiobs.DataSource != null)
                {
                    object sq = ((DataTable)this.sizeratiobs.DataSource).DefaultView.ToTable().Compute("SUM(Qty)", string.Empty);
                    if (!sq.Empty())
                    {
                        newsumQty = MyUtility.Convert.GetInt(this.CurrentDetailData["layer"]) * Convert.ToInt32(sq);
                    }
                }

                // 重算DistributeqQty
                int oldttlqty = (int)this.numTotalDistributionQty.Value;
                int diff = newsumQty - oldttlqty;
                this.numTotalDistributionQty.Value = newsumQty;

                if (diff > 0)
                {
                    DataRow[] sizeR = this.sizeratioTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = '{1}'", this.CurrentDetailData["Ukey"].ToString(), this.CurrentDetailData["NewKey"].ToString()));
                    foreach (DataRow drr in sizeR)
                    {
                        this.UpdateExcess(Convert.ToInt32(this.CurrentDetailData["Ukey"]), Convert.ToInt32(this.CurrentDetailData["NewKey"]), drr["SizeCode"].ToString());
                    }
                }

                if (diff < 0)
                {
                    string sizetmp = string.Empty;
                    DataRow[] dist = this.distqtyTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = '{1}' ", this.CurrentDetailData["Ukey"].ToString(), this.CurrentDetailData["NewKey"].ToString()));

                    foreach (DataRow drr in dist)
                    {
                        DataRow[] sizeR = this.sizeratioTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = '{1}' and SizeCode = '{2}'", this.CurrentDetailData["Ukey"].ToString(), this.CurrentDetailData["NewKey"].ToString(), drr["SizeCode"].ToString()));

                        // size是否和前一筆相同，判斷是否有重複的size
                        if (sizetmp == drr["SizeCode"].ToString())
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

                this.Cal_Cons(true, true);
            };
            #endregion
            #region SP
            this.col_sp.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode && this.CurrentMaintain["WorkType"].ToString() != "1")
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_sp.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode || this.CurrentMaintain["WorkType"].ToString() == "1")
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
            this.col_sp.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (this.CurrentMaintain["WorkType"].ToString() == "1" || !MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]))
                    {
                        return;
                    }

                    // Parent form 若是非編輯狀態就 return
                    if (!this.EditMode)
                    {
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    SelectItem sele;

                    sele = new SelectItem(this.spTb, "ID", "15@300,400", dr["OrderID"].ToString(), columndecimals: "50");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            this.col_sp.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["orderid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                DataRow[] seledr = this.spTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["orderid"] = string.Empty;
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
            this.col_seq1.EditingMouseDown += (s, e) =>
            {
                if (!MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]))
                {
                    return;
                }

                P02_PublicFunction.Seq1EditingMouseDown(s, e, this, this.detailgrid, this.Poid);
            };
            this.col_seq1.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_seq1.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_seq1.CellValidating += (s, e) =>
            {
                P02_PublicFunction.Seq1CellValidating(s, e, this, this.detailgrid, this.Poid);
            };
            #endregion
            #region SEQ2
            this.col_seq2.EditingMouseDown += (s, e) =>
            {
                if (!MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]))
                {
                    return;
                }

                P02_PublicFunction.Seq2EditingMouseDown(s, e, this, this.detailgrid, this.Poid);
            };
            this.col_seq2.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_seq2.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_seq2.CellValidating += (s, e) =>
            {
                P02_PublicFunction.Seq2CellValidating(s, e, this, this.detailgrid, this.Poid);
            };
            #endregion
            #region estcutdate
            this.col_estcutdate.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.DateBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true;
                }
            };
            this.col_estcutdate.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_wketa.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true;
                    ((Ict.Win.UI.DateBox)e.Control).Enabled = true;
                }
                else
                {
                    ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true;
                    ((Ict.Win.UI.DateBox)e.Control).Enabled = false;
                }
            };
            this.col_wketa.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_cutcell.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_cutcell.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_cutcell.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    if (!this.EditMode)
                    {
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                    // 若 cutref != empty 則不可編輯
                    if (!MyUtility.Check.Empty(dr["Cutplanid"]))
                    {
                        return;
                    }

                    SelectItem sele;
                    DBProxy.Current.Select(null, string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", this.KeyWord), out DataTable cellTb);
                    sele = new SelectItem(cellTb, "ID", "10@300,300", dr["CutCellid"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["cutCellid"] = sele.GetSelectedString();
                    dr.EndEdit();

                    this.CheckCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                    cellchk = false;
                }
            };
            this.col_cutcell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty())
                {
                    return;
                }

                string oldvalue = dr["cutcellid"].ToString();
                string newvalue = e.FormattedValue.ToString();

                if (oldvalue == newvalue)
                {
                    return;
                }

                DBProxy.Current.Select(null, string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", this.KeyWord), out DataTable cellTb);
                DataRow[] seledr = cellTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["cutCellid"] = string.Empty;
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
                    this.CheckCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                }

                dr.EndEdit();
            };
            #endregion
            #region Shift
            this.col_shift.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_shift.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_SpreadingNoID.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_SpreadingNoID.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_SpreadingNoID.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    if (!this.EditMode)
                    {
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                    // 若 cutref != empty 則不可編輯
                    if (!MyUtility.Check.Empty(dr["Cutplanid"]))
                    {
                        return;
                    }

                    SelectItem sele;
                    DBProxy.Current.Select(null, $"Select id,CutCell = CutCellID from SpreadingNo WITH (NOLOCK) where mDivisionid = '{this.KeyWord}' and junk=0", out DataTable spreadingNoIDTb);

                    sele = new SelectItem(spreadingNoIDTb, "ID,CutCell", "10@400,300", dr["SpreadingNoID"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["SpreadingNoID"] = sele.GetSelectedString();
                    if (!MyUtility.Check.Empty(sele.GetSelecteds()[0]["CutCell"]))
                    {
                        dr["cutCellid"] = sele.GetSelecteds()[0]["CutCell"];
                        this.CheckCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                    }

                    dr.EndEdit();

                    col_SpreadingNoIDchk = false;
                }
            };
            this.col_SpreadingNoID.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty())
                {
                    return;
                }

                string oldvalue = dr["SpreadingNoID"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                string sqlSpreading = $"Select CutCellID from SpreadingNo WITH (NOLOCK) where mDivisionid = '{this.KeyWord}' and  id = '{newvalue}' and junk=0";
                if (!MyUtility.Check.Seek(sqlSpreading, out DataRow spreadingNodr))
                {
                    dr["SpreadingNoID"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SpreadingNo> : {0} data not found!", newvalue));
                    return;
                }

                dr["SpreadingNoID"] = newvalue;

                if (!MyUtility.Check.Empty(spreadingNodr["CutCellID"]))
                {
                    dr["cutCellid"] = spreadingNodr["CutCellID"];
                }

                if (!col_SpreadingNoIDchk)
                {
                    col_SpreadingNoIDchk = true;
                }
                else
                {
                    this.CheckCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                }

                dr.EndEdit();
            };
            #endregion
            #region col_ActCuttingPerimeterNew
            this.col_ActCuttingPerimeterNew.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_ActCuttingPerimeterNew.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_StraightLengthNew.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_StraightLengthNew.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_CurvedLengthNew.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.MaskedTextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_CurvedLengthNew.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_sizeRatio_size.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    if (!this.EditMode || this.CurrentDetailData["Cutplanid"].ToString() != string.Empty)
                    {
                        return;
                    }

                    DataRow dr = this.gridSizeRatio.GetDataRow(e.RowIndex);
                    SelectItem sele;

                    string oldvalue = dr["SizeCode"].ToString();

                    sele = new SelectItem(this.sizeGroup, "SizeCode", "15@300,300", dr["SizeCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                    string newvalue = sele.GetSelectedString();
                    dr["SizeCode"] = newvalue;
                    dr.EndEdit();

                    this.Redetailsize(Convert.ToInt32(this.CurrentDetailData["Ukey"]), Convert.ToInt32(this.CurrentDetailData["NewKey"]));
                    this.Cal_TotalCutQty(this.CurrentDetailData["Ukey"], this.CurrentDetailData["NewKey"]);
                    this.TotalDisQty();
                    DataRow[] distdrs = this.distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' ", Convert.ToInt32(this.CurrentDetailData["Ukey"]), Convert.ToInt32(this.CurrentDetailData["NewKey"]), oldvalue));
                    foreach (DataRow disdr in distdrs)
                    {
                        disdr["SizeCode"] = newvalue;
                    }
                }
            };
            this.col_sizeRatio_size.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridSizeRatio.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_sizeRatio_size.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridSizeRatio.GetDataRow(e.RowIndex);
                string oldvalue = dr["SizeCode"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                DataRow[] chkrow = this.chksize.Select(string.Format("SizeCode = '{0}'", newvalue));
                if (chkrow.Length == 0)
                {
                    e.Cancel = true;
                    this.ShowInfo(string.Format("Size <{0}> not found", newvalue));
                    this.gridSizeRatio.EditingControl.Select();
                    return;
                }

                dr["SizeCode"] = newvalue;
                dr.EndEdit();

                this.Redetailsize(Convert.ToInt32(this.CurrentDetailData["Ukey"]), Convert.ToInt32(this.CurrentDetailData["NewKey"]));
                this.Cal_TotalCutQty(this.CurrentDetailData["Ukey"], this.CurrentDetailData["NewKey"]);
                this.TotalDisQty();
                DataRow[] distdrs = this.distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' ", Convert.ToInt32(this.CurrentDetailData["Ukey"]), Convert.ToInt32(this.CurrentDetailData["NewKey"]), oldvalue));
                foreach (DataRow disdr in distdrs)
                {
                    disdr["SizeCode"] = newvalue;
                }
            };
            this.col_sizeRatio_qty.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridSizeRatio.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]) && this.EditMode)
                {
                    ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;
                }
            };
            this.col_sizeRatio_qty.CellValidating += (s, e) =>
            {
                // Parent form 若是非編輯狀態就 return
                if (!this.EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridSizeRatio.GetDataRow(e.RowIndex);
                int oldvalue = Convert.ToInt32(dr["Qty"]);
                int newvalue = Convert.ToInt32(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                dr["Qty"] = newvalue;
                dr.EndEdit();
                this.Cal_Cons(true, false);

                // cal_TotalCutQty(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                this.Redetailsize(Convert.ToInt32(this.CurrentDetailData["Ukey"]), Convert.ToInt32(this.CurrentDetailData["NewKey"]));
                this.Cal_TotalCutQty(this.CurrentDetailData["Ukey"], this.CurrentDetailData["NewKey"]);

                this.UpdateExcess(Convert.ToInt32(this.CurrentDetailData["Ukey"]), Convert.ToInt32(this.CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());
                this.TotalDisQty();
            };
            #endregion
            #region Distribute
            this.col_dist_sp.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    if (!this.EditMode)
                    {
                        return;
                    }

                    if (this.CurrentDetailData == null)
                    {
                        return;
                    }

                    DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString().ToUpper() == "EXCESS" || this.CurrentDetailData["Cutplanid"].ToString() != string.Empty)
                    {
                        return;
                    }

                    sele = new SelectItem(this.spTb, "ID", "15@300,400", dr["OrderID"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            this.col_dist_sp.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS")
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_dist_sp.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["orderid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue || newvalue.ToUpper() == "EXCESS")
                {
                    return;
                }

                DataRow[] seledr = this.spTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["orderid"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SP> : {0} data not found!", newvalue));
                    return;
                }

                if (!MyUtility.Check.Empty(dr["SizeCode"]) && !MyUtility.Check.Empty(dr["Article"]))
                {
                    seledr = this.qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", newvalue, dr["SizeCode"], dr["Article"]));
                    if (seledr.Length == 0)
                    {
                        dr["OrderID"] = string.Empty;
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2}", dr["OrderID"], newvalue, dr["Article"]));
                        return;
                    }
                }

                dr["orderid"] = newvalue;
                dr.EndEdit();
                this.TotalDisQty();
            };

            this.col_dist_size.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    if (!this.EditMode)
                    {
                        return;
                    }

                    if (this.CurrentDetailData == null)
                    {
                        return;
                    }

                    DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString().ToUpper() == "EXCESS" || this.CurrentDetailData["Cutplanid"].ToString() != string.Empty)
                    {
                        return;
                    }

                    DataTable srdt = ((DataTable)this.sizeratiobs.DataSource).DefaultView.ToTable();
                    MyUtility.Tool.ProcessWithDatatable(srdt, "sizecode", "Select distinct SizeCode from #tmp", out DataTable sizeGroup2);
                    sele = new SelectItem(sizeGroup2, "SizeCode", "15@300,300", dr["SizeCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            this.col_dist_size.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS")
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_dist_size.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["SizeCode"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                DataRow[] seledr = this.sizeGroup.Select(string.Format("SizeCode='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["SizeCode"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Size> : {0} data not found!", newvalue));
                    return;
                }

                if (!MyUtility.Check.Empty(dr["OrderID"]) && !MyUtility.Check.Empty(dr["Article"]))
                {
                    seledr = this.qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", dr["OrderID"], newvalue, dr["Article"]));
                    if (seledr.Length == 0)
                    {
                        dr["SizeCode"] = string.Empty;
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2} data not found", dr["OrderID"], dr["Article"], newvalue));
                        return;
                    }
                }

                dr["SizeCode"] = newvalue;
                dr.EndEdit();

                this.UpdateExcess(Convert.ToInt32(this.CurrentDetailData["Ukey"]), Convert.ToInt32(this.CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());
                this.TotalDisQty();
            };
            this.col_dist_article.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    if (!this.EditMode)
                    {
                        return;
                    }

                    if (this.CurrentDetailData == null)
                    {
                        return;
                    }

                    DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString().ToUpper() == "EXCESS" || this.CurrentDetailData["Cutplanid"].ToString() != string.Empty)
                    {
                        return;
                    }

                    sele = new SelectItem(this.artTb, "article", "15@300,300", dr["Article"].ToString(), false, ",", gridFilter: $"ID = '{dr["OrderID"].ToString()}'");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            this.col_dist_article.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS")
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
            this.col_dist_article.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["Article"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                DataRow[] seledr = this.artTb.Select(string.Format("Article='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["Article"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Article> : {0} data not found!", newvalue));
                    return;
                }

                if (!MyUtility.Check.Empty(dr["OrderID"]) && !MyUtility.Check.Empty(dr["SizeCode"]))
                {
                    seledr = this.qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", dr["OrderID"], dr["SizeCode"], newvalue));
                    if (seledr.Length == 0)
                    {
                        dr["Article"] = string.Empty;
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2}", dr["OrderID"], newvalue, dr["SizeCode"]));
                        return;
                    }
                }

                dr["Article"] = newvalue;
                dr.EndEdit();
            };

            // 依據Cutplanid&OrderID來設定是否能修改
            this.col_dist_qty.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS")
                {
                    ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                }
                else
                {
                    ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;
                }
            };

            // 重算qty
            this.col_dist_qty.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                // 右鍵彈出功能
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["Qty"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                dr["Qty"] = newvalue;
                dr.EndEdit();

                this.UpdateExcess(Convert.ToInt32(this.CurrentDetailData["Ukey"]), Convert.ToInt32(this.CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());

                // 計算完EXCESS正確後再Total計算
                this.TotalDisQty();
            };
            #endregion
        }

        private void Redetailsize(int workorderukey, int newkey) // 重組detailgrid的size
        {
            DataRow[] dsr = this.sizeratioTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1}", workorderukey, newkey));
            string sizeqty = string.Empty;
            foreach (DataRow dsrr in dsr)
            {
                sizeqty += ", " + dsrr["SizeCode"] + "/ " + dsrr["Qty"];
            }

            sizeqty = sizeqty.Substring(1);
            DataRow[] dr = ((DataTable)this.detailgridbs.DataSource).Select(string.Format("Ukey={0} and NewKey = {1}", workorderukey, newkey));
            dr[0]["SizeCode"] = sizeqty;
        }

        private void UpdateExcess(int workorderukey, int newkey, string sizecode) // 計算Excess
        {
            DataRow[] sizeview = this.sizeratioTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode = '{2}'", workorderukey, newkey, sizecode));
            foreach (DataRow dr in sizeview)
            {
                int now_distqty, org_distqty;
                object comput = this.distqtyTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode = '{2}'", workorderukey, newkey, dr["SizeCode"]));
                if (comput == DBNull.Value)
                {
                    now_distqty = 0;
                }
                else
                {
                    org_distqty = Convert.ToInt32(comput);
                }

                now_distqty = Convert.ToInt32(dr["Qty"]) * Convert.ToInt32(MyUtility.Check.Empty(this.CurrentDetailData["Layer"]) ? 0 : this.CurrentDetailData["Layer"]);
                DataRow[] distdr = this.distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' ", workorderukey, newkey, dr["SizeCode"]));
                if (distdr.Length == 0)
                {
                    DataRow ndr = this.distqtyTb.NewRow();
                    ndr["WorkOrderUKey"] = workorderukey;
                    ndr["NewKey"] = newkey;
                    ndr["OrderID"] = "EXCESS";
                    ndr["SizeCode"] = dr["SizeCode"];
                    ndr["Qty"] = now_distqty;
                    this.distqtyTb.Rows.Add(ndr);
                }
                else
                {
                    foreach (DataRow dr2 in distdr)
                    {
                        if (dr2["OrderID"].ToString().Trim().ToUpper() != "EXCESS")
                        {
                            now_distqty -= Convert.ToInt32(dr2["Qty"]);
                        }
                    }

                    DataRow[] exdr = this.distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' and OrderID ='EXCESS' ", workorderukey, newkey, dr["SizeCode"]));
                    if (exdr.Length == 0 && now_distqty > 0)
                    {
                        DataRow ndr = this.distqtyTb.NewRow();
                        ndr["WorkOrderUKey"] = workorderukey;
                        ndr["NewKey"] = newkey;
                        ndr["OrderID"] = "EXCESS";
                        ndr["SizeCode"] = dr["SizeCode"];
                        ndr["Qty"] = now_distqty;
                        this.distqtyTb.Rows.Add(ndr);
                    }
                    else if (exdr.Length > 0)
                    {
                        exdr[0]["Qty"] = now_distqty < 0 ? 0 : now_distqty;
                    }

                    this.gridDistributetoSPNo.EndEdit();
                }
            }
        }

        private void TotalDisQty()
        {
            if (this.distributebs.DataSource != null)
            {
                object sq = ((DataTable)this.distributebs.DataSource).DefaultView.ToTable().Compute("SUM(Qty)", string.Empty);
                if (!sq.Empty())
                {
                    this.numTotalDistributionQty.Value = Convert.ToInt32(sq);
                }
            }
        }

        private void GridValid()
        {
            this.gridSizeRatio.ValidateControl();
            this.gridDistributetoSPNo.ValidateControl();
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.sizeratioMenuStrip != null)
            {
                this.sizeratioMenuStrip.Enabled = this.EditMode;
            }

            if (this.distributeMenuStrip != null)
            {
                this.distributeMenuStrip.Enabled = this.EditMode;
            }

            if (this.BtnImportMarker != null)
            {
                this.BtnImportMarker.Enabled = this.EditMode;
            }
        }

        /// <inheritdoc/>
        protected override void DoPrint()
        {
            // 1394: CUTTING_P02_Cutting Work Order。KEEP當前的資料。
            this.drTEMP = this.CurrentDetailData;
            base.DoPrint();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRowChanged()
        {
            this.GridValid();
            base.OnDetailGridRowChanged();

            // Binding 資料來源
            if (this.CurrentDetailData == null)
            {
                return;
            }

            this.bindingSource2.SetRow(this.CurrentDetailData);

            #region 根據左邊Grid Filter 右邊資訊
            string filter = $@"Workorderukey = {this.CurrentDetailData["Ukey"]} and NewKey = {this.CurrentDetailData["NewKey"]}";
            this.sizeratioTb.DefaultView.RowFilter = filter;
            this.distqtyTb.DefaultView.RowFilter = filter;
            if (!MyUtility.Check.Empty(this.CurrentDetailData["Ukey"]))
            {
                this.gridDistributetoSPNo.SelectRowTo(0);
                for (int i = 0; i < this.gridDistributetoSPNo.Rows.Count; i++)
                {
                    if (this.gridDistributetoSPNo.Rows[i].Cells["OrderID"].Value.Equals(this.CurrentDetailData["OrderID"]))
                    {
                        this.gridDistributetoSPNo.SelectRowTo(i);
                        break;
                    }
                }
            }
            #endregion

            #region Total Dist
            this.TotalDisQty();
            #endregion

            int sumlayer = 0;
            int sumlayer2 = 0;

            if (MyUtility.Check.Empty(this.CurrentDetailData["Order_EachConsUkey"]))
            {
                DataRow[] aA = ((DataTable)this.detailgridbs.DataSource).Select(string.Format("MarkerName = '{0}' and Colorid = '{1}'", this.CurrentDetailData["MarkerName"], this.CurrentDetailData["Colorid"]));
                DataRow[] b = this.layersTb.Select(string.Format("MarkerName = '{0}' and Colorid = '{1}'", this.CurrentDetailData["MarkerName"], this.CurrentDetailData["Colorid"]));
                foreach (DataRow l in aA)
                {
                    sumlayer += MyUtility.Convert.GetInt(l["layer"]);
                }

                foreach (DataRow l in b)
                {
                    sumlayer2 += MyUtility.Convert.GetInt(l["layer"]);
                }
            }
            else
            {
                DataRow[] aA = ((DataTable)this.detailgridbs.DataSource).Select(string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", this.CurrentDetailData["Order_EachConsUkey"], this.CurrentDetailData["Colorid"]));
                DataRow[] b = this.layersTb.Select(string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", this.CurrentDetailData["Order_EachConsUkey"], this.CurrentDetailData["Colorid"]));
                foreach (DataRow l in aA)
                {
                    sumlayer += MyUtility.Convert.GetInt(l["layer"]);
                }

                foreach (DataRow l in b)
                {
                    sumlayer2 += MyUtility.Convert.GetInt(l["layer"]);
                }
            }

            if (this.CurrentDetailData["Order_EachConsUkey"] == DBNull.Value)
            {// old rule
                string selectcondition = string.Format("MarkerName = '{0}' and Colorid = '{1}'", this.CurrentDetailData["MarkerName"], this.CurrentDetailData["Colorid"]);
                DataRow[] laydr = this.layersTb.Select(selectcondition);
                if (laydr.Length == 0)
                {
                    this.numTotalLayer.Value = 0;
                    this.numBalanceLayer.Value = 0;
                }
                else
                {
                    this.numTotalLayer.Value = (decimal)laydr[0]["TotalLayerMarker"];
                    this.numBalanceLayer.Value = sumlayer - (decimal)laydr[0]["TotalLayerMarker"];
                }
            }
            else
            { // New rule
                long order_EachConsTemp = MyUtility.Convert.GetLong(this.CurrentDetailData["Order_EachConsUkey"]);
                string selectcondition = string.Format("Order_EachConsUkey = {0} and  Colorid = '{1}'", order_EachConsTemp, this.CurrentDetailData["Colorid"]);
                DataRow[] laydr = this.layersTb.Select(selectcondition);
                if (laydr.Length == 0)
                {
                    this.numTotalLayer.Value = 0;
                    this.numBalanceLayer.Value = 0;
                }
                else
                {
                    this.numTotalLayer.Value = (decimal)laydr[0]["TotalLayerUkey"];
                    this.numBalanceLayer.Value = sumlayer - (decimal)laydr[0]["TotalLayerUkey"];
                }
            }

            #region 判斷可否開放修改
            if (MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]) && this.EditMode)
            {
                this.numMarkerLengthY.ReadOnly = false;
                this.txtMarkerLengthE.ReadOnly = false;
                this.numUnitCons.ReadOnly = false;
                this.txtCutCell.ReadOnly = false;
                this.txtFabricCombo.ReadOnly = false;
                this.txtFabricPanelCode.ReadOnly = false;
                this.sizeratioMenuStrip.Enabled = true;
                this.distributeMenuStrip.Enabled = true;
                this.txtBoxMarkerNo.IsSupportEditMode = true;
                this.txtBoxMarkerNo.ReadOnly = false;
            }
            else
            {
                this.numMarkerLengthY.ReadOnly = true;
                this.txtMarkerLengthE.ReadOnly = true;
                this.numUnitCons.ReadOnly = true;
                this.txtCutCell.ReadOnly = true;
                this.txtFabricCombo.ReadOnly = true;
                this.txtFabricPanelCode.ReadOnly = true;
                this.sizeratioMenuStrip.Enabled = false;
                this.distributeMenuStrip.Enabled = false;
                this.txtBoxMarkerNo.IsSupportEditMode = false;
                this.txtBoxMarkerNo.ReadOnly = true;
            }
            #endregion
            this.TotalDisQty();

            #region 按鈕可否按
            this.btnDist.Enabled = MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]) && this.EditMode;
            #endregion

            this.gridSizeRatio.AutoResizeColumns();
            this.gridQtyBreakdown.AutoResizeColumns();

            // 抓到當前編輯的cell
            if (MyUtility.Check.Empty(this.detailgrid.CurrentCell))
            {
                return;
            }

            this.detailgrid.CurrentCell = this.detailgrid[this.detailgrid.CurrentCell.ColumnIndex, this.detailgrid.CurrentCell.RowIndex];
            this.detailgrid.BeginEdit(true);
        }

        /// <inheritdoc/>
        protected override void OnFormDispose()
        {
            // 程式產生的BindingSource 必須自行Dispose, 以節省資源
            base.OnFormDispose();
            this.bindingSource2.Dispose();
        }

        private void Sorting(string sort)
        {
            this.detailgrid.ValidateControl();
            if (this.CurrentDetailData == null)
            {
                return;
            }

            DataView dv = ((DataTable)this.detailgridbs.DataSource).DefaultView;
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
            this.GridValid();
            this.detailgrid.ValidateControl();
            #region 變更先將同d,Cutref, FabricPanelCode, CutNo, MarkerName, estcutdate 且有cutref,Cuno無cutplanid 的cutref值找出來Group by→cutref 會相同
            string cmdsql = $@"
SELECT isnull(Cutref,'') as cutref, isnull(FabricCombo,'') as FabricCombo, CutNo,
isnull(MarkerName,'') as MarkerName, estcutdate
FROM Workorder WITH (NOLOCK) 
WHERE (cutplanid is null or cutplanid ='') AND (CutNo is not null )
AND (cutref is not null and cutref !='') and id = '{this.CurrentMaintain["ID"]}' and mDivisionid = '{this.KeyWord}'
GROUP BY Cutref, FabricCombo, CutNo, MarkerName, estcutdate
";
            DualResult cutrefresult = DBProxy.Current.Select(null, cmdsql, out DataTable cutreftb);
            if (!cutrefresult)
            {
                this.ShowErr(cmdsql, cutrefresult);
                return;
            }
            #endregion

            // 找出空的cutref
            cmdsql = $@"
Select * 
From workorder WITH (NOLOCK) 
Where (CutNo is not null ) and (cutref is null or cutref ='') 
and (estcutdate is not null and estcutdate !='' )
and (CutCellid is not null and CutCellid !='' )
and id = '{this.CurrentMaintain["ID"]}' and mDivisionid = '{this.KeyWord}'
order by FabricCombo,cutno
"; // 找出空的cutref
            cutrefresult = DBProxy.Current.Select(null, cmdsql, out DataTable workordertmp);
            if (!cutrefresult)
            {
                this.ShowErr(cmdsql, cutrefresult);
                return;
            }

            string maxref = MyUtility.GetValue.Lookup("Select isnull(Max(cutref),'000000') from Workorder WITH (NOLOCK)"); // 找最大Cutref
            if (MyUtility.Check.Empty(maxref))
            {
                maxref = "000000";
            }

            string updatecutref = @"
DECLARE @chk tinyint
SET @chk = 0
Begin Transaction [Trans_Name] -- Trans_Name 
";

            // 寫入空的Cutref
            foreach (DataRow dr in workordertmp.Rows)
            {
                string newcutref = string.Empty;
                DataRow[] findrow = cutreftb.Select(string.Format(@"MarkerName = '{0}' and FabricCombo = '{1}' and Cutno = {2} and estcutdate = '{3}' ", dr["MarkerName"], dr["FabricCombo"], dr["Cutno"], dr["estcutdate"]));

                // 若有找到同馬克同部位同Cutno同裁剪日就寫入同cutref
                if (findrow.Length != 0)
                {
                    newcutref = findrow[0]["cutref"].ToString();
                }
                else
                {
                    maxref = MyUtility.GetValue.GetNextValue(maxref, 0);
                    DataRow newdr = cutreftb.NewRow();
                    newdr["MarkerName"] = dr["MarkerName"] ?? string.Empty;
                    newdr["FabricCombo"] = dr["FabricCombo"] ?? string.Empty;
                    newdr["Cutno"] = dr["Cutno"];
                    newdr["estcutdate"] = dr["estcutdate"] ?? string.Empty;
                    newdr["cutref"] = maxref;
                    cutreftb.Rows.Add(newdr);
                    newcutref = maxref;
                }

                updatecutref += string.Format($@"
    if (select COUNT(1) from Workorder WITH (NOLOCK) where cutref = '{newcutref}' and mDivisionid = '{this.KeyWord}' and id != '{this.CurrentMaintain["id"]}')>0
	begin
		RAISERROR ('Duplicate Cutref. Please redo Auto Ref#',12, 1) 
		Rollback Transaction [Trans_Name] -- 復原所有操作所造成的變更
	end
    Update Workorder set cutref = '{newcutref}' where ukey = '{dr["ukey"]}';");
            }

            updatecutref += @"
    IF @@Error <> 0 BEGIN SET @chk = 1 END
IF @chk <> 0 BEGIN -- 若是新增資料發生錯誤
    Rollback Transaction [Trans_Name] -- 復原所有操作所造成的變更
END
ELSE BEGIN
    Commit Transaction [Trans_Name] -- 提交所有操作所造成的變更
END";

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
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
                            this.ShowErr(upResult);
                        }
                    }
                    else
                    {
                        transactionscope.Complete();
                    }
                }
            }

            this.RenewData();
            this.Sorting(this.comboBox1.Text);  // 避免順序亂掉
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
            this.GridValid();
            this.detailgrid.ValidateControl();

            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["cutno"]) && !MyUtility.Check.Empty(dr["estcutdate"]))
                {
                    string estcutdate = ((DateTime)dr["estcutdate"]).ToString("d");
                    DataTable wk = (DataTable)this.detailgridbs.DataSource;

                    // 編碼組合找出最大 + 1
                    decimal maxNo = 1 + MyUtility.Convert.GetDecimal(wk.Compute("Max(cutno)", $"FabricCombo ='{dr["FabricCombo"]}'"));

                    // 找合併組合相同資料, 且還沒產生Cutref
                    DataTable sdt = wk.Select($"FabricCombo ='{dr["FabricCombo"]}' and FabricPanelCode ='{dr["FabricPanelCode"]}' and MarkerNo ='{dr["MarkerNo"]}' and Markername ='{dr["Markername"]}' and estcutdate ='{estcutdate}' and SizeCode ='{dr["SizeCode"]}' and isnull(CutRef,'') = ''").TryCopyToDataTable(wk);

                    decimal sumLayer = MyUtility.Convert.GetDecimal(sdt.Compute("sum(Layer)", string.Empty));

                    // 最大裁剪數看其中一筆即可
                    if (sumLayer > MyUtility.Convert.GetDecimal(dr["CuttingLayer"]))
                    {
                        dr["cutno"] = maxNo;
                    }
                    else
                    {
                        decimal hm = sdt.AsEnumerable().Max(m => MyUtility.Convert.GetDecimal(m["cutno"]));
                        if (hm != 0)
                        {
                            foreach (DataRow item in sdt.AsEnumerable().Where(w => MyUtility.Check.Empty(w["cutno"])))
                            {
                                item["cutno"] = hm;
                            }
                        }
                        else
                        {
                            foreach (DataRow item in sdt.Rows)
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
            this.GridValid();
            this.detailgrid.ValidateControl();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            var frm = new P02_PackingMethod(false, this.CurrentMaintain["id"].ToString(), null, null);
            frm.ShowDialog(this);
            this.RenewData();
            this.Sorting(this.comboBox1.Text);  // 避免順序亂掉
            this.OnDetailEntered();
        }

        private void BtnBatchAssign_Click(object sender, EventArgs e)
        {
            this.GridValid();
            this.detailgrid.ValidateControl();
            var frm = new P02_BatchAssign((DataTable)this.detailgridbs.DataSource, this.CurrentMaintain["ID"].ToString(), this.distqtyTb);
            frm.ShowDialog(this);
        }

        private void BtnStdQtyWIP_Click(object sender, EventArgs e)
        {
            this.GridValid();
            this.detailgrid.ValidateControl();
            var frm = new P02_StandardQtyPlannedCuttingWIP(this.CurrentMaintain["ID"].ToString(), this.distqtyTb);
            frm.ShowDialog(this);
        }

        // grid新增一筆的btn
        private bool flag = false;
        private bool isAdditionalrevisedmarker = false;

        /// <inheritdoc/>
        protected override void OnDetailGridAppendClick()
        {
            this.flag = true;
            base.OnDetailGridAppendClick();
            this.detailgrid.SelectRowTo(0);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            // grid插入的btn, override成複製功能
            // 注意 index 值是底層帶入的, 而表身的排序會使 this.detailgridbs.Position 與 index 變得不對應
            // base.OnDetailGridInsert(index);
            // DataTable oriPatternPanel = null;
            DataRow newRow = ((DataTable)this.detailgridbs.DataSource).NewRow();
            DataRow oldRow = this.CurrentDetailData ?? newRow; // 游標停駐處的 WorKOrder 為複製來源
            int maxkey = MyUtility.Convert.GetInt(((DataTable)this.detailgridbs.DataSource).Compute("Max(newkey)", string.Empty)) + 1; // 新值
            if (index == -1)
            {
                index = ((DataTable)this.detailgridbs.DataSource).Rows.Count;
            }

            #region 基本資訊欄位, 新增 & 插入
            newRow["ID"] = oldRow["ID"];
            newRow["Type"] = oldRow["Type"];
            newRow["MDivisionId"] = oldRow["MDivisionId"];
            newRow["FactoryID"] = oldRow["FactoryID"];
            newRow["SCIRefno"] = oldRow["SCIRefno"];
            newRow["Refno"] = oldRow["Refno"];
            newRow["FabricCombo"] = oldRow["FabricCombo"];
            newRow["FabricPanelCode"] = oldRow["FabricPanelCode"];
            newRow["UKey"] = 0;
            newRow["Newkey"] = maxkey;
            newRow["Cutno"] = DBNull.Value;
            #endregion

            if (this.isAdditionalrevisedmarker)
            {
                this.CurrentDetailData["isbyAdditionalRevisedMarker"] = 1;
                newRow["isbyAdditionalRevisedMarker"] = 2;
            }
            else
            {
                newRow["isbyAdditionalRevisedMarker"] = 0;
            }

            // 因按下新增也會進來OnDetailGridInsert，但不要複製全部
            // 按新增
            if (this.flag || ((DataTable)this.detailgridbs.DataSource).Rows.Count <= 0)
            {
                newRow["OrderID"] = MyUtility.Convert.GetString(oldRow["Type"]) == "1" ? oldRow["OrderID"] : oldRow["ID"];
            }

            // 按複製
            else
            {
                #region 複製欄位其它, 不複製 CutRef, Cutno, Cutplanid, Addname, AddDate, EditName, EditDate
                newRow["OrderID"] = oldRow["OrderID"];
                newRow["SEQ1"] = oldRow["SEQ1"];
                newRow["SEQ2"] = oldRow["SEQ2"];
                newRow["Layer"] = oldRow["Layer"];
                newRow["Colorid"] = oldRow["Colorid"];
                newRow["Markername"] = oldRow["Markername"];
                newRow["EstCutDate"] = oldRow["EstCutDate"];
                newRow["Cutcellid"] = oldRow["Cutcellid"];
                newRow["MarkerLength"] = oldRow["MarkerLength"];
                newRow["ConsPC"] = oldRow["ConsPC"];
                newRow["Cons"] = oldRow["Cons"];
                newRow["Refno"] = oldRow["Refno"];
                newRow["MarkerNo"] = oldRow["MarkerNo"];
                newRow["MarkerVersion"] = oldRow["MarkerVersion"];
                newRow["MarkerDownLoadID"] = oldRow["MarkerDownLoadID"];
                newRow["FabricCode"] = oldRow["FabricCode"];
                newRow["Order_EachconsUKey"] = oldRow["Order_EachconsUKey"];
                newRow["Article"] = oldRow["Article"];
                newRow["SizeCode"] = oldRow["SizeCode"];
                newRow["CutQty"] = oldRow["CutQty"];
                newRow["FabricPanelCode1"] = oldRow["FabricPanelCode1"];
                newRow["PatternPanel"] = oldRow["PatternPanel"];
                newRow["Fabeta"] = oldRow["Fabeta"];
                newRow["sewinline"] = oldRow["sewinline"];
                newRow["actcutdate"] = oldRow["actcutdate"];
                newRow["Adduser"] = this.LoginID;
                newRow["edituser"] = oldRow["edituser"];
                newRow["totallayer"] = oldRow["totallayer"];
                newRow["multisize"] = oldRow["multisize"];
                newRow["Order_SizeCode_Seq"] = oldRow["Order_SizeCode_Seq"];
                newRow["SORT_NUM"] = oldRow["SORT_NUM"];
                newRow["WeaveTypeID"] = oldRow["WeaveTypeID"];
                newRow["DescDetail"] = oldRow["DescDetail"];
                newRow["MarkerLengthY"] = oldRow["MarkerLengthY"];
                newRow["MarkerLengthE"] = oldRow["MarkerLengthE"];
                newRow["MtlTypeID_SCIRefno"] = oldRow["MtlTypeID_SCIRefno"];
                newRow["Description"] = oldRow["Description"];
                newRow["ActCuttingPerimeterNew"] = oldRow["ActCuttingPerimeterNew"];
                newRow["StraightLengthNew"] = oldRow["StraightLengthNew"];
                newRow["CurvedLengthNew"] = oldRow["CurvedLengthNew"];
                newRow["ActCuttingPerimeter"] = oldRow["ActCuttingPerimeterNew"];
                newRow["StraightLength"] = oldRow["StraightLengthNew"];
                newRow["CurvedLength"] = oldRow["CurvedLengthNew"];
                newRow["fromukey"] = oldRow["fromukey"];
                #endregion

                this.AddThirdDatas(this.sizeratioTb, MyUtility.Convert.GetLong(oldRow["ukey"]), MyUtility.Convert.GetLong(oldRow["newkey"]), maxkey);
                this.AddThirdDatas(this.distqtyTb, MyUtility.Convert.GetLong(oldRow["ukey"]), MyUtility.Convert.GetLong(oldRow["newkey"]), maxkey);
                this.AddThirdDatas(this.PatternPanelTb, MyUtility.Convert.GetLong(oldRow["ukey"]), MyUtility.Convert.GetLong(oldRow["newkey"]), maxkey);
            }

            oldRow.Table.Rows.InsertAt(newRow, index); // 插入新的 WorkOrder
            this.flag = false;
        }

        private void AddThirdDatas(DataTable target, long ukey, long newkey, long maxkey)
        {
            DataTable source = target.Select($"WorkOrderUkey={ukey} and newkey = {newkey}").TryCopyToDataTable(target);
            foreach (DataRow ddr in source.Rows)
            {
                ddr["WorkOrderUkey"] = 0;
                ddr["newkey"] = maxkey;
                target.ImportRowAdded(ddr);
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridDelete()
        {
            if (this.CurrentDetailData == null)
            {
                return;
            }

            long ukey = MyUtility.Convert.GetLong(this.CurrentDetailData["Ukey"]);
            long newKey = MyUtility.Convert.GetLong(this.CurrentDetailData["NewKey"]);

            // 判斷有 CutPlanID不能刪除
            if (!MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]))
            {
                MyUtility.Msg.WarningBox($"it's scheduled in P04. Cutting Daily Plan : {this.CurrentDetailData["Cutplanid"]}, can't be deleted.");
                return;
            }

            if (this.CurrentDetailData.RowState != DataRowState.Added)
            {
                string sqlchkOutput = $@"select id from CuttingOutput_Detail where WorkOrderUkey = '{ukey}'";
                if (MyUtility.Check.Seek(sqlchkOutput, out DataRow dataRow))
                {
                    MyUtility.Msg.WarningBox($"Already create output <{dataRow["id"]}>, cann't be deleted");
                    return;
                }
            }

            DeleteThirdDatas(this.sizeratioTb, ukey, newKey);
            DeleteThirdDatas(this.distqtyTb, ukey, newKey);

            base.OnDetailGridDelete();
        }

        /// <inheritdoc/>
        internal static void DeleteThirdDatas(DataTable thirdTable, long workOrderUkey, long newKey)
        {
            DataRow[] byKeyThirdDatas = thirdTable.Select($"WorkOrderUkey = '{workOrderUkey}' and NewKey = {newKey}");
            for (int i = byKeyThirdDatas.Count() - 1; i >= 0; i--)
            {
                byKeyThirdDatas[i].Delete();
            }
        }

        private void InsertSizeRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow ndr = this.sizeratioTb.NewRow();
            ndr["newkey"] = this.CurrentDetailData["newkey"];
            ndr["WorkorderUkey"] = this.CurrentDetailData["Ukey"];
            ndr["Qty"] = 0;
            this.sizeratioTb.Rows.Add(ndr);
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow selectDr = ((DataRowView)this.gridSizeRatio.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();

            this.Cal_TotalCutQty(this.CurrentDetailData["Ukey"], this.CurrentDetailData["NewKey"]);
        }

        #region MarkerLengt驗證/UnitCons/Cons計算
        private void NumMarkerLengthY_MarkerLengthY_Validated(object sender, EventArgs e)
        {
            if (this.numMarkerLengthY.Text.Trim() == string.Empty)
            {
                return;
            }

            if (this.numMarkerLengthY.OldValue == this.numMarkerLengthY.Text)
            {
                return;
            }

            int y;
            y = int.Parse(this.numMarkerLengthY.Text);
            this.CurrentDetailData["MarkerLengthY"] = y.ToString("D2");
            this.Cal_Cons(true, true);
        }

        private void TxtMarkerLengthE_MarkerLengthE_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtMarkerLengthE.OldValue == this.txtMarkerLengthE.Text)
            {
                return;
            }

            this.CurrentDetailData["MarkerLengthE"] = this.txtMarkerLengthE.Text;
            this.Cal_Cons(true, true);
        }

        private void NumUnitCons_UnitCons_Validated(object sender, EventArgs e)
        {
            // 與marklength變更規則不一樣
            decimal cp = MyUtility.Convert.GetDecimal(this.CurrentDetailData["Conspc"]);
            decimal la = MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]);
            decimal ttsr = MyUtility.Convert.GetDecimal(this.sizeratioTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey = '{0}' and newkey = '{1}'", this.CurrentDetailData["Ukey"], this.CurrentDetailData["newkey"])));
            this.CurrentDetailData["Cons"] = cp * la * ttsr;
        }

        private void Cal_Cons(bool updateConsPC, bool updateCons) // update Cons
        {
            this.GridValid();
            if (this.numMarkerLengthY.Text.Trim() == string.Empty)
            {
                return;
            }

            int sizeRatioQty;
            object comput;
            comput = this.sizeratioTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey = '{0}' and newkey = '{1}'", this.CurrentDetailData["Ukey"], this.CurrentDetailData["newkey"]));
            if (comput == DBNull.Value)
            {
                sizeRatioQty = 0;
            }
            else
            {
                sizeRatioQty = Convert.ToInt32(comput);
            }

            decimal markerLengthNum, conspc;
            string markerLengthstr, lenY, lenE;
            if (MyUtility.Check.Empty(this.CurrentDetailData["MarkerLengthY"]))
            {
                lenY = "0";
            }
            else
            {
                lenY = this.CurrentDetailData["MarkerLengthY"].ToString();
            }

            if (MyUtility.Check.Empty(this.CurrentDetailData["MarkerLengthE"]))
            {
                lenE = "0-0/0+0\"";
            }
            else
            {
                lenE = this.CurrentDetailData["MarkerLengthE"].ToString();
            }

            markerLengthstr = lenY + "Y" + lenE;
            markerLengthNum = Convert.ToDecimal(MyUtility.GetValue.Lookup(string.Format("Select dbo.MarkerLengthToYDS('{0}')", markerLengthstr)));
            if (sizeRatioQty == 0)
            {
                conspc = 0;
            }
            else
            {
                conspc = markerLengthNum / sizeRatioQty;
            }

            if (updateConsPC)
            {
                this.CurrentDetailData["Conspc"] = conspc;
            }

            if (updateCons)
            {
                if (MyUtility.Check.Empty(this.CurrentDetailData["Layer"]))
                {
                    this.CurrentDetailData["Cons"] = markerLengthNum * 0;
                }
                else
                {
                    this.CurrentDetailData["Cons"] = markerLengthNum * Convert.ToInt32(this.CurrentDetailData["Layer"]);
                }
            }

            this.txtMarkerLength.Text = markerLengthstr;
            this.txtMarkerLength.ValidateControl();
        }
        #endregion

        private void Cal_TotalCutQty(object workorderukey, object newkey)
        {
            this.GridValid();
            string totalCutQtystr;
            totalCutQtystr = string.Empty;
            DataRow[] sizeview = this.sizeratioTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} ", Convert.ToInt32(workorderukey), Convert.ToInt32(newkey)));

            foreach (DataRow dr in sizeview)
            {
                if (totalCutQtystr == string.Empty)
                {
                    totalCutQtystr = totalCutQtystr + dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(MyUtility.Check.Empty(this.CurrentDetailData["Layer"]) ? 0 : this.CurrentDetailData["Layer"])).ToString();
                }
                else
                {
                    totalCutQtystr = totalCutQtystr + "," + dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(MyUtility.Check.Empty(this.CurrentDetailData["Layer"]) ? 0 : this.CurrentDetailData["Layer"])).ToString();
                }
            }

            this.CurrentDetailData["CutQty"] = totalCutQtystr;
        }

        private void InsertNewRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow ndr = this.distqtyTb.NewRow();
            ndr["newkey"] = this.CurrentDetailData["newkey"];
            ndr["WorkorderUkey"] = this.CurrentDetailData["Ukey"];
            ndr["Qty"] = 0;
            this.distqtyTb.Rows.Add(ndr);
        }

        private void DeleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow selectDr = ((DataRowView)this.gridDistributetoSPNo.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();

            this.TotalDisQty();
        }

        #region Save Before Post After

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            this.GridValid();

            int index = 0;
            foreach (DataRow row in this.DetailDatas.Where(x => MyUtility.Check.Empty(x["MarkerNo"].ToString())))
            {
                index = this.DetailDatas.IndexOf(row);
                this.detailgrid.SelectRowTo(index);
                MyUtility.Msg.WarningBox("Marker No cannot be empty.");
                return false;
            }

            foreach (DataRow row in this.DetailDatas.Where(x => MyUtility.Check.Empty(x["FabricPanelCode"].ToString())))
            {
                index = this.DetailDatas.IndexOf(row);
                this.detailgrid.SelectRowTo(index);
                MyUtility.Msg.WarningBox("Fab_Panel Code cannot be empty.");
                return false;
            }

            foreach (DataRow row in this.DetailDatas.Where(x => MyUtility.Check.Empty(x["CutRef"].ToString()) && !MyUtility.Check.Empty(x["CutNo"].ToString())))
            {
                // 與該筆相同 FabricCombo、Cut# 的資料
                List<DataRow> sameDatas = this.DetailDatas.Where(o =>
                                                                o["CutRef"].ToString() == string.Empty
                                                                && o["CutNo"].ToString() == row["CutNo"].ToString()
                                                                && o["FabricCombo"].ToString() == row["FabricCombo"].ToString()
                                                                && o["Ukey"].ToString() != row["Ukey"].ToString()).ToList();

                if (sameDatas.Count > 0)
                {
                    var signgleData = sameDatas.Where(x => x["MarkerName"].ToString() != row["MarkerName"].ToString() ||
                                                           x["MarkerNo"].ToString() != row["MarkerNo"].ToString())
                                               .ToList();
                    if (signgleData.Count > 0)
                    {
                        index = this.DetailDatas.IndexOf(row);
                        this.detailgrid.SelectRowTo(index);
                        MyUtility.Msg.WarningBox("In the same fabric combo, different 'Marker Name' and 'Marker No' cannot cut in one time which means cannot set the same cut#.");
                        return false;
                    }
                }
            }

            var query = this.DetailDatas.Where(x => x.RowState != DataRowState.Deleted &&
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
            DataRow[] sizeratioTbrow = this.sizeratioTb.AsEnumerable().Where(
                                x => x.RowState != DataRowState.Deleted &&
                                (MyUtility.Convert.GetInt(x["Qty"]) == 0 || MyUtility.Check.Empty(x["SizeCode"]))).ToArray();
            for (int i = sizeratioTbrow.Count() - 1; i >= 0; i--)
            {
                sizeratioTbrow[i].Delete();
            }

            DataRow[] distqtyTbrow = this.distqtyTb.AsEnumerable().Where(
                                x => this.DistributeToSPSaveBeforeCheck(x)).ToArray();
            for (int i = distqtyTbrow.Count() - 1; i >= 0; i--)
            {
                distqtyTbrow[i].Delete();
            }
            #endregion

            string msg = string.Empty;
            string sqlcmd = "Select SizeCode,WorkOrderUkey,NewKey,Count() as countN from #tmp having countN >1 Group by SizeCode,WorkOrderUkey,NewKey";
            MyUtility.Tool.ProcessWithDatatable(this.sizeratioTb, "SizeCode,WorkOrderUkey,NewKey", sqlcmd, out DataTable dt);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    msg = msg + dr["WorkOrderUkey"].ToString() + "\n";
                }
            }

            if (!MyUtility.Check.Empty(msg))
            {
                MyUtility.Msg.WarningBox("The SizeRatio duplicate ,Please see below <Ukey> \n" + msg);
                return false;
            }

            sqlcmd = @"
Select OrderID,Article,SizeCode,WorkOrderUkey,NewKey,Count(1) as countN
from #tmp
Group by OrderID,Article,SizeCode,WorkOrderUkey,NewKey
having Count(1) >1";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.distqtyTb, string.Empty, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            foreach (DataRow dr in dt.Rows)
            {
                msg = msg + dr["WorkOrderUkey"].ToString() + "\n";
            }

            if (!MyUtility.Check.Empty(msg))
            {
                MyUtility.Msg.WarningBox("The Distribute Qty data duplicate ,Please see below <Ukey> \n" + msg);
                return false;
            }

            sqlcmd = @"
Select WorkOrderUkey,PatternPanel,FabricPanelCode,NewKey,Count(1) as countN
from #tmp
Group by WorkOrderUkey,PatternPanel,FabricPanelCode,NewKey
having Count(1) >1";
            result = MyUtility.Tool.ProcessWithDatatable(this.PatternPanelTb, string.Empty, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            foreach (DataRow dr in dt.Rows)
            {
                msg = msg + dr["WorkOrderUkey"].ToString() + "\n";
            }

            if (!MyUtility.Check.Empty(msg))
            {
                MyUtility.Msg.WarningBox("The PatternPanel data duplicate ,Please see below <Ukey> \n" + msg);
                return false;
            }

            #region 檢查每一筆 Total distributionQty是否大於TotalCutQty總和
            foreach (DataRow dr_d in this.DetailDatas.Where(x => x.RowState != DataRowState.Deleted))
            {
                decimal ttlcutqty = 0, ttldisqty = 0;
                DataRow[] sizedr = this.sizeratioTb.Select(string.Format("newkey = '{0}' and workorderUkey= '{1}'", dr_d["newkey"].ToString(), dr_d["Ukey"].ToString()));
                DataRow[] distdr = this.distqtyTb.Select(string.Format("newkey = '{0}' and workorderUkey= '{1}'", dr_d["newkey"].ToString(), dr_d["Ukey"].ToString()));
                ttlcutqty = sizedr.Sum(x => x.Field<decimal>("Qty")) * MyUtility.Convert.GetDecimal(dr_d["Layer"]);
                ttldisqty = distdr.Sum(x => x.Field<decimal>("Qty"));
                if (ttlcutqty < ttldisqty)
                {
                    this.ShowErr(string.Format("Key:{0} Distribution Qty can not exceed total Cut qty", dr_d["Ukey"].ToString()));
                    return false;
                }
            }
            #endregion
            this.CurrentMaintain["cutinline"] = ((DataTable)this.detailgridbs.DataSource).Compute("Min(estcutdate)", null);
            this.CurrentMaintain["CutOffLine"] = ((DataTable)this.detailgridbs.DataSource).Compute("MAX(estcutdate)", null);

            return base.ClickSaveBefore();
        }

        private bool DistributeToSPSaveBeforeCheck(DataRow distDr)
        {
            if (distDr.RowState != DataRowState.Deleted &&
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

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            #region RevisedMarkerOriginalData 非AdditionalRevisedMarker功能增加的資料 isbyAdditionalRevisedMarker == 0
            string sqlInsertRevisedMarkerOriginalData = string.Empty;
            sqlInsertRevisedMarkerOriginalData += "declare @ID bigint";
            foreach (DataRow dr in this.DetailDatas.Where(w => (w.RowState == DataRowState.Modified) &&
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
            foreach (DataRow dr in this.DetailDatas.Where(w => (w.RowState == DataRowState.Modified) &&
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

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            #region RevisedMarkerOriginalData AdditionalRevisedMarker功能處理的資料, 在此取拆出來資料的ukey,處理刪除的資料
            string sqlUpdateRevisedMarkerOriginalData = string.Empty;
            var listAdditionalRevisedMarkerSeparate = this.DetailDatas.Where(w => (w.RowState == DataRowState.Modified || w.RowState == DataRowState.Added) &&
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
                delete WorkOrderRevisedMarkerOriginalData_Detail where WorkorderUkey = {dr["Ukey", DataRowVersion.Original]}
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
            foreach (DataRow dr in this.DetailDatas.Where(w => w.RowState != DataRowState.Deleted))
            {
                ukey = Convert.ToInt32(dr["Ukey"]);
                newkey = Convert.ToInt32(dr["Newkey"]);

                dray = this.sizeratioTb.Select(string.Format("newkey={0} and workorderUkey= 0", newkey)); // 0表示新增
                foreach (DataRow dr2 in dray)
                {
                    dr2["WorkOrderUkey"] = ukey;
                }

                dray = this.distqtyTb.Select(string.Format("newkey={0} and workorderUkey= 0", newkey)); // 0表示新增
                foreach (DataRow dr2 in dray)
                {
                    dr2["WorkOrderUkey"] = ukey;
                }

                dray = this.PatternPanelTb.Select(string.Format("newkey={0} and workorderUkey= 0", newkey)); // 0表示新增
                foreach (DataRow dr2 in dray)
                {
                    dr2["WorkOrderUkey"] = ukey;
                }
            }

            string delsql = string.Empty, updatesql = string.Empty, insertsql = string.Empty;
            string cId = this.CurrentMaintain["ID"].ToString();
            #region SizeRatio 修改
            #region 刪除
            foreach (DataRow dr in this.sizeratioTb.AsEnumerable().Where(x => x.RowState == DataRowState.Deleted))
            {
                delsql += string.Format("Delete From WorkOrder_SizeRatio Where WorkOrderUkey={0} and SizeCode ='{1}' and ID ='{2}';", dr["WorkOrderUkey", DataRowVersion.Original], dr["SizeCode", DataRowVersion.Original], cId);
            }
            #endregion
            #region 修改
            foreach (DataRow dr in this.sizeratioTb.AsEnumerable().Where(x => x.RowState == DataRowState.Modified))
            {
                updatesql += string.Format("Update WorkOrder_SizeRatio set Qty = {0},SizeCode = '{4}' where WorkOrderUkey ={1} and SizeCode = '{2}' and id ='{3}';", dr["Qty"], dr["WorkOrderUkey"], dr["SizeCode", DataRowVersion.Original], cId, dr["SizeCode"]);
            }
            #endregion
            #region 新增
            foreach (DataRow dr in this.sizeratioTb.AsEnumerable().Where(x => x.RowState == DataRowState.Added))
            {
                insertsql += string.Format("Insert into WorkOrder_SizeRatio(WorkOrderUkey,SizeCode,Qty,ID) values({0},'{1}',{2},'{3}'); ", dr["WorkOrderUkey"], dr["SizeCode"], dr["Qty"], cId);
            }
            #endregion
            #endregion
            #region Distribute 修改
            #region 刪除
            foreach (DataRow dr in this.distqtyTb.AsEnumerable().Where(x => x.RowState == DataRowState.Deleted))
            {
                delsql += string.Format("Delete From WorkOrder_distribute Where WorkOrderUkey={0} and SizeCode ='{1}' and Article = '{2}' and OrderID = '{3}' and id='{4}';", dr["WorkOrderUkey", DataRowVersion.Original], dr["SizeCode", DataRowVersion.Original], dr["Article", DataRowVersion.Original], dr["Orderid", DataRowVersion.Original], cId);
            }
            #endregion
            #region 修改
            foreach (DataRow dr in this.distqtyTb.AsEnumerable().Where(x => x.RowState == DataRowState.Modified))
            {
                updatesql += $@"
Update WorkOrder_distribute
set Qty = {dr["Qty"]},SizeCode = '{dr["SizeCode"]}',Article = '{dr["Article"]}',OrderID = '{dr["OrderID"]}'
where WorkOrderUkey ={dr["WorkOrderUkey"]} 
and SizeCode = '{dr["SizeCode", DataRowVersion.Original]}'
and Article = '{dr["Article", DataRowVersion.Original]}'
and OrderID = '{dr["OrderID", DataRowVersion.Original]}'
and ID ='{dr["ID", DataRowVersion.Original]}'; ";
            }
            #endregion
            #region 新增
            foreach (DataRow dr in this.distqtyTb.AsEnumerable().Where(x => x.RowState == DataRowState.Added))
            {
                insertsql += string.Format("Insert into WorkOrder_distribute(WorkOrderUkey,SizeCode,Qty,Article,OrderID,ID) values({0},'{1}',{2},'{3}','{4}','{5}'); ", dr["WorkOrderUkey"], dr["SizeCode"], dr["Qty"], dr["Article"], dr["OrderID"], cId);
            }
            #endregion
            #endregion
            #region PatternPanel 刪除/新增/(無修改)
            #region 刪除
            foreach (DataRow dr in this.PatternPanelTb.AsEnumerable().Where(x => x.RowState == DataRowState.Deleted))
            {
                delsql += string.Format("Delete From WorkOrder_PatternPanel Where WorkOrderUkey={0} and PatternPanel ='{1}' and FabricPanelCode = '{2}' ;", dr["WorkOrderUkey", DataRowVersion.Original], dr["PatternPanel", DataRowVersion.Original], dr["FabricPanelCode", DataRowVersion.Original]);
            }
            #endregion
            #region 新增
            foreach (DataRow dr in this.PatternPanelTb.AsEnumerable().Where(x => x.RowState == DataRowState.Added))
            {
                insertsql += string.Format("Insert into WorkOrder_PatternPanel(WorkOrderUkey,PatternPanel,FabricPanelCode,ID) values({0},'{1}','{2}','{3}'); ", dr["WorkOrderUkey"], dr["PatternPanel"], dr["FabricPanelCode"], cId);
            }
            #endregion
            #endregion

            #region 回寫orders CutInLine,CutOffLine
            string cutInLine, cutOffLine;

            // aa = Convert.ToDateTime(((DataTable)detailgridbs.DataSource).Compute("Min(estcutdate)", null));
            cutInLine = ((DataTable)this.detailgridbs.DataSource).Compute("Min(estcutdate)", null) == DBNull.Value ? string.Empty : Convert.ToDateTime(((DataTable)this.detailgridbs.DataSource).Compute("Min(estcutdate)", null)).ToString("yyyy-MM-dd HH:mm:ss");
            cutOffLine = ((DataTable)this.detailgridbs.DataSource).Compute("Max(estcutdate)", null) == DBNull.Value ? string.Empty : Convert.ToDateTime(((DataTable)this.detailgridbs.DataSource).Compute("Max(estcutdate)", null)).ToString("yyyy-MM-dd HH:mm:ss");
            updatesql += string.Format("Update orders set CutInLine = iif('{0}' = '',null,'{0}'),CutOffLine =  iif('{1}' = '',null,'{1}') where POID = '{2}';", cutInLine, cutOffLine, this.CurrentMaintain["ID"]);

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
            var listChangedDetail = ((DataTable)this.detailgridbs.DataSource)
                .AsEnumerable()
                .Where(s => s.RowState == DataRowState.Added || (s.RowState == DataRowState.Modified && s.CompareDataRowVersionValue(compareCol)));

            if (listChangedDetail.Any())
            {
                DataTable dtWorkOrder = listChangedDetail.TryCopyToDataTable((DataTable)this.detailgridbs.DataSource);
                Task.Run(() => new Guozi_AGV().SentWorkOrderToAGV(dtWorkOrder));
            }
            #endregion

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            foreach (DataRow dr in this.DetailDatas)
            {
                dr["SORT_NUM"] = 0;  // 編輯後存檔，將[SORT_NUM]歸零
            }

            this.OnDetailEntered();
        }
        #endregion

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            P02_Print callNextForm;
            if (this.drTEMP != null)
            {
                callNextForm = new P02_Print(this.drTEMP, this.CurrentMaintain["ID"].ToString(), MyUtility.Convert.GetInt(this.CurrentMaintain["WorkType"]));
                callNextForm.ShowDialog(this);
            }
            else if (this.drTEMP == null && this.CurrentDetailData != null)
            {
                callNextForm = new P02_Print(this.CurrentDetailData, this.CurrentMaintain["ID"].ToString(), MyUtility.Convert.GetInt(this.CurrentMaintain["WorkType"]));
                callNextForm.ShowDialog(this);
            }
            else
            {
                MyUtility.Msg.InfoBox("No datas");
                return false;
            }

            return base.ClickPrint();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            // 編輯時，將[SORT_NUM]賦予流水號
            base.ClickEditAfter();

            // 編輯時，將[SORT_NUM]賦予流水號
            int serial = 1;
            this.detailgridbs.SuspendBinding();
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["SORT_NUM"] = serial;
                serial++;
            }

            this.detailgridbs.ResumeBinding();
            this.detailgrid.SelectRowTo(0);
        }

        private void TxtBoxMarkerNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentDetailData["Cutplanid"]) && this.EditMode)
            {
                string sqlCmd = $@"
select distinct a.MarkerNo from Order_EachCons a
inner join orders b on a.id = b.ID
where b.poid = '{this.CurrentMaintain["ID"]}'
";
                SelectItem item = new SelectItem(sqlCmd, "20", this.txtBoxMarkerNo.Text);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.txtBoxMarkerNo.Text = item.GetSelectedString();
            }
        }

        private void TxtBoxMarkerNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                if (!MyUtility.Check.Seek($@"
select 1 from Order_EachCons a
inner join orders b on a.id = b.ID
where b.poid = '{this.CurrentMaintain["ID"]}' and a.MarkerNo='{this.txtBoxMarkerNo.Text}'
"))
                {
                    MyUtility.Msg.WarningBox(string.Format("<MarkerNO: {0} > is not found!", this.txtBoxMarkerNo.Text));
                    this.CurrentDetailData["MarkerNo"] = string.Empty;
                    e.Cancel = true;
                    return;
                }
            }

            if (MyUtility.Check.Empty(this.txtBoxMarkerNo.Text))
            {
                MyUtility.Msg.WarningBox(string.Format("<MarkerNO > cannot be null"));
                this.CurrentDetailData["MarkerNo"] = string.Empty;
                e.Cancel = true;
                return;
            }

            return;
        }

        private void TxtFabricPanelCode_Validating(object sender, CancelEventArgs e)
        {
            string new_FabricPanelCode = this.txtFabricPanelCode.Text;
            string sqlcmd = $@"
select ob.SCIRefno,f.Description ,f.WeaveTypeID,ob.Refno
from Order_BoF ob 
left join Fabric f on ob.SCIRefno = f.SCIRefno
where exists (select id from Order_FabricCode ofa where ofa.id = '{this.CurrentMaintain["ID"]}' and ofa.FabricPanelCode = '{new_FabricPanelCode}'
and ofa.id = ob.id and ofa.FabricCode = ob.FabricCode)
";

            if (MyUtility.Check.Seek(sqlcmd, out DataRow dr))
            {
                this.CurrentDetailData["Refno"] = dr["Refno"].ToString();
                this.CurrentDetailData["SCIRefno"] = dr["SCIRefno"].ToString();
                this.CurrentDetailData["MtlTypeID_SCIRefno"] = dr["WeaveTypeID"].ToString() + " / " + dr["SCIRefno"].ToString();
                this.CurrentDetailData["Description"] = dr["Description"].ToString();
                this.CurrentDetailData["FabricPanelCode"] = new_FabricPanelCode;
            }
            else
            {
                MyUtility.Msg.WarningBox(string.Format("This FabricPanelCode<{0}> is wrong", this.txtFabricPanelCode.Text));
                this.CurrentDetailData["FabricPanelCode"] = string.Empty;
                e.Cancel = true;
                return;
            }
        }

        private void DisplayTime_DoubleClick(object sender, EventArgs e)
        {
            if (this.CurrentDetailData == null)
            {
                return;
            }

            var frm = new P02_OriginalData(this.CurrentDetailData);
            frm.ShowDialog(this);
        }

        private void BtnAdditionalrevisedmarker_Click(object sender, EventArgs e)
        {
            this.isAdditionalrevisedmarker = true;
            this.OnDetailGridInsert(-1);
            this.isAdditionalrevisedmarker = false;
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.tabs.TabPages[0].Equals(this.tabs.SelectedTab))
            {
                this.btnCutplanChangeHistory.Enabled = true;
            }
            else
            {
                this.btnCutplanChangeHistory.Enabled = false;
            }
        }

        private void BtnCutplanChangeHistory_Click(object sender, EventArgs e)
        {
            if (this.callP07 != null && this.callP07.Visible == true)
            {
                this.callP07.P07Data(this.CurrentMaintain["ID"].ToString());
                this.callP07.Activate();
            }
            else
            {
                this.P07FormOpen();
            }
        }

        // Quantity Breakdown
        private void Qtybreak_Click(object sender, EventArgs e)
        {
            MyUtility.Check.Seek(string.Format("select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList from Orders o WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"]), out DataRow dr);
            PPIC.P01_Qty callNextForm = new PPIC.P01_Qty(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), dr["PoList"].ToString());
            callNextForm.ShowDialog(this);
        }

        // PatternPanel
        private void BtnPatternPanel_Click(object sender, EventArgs e)
        {
            var form = new P02_PatternPanel_n(this.PatternPanelTb, this.CurrentDetailData);
            form.ShowDialog();
            this.CurrentDetailData["PatternPanel"] = this.PatternPanelTb
                .Select($@"Workorderukey = {this.CurrentDetailData["Ukey"]} and newkey = {this.CurrentDetailData["NewKey"]}")
                .AsEnumerable()
                .Where(w => w.RowState != DataRowState.Deleted)
                .Select(s => MyUtility.Convert.GetString(s["PatternPanel"])).ToList().JoinToString("+");
        }

        private void BtnDist_Click(object sender, EventArgs e)
        {
            this.GridValid();
            this.detailgrid.ValidateControl();
            var frm = new P02_AutoDistToSP(this.CurrentDetailData, this.sizeratioTb, this.distqtyTb, this.PatternPanelTb);
            frm.ShowDialog(this);
        }

        private void GridSizeRatio_EditingKeyProcessing(object sender, Ict.Win.UI.DataGridViewEditingKeyProcessingEventArgs e)
        {
            if (this.EditMode && e.KeyCode == Keys.Enter)
            {
                int rowindex = this.gridSizeRatio.CurrentCell.RowIndex;
                if (rowindex == this.gridSizeRatio.RowCount - 1)
                {
                    DataRow ndr = this.sizeratioTb.NewRow();
                    ndr["newkey"] = this.CurrentDetailData["newkey"];
                    ndr["WorkorderUkey"] = this.CurrentDetailData["Ukey"];
                    ndr["Qty"] = 0;
                    this.sizeratioTb.Rows.Add(ndr);
                }
            }
        }

        private void BtnImportMarker_Click(object sender, EventArgs e)
        {
            string id = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            string sqlcmd = $@"
select top 1 s.SizeGroup, s.PatternNo, oe.markerNo, s.ID, p.Version
from Order_EachCons oe 
inner join dbo.SMNotice s on oe.SMNoticeID = s.ID
inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
inner join Pattern p with(nolock)on p.id = sd.id
where oe.ID = '{id}'
and sd.PhaseID = 'Bulk'
and p.Status='Completed'
order by p.EditDate desc
";
            if (MyUtility.Check.Seek(sqlcmd, out DataRow drSMNotice))
            {
                string styleUkey = MyUtility.GetValue.Lookup($@"select o.StyleUkey from Orders o where o.id = '{id}'");
                var form = new P02_ImportML(styleUkey, id, drSMNotice, (DataTable)this.detailgridbs.DataSource);
                form.ShowDialog();
            }
            else
            {
                MyUtility.Msg.InfoBox("Not found SMNotice Datas"); // 正常不會發生這狀況
            }

            #region 產生第3層 PatternPanel 只有一筆
            this.DetailDatas.AsEnumerable().Where(w => MyUtility.Convert.GetBool(w["ImportML"])).ToList().ForEach(row =>
            {
                DataRow drNEW = this.PatternPanelTb.NewRow();
                drNEW["id"] = this.CurrentMaintain["ID"];
                drNEW["WorkOrderUkey"] = 0;  // 新增WorkOrderUkey塞0
                drNEW["PatternPanel"] = row["PatternPanel"];
                drNEW["FabricPanelCode"] = row["FabricPanelCode"];
                this.PatternPanelTb.Rows.Add(drNEW);
            });
            #endregion

            int icount = this.DetailDatas.AsEnumerable().Where(w => MyUtility.Convert.GetBool(w["ImportML"])).Count();
            for (int i = 0; i <= icount; i++)
            {
                this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["Layer"]; // 移動到指定cell 觸發 Con 計算
            }

            this.detailgrid.SelectRowTo(0);
        }

        private void Distribute_grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            this.RenewData();
            this.Sorting(this.comboBox1.Text);  // 避免順序亂掉
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.RenewData();
            this.OnDetailEntered();
        }

        private void GridDistributetoSPNo_SelectionChanged(object sender, EventArgs e)
        {
            // 更換qtybreakdown index
            DataRow spNoRow = this.gridDistributetoSPNo.GetDataRow(this.gridDistributetoSPNo.GetSelectedRowIndex());
            if (MyUtility.Check.Empty(spNoRow))
            {
                return;
            }

            string article = spNoRow["Article"].ToString();
            string sizeCode = spNoRow["SizeCode"].ToString();
            string spNo = spNoRow["Orderid"].ToString();
            int rowIndex = 0;

            if (!MyUtility.Check.Empty(this.distqtyTb) || this.distqtyTb.Rows.Count > 1)
            {
                for (int rIdx = 0; rIdx < this.gridQtyBreakdown.Rows.Count; rIdx++)
                {
                    DataGridViewRow dvr = this.gridQtyBreakdown.Rows[rIdx];
                    DataRow row = ((DataRowView)dvr.DataBoundItem).Row;
                    if (row["article"].ToString() == article && row["SizeCode"].ToString() == sizeCode && row["id"].ToString() == spNo)
                    {
                        rowIndex = rIdx;
                        break;
                    }
                }

                this.gridQtyBreakdown.SelectRowTo(rowIndex);
            }
        }

        /// <summary>
        /// 確認【布】寬是否會超過【裁桌】的寬度
        /// 若是 CutCell 取得寬度為 0 ，則不顯示訊息
        /// </summary>
        /// <param name="strCutCellID">CutCellID</param>
        /// <param name="strSCIRefno">SciRefno</param>
        private void CheckCuttingWidth(string strCutCellID, string strSCIRefno)
        {
            string chkwidth = MyUtility.GetValue.Lookup($@"select width_cm = width*2.54  from Fabric where SCIRefno = '{strSCIRefno}'");
            string strCuttingWidth = MyUtility.GetValue.Lookup($@"
select cuttingWidth = isnull (cuttingWidth, 0) 
from CutCell 
where id = '{strCutCellID}'
and MDivisionID = '{this.KeyWord}'");
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

        private void ComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            this.GridValid();
            this.detailgrid.ValidateControl();
            this.Sorting(this.comboBox1.Text);
        }

        private P07 callP07 = null;

        private void P07FormOpen()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is P07)
                {
                    form.Activate();
                    P07 activateForm = (P07)form;
                    activateForm.setTxtSPNo(this.CurrentMaintain["ID"].ToString());
                    activateForm.Queryable();
                    return;
                }
            }

            ToolStripMenuItem p07MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Sci.Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Cutting"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P07. Query for Change Est. Cut Date Record"))
                            {
                                p07MenuItem = (ToolStripMenuItem)subMenuItem;
                                break;
                            }
                        }
                    }
                }
            }

            this.callP07 = new P07(p07MenuItem)
            {
                MdiParent = this.MdiParent,
            };
            this.callP07.Show();
            this.callP07.P07Data(this.CurrentMaintain["ID"].ToString());
        }

        /// <inheritdoc/>
        public static void ProcessColumns(DataRow currentRow)
        {
            // MarkerLengthY, MarkerLengthE, ActCuttingPerimeterNew, StraightLengthNew, CurvedLengthNew
            if (!MyUtility.Check.Empty(currentRow["MarkerLength"]))
            {
                string markerLength = MyUtility.Convert.GetString(currentRow["MarkerLength"]);
                int indexY = markerLength.IndexOf("Ｙ");
                currentRow["markerLengthY"] = markerLength.Substring(0, indexY).PadLeft(2, '0');
                currentRow["markerLengthE"] = markerLength.Substring(indexY + 1);
            }

            PadLeftTen("ActCuttingPerimeter", currentRow);
            PadLeftTen("StraightLength", currentRow);
            PadLeftTen("CurvedLength", currentRow);
        }

        /// <inheritdoc/>
        private static void PadLeftTen(string columnName, DataRow currentRow)
        {
            if (!MyUtility.Check.Empty(currentRow[columnName]))
            {
                currentRow[columnName + "New"] = MyUtility.Convert.GetString(currentRow[columnName]).PadLeft(10, '0');
            }
        }
    }
}
