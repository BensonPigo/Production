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

namespace Sci.Production.Subcon
{
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        string artworktype, factory, subcon, poid, style, mdivision, bundleno1,bundleno2;
        DateTime? farmoutdate1, farmoutdate2, scidelivery1,scidelivery2;
        DataTable printData;

        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            txtM.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateFarmOutDate.Value1) && MyUtility.Check.Empty(dateFarmOutDate.Value2))
            {
                MyUtility.Msg.WarningBox("Farm Out Date can't empty!!");
                return false;
            }
            farmoutdate1 = dateFarmOutDate.Value1;
            farmoutdate2 = dateFarmOutDate.Value2;
            scidelivery1 = dateSCIDelivery.Value1;
            scidelivery2 = dateSCIDelivery.Value2;

            artworktype = txtartworktype_ftyArtworkType.Text;
            mdivision = txtM.Text;
            factory = comboFactory.Text;
            subcon = txtsubconSupplier.TextBox1.Text;
            poid = txtMasterSPNo.Text;
            style = txtstyle.Text;
            
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- Sql Command --
            StringBuilder sqlCmd = new StringBuilder();  
            sqlCmd.Append(@"
select  AP.MDivisionID
	  , AP.FactoryId
	  , AP.ID
	  , localsupp = AP.LocalSuppID+'-'+(select abb from localsupp WITH (NOLOCK) where id = AP.localsuppid)
	  , AP.ArtworkTypeID
	  , O.POID 
	  , APD.OrderID 
	  , O.StyleID
	  , APD.PatternCode
      , APD.ukey
into #tmpAll
from ArtworkPO AP
left join ArtworkPO_Detail APD WITH (NOLOCK) on APD.ID=AP.ID
left join orders O WITH (NOLOCK) on APD.OrderID = O.ID
where 1=1  --從畫面上帶入條件
");
            #endregion

            System.Data.SqlClient.SqlParameter sp_farmoutdate1 = new System.Data.SqlClient.SqlParameter();
            sp_farmoutdate1.ParameterName = "@farmoutdate1";

            System.Data.SqlClient.SqlParameter sp_farmoutdate2 = new System.Data.SqlClient.SqlParameter();
            sp_farmoutdate2.ParameterName = "@farmoutdate2";

            System.Data.SqlClient.SqlParameter sp_artworktype = new System.Data.SqlClient.SqlParameter();
            sp_artworktype.ParameterName = "@artworktype";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_subcon = new System.Data.SqlClient.SqlParameter();
            sp_subcon.ParameterName = "@subcon";

            System.Data.SqlClient.SqlParameter sp_poid = new System.Data.SqlClient.SqlParameter();
            sp_poid.ParameterName = "@poid";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";

            System.Data.SqlClient.SqlParameter sp_scidelivery1 = new System.Data.SqlClient.SqlParameter();
            sp_scidelivery1.ParameterName = "@scidelivery1";

            System.Data.SqlClient.SqlParameter sp_scidelivery2 = new System.Data.SqlClient.SqlParameter();
            sp_scidelivery2.ParameterName = "@scidelivery2";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            if (!MyUtility.Check.Empty(farmoutdate1))
            {
                sqlCmd.Append(" and AP.issuedate >= @farmoutdate1");
                sp_farmoutdate1.Value = farmoutdate1;
                cmds.Add(sp_farmoutdate1);
            }

            if (!MyUtility.Check.Empty(farmoutdate2))
            {
                sqlCmd.Append(" and AP.issuedate <= @farmoutdate2");
                sp_farmoutdate2.Value = farmoutdate2;
                cmds.Add(sp_farmoutdate2);
            }                     

            if (!MyUtility.Check.Empty(scidelivery1))
            {
                sqlCmd.Append(" and o.scidelivery >= @scidelivery1");
                sp_scidelivery1.Value = scidelivery1;
                cmds.Add(sp_scidelivery1);                
            }

            if (!MyUtility.Check.Empty(scidelivery2))
            {
                sqlCmd.Append(" and o.scidelivery <= @scidelivery2");
                sp_scidelivery2.Value = scidelivery2;
                cmds.Add(sp_scidelivery2);
            }

            if (!MyUtility.Check.Empty(artworktype))
            {
                sqlCmd.Append(" and AP.artworktypeid = @artworktype");
                sp_artworktype.Value = artworktype;
                cmds.Add(sp_artworktype);
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and AP.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and AP.factoryid = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(subcon))
            {
                sqlCmd.Append(" and AP.localsuppid = @subcon");
                sp_subcon.Value = subcon;
                cmds.Add(sp_subcon);
            }

            if (!MyUtility.Check.Empty(poid))
            {
                sqlCmd.Append(" and o.poid = @poid ");
                sp_poid.Value = poid;
                cmds.Add(sp_poid);
            }

            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(" and c.styleid = @style");
                sp_style.Value = style;
                cmds.Add(sp_style);
            }
            sqlCmd.Append(@"

select RowID= CONCAT( ArtworkPo_DetailUkey,issuedate,row_number() over(partition by ArtworkPo_DetailUkey,issuedate order by ArtworkPo_DetailUkey,issuedate))
, QtyIn = qty
, QtyOut = 0
, issuedate = b.issuedate
, ArtworkPo_DetailUkey
into #Targer
from farmIn_detail a
inner join farmIn b on a.id=b.id
where b.Status = 'Confirmed'
and ArtworkPo_DetailUkey in (select distinct ukey from #tmpAll)
order by b.issuedate

select RowID= CONCAT( ArtworkPo_DetailUkey,issuedate,row_number() over(partition by ArtworkPo_DetailUkey,issuedate order by ArtworkPo_DetailUkey,issuedate))
, QtyIn = 0
, QtyOut = qty
, issuedate = b.issuedate
, ArtworkPo_DetailUkey
into #Source
from farmOut_detail a
inner join farmOut b on a.id=b.id
where b.Status = 'Confirmed'
and ArtworkPo_DetailUkey in (select distinct ukey from #tmpAll)
order by b.issuedate


merge #Targer as t
using #Source as s
	on  t.RowID=s.RowID 	
when matched then 
	update set
	t.QtyOut=s.QtyOut	
when not matched by target then 
	insert (  Rowid,  QtyIn,  QtyOut,  issuedate,  ArtworkPo_DetailUkey)
	values (s.Rowid,s.QtyIn,s.QtyOut,s.issuedate,s.ArtworkPo_DetailUkey);


SELECT  MDivisionID
	, FactoryId
	, ID
	, localsupp
	, ArtworkTypeID
	, POID 
	, OrderID 
	, StyleID
	, PatternCode
	, [Date] = Farm.issuedate
	, [Farm Out Qty] = Farm.QtyOut
	, [Farm In Qty] = Farm.QtyIn	
from #tmpAll
OUTER APPLY(
	select * from #Targer where ArtworkPo_DetailUkey=ukey
) Farm
Order by MDivisionID,id, POID, Orderid,ukey,date

drop table #Targer,#Source,#tmpAll
 ");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(),cmds, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R06.xltx",2);
            return true;
        }
    }
}
