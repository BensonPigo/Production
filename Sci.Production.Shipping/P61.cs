using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P61 : Win.Tems.Input6
    {
        private readonly List<string> customsTypelist = new List<string>() { "Fabric", "Accessory", "Machine" };
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Fty;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ShipMode;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Vessel;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Port;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Refno;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_ETA;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_PortDate;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_WHDate;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_NW;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_GW;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_ActNetKg;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_ActWeightKg;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_ActAmount;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ActHSCode;
        /// <inheritdoc/>
        public static DataTable ShareDt { get; set; }

        /// <inheritdoc/>
        public P61(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.labelNotApprove.Text = this.CurrentMaintain["status"].ToString();
            this.displayPortofDischarge.Text = "KH";

            this.ReCalculat();
            this.InitReadOnly(true);
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select id2.* 
,kd.CustomsType
,[CustomsDescription] = kd.CDCName
,[CDCAmount] = id2.CDCQty * id2.CDCUnitPrice
,[NWDiff] = id2.ActNetKg - id2.NetKg
,[HSCode] = kcd.HSCode
,kd.CDCCode
,rn = row_number()over(order by id2.ukey)
from KHImportDeclaration_Detail id2
inner join KHImportDeclaration i on i.ID = id2.ID
left join KHCustomsItem kc on kc.RefNo=id2.Refno
left join KHCustomsItem_Detail kcd on kc.Ukey=kcd.KHCustomsItemUkey 
    and kcd.KHCustomsItemUkey = id2.KHCustomsItemUkey and kcd.Port = i.ImportPort
left join KHCustomsDescription kd on kd.CDCName = kc.KHCustomsDescriptionCDCName
    and kd.CustomsType in ('Fabric', 'Accessory', 'Machine')
left join KHCustomsDescription_Detail kdd on kd.CDCName=kdd.CDCName and kdd.PurchaseUnit = id2.UnitId   
where id2.id = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region Setting
            DataGridViewGeneratorTextColumnSettings fty_setting = new DataGridViewGeneratorTextColumnSettings();
            fty_setting.CellValidating += (s, e) =>
            {
                if (!this.EditMode || this.CurrentDetailData == null || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string sqlcmd = $@"select distinct FTYGroup from Factory where IsProduceFty = 1 and id ='{e.FormattedValue}'";

                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                    e.Cancel = true;
                    return;
                }

                dr["FactoryID"] = e.FormattedValue;
                dr.EndEdit();
            };

            fty_setting.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null || !this.EditMode || this.col_Fty.IsEditingReadOnly)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"select distinct FTYGroup from Factory where IsProduceFty = 1";
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "20", this.CurrentDetailData["FactoryID"].ToString(), "FTYGroup");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    dr["FactoryID"] = item.GetSelecteds()[0]["FTYGroup"];
                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorTextColumnSettings shipMode_setting = new DataGridViewGeneratorTextColumnSettings();
            shipMode_setting.CellValidating += (s, e) =>
            {
                if (!this.EditMode || this.CurrentDetailData == null || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string sqlcmd = $@"select * from ShipMode sm where Junk = 0 and id ='{e.FormattedValue}'";

                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                    e.Cancel = true;
                    return;
                }

                dr["ShipModeID"] = e.FormattedValue;
                dr.EndEdit();
            };

            shipMode_setting.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null || !this.EditMode || this.col_ShipMode.IsEditingReadOnly)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"select id,Description from ShipMode sm where Junk =0";
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "15,30", this.CurrentDetailData["ShipModeID"].ToString(), "id,Description");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    dr["ShipModeID"] = item.GetSelecteds()[0]["id"];
                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorTextColumnSettings port_setting = new DataGridViewGeneratorTextColumnSettings();
            port_setting.CellValidating += (s, e) =>
            {
                if (!this.EditMode || this.CurrentDetailData == null || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string sqlcmd = $@"select ID, Name from Port where Junk = 0 and id ='{e.FormattedValue}'";

                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                    e.Cancel = true;
                    return;
                }

                dr["ExportPort"] = e.FormattedValue;
                dr.EndEdit();
            };

            port_setting.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null || !this.EditMode || this.col_Port.IsEditingReadOnly)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"select ID, Name from Port where Junk = 0";
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "20,15", this.CurrentDetailData["ExportPort"].ToString(), "ID, Name");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    dr["ExportPort"] = item.GetSelecteds()[0]["id"];
                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorTextColumnSettings refno_setting = new DataGridViewGeneratorTextColumnSettings();
            refno_setting.CellValidating += (s, e) =>
            {
                if (!this.EditMode || this.CurrentDetailData == null || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                DataRow drSeek;
                string sqlcmd = $@"
Select Refno,Description,Unit 
from view_KHImportItem 
where RefNo='{e.FormattedValue}' and junk=0
";

                if (!MyUtility.Check.Seek(sqlcmd, out drSeek))
                {
                    MyUtility.Msg.WarningBox("There is no this <Ref#>.");
                    dr["Description"] = string.Empty;
                    dr["UnitID"] = string.Empty;
                    dr["Refno"] = string.Empty;
                    e.Cancel = true;
                    dr.EndEdit();
                    return;
                }

                dr["Description"] = drSeek["Description"];
                dr["UnitID"] = drSeek["Unit"];

                sqlcmd = $@"
Select kd.CDCUnit, Ki.CDCUnitPrice,kd.CustomsType
,[CustomsDescription] = kd.CDCName  
, [CDCUnit] = kd.CDCUnit  
, [CDCUnitPrice] = ki.CDCUnitPrice  
from view_KHImportItem vk
inner join KHCustomsItem ki on vk.Refno=Ki.Refno
inner join KHCustomsDescription kd on  kd.CDCName=ki.KHCustomsDescriptionCDCName and vk.CustomsType = kd.CustomsType
inner join KHCustomsDescription_Detail kdd on  kd.CDCName=kdd.CDCName and kdd.PurchaseUnit = vk.Unit
where vk.Refno = '{e.FormattedValue}'
";
                if (MyUtility.Check.Seek(sqlcmd, out drSeek))
                {
                    dr["CustomsType"] = drSeek["CustomsType"];
                    dr["CustomsDescription"] = drSeek["CustomsDescription"];
                    dr["CDCUnit"] = drSeek["CDCUnit"];
                    dr["CDCUnitPrice"] = drSeek["CDCUnitPrice"];
                }
                else
                {
                    dr["CustomsType"] = string.Empty;
                    dr["CustomsDescription"] = string.Empty;
                    dr["CDCUnit"] = string.Empty;
                    dr["CDCUnitPrice"] = 0;
                }

                dr["Refno"] = e.FormattedValue;
                dr.EndEdit();
            };

            refno_setting.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null || !this.EditMode || this.col_Refno.IsEditingReadOnly)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"Select SCIRefno,Refno,CustomsType,Description,Unit from view_KHImportItem where Junk = 0";
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "23,20,12,35,10", this.CurrentDetailData["Refno"].ToString(), "SCIRefno,Refno,CustomsType,Description,Unit");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    dr["Refno"] = item.GetSelecteds()[0]["SCIRefno"];
                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorNumericColumnSettings qty_setting = new DataGridViewGeneratorNumericColumnSettings();
            qty_setting.CellValidating += (s, e) =>
            {
                if (!this.EditMode || this.CurrentDetailData == null)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["Qty"] = 0;
                    dr["CDCQty"] = 0;
                    dr["CDCAmount"] = 0;
                    dr.EndEdit();
                }

                DataRow drSeek;
                decimal qty = MyUtility.Convert.GetDecimal(e.FormattedValue);
                string sqlcmd = $@"
Select [CDCQty] = kdd.Ratio * {qty} 
, [CDCAmount] = kdd.Ratio * {qty} * ki.CDCUnitPrice  
from view_KHImportItem vk
inner join KHCustomsItem ki on vk.Refno=Ki.Refno
inner join KHCustomsDescription kd on  kd.CDCName=ki.KHCustomsDescriptionCDCName and vk.CustomsType = kd.CustomsType
inner join KHCustomsDescription_Detail kdd on  kd.CDCName=kdd.CDCName and kdd.PurchaseUnit = vk.Unit
where vk.Refno = '{dr["Refno"]}'
";
                if (MyUtility.Check.Seek(sqlcmd, out drSeek))
                {
                    dr["CDCQty"] = drSeek["CDCQty"];
                    dr["CDCAmount"] = drSeek["CDCAmount"];
                }
                else
                {
                    dr["CDCQty"] = 0;
                    dr["CDCAmount"] = 0;
                }

                dr["Qty"] = e.FormattedValue;
                dr.EndEdit();
            };

            DataGridViewGeneratorNumericColumnSettings actnum = new DataGridViewGeneratorNumericColumnSettings();
            actnum.CellPainting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                e.CellStyle.BackColor = this.customsTypelist.Contains(MyUtility.Convert.GetString(dr["CustomsType"])) ? Color.White : Color.Pink;
                e.CellStyle.ForeColor = !this.EditMode ? Color.Black :
                                        this.customsTypelist.Contains(MyUtility.Convert.GetString(dr["CustomsType"])) ? Color.Black : Color.Red;
            };

            DataGridViewGeneratorTextColumnSettings actHSCode = new DataGridViewGeneratorTextColumnSettings();
            actHSCode.CellPainting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                e.CellStyle.BackColor = this.customsTypelist.Contains(MyUtility.Convert.GetString(dr["CustomsType"])) ? Color.White : Color.Pink;
                e.CellStyle.ForeColor = !this.EditMode ? Color.Black :
                                        this.customsTypelist.Contains(MyUtility.Convert.GetString(dr["CustomsType"])) ? Color.Black : Color.Red;
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
           .Text("ExportID", header: "WK#", width: Widths.AnsiChars(13), iseditingreadonly: true)
           .Text("Consignee", header: "Consignee", width: Widths.AnsiChars(8), iseditingreadonly: false)
           .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(8), settings: fty_setting, iseditingreadonly: true).Get(out this.col_Fty)
           .Date("ETA", header: "ETA", width: Widths.AnsiChars(10), iseditingreadonly: true).Get(out this.col_ETA) // Edit on AddRow by hand
           .Date("PortArrival", header: "Arrived Port Date", width: Widths.AnsiChars(10), iseditingreadonly: true).Get(out this.col_PortDate) // Edit on AddRow by hand
           .Date("WhseArrival", header: "Arrived WH Date", width: Widths.AnsiChars(10), iseditingreadonly: true).Get(out this.col_WHDate) // Edit on AddRow by hand
           .Text("ShipModeID", header: "ShipMode", width: Widths.AnsiChars(10), settings: shipMode_setting, iseditingreadonly: true).Get(out this.col_ShipMode) // Edit on AddRow by hand
           .Text("Vessel", header: "Vessel", width: Widths.AnsiChars(25), iseditingreadonly: true).Get(out this.col_Vessel) // Edit on AddRow by hand
           .Text("ExportPort", header: "Loading (Port)", width: Widths.AnsiChars(15), settings: port_setting, iseditingreadonly: true).Get(out this.col_Port) // Edit on AddRow by hand
           .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(23), settings: refno_setting, iseditingreadonly: true).Get(out this.col_Refno) // Edit on AddRow by hand
           .Text("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
           .Numeric("Qty", header: "Q'ty", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 9, settings: qty_setting, iseditingreadonly: true).Get(out this.col_Qty) // Edit on AddRow by hand
           .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Numeric("NetKg", header: "N.W.", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 9, iseditingreadonly: true).Get(out this.col_NW) // Edit on AddRow by hand
           .Numeric("WeightKg", header: "G.W.", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 9, iseditingreadonly: true).Get(out this.col_GW) // Edit on AddRow by hand
           .Text("CustomsType", header: "Customs Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Text("CDCCode", header: "CDC Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Text("CustomsDescription", header: "Customs Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
           .Numeric("CDCQty", header: "CDCQty", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
           .Text("CDCUnit", header: "CDC Unit", width: Widths.AnsiChars(25), iseditingreadonly: true)
           .Numeric("CDCUnitPrice", header: "CDC Unit Price", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
           .Numeric("CDCAmount", header: "CDC Amount", width: Widths.AnsiChars(11), decimal_places: 4, integer_places: 9, iseditingreadonly: true)
           .Numeric("ActNetKg", header: "Act. N.W.", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, settings: actnum).Get(out this.col_ActNetKg)
           .Numeric("ActWeightKg", header: "Act. G.W.", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, settings: actnum).Get(out this.col_ActWeightKg)
           .Numeric("ActAmount", header: "Act. Amount", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, settings: actnum).Get(out this.col_ActAmount)
           .Numeric("NWDiff", header: "N.W. Diff", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
           .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(14), iseditingreadonly: true)
           .Text("ActHSCode", header: "Act. HS Code", width: Widths.AnsiChars(14), settings: actHSCode).Get(out this.col_ActHSCode)
           ;

            // 變色
            this.detailgrid.Columns["Consignee"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["FactoryID"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ETA"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["PortArrival"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["WhseArrival"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ShipModeID"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Vessel"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ExportPort"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["RefNo"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["NetKg"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["WeightKg"].DefaultCellStyle.BackColor = Color.Pink;

            // 設定是否可以編輯
            this.detailgrid.RowEnter += this.Detailgrid_RowEnter;
        }

        private string GetDetailData()
        {
            string blNo = this.txtBLNo.Text;
            string sqlcmd = $@"
declare @BLNo varchar(30) = '{blNo}'

;with tmpExport as (
	select e.Consignee,e.FactoryID,e.Eta,po3.SCIRefno,ed.UnitId,e.ID
	,Qty = sum(ed.Qty)
	,NW = sum(ed.NetKg)
	,GW = sum(ed.WeightKg)
	from Export e 
	inner join Export_Detail ed on e.id=ed.ID  
	left join PO_Supp_Detail po3 on po3.ID = ed.PoID and ed.Seq1 = po3.SEQ1 and ed.Seq2 = po3.SEQ2
	where 1=1 
	and e.NonDeclare=0
	and e.Blno = @BLNo
	group by e.Consignee,e.FactoryID,e.Eta,po3.SCIRefno,ed.UnitId,e.ID
)
,tmpFtyExport as (
	select fe.Consignee,f2.SCIRefno,fed.UnitId,fe.id
	,Qty = sum(fed.Qty)
	,NW = sum(fed.NetKg)
	,GW = sum(fed.WeightKg)
	from FtyExport fe 
    inner join FtyExport_Detail fed on fed.id=fe.ID 
    inner join Fabric f2 on f2.SCIRefno =fed.SCIRefno 
	where 1=1 
	and fe.NonDeclare=0
	and fe.type in ('1','2','4') 
	and fe.Blno = @BLNo
	group by fe.Consignee,f2.SCIRefno,fed.UnitId,fe.ID
) 
select *,rn = row_number()over(order by ExportID)
from (
select distinct [ExportID] = e.ID
             , [BlNo] = e.Blno
             , [Consignee] = e.Consignee 
             , [FactoryID] = e.FactoryID
             , [ETA] = e.Eta
             , [PortArrival] = e.PortArrival 
             , [WhseArrival] = e.WhseArrival
             , [ShipModeID] = e.ShipModeID
             , [Vessel] = e.Vessel
             , [ExportPort] = e.ExportPort
             , [RefNo] = kc.Refno
             , [Description] = kc.Description 
             , [QTY] = s.Qty
             , [UnitID] = s.UnitId 
             , [NetKg] = s.NW 
             , [WeightKg] = s.GW
             , [CustomsType] = kd.CustomsType  
             , kd.CDCCode
             , [CustomsDescription] = kd.CDCName
             , [CDCQty] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)
             , [CDCUnit] = kd.CDCUnit  
             , [CDCUnitPrice] = kc.CDCUnitPrice  
             , [CDCAmount] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)*kc.CDCUnitPrice  
             , [ActNetKg] = s.NW  
             , [ActWeightKg] = s.GW  
             , [ActAmount] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)*kc.CDCUnitPrice  
             , [NWDiff] = (s.NW-s.nw)  
             , [HSCode] = kcd.HSCode  
             , [ActHSCode] = kcd.HSCode     
             , [KHCustomsItemUkey] = kc.Ukey 
from tmpExport s
inner join Export e on s.ID = e.ID
inner join KHCustomsItem kc on kc.RefNo = s.SCIRefno
left  join KHCustomsItem_Detail kcd on kc.Ukey=kcd.KHCustomsItemUkey and kcd.Port=e.ImportPort          
left  join KHCustomsDescription kd on kd.CDCName = kc.KHCustomsDescriptionCDCName
    and kd.CustomsType in ('Fabric', 'Accessory', 'Machine')
left  join KHCustomsDescription_Detail kdd on kd.CDCName=kdd.CDCName and kdd.PurchaseUnit = s.UnitId   

union all

select [ExportID] = FE.ID  
             , [BlNo] = fe.Blno
             , [Consignee] = FE.Consignee  
             , [FactoryID] = ''  
             , [ETA] = null  
             , [PortArrival] = fe.PortArrival  
             , [WhseArrival] = fe.WhseArrival  
             , [ShipModeID] = fe.ShipModeID   
             , [Vessel] = fe.Vessel  
             , [ExportPort] = fe.ExportPort  
             , [RefNo] = kc.Refno  
             , [Description] = kc.Description  
             , [QTY] = s.Qty  
             , [UnitID] = s.UnitID   
             , [NetKg] = s.NW  
             , [WeightKg] = s.GW  
             , [CustomsType] = kd.CustomsType  
             , kd.CDCCode
             , [CustomsDescription] = kd.CDCName  
             , [CDCQty] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)
             , [CDCUnit] = kd.CDCUnit  
             , [CDCUnitPrice] = kc.CDCUnitPrice  
             , [CDCAmount] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)*kc.CDCUnitPrice  
             , [ActNetKg] = s.NW  
             , [ActWeightKg] = s.GW  
             , [ActAmount] = iif(kd.IsDeclareByNetKg = 1, s.NW, s.Qty*kdd.Ratio)*kc.CDCUnitPrice  
             , [NWDiff] = (s.NW-s.NW)  
             , [HSCode] = kcd.HSCode  
             , [ActHSCode] = kcd.HSCode  
			 , [KHCustomsItemUkey] = kc.Ukey
from tmpFtyExport s
inner join FtyExport fe on s.ID = fe.ID  
inner  join KHCustomsItem kc on kc.RefNo = s.SCIRefno
left  join KHCustomsItem_Detail kcd on kc.Ukey=kcd.KHCustomsItemUkey and kcd.Port=fe.ImportPort        
left  join KHCustomsDescription kd on kd.CDCName = kc.KHCustomsDescriptionCDCName
    and kd.CustomsType in ('Fabric', 'Accessory')
left  join KHCustomsDescription_Detail kdd on kd.CDCName=kdd.CDCName and kdd.PurchaseUnit = s.UnitId 
) a
 where 1=1
   and not EXISTS (select * from KHImportDeclaration where Blno =@BLNo)
";
            return sqlcmd;
        }

        /// <summary>
        /// 將表身資料加總後更新到表頭
        /// </summary>
        private void ReCalculat()
        {
            decimal ttlNW = 0, ttlGW = 0, ttlDecNW = 0, ttlDecGW = 0, ttlAmount = 0, ttlDecAmount = 0, ttlCDCAmount = 0;
            foreach (DataRow dr in this.DetailDatas)
            {
                ttlNW += MyUtility.Convert.GetDecimal(dr["NetKg"]);
                ttlGW += MyUtility.Convert.GetDecimal(dr["WeightKg"]);
                ttlDecNW += MyUtility.Convert.GetDecimal(dr["ActNetKg"]);
                ttlDecGW += MyUtility.Convert.GetDecimal(dr["ActWeightKg"]);
                ttlAmount += MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["Price"]), 4);
                ttlDecAmount += MyUtility.Convert.GetDecimal(dr["ActAmount"]);
                ttlCDCAmount += MyUtility.Convert.GetDecimal(dr["CDCAmount"]);
            }

            this.numTtlNW.Value = ttlNW;
            this.numTtlGW.Value = ttlGW;
            this.numTtlDeclGW.Value = ttlDecGW;
            this.numTtlDeclNW.Value = ttlDecNW;
            this.numTtlAmount.Value = ttlAmount;
            this.numTtlCDCAmount.Value = ttlCDCAmount;
            this.numDeclAmount.Value = ttlDecAmount;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            // 表頭檢查
            if (MyUtility.Check.Empty(this.CurrentMaintain["ImportPort"]) || MyUtility.Check.Empty(this.CurrentMaintain["CDate"]) || MyUtility.Check.Empty(this.CurrentMaintain["DeclareNo"]))
            {
                MyUtility.Msg.WarningBox("<Discharge(Port)>, <Declaration Date> and <Declaration#>cannot be empty.");
                return false;
            }

            // 表身檢查
            if (this.DetailDatas.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Detail can’t be empty.");
                return false;
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["CustomsType"]) ||
                    MyUtility.Check.Empty(dr["CustomsDescription"]) ||
                    MyUtility.Check.Empty(dr["CDCQty"]) ||
                    MyUtility.Check.Empty(dr["CDCUnit"]) ||
                    MyUtility.Check.Empty(dr["CDCUnitPrice"]) ||
                    MyUtility.Check.Empty(dr["ActNetKg"]) ||
                    MyUtility.Check.Empty(dr["ActWeightKg"]) ||
                    MyUtility.Check.Empty(dr["ActAmount"]) ||
                    MyUtility.Check.Empty(dr["ActHSCode"]))
                {
                    MyUtility.Msg.WarningBox(@"These columns <Customs Type>, <Customs Description>, <CDC Qty>, <CDC Unit>, <CDC Unit Price>, <Act N.W.>, <Act. G.W.>, <Act. Amount>, <Act. HS code> cannot be empty.");
                    return false;
                }
            }

            this.ReCalculat();

            // 取單號
            if (this.IsDetailInserting)
            {
                string getID = MyUtility.GetValue.GetID(Env.User.Keyword + "KI", "KHImportDeclaration");
                if (MyUtility.Check.Empty(getID))
                {
                    MyUtility.Msg.ErrorBox("Get ID fail !!");
                    return false;
                }

                this.CurrentMaintain["id"] = getID;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            this.InitReadOnly(true);
            return base.ClickSave();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            DataTable dt = this.GetSumbyCustomsTypeDescription();
            string sqlcmd = $@"
DELETE KHImportDeclaration_ShareCDCExpense where id = '{this.CurrentMaintain["ID"]}'
INSERT INTO [dbo].[KHImportDeclaration_ShareCDCExpense]
           ([ID]
           ,[KHCustomsDescriptionCDCName]
           ,[OriTtlNetKg]
           ,[OriTtlWeightKg]
           ,[OriTtlCDCAmount]
           ,[ActTtlNetKg]
           ,[ActTtlWeightKg]
           ,[ActTtlAmount]
           ,[ActHSCode])
select 
            [ID] = '{this.CurrentMaintain["ID"]}'
           ,[CustomsDescription]
           ,[OriTtlNetKg]
           ,[OriTtlWeightKg]
           ,[OriTtlCDCAmount]
           ,[ActTtlNetKg]
           ,[ActTtlWeightKg]
           ,[ActTtlAmount]
           ,[ActHSCode]
from #tmp
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sqlcmd, out DataTable odt);
            if (!result)
            {
                return result;
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string sqlcmd = $@"
update KHImportDeclaration
set Status='Confirmed'
,EditDate = GetDate()
,EditName = '{Env.User.UserID}'
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail !\r\n" + result.ToString());
                return;
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            string sqlcmd = $@"
update KHImportDeclaration
set Status='New'
,EditDate = GetDate()
,EditName = '{Env.User.UserID}'
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("UnConfirmed fail !\r\n" + result.ToString());
                return;
            }

            MyUtility.Msg.InfoBox("UnConfirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.CurrentMaintain["Status"] = "New";
            this.InitReadOnly(false);
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            this.InitReadOnly(true);
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            if (this.IsDetailInserting)
            {
                this.InitReadOnly(false);
            }
            else
            {
                this.InitReadOnly(true);
            }

            base.ClickUndo();
        }

        /// <summary>
        /// 設定是否可以編輯
        /// </summary>
        /// <param name="readOnly">bool</param>
        private void InitReadOnly(bool readOnly)
        {
            // Status=New, 可以編輯
            bool isNewStatus = false;
            if (this.CurrentMaintain["Status"].EqualString("NEW") && this.EditMode)
            {
                isNewStatus = true;
            }

            this.txtBLNo.ReadOnly = readOnly;
            this.txtImportPort.ReadOnly = readOnly;
            this.dateCDate.ReadOnly = isNewStatus ? false : readOnly;
            this.txtDeclareNo.ReadOnly = isNewStatus ? false : readOnly;
            this.gridicon.Enabled = isNewStatus ? true : false;
            this.detailgrid.IsEditable = isNewStatus ? true : false;
            this.InsertDetailGridOnDoubleClick = isNewStatus ? true : false;
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || this.EditMode == false)
            {
                return;
            }

            var data = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            // Unoriginal= true 非原生資料行, Roll,Dyelot不能編輯
            if (MyUtility.Check.Empty(data["ExportID"]))
            {
                this.col_Fty.IsEditingReadOnly = false;
                this.col_ShipMode.IsEditingReadOnly = false;
                this.col_Vessel.IsEditingReadOnly = false;
                this.col_Port.IsEditingReadOnly = false;
                this.col_Refno.IsEditingReadOnly = false;
                this.col_ETA.IsEditingReadOnly = false;
                this.col_PortDate.IsEditingReadOnly = false;
                this.col_WHDate.IsEditingReadOnly = false;
                this.col_Qty.IsEditingReadOnly = false;
                this.col_NW.IsEditingReadOnly = false;
                this.col_GW.IsEditingReadOnly = false;
                this.col_ActNetKg.IsEditingReadOnly = false;
                this.col_ActWeightKg.IsEditingReadOnly = false;
                this.col_ActAmount.IsEditingReadOnly = false;
                this.col_ActHSCode.IsEditingReadOnly = false;
            }
            else
            {
                this.col_Fty.IsEditingReadOnly = true;
                this.col_ShipMode.IsEditingReadOnly = true;
                this.col_Vessel.IsEditingReadOnly = true;
                this.col_Port.IsEditingReadOnly = true;
                this.col_Refno.IsEditingReadOnly = true;
                this.col_ETA.IsEditingReadOnly = true;
                this.col_PortDate.IsEditingReadOnly = true;
                this.col_WHDate.IsEditingReadOnly = true;
                this.col_Qty.IsEditingReadOnly = true;
                this.col_NW.IsEditingReadOnly = true;
                this.col_GW.IsEditingReadOnly = true;
                this.col_ActNetKg.IsEditingReadOnly = true;
                this.col_ActWeightKg.IsEditingReadOnly = true;
                this.col_ActAmount.IsEditingReadOnly = true;
                this.col_ActHSCode.IsEditingReadOnly = true;
            }
        }

        private void TxtImportPort_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain) && this.EditMode)
            {
                string sqlcmd = @"select ID, Name from Port where CountryID ='KH' and Junk =0";
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "15,20", this.CurrentMaintain["ImportPort"].ToString(), "ID,Name");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["ImportPort"] = item.GetSelecteds()[0]["ID"];
                this.displayPortofDischarge.Text = "KH";
            }
        }

        private void TxtImportPort_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain) && this.EditMode && !MyUtility.Check.Empty(this.txtImportPort.Text))
            {
                DataRow dr;
                string sqlcmd = $@"select ID, Name from Port where CountryID ='KH' and Junk =0 and id ='{this.txtImportPort.Text}'";
                if (!MyUtility.Check.Seek(sqlcmd, out dr))
                {
                    MyUtility.Msg.WarningBox("Cannot find this [Discharge(Port)].");
                    this.txtImportPort.Select();
                    e.Cancel = true;
                    return;
                }
                else
                {
                    this.CurrentMaintain["ImportPort"] = this.txtImportPort.Text;
                    this.displayPortofDischarge.Text = "KH";
                }
            }
        }

        private void TxtBLNo_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain) && this.EditMode)
            {
                if (this.txtBLNo.Text != this.txtBLNo.OldValue && !MyUtility.Check.Empty(this.txtBLNo.OldValue))
                {
                    // Yes 清空 No 保留
                    DialogResult questionResult = MyUtility.Msg.QuestionBox($@" It will clear detail records if change <B/L No.>.");
                    if (questionResult == DialogResult.No)
                    {
                        return;
                    }

                    foreach (var item in this.DetailDatas)
                    {
                        item.Delete();
                    }
                }

                if (MyUtility.Check.Empty(this.txtBLNo.Text))
                {
                    return;
                }

                string sqlcmd = $@"select id from KHImportDeclaration where Blno = '{this.txtBLNo.Text}'";
                if (MyUtility.Check.Seek(sqlcmd, out DataRow dr))
                {
                    MyUtility.Msg.WarningBox($"<B/L No> has already in ID: {dr["id"]}");
                    this.txtBLNo.Select();
                    e.Cancel = true;
                    return;
                }

                DualResult result = DBProxy.Current.Select(null, this.GetDetailData(), out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("No data found!");
                    return;
                }

                dt.AsEnumerable().ToList().ForEach(row => ((DataTable)this.detailgridbs.DataSource).ImportRowAdded(row));
            }
        }

        private void BtnSharebyCDC_Click(object sender, System.EventArgs e)
        {
            ShareDt = null;
            var chkList = this.DetailDatas.Where(w => this.customsTypelist.Contains(MyUtility.Convert.GetString(w["CustomsType"])));
            if (!chkList.Any())
            {
                MyUtility.Msg.WarningBox("There is no <Customs Type>: Fabric/ Accessory/ Machine data.");
                return;
            }

            var emptyList = chkList.Where(w => MyUtility.Check.Empty(w["CDCUnitPrice"]));
            if (emptyList.Any())
            {
                var refnolist = emptyList.Select(s => "<RefNo>: " + MyUtility.Convert.GetString(s["Refno"])).ToList();
                string msg = "Below <RefNo> has no <CDC Unit Price>, please maintain on Shipping B62\r\n" + refnolist.JoinToString("\r\n");
                MyUtility.Msg.WarningBox(msg);
                return;
            }

            var frm = new P61_ShareByCDCItem(this.GetSumbyCustomsTypeDescription(), this.GetRatebyCustomsTypeDescription());
            frm.ShowDialog();
            if (ShareDt != null)
            {
                foreach (DataRow dr in this.DetailDatas)
                {
                    dr["ActNetKg"] = MyUtility.Convert.GetDecimal(ShareDt.Select($"rn = {dr["rn"]}")[0]["ActNetKg"]);
                    dr["ActWeightKg"] = MyUtility.Convert.GetDecimal(ShareDt.Select($"rn = {dr["rn"]}")[0]["ActWeightKg"]);
                    dr["ActAmount"] = MyUtility.Convert.GetDecimal(ShareDt.Select($"rn = {dr["rn"]}")[0]["ActAmount"]);
                    dr["ActHSCode"] = MyUtility.Convert.GetString(ShareDt.Select($"rn = {dr["rn"]}")[0]["ActHSCode"]);
                }
            }
        }

        private DataTable GetSumbyCustomsTypeDescription()
        {
            var xlist = ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                .Where(w => w.RowState != DataRowState.Deleted)
                .Select(s => new
                {
                    CustomsType = MyUtility.Convert.GetString(s["CustomsType"]),
                    CDCCode = MyUtility.Convert.GetString(s["CDCCode"]),
                    CustomsDescription = MyUtility.Convert.GetString(s["CustomsDescription"]),
                    CDCUnit = MyUtility.Convert.GetString(s["CDCUnit"]),
                    NetKg = MyUtility.Convert.GetDecimal(s["NetKg"]),
                    WeightKg = MyUtility.Convert.GetDecimal(s["WeightKg"]),
                    CDCAmount = MyUtility.Convert.GetDecimal(s["CDCAmount"]),
                    ActNetKg = MyUtility.Convert.GetDecimal(s["ActNetKg"]),
                    ActWeightKg = MyUtility.Convert.GetDecimal(s["ActWeightKg"]),
                    ActAmount = MyUtility.Convert.GetDecimal(s["ActAmount"]),
                    ActHSCode = MyUtility.Convert.GetString(s["ActHSCode"]),
                })
                .Where(w => this.customsTypelist.Contains(w.CustomsType))
                .ToList();
            var sumlist = xlist
                .GroupBy(g => new { g.CustomsType, g.CDCCode, g.CustomsDescription, g.CDCUnit, g.ActHSCode })
                .Select(s => new
                {
                    s.Key.CustomsType,
                    s.Key.CDCCode,
                    s.Key.CustomsDescription,
                    s.Key.CDCUnit,
                    s.Key.ActHSCode,
                    OriTtlNetKg = s.Sum(su => su.NetKg),
                    OriTtlWeightKg = s.Sum(su => su.WeightKg),
                    OriTtlCDCAmount = s.Sum(su => su.CDCAmount),
                    ActTtlNetKg = s.Sum(su => su.ActNetKg),
                    ActTtlWeightKg = s.Sum(su => su.ActWeightKg),
                    ActTtlAmount = s.Sum(su => su.ActAmount),
                }).ToList();
            return PublicPrg.ListToDataTable.ToDataTable(sumlist);
        }

        private DataTable GetRatebyCustomsTypeDescription()
        {
            DataTable sumDt = this.GetSumbyCustomsTypeDescription();
            string filter = "CustomsType = '{0}' and CustomsDescription = '{1}'";
            var ratelist = ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                .Where(w => w.RowState != DataRowState.Deleted)
                .Select(s => new
                {
                    rn = MyUtility.Convert.GetLong(s["rn"]),
                    CustomsType = MyUtility.Convert.GetString(s["CustomsType"]),
                    CustomsDescription = MyUtility.Convert.GetString(s["CustomsDescription"]),
                    NetKg = MyUtility.Convert.GetDecimal(s["ActNetKg"]),
                    WeightKg = MyUtility.Convert.GetDecimal(s["ActWeightKg"]),
                    CDCAmount = MyUtility.Convert.GetDecimal(s["CDCAmount"]),
                })
                .Where(w => this.customsTypelist.Contains(w.CustomsType))
                .Select(s => new
                {
                    s.rn,
                    s.CustomsType,
                    s.CustomsDescription,
                    RateNetKg = MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CustomsDescription))[0]["OriTtlNetKg"]) == 0 ? 0 : s.NetKg / MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CustomsDescription))[0]["OriTtlNetKg"]),
                    RateWeightKg = MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CustomsDescription))[0]["OriTtlWeightKg"]) == 0 ? 0 : s.WeightKg / MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CustomsDescription))[0]["OriTtlWeightKg"]),
                    RateCDCAmount = MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CustomsDescription))[0]["OriTtlCDCAmount"]) == 0 ? 0 : s.CDCAmount / MyUtility.Convert.GetDecimal(sumDt.Select(string.Format(filter, s.CustomsType, s.CustomsDescription))[0]["OriTtlCDCAmount"]),
                }).ToList();
            return PublicPrg.ListToDataTable.ToDataTable(ratelist);
        }
    }
}
