using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace RFID_Monitor_Board
{
    public partial class RFID_Monitor_Board : Sci.Win.Tems.QueryForm
    {
        public RFID_Monitor_Board()
        {
            InitializeComponent();
            //this.EditMode = true;
            DataRow dr;
            MyUtility.Check.Seek("select * from MonitorBoardParameter", out dr);
            timer1.Interval = MyUtility.Convert.GetInt(dr["InteralAll"]) * 1000;//幾毫秒 重讀全部
            timer2.Interval = MyUtility.Convert.GetInt(dr["IntervalPerPage"]) * 1000;//幾毫秒 切下頁面
            setGridStyle();
            Query();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Query();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            changepages();
        }

        private void RFID_Monitor_Board_Load(object sender, EventArgs e)
        {
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("sp", header: "SP", iseditingreadonly: true, width: Widths.AnsiChars(16))
                .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("Qty", header: "Order Qty", iseditable: true, decimal_places: 0, integer_places: 10, width: Widths.AnsiChars(6))
                .Numeric("cpqs", header: "Cut part Qty / Set", iseditable: true, decimal_places: 0, integer_places: 10, width: Widths.AnsiChars(6))
                .Numeric("QtySM", header: "IN-Set Qty", iseditable: true, decimal_places: 0, integer_places: 10, width: Widths.AnsiChars(6))
                .Numeric("icpq", header: "IN-Cut parts Qty", iseditable: true, decimal_places: 0, integer_places: 10, width: Widths.AnsiChars(6))
                .Numeric("osq", header: "Out-Set Qty", iseditable: true, decimal_places: 0, integer_places: 10, width: Widths.AnsiChars(6))
               ;
            this.grid1.Columns["QtySM"].DefaultCellStyle.BackColor = Color.Orange;
            this.grid1.Columns["icpq"].DefaultCellStyle.BackColor = Color.Orange;
            this.grid1.Columns["osq"].DefaultCellStyle.BackColor = Color.Red;
        }

        DataTable DT;//grid datas
        private void Query()
        {
            DataRow dr;
            MyUtility.Check.Seek("select * from MonitorBoardParameter", out dr);
            DateTime? m1 = MyUtility.Convert.GetDate(dr["MorningShiftStart"]);
            DateTime? m2 = MyUtility.Convert.GetDate(dr["MorningShiftEnd"]);
            DateTime? n1 = MyUtility.Convert.GetDate(dr["NightShiftStart"]);
            DateTime? n2 = n2 = MyUtility.Convert.GetDate(dr["NightShiftEnd"]) < m1 ? MyUtility.Convert.GetDate(dr["NightShiftEnd"]).Value.AddDays(1) : MyUtility.Convert.GetDate(dr["NightShiftEnd"]);
            DateTime? n1b = n1.Value.AddDays(-MyUtility.Convert.GetInt(dr["WIPRange"]));

            string factory = MyUtility.Convert.GetString(dr["factoryID"]);
            string subprocess = MyUtility.Convert.GetString(dr["subprocessID"]);
            if (m1 == null || m2 == null || n1 == null || n2 == null)
            {
                MyUtility.Msg.ErrorBox("Monitor Board Parameter is not complete, please check it !!");
                return;
            }
            #region txtFactory & txtSunProcess
            txtFactory.Text = factory;
            txtSunProcess.Text = subprocess;
            #endregion
            #region Date range:系統當日時間。 依系統時間顯示，需在早晚班的時間範圍內。若低於早班起始，則為前一天。
            if (DateTime.Now <= m1)
            {
                m1 = m1.Value.AddDays(-1);
                m2 = m2.Value.AddDays(-1);
                n1 = n1.Value.AddDays(-1);
                n2 = n2.Value.AddDays(-1);
                n1b = n1b.Value.AddDays(-1);
            }
            txtDaterange.Text = m1.Value.ToShortDateString();
            #endregion
            #region txtMorningOutput & txtNightOutput & txtPreviousDateTotalOutput
            string qty_sql = @"
--可完成的件數
select QtyAll=format(isnull(sum(QtySM),0),'#,0.')
from
(
	select FactoryID,[SP#],[Article],SizeCode,[Comb],QtySM = min(QtySum)
	from(
		Select DISTINCT
				o.FactoryID,
			[SP#] = b.Orderid,
			[Article] = b.Article,
			bd.SizeCode,
			[Comb] = b.PatternPanel,
			[Artwork] = sub.sub,
			[Pattern] = bd.PatternCode,
			[QtySum] = sum(bd.Qty) over(partition by o.FactoryID,b.Orderid,b.Article,bd.SizeCode,b.PatternPanel,sub.sub,bd.PatternCode)
		from Bundle b WITH (NOLOCK) 
		inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
		left join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
		inner join orders o WITH (NOLOCK) on o.Id = b.OrderId
		inner join SubProcess s WITH (NOLOCK) on (s.IsRFIDDefault = 1 or s.Id = bda.SubprocessId) 
		inner join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
		outer apply(
				select sub= stuff((
					Select distinct concat('+', bda.SubprocessId)
					from Bundle_Detail_Art bda WITH (NOLOCK) 
					where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
					for xml path('')
				),1,1,'')
		) as sub
		where 1=1
		and (s.Id = '{0}' or '{0}' = '')
        and (o.FactoryID = '{1}' or '{1}' = '')
        and bio.InComing between '{2}'and '{3}' {4}
		and b.PatternPanel in(
			Select distinct oe.PatternPanel
			From dbo.Order_EachCons a WITH (NOLOCK) 
			Left Join dbo.Orders b WITH (NOLOCK) On a.ID = b.ID  
			left join dbo.Order_BOF bof WITH (NOLOCK) on bof.Id = a.Id and bof.FabricCode = a.FabricCode
			left join Order_EachCons_PatternPanel oe WITH (NOLOCK) on oe.Order_EachConsUkey = a.Ukey
			Where a.ID = o.POID and bof.kind !=0
		)
	)aa
	group by FactoryID,[SP#],[Article],SizeCode,[Comb]
)bb";
            string m_qty = string.Format(qty_sql, subprocess, factory, m1.Value.ToAppDateTimeFormatString(), m2.Value.ToAppDateTimeFormatString(), "");
            string n_qty = string.Format(qty_sql, subprocess, factory, n1.Value.ToAppDateTimeFormatString(), n2.Value.ToAppDateTimeFormatString(), "");
            string A_qty = string.Format(qty_sql, subprocess, factory, m1.Value.AddDays(-1).ToAppDateTimeFormatString(), n2.Value.AddDays(-1).ToAppDateTimeFormatString(), "");
            string B_qty = string.Format(qty_sql, subprocess, factory, n1b.Value.ToAppDateTimeFormatString(), n2.Value.ToAppDateTimeFormatString(), "and OutGoing is null");

            txtMorningOutput.Text = MyUtility.GetValue.Lookup(m_qty);
            txtNightOutput.Text = MyUtility.GetValue.Lookup(n_qty);
            txtPreviousDateTotalOutput.Text = MyUtility.GetValue.Lookup(A_qty);
            txtPreviousDateWIP.Text = MyUtility.GetValue.Lookup(B_qty);
            #endregion

            #region grid datas重撈
            DT = null;
            string sqlcmd = string.Format(@"
select FactoryID,[SP#]
	,styleid
	,Qty
	,[Cut part Qty/Set] = count([Pattern])
	,[Article],SizeCode,[Comb],QtySM = min(QtySum)
into #tmp1
from(
	Select DISTINCT
		o.FactoryID,
		[SP#] = b.Orderid,
		o.styleid,
		o.Qty,
		[Article] = b.Article,
		bd.SizeCode,
		[Comb] = b.PatternPanel,
		[Artwork] = sub.sub,
		[Pattern] = bd.PatternCode,
		[QtySum] = sum(bd.Qty) over(partition by o.FactoryID,b.Orderid,b.Article,bd.SizeCode,b.PatternPanel,sub.sub,bd.PatternCode)
	from Bundle b WITH (NOLOCK) 
	inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
	left join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
	inner join orders o WITH (NOLOCK) on o.Id = b.OrderId
	inner join SubProcess s WITH (NOLOCK) on (s.IsRFIDDefault = 1 or s.Id = bda.SubprocessId) 
	inner join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
	outer apply(
			select sub= stuff((
				Select distinct concat('+', bda.SubprocessId)
				from Bundle_Detail_Art bda WITH (NOLOCK) 
				where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
				for xml path('')
			),1,1,'')
	) as sub
	where 1=1
    and (s.Id = '{0}' or '{0}' = '')
    and (o.FactoryID = '{1}' or '{1}' = '')
    and bio.InComing between '{2}'and '{3}'
	and b.PatternPanel in(
		Select distinct oe.PatternPanel
		From dbo.Order_EachCons a WITH (NOLOCK) 
		Left Join dbo.Orders b WITH (NOLOCK) On a.ID = b.ID  
		left join dbo.Order_BOF bof WITH (NOLOCK) on bof.Id = a.Id and bof.FabricCode = a.FabricCode
		left join Order_EachCons_PatternPanel oe WITH (NOLOCK) on oe.Order_EachConsUkey = a.Ukey
		Where a.ID = o.POID and bof.kind !=0
	)
)aa
group by FactoryID,[SP#],Qty,styleid,[Article],SizeCode,[Comb]
-----
select 
	o.FactoryID,
	[SP#] = b.Orderid,
	[IN CUT PARTS QTY]=sum(bd.qty)
into #tmp2
from Bundle b WITH (NOLOCK) 
inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
inner join orders o WITH (NOLOCK) on o.Id = b.OrderId
inner join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno
	where 1=1
and (bio.SubProcessId = '{0}' or '{0}' = '')
and (o.FactoryID = '{1}' or '{1}' = '')
and bio.InComing between '{2}'and '{3}'
group by o.FactoryID,b.Orderid
------------------------------------------------------------------
select FactoryID,[SP#],[Article],SizeCode,[Comb],[OUT SET QTY] = min([QtySum])
into #tmp3
from(
	Select DISTINCT
		o.FactoryID,
		[SP#] = b.Orderid,
		o.styleid,
		o.Qty,
		[Article] = b.Article,
		bd.SizeCode,
		[Comb] = b.PatternPanel,
		[Artwork] = sub.sub,
		[Pattern] = bd.PatternCode,
		[QtySum] = sum(bd.Qty) over(partition by o.FactoryID,b.Orderid,b.Article,bd.SizeCode,b.PatternPanel,sub.sub,bd.PatternCode)
	from Bundle b WITH (NOLOCK) 
	inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
	left join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
	inner join orders o WITH (NOLOCK) on o.Id = b.OrderId
	inner join SubProcess s WITH (NOLOCK) on (s.IsRFIDDefault = 1 or s.Id = bda.SubprocessId) 
	inner join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
	outer apply(
			select sub= stuff((
				Select distinct concat('+', bda.SubprocessId)
				from Bundle_Detail_Art bda WITH (NOLOCK) 
				where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
				for xml path('')
			),1,1,'')
	) as sub
	where 1=1
    and (s.Id = '{0}' or '{0}' = '')
    and (o.FactoryID = '{1}' or '{1}' = '')
    and bio.InComing between '{2}'and '{3}'
	and bio.OutGoing is not null
	and b.PatternPanel in(
		Select distinct oe.PatternPanel
		From dbo.Order_EachCons a WITH (NOLOCK) 
		Left Join dbo.Orders b WITH (NOLOCK) On a.ID = b.ID  
		left join dbo.Order_BOF bof WITH (NOLOCK) on bof.Id = a.Id and bof.FabricCode = a.FabricCode
		left join Order_EachCons_PatternPanel oe WITH (NOLOCK) on oe.Order_EachConsUkey = a.Ukey
		Where a.ID = o.POID and bof.kind !=0
	)
)aa
group by FactoryID,[SP#],Qty,styleid,[Article],SizeCode,[Comb]


select sp=a.SP#
	,a.StyleID
	,a.Qty
	,cpqs=a.[Cut part Qty/Set]
	,QtySM = sum(a.QtySM)
	,icpq = sum(b.[IN CUT PARTS QTY])
	,osq = sum(c.[OUT SET QTY])
from #tmp1 a 
inner join #tmp2 b on a.FactoryID =b.FactoryID and a.SP# = b. SP#
inner join #tmp3 c on a.FactoryID =c.FactoryID and a.SP# = c. SP# and a.Article = c.Article and a.SizeCode = c.SizeCode and a.Comb = c. Comb
group by a.SP#	,a.StyleID	,a.Qty	,a.[Cut part Qty/Set]
order by a.SP#
drop table #tmp1,#tmp2,#tmp3
", subprocess, factory, m1.Value.ToAppDateTimeFormatString(), n2.Value.ToAppDateTimeFormatString());

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DT);

            if (DT == null) return;
            if (DT.Rows.Count == 0) return;
            listControlBindingSource1.DataSource = null;
            listControlBindingSource1.DataSource = DT.AsEnumerable().Take(c1).CopyToDataTable();//依據c1筆數,顯示在第一頁
            grid1.AutoResizeColumns();
            #endregion
            #region 分頁的label
            count = DT.Rows.Count;
            last = count % c1;
            pages = last == 0 ? count / c1 : count / c1 + 1;//重設總頁數
            p = 1;//重撈後,當前頁數回到第1頁
            lbPages.Text = string.Format("{0} / {1}", p, pages);
            #endregion
        }

        int count;//總筆數
        int c1 = 10;//設定每頁筆數,預設15筆
        int pages;//總頁數
        int last;//最後一頁筆數
        int p = 1;//當前頁數
        private void changepages()
        {
            if (DT == null) return;
            if (DT.Rows.Count == 0) return;

            #region grid顯示
            listControlBindingSource1.DataSource = null;
            listControlBindingSource1.DataSource = DT.AsEnumerable().Skip(c1 * (p - 1)).Take(c1).CopyToDataTable();//跳過每頁幾筆,顯示
            grid1.AutoResizeColumns();
            #endregion

            #region 顯示當前第幾頁 /總頁數
            lbPages.Text = string.Format("{0} / {1}", p, pages);
            if (p < pages) p++;
            else p = 1;
            #endregion
        }

        int width = SystemInformation.PrimaryMonitorSize.Width;
        int height = SystemInformation.PrimaryMonitorSize.Height;
        int widthbase = 0;
        int heightbase = 30;
        int HeaderFontSize = 18;
        int FontSize = 15;
        private void setGridStyle()
        {
            /*
                 * 依照Taipei IT 解析度 width= 1440 * Height= 900 為基準值
                 * GridView欄高= 基準值(126) * (user螢幕解析度 高度)/ 900 (Taipei IT 螢幕解析度Height= 900)
                 * GridView欄寬= (user螢幕解析度) * 0.8(GridView佔畫面80%)/ 27 (GridView內容字數分成27等份)
                 * FontSize= 基準值(30=字型size ,18=Header字型size) * width (user螢幕解析度 寬度)/ 1440 (Taipei IT 螢幕解析度width= 1440)
                */

            if (width < 1440 || height < 900)
            {
                //比基準值小將寬度*0.95 ,如果user解析度=1024*768則再*0.88
                widthbase = width <= 1024 ? (int)(((width * 0.8) / 27) * 0.88) : (int)(((width * 0.8) / 27) * 0.85);
                HeaderFontSize = (int)Math.Round((double)(18 * (width / 1440.00)), 0);
                FontSize = (int)Math.Round((double)(15 * (width / 1440.00)), 0);

                heightbase = (int)Math.Round((double)(70 * (height / 900.00)), 0);

            }
            else
            {
                widthbase = (int)(width * 0.8) / 27;
                HeaderFontSize = (int)Math.Round((double)(18 * (width / 1440.00)), 0);
                FontSize = (int)Math.Round((double)(23 * (width / 1440.00)), 0);

                heightbase = (int)Math.Round((double)(70 * (height / 900.00)), 0);
            }


            this.grid1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", HeaderFontSize);
            this.grid1.DefaultCellStyle.Font = new Font("Tahoma", FontSize);
            grid1.RowTemplate.Height = heightbase;
            grid1.AutoResizeColumns();
        }

        private void RFID_Monitor_Board_Resize(object sender, EventArgs e)
        {
            setGridStyle();
        }

    }
}