using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class R40 : Sci.Win.Tems.PrintForm
    {
        string contract, hscode, nlcode, sp;
        bool liguidationonly;
        DataTable Summary, WHDetail, WIPDetail, ProdDetail, ScrapDetail;
        public R40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(textBox1.Text))
            {
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                textBox1.Focus();
                return false;
            }

            if (!checkBox1.Checked)
            {
                if (MyUtility.Check.Empty(textBox4.Text))
                {
                    MyUtility.Msg.WarningBox("SP# start from can't empty!!");
                    textBox4.Focus();
                    return false;
                }
            }
            contract = textBox1.Text;
            hscode = textBox2.Text;
            nlcode = textBox3.Text;
            sp = textBox4.Text;
            liguidationonly = checkBox1.Checked;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            sqlCmd.Append(string.Format(@"DECLARE @contract VARCHAR(15),
		@mdivision VARCHAR(8)
SET @contract = '{0}';
SET @mdivision = '{1}';

--撈合約資料
select HSCode,NLCode,Qty,UnitID into #tmpContract
from VNContract_Detail 
where ID = @contract

--撈進/出口與調整資料
select a.NLCode,sum(a.Qty) as Qty into #tmpDeclare from (
select vid.NLCode,vid.Qty
from VNImportDeclaration vi
inner join VNImportDeclaration_Detail vid on vid.ID = vi.ID
where vi.VNContractID = @contract
union all
select vcd.NLCode,vcd.Qty
from VNExportDeclaration ve
inner join VNExportDeclaration_Detail ved on ved.ID = ve.ID
inner join VNConsumption vc on vc.VNContractID = ve.VNContractID and ved.CustomSP = vc.CustomSP
inner join VNConsumption_Detail vcd on vcd.ID = vc.ID
where ve.VNContractID = @contract
union all
select vcd.NLCode,vcd.Qty
from VNContractQtyAdjust vc
inner join VNContractQtyAdjust_Detail vcd on vc.ID = vcd.ID
where vc.VNContractID = @contract
) a
group by a.NLCode;", contract, Sci.Env.User.Keyword));

            if (liguidationonly)
            {
                sqlCmd.Append(@"
select isnull(tc.HSCode,'') as HSCode,a.NLCode,isnull(vcd.DescEN,'') as Description,isnull(tc.UnitID,'') as UnitID,isnull(tc.Qty,0)-isnull(td.Qty,0) as LiqQty
from (select NLCode from #tmpContract union select NLCode from #tmpDeclare) a
left join #tmpContract tc on a.NLCode = tc.NLCode
left join VNNLCodeDesc vcd on a.NLCode = vcd.NLCode
left join #tmpDeclare td on a.NLCode = td.NLCode
where 1 = 1");

                if (!MyUtility.Check.Empty(hscode))
                {
                    sqlCmd.Append(string.Format(" and tc.HSCode = '{0}'", hscode));
                }
                if (!MyUtility.Check.Empty(nlcode))
                {
                    sqlCmd.Append(string.Format(" and a.NLCode = '{0}'", nlcode));
                }
                sqlCmd.Append(@"                                                                                                       
order by CONVERT(int,SUBSTRING(a.NLCode,3,3))");
            }
            else
            {
                sqlCmd.Append(@"
--撈W/House資料
select distinct o.POID, o.MDivisionID into #tmpPOID
from Orders o
where (o.Category = 'B' or o.Category = 'S' or o.Category = 'M')
and o.LocalOrder = 0 and o.MDivisionID = @mdivision

select isnull(f.HSCode,'') as HSCode,isnull(f.NLCode,'') as NLCode,t.POID,(fi.Seq1+'-'+fi.Seq2) as Seq, 
fi.Roll,fi.Dyelot,fi.StockType,
isnull((select CONCAT(fid.MtlLocationID,',') from FtyInventory_Detail fid where fid.Ukey = fi.UKey for xml path('')),'') as Location,
IIF(fi.InQty-fi.OutQty+fi.AdjustQty <> 0,dbo.getVNUnitTransfer(f.Type,psd.StockUnit,f.CustomsUnit,(fi.InQty-fi.OutQty+fi.AdjustQty),f.Width,f.PcsWidth,f.PcsLength,f.PcsKg,isnull(IIF(f.CustomsUnit = 'M2',(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M'),(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = f.CustomsUnit)),1),isnull(IIF(f.CustomsUnit = 'M2',(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M'),(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = f.CustomsUnit)),'')),0) as Qty
into #tmpWHQty
from #tmpPOID t
inner join FtyInventory fi on fi.POID = t.POID and fi.MDivisionID = t.MDivisionID
inner join PO_Supp_Detail psd on t.POID = psd.ID and psd.SEQ1 = fi.Seq1 and psd.SEQ2 = fi.Seq2
left join Fabric f on psd.SCIRefno = f.SCIRefno
where (fi.StockType = 'B' or fi.StockType = 'I');
--等Local物料的Inventory結構開好再補


--撈Scrap資料
select isnull(f.HSCode,'') as HSCode,isnull(f.NLCode,'') as NLCode,t.POID,(mdp.Seq1+'-'+mdp.Seq2) as Seq,psd.Refno,
isnull(f.Type,'') as Type,isnull(f.Description,'') as Description,psd.StockUnit,mdp.LObQty,isnull(f.CustomsUnit,'') as CustomsUnit, 
IIF(mdp.LObQty <> 0,dbo.getVNUnitTransfer(f.Type,psd.StockUnit,f.CustomsUnit,mdp.LObQty,f.Width,f.PcsWidth,f.PcsLength,f.PcsKg,isnull(IIF(f.CustomsUnit = 'M2',(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M'),(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = f.CustomsUnit)),1),isnull(IIF(f.CustomsUnit = 'M2',(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M'),(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = f.CustomsUnit)),'')),0) as Qty
into #tmpScrapQty
from #tmpPOID t
inner join MDivisionPoDetail mdp on mdp.POID = t.POID and mdp.MDivisionID = t.MDivisionID
inner join PO_Supp_Detail psd on t.POID = psd.ID and psd.SEQ1 = mdp.Seq1 and psd.SEQ2 = mdp.Seq2
left join Fabric f on psd.SCIRefno = f.SCIRefno");
                if (!MyUtility.Check.Empty(sp))
                {
                    sqlCmd.Append(string.Format(@"
                where t.POID >= '{0}'", sp));
                }

                sqlCmd.Append(@"
--等Local物料的Inventory結構開好再補


--撈已發料數量
select o.ID,o.MDivisionID,o.StyleID,o.BrandID,o.StyleUKey,o.Category,o.POID into #tmpWHNotClose 
from Orders o 
where o.WhseClose is null and (o.Category = 'B' or o.Category = 'S') and o.MDivisionID = @mdivision and o.LocalOrder = 0

--台北買的物料
select isnull(f.HSCode,'') as HSCode,isnull(f.NLCode,'') as NLCode,t.ID,(mdp.Seq1+'-'+mdp.Seq2) as Seq,
psd.Refno,f.Type,f.Description,psd.StockUnit,mdp.OutQty-mdp.LObQty as IssueQty,f.CustomsUnit, 
IIF((mdp.OutQty-mdp.LObQty) <> 0,dbo.getVNUnitTransfer(f.Type,psd.StockUnit,f.CustomsUnit,(mdp.OutQty-mdp.LObQty),f.Width,f.PcsWidth,f.PcsLength,f.PcsKg,isnull(IIF(f.CustomsUnit = 'M2',(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M'),(select RateValue from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = f.CustomsUnit)),1),isnull(IIF(f.CustomsUnit = 'M2',(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = 'M'),(select Rate from dbo.View_Unitrate where FROM_U = psd.StockUnit and TO_U = f.CustomsUnit)),'')),0) as Qty
into #tmpIssueQty
from #tmpWHNotClose t
inner join MDivisionPoDetail mdp on mdp.POID = t.ID and mdp.MDivisionID = t.MDivisionID
inner join PO_Supp_Detail psd on mdp.POID = psd.ID and psd.SEQ1 = mdp.Seq1 and psd.SEQ2 = mdp.Seq2
left join Fabric f on psd.SCIRefno = f.SCIRefno

--Local Purchase的物料

--撈各Style目前最後的CustomSP
select v.ID,v.CustomSP,v.StyleID,v.BrandID,v.Category into #tmpCustomSP
from VNConsumption v
inner join (select vc.StyleID,vc.BrandID,vc.Category,MAX(vc.CustomSP) as CustomSP
			from VNConsumption vc where vc.VNContractID = @contract
			group by vc.StyleID,vc.BrandID,vc.Category) vc on vc.CustomSP = v.CustomSP
where v.VNContractID = @contract


--撈已Sewing數量
select t.ID, sdd.ComboType,sdd.Article,sdd.SizeCode,sum(sdd.QAQty) as QAQty,
isnull(a.HSCode,'') as HSCode,isnull(a.NLCode,'') as NLCode,
isnull(a.UnitID,'') as UnitID,sum(isnull(a.Qty ,0)) as Qty,isnull(a.CustomSP,'') as CustomSP,t.POID
into #tmpWHNotCloseSewingOutput
from #tmpWHNotClose t 
inner join SewingOutput_Detail_Detail sdd on sdd.OrderId = t.ID
left join (select sdd.OrderId,sdd.ComboType,sdd.Article,sdd.SizeCode,sdd.QAQty,vd.HSCode,vd.NLCode,vd.UnitID,(sl.Rate/100*sdd.QAQty)*vd.Qty as Qty,tc.CustomSP
		   from #tmpWHNotClose t
		   inner join SewingOutput_Detail_Detail sdd on sdd.OrderId = t.ID
		   inner join Style_Location sl on sl.StyleUkey = t.StyleUKey and sl.Location = sdd.ComboType
		   inner join #tmpCustomSP tc on tc.StyleID = t.StyleID and tc.BrandID = t.BrandID and tc.Category = t.Category
		   inner join VNConsumption_Article va on va.ID = tc.ID and va.Article = sdd.Article
		   inner join VNConsumption_SizeCode vs on vs.ID = tc.ID and vs.SizeCode = sdd.SizeCode
		   inner join VNConsumption_Detail vd on vd.ID = tc.ID) a on t.ID = a.OrderId and sdd.ComboType = a.ComboType and sdd.Article = a.Article and sdd.SizeCode = a.SizeCode
group by t.ID, sdd.ComboType,sdd.Article,sdd.SizeCode,isnull(a.HSCode,''),isnull(a.NLCode,''),
isnull(a.UnitID,''),isnull(a.CustomSP,''),t.POID
order by t.ID, sdd.ComboType,sdd.Article,sdd.SizeCode

--組WIP明細
select IIF(ti.ID is null,tw.POID,ti.ID) as ID,IIF(ti.HSCode is null,tw.HSCode,ti.HSCode) as HSCode,IIF(ti.NLCode is null,tw.NLCode,ti.NLCode) as NLCode,(isnull(ti.Qty,0)-isnull(tw.Qty,0)) as Qty
into #tmpWIPDetail
from (select ID,HSCode,NLCode,SUM(Qty) as Qty from #tmpIssueQty group by ID,HSCode,NLCode) ti
full outer join (select POID,HSCode,NLCode,SUM(Qty) as Qty from #tmpWHNotCloseSewingOutput group by POID,HSCode,NLCode) tw on tw.POID = ti.ID and tw.NLCode = ti.NLCode
order by IIF(ti.ID is null,tw.POID,ti.ID)

--撈尚未Pullout Complete的資料
select ID,StyleID,BrandID,StyleUkey,Category
into #tmpNoPullOutComp
from Orders 
where PulloutComplete = 0
and (Category = 'B' or Category = 'S') and MDivisionID = @mdivision and LocalOrder = 0

select a.*,a.SewQty - a.PullQty as Qty into #tmpPreProdQty
from (select t.ID,t.StyleID,t.BrandID,t.StyleUkey,t.Category,oq.Article,oq.SizeCode,
	  isnull(dbo.getMinCompleteSewQty(t.ID,oq.Article,oq.SizeCode),0) as SewQty,
	  isnull((select Sum(pdd.ShipQty) from Pullout_Detail_Detail pdd where pdd.OrderID = t.ID  and pdd.Article = oq.Article and pdd.SizeCode = oq.SizeCode),0) as PullQty
	  from #tmpNoPullOutComp t
	  inner join Order_Qty oq on t.ID = oq.ID) a

select t.ID,t.Article,t.SizeCode,t.SewQty,t.PullQty,
isnull(a.HSCode,'') as HSCode,isnull(a.NLCode,'') as NLCode,
isnull(a.UnitID,'') as UnitID,isnull(a.Qty ,0) as Qty,isnull(a.CustomSP,'') as CustomSP
into #tmpProdQty
from #tmpPreProdQty t 
left join (select t.ID,t.Article,t.SizeCode,t.SewQty,t.PullQty,vd.HSCode,vd.NLCode,vd.UnitID,t.qty*vd.Qty as Qty,tc.CustomSP
		   from #tmpPreProdQty t
		   inner join #tmpCustomSP tc on tc.StyleID = t.StyleID and tc.BrandID = t.BrandID and tc.Category = t.Category
		   inner join VNConsumption_Article va on va.ID = tc.ID and va.Article = t.Article
		   inner join VNConsumption_SizeCode vs on vs.ID = tc.ID and vs.SizeCode = t.SizeCode
		   inner join VNConsumption_Detail vd on vd.ID = tc.ID) a on t.ID = a.ID and t.Article = a.Article and t.SizeCode = a.SizeCode
order by t.ID,t.Article,t.SizeCode,t.SewQty,t.PullQty");

                sqlCmd.Append(@"
--整理出Summary
select isnull(tc.HSCode,'') as HSCode,a.NLCode,isnull(vcd.DescEN,'') as Description,isnull(tc.UnitID,'') as UnitID,isnull(tc.Qty,0)-isnull(td.Qty,0) as LiqQty,
isnull(tw.Qty,0) as WHQty, isnull(ti.Qty,0) as WIPQty,isnull(tp.Qty,0) as ProdQty, isnull(ts.Qty,0) as ScrapQty
from (select NLCode from #tmpContract union select NLCode from #tmpDeclare union select NLCode from (select Distinct NLCode from #tmpWHQty) t union select NLCode from (select Distinct NLCode from #tmpWIPDetail) t union select NLCode from (select Distinct NLCode from #tmpProdQty) t union select NLCode from (select Distinct NLCode from #tmpScrapQty) t) a
left join #tmpContract tc on a.NLCode = tc.NLCode
left join VNNLCodeDesc vcd on a.NLCode = vcd.NLCode
left join #tmpDeclare td on a.NLCode = td.NLCode
left join (select NLCode,SUM(Qty) as Qty from #tmpWHQty group by NLCode) tw on a.NLCode = tw.NLCode
left join (select NLCode,SUM(Qty) as Qty from #tmpWIPDetail group by NLCode) ti on a.NLCode = ti.NLCode
left join (select NLCode,sum(Qty) as Qty from #tmpProdQty group by NLCode) tp on a.NLCode = tp.NLCode
left join (select NLCode,sum(Qty) as Qty from #tmpScrapQty group by NLCode) ts on a.NLCode = ts.NLCode
where 1 = 1");

                if (!MyUtility.Check.Empty(hscode))
                {
                    sqlCmd.Append(string.Format(" and tc.HSCode = '{0}'", hscode));
                }
                if (!MyUtility.Check.Empty(nlcode))
                {
                    sqlCmd.Append(string.Format(" and a.NLCode = '{0}'", nlcode));
                }

                sqlCmd.Append(string.Format(@"                                                                                                       
order by CONVERT(int,SUBSTRING(a.NLCode,3,3))

--W/H明細
select * from #tmpWHQty where Qty > 0 {0} {1} order by POID,Seq

--WIP明細
select * from #tmpWIPDetail where Qty > 0 {0} {1} order by ID

--Prod明細
select * from #tmpProdQty where Qty > 0 {0} {1} order by ID,Article,SizeCode

--Scrap明細
select * from #tmpScrapQty where Qty > 0 {0} {1} order by POID,Seq", MyUtility.Check.Empty(hscode) ? "" : string.Format("and HSCode = '{0}'", hscode)
                                                                       , MyUtility.Check.Empty(nlcode) ? "" : string.Format("and NLCode = '{0}'", nlcode)));
            }
            #endregion

            DataSet AllData;
            
            if (!SQL.Selects("", sqlCmd.ToString(), out AllData))
            {
                DualResult failResult = new DualResult(false, "Query data fail");
                return failResult;
            }
            Summary = AllData.Tables[0];

            if (!liguidationonly)
            {
                WHDetail = AllData.Tables[1];
                WIPDetail = AllData.Tables[2];
                ProdDetail = AllData.Tables[3];
                ScrapDetail = AllData.Tables[4];
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (Summary.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            SetCount(Summary.Rows.Count + (liguidationonly ? 0 : WHDetail.Rows.Count + WIPDetail.Rows.Count + ProdDetail.Rows.Count + ScrapDetail.Rows.Count));

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
            bool result;
            MyUtility.Msg.WaitWindows("Starting EXCEL...Summary");

            if (!liguidationonly)
            {
                result = MyUtility.Excel.CopyToXls(Summary, "", xltfile: "Shipping_R40_Summary.xltx", headerRow: 1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }

                if (WHDetail.Rows.Count > 0)
                {
                    MyUtility.Msg.WaitWindows("Starting EXCEL...WHouse Qty Detail");
                    result = MyUtility.Excel.CopyToXls(WHDetail, "", xltfile: "Shipping_R40_WHQtyDetail.xltx", headerRow: 1);
                    if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
                }

                if (WIPDetail.Rows.Count > 0)
                {
                    MyUtility.Msg.WaitWindows("Starting EXCEL...WIP Qty Detail");
                    result = MyUtility.Excel.CopyToXls(WIPDetail, "", xltfile: "Shipping_R40_WIPQtyDetail.xltx", headerRow: 1);
                    if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
                }

                if (ProdDetail.Rows.Count > 0)
                {
                    MyUtility.Msg.WaitWindows("Starting EXCEL...Prod. Qty Detail");
                    result = MyUtility.Excel.CopyToXls(ProdDetail, "", xltfile: "Shipping_R40_ProdQtyDetail.xltx", headerRow: 1);
                    if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
                }

                if (ScrapDetail.Rows.Count > 0)
                {
                    MyUtility.Msg.WaitWindows("Starting EXCEL...Scrap Qty Detail");
                    result = MyUtility.Excel.CopyToXls(ScrapDetail, "", xltfile: "Shipping_R40_ScrapQtyDetail.xltx", headerRow: 1);
                    if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
                }
            }
            else
            {
                result = MyUtility.Excel.CopyToXls(Summary, "", xltfile: "Shipping_R40_Summary(Only Liquidation).xltx", headerRow: 1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }

            MyUtility.Msg.WaitClear();
            return true;
        }

    }
}
