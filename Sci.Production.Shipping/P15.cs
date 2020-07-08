using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class P15 : Win.Tems.Input6
    {
        public P15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "ID = MainExportID  AND AddDate > DATEADD(Year,-2,GETDATE())";
            this.detailgrid.AllowUserToOrderColumns = true;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            DataTable dtBl;
            DataTable dtHeader;

            #region SQL 表頭資料
            string cmd = $@"


-- 表頭資料
select Blno
		, LoadDate
		, CloseDate
		, ETD
		, ETA
		, PortArrival
		, WhseArrival
		, ExportPort = Concat (ExportPort, '-', ExportCountry)--, display_From + Country
		, ImportPort = Concat (ImportPort, '-', ImportCountry)--, display_To + Country
		, Consignee
		, ShipModeID
		, ID
		, Handle--, display_Handle get TPE 
		, Payer = (	select name
					from dbo.DropDownList
					where id = Payer
						  and Type = 'WK_Payer')
		, ETA_ETC = DATEDIFF(Day, closeDate, ETA)--, display_ETA_ETC
		, ETA_ETD = DATEDIFF(Day, EtD, ETA)--, display_ETA_ETD
		, Port_ETA = iif (isnull (PortArrival, '') = '', 0, DateDiff (Day, ETA, PortArrival))--, display_Port_ETA		
		, Whse_ETA = iif (isnull (WhseArrival, '') = '', 0, DateDiff (Day, ETA, WhseArrival))--, display_Whse_ETA
		, FormStatus = (select name
						from dbo.DropDownList
						where id = FormStatus
							  and Type = 'WK_FormStatus')--, display_FormMode
from Export e
outer apply (
	select	TtlWeightKg = sum (TtlE.WeightKg)
			, TtlPackages = sum (TtlE.Packages)
			, FOBCBM = sum(Isnull(getCBMFOB.Cbm,0))
			, FORCBM = sum(Isnull(getCBMFOR.Cbm,0))
			, FOR_CYCBM = sum(Isnull(getCBMFOR_CY.CbmFor, 0))
	from Export TtlE
	outer apply (
		select export.Cbm 
		from export 
		where export.id = e.id 
			  and (export.ShipmentTerm In ('FOB', 'FCA', 'EXW'))
	) as getCBMFOB
	outer apply (
		select export.Cbm 
		from export 
		where export.id = e.id 
			  and (export.ShipmentTerm = 'FOR')
	) as getCBMFOR
	outer apply (
		select export.CbmFor 
		from export 
		where export.id = e.id 
			  and (export.ShipmentTerm = 'FOR')
	) as getCBMFOR_CY
	where TtlE.MainExportID = e.ID
		  and Junk = 0
) Combin
where id = '{this.CurrentMaintain["ID"]}'
";
            #endregion
            DualResult result = DBProxy.Current.Select(null, cmd, out dtHeader);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            #region SQL加總同 BLNO 的相關資訊
            cmd = $@"

-- 加總同 BLNO 的相關資訊

select	TtlPackages = sum (TtlE.Packages)
		, TtlWeightKg = sum (TtlE.WeightKg)
		, Ttl20 = Concat ('20 x ', isnull (sum (Container.Count20), 0))
		, Ttl40 = Concat ('40 x ', isnull (sum (Container.Count40), 0))
		, TtlHQ = Concat ('HQ x ', isnull (sum (Container.CountHQ), 0))
		, FOBCBM = sum(Isnull(getCBMFOB.Cbm,0))
		, FORCBM = sum(Isnull(getCBMFOR.Cbm,0))
		, FOR_CYCBM = sum(Isnull(getCBMFOR_CY.CbmFor, 0))
from Export TtlE
outer apply (
	select	Count20 = sum (case when ex.Type = '20' then 1 else 0 end)
			, Count40 = sum (case when ex.Type = '40' then 1 else 0 end)
			, CountHQ = sum (case when ex.Type = 'HQ' then 1 else 0 end)
	from dbo.Export_Container ex
	where TtlE.ID = ex.ID
		  and TtlE.CYCFS not like '%CFS%'
) Container
outer apply (
	select export.Cbm 
	from export 
	where export.id = TtlE.id 
			and (export.ShipmentTerm In ('FOB', 'FCA', 'EXW'))
) as getCBMFOB
outer apply (
	select export.Cbm 
	from export 
	where export.id = TtlE.id 
			and (export.ShipmentTerm = 'FOR')
) as getCBMFOR
outer apply (
	select export.CbmFor 
	from export 
	where export.id = TtlE.id 
			and (export.ShipmentTerm = 'FOR')
) as getCBMFOR_CY
where TtlE.MainExportID = '{this.CurrentMaintain["ID"]}'
and Junk = 0
";
            #endregion
            result = DBProxy.Current.Select(null, cmd, out dtBl);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            // 填入表頭資料
            if (dtBl != null && dtBl.Rows.Count > 0)
            {
                this.disLoadDate.Text = MyUtility.Check.Empty(dtHeader.Rows[0]["LoadDate"]) ? string.Empty : Convert.ToDateTime(dtHeader.Rows[0]["LoadDate"]).ToShortDateString();
                this.disETC.Text = MyUtility.Check.Empty(dtHeader.Rows[0]["CloseDate"]) ? string.Empty : Convert.ToDateTime(dtHeader.Rows[0]["CloseDate"]).ToShortDateString();
                this.disETD.Text = MyUtility.Check.Empty(dtHeader.Rows[0]["ETD"]) ? string.Empty : Convert.ToDateTime(dtHeader.Rows[0]["ETD"]).ToShortDateString();
                this.disETA.Text = MyUtility.Check.Empty(dtHeader.Rows[0]["ETA"]) ? string.Empty : Convert.ToDateTime(dtHeader.Rows[0]["ETA"]).ToShortDateString();
                this.disApd.Text = MyUtility.Check.Empty(dtHeader.Rows[0]["PortArrival"]) ? string.Empty : Convert.ToDateTime(dtHeader.Rows[0]["PortArrival"]).ToShortDateString();
                this.disAwd.Text = MyUtility.Check.Empty(dtHeader.Rows[0]["WhseArrival"]) ? string.Empty : Convert.ToDateTime(dtHeader.Rows[0]["WhseArrival"]).ToShortDateString();

                this.txtHandle.DisplayBox1Binding = dtHeader.Rows[0]["Handle"].ToString();
                this.disPayer.Text = dtHeader.Rows[0]["Payer"].ToString();
                this.disEtaEtc.Text = dtHeader.Rows[0]["ETA_ETC"].ToString();
                this.disEtaEtd.Text = dtHeader.Rows[0]["ETA_ETD"].ToString();
                this.disPaEta.Text = dtHeader.Rows[0]["Port_ETA"].ToString();
                this.disWaEta.Text = dtHeader.Rows[0]["Whse_ETA"].ToString();
                this.disShipMode.Text = dtHeader.Rows[0]["FormStatus"].ToString();
            }

            // BLNO 的相關資訊
            if (dtBl != null && dtBl.Rows.Count > 0)
            {
                this.numTtlpackage.Value = MyUtility.Convert.GetInt(dtBl.Rows[0]["TtlPackages"]);
                this.numTtlWeight.Value = MyUtility.Convert.GetDecimal(dtBl.Rows[0]["TtlWeightKg"]);
                this.disTtl20.Text = dtBl.Rows[0]["Ttl20"].ToString();
                this.disTtl40.Text = dtBl.Rows[0]["Ttl40"].ToString();
                this.disTtlHQ.Text = dtBl.Rows[0]["TtlHQ"].ToString();
                this.numFobCBM.Value = MyUtility.Convert.GetDecimal(dtBl.Rows[0]["FOBCBM"]);
                this.numForCBM.Value = MyUtility.Convert.GetDecimal(dtBl.Rows[0]["FORCBM"]);
                this.numForCBMCY.Value = MyUtility.Convert.GetDecimal(dtBl.Rows[0]["FOR_CYCBM"]);
            }

            this.detailgrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string ExportID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);

            this.DetailSelectCommand = $@"
select	e.ID
		, e.FactoryID
		, e.CYCFS
		, CountAll = isnull (ContainerInfo.CountAll, 0)
		, Count20 = isnull (ContainerInfo.Count20, 0)
		, Count40 = isnull (ContainerInfo.Count40, 0)
		, CountHQ = isnull (ContainerInfo.CountHQ, 0)
		, e.ShipmentTerm
		, e.CBM
		, e.Packages
		, e.WeightKg
		, e.NoImportCharges
		, ContainerNo = ContainerNo.v
from Export e
outer apply (
	select	Count20 = sum (case when ex.Type = '20' then 1 else 0 end)
			, Count40 = sum (case when ex.Type = '40' then 1 else 0 end)
			, CountHQ = sum (case when ex.Type = 'HQ' then 1 else 0 end)
			, CountAll = count(1)
	from dbo.Export_Container ex
	where e.ID = ex.ID
		  and e.CYCFS = 'CY'
) ContainerInfo
outer apply (
	select v = stuff ((
					select concat(';', ec.Type, '-', ec.Container)
					from dbo.Export_Container as ec
					where ec.ID = e.ID
					ORDER BY ec.Seq
					for xml path('')					
				), 1, 1, '')
) ContainerNo
where e.MainExportID = '{ExportID}'
order by e.ID
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings colContainers = new DataGridViewGeneratorTextColumnSettings();
            colContainers.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (e.RowIndex != -1 && this.CurrentDetailData["ContainerNo"].Empty() == false)
                    {
                        string exportID = this.CurrentDetailData["ID"].ToString();
                        P15_Containers form = new P15_Containers(exportID);
                        form.ShowDialog();
                    }
                }
            };

            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ID", header: "WK#", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("FactoryID", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("CYCFS", header: "CYCFS", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("CountAll", header: "Ttl Qty", iseditingreadonly: true, decimal_places: 0, width: Widths.AnsiChars(5))
                .Numeric("Count20", header: "20", iseditingreadonly: true, decimal_places: 0, width: Widths.AnsiChars(4))
                .Numeric("Count40", header: "40", iseditingreadonly: true, decimal_places: 0, width: Widths.AnsiChars(4))
                .Numeric("CountHQ", header: "HQ", iseditingreadonly: true, decimal_places: 0, width: Widths.AnsiChars(4))
                .Text("ShipmentTerm", header: "Term", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Numeric("CBM", header: "CBM", iseditingreadonly: true, decimal_places: 3, width: Widths.AnsiChars(4))
                .Numeric("Packages", header: "Packages", iseditingreadonly: true, decimal_places: 0, width: Widths.AnsiChars(4))
                .Numeric("WeightKg", header: "Weight", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(5))
                .CheckBox("NoImportCharges", header: "No Import Charge", iseditable: false, width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0)
                .Text("ContainerNo", header: "Containers", iseditingreadonly: true, width: Widths.AnsiChars(15), settings: colContainers)
                ;
        }
    }
}
