using Sci.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Ict;
using Sci.Data;
using System.Net;
using System.IO;
using Sci;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Net.Mail;

namespace PMS_WeeklyPurchasingMonitoring
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        bool isAuto = false;
        DataRow mailTo_Carton;
        DataRow mailTo_Misc;
        TransferPms transferPMS = new TransferPms();
        StringBuilder sqlmsg = new StringBuilder();
        private DataSet ds = new DataSet();
        private string pathName;
        private string pathName2;
        private string MailList_ForError = ConfigurationManager.AppSettings["MailList_ForError"].ToString();

        public Main()
        {
            InitializeComponent();
            this.EditMode = true;
            isAuto = false;
            this.dateExcute.Value = DateTime.Now;
            this.dateExcute.ReadOnly = false;
        }

        public Main(String _isAuto)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(_isAuto))
            {
                isAuto = true;
                this.OnFormLoaded();
            }
            this.EditMode = true;
            this.dateExcute.ReadOnly = false;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            OnRequery("Production");
            OnRequery("Machine");

            if (isAuto)
            {
                ClickExcute();
                this.Close();
            }
        }

        private void OnRequery(string DBConnection)
        {
            DataTable _mailTo;
            String sqlCmd;

            switch (DBConnection)
            {
                case "Production":
                    sqlCmd = "Select * From dbo.MailTo Where ID = '025'";
                    break;
                case "Machine":
                    sqlCmd = "Select * From dbo.MailTo Where ID = '011'";
                    break;
                default:
                    sqlCmd = "";
                    break;
            }

            DualResult result = DBProxy.Current.Select(DBConnection, sqlCmd, out _mailTo);

            if (!result)
            {
                if (this.isAuto)
                {
                    throw result.GetException();
                }
                else
                {
                    ShowErr(result);
                    return;
                }
            }

            if (DBConnection == "Production")
            {
                if (_mailTo.Rows.Count <= 0)
                {
                    ShowErr("MailTo ID 025 not exists!");
                    return;
                }
                else
                {
                    this.mailTo_Carton = _mailTo.Rows[0];
                }
            }
            else
            {
                if (_mailTo.Rows.Count <= 0)
                {
                    ShowErr("MailTo ID 011 not exists!");
                    return;
                }
                else
                {
                    this.mailTo_Misc = _mailTo.Rows[0];
                }
            }
        }

        #region 接Sql Server 進度訊息用
        private void InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            BeginInvoke(() => { this.labelProgress.Text = e.Message; });
        }
        #endregion

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ClickExcute();
        }

        #region Send Mail
        private void SendMail(DataRow drMailTo, string mailType, string pathName, bool isFail = false)
        {
            String mailServer = this.CurrentData["MailServer"].ToString();
            String eMailID = this.CurrentData["EMailID"].ToString();
            String eMailPwd = this.CurrentData["EMailPwd"].ToString();
            ushort mailServerPort = MyUtility.Check.Empty(this.CurrentData["MailServerPort"]) ? Convert.ToUInt16(25) : Convert.ToUInt16(this.CurrentData["MailServerPort"]);
            transferPMS.SetSMTP(mailServer, mailServerPort, eMailID, eMailPwd);
            List<Attachment> listAttach = new List<Attachment>();

            if (!MyUtility.Check.Empty(pathName))
            {
                listAttach.Add(new Attachment(pathName));
            }

            String sendFrom = this.CurrentData["SendFrom"].ToString();
            String subject = drMailTo["Subject"].ToString();
            string desc = string.Empty;
            #region 組html Desc
            desc += HtmlStyle() + Environment.NewLine +
               $@"
<font size=4>
Outstanding Qty as of {((DateTime)this.dateExcute.Value).AddDays(-1).ToString("yyyy/MM/dd")} <br>
";
            if (mailType == "Carton")
            {
                if (this.ds.Tables["PendingPIVOT"].Rows.Count > 0)
                {
                    desc += DataTableChangeHtml_WithOutStyleHtml(this.ds.Tables["PendingPIVOT"]) + "<br>";
                }
            }
            else if (mailType == "Misc")
            {
                if (this.ds.Tables["OutstandingPIVOT"].Rows.Count > 0)
                {
                    desc += DataTableChangeHtml_WithOutStyleHtml(this.ds.Tables["OutstandingPIVOT"]) + "<br>";
                }
            }
            else
            {
                desc = drMailTo["Description"].ToString();
            }

            desc += HtmlStyle() + Environment.NewLine +
               $@"
<font size=4>
On-Time rate from {((DateTime)this.dateExcute.Value).AddDays(-8).ToString("yyyy/MM/dd")} to {((DateTime)this.dateExcute.Value).AddDays(-1).ToString("yyyy/MM/dd")} <br>
";
            if (mailType == "Carton")
            {
                if (this.ds.Tables["POListPIVOT"].Rows.Count > 0)
                {
                    desc += DataTableChangeHtml_WithOutStyleHtml(this.ds.Tables["POListPIVOT"]) + "<br>";
                }
            }
            else if (mailType == "Misc")
            {
                if (this.ds.Tables["OnTimePIVOT"].Rows.Count > 0)
                {
                    desc += DataTableChangeHtml_WithOutStyleHtml(this.ds.Tables["OnTimePIVOT"]) + "<br>";
                }
            }
            else
            {
                desc = drMailTo["Description"].ToString();
            }
            #endregion

            String toAddress = MyUtility.Check.Empty(drMailTo["ToAddress"].ToString()) ? drMailTo["CcAddress"].ToString() : drMailTo["ToAddress"].ToString();
            String ccAddress = MyUtility.Check.Empty(drMailTo["CcAddress"].ToString()) ? string.Empty : drMailTo["CcAddress"].ToString();

            if (isFail)
            {
                toAddress = MailList_ForError;
            }

            if (!MyUtility.Check.Empty(toAddress))
            {
                //Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, pathName, desc, true, true);
                MailToHtml(
                     mailFrom: sendFrom
                    , mailServer: mailServer
                    , eMailID: eMailID
                    , eMailPwd: eMailPwd
                    , mailPort: mailServerPort
                    , subject: subject
                    , attachs: listAttach
                    , desc: desc
                    , mailTO: toAddress
                    , mailTOCC: ccAddress
                    , isBodyHtml: true
                    );
            }
        }
        #endregion

        #region Update/Update動作
        private void ClickExcute()
        {
            SqlConnection conn;

            if (!Sci.SQL.GetConnection(out conn)) { return; }

            conn.InfoMessage += new SqlInfoMessageEventHandler(InfoMessage);

            DualResult result;
            result = Query();
            //result = AsyncUpdateExport();
            //if (isAuto)
            //{
            //    result = Query();
            //}
            //else
            //{
            //    result = AsyncHelper.Current.DataProcess(this, () =>
            //    {
            //        return Query();
            //    });
            //}

            if (this.ds == null)
            {
                mymailTo(result.ToString());
                return;
            }

            List<string> files = this.CreateExcel_OneSheet();

            if (files.Count > 0)
            {
                mymailTo(result.ToString());
            }

            conn.Close();
            issucess = true;
        }

        private void mymailTo(string msg)
        {
            #region 完成後發送Mail
            #region 組合 Desc

            if (!MyUtility.Check.Empty(msg))
            {
                issucess = false;
            }
            #endregion

            #region 第一封信
            SendMail(this.mailTo_Carton, "Carton", this.pathName, !issucess);
            #endregion

            #region 第二封信
            SendMail(this.mailTo_Misc, "Misc", this.pathName2, !issucess);
            #endregion


            #endregion
        }
        #endregion
        bool issucess = true;

        private DualResult Query()
        {
            DualResult result;
            string sqlcmd = string.Empty;

            // Production Subcon_R26
            #region Pending
            sqlcmd = $@"
select DISTINCT c.FactoryID
	        ,[OriFty] = a.FactoryId
            ,c.POID
	        ,b.OrderId
	        ,c.StyleID
	        ,s.StyleName
            ,c.SciDelivery
            ,c.BuyerDelivery
            ,c.SewInLine
            ,c.brandid
	        ,c.SeasonID
	        ,[Supp] = a.LocalSuppID+'-'+d.Abb 
	        ,b.Delivery
	        ,b.Refno
            ,Category = Iif(a.category = 'CARTON' AND iscarton = 0, 'CARDBOARD', a.category) 
            ,[IsCarton] = iif(li.IsCarton = 1 ,'Y','')
	        ,[Desc] = b.ThreadColorID + ' - ' + ThreadColor.Description		
			,[PRConfirmedDate]=CASE WHEN a.Category = 'CARTON' 
									THEN (SELECT TOP 1 p.ApvToPurchaseDate 
											FROM PackingList p
											WHERE p.ID=b.RequestID)
									WHEN a.Category = 'SP_THREAD' 
									THEN (SELECT TOP 1 t.EditDate
											FROM ThreadRequisition t
											WHERE t.OrderID=b.RequestID )
									ELSE NULL
								END
	        ,a.IssueDate
	        ,[Description] = dbo.getItemDesc(a.Category,b.Refno) 
	        ,b.qty
	        ,b.UnitId
            ,a.CurrencyID 
	        ,b.Price
	        ,[Amount] = b.Qty*b.Price
            ,x.KPIRate
            ,[Amount (USD)] = iif(x.KPIRate = 0, 0, b.Qty*b.Price/x.KPIRate)
	        ,b.InQty
            ,[OnRoadBalance] = CONVERT(int, b.qty) - CONVERT(int, b.InQty)
	        ,b.APQty
            ,a.id
			,a.Status
			,[LastIssueDate] = rec.IssueDate
            ,b.RequestID
	        ,b.Remark
into #tmp
from localpo a WITH (NOLOCK) 
inner join LocalPO_Detail b WITH (NOLOCK) on a.id=b.id
inner join orders c WITH (NOLOCK) on c.ID = b.OrderId
left join Style s (NOLOCK) on s.Ukey = c.StyleUkey
left join localsupp d  WITH (NOLOCK) on  d.id =a.LocalSuppID 
left join ThreadColor on b.ThreadColorID = ThreadColor.ID
left join LocalItem li WITH (NOLOCK) on li.RefNo=b.Refno
outer apply(select KPIRate = dbo.getrate('KP','USD',a.CurrencyID ,a.IssueDate))x
OUTER APPLY(
	SELECT TOP 1 l.IssueDate
	FROM LocalReceiving_Detail LD
	INNER JOIN LocalReceiving L on L.Id=LD.Id 
	WHERE LD.LocalPo_detailukey = b.Ukey
	ORDER BY l.AddDate DESC
)rec
    where b.qty > b.InQty and a.Status<>'Closed' 
    and b.Delivery <= '{((DateTime)this.dateExcute.Value).AddDays(-1).ToString("yyyy/MM/dd")}'

select * from #tmp

select distinct BrandID,Description,Total 
into #tmpTTL
from (
	select * 
	,[Total] = m.Value
	from #tmp f
	outer apply(
		select [Value] = sum(OnRoadBalance)
		from #tmp a
		where a.Description = f.Description
		and a.BrandID = f.BrandID
	)M
) a

select distinct BrandID,Description = '',Total
into #tmpTTL_Brand
from (
	select * 
	,[Total] = m.Value
	from #tmp f
	outer apply(
		select [Value] = sum(OnRoadBalance)
		from #tmp a
		where a.BrandID = f.BrandID
	)M
) a

--select * from #tmpTTL_Brand

select * 
into #tmpTTL2
from 
(
	select BrandID, Description, Total from #tmpTTL
	union all
	select BrandID, Description, Total from #tmpTTL_Brand
) a



select a.* ,b.Total
into #tmpFinal
from #tmp a
left join #tmpTTL2 b on a.Description = b.Description and a.BrandID = b.BrandID

select a.* ,b.Total
into #tmpFinal_2
from #tmp a
inner join #tmpTTL2 b on a.BrandID = b.BrandID --and a.Description = b.Description
where b.Description = ''


declare @columnsName nvarchar(max) = stuff(REPLACE((select concat(',[', supp,']') from(select distinct supp from #tmp ) s for xml path('')),'&amp;','&'),1,1,'')
--select @columnsName

declare @lastSql nvarchar(max) = N'


select BrandID, Description = replace(Description, ''ZZZ'', ''''),'+@columnsName+N' , [Total]
from
(
    select BrandID, Description,'+@columnsName+N' , [Total]
    from (
        select BrandID, Description, sum(OnRoadBalance) as OnRoadBalance, SUPP, Total
        from #tmpFinal
		group by BrandID, Description, SUPP, Total

    ) as t
    PIVOT(
        SUM(OnRoadBalance) for SUPP IN ('+@columnsName+N')
	) as s

    union all

    select BrandID,Description = ''ZZZ'' ,'+@columnsName+N' ,[Total]
        from(
        select BrandID, sum(OnRoadBalance) as OnRoadBalance, SUPP, Total
        from #tmpFinal_2
		group by BrandID, SUPP, Total

    ) as t
    PIVOT(
        SUM(OnRoadBalance) for SUPP IN ('+@columnsName+N')
	) as s
) a
order by BrandID, Description 
'

EXEC sp_executesql @lastSql
--print @lastsql
drop table #tmp,#tmpFinal,#tmpFinal_2,#tmpTTL,#tmpTTL_Brand,#tmpTTL2


";

         
            if (!(result = DBProxy.Current.Select("Production",sqlcmd, out DataTable[] dsPending)))
            {
                this.ShowErr(result);
                return result;
            }
            #endregion

            #region PO List
            sqlcmd = $@"

select DISTINCT c.FactoryID
	        ,[OriFty] = a.FactoryId
            ,c.POID
	        ,b.OrderId
	        ,c.StyleID
	        ,s.StyleName
            ,c.SciDelivery
            ,c.BuyerDelivery
            ,c.SewInLine
            ,c.brandid
	        ,c.SeasonID
	        ,[Supp] = a.LocalSuppID+'-'+d.Abb 
	        ,b.Delivery
	        ,b.Refno
            ,Category = Iif(a.category = 'CARTON' AND iscarton = 0, 'CARDBOARD', a.category) 
            ,[IsCarton] = iif(li.IsCarton = 1 ,'Y','')
	        ,[Desc] = b.ThreadColorID + ' - ' + ThreadColor.Description		
			,[PRConfirmedDate]=CASE WHEN a.Category = 'CARTON' 
									THEN (SELECT TOP 1 p.ApvToPurchaseDate 
											FROM PackingList p
											WHERE p.ID=b.RequestID)
									WHEN a.Category = 'SP_THREAD' 
									THEN (SELECT TOP 1 t.EditDate
											FROM ThreadRequisition t
											WHERE t.OrderID=b.RequestID )
									ELSE NULL
								END
	        ,a.IssueDate
	        ,[Description] = dbo.getItemDesc(a.Category,b.Refno) 
	        ,b.qty
	        ,b.UnitId
            ,a.CurrencyID 
	        ,b.Price
	        ,[Amount] = b.Qty*b.Price
            ,x.KPIRate
            ,[Amount (USD)] = iif(x.KPIRate = 0, 0, b.Qty*b.Price/x.KPIRate)
	        ,b.InQty
            ,[OnRoadBalance] = CONVERT(int, b.qty) - CONVERT(int, b.InQty)
	        ,b.APQty
            ,a.id
			,a.Status
			,[LastIssueDate] = rec.IssueDate
			,[DelayStatus] = case when rec.IssueDate > b.Delivery then 'Delay' 
							  when b.qty - b.InQty > 0 then 'Delay'
							  else 'One-Time' end
            ,b.RequestID
	        ,b.Remark
into #tmp
from localpo a WITH (NOLOCK) 
inner join LocalPO_Detail b WITH (NOLOCK) on a.id=b.id
left join orders c WITH (NOLOCK) on c.ID = b.OrderId
left join Style s (NOLOCK) on s.Ukey = c.StyleUkey
left join localsupp d  WITH (NOLOCK) on  d.id =a.LocalSuppID 
left join ThreadColor on b.ThreadColorID = ThreadColor.ID
left join LocalItem li WITH (NOLOCK) on li.RefNo=b.Refno
outer apply(select KPIRate = dbo.getrate('KP','USD',a.CurrencyID ,a.IssueDate))x
OUTER APPLY(
	SELECT TOP 1 l.IssueDate
	FROM LocalReceiving_Detail LD
	INNER JOIN LocalReceiving L on L.Id=LD.Id 
	WHERE LD.LocalPo_detailukey = b.Ukey
	ORDER BY l.AddDate DESC
)rec
    where a.Status<>'Closed' 
    and b.Delivery between  '{((DateTime)this.dateExcute.Value).AddDays(-8).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateExcute.Value).AddDays(-1).ToString("yyyy/MM/dd")}'

select * from #tmp

select Supp,[Delay] = CONVERT(varchar, [Delay]) + '%',[One-Time] = CONVERT(varchar, [One-Time]) + '%'
	from (
		select Supp, [Percent] = ROUND(CONVERT(float,sum(t.Qty))/ttl.Qty,4) * 100 
		,DelayStatus
		from #tmp t
		outer apply(
			select Qty = sum(Qty)
			from #tmp s
			where t.Supp = s.Supp
		)	ttl
		group by Supp, DelayStatus,ttl.Qty
	) as t
	PIVOT(
		sum([Percent]) for DelayStatus IN ([Delay],[One-Time]) 
	) as s
	order by Supp

drop table #tmp
"; 

            if (!(result = DBProxy.Current.Select("Production", sqlcmd, out DataTable[] dsPOList)))
            {
                this.ShowErr(result);
                return result;
            }

            #endregion

            // Machine Misc_R02.Purchase Order List
            #region Outstanding Report

            sqlcmd = $@"

SELECT distinct [PurchaseFrom] = forPurchaseFrom.PurchaseFrom
    ,P.MDivisionID
    ,P.FactoryID
    ,P.ID
    ,MiscReq.ApproveDate
    ,P.CDate
    ,P.DeliveryDate
    ,[Age] = DATEDIFF(day,p.DeliveryDate,getdate())
	,[Delay] = case when DATEDIFF(day,p.DeliveryDate,getdate()) < 8 then 'One Week'
					when DATEDIFF(day,p.DeliveryDate,getdate()) between 8 and 14 then 'Two Weeks'
					when DATEDIFF(day,p.DeliveryDate,getdate()) < 31 then 'One Month'
					else 'Beyond One Month' end
    ,C.PurchaseType
    ,[Supplier] = forPurchaseFrom.Supplier
    ,[Status] = iif(P.PurchaseFrom ='T' and PD.Junk =1 , 'Cancel', p.Status)
    ,PD.MiscReqID
	,[PR Date] = MiscReq.CDate
	,PD.MiscID
    ,C.Description
    ,PD.Qty
    ,PD.UnitID
    ,[CurrencyID] = forPurchaseFrom.CurrencyID
    ,[Price] = forPurchaseFrom.Price
	,[U/P(USD)]= cast(isnull(forPurchaseFrom.Price / CurrencyRate.Rate ,0) as numeric(16,4))
	,[amount] = case when P.PurchaseFrom ='L' then
					case UPPER(MiscReq.Purchasetype) 
						when 'RENTAL MACHINE' then PD.Qty * forPurchaseFrom.Price *  isnull(MiscReq.RentalDay,0)
						else PD.Qty * forPurchaseFrom.Price
					 end
				else PD.QTY * forPurchaseFrom.Price
				end
    ,[POAmount(USD)]= case when P.PurchaseFrom ='L' then
					case UPPER(MiscReq.Purchasetype) 
						when 'RENTAL MACHINE' then PD.Qty * cast(isnull(forPurchaseFrom.Price / CurrencyRate.Rate ,0) as numeric(16,4)) *  isnull(MiscReq.RentalDay,0)
						else PD.Qty * cast(isnull(forPurchaseFrom.Price / CurrencyRate.Rate ,0) as numeric(16,4))
					 end
				else PD.QTY * cast(isnull(forPurchaseFrom.Price / CurrencyRate.Rate ,0) as numeric(16,4))
				end
    ,PD.InQty
    ,PD.TPEPOID
    ,PD.TPEQty
    ,PD.TPECurrencyID
    ,PD.TPEPrice
    ,[Amount(Taipei)] = isnull(PD.TPEPrice, 0) * isnull(PD.TPEQty,0)
    ,pd.ApQty
	,[APAmount] = MiscAP_amount.val
    ,[Rental Day] = isnull(MiscReq.RentalDay,0)
	,[Incoming Date] = MiscIn_MaxCDate.CDate
	,[A/P Approved Date] =MiscAP_MaxCDate.ApproveDate
	,[Invoice #] = Stuff((SELECT distinct concat( ',', t.val)
						from 
						(
							select [val] = a.Invoice
							from MiscAP a
							inner join MiscAp_Detail b on a.ID=b.ID
							where b.MiscPOID= p.ID  and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
							and b.MiscID=c.ID and b.MiscReqID=MR.ID and b.DepartmentID=pd.DepartmentID    
							and a.Status = 'Approved'
						)t for xml path('')),1,1,'') 
    ,[RequestReason] = MRD.Reason
	,PD.DepartmentID
    ,c.AccountID
    ,[AccountName] = (select name from dbo.SciFMS_AccountNo where id=c.AccountID)
    ,[AccountCategory] = (select AccountCategoryID from dbo.SciFMS_AccountNo where id=c.AccountID)
	,[Budget] = dbo.GetBudgetType(P.MDivisionID,c.AccountID)
	,[Internal Remarks] = p.Remark1
    ,[APID] = Stuff((SELECT distinct concat( ',', t.val)
					 from 
					 (
					 	select [val] = a.ID
					 	from MiscAP a
					 	inner join MiscAp_Detail b on a.ID=b.ID
					 	where b.MiscPOID= p.ID  and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
					 	and b.MiscID=c.ID and b.MiscReqID=MR.ID and b.DepartmentID=pd.DepartmentID    
					 	and a.Status = 'Approved'
					 )t for xml path('')),1,1,'')
into #tmp
FROM MiscPO P
LEFT JOIN SciProduction_LocalSupp LS ON LS.ID= P.LocalSuppID
LEFT JOIN MiscPO_Detail PD ON PD.ID = P.ID 
LEFT JOIN SciProduction_Supp S ON S.ID = PD.SuppID
LEFT JOIN Misc C ON C.ID = PD.MiscID
LEFT JOIN MiscReq MR ON MR.ID = PD.Miscreqid
LEFT JOIN MiscReq_Detail MRD ON MR.ID = MRD.ID and PD.MiscID = MRD.MISCID
OUTER APPLY(
	select a.CDate ,a.ID ,a.ApproveDate, a.Purchasetype, b.RentalDay
	from MiscReq a
	inner join MiscReq_Detail b on a.ID=b.ID
	where b.MiscID=c.ID
	and MiscPOID=p.ID
    and a.id = PD.MiscReqID
	and P.PurchaseFrom = 'L'
)MiscReq
OUTER APPLY(
	select b.RentalDay
	from MiscIn a 
	inner join MiscIn_Detail b on a.ID=b.ID
	where b.MiscPOID=p.ID 
	and a.Status = 'Confirmed'
	and b.MiscReqID=MiscReq.ID and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
)MiscIn
OUTER APPLY(
	select [CDate] = max(a.CDate) 
	from MiscIn a 
	inner join MiscIn_Detail b on a.ID=b.ID
	where b.MiscPOID=p.ID 
	and a.Status = 'Confirmed'
	and b.MiscReqID=MiscReq.ID and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
)MiscIn_MaxCDate
OUTER APPLY(
	select [val] = sum(t.amount)
	from 
	(
		select [amount] = case UPPER(MiscReq.Purchasetype) 
							when 'RENTAL MACHINE' then b.Qty * b.Price *  isnull(MiscIn.RentalDay,0)
							else b.Qty * b.Price end
		from MiscAP a
		inner join MiscAp_Detail b on a.ID=b.ID
		where b.MiscPOID= p.ID and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
		and b.MiscID=c.ID and b.MiscReqID=MiscReq.ID and b.DepartmentID=pd.DepartmentID    
		and a.Status = 'Approved'
	)t
)MiscAP_amount 
OUTER APPLY(
	select [ApproveDate] = max(a.ApproveDate) 
	from MiscAP a
	inner join MiscAp_Detail b on a.ID=b.ID
	where b.MiscPOID= p.ID and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
	and b.MiscID=c.ID and b.MiscReqID=MiscReq.ID and b.DepartmentID=pd.DepartmentID
	and a.Status = 'Approved'
)MiscAP_MaxCDate 
OUTER APPLY(
	select [CurrencyID] = case when P.PurchaseFrom ='T'  
                            then iif( (PD.ShipQty + PD.ShipFoc)> 0 and PD.TPECurrencyID > '' , PD.TPECurrencyID ,S.Currencyid)							
							else P.CurrencyID end
		,[Price] = case when P.PurchaseFrom ='T'  
                            then iif( (PD.ShipQty + PD.ShipFoc)> 0 and PD.TPECurrencyID > '' , PD.TPEPrice ,PD.Price)							
							else PD.Price end 
		,[Supplier] = case when P.PurchaseFrom ='L' then (P.LocalSuppID+'-'+ LS.Abb)
						   when P.PurchaseFrom ='T' then (PD.SuppID+'-'+S.AbbEN)
						else ''
					end
		,[PurchaseFrom] = case when P.PurchaseFrom = 'L' then 'Local' 
											  when P.PurchaseFrom = 'T' then 'Taipei'   
										 else ''
										 end
)forPurchaseFrom
OUTER APPLY(
	select *
	from dbo.[GetCurrencyRate]('KP', 'USD', forPurchaseFrom.CurrencyID, P.CDate)
)CurrencyRate
where 1=1
and  p.Status='Approved' 
and  pd.Qty  > pd.InQty  
and  p.PurchaseFrom='L'
and P.DeliveryDate <= '{((DateTime)this.dateExcute.Value).AddDays(-1).ToString("yyyy/MM/dd")}'

select * from #tmp 
order by MDivisionID,FactoryID,CDate,Supplier,ID 

select distinct Supplier,Total 
into #tmpTTL
from (
	select * 
	,[Total] = m.Value
	from #tmp f
	outer apply(
		select [Value] = COUNT(ID)
		from #tmp a
		where a.Supplier = f.Supplier
	)M
) a

select a.*,b.Total 
into #tmpFinal
from #tmp a
inner join #tmpTTL b on a.Supplier = b.Supplier

declare @columnsName nvarchar(max) = stuff(REPLACE( (select concat(',[',Delay,']') from (select distinct Delay from #tmp ) s for xml path('')),'&amp;','&'),1,1,'')
--select @columnsName

declare @lastSql nvarchar(max) = N'
select Supplier,'+@columnsName+N' ,[Total] 
	from (
		select Supplier, count(ID) as ID,Delay,Total
		from #tmpFinal
		group by Supplier, Delay,Total
	) as t
	PIVOT(
		sum(ID) for Delay IN ('+@columnsName+N') 
	) as s
	order by Supplier
'

EXEC sp_executesql @lastSql

drop table #tmp,#tmpFinal,#tmpTTL
";
          
            if (!(result = DBProxy.Current.Select("Machine", sqlcmd, out DataTable[] dsOutstanding)))
            {
                this.ShowErr(result);
                return result;
            }
            #endregion

            #region Detail for on time
            sqlcmd = $@"

SELECT distinct [PurchaseFrom] = forPurchaseFrom.PurchaseFrom
    ,P.MDivisionID
    ,P.FactoryID
    ,P.ID
    ,MiscReq.ApproveDate
    ,P.CDate
    ,P.DeliveryDate
    ,C.PurchaseType
    ,[Supplier] = forPurchaseFrom.Supplier
    ,[Status] = iif(P.PurchaseFrom ='T' and PD.Junk =1 , 'Cancel', p.Status)
    ,PD.MiscReqID
	,[PR Date] = MiscReq.CDate
	,PD.MiscID
    ,C.Description
    ,PD.Qty
    ,PD.UnitID
    ,[CurrencyID] = forPurchaseFrom.CurrencyID
    ,[Price] = forPurchaseFrom.Price
	,[U/P(USD)]= cast(isnull(forPurchaseFrom.Price / CurrencyRate.Rate ,0) as numeric(16,4))
	,[amount] = case when P.PurchaseFrom ='L' then
					case UPPER(MiscReq.Purchasetype) 
						when 'RENTAL MACHINE' then PD.Qty * forPurchaseFrom.Price *  isnull(MiscReq.RentalDay,0)
						else PD.Qty * forPurchaseFrom.Price
					 end
				else PD.QTY * forPurchaseFrom.Price
				end
    ,[POAmount(USD)]= case when P.PurchaseFrom ='L' then
					case UPPER(MiscReq.Purchasetype) 
						when 'RENTAL MACHINE' then PD.Qty * cast(isnull(forPurchaseFrom.Price / CurrencyRate.Rate ,0) as numeric(16,4)) *  isnull(MiscReq.RentalDay,0)
						else PD.Qty * cast(isnull(forPurchaseFrom.Price / CurrencyRate.Rate ,0) as numeric(16,4))
					 end
				else PD.QTY * cast(isnull(forPurchaseFrom.Price / CurrencyRate.Rate ,0) as numeric(16,4))
				end
    ,PD.InQty
    ,PD.TPEPOID
    ,PD.TPEQty
    ,PD.TPECurrencyID
    ,PD.TPEPrice
    ,[Amount(Taipei)] = isnull(PD.TPEPrice, 0) * isnull(PD.TPEQty,0)
    ,pd.ApQty
	,[APAmount] = MiscAP_amount.val
    ,[Rental Day] = isnull(MiscReq.RentalDay,0)
	,[Incoming Date] = MiscIn_MaxCDate.CDate
    ,[DelayStatus] = case when PD.Qty > PD.InQty then 'Delay' 
						      when MiscIn_MaxCDate.CDate > p.DeliveryDate then 'Delay'
						      else 'One-Time' end
	,[A/P Approved Date] =MiscAP_MaxCDate.ApproveDate
	,[Invoice #] = Stuff((SELECT distinct concat( ',', t.val)
						from 
						(
							select [val] = a.Invoice
							from MiscAP a
							inner join MiscAp_Detail b on a.ID=b.ID
							where b.MiscPOID= p.ID  and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
							and b.MiscID=c.ID and b.MiscReqID=MR.ID and b.DepartmentID=pd.DepartmentID    
							and a.Status = 'Approved'
						)t for xml path('')),1,1,'') 
    ,[RequestReason] = MRD.Reason
	,PD.DepartmentID
    ,c.AccountID
    ,[AccountName] = (select name from dbo.SciFMS_AccountNo where id=c.AccountID)
    ,[AccountCategory] = (select AccountCategoryID from dbo.SciFMS_AccountNo where id=c.AccountID)
	,[Budget] = dbo.GetBudgetType(P.MDivisionID,c.AccountID)
	,[Internal Remarks] = p.Remark1
    ,[APID] = Stuff((SELECT distinct concat( ',', t.val)
					 from 
					 (
					 	select [val] = a.ID
					 	from MiscAP a
					 	inner join MiscAp_Detail b on a.ID=b.ID
					 	where b.MiscPOID= p.ID  and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
					 	and b.MiscID=c.ID and b.MiscReqID=MR.ID and b.DepartmentID=pd.DepartmentID    
					 	and a.Status = 'Approved'
					 )t for xml path('')),1,1,'')
into #tmp
FROM MiscPO P
LEFT JOIN SciProduction_LocalSupp LS ON LS.ID= P.LocalSuppID
LEFT JOIN MiscPO_Detail PD ON PD.ID = P.ID 
LEFT JOIN SciProduction_Supp S ON S.ID = PD.SuppID
LEFT JOIN Misc C ON C.ID = PD.MiscID
LEFT JOIN MiscReq MR ON MR.ID = PD.Miscreqid
LEFT JOIN MiscReq_Detail MRD ON MR.ID = MRD.ID and PD.MiscID = MRD.MISCID
OUTER APPLY(
	select a.CDate ,a.ID ,a.ApproveDate, a.Purchasetype, b.RentalDay
	from MiscReq a
	inner join MiscReq_Detail b on a.ID=b.ID
	where b.MiscID=c.ID
	and MiscPOID=p.ID
    and a.id = PD.MiscReqID
	and P.PurchaseFrom = 'L'
)MiscReq
OUTER APPLY(
	select b.RentalDay
	from MiscIn a 
	inner join MiscIn_Detail b on a.ID=b.ID
	where b.MiscPOID=p.ID 
	and a.Status = 'Confirmed'
	and b.MiscReqID=MiscReq.ID and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
)MiscIn
OUTER APPLY(
	select [CDate] = max(a.CDate) 
	from MiscIn a 
	inner join MiscIn_Detail b on a.ID=b.ID
	where b.MiscPOID=p.ID 
	and a.Status = 'Confirmed'
	and b.MiscReqID=MiscReq.ID and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
)MiscIn_MaxCDate
OUTER APPLY(
	select [val] = sum(t.amount)
	from 
	(
		select [amount] = case UPPER(MiscReq.Purchasetype) 
							when 'RENTAL MACHINE' then b.Qty * b.Price *  isnull(MiscIn.RentalDay,0)
							else b.Qty * b.Price end
		from MiscAP a
		inner join MiscAp_Detail b on a.ID=b.ID
		where b.MiscPOID= p.ID and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
		and b.MiscID=c.ID and b.MiscReqID=MiscReq.ID and b.DepartmentID=pd.DepartmentID    
		and a.Status = 'Approved'
	)t
)MiscAP_amount 
OUTER APPLY(
	select [ApproveDate] = max(a.ApproveDate) 
	from MiscAP a
	inner join MiscAp_Detail b on a.ID=b.ID
	where b.MiscPOID= p.ID and b.SEQ1=pd.SEQ1 and b.SEQ2=pd.SEQ2
	and b.MiscID=c.ID and b.MiscReqID=MiscReq.ID and b.DepartmentID=pd.DepartmentID
	and a.Status = 'Approved'
)MiscAP_MaxCDate 
OUTER APPLY(
	select [CurrencyID] = case when P.PurchaseFrom ='T'  
                            then iif( (PD.ShipQty + PD.ShipFoc)> 0 and PD.TPECurrencyID > '' , PD.TPECurrencyID ,S.Currencyid)							
							else P.CurrencyID end
		,[Price] = case when P.PurchaseFrom ='T'  
                            then iif( (PD.ShipQty + PD.ShipFoc)> 0 and PD.TPECurrencyID > '' , PD.TPEPrice ,PD.Price)							
							else PD.Price end 
		,[Supplier] = case when P.PurchaseFrom ='L' then (P.LocalSuppID+'-'+ LS.Abb)
						   when P.PurchaseFrom ='T' then (PD.SuppID+'-'+S.AbbEN)
						else ''
					end
		,[PurchaseFrom] = case when P.PurchaseFrom = 'L' then 'Local' 
											  when P.PurchaseFrom = 'T' then 'Taipei'   
										 else ''
										 end
)forPurchaseFrom
OUTER APPLY(
	select *
	from dbo.[GetCurrencyRate]('KP', 'USD', forPurchaseFrom.CurrencyID, P.CDate)
)CurrencyRate
where 1=1
and  p.Status='Approved' 
and  pd.Qty  > pd.InQty  
and  p.PurchaseFrom='L'
and P.DeliveryDate between  '{((DateTime)this.dateExcute.Value).AddDays(-8).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateExcute.Value).AddDays(-1).ToString("yyyy/MM/dd")}'

select * from #tmp 
order by MDivisionID,FactoryID,CDate,Supplier,ID 

select Supplier,[Delay] = CONVERT(varchar, [Delay]) + '%',[One-Time] = CONVERT(varchar, [One-Time]) + '%'
	from (
		select Supplier, [Percent] = ROUND(CONVERT(float,sum(t.Qty))/ttl.Qty,4) * 100 
		,DelayStatus
		from #tmp t
		outer apply(
			select Qty = sum(Qty)
			from #tmp s
			where t.Supplier = s.Supplier
		)	ttl
		group by Supplier, DelayStatus,ttl.Qty
	) as t
	PIVOT(
		sum([Percent]) for DelayStatus IN ([Delay],[One-Time]) 
	) as s
	order by Supplier
drop table #tmp
";

            if (!(result = DBProxy.Current.Select("Machine", sqlcmd, out DataTable[] dsOnTime)))
            {
                this.ShowErr(result);
                return result;
            }
            #endregion

            ds.Tables.Add(dsPending[0]);
            ds.Tables[0].TableName = "Pending";
            ds.Tables.Add(dsPending[1]);
            ds.Tables[1].TableName = "PendingPIVOT";

            ds.Tables.Add(dsPOList[0]);
            ds.Tables[2].TableName = "POList";
            ds.Tables.Add(dsPOList[1]);
            ds.Tables[3].TableName = "POListPIVOT";


            ds.Tables.Add(dsOutstanding[0]);
            ds.Tables[4].TableName = "Outstanding";
            ds.Tables.Add(dsOutstanding[1]);
            ds.Tables[5].TableName = "OutstandingPIVOT";

            ds.Tables.Add(dsOnTime[0]);
            ds.Tables[6].TableName = "OnTime";
            ds.Tables.Add(dsOnTime[1]);
            ds.Tables[7].TableName = "OnTimePIVOT";

            return Ict.Result.True;
        }

        private List<string> CreateExcel_OneSheet()
        {
            List<string> files = new List<string>();

            Ict.Logs.APP.LogInfo("Create XLT Start");
            Excel.Application objApp;
            Excel.Worksheet worksheet;
            Excel.Worksheet worksheet2;

            #region Carton Purchase Monitor
            objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Carton_Purchase_Monitor.xltx"); //預先開啟excel app
            objApp.Visible = false;
            worksheet = objApp.ActiveWorkbook.Worksheets[1];
            worksheet2 = objApp.ActiveWorkbook.Worksheets[2];

            if (this.ds.Tables["Pending"].Rows.Count > 0 || this.ds.Tables["POList"].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.ds.Tables["Pending"], null, "Carton_Purchase_Monitor.xltx", headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[1], showExcel: false, DisplayAlerts_ForSaveFile: true, showSaveMsg: false);

                MyUtility.Excel.CopyToXls(this.ds.Tables["POList"], null, "Carton_Purchase_Monitor.xltx", headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[2], showExcel: false, DisplayAlerts_ForSaveFile: true, showSaveMsg: false);
            }

            
            worksheet.Columns.AutoFit();
            worksheet2.Columns.AutoFit();

            // Save Excel
            pathName = GetName("Carton_Purchase_Monitor");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(pathName);
            workbook.Close();
            #endregion


            #region Outstanding Report
            objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Outstanding_Report.xltx"); //預先開啟excel app
            objApp.Visible = false;
            worksheet = objApp.ActiveWorkbook.Worksheets[1];
            worksheet2 = objApp.ActiveWorkbook.Worksheets[2];

            if (this.ds.Tables["Outstanding"].Rows.Count > 0 || this.ds.Tables["OnTime"].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.ds.Tables["Outstanding"], null, "Outstanding_Report.xltx", headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[1], showExcel: false, DisplayAlerts_ForSaveFile: true, showSaveMsg: false);

                MyUtility.Excel.CopyToXls(this.ds.Tables["OnTime"], null, "Outstanding_Report.xltx", headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[2], showExcel: false, DisplayAlerts_ForSaveFile: true, showSaveMsg: false);
            }

            worksheet.Columns.AutoFit();
            worksheet2.Columns.AutoFit();

            // Save Excel
            pathName2 = GetName("Outstanding_Report");
            Microsoft.Office.Interop.Excel.Workbook workbook2 = objApp.ActiveWorkbook;
            workbook2.SaveAs(pathName2);
            workbook2.Close();

            #endregion

            Ict.Logs.APP.LogInfo("Create XLT Start Report");

            #region Save Excel
            objApp.Quit();
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(objApp);
            #endregion

            files.Add(pathName);
            files.Add(pathName2);

            return files;
        }

        public static class ExcelFileNameExtension
        {
            public const string Xlsm = ".xlsm", Xlsx = ".xlsx";
        }

        public static string GetName(string ProcessName, string NameExtension = ExcelFileNameExtension.Xlsx)
        {
            string fileName = ProcessName.Trim()
                                + DateTime.Now.ToString("_yyMMdd_HHmmssfff")
                                + NameExtension;
            return Path.Combine(Sci.Env.Cfg.ReportTempDir, fileName);
        }

        private string HtmlStyle()
        {
            string style = @"
<style>
    .DataTable {
        width: 92vw;
        font-size: 1rem;
        font-weight: bold;
        border: solid 1px black;
        background-color: white;
    }
        .DataTable > tbody > tr:nth-of-type(odd) {
            background-color: #ffffff;
        }

        .DataTable > tbody > tr:nth-of-type(even) {
            background-color: #F0F2F2;
        }

        .DataTable > tbody > tr > td {
            border: solid 1px gray;
            padding: 1em;
            text-align: left;
            vertical-align: middle;
        }
</style>
";
            return style;
        }

        private string DataTableChangeHtml_WithOutStyleHtml(DataTable dt)
        {
            string html = "<html> ";

            html += @"
<style>
    .tg {border-collapse:collapse;border-spacing:0;}
.tg td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}
.tg th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}
        }
</style>
";

            html += "<body> ";
            html += "<table class='tg'> ";
            html += "<thead><tr bgcolor=\"#FFDEA1\" > ";
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                html += "<th>" + dt.Columns[i].ColumnName + "</th> ";
            }

            html += "</tr></thead> ";
            html += "<tbody> ";
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                html += "<tr> ";
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td> ";
                }

                html += "</tr> ";
            }

            html += "</tbody> ";
            html += "</table> ";
            html += "</body> ";
            html += "</html> ";
            return html;
        }

        private void MailToHtml(string mailFrom, string mailServer, string eMailID, string eMailPwd, int mailPort, string subject, List<Attachment> attachs, string desc, string mailTO, string mailTOCC, bool isBodyHtml)
        {
            //寄件者 & 收件者
            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.From = new MailAddress(mailFrom);
            message.To.Add(mailTO);
            if (!MyUtility.Check.Empty(mailTOCC))
            {
                message.Bcc.Add(mailTOCC);
            }

            if (message.To.Count == 0)
            {
                return;
            }

            message.Body = desc;
            message.IsBodyHtml = isBodyHtml;
            //Gmail Smtp
            SmtpClient client = new SmtpClient(mailServer);
            //寄件者 帳密
            client.Credentials = new NetworkCredential(eMailID, eMailPwd);
            client.Port = mailPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            //夾檔
            if (attachs != null)
            {
                foreach (Attachment attach in attachs)
                {
                    message.Attachments.Add(attach);
                }
            }

            client.Send(message);
        }

        private void btnTestMail_Click(object sender, EventArgs e)
        {
            string sqlcmd = @"select [Subject] = 'Test Test Test PMS_WeeklyPurchasingMonitoring', [Description] = 'PMS_WeeklyPurchasingMonitoring test test test',[ToAddress] = 'willy.wei@sportscity.com.tw', [CcAddress] = 'willy.wei@sportscity.com.tw'";
            DualResult result = DBProxy.Current.Select("Production", sqlcmd, out DataTable _mailTo);

            if (!result)
            {
                if (this.isAuto)
                {
                    throw result.GetException();
                }
                else
                {
                    ShowErr(result);
                    return;
                }
            }

            SendMail(_mailTo.Rows[0], string.Empty, string.Empty);
        }
    }
}
