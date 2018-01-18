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
using System.Runtime.InteropServices;
using System.IO;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P03_Print
    /// </summary>
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        private DataRow masterData;
        private string display;
        private string contentType;
        private DataTable actCycleTime;
        private DataTable operationCode;
        private DataTable noda;
        private DataTable mt;
        private DataTable summt;
        private DataTable nodist;
        private DataTable noppa;
        private DataTable atct;
        private DataTable GCTime;
        private DataTable maxcycle;
        private decimal styleCPU;
        private decimal takt1;
        private decimal takt2;
        private decimal changp;
        private decimal count1;
        private decimal count2;
        private decimal standardOutput1;
        private decimal standardOutput2;
        private decimal dailyDemand1;
        private decimal dailyDemand2;
        private decimal totalCycle1;
        private decimal totalCycle2;
        private decimal totalGSD1;
        private decimal totalGSD2;
        private bool change = false;
        private DataTable summt2;
        private DataTable actCycleTime2;
        private DataTable GCTime2;
        private DataTable atct2;
        private DataTable nodist2;
        private DataTable noda2;
        private DataTable mt2;
        private DataTable maxcycle2;

        /// <summary>
        /// P03_Print
        /// </summary>
        /// <param name="masterData">MasterData</param>
        /// <param name="styleCPU">StyleCPU</param>
        public P03_Print(DataRow masterData, decimal styleCPU)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.styleCPU = styleCPU;
            this.radioU.Checked = true;
            this.radioDescription.Checked = true;
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this.display = this.radioU.Checked ? "U" : "Z";
            this.contentType = this.radioDescription.Checked ? "D" : "A";
            this.changp = MyUtility.Convert.GetDecimal(this.numpage.Value);
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 切分頁計算 takt
            decimal ttlnocount = MyUtility.Convert.GetInt(this.masterData["CurrentOperators"]);
            if (this.chkpage.Checked && ttlnocount > this.changp)
            {
                string sqlp1 = string.Format(
                    @"
select no = count(distinct no)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and no<={1}
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                string sqlp2 = string.Format(
                    @"
select no = count(distinct no)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and no>{1}
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                this.count1 = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp1));
                this.count2 = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp2));
                sqlp1 = string.Format(
                    @"
select sumCycle = sum(Cycle)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)  and no<={1}
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                sqlp2 = string.Format(
                    @"
select sumCycle = sum(Cycle)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and no>{1}
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);

                this.totalCycle1 = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp1)), 0);
                this.totalCycle2 = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp2)), 0);
                decimal currentOperators1 = this.count1;
                decimal currentOperators2 = this.count2;
                this.standardOutput1 = this.totalCycle1 == 0 ? 0 : MyUtility.Math.Round(3600 * currentOperators1 / this.totalCycle1, 0);
                this.standardOutput2 = this.totalCycle2 == 0 ? 0 : MyUtility.Math.Round(3600 * currentOperators2 / this.totalCycle2, 0);
                this.dailyDemand1 = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.masterData["Workhour"]) * this.standardOutput1, 0);
                this.dailyDemand2 = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.masterData["Workhour"]) * this.standardOutput2, 0);
                this.takt1 = this.dailyDemand1 == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.masterData["NetTime"]) / this.dailyDemand1, 0);
                this.takt2 = this.dailyDemand2 == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.masterData["NetTime"]) / this.dailyDemand2, 0);

                sqlp1 = string.Format(
                   @"
select sumTotalGSD = sum(GSD)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)  and no<={1}
",
                   MyUtility.Convert.GetString(this.masterData["ID"]),
                   this.changp);
                sqlp2 = string.Format(
                    @"
select sumTotalGSD = sum(GSD)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and no>{1}
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                this.totalGSD1 = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp1)), 0);
                this.totalGSD2 = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp2)), 0);
                this.change = true;
            }
            else
            {
                this.change = false;
            }
            #endregion

            string sqlCmd;

            #region 第一頁
            sqlCmd = string.Format(
                @"
select a.GroupKey,a.OperationID,a.Annotation,a.GSD,MachineTypeID = iif(m.MachineGroupID = '','',a.MachineTypeID),a.at
,isnull(o.DescEN,'') as DescEN,rn = ROW_NUMBER() over(order by a.GroupKey)
from (select GroupKey,OperationID,Annotation,max(GSD) as GSD,MachineTypeID 
		,AT = case 
	        when isnull(Attachment,'') != '' and isnull(Template,'') != '' then Attachment+','+Template
	        when isnull(Attachment,'') != '' and isnull(Template,'') = '' then Attachment
	        when isnull(Attachment,'') = '' and isnull(Template,'') != '' then Template 
	        else ''end
	  from LineMapping_Detail ld WITH (NOLOCK) 
	  where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
	  group by GroupKey,OperationID,Annotation,MachineTypeID,Attachment,Template
) a
left join Operation o WITH (NOLOCK) on o.ID = a.OperationID
left join MachineType m WITH (NOLOCK) on m.id =  a.MachineTypeID
order by a.GroupKey", MyUtility.Convert.GetString(this.masterData["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.operationCode);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query operation code data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion
            #region Machine Type IsPPa
            sqlCmd = string.Format(
                @"
select ld.OperationID,MachineTypeID = iif(m.MachineGroupID = '','',ld.MachineTypeID),ld.Annotation,DescEN=isnull(o.DescEN,ld.Annotation)
from LineMapping_Detail ld WITH (NOLOCK)
left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
left join Operation o WITH (NOLOCK) on o.ID = ld.OperationID
where ld.ID = {0} and IsPPa = 1 
order by ld.No,ld.GroupKey", MyUtility.Convert.GetString(this.masterData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out this.noppa);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region 第二頁
            if (!this.change)
            {
                sqlCmd = string.Format(
                    @"select No,CT = COUNT(1)
from LineMapping_Detail ld WITH (NOLOCK) 
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
GROUP BY NO
order by no", MyUtility.Convert.GetString(this.masterData["ID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodist);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select * into #tmp from
(
	select a.No,MachineTypeID=iif(m.MachineGroupID = '','',a.MachineTypeID),MachineTypeID2 = ltrim(rtrim(iif(m.MachineGroupID = '','',a.MachineTypeID)+' '+Attachment+' '+Template+' '+ThreadColor))
	from(
		select ld.No,MachineTypeID ,Attachment = isnull(Attachment,''),Template = isnull(Template,''),ThreadColor = isnull(ThreadColor,'')
		from LineMapping_Detail ld WITH (NOLOCK) 
		where ld.ID = {0} and  (IsPPa = 0 or IsPPa is null)
		GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
	)a
	left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
)b
where b.MachineTypeID!=''

select ld.No,ld.Cycle,ld.GSD,MachineTypeID = iif(m.MachineGroupID = '','',ld.MachineTypeID),e2.Name,ld.Annotation,o.DescEN
into #tmp2
from LineMapping_Detail ld WITH (NOLOCK) 
left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
left join Operation o WITH (NOLOCK) on o.ID = ld.OperationID
outer apply(
    select Name = stuff((
        select distinct concat(',',Name)
        from Employee e WITH (NOLOCK) 
        where e.ID in (  
			select distinct EmployeeID
			from LineMapping_Detail ld2 WITH (NOLOCK) 
			where ld2.ID = ld.id and ld2.no = ld.no
		)
        for xml path('')
    ),1,1,'')
)e2
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
order by ld.No,ld.MachineTypeID,ld.attachment,ld.template,ld.GroupKey

select t2.No,t2.Cycle,t2.GSD,t2.Name,t2.Annotation,t2.DescEN,MachineTypeID = t.MachineTypeID2
from #tmp2 t2,#tmp t
where t2.no=t.no and t2.MachineTypeID = t.MachineTypeID
", MyUtility.Convert.GetString(this.masterData["ID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out this.noda);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select MachineTypeID,sumct = sum(ct)
from(
	SELECT No,MachineTypeID,ct=count(1)
	FROM(
		select ld.No,ld.MachineTypeID,ThreadColor=isnull(ThreadColor,''),Attachment=isnull(Attachment,''),Template=isnull(Template,'')
		from LineMapping_Detail ld WITH (NOLOCK) 
		left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
		where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and (m.MachineGroupID != '' or m.MachineGroupID is null)
		GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
	)A
	group by No,MachineTypeID
)x
group by MachineTypeID", MyUtility.Convert.GetString(this.masterData["ID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out this.summt);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select * from(
select a_ct = count(a.no)
from(
	select Attachment=isnull(Attachment,'')
	from LineMapping_Detail ld WITH (NOLOCK) 
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and (isnull(Attachment,'') !='' or isnull(Template,'') !='')
	GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
)x
outer apply(select * from SplitString(Attachment,','))a
)z,(
select t_ct = count(t.no)
from(
	select Template=isnull(Template,'')
	from LineMapping_Detail ld WITH (NOLOCK) 
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and (isnull(Attachment,'') !='' or isnull(Template,'') !='')
	GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
)x
outer apply(select * from SplitString(Template,','))t)z2", MyUtility.Convert.GetString(this.masterData["ID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out this.atct);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                // 圖1用
                sqlCmd = string.Format(
                    @"
select distinct No,ActCycle,{1} as TaktTime
from LineMapping_Detail ld WITH (NOLOCK) 
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
order by No",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    MyUtility.Convert.GetString(this.masterData["TaktTime"]));
                result = DBProxy.Current.Select(null, sqlCmd, out this.actCycleTime);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query cycle time data fail\r\n" + result.ToString());
                    return failResult;
                }

                // 圖2用
                sqlCmd = string.Format(
                    @"
select distinct No,TotalGSD,TotalCycle
from LineMapping_Detail ld WITH (NOLOCK) 
where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
order by No",
                    MyUtility.Convert.GetString(this.masterData["ID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out this.GCTime);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query cycle time data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            #endregion
            #region 第二頁 有分頁
            else
            {
                #region MACHINE INVENTORY
                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and (m.MachineGroupID != '' or m.MachineGroupID is null)
	and no <= {1}
)x
group by ID

select MachineTypeID,sumct = sum(ct)
from(
	SELECT No,MachineTypeID,ct=count(1)
	FROM(
		select ld.No,ld.MachineTypeID,ThreadColor=isnull(ThreadColor,''),Attachment=isnull(Attachment,''),Template=isnull(Template,'')
		from LineMapping_Detail ld WITH (NOLOCK) 
		left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
		inner join #tmp t on ld.ID = t.ID
		where  (IsPPa = 0 or IsPPa is null) and (m.MachineGroupID != '' or m.MachineGroupID is null) and no between t.minno and t.maxno
		GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
	)A
	group by No,MachineTypeID
)x
group by MachineTypeID",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.summt);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and (m.MachineGroupID != '' or m.MachineGroupID is null)
	and no > {1}
)x
group by ID

select MachineTypeID,sumct = sum(ct)
from(
	SELECT No,MachineTypeID,ct=count(1)
	FROM(
		select ld.No,ld.MachineTypeID,ThreadColor=isnull(ThreadColor,''),Attachment=isnull(Attachment,''),Template=isnull(Template,'')
		from LineMapping_Detail ld WITH (NOLOCK) 
		left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
		inner join #tmp t on ld.ID = t.ID
		where  (IsPPa = 0 or IsPPa is null) and (m.MachineGroupID != '' or m.MachineGroupID is null) and no between t.minno and t.maxno
		GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
	)A
	group by No,MachineTypeID
)x
group by MachineTypeID",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.summt2);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
                #endregion

                #region 長條圖資料

                // 圖1用
                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
	and no <= {1}
)x
group by ID

select distinct No,ActCycle
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID
where  (IsPPa = 0 or IsPPa is null)  and no between t.minno and t.maxno
order by No",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.actCycleTime);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query cycle time data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) 
	and no > {1}
)x
group by ID

select distinct No,ActCycle
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID
where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
order by No",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.actCycleTime2);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query cycle time data fail\r\n" + result.ToString());
                    return failResult;
                }

                // 圖2用
                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
	and no <= {1}
)x
group by ID

select distinct No,TotalGSD,TotalCycle
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID
where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
order by No",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.GCTime);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query cycle time data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
	and no > {1}
)x
group by ID

select distinct No,TotalGSD,TotalCycle
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID
where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
order by No",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.GCTime2);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query cycle time data fail\r\n" + result.ToString());
                    return failResult;
                }
                #endregion

                #region MACHINE
                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and (m.MachineGroupID != '' or m.MachineGroupID is null)
	and no <= {1}
)x
group by ID

select*from(
select a_ct = count(a.no)
from(
	select Attachment=isnull(Attachment,''),Template=isnull(Template,'')
	from LineMapping_Detail ld WITH (NOLOCK) 
	inner join #tmp t on ld.ID = t.ID
	where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
	and (isnull(Attachment,'') !='' or isnull(Template,'') !='') 
	GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
)x
outer apply(select * from SplitString(Attachment,','))a
)z,(
select t_ct = count(t.no)
from(
	select Attachment=isnull(Attachment,''),Template=isnull(Template,'')
	from LineMapping_Detail ld WITH (NOLOCK) 
	inner join #tmp t on ld.ID = t.ID
	where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
	and (isnull(Attachment,'') !='' or isnull(Template,'') !='') 
	GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
)x
outer apply(select * from SplitString(Template,','))t)z2
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.atct);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and (m.MachineGroupID != '' or m.MachineGroupID is null)
	and no > {1}
)x
group by ID

select*from(
select a_ct = count(a.no)
from(
	select Attachment=isnull(Attachment,''),Template=isnull(Template,'')
	from LineMapping_Detail ld WITH (NOLOCK) 
	inner join #tmp t on ld.ID = t.ID
	where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
	and (isnull(Attachment,'') !='' or isnull(Template,'') !='') 
	GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
)x
outer apply(select * from SplitString(Attachment,','))a
)z,(
select t_ct = count(t.no)
from(
	select Attachment=isnull(Attachment,''),Template=isnull(Template,'')
	from LineMapping_Detail ld WITH (NOLOCK) 
	inner join #tmp t on ld.ID = t.ID
	where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
	and (isnull(Attachment,'') !='' or isnull(Template,'') !='') 
	GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
)x
outer apply(select * from SplitString(Template,','))t)z2
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.atct2);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
                #endregion

                #region
                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
	and no <= {1}
)x
group by ID

select No,CT = COUNT(1)
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID
where  (IsPPa = 0 or IsPPa is null)  and no between t.minno and t.maxno
GROUP BY NO
order by no

",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodist);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) 
	and no > {1}
)x
group by ID

select No,CT = COUNT(1)
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID
where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
GROUP BY NO
order by no

",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodist2);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
                #endregion

                #region
                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
	and no <= {1}
)x
group by ID

select * into #tmpmt from
(
    select a.No,MachineTypeID=iif(m.MachineGroupID = '','',a.MachineTypeID),MachineTypeID2 = ltrim(rtrim(iif(m.MachineGroupID = '','',a.MachineTypeID)+' '+Attachment+' '+Template+' '+ThreadColor))
    from(
	    select ld.No,MachineTypeID ,Attachment = isnull(Attachment,''),Template = isnull(Template,''),ThreadColor = isnull(ThreadColor,'')
	    from LineMapping_Detail ld WITH (NOLOCK) 
	    inner join #tmp t on ld.ID = t.ID
	    where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
	    GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
    )a
    left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
)b
where b.MachineTypeID!=''
order by no

select ld.No,ld.Cycle,ld.GSD,MachineTypeID = iif(m.MachineGroupID = '','',ld.MachineTypeID),e2.Name,ld.Annotation,o.DescEN
into #tmpmt2
from LineMapping_Detail ld WITH (NOLOCK) 
left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
inner join #tmp t on ld.ID = t.ID
left join Operation o WITH (NOLOCK) on o.ID = ld.OperationID
outer apply(
    select Name = stuff((
        select distinct concat(',',Name)
        from Employee e WITH (NOLOCK) 
        where e.ID in (  
			select distinct EmployeeID
			from LineMapping_Detail ld2 WITH (NOLOCK) 
			where ld2.ID = ld.id and ld2.no = ld.no
		)
        for xml path('')
    ),1,1,'')
)e2
where (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
order by ld.No,ld.MachineTypeID,ld.attachment,ld.template,ld.GroupKey

select t2.No,t2.Cycle,t2.GSD,t2.Name,t2.Annotation,t2.DescEN,MachineTypeID = t.MachineTypeID2
from #tmpmt2 t2,#tmpmt t
where t2.no=t.no and t2.MachineTypeID = t.MachineTypeID
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.noda);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
	and no > {1}
)x
group by ID

select * into #tmpmt from
(
    select a.No,MachineTypeID=iif(m.MachineGroupID = '','',a.MachineTypeID),MachineTypeID2 = ltrim(rtrim(iif(m.MachineGroupID = '','',a.MachineTypeID)+' '+Attachment+' '+Template+' '+ThreadColor))
    from(
	    select ld.No,MachineTypeID ,Attachment = isnull(Attachment,''),Template = isnull(Template,''),ThreadColor = isnull(ThreadColor,'')
	    from LineMapping_Detail ld WITH (NOLOCK) 
	    inner join #tmp t on ld.ID = t.ID
	    where  (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
	    GROUP BY ld.No,ld.MachineTypeID,isnull(ThreadColor,''),isnull(Attachment,''),isnull(Template,'')
    )a
    left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
)b
where b.MachineTypeID!=''
order by no

select ld.No,ld.Cycle,ld.GSD,MachineTypeID = iif(m.MachineGroupID = '','',ld.MachineTypeID),e2.Name,ld.Annotation,o.DescEN
into #tmpmt2
from LineMapping_Detail ld WITH (NOLOCK) 
left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
inner join #tmp t on ld.ID = t.ID
left join Operation o WITH (NOLOCK) on o.ID = ld.OperationID
outer apply(
    select Name = stuff((
        select distinct concat(',',Name)
        from Employee e WITH (NOLOCK) 
        where e.ID in (  
			select distinct EmployeeID
			from LineMapping_Detail ld2 WITH (NOLOCK) 
			where ld2.ID = ld.id and ld2.no = ld.no
		)
        for xml path('')
    ),1,1,'')
)e2
where (IsPPa = 0 or IsPPa is null) and no between t.minno and t.maxno
order by ld.No,ld.MachineTypeID,ld.attachment,ld.template,ld.GroupKey

select t2.No,t2.Cycle,t2.GSD,t2.Name,t2.Annotation,t2.DescEN,MachineTypeID = t.MachineTypeID2
from #tmpmt2 t2,#tmpmt t
where t2.no=t.no and t2.MachineTypeID = t.MachineTypeID
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.noda2);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
                #endregion

                #region maxHighCycle
                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null)
	and no <= {1}
)x
group by ID

select maxHighCycle = max(TotalCycle)
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID
where  (IsPPa = 0 or IsPPa is null)  and no between t.minno and t.maxno",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.maxcycle);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = string.Format(
                    @"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
	where ld.ID = {0} and (IsPPa = 0 or IsPPa is null) and (m.MachineGroupID != '' or m.MachineGroupID is null)
	and no > {1}
)x
group by ID

select maxHighCycle = max(TotalCycle)
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID
where  (IsPPa = 0 or IsPPa is null)  and no between t.minno and t.maxno",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    this.changp);
                result = DBProxy.Current.Select(null, sqlCmd, out this.maxcycle2);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
                #endregion
            }
            #endregion

            return Result.True;
        }

        /// <summary>
        /// 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.operationCode.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // string strXltName = Sci.Env.Cfg.XltPathDir + (this.display == "U" ? "\\IE_P03_Print_U.xltx" : "\\IE_P03_Print_Z.xltx");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_P03_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            #region 第一頁
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            string factory = MyUtility.Convert.GetString(this.masterData["FactoryID"]);
            worksheet.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format("select NameEN from factory WITH (NOLOCK) where id = '{0}'", factory));
            string style = MyUtility.Convert.GetString(this.masterData["styleID"]);
            string brand = MyUtility.Convert.GetString(this.masterData["brandID"]);
            string season = MyUtility.Convert.GetString(this.masterData["SeasonID"]);
            string combotype = MyUtility.Convert.GetString(this.masterData["combotype"]);
            worksheet.Cells[2, 2] = style + " " + season + " " + brand + " " + combotype;

            // 填Operation
            int intRowsStart = 6;
            object[,] objArray = new object[1, 5];
            foreach (DataRow dr in this.operationCode.Rows)
            {
                objArray[0, 0] = dr["rn"];
                objArray[0, 1] = this.contentType == "A" ? MyUtility.Convert.GetString(dr["Annotation"]).Trim() : MyUtility.Convert.GetString(dr["DescEN"]).Trim();
                objArray[0, 2] = dr["MachineTypeID"];
                objArray[0, 3] = dr["AT"];
                objArray[0, 4] = dr["GSD"];
                worksheet.Range[string.Format("A{0}:E{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            worksheet.Cells[intRowsStart, 1] = string.Format("=MAX($A$2:A{0})+1", intRowsStart - 1);
            worksheet.Cells[intRowsStart, 5] = string.Format("=SUM(E6:E{0})", intRowsStart - 1);
            worksheet.Range[string.Format("A5:E{0}", intRowsStart)].Borders.Weight = 1; // 1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[string.Format("A5:E{0}", intRowsStart)].Borders.LineStyle = 1;

            intRowsStart++;
            worksheet.Cells[intRowsStart, 5] = string.Format("=E{0}/{1}", intRowsStart - 1, MyUtility.Convert.GetInt(this.masterData["CurrentOperators"]));
            worksheet.get_Range("E" + intRowsStart, "E" + intRowsStart).Font.Bold = true;

            intRowsStart++;
            worksheet.Cells[intRowsStart, 2] = "Picture";
            worksheet.get_Range("B" + intRowsStart, "B" + intRowsStart).Font.Bold = true;

            // 插圖 Picture1
            intRowsStart++;
            string destination_path = MyUtility.GetValue.Lookup("select PicPath from System WITH (NOLOCK) ", null);
            string picture12 = string.Format("select Picture1, Picture2 from Style where id = '{0}' and BrandID = '{1}' and SeasonID = '{2}'", style, brand, season);
            DataRow pdr;
            MyUtility.Check.Seek(picture12, out pdr);
            string filepath;
            Image img = null;
            double xltPixelRate;
            dynamic left;
            Microsoft.Office.Interop.Excel.Range cell = worksheet.Cells[intRowsStart, 2];
            left = cell.Left;
            if (!MyUtility.Check.Empty(pdr["Picture1"]))
            {
                filepath = destination_path + MyUtility.Convert.GetString(pdr["Picture1"]);
                if (File.Exists(filepath))
                {
                    img = Image.FromFile(filepath);
                    xltPixelRate = img.Width / 180 > 1 ? img.Width / 180 : 1;
                    worksheet.Shapes.AddPicture(filepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left, cell.Top, (float)(img.Width / xltPixelRate), (float)(img.Height / xltPixelRate));
                    left += 220;
                }
            }

            // Picture2
            if (!MyUtility.Check.Empty(pdr["Picture2"]))
            {
                filepath = destination_path + MyUtility.Convert.GetString(pdr["Picture2"]);
                if (File.Exists(filepath))
                {
                    img = Image.FromFile(filepath);
                    xltPixelRate = img.Width / 180 > 1 ? img.Width / 180 : 1;
                    worksheet.Shapes.AddPicture(filepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left, cell.Top, (float)(img.Width / xltPixelRate), (float)(img.Height / xltPixelRate));
                }
            }
            #endregion

            if (this.change)
            {
                #region 長條圖資料

                // 填act Cycle Time 1
                worksheet = excel.ActiveWorkbook.Worksheets[3];
                intRowsStart = 2;
                objArray = new object[1, 3];
                foreach (DataRow dr in this.actCycleTime.Rows)
                {
                    objArray[0, 0] = dr["No"];
                    objArray[0, 1] = dr["ActCycle"];
                    objArray[0, 2] = this.takt1;
                    worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }

                // 填act Cycle Time 2
                worksheet = excel.ActiveWorkbook.Worksheets[6];
                int intRowsStart2 = 2;
                objArray = new object[1, 3];
                foreach (DataRow dr in this.actCycleTime2.Rows)
                {
                    objArray[0, 0] = dr["No"];
                    objArray[0, 1] = dr["ActCycle"];
                    objArray[0, 2] = this.takt2;
                    worksheet.Range[string.Format("A{0}:C{0}", intRowsStart2)].Value2 = objArray;
                    intRowsStart2++;
                }

                // 填 TotalGSD 1
                worksheet = excel.ActiveWorkbook.Worksheets[4];
                intRowsStart = 2;
                objArray = new object[1, 3];
                foreach (DataRow dr in this.GCTime.Rows)
                {
                    objArray[0, 0] = dr["No"];
                    objArray[0, 1] = dr["TotalGSD"];
                    objArray[0, 2] = dr["TotalCycle"];
                    worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }

                // 填 TotalGSD 2
                worksheet = excel.ActiveWorkbook.Worksheets[7];
                intRowsStart2 = 2;
                objArray = new object[1, 3];
                foreach (DataRow dr in this.GCTime2.Rows)
                {
                    objArray[0, 0] = dr["No"];
                    objArray[0, 1] = dr["TotalGSD"];
                    objArray[0, 2] = dr["TotalCycle"];
                    worksheet.Range[string.Format("A{0}:C{0}", intRowsStart2)].Value2 = objArray;
                    intRowsStart2++;
                }
                #endregion

                #region 固定資料

                // 左上表頭資料
                worksheet = excel.ActiveWorkbook.Worksheets[2];
                worksheet.Cells[1, 5] = factory;
                worksheet.Cells[5, 5] = MyUtility.Convert.GetString(this.masterData["SewingLineID"]);
                worksheet.Cells[7, 5] = style;
                worksheet.Cells[9, 5] = this.styleCPU;

                // 右下簽名位置
                worksheet.Cells[28, 15] = DateTime.Now.ToString("d");
                worksheet.Cells[31, 15] = Sci.Env.User.UserName;

                // 左下表頭資料
                worksheet.Cells[56, 4] = this.masterData["Version"];
                worksheet.Cells[58, 4] = this.masterData["Workhour"];
                worksheet.Cells[60, 4] = this.count1;
                worksheet.Cells[62, 4] = this.standardOutput1;
                worksheet.Cells[64, 4] = this.dailyDemand1;
                worksheet.Cells[66, 4] = this.takt1;
                worksheet.Cells[68, 4] = MyUtility.Check.Empty(this.maxcycle.Rows[0]["maxHighCycle"]) ? 0 : MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.maxcycle.Rows[0]["maxHighCycle"]), 2);
                worksheet.Cells[70, 4] = this.totalCycle1;
                worksheet.Cells[72, 4] = this.totalGSD1;
                worksheet.Cells[74, 4] = MyUtility.Check.Empty(this.totalGSD1) ? 0 : (this.totalGSD1 - this.totalCycle1) / this.totalGSD1;
                worksheet.Cells[76, 4] = MyUtility.Check.Empty(this.maxcycle.Rows[0]["maxHighCycle"]) || MyUtility.Check.Empty(this.count1) ? 0 : this.totalCycle1 / MyUtility.Convert.GetDecimal(this.maxcycle.Rows[0]["maxHighCycle"]) / this.count1;
                worksheet.Cells[78, 4] = MyUtility.Check.Empty(this.takt1) || MyUtility.Check.Empty(this.count1) ? 0 : this.totalCycle1 / this.takt1 / this.count1;
                worksheet.Cells[80, 4] = MyUtility.Check.Empty(this.maxcycle.Rows[0]["maxHighCycle"]) || MyUtility.Check.Empty(this.count1) ? 0 : this.totalGSD1 / MyUtility.Convert.GetDecimal(this.maxcycle.Rows[0]["maxHighCycle"]) / this.count1;
                worksheet.Cells[82, 4] = MyUtility.Check.Empty(this.maxcycle.Rows[0]["maxHighCycle"]) || MyUtility.Check.Empty(this.count1) ? 0 : MyUtility.Math.Round(3600m / MyUtility.Convert.GetDecimal(this.maxcycle.Rows[0]["maxHighCycle"]), 2) * this.styleCPU / this.count1;

                // 左上表頭資料
                worksheet = excel.ActiveWorkbook.Worksheets[5];
                worksheet.Cells[1, 5] = factory;
                worksheet.Cells[5, 5] = MyUtility.Convert.GetString(this.masterData["SewingLineID"]);
                worksheet.Cells[7, 5] = style;
                worksheet.Cells[9, 5] = this.styleCPU;

                // 右下簽名位置
                worksheet.Cells[28, 15] = DateTime.Now.ToString("d");
                worksheet.Cells[31, 15] = Sci.Env.User.UserName;

                // 左下表頭資料
                worksheet.Cells[56, 4] = this.masterData["Version"];
                worksheet.Cells[58, 4] = this.masterData["Workhour"];
                worksheet.Cells[60, 4] = this.count2;
                worksheet.Cells[62, 4] = this.standardOutput2;
                worksheet.Cells[64, 4] = this.dailyDemand2;
                worksheet.Cells[66, 4] = this.takt2;
                worksheet.Cells[68, 4] = MyUtility.Check.Empty(this.maxcycle2.Rows[0]["maxHighCycle"]) ? 0 : MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.maxcycle2.Rows[0]["maxHighCycle"]), 2);
                worksheet.Cells[70, 4] = this.totalCycle2;
                worksheet.Cells[72, 4] = this.totalGSD2;
                worksheet.Cells[74, 4] = MyUtility.Check.Empty(this.totalGSD2) ? 0 : (this.totalGSD2 - this.totalCycle2) / this.totalGSD2;
                worksheet.Cells[76, 4] = MyUtility.Check.Empty(this.maxcycle2.Rows[0]["maxHighCycle"]) || MyUtility.Check.Empty(this.count2) ? 0 : this.totalCycle2 / MyUtility.Convert.GetDecimal(this.maxcycle2.Rows[0]["maxHighCycle"]) / this.count2;
                worksheet.Cells[78, 4] = MyUtility.Check.Empty(this.takt2) || MyUtility.Check.Empty(this.count2) ? 0 : this.totalCycle2 / this.takt2 / this.count2;
                worksheet.Cells[80, 4] = MyUtility.Check.Empty(this.maxcycle2.Rows[0]["maxHighCycle"]) || MyUtility.Check.Empty(this.count2) ? 0 : this.totalGSD2 / MyUtility.Convert.GetDecimal(this.maxcycle2.Rows[0]["maxHighCycle"]) / this.count2;
                worksheet.Cells[82, 4] = MyUtility.Check.Empty(this.maxcycle2.Rows[0]["maxHighCycle"]) || MyUtility.Check.Empty(this.count2) ? 0 : MyUtility.Math.Round(3600m / MyUtility.Convert.GetDecimal(this.maxcycle2.Rows[0]["maxHighCycle"]), 2) * this.styleCPU / this.count2;
                #endregion

                #region MACHINE INVENTORY
                worksheet = excel.ActiveWorkbook.Worksheets[2];
                if (this.summt.Rows.Count > 3)
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A52:A52").EntireRow; // 選取要被複製的資料
                    for (int i = 3; i < this.summt.Rows.Count; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A52", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rng.Copy(Type.Missing)); // 貼上
                    }
                }

                int sumrow = 0;
                foreach (DataRow item in this.summt.Rows)
                {
                    worksheet.Cells[51 + sumrow, 1] = item["MachineTypeID"];
                    worksheet.Cells[51 + sumrow, 2] = item["sumct"];
                    sumrow++;
                }

                worksheet = excel.ActiveWorkbook.Worksheets[5];
                if (this.summt2.Rows.Count > 3)
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A52:A52").EntireRow; // 選取要被複製的資料
                    for (int i = 3; i < this.summt2.Rows.Count; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A52", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rng.Copy(Type.Missing)); // 貼上
                    }
                }

               sumrow = 0;
                foreach (DataRow item in this.summt2.Rows)
                {
                    worksheet.Cells[51 + sumrow, 1] = item["MachineTypeID"];
                    worksheet.Cells[51 + sumrow, 2] = item["sumct"];
                    sumrow++;
                }
                #endregion

                #region 新增長條圖 2

                // 新增長條圖
                Microsoft.Office.Interop.Excel.Worksheet chartData2 = excel.ActiveWorkbook.Worksheets[4];
                worksheet = excel.ActiveWorkbook.Worksheets[2];
                Microsoft.Office.Interop.Excel.Range chartRange2;
                object misValue2 = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.ChartObjects xlsCharts2 = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
                Microsoft.Office.Interop.Excel.ChartObject myChart2 = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts2.Add(378, 1082, 1000, 350);
                Microsoft.Office.Interop.Excel.Chart chartPage2 = myChart2.Chart;
                chartRange2 = chartData2.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                chartPage2.SetSourceData(chartRange2, misValue2);

                chartPage2.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

                // 新增折線圖
                Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection2 = chartPage2.SeriesCollection();
                Microsoft.Office.Interop.Excel.Series series2 = seriesCollection2.NewSeries();
                series2.Values = chartData2.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                series2.XValues = chartData2.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                series2.Name = "TotalCycle Time";

                // 折線圖的資料標籤不顯示
                series2.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

                // 隱藏Sheet
                chartData2.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;

                // 新增長條圖
                chartData2 = excel.ActiveWorkbook.Worksheets[7];
                worksheet = excel.ActiveWorkbook.Worksheets[5];
                misValue2 = System.Reflection.Missing.Value;
                xlsCharts2 = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
                myChart2 = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts2.Add(378, 1082, 1000, 350);
                chartPage2 = myChart2.Chart;
                chartRange2 = chartData2.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(intRowsStart2 - 1)));
                chartPage2.SetSourceData(chartRange2, misValue2);

                chartPage2.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

                // 新增折線圖
                seriesCollection2 = chartPage2.SeriesCollection();
                series2 = seriesCollection2.NewSeries();
                series2.Values = chartData2.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(intRowsStart2 - 1)));
                series2.XValues = chartData2.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(intRowsStart2 - 1)));
                series2.Name = "TotalCycle Time";

                // 折線圖的資料標籤不顯示
                series2.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

                // 隱藏Sheet
                chartData2.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                #endregion

                #region 新增長條圖 1

                // 新增長條圖
                Microsoft.Office.Interop.Excel.Worksheet chartData = excel.ActiveWorkbook.Worksheets[3];
                worksheet = excel.ActiveWorkbook.Worksheets[2];
                Microsoft.Office.Interop.Excel.Range chartRange;
                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.ChartObjects xlsCharts = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
                Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts.Add(378, 718.5, 1000, 350);
                Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
                chartRange = chartData.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                chartPage.SetSourceData(chartRange, misValue);

                chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

                // 新增折線圖
                Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection = chartPage.SeriesCollection();
                Microsoft.Office.Interop.Excel.Series series1 = seriesCollection.NewSeries();
                series1.Values = chartData.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                series1.XValues = chartData.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                series1.Name = "Takt time";
                series1.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

                // 更改圖表版面配置 && 填入圖表標題 & 座標軸標題
                chartPage.ApplyLayout(9);
                chartPage.ChartTitle.Select();
                chartPage.ChartTitle.Text = "Line Balancing Graph";
                Microsoft.Office.Interop.Excel.Axis z = (Microsoft.Office.Interop.Excel.Axis)chartPage.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                z.AxisTitle.Text = "Act Cycle Time (in secs)";
                z = (Microsoft.Office.Interop.Excel.Axis)chartPage.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                z.AxisTitle.Text = "Operator No.";

                // 新增資料標籤
                // chartPage.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowValue, false, true);

                // 折線圖的資料標籤不顯示
                series1.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

                // 隱藏Sheet
                chartData.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;

                // 新增長條圖
                chartData = excel.ActiveWorkbook.Worksheets[6];
                worksheet = excel.ActiveWorkbook.Worksheets[5];
                misValue = System.Reflection.Missing.Value;
                xlsCharts = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
                myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts.Add(378, 718.5, 1000, 350);
                chartPage = myChart.Chart;
                chartRange = chartData.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(intRowsStart2 - 1)));
                chartPage.SetSourceData(chartRange, misValue);

                chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

                // 新增折線圖
                seriesCollection = chartPage.SeriesCollection();
                series1 = seriesCollection.NewSeries();
                series1.Values = chartData.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(intRowsStart2 - 1)));
                series1.XValues = chartData.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(intRowsStart2 - 1)));
                series1.Name = "Takt time";
                series1.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

                // 更改圖表版面配置 && 填入圖表標題 & 座標軸標題
                chartPage.ApplyLayout(9);
                chartPage.ChartTitle.Select();
                chartPage.ChartTitle.Text = "Line Balancing Graph";
                z = (Microsoft.Office.Interop.Excel.Axis)chartPage.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                z.AxisTitle.Text = "Act Cycle Time (in secs)";
                z = (Microsoft.Office.Interop.Excel.Axis)chartPage.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                z.AxisTitle.Text = "Operator No.";

                // 新增資料標籤
                // chartPage.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowValue, false, true);

                // 折線圖的資料標籤不顯示
                series1.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

                // 隱藏Sheet
                chartData.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                #endregion

                #region MACHINE
                worksheet = excel.ActiveWorkbook.Worksheets[2];
                decimal allct = Math.Ceiling((decimal)this.summt.Rows.Count / 3);
                Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A42:A42").EntireRow; // 選取要被複製的資料
                for (int i = 0; i < allct - 5; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A42", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                }

                int surow = 0;
                int sucol = 0;
                foreach (DataRow item in this.summt.Rows)
                {
                    worksheet.Cells[38 + surow, 4 + sucol] = item["MachineTypeID"];
                    worksheet.Cells[38 + surow, 5 + sucol] = item["sumct"];
                    surow++;
                    if (allct == surow)
                    {
                        surow = 0;
                        sucol += 2;
                    }
                }

                worksheet.Cells[38, 3] = this.atct.Rows[0]["a_ct"];
                worksheet.Cells[40, 3] = this.atct.Rows[0]["t_ct"];

                worksheet = excel.ActiveWorkbook.Worksheets[5];
                allct = Math.Ceiling((decimal)this.summt2.Rows.Count / 3);
                rngToCopy = worksheet.get_Range("A42:A42").EntireRow; // 選取要被複製的資料
                for (int i = 0; i < allct - 5; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A42", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                }

                surow = 0;
                sucol = 0;
                foreach (DataRow item in this.summt2.Rows)
                {
                    worksheet.Cells[38 + surow, 4 + sucol] = item["MachineTypeID"];
                    worksheet.Cells[38 + surow, 5 + sucol] = item["sumct"];
                    surow++;
                    if (allct == surow)
                    {
                        surow = 0;
                        sucol += 2;
                    }
                }

                worksheet.Cells[38, 3] = this.atct2.Rows[0]["a_ct"];
                worksheet.Cells[40, 3] = this.atct2.Rows[0]["t_ct"];
                #endregion

                #region Machine Type	
                worksheet = excel.ActiveWorkbook.Worksheets[2];
                if (this.noppa.Rows.Count > 10)
                {
                    rngToCopy = worksheet.get_Range("A25:A25").EntireRow; // 選取要被複製的資料
                    for (int i = 10; i < this.noppa.Rows.Count; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A25", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                    }
                }

                int idxppa = 0;
                foreach (DataRow item in this.noppa.Rows)
                {
                    worksheet.Cells[25 + idxppa, 1] = item["OperationID"];
                    worksheet.Cells[25 + idxppa, 4] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                    worksheet.Cells[25 + idxppa, 9] = item["MachineTypeID"];
                    idxppa++;
                }

                worksheet = excel.ActiveWorkbook.Worksheets[5];
                if (this.noppa.Rows.Count > 10)
                {
                    rngToCopy = worksheet.get_Range("A25:A25").EntireRow; // 選取要被複製的資料
                    for (int i = 10; i < this.noppa.Rows.Count; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A25", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                    }
                }

                idxppa = 0;
                foreach (DataRow item in this.noppa.Rows)
                {
                    worksheet.Cells[25 + idxppa, 1] = item["OperationID"];
                    worksheet.Cells[25 + idxppa, 4] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                    worksheet.Cells[25 + idxppa, 9] = item["MachineTypeID"];
                    idxppa++;
                }
                #endregion

                #region 預設站數為2站，當超過2站就要新增
                worksheet = excel.ActiveWorkbook.Worksheets[2];
                decimal no_count = MyUtility.Convert.GetDecimal(this.nodist.Rows.Count);
                int j = 2; // 資料組數，預設為1
                if (no_count > 2)
                {
                    rngToCopy = worksheet.get_Range("A17:A21").EntireRow; // 選取要被複製的資料
                    for (j = 2; j <= MyUtility.Convert.GetInt(Math.Ceiling(no_count / 2)); j++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A17", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                    }
                }

                worksheet = excel.ActiveWorkbook.Worksheets[5];
                no_count = MyUtility.Convert.GetDecimal(this.nodist2.Rows.Count);
                int j2 = 2; // 資料組數，預設為1
                if (no_count > 2)
                {
                    rngToCopy = worksheet.get_Range("A17:A21").EntireRow; // 選取要被複製的資料
                    for (j2 = 2; j2 <= MyUtility.Convert.GetInt(Math.Ceiling(no_count / 2)); j2++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A17", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                    }
                }
                #endregion

                #region U字型列印
                if (this.display == "U")
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[2];
                    int norow = 17 + ((j - 2) * 5); // No格子上的位置Excel Y軸
                    int nocolumn = 9;
                    int maxct = 3;
                    int di = this.nodist.Rows.Count;
                    int addct = 0;
                    bool flag = true;
                    decimal dd = Math.Ceiling((decimal)di / 2);
                    List<int> max_ct = new List<int>();
                    for (int i = 0; i < dd; i++)
                    {
                        int a = MyUtility.Convert.GetInt(this.nodist.Rows[i]["ct"]);
                        int d = 0;
                        if (di % 2 == 1 && flag)
                        {
                            flag = false;
                        }
                        else
                        {
                            if (di % 2 == 1)
                            {
                                d = MyUtility.Convert.GetInt(this.nodist.Rows[di - i]["ct"]);
                            }
                            else
                            {
                                d = MyUtility.Convert.GetInt(this.nodist.Rows[di - 1 - i]["ct"]);
                            }
                        }

                        maxct = a > d ? a : d;
                        maxct = maxct > 3 ? maxct : 3;
                        max_ct.Add(maxct);
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                        for (int k = 3; k < maxct; k++)
                        {
                            rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                            worksheet.get_Range(string.Format("D{0}:H{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("I{0}:J{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("L{0}:M{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("N{0}:P{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            if (i > 0)
                            {
                                addct++;
                            }
                        }

                        norow = norow - 5;
                        maxct = 3;
                    }

                    bool leftDirection = true;
                    norow = 17 + ((j - 2) * 5) + addct;
                    int m = 0;
                    foreach (DataRow nodr in this.nodist.Rows)
                    {
                        if (leftDirection)
                        {
                            nocolumn = 9;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn - 7] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn - 6] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn - 5] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn - 4] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            m++;
                            if (m == dd)
                            {
                                leftDirection = false;
                                m--;
                                continue;
                            }

                            norow = norow - 5 - (max_ct[m] - 3);
                        }
                        else
                        {
                            nocolumn = 12;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn + 6] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn + 5] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn + 2] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn + 3] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            norow = norow + 5 + (max_ct[m] - 3);
                            m--;
                        }
                    }

                    worksheet = excel.ActiveWorkbook.Worksheets[5];
                    norow = 17 + ((j2 - 2) * 5); // No格子上的位置Excel Y軸
                    nocolumn = 9;
                    maxct = 3;
                    di = this.nodist2.Rows.Count;
                    addct = 0;
                     flag = true;
                     dd = Math.Ceiling((decimal)di / 2);
                    max_ct.Clear();
                    for (int i = 0; i < dd; i++)
                    {
                        int a = MyUtility.Convert.GetInt(this.nodist2.Rows[i]["ct"]);
                        int d = 0;
                        if (di % 2 == 1 && flag)
                        {
                            flag = false;
                        }
                        else
                        {
                            if (di % 2 == 1)
                            {
                                d = MyUtility.Convert.GetInt(this.nodist2.Rows[di - i]["ct"]);
                            }
                            else
                            {
                                d = MyUtility.Convert.GetInt(this.nodist2.Rows[di - 1 - i]["ct"]);
                            }
                        }

                        maxct = a > d ? a : d;
                        maxct = maxct > 3 ? maxct : 3;
                        max_ct.Add(maxct);
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                        for (int k = 3; k < maxct; k++)
                        {
                            rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                            worksheet.get_Range(string.Format("D{0}:H{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("I{0}:J{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("L{0}:M{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("N{0}:P{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            if (i > 0)
                            {
                                addct++;
                            }
                        }

                        norow = norow - 5;
                        maxct = 3;
                    }

                    leftDirection = true;
                    norow = 17 + ((j2 - 2) * 5) + addct;
                    m = 0;
                    foreach (DataRow nodr in this.nodist2.Rows)
                    {
                        if (leftDirection)
                        {
                            nocolumn = 9;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda2.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn - 7] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn - 6] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn - 5] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn - 4] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            m++;
                            if (m == dd)
                            {
                                leftDirection = false;
                                m--;
                                continue;
                            }

                            norow = norow - 5 - (max_ct[m] - 3);
                        }
                        else
                        {
                            nocolumn = 12;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda2.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn + 6] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn + 5] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn + 2] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn + 3] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            norow = norow + 5 + (max_ct[m] - 3);
                            m--;
                        }
                    }
                }
                #endregion
                #region Z字型列印
                else
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[2];
                    int norow = 17 + ((j - 2) * 5); // No格子上的位置Excel Y軸
                    int nocolumn = 9;
                    int maxct = 3;
                    int ct = 0;
                    int addct = 0;
                    int indx = 1;
                    for (int l = 0; l < this.nodist.Rows.Count+1; l++)
                    {
                        if (l < this.nodist.Rows.Count)
                        {
                            maxct = MyUtility.Convert.GetInt(this.nodist.Rows[l]["ct"]) > maxct ? MyUtility.Convert.GetInt(this.nodist.Rows[l]["ct"]) : maxct;
                        }

                        ct++;
                        if (ct == 2)
                        {
                            Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                            for (int i = 3; i < maxct; i++)
                            {
                                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                                worksheet.get_Range(string.Format("D{0}:H{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("I{0}:J{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("L{0}:M{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("N{0}:P{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                if (indx > 2)
                                {
                                    addct++;
                                }
                            }

                            norow = norow - 5;
                            ct = 0;
                            maxct = 3;
                        }

                        if (l < this.nodist.Rows.Count)
                        {
                            indx++;
                        }
                    }

                    norow = 17 + ((j - 2) * 5) + addct;
                    int leftright_count = 2;
                    bool leftDirection = true;
                    indx = 2;
                    foreach (DataRow nodr in this.nodist.Rows)
                    {
                        if (leftDirection)
                        {
                            nocolumn = 9;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn - 7] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn - 6] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn - 5] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn - 4] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            leftright_count++;
                            if (leftright_count > 2)
                            {
                                leftright_count = 1;
                                leftDirection = false;
                            }
                        }
                        else
                        {
                            nocolumn = 12;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn + 6] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn + 5] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn + 2] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn + 3] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            leftright_count++;
                            if (leftright_count > 2)
                            {
                                leftright_count = 1;
                                leftDirection = true;
                            }
                        }

                        if (this.nodist.Rows.Count > indx)
                        {
                            maxct = MyUtility.Convert.GetInt(this.nodist.Rows[indx]["ct"]) > maxct ? MyUtility.Convert.GetInt(this.nodist.Rows[indx]["ct"]) : maxct;
                        }

                        if (leftright_count == 2)
                        {
                            norow = norow - 5 - (maxct - 3);
                            maxct = 3;
                        }

                        indx++;
                    }

                    worksheet = excel.ActiveWorkbook.Worksheets[5];
                    norow = 17 + ((j2 - 2) * 5); // No格子上的位置Excel Y軸
                    nocolumn = 9;
                    maxct = 3;
                    ct = 0;
                    addct = 0;
                    indx = 1;
                    for (int l = 0; l < this.nodist2.Rows.Count+1; l++)
                    {
                        if (l < this.nodist2.Rows.Count)
                        {
                            maxct = MyUtility.Convert.GetInt(this.nodist2.Rows[l]["ct"]) > maxct ? MyUtility.Convert.GetInt(this.nodist2.Rows[l]["ct"]) : maxct;
                        }

                        ct++;
                        if (ct == 2)
                        {
                            Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                            for (int i = 3; i < maxct; i++)
                            {
                                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                                worksheet.get_Range(string.Format("D{0}:H{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("I{0}:J{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("L{0}:M{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("N{0}:P{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                if (indx > 2)
                                {
                                    addct++;
                                }
                            }

                            norow = norow - 5;
                            ct = 0;
                            maxct = 3;
                        }

                        if (l < this.nodist2.Rows.Count)
                        {
                            indx++;
                        }
                    }

                    norow = 17 + ((j2 - 2) * 5) + addct;
                     leftright_count = 2;
                     leftDirection = true;
                    indx = 2;
                    foreach (DataRow nodr in this.nodist2.Rows)
                    {
                        if (leftDirection)
                        {
                            nocolumn = 9;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda2.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn - 7] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn - 6] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn - 5] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn - 4] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            leftright_count++;
                            if (leftright_count > 2)
                            {
                                leftright_count = 1;
                                leftDirection = false;
                            }
                        }
                        else
                        {
                            nocolumn = 12;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda2.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn + 6] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn + 5] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn + 2] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn + 3] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            leftright_count++;
                            if (leftright_count > 2)
                            {
                                leftright_count = 1;
                                leftDirection = true;
                            }
                        }

                        if (this.nodist2.Rows.Count > indx)
                        {
                            maxct = MyUtility.Convert.GetInt(this.nodist2.Rows[indx]["ct"]) > maxct ? MyUtility.Convert.GetInt(this.nodist2.Rows[indx]["ct"]) : maxct;
                        }

                        if (leftright_count == 2)
                        {
                            norow = norow - 5 - (maxct - 3);
                            maxct = 3;
                        }

                        indx++;
                    }
                }
                #endregion
            }
            else
            {
                #region 長條圖資料

                // 填act Cycle Time
                worksheet = excel.ActiveWorkbook.Worksheets[3];
                intRowsStart = 2;
                objArray = new object[1, 3];
                foreach (DataRow dr in this.actCycleTime.Rows)
                {
                    objArray[0, 0] = dr["No"];
                    objArray[0, 1] = dr["ActCycle"];
                    objArray[0, 2] = dr["TaktTime"];
                    worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }

                // 填 TotalGSD
                worksheet = excel.ActiveWorkbook.Worksheets[4];
                intRowsStart = 2;
                objArray = new object[1, 3];
                foreach (DataRow dr in this.GCTime.Rows)
                {
                    objArray[0, 0] = dr["No"];
                    objArray[0, 1] = dr["TotalGSD"];
                    objArray[0, 2] = dr["TotalCycle"];
                    worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
                #endregion

                #region 第二頁

                #region 固定資料

                // 左上表頭資料
                worksheet = excel.ActiveWorkbook.Worksheets[2];
                worksheet.Cells[1, 5] = factory;
                worksheet.Cells[5, 5] = MyUtility.Convert.GetString(this.masterData["SewingLineID"]);
                worksheet.Cells[7, 5] = style;
                worksheet.Cells[9, 5] = this.styleCPU;

                // 右下簽名位置
                worksheet.Cells[28, 15] = DateTime.Now.ToString("d");
                worksheet.Cells[31, 15] = Sci.Env.User.UserName;

                // 左下表頭資料
                worksheet.Cells[56, 4] = this.masterData["Version"];
                worksheet.Cells[58, 4] = this.masterData["Workhour"];
                worksheet.Cells[60, 4] = this.masterData["CurrentOperators"];
                worksheet.Cells[62, 4] = this.masterData["StandardOutput"];
                worksheet.Cells[64, 4] = this.masterData["DailyDemand"];
                worksheet.Cells[66, 4] = this.masterData["TaktTime"];
                worksheet.Cells[68, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.masterData["HighestCycle"]), 2);
                worksheet.Cells[70, 4] = this.masterData["TotalCycle"];
                worksheet.Cells[72, 4] = this.masterData["TotalGSD"];
                worksheet.Cells[74, 4] = MyUtility.Check.Empty(this.masterData["TotalGSD"]) ? 0 : (MyUtility.Convert.GetDecimal(this.masterData["TotalGSD"]) - MyUtility.Convert.GetDecimal(this.masterData["TotalCycle"])) / MyUtility.Convert.GetDecimal(this.masterData["TotalGSD"]);
                worksheet.Cells[76, 4] = MyUtility.Check.Empty(this.masterData["HighestCycle"]) || MyUtility.Check.Empty(this.masterData["CurrentOperators"]) ? 0 : MyUtility.Convert.GetDecimal(this.masterData["TotalCycle"]) / MyUtility.Convert.GetDecimal(this.masterData["HighestCycle"]) / MyUtility.Convert.GetDecimal(this.masterData["CurrentOperators"]);
                worksheet.Cells[78, 4] = MyUtility.Check.Empty(this.masterData["TaktTime"]) || MyUtility.Check.Empty(this.masterData["CurrentOperators"]) ? 0 : MyUtility.Convert.GetDecimal(this.masterData["TotalCycle"]) / MyUtility.Convert.GetDecimal(this.masterData["TaktTime"]) / MyUtility.Convert.GetDecimal(this.masterData["CurrentOperators"]);
                worksheet.Cells[80, 4] = MyUtility.Check.Empty(this.masterData["HighestCycle"]) || MyUtility.Check.Empty(this.masterData["CurrentOperators"]) ? 0 : MyUtility.Convert.GetDecimal(this.masterData["TotalGSD"]) / MyUtility.Convert.GetDecimal(this.masterData["HighestCycle"]) / MyUtility.Convert.GetDecimal(this.masterData["CurrentOperators"]);
                worksheet.Cells[82, 4] = MyUtility.Check.Empty(this.masterData["HighestCycle"]) || MyUtility.Check.Empty(this.masterData["CurrentOperators"]) ? 0 : MyUtility.Math.Round(3600m / MyUtility.Convert.GetDecimal(this.masterData["HighestCycle"]), 2) * this.styleCPU / MyUtility.Convert.GetDecimal(this.masterData["CurrentOperators"]);
                #endregion

                #region MACHINE INVENTORY
                if (this.summt.Rows.Count > 3)
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A52:A52").EntireRow; // 選取要被複製的資料
                    for (int i = 3; i < this.summt.Rows.Count; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A52", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rng.Copy(Type.Missing)); // 貼上
                    }
                }

                int sumrow = 0;
                foreach (DataRow item in this.summt.Rows)
                {
                    worksheet.Cells[51 + sumrow, 1] = item["MachineTypeID"];
                    worksheet.Cells[51 + sumrow, 2] = item["sumct"];
                    sumrow++;
                }
                #endregion

                #region 新增長條圖 2

                // 新增長條圖
                Microsoft.Office.Interop.Excel.Worksheet chartData2 = excel.ActiveWorkbook.Worksheets[4];
                worksheet = excel.ActiveWorkbook.Worksheets[2];
                Microsoft.Office.Interop.Excel.Range chartRange2;
                object misValue2 = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.ChartObjects xlsCharts2 = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
                Microsoft.Office.Interop.Excel.ChartObject myChart2 = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts2.Add(378, 1082, 1000, 350);
                Microsoft.Office.Interop.Excel.Chart chartPage2 = myChart2.Chart;
                chartRange2 = chartData2.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                chartPage2.SetSourceData(chartRange2, misValue2);

                chartPage2.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

                // 新增折線圖
                Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection2 = chartPage2.SeriesCollection();
                Microsoft.Office.Interop.Excel.Series series2 = seriesCollection2.NewSeries();
                series2.Values = chartData2.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                series2.XValues = chartData2.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                series2.Name = "TotalCycle Time";

                // 折線圖的資料標籤不顯示
                series2.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

                // 隱藏Sheet
                chartData2.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                #endregion

                #region 新增長條圖 1

                // 新增長條圖
                Microsoft.Office.Interop.Excel.Worksheet chartData = excel.ActiveWorkbook.Worksheets[3];
                worksheet = excel.ActiveWorkbook.Worksheets[2];
                Microsoft.Office.Interop.Excel.Range chartRange;
                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.ChartObjects xlsCharts = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
                Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts.Add(378, 718.5, 1000, 350);
                Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
                chartRange = chartData.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                chartPage.SetSourceData(chartRange, misValue);

                chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

                // 新增折線圖
                Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection = chartPage.SeriesCollection();
                Microsoft.Office.Interop.Excel.Series series1 = seriesCollection.NewSeries();
                series1.Values = chartData.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                series1.XValues = chartData.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(intRowsStart - 1)));
                series1.Name = "Takt time";
                series1.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

                // 更改圖表版面配置 && 填入圖表標題 & 座標軸標題
                chartPage.ApplyLayout(9);
                chartPage.ChartTitle.Select();
                chartPage.ChartTitle.Text = "Line Balancing Graph";
                Microsoft.Office.Interop.Excel.Axis z = (Microsoft.Office.Interop.Excel.Axis)chartPage.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                z.AxisTitle.Text = "Act Cycle Time (in secs)";
                z = (Microsoft.Office.Interop.Excel.Axis)chartPage.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                z.AxisTitle.Text = "Operator No.";

                // 新增資料標籤
                // chartPage.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowValue, false, true);

                // 折線圖的資料標籤不顯示
                series1.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

                // 隱藏Sheet
                chartData.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                #endregion

                #region MACHINE
                decimal allct = Math.Ceiling((decimal)this.summt.Rows.Count / 3);
                Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A42:A42").EntireRow; // 選取要被複製的資料
                for (int i = 0; i < allct - 5; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A42", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                }

                int surow = 0;
                int sucol = 0;
                foreach (DataRow item in this.summt.Rows)
                {
                    worksheet.Cells[38 + surow, 4 + sucol] = item["MachineTypeID"];
                    worksheet.Cells[38 + surow, 5 + sucol] = item["sumct"];
                    surow++;
                    if (allct == surow)
                    {
                        surow = 0;
                        sucol += 2;
                    }
                }

                worksheet.Cells[38, 3] = this.atct.Rows[0]["a_ct"];
                worksheet.Cells[40, 3] = this.atct.Rows[0]["t_ct"];
                #endregion

                #region Machine Type	
                if (this.noppa.Rows.Count > 10)
                {
                    rngToCopy = worksheet.get_Range("A25:A25").EntireRow; // 選取要被複製的資料
                    for (int i = 10; i < this.noppa.Rows.Count; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A25", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                    }
                }

                int idxppa = 0;
                foreach (DataRow item in this.noppa.Rows)
                {
                    worksheet.Cells[25 + idxppa, 1] = item["OperationID"];
                    worksheet.Cells[25 + idxppa, 4] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                    worksheet.Cells[25 + idxppa, 9] = item["MachineTypeID"];
                    idxppa++;
                }
                #endregion

                #region 預設站數為2站，當超過2站就要新增
                decimal no_count = MyUtility.Convert.GetDecimal(this.nodist.Rows.Count);
                int j = 2; // 資料組數，預設為1
                if (no_count > 2)
                {
                    rngToCopy = worksheet.get_Range("A17:A21").EntireRow; // 選取要被複製的資料
                    for (j = 2; j <= MyUtility.Convert.GetInt(Math.Ceiling(no_count / 2)); j++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A17", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                    }
                }
                #endregion

                int norow = 17 + ((j - 2) * 5); // No格子上的位置Excel Y軸
                int nocolumn = 9;
                #region U字型列印
                if (this.display == "U")
                {
                    int maxct = 3;
                    int di = this.nodist.Rows.Count;
                    int addct = 0;
                    bool flag = true;
                    decimal dd = Math.Ceiling((decimal)di / 2);
                    List<int> max_ct = new List<int>();
                    for (int i = 0; i < dd; i++)
                    {
                        int a = MyUtility.Convert.GetInt(this.nodist.Rows[i]["ct"]);
                        int d = 0;
                        if (di % 2 == 1 && flag)
                        {
                            flag = false;
                        }
                        else
                        {
                            if (di % 2 == 1)
                            {
                                d = MyUtility.Convert.GetInt(this.nodist.Rows[di - i]["ct"]);
                            }
                            else
                            {
                                d = MyUtility.Convert.GetInt(this.nodist.Rows[di - 1 - i]["ct"]);
                            }
                        }

                        maxct = a > d ? a : d;
                        maxct = maxct > 3 ? maxct : 3;
                        max_ct.Add(maxct);
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                        for (int k = 3; k < maxct; k++)
                        {
                            rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                            worksheet.get_Range(string.Format("D{0}:H{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("I{0}:J{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("L{0}:M{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("N{0}:P{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            if (i > 0)
                            {
                                addct++;
                            }
                        }

                        norow = norow - 5;
                        maxct = 3;
                    }

                    bool leftDirection = true;
                    norow = 17 + ((j - 2) * 5) + addct;
                    int m = 0;
                    foreach (DataRow nodr in this.nodist.Rows)
                    {
                        if (leftDirection)
                        {
                            nocolumn = 9;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn - 7] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn - 6] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn - 5] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn - 4] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;
                                ridx++;
                            }

                            m++;
                            if (m == dd)
                            {
                                leftDirection = false;
                                m--;
                                continue;
                            }

                            norow = norow - 5 - (max_ct[m] - 3);
                        }
                        else
                        {
                            nocolumn = 12;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn + 6] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn + 5] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn + 2] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn + 3] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            norow = norow + 5 + (max_ct[m] - 3);
                            m--;
                        }
                    }
                }
                #endregion
                #region Z字型列印
                else
                {
                    int maxct = 3;
                    int ct = 0;
                    int addct = 0;
                    int indx = 1;
                    for (int l = 0; l < this.nodist.Rows.Count + 1; l++)
                    {
                        if (l < this.nodist.Rows.Count)
                        {
                            maxct = MyUtility.Convert.GetInt(this.nodist.Rows[l]["ct"]) > maxct ? MyUtility.Convert.GetInt(this.nodist.Rows[l]["ct"]) : maxct;
                        }

                        ct++;
                        if (ct == 2)
                        {
                            Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                            for (int i = 3; i < maxct; i++)
                            {
                                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                                worksheet.get_Range(string.Format("D{0}:H{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("I{0}:J{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("L{0}:M{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("N{0}:P{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                if (indx > 2)
                                {
                                    addct++;
                                }
                            }

                            norow = norow - 5;
                            ct = 0;
                            maxct = 3;
                        }

                        if (l < this.nodist.Rows.Count)
                        {
                            indx++;
                        }
                    }

                    norow = 17 + ((j - 2) * 5) + addct;
                    int leftright_count = 2;
                    bool leftDirection = true;
                    indx = 2;
                    foreach (DataRow nodr in this.nodist.Rows)
                    {
                        if (leftDirection)
                        {
                            nocolumn = 9;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn - 7] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn - 6] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn - 5] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn - 4] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            leftright_count++;
                            if (leftright_count > 2)
                            {
                                leftright_count = 1;
                                leftDirection = false;
                            }
                        }
                        else
                        {
                            nocolumn = 12;
                            worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);

                            DataRow[] nodrs = this.noda.Select(string.Format("no = '{0}'", MyUtility.Convert.GetString(nodr["No"])));
                            int ridx = 2;
                            string machinetype = string.Empty;
                            string machinetypeL = string.Empty;
                            foreach (DataRow item in nodrs)
                            {
                                worksheet.Cells[norow + ridx, nocolumn + 6] = item["cycle"];
                                worksheet.Cells[norow + ridx, nocolumn + 5] = item["gsd"];
                                worksheet.Cells[norow + ridx, nocolumn + 2] = this.contentType == "A" ? MyUtility.Convert.GetString(item["Annotation"]).Trim() : MyUtility.Convert.GetString(item["DescEN"]).Trim();
                                worksheet.Cells[norow, nocolumn + 3] = item["name"];
                                machinetypeL = MyUtility.Convert.GetString(item["machineTypeid"]).EqualString(machinetype) ? string.Empty : MyUtility.Convert.GetString(item["machineTypeid"]);
                                machinetype = MyUtility.Convert.GetString(item["machineTypeid"]);
                                worksheet.Cells[norow + ridx, nocolumn] = machinetypeL;

                                ridx++;
                            }

                            leftright_count++;
                            if (leftright_count > 2)
                            {
                                leftright_count = 1;
                                leftDirection = true;
                            }
                        }

                        if (this.nodist.Rows.Count > indx)
                        {
                            maxct = MyUtility.Convert.GetInt(this.nodist.Rows[indx]["ct"]) > maxct ? MyUtility.Convert.GetInt(this.nodist.Rows[indx]["ct"]) : maxct;
                        }

                        if (leftright_count == 2)
                        {
                            norow = norow - 5 - (maxct - 3);
                            maxct = 3;
                        }

                        indx++;
                    }
                }
                #endregion

                excel.ActiveWorkbook.Worksheets[5].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                excel.ActiveWorkbook.Worksheets[6].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                excel.ActiveWorkbook.Worksheets[7].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                #endregion
            }

            // 寫此行目的是要將Excel畫面上顯示Copy給取消
            excel.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCopy;

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("IE_P03_Print");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
