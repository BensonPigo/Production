using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P03_Print
    /// </summary>
    public partial class P03_Print : Win.Tems.PrintForm
    {
        private DataRow masterData;
        private string display;
        private string contentType;
        private string strLanguage;
        private DataTable operationCode;
        private DataTable machineTypeDT;
        private DataTable nodist;
        private DataTable nodist2;
        private DataTable nodistPPA;
        private DataTable nodist2PPA;
        private DataTable noppa;
        private List<AttachmentData> AttachmentDataList;
        private decimal styleCPU;
        private decimal changp;
        private string changpPPA;
        private decimal count1;
        private decimal count2;
        private decimal count1PPA;
        private decimal count2PPA;
        private bool change = false;
        private bool changePPA = false;
        private bool nonSewing;
        private bool isPPA;

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
            this.radioU_Left.Checked = true;
            this.radioDescription.Checked = true;
            this.rbDetail.Checked = true;
            MyUtility.Tool.SetupCombox(this.comboLanguage, 2, 1, "en,English,cn,Chinese,vn,Vietnam,kh,Cambodia");
            this.comboLanguage.SelectedIndex = 0;
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (this.chkpagePPA.Checked && MyUtility.Check.Empty(this.txtPagePPA.Text))
            {
                MyUtility.Msg.ErrorBox("PPA can't be empty.");
                return false;
            }

            if (this.chkpagePPA.Checked && !this.txtPagePPA.Text.ToString().Substring(0, 1).ToUpper().EqualString("P"))
            {
                MyUtility.Msg.ErrorBox("The first word must be P if the [Change page 2 at No(For PPA)] is checked.");
                return false;
            }

            if (this.radioU_Left.Checked)
            {
                this.display = "U_Left";
            }
            else if (this.radioU_Right.Checked)
            {
                this.display = "U_Right";
            }
            else if (this.radioZ_Left.Checked)
            {
                this.display = "Z_Left";
            }
            else
            {
                this.display = "Z_Right";
            }

            this.contentType = this.radioDescription.Checked ? "D" : "A";
            this.changp = MyUtility.Convert.GetDecimal(this.numpage.Value);
            this.changpPPA = this.txtPagePPA.Text.ToString();
            this.strLanguage = this.comboLanguage.SelectedValue.ToString();
            this.nonSewing = this.chkNonSewing.Checked;
            this.isPPA = this.chkPPA.Checked;
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlp1 = string.Empty;
            string sqlp2 = string.Empty;
            #region 切分頁計算 takt Page2
            decimal ttlnocount = MyUtility.Convert.GetInt(this.masterData["CurrentOperators"]);
            if (this.chkpage.Checked && ttlnocount > this.changp)
            {
                sqlp1 = $@"
select no 
into #tmp
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = '{MyUtility.Convert.GetString(this.masterData["ID"])}'
and ld.PPA != 'C'
and (ld.IsHide = 0 or ld.IsHide is null)

select count(distinct no)
FROM #tmp
where  no <= {this.changp}

drop table #tmp
";
                sqlp2 = $@"
select no 
into #tmp
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = '{MyUtility.Convert.GetString(this.masterData["ID"])} '
and ld.PPA != 'C'
and (ld.IsHide = 0 or ld.IsHide is null)

select count(distinct no)
FROM #tmp
where  no > {this.changp}

drop table #tmp
";
                this.count1 = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp1));
                this.count2 = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp2));
                this.change = true;
            }
            else
            {
                this.change = false;
            }
            #endregion

            #region 切分頁計算 takt Page3 PPA
            if (this.chkpagePPA.Checked)
            {
                sqlp1 = $@"
select no = count(distinct no)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = '{MyUtility.Convert.GetString(this.masterData["ID"])}' 
--and IsPPa = 1 
and IsHide = 0 
and no <= '{this.changpPPA}'
{(!this.isPPA ? " and ld.PPA != 'C' " : " and ld.PPA = 'C' ")}
";
                sqlp2 = $@"
select no = count(distinct no)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = '{MyUtility.Convert.GetString(this.masterData["ID"])}' 
--and IsPPa = 1 
and IsHide = 0 
and no > '{this.changpPPA}'
{(!this.isPPA ? " and ld.PPA != 'C' " : string.Empty)}
";
                this.count1PPA = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp1));
                this.count2PPA = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp2));
                this.changePPA = true;
            }
            else
            {
                sqlp1 = $@"
select no = count(distinct no)
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = '{MyUtility.Convert.GetString(this.masterData["ID"])}' 
--and IsPPa = 1 
and IsHide = 0
{(!this.isPPA ? " and ld.PPA != 'C' " : string.Empty)}
";
                this.count1PPA = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlp1));
                this.changePPA = false;
            }
            #endregion

            string sqlCmd;

            #region 第一頁

            sqlCmd = $@"
select   a.GroupKey
        ,a.OperationID
        ,a.Annotation
        ,a.GSD
        ,a.MachineTypeID--MachineTypeID = iif(m.MachineGroupID = '','',a.MachineTypeID)
		,a.MasterPlusGroup
        ,a.Attachment
        ,a.Template
        ,a.ThreadColor
        ,DescEN = case when '{this.strLanguage}' = 'cn' then isnull(o.DescCH,o.DescEN)
                       when '{this.strLanguage}' = 'vn' then isnull(o.DescVN,o.DescEN)
                       when '{this.strLanguage}' = 'kh' then isnull(o.DescKH,o.DescEN)
            else o.DescEN end
        ,rn = ROW_NUMBER() over(order by case when left(a.No, 1) = 'P' then 1 when a.No <> '' then 2 else 3 end
										,a.GroupKey ,iif(IsPPa=1,1,0) ,a.NO, a.MachineTypeID, a.Attachment, a.Template, a.ThreadColor)
        ,a.Cycle
        ,a.ActCycle
        ,a.IsPPa
        ,a.PPA
        ,a.No
        ,[IsHide] = isnull(a.IsHide, 0)
        ,[GroupHeader] = iif(left(a.OperationID, 2) = '--', '', ld.OperationID)
        ,[IsShowinIEP03] = cast(iif( a.OperationID like '--%' , 1, isnull(show.IsShowinIEP03, 1)) as bit)
        ,[OtherBy] = concat(a.MachineTypeID,a.Attachment,a.Template,a.ThreadColor)
		,a.SewingMachineAttachmentID
        ,a.MachineCount
from LineMapping_Detail a 
inner join LineMapping b WITH (NOLOCK) on a.ID = b.ID
left join Operation o WITH (NOLOCK) on o.ID = a.OperationID
left join MachineType m WITH (NOLOCK) on m.id =  a.MachineTypeID
outer apply
(
	select OperationID
	from LineMapping_Detail
	where a.ID = ID
	and OriNO in 
	(
		select max(ld.OriNO)
		from LineMapping_Detail ld
		where a.ID = ld.ID
		and left(ld.OperationID , 2) = '--'
		and ld.OriNO < a.OriNO
		and not exists (select 1 from DropDownList d where d.Type = 'IEP03HideGroupHeader' and d.ID = ld.OperationID)
	)
)ld
outer apply (
	select IsShowinIEP03 = IIF(isnull(md2.IsNotShownInP03, 0) = 0, 1, 0)
		, IsDesignatedArea = ISNULL(md2.IsNonSewingLine,0)
	from MachineType m2 WITH (NOLOCK)
    inner join MachineType_Detail md2 WITH (NOLOCK) on md2.ID = m2.ID and md2.FactoryID = b.FactoryID
	where o.MachineTypeID = m2.ID and m2.junk = 0
)show
where a.ID = {MyUtility.Convert.GetString(this.masterData["ID"])}
{(!this.nonSewing ? $@"and not exists(select 1 from MachineType_Detail md where md.ID = m.ID and md.IsNonSewingLine = 1 and md.FactoryID=b.FactoryID)" : string.Empty)}
{(!this.isPPA ? " and a.PPA != 'C' " : string.Empty)}
";

            List<AttachmentData> tmpAttachmentDataList = new List<AttachmentData>();
            this.AttachmentDataList = new List<AttachmentData>();
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.machineTypeDT);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query operation code data fail\r\n" + result.ToString());
                return failResult;
            }

            if (this.machineTypeDT == null ||
                this.machineTypeDT.Rows.Count == 0 ||
                this.machineTypeDT.AsEnumerable().Where(x => x.Field<bool>("IsShowinIEP03")).ToList().Count == 0)
            {
                DualResult failResult = new DualResult(false, "no detail data to Print");
                return failResult;
            }

            foreach (DataRow dr in this.machineTypeDT.Rows)
            {
                if (MyUtility.Check.Empty(dr["No"]))
                {
                    // 沒有No的Attachment和Template都不要被計算
                    continue;
                }

                AttachmentData rawData = new AttachmentData();
                rawData.No = MyUtility.Convert.GetString(dr["No"]);
                rawData.STMC_Type = MyUtility.Convert.GetString(dr["MachineTypeID"]);
                rawData.MachineGroup = MyUtility.Convert.GetString(dr["MasterPlusGroup"]);
                rawData.Template = MyUtility.Convert.GetString(dr["Template"]);
                rawData.Attachment = MyUtility.Convert.GetString(dr["Attachment"]);
                rawData.AttachmentPartID = MyUtility.Convert.GetString(dr["SewingMachineAttachmentID"]);
                rawData.PPA = MyUtility.Convert.GetString(dr["PPA"]);
                rawData.IsHide = MyUtility.Convert.GetBool(dr["IsHide"]);

                tmpAttachmentDataList.Add(rawData);
            }

            /*  資料分成以下兩種，然後Attachment和Template都會有多筆(用逗號隔開)
                ST/MC Type + Machine Group + No. + Attachment + PartID
                ST/MC Type + Machine Group + No. + Template + PartID
                其中，上面Template的PartID沒有有紀錄在表身，要動態查
             */
            foreach (var rawData in tmpAttachmentDataList)
            {
                string tt = string.Empty;
                if (MyUtility.Check.Empty(rawData.Attachment) && MyUtility.Check.Empty(rawData.Template))
                {
                    continue;
                }

                // Attachment不為空
                if (!MyUtility.Check.Empty(rawData.Attachment) && rawData.Attachment.Split(',').Where(o => !string.IsNullOrEmpty(o)).Any())
                {
                    var attList = rawData.Attachment.Split(',').Where(o => !string.IsNullOrEmpty(o));
                    var partList = !MyUtility.Check.Empty(rawData.AttachmentPartID) && rawData.AttachmentPartID.Split(',').Where(o => !string.IsNullOrEmpty(o)).Any() ?
                        rawData.AttachmentPartID.Split(',').Where(o => !string.IsNullOrEmpty(o)).ToList() : new List<string>();

                    foreach (var att in attList)
                    {
                        // 紀錄該Attachment底下是否有任何PartID
                        bool hasAnyPartID = false;

                        if (partList.Any())
                        {
                            foreach (var part in partList)
                            {
                                // 覆蓋Attachment
                                AttachmentData newData = new AttachmentData()
                                {
                                    No = rawData.No,
                                    STMC_Type = rawData.STMC_Type,
                                    MachineGroup = rawData.MachineGroup,
                                    PPA = rawData.PPA,
                                    IsHide = rawData.IsHide,
                                    Attachment = att,
                                };

                                // 判斷Part 隸屬於這個Attachment
                                string sql = $@" select 1 from SewingMachineAttachment  where MoldID = @Attachment and ID = @PartID ";
                                List<SqlParameter> paras = new List<SqlParameter>()
                                {
                                    new SqlParameter("@Attachment", att),
                                    new SqlParameter("@PartID", part),
                                };
                                bool isPartMatch = MyUtility.Check.Seek(sql, paras);

                                if (isPartMatch)
                                {
                                    hasAnyPartID = true;

                                    // 覆蓋PartID
                                    newData.AttachmentPartID = part;
                                    newData.Template = string.Empty;
                                    bool exists = this.AttachmentDataList.Where(o => o.No == newData.No && o.STMC_Type == newData.STMC_Type && o.MachineGroup == newData.MachineGroup && o.Attachment == newData.Attachment && o.AttachmentPartID == newData.AttachmentPartID).Any();
                                    if (!exists)
                                    {
                                        this.AttachmentDataList.Add(newData);
                                    }
                                }
                                else
                                {
                                    // 判斷這個Part 是否隸屬其他 Attachment
                                    var otherAtt = attList.Where(o => o != att).ToList();
                                    List<string> otherAttStr = new List<string>();

                                    int count = 0;
                                    foreach (var item in otherAtt)
                                    {
                                        otherAttStr.Add($"@Attachment{count}");
                                        paras.Add(new SqlParameter($"@Attachment{count}", item));
                                    }

                                    sql = $@" select 1 from SewingMachineAttachment  where MoldID != @Attachment and ID = @PartID and MoldID IN ({string.Join(",", otherAttStr)})";
                                    isPartMatch = MyUtility.Check.Seek(sql, paras);

                                    if (!isPartMatch)
                                    {
                                        newData.AttachmentPartID = string.Empty;
                                        newData.Template = string.Empty;
                                        bool exists = this.AttachmentDataList.Where(o => o.No == newData.No && o.STMC_Type == newData.STMC_Type && o.MachineGroup == newData.MachineGroup && o.Attachment == newData.Attachment && o.AttachmentPartID == newData.AttachmentPartID).Any();
                                        if (!exists)
                                        {
                                            this.AttachmentDataList.Add(newData);
                                        }
                                    }
                                }
                            }

                            // 如果Attachment底下沒有任何PartID，則塞一筆Part ID空白的資料
                            if (hasAnyPartID == false)
                            {
                                AttachmentData newData = new AttachmentData()
                                {
                                    No = rawData.No,
                                    STMC_Type = rawData.STMC_Type,
                                    MachineGroup = rawData.MachineGroup,
                                    PPA = rawData.PPA,
                                    IsHide = rawData.IsHide,
                                    Attachment = att,
                                };
                                newData.AttachmentPartID = string.Empty;
                                newData.Template = string.Empty;
                                bool exists = this.AttachmentDataList.Where(o => o.No == newData.No && o.STMC_Type == newData.STMC_Type && o.MachineGroup == newData.MachineGroup && o.Attachment == newData.Attachment && o.AttachmentPartID == newData.AttachmentPartID).Any();
                                if (!exists)
                                {
                                    this.AttachmentDataList.Add(newData);
                                }
                            }
                        }
                        else
                        {
                            // 覆蓋Attachment
                            AttachmentData newData = new AttachmentData()
                            {
                                No = rawData.No,
                                STMC_Type = rawData.STMC_Type,
                                MachineGroup = rawData.MachineGroup,
                                PPA = rawData.PPA,
                                IsHide = rawData.IsHide,
                                Attachment = att,
                                Template = string.Empty,
                            };

                            this.AttachmentDataList.Add(newData);
                        }
                    }
                }

                // Template不為空，要動態查Template的PartID
                if (!MyUtility.Check.Empty(rawData.Template) && rawData.Template.Split(',').Where(o => !string.IsNullOrEmpty(o)).Any())
                {
                    var templateList = rawData.Template.Split(',').Where(o => !string.IsNullOrEmpty(o));

                    foreach (var template in templateList)
                    {
                        // 覆蓋Template
                        AttachmentData newData = new AttachmentData()
                        {
                            No = rawData.No,
                            STMC_Type = rawData.STMC_Type,
                            MachineGroup = rawData.MachineGroup,
                            PPA = rawData.PPA,
                            IsHide = rawData.IsHide,
                            Template = template,
                        };

                        // 覆蓋Attachment
                        newData.Template = template;

                        string sql = $@"
select PartID = smt.ID , m.DescEN ,MoldID = m.ID
from Mold m WITH (NOLOCK)
right join SewingMachineTemplate smt on m.ID = smt.MoldID
where m.Junk = 0 and m.IsTemplate = 1 and smt.Junk = 0 AND smt.ID = @Template
";
                        List<SqlParameter> paras = new List<SqlParameter>()
                        {
                            new SqlParameter("@Template", template),
                        };

                        DataTable dt;
                        DualResult r = DBProxy.Current.Select(null, sql, paras, out dt);

                        if (!r)
                        {
                            this.ShowErr(r);
                        }

                        string moldID = string.Empty;
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            moldID = MyUtility.Convert.GetString(dt.Rows[0]["MoldID"]);
                        }

                        newData.TemplateMoldID = moldID;
                        bool exists = this.AttachmentDataList.Where(o => o.No == newData.No && o.STMC_Type == newData.STMC_Type && o.MachineGroup == newData.MachineGroup && o.Template == newData.Template && o.TemplateMoldID == newData.TemplateMoldID).Any();
                        if (!exists)
                        {
                            newData.Attachment = string.Empty;
                            this.AttachmentDataList.Add(newData);
                        }
                    }
                }
            }

            this.operationCode = this.machineTypeDT.AsEnumerable().Where(x => x.Field<bool>("IsShowinIEP03")).CopyToDataTable();
            #endregion
            #region Machine Type IsPPa

            sqlCmd = $@"
select   ld.OperationID
        ,ld.MachineTypeID--MachineTypeID = iif(m.MachineGroupID = '','',ld.MachineTypeID)
        ,ld.Annotation
        ,DescEN = case when '{this.strLanguage}' = 'cn' then isnull(o.DescCH,o.DescEN)
                       when '{this.strLanguage}' = 'vn' then isnull(o.DescVN,o.DescEN)
                       when '{this.strLanguage}' = 'kh' then isnull(o.DescKH,o.DescEN)
            else o.DescEN end
from LineMapping_Detail ld WITH (NOLOCK)
left join MachineType m WITH (NOLOCK) on m.id =  MachineTypeID
left join Operation o WITH (NOLOCK) on o.ID = ld.OperationID
where ld.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} and (ld.IsHide = 1 or ld.IsPPa  = 1)
and left(ld.OperationID, 2) != '--'
and (ld.IsHide = 1  {(!this.isPPA ? "or ld.PPA != 'C' " : string.Empty)})
order by ld.No,ld.GroupKey


";
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
                sqlCmd = $@"
select No 
    ,CT = COUNT(1)
    ,[ActCycle] = Max(ld.ActCycle)
    ,[ActCycleTime(average)]=ActCycle.Value
into #tmp
from LineMapping_Detail ld WITH (NOLOCK) 
OUTER APPLY(
	SELECT [Value]=SUM(ActCycle)/COUNT(NO) FROM 
	(
		SELECT DISTINCT No, ActCycle, TotalGSD, TotalCycle
		FROM 
		(
			select  ld.*
					, Description = case when '{this.strLanguage}' = 'cn' then isnull(o.DescCH,o.DescEN)
                                         when '{this.strLanguage}' = 'vn' then isnull(o.DescVN,o.DescEN)
                                         when '{this.strLanguage}' = 'kh' then isnull(o.DescKH,o.DescEN)
                                         else o.DescEN end
					, e.Name as EmployeeName
					, e.Skill as EmployeeSkill
					, iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100) as Efficiency
			from LineMapping_Detail ld WITH (NOLOCK) 
			left join Employee e WITH (NOLOCK) on ld.EmployeeID = e.ID
			left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
			where ld.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} AND No <> ''
		)a
	)b
)ActCycle
where ld.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} 
and (ld.IsHide = 0 or ld.IsHide is null)
and no <> ''
and ld.PPA != 'C' 
GROUP BY NO ,ActCycle.Value
order by no

SELECT 
    t.*,
    [Name] = e.val
FROM #tmp t
OUTER APPLY (
   SELECT val = STUFF((
        SELECT distinct CONCAT(' ', tmp.[Name], ' / ')
        FROM (
            SELECT [Name] = IIF(c.Junk = 1, c.ID + ' ' + c.[Name], c.ID + ' ' + c.LastName + ',' + c.FirstName)
            FROM Employee c
            INNER JOIN LineMapping_Detail e ON c.id = e.EmployeeID
            WHERE e.no = t.no and e.id = {MyUtility.Convert.GetString(this.masterData["ID"])} and c.junk = 0
        ) tmp 
        FOR XML PATH('')
    ), 1, 1, '')
) e;

drop TABLE #tmp

";
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodist);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            #endregion
            #region 第二頁 有分頁
            else
            {
                #region

                sqlCmd = $@"
select No 
    ,CT = COUNT(1)
    ,[ActCycle] = Max(ld.ActCycle)
    ,[ActCycleTime(average)]=ActCycle.Value
into #tmp
from LineMapping_Detail ld WITH (NOLOCK) 
OUTER APPLY(
	SELECT [Value]=SUM(ActCycle)/COUNT(NO) FROM 
	(
		SELECT DISTINCT No, ActCycle, TotalGSD, TotalCycle
		FROM 
		(
			select  ld.*
					, Description = case when '{this.strLanguage}' = 'cn' then isnull(o.DescCH,o.DescEN)
                                         when '{this.strLanguage}' = 'vn' then isnull(o.DescVN,o.DescEN)
                                         when '{this.strLanguage}' = 'kh' then isnull(o.DescKH,o.DescEN)
                                         else o.DescEN end
					, e.Name as EmployeeName
					, e.Skill as EmployeeSkill
					, iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100) as Efficiency
			from LineMapping_Detail ld WITH (NOLOCK) 
			left join Employee e WITH (NOLOCK) on ld.EmployeeID = e.ID
			left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
			where ld.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} AND No <> ''
		)a
	)b
)ActCycle
where ld.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} 
and (ld.IsHide = 0 or ld.IsHide is null)
and no <> ''
and ld.PPA != 'C' 
GROUP BY NO ,ActCycle.Value
order by no

SELECT 
    t.*,
    [Name] = e.val
FROM #tmp t
OUTER APPLY (
   SELECT val = STUFF((
        SELECT distinct CONCAT(' ', tmp.[Name], ' / ')
        FROM (
            SELECT [Name] = IIF(c.Junk = 1, c.ID + ' ' + c.[Name], c.ID + ' ' + c.LastName + ',' + c.FirstName)
            FROM Employee c
            INNER JOIN LineMapping_Detail e ON c.id = e.EmployeeID
            WHERE e.no = t.no and e.id = {MyUtility.Convert.GetString(this.masterData["ID"])} and c.junk = 0
        ) tmp 
        FOR XML PATH('')
    ), 1, 1, '')
) e;

drop TABLE #tmp
";

                result = DBProxy.Current.Select(null, sqlCmd, out this.nodist);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = $@"
select distinct ld.ID ,no
into #tmpBase
from LineMapping_Detail ld WITH (NOLOCK)
where ld.ID = '{MyUtility.Convert.GetString(this.masterData["ID"])}'
and ld.PPA != 'C'
and (ld.IsHide = 0 or ld.IsHide is null)


select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select  ID ,no
	from #tmpBase
	where no > {this.changp} 
)x
group by ID

select No,CT = COUNT(1),[ActCycle] = Max(ld.ActCycle)
,[ActCycleTime(average)]=ActCycle.Value
into #tmp1
from LineMapping_Detail ld WITH (NOLOCK) 
inner join #tmp t on ld.ID = t.ID
OUTER APPLY(
	SELECT [Value]=SUM(ActCycle)/COUNT(NO) FROM 
	(
		SELECT DISTINCT No, ActCycle, TotalGSD, TotalCycle
		FROM 
		(
			select  ld.*
					, Description = case when '{this.strLanguage}' = 'cn' then isnull(o.DescCH,o.DescEN)
                                         when '{this.strLanguage}' = 'vn' then isnull(o.DescVN,o.DescEN)
                                         when '{this.strLanguage}' = 'kh' then isnull(o.DescKH,o.DescEN)
                                         else o.DescEN end
					, e.Name as EmployeeName
					, e.Skill as EmployeeSkill
					, iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100) as Efficiency
			from LineMapping_Detail ld WITH (NOLOCK) 
			left join Employee e WITH (NOLOCK) on ld.EmployeeID = e.ID
			left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
			where ld.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} AND No <> ''
		)a
	)b
)ActCycle
where  (ld.IsPPa = 0 or ld.IsPPa is null) 
and (ld.IsHide = 0 or ld.IsHide is null) 
and no between t.minno and t.maxno
and ld.PPA != 'C'
GROUP BY NO ,ActCycle.Value
order by no

SELECT 
    t.*,
    [Name] = e.val
FROM #tmp1 t
OUTER APPLY (
   SELECT val = STUFF((
        SELECT distinct CONCAT(' / ', tmp.[Name])
        FROM (
            SELECT [Name] = IIF(c.Junk = 1, c.ID + ' ' + c.[Name], c.ID + ' ' + c.LastName + ',' + c.FirstName)
            FROM Employee c
            INNER JOIN LineMapping_Detail e ON c.id = e.EmployeeID
            WHERE e.no = t.no and e.id = {MyUtility.Convert.GetString(this.masterData["ID"])} and c.junk = 0
        ) tmp 
        FOR XML PATH('')
    ), 1, 1, '')
) e;

drop TABLE #tmp,#tmp1
";
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodist2);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
                #endregion
            }
            #endregion

            #region 第三頁 PPA
            if (!this.changePPA)
            {
                sqlCmd = $@"
select No 
    ,CT = COUNT(1)
    ,[ActCycle] = Max(ld.ActCycle)
    ,[ActCycleTime(average)]=ActCycle.Value
into #tmp
from LineMapping_Detail ld WITH (NOLOCK) 
inner join LineMapping l WITH (NOLOCK) on l.ID = ld.ID
left join MachineType m WITH (NOLOCK) on m.id =  ld.MachineTypeID
OUTER APPLY(
	SELECT [Value]=SUM(ActCycle)/COUNT(NO) FROM 
	(
		SELECT DISTINCT No, ActCycle, TotalGSD, TotalCycle
		FROM 
		(
			select  ldd.*
					, Description = case when '{this.strLanguage}' = 'cn' then isnull(o.DescCH,o.DescEN)
                                         when '{this.strLanguage}' = 'vn' then isnull(o.DescVN,o.DescEN)
                                         when '{this.strLanguage}' = 'kh' then isnull(o.DescKH,o.DescEN)
                                         else o.DescEN end
					, e.Name as EmployeeName
					, e.Skill as EmployeeSkill
					, iif(ldd.Cycle = 0,0,ROUND(ldd.GSD/ldd.Cycle,2)*100) as Efficiency
			from LineMapping_Detail ldd WITH (NOLOCK) 
			left join Employee e WITH (NOLOCK) on ldd.EmployeeID = e.ID
			left join Operation o WITH (NOLOCK) on ldd.OperationID = o.ID
			where ldd.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} and IsHide = 0 and ldd.PPA = 'C' 
		)a
	)b
)ActCycle
where ld.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} and IsHide = 0
{(!this.nonSewing ? $@" and not exists(select 1 from MachineType_Detail md where md.ID = m.ID and md.IsNonSewingLine = 1 and md.FactoryID = l.FactoryID)" : string.Empty)}
{(!this.isPPA ? " and ld.PPA = 'C' and ld.PPA != '' " : " and ld.PPA = 'C' ")}
GROUP BY NO, ActCycle.Value
order by NO


SELECT 
    t.*,
    [Name] = e.val
FROM #tmp t
OUTER APPLY (
   SELECT val = STUFF((
        SELECT distinct CONCAT(' ', tmp.[Name], ' / ')
        FROM (
            SELECT [Name] = IIF(c.Junk = 1, c.ID + ' ' + c.[Name], c.ID + ' ' + c.LastName + ',' + c.FirstName)
            FROM Employee c
            INNER JOIN LineMapping_Detail e ON c.id = e.EmployeeID
            WHERE e.no = t.no and e.id = {MyUtility.Convert.GetString(this.masterData["ID"])} and c.junk = 0
        ) tmp 
        FOR XML PATH('')
    ), 1, 1, '')
) e;

drop TABLE #tmp
";
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodistPPA);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            #endregion
            #region 第三頁 PPA 有分頁
            else
            {
                #region

                sqlCmd = $@"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} and IsHide = 0 and ld.PPA = 'C' 
	and no <= '{this.changpPPA}'
)x
group by ID

select No
    ,CT = COUNT(1)
    ,[ActCycle] = Max(ld.ActCycle)
    ,[ActCycleTime(average)]=ActCycle.Value
into #tmp
from LineMapping_Detail ld WITH (NOLOCK) 
inner join LineMapping l WITH (NOLOCK) on l.ID = ld.ID 
inner join #tmp t on ld.ID = t.ID
left join MachineType m WITH (NOLOCK) on m.id =  ld.MachineTypeID
OUTER APPLY(
	SELECT [Value]=SUM(ActCycle)/COUNT(NO) FROM 
	(
		SELECT DISTINCT No, ActCycle, TotalGSD, TotalCycle
		FROM 
		(
			select  ldd.*
					, Description = case when '{this.strLanguage}' = 'cn' then isnull(o.DescCH,o.DescEN)
                                         when '{this.strLanguage}' = 'vn' then isnull(o.DescVN,o.DescEN)
                                         when '{this.strLanguage}' = 'kh' then isnull(o.DescKH,o.DescEN)
                                         else o.DescEN end
					, e.Name as EmployeeName
					, e.Skill as EmployeeSkill
					, iif(ldd.Cycle = 0,0,ROUND(ldd.GSD/ldd.Cycle,2)*100) as Efficiency
			from LineMapping_Detail ldd WITH (NOLOCK) 
			left join Employee e WITH (NOLOCK) on ldd.EmployeeID = e.ID
			left join Operation o WITH (NOLOCK) on ldd.OperationID = o.ID
			where ldd.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} AND No <> '' and IsHide = 0 and ldd.PPA = 'C' 
		)a
	)b
)ActCycle
where /*IsPPa = 1 and*/  IsHide = 0 
and no between t.minno and t.maxno
and ld.PPA = 'C' 
{(!this.nonSewing ? $@" and not exists(select 1 from MachineType_Detail md where md.ID = m.ID and md.IsNonSewingLine = 1 and md.FactoryID = l.FactoryID)" : string.Empty)}
GROUP BY NO, ActCycle.Value
order by NO


SELECT 
    t.*,
    [Name] = e.val
FROM #tmp t
OUTER APPLY (
   SELECT val = STUFF((
        SELECT distinct CONCAT(' ', tmp.[Name], ' / ')
        FROM (
            SELECT [Name] = IIF(c.Junk = 1, c.ID + ' ' + c.[Name], c.ID + ' ' + c.LastName + ',' + c.FirstName)
            FROM Employee c
            INNER JOIN LineMapping_Detail e ON c.id = e.EmployeeID
            WHERE e.no = t.no and e.id = {MyUtility.Convert.GetString(this.masterData["ID"])} and c.junk = 0
        ) tmp 
        FOR XML PATH('')
    ), 1, 1, '')
) e;

drop TABLE #tmp
";
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodistPPA);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd = $@"
select id, minno = min(no), maxno = max(no)
into #tmp
from(
	select distinct ld.ID,no
	from LineMapping_Detail ld WITH (NOLOCK)
	where ld.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} and IsHide = 0 
    {(!this.isPPA ? " and ld.PPA != 'C' " : " and ld.PPA != '' ")}
	and no > '{this.changpPPA}'
)x
group by ID

select No
    ,CT = COUNT(1)
    ,[ActCycle] = Max(ld.ActCycle)
    ,[ActCycleTime(average)]=ActCycle.Value
into #tmp1
from LineMapping_Detail ld WITH (NOLOCK) 
inner join LineMapping l WITH (NOLOCK) on l.ID = ld.ID 
inner join #tmp t on ld.ID = t.ID
left join MachineType m WITH (NOLOCK) on m.id =  ld.MachineTypeID
OUTER APPLY(
	SELECT [Value]=SUM(ActCycle)/COUNT(NO) FROM 
	(
		SELECT DISTINCT No, ActCycle, TotalGSD, TotalCycle
		FROM 
		(
			select  ldd.*
					, Description = case when '{this.strLanguage}' = 'cn' then isnull(o.DescCH,o.DescEN)
                                         when '{this.strLanguage}' = 'vn' then isnull(o.DescVN,o.DescEN)
                                         when '{this.strLanguage}' = 'kh' then isnull(o.DescKH,o.DescEN)
                                         else o.DescEN end
					, e.Name as EmployeeName
					, e.Skill as EmployeeSkill
					, iif(ldd.Cycle = 0,0,ROUND(ldd.GSD/ldd.Cycle,2)*100) as Efficiency
			from LineMapping_Detail ldd WITH (NOLOCK) 
			left join Employee e WITH (NOLOCK) on ldd.EmployeeID = e.ID
			left join Operation o WITH (NOLOCK) on ldd.OperationID = o.ID
			where ldd.ID = {MyUtility.Convert.GetString(this.masterData["ID"])} and IsHide = 0 and ldd.PPA = 'C'
		)a
	)b
)ActCycle
where IsHide = 0 
and no between t.minno and t.maxno
and ld.PPA = 'C'
{(!this.nonSewing ? $@" and not exists(select 1 from MachineType_Detail md where md.ID = m.ID and md.IsNonSewingLine = 1 and md.FactoryID = l.FactoryID)" : string.Empty)}
GROUP BY NO, ActCycle.Value
order by NO


SELECT 
    t.*,
    [Name] = e.val
FROM #tmp1 t
OUTER APPLY (
   SELECT val = STUFF((
        SELECT CONCAT(' ', tmp.[Name], ' / ')
        FROM (
            SELECT [Name] = IIF(c.Junk = 1, c.ID + ' ' + c.[Name], c.ID + ' ' + c.LastName + ',' + c.FirstName)
            FROM Employee c
            INNER JOIN LineMapping_Detail e ON c.id = e.EmployeeID
            WHERE e.no = t.no and e.id = {MyUtility.Convert.GetString(this.masterData["ID"])} 
        ) tmp 
        FOR XML PATH('')
    ), 1, 1, '')
) e;

drop TABLE #tmp1
";
                result = DBProxy.Current.Select(null, sqlCmd, out this.nodist2PPA);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query print data fail\r\n" + result.ToString());
                    return failResult;
                }
                #endregion
            }
            #endregion
            return Ict.Result.True;
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

            string strXltName = Env.Cfg.XltPathDir + "\\IE_P03_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);

#if DEBUG
            // excel.Visible = true;
#endif
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

            // excel 範圍別名宣告 公式使用
            excel.ActiveWorkbook.Names.Add("Operation", worksheet.Range["A6", "N" + this.operationCode.Rows.Count + 5]);

            // 填Operation
            int intRowsStart = 6;
            object[,] objArray = new object[1, 14];
            foreach (DataRow dr in this.operationCode.Rows)
            {
                objArray[0, 0] = dr["rn"];
                objArray[0, 1] = dr["GroupHeader"];
                objArray[0, 2] = this.contentType == "A" ? MyUtility.Convert.GetString(dr["Annotation"]).Trim() : MyUtility.Convert.GetString(dr["DescEN"]).Trim();
                objArray[0, 3] = dr["MachineTypeID"];
                objArray[0, 4] = dr["MasterPlusGroup"];
                objArray[0, 5] = dr["Attachment"];
                objArray[0, 6] = dr["SewingMachineAttachmentID"];
                objArray[0, 7] = dr["Template"];
                objArray[0, 8] = dr["GSD"];
                objArray[0, 9] = dr["Cycle"];
                objArray[0, 10] = dr["ThreadColor"];
                objArray[0, 11] = dr["OperationID"];
                objArray[0, 12] = $"=CONCATENATE(D{intRowsStart},\" \",F{intRowsStart},\" \",H{intRowsStart},\" \",K{intRowsStart})";
                objArray[0, 13] = dr["MasterPlusGroup"];
                worksheet.Range[string.Format("A{0}:N{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            worksheet.Cells[intRowsStart, 1] = string.Format("=MAX($A$2:A{0})+1", intRowsStart - 1);

            // time
            worksheet.Cells[intRowsStart, 8] = string.Format("=SUM(I6:I{0})", intRowsStart - 1);
            worksheet.Range[string.Format("A5:L{0}", intRowsStart)].Borders.Weight = 1; // 1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[string.Format("A5:L{0}", intRowsStart)].Borders.LineStyle = 1;

            intRowsStart++;
            worksheet.Cells[intRowsStart, 8] = string.Format("=H{0}/{1}", intRowsStart - 1, MyUtility.Convert.GetInt(this.masterData["CurrentOperators"]));
            worksheet.get_Range("H" + intRowsStart, "H" + intRowsStart).Font.Bold = true;

            intRowsStart++;
            worksheet.Cells[intRowsStart, 2] = "Picture";
            worksheet.get_Range("C" + intRowsStart, "C" + intRowsStart).Font.Bold = true;

            // 插圖 Picture1
            intRowsStart++;
            string destination_path = MyUtility.GetValue.Lookup("select StyleSketch from System WITH (NOLOCK) ", null);
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

            #region 第二頁
            if (this.change)
            {
                if (this.rbDetail.Checked)
                {
                    this.ExcelMainData(excel.ActiveWorkbook.Worksheets[2], excel.ActiveWorkbook.Worksheets[3], excel.ActiveWorkbook.Worksheets[4], factory, style, this.nodist, this.count1, "Line Mapping", isDetail: true);
                    this.ExcelMainData(excel.ActiveWorkbook.Worksheets[5], excel.ActiveWorkbook.Worksheets[6], excel.ActiveWorkbook.Worksheets[7], factory, style, this.nodist2, this.count2, "Line Mapping", isDetail: true);
                    excel.ActiveWorkbook.Worksheets[14].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    excel.ActiveWorkbook.Worksheets[16].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    excel.ActiveWorkbook.Worksheets[17].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    excel.ActiveWorkbook.Worksheets[15].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }
                else
                {
                    this.ExcelMainData(excel.ActiveWorkbook.Worksheets[14], excel.ActiveWorkbook.Worksheets[3], excel.ActiveWorkbook.Worksheets[4], factory, style, this.nodist, this.count1, "Line Mapping");
                    this.ExcelMainData(excel.ActiveWorkbook.Worksheets[16], excel.ActiveWorkbook.Worksheets[6], excel.ActiveWorkbook.Worksheets[7], factory, style, this.nodist2, this.count2, "Line Mapping");
                    excel.ActiveWorkbook.Worksheets[2].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    excel.ActiveWorkbook.Worksheets[5].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    excel.ActiveWorkbook.Worksheets[17].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }
            }
            else
            {

                decimal currentOperators = this.masterData["CurrentOperators"] == null ? 0 : Convert.ToDecimal(this.masterData["CurrentOperators"]);
                if (this.rbDetail.Checked)
                {
                    this.ExcelMainData(excel.ActiveWorkbook.Worksheets[2], excel.ActiveWorkbook.Worksheets[3], excel.ActiveWorkbook.Worksheets[4], factory, style, this.nodist, currentOperators, "Line Mapping", isDetail: true);
                    excel.ActiveWorkbook.Worksheets[14].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    excel.ActiveWorkbook.Worksheets[16].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    excel.ActiveWorkbook.Worksheets[17].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    excel.ActiveWorkbook.Worksheets[15].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }
                else
                {
                    this.ExcelMainData(excel.ActiveWorkbook.Worksheets[14], excel.ActiveWorkbook.Worksheets[3], excel.ActiveWorkbook.Worksheets[4], factory, style, this.nodist, currentOperators, "Line Mapping");
                    excel.ActiveWorkbook.Worksheets[2].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    excel.ActiveWorkbook.Worksheets[16].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    excel.ActiveWorkbook.Worksheets[17].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }

                excel.ActiveWorkbook.Worksheets[5].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                excel.ActiveWorkbook.Worksheets[6].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                excel.ActiveWorkbook.Worksheets[7].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;

            }
            #endregion

            #region 第三頁
            if (this.changePPA)
            {
                this.ExcelMainData(excel.ActiveWorkbook.Worksheets[8], excel.ActiveWorkbook.Worksheets[9], excel.ActiveWorkbook.Worksheets[10], factory, style, this.nodistPPA, this.count1PPA, "PPA & non-sewing", true);
                this.ExcelMainData(excel.ActiveWorkbook.Worksheets[11], excel.ActiveWorkbook.Worksheets[12], excel.ActiveWorkbook.Worksheets[13], factory, style, this.nodist2PPA, this.count2PPA, "PPA & non-sewing", true);
            }
            else
            {
                if (this.rbDetail.Checked)
                {
                    this.ExcelMainData(excel.ActiveWorkbook.Worksheets[8], excel.ActiveWorkbook.Worksheets[9], excel.ActiveWorkbook.Worksheets[10], factory, style, this.nodistPPA, this.count1PPA, "PPA & non-sewing", true);
                }
                else
                {
                    this.ExcelMainData(excel.ActiveWorkbook.Worksheets[15], excel.ActiveWorkbook.Worksheets[9], excel.ActiveWorkbook.Worksheets[10], factory, style, this.nodistPPA, this.count1PPA, "PPA & non-sewing", true);
                    excel.ActiveWorkbook.Worksheets[8].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                }

                excel.ActiveWorkbook.Worksheets[11].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                excel.ActiveWorkbook.Worksheets[12].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                excel.ActiveWorkbook.Worksheets[13].Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            }
            #endregion

            // 寫此行目的是要將Excel畫面上顯示Copy給取消
            excel.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCopy;
            worksheet.Select();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("IE_P03_Print");
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

        private void AddLineMappingFormula(Microsoft.Office.Interop.Excel.Worksheet worksheet, int rownum)
        {
            int iSimplify = this.rbDetail.Checked ? 0 : 1;
            // Operation
            worksheet.Cells[rownum, 5 - iSimplify] = $"=IF(ISNA(VLOOKUP(AI{rownum},Operation,3,0)),\"\",VLOOKUP(AI{rownum},Operation,3,0))";
            worksheet.Cells[rownum, 20 - iSimplify] = $"=IF(ISNA(VLOOKUP(AJ{rownum},Operation,3,0)),\"\",VLOOKUP(AJ{rownum},Operation,3,0))";

            // GSD
            worksheet.Cells[rownum, 3] = $"=IF(ISNA(VLOOKUP(AI{rownum},Operation,9,0)),\"\",VLOOKUP(AI{rownum},Operation,9,0))";
            worksheet.Cells[rownum, 23] = $"=IF(ISNA(VLOOKUP(AJ{rownum},Operation,9,0)),\"\",VLOOKUP(AJ{rownum},Operation,9,0))";

            // TMS
            worksheet.Cells[rownum, 2] = $"=IF(ISNA(VLOOKUP(AI{rownum},Operation,10,0)),\"\",VLOOKUP(AI{rownum},Operation,10,0))";
            worksheet.Cells[rownum, 24] = $"=IF(ISNA(VLOOKUP(AJ{rownum},Operation,10,0)),\"\",VLOOKUP(AJ{rownum},Operation,10,0))";

            // ST/MC type
            worksheet.Cells[rownum, 10] = $"=IF(ISNA(VLOOKUP(AI{rownum},Operation,13,0)),\"\",IF(VLOOKUP(AI{rownum},Operation,13,0)=IF(ISNA(VLOOKUP(AI{rownum - 1},Operation,13,0)),\"\",VLOOKUP(AI{rownum - 1},Operation,13,0)),\"\",VLOOKUP(AI{rownum},Operation,13,0)))";
            worksheet.Cells[rownum, 17] = $"=IF(ISNA(VLOOKUP(AJ{rownum},Operation,13,0)),\"\",IF(VLOOKUP(AJ{rownum},Operation,13,0)=IF(ISNA(VLOOKUP(AJ{rownum - 1},Operation,13,0)),\"\",VLOOKUP(AJ{rownum - 1},Operation,13,0)),\"\",VLOOKUP(AJ{rownum},Operation,13,0)))";

            // Machine Group
            if (this.rbDetail.Checked)
            {
                worksheet.Cells[rownum, 12] = $"=IF(ISNA(VLOOKUP(AI{rownum},Operation,5,0)),\"\",IF(VLOOKUP(AI{rownum},Operation,5,0)=IF(ISNA(VLOOKUP(AI{rownum - 1},Operation,5,0)),\"\",VLOOKUP(AI{rownum - 1},Operation,5,0)),\"\",VLOOKUP(AI{rownum},Operation,5,0)))";
                worksheet.Cells[rownum, 16] = $"=IF(ISNA(VLOOKUP(AJ{rownum},Operation,5,0)),\"\",IF(VLOOKUP(AJ{rownum},Operation,5,0)=IF(ISNA(VLOOKUP(AJ{rownum - 1},Operation,5,0)),\"\",VLOOKUP(AJ{rownum - 1},Operation,5,0)),\"\",VLOOKUP(AJ{rownum},Operation,5,0)))";

            }

            // Attachment
            worksheet.Cells[rownum, 31] = $"=IF(OR(ISNA(VLOOKUP(AI{rownum},Operation,6,0)),J{rownum}=\"\"),\"\",IF(VLOOKUP(AI{rownum},Operation,6,0)=\"\",\"\",\"Attachment\"))";
            worksheet.Cells[rownum, 21] = $"=IF(OR(ISNA(VLOOKUP(AJ{rownum},Operation,6,0)),Q{rownum}=\"\"),\"\",IF(VLOOKUP(AJ{rownum},Operation,6,0)=\"\",\"\",\"Attachment\"))";

            // Template
            worksheet.Cells[rownum, 32] = $"=IF(OR(ISNA(VLOOKUP(AI{rownum},Operation,8,0)),J{rownum}=\"\"),\"\",IF(VLOOKUP(AI{rownum},Operation,8,0)=\"\",\"\",\"Template\"))";
            worksheet.Cells[rownum, 22] = $"=IF(OR(ISNA(VLOOKUP(AJ{rownum},Operation,8,0)),Q{rownum}=\"\"),\"\",IF(VLOOKUP(AJ{rownum},Operation,8,0)=\"\",\"\",\"Template\"))";

            // only Machine Type
            worksheet.Cells[rownum, 27] = $"=IF(ISNA(VLOOKUP(AI{rownum},Operation,13,0)),\"\",IF(VLOOKUP(AI{rownum},Operation,13,0)=IF(ISNA(VLOOKUP(AI{rownum - 1},Operation,13,0)),\"\",VLOOKUP(AI{rownum - 1},Operation,13,0)),\"\",VLOOKUP(AI{rownum},Operation,4,0)))";
            worksheet.Cells[rownum, 28] = $"=IF(ISNA(VLOOKUP(AJ{rownum},Operation,13,0)),\"\",IF(VLOOKUP(AJ{rownum},Operation,13,0)=IF(ISNA(VLOOKUP(AJ{rownum - 1},Operation,13,0)),\"\",VLOOKUP(AJ{rownum - 1},Operation,13,0)),\"\",VLOOKUP(AJ{rownum},Operation,4,0)))";

            // Machine Group
            worksheet.Cells[rownum, 29] = $"=IF(ISNA(VLOOKUP(AI{rownum},Operation,14,0)),\"\",IF(VLOOKUP(AI{rownum},Operation,14,0)=IF(ISNA(VLOOKUP(AI{rownum - 1},Operation,14,0)),\"\",VLOOKUP(AI{rownum - 1},Operation,14,0)),\"\",VLOOKUP(AI{rownum},Operation,5,0)))";
            worksheet.Cells[rownum, 30] = $"=IF(ISNA(VLOOKUP(AJ{rownum},Operation,14,0)),\"\",IF(VLOOKUP(AJ{rownum},Operation,14,0)=IF(ISNA(VLOOKUP(AJ{rownum - 1},Operation,14,0)),\"\",VLOOKUP(AJ{rownum - 1},Operation,14,0)),\"\",VLOOKUP(AJ{rownum},Operation,5,0)))";
        }

        private void ExcelMainData(Microsoft.Office.Interop.Excel.Worksheet worksheet, Microsoft.Office.Interop.Excel.Worksheet cycleTimeSheet, Microsoft.Office.Interop.Excel.Worksheet gcTimeSheet, string factory, string style, DataTable nodist, decimal currentOperators, string sheetName, bool showMachineType = false, bool isDetail = false)
        {
            #region 列印設置
            // 設置列印設置
            worksheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA4; // 設置紙張大小為 A4
            worksheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlPortrait; // 設置為縱向列印
            worksheet.PageSetup.Zoom = false; // 禁用默認縮放比例
            worksheet.PageSetup.FitToPagesWide = 1; // 將內容縮放以適合一頁寬
            worksheet.PageSetup.FitToPagesTall = false;
            #endregion
            #region 第二或三頁

            #region 固定資料

            // 左上表頭資料
            worksheet.Cells[1, 6] = factory;
            worksheet.Cells[5, 6] = MyUtility.Convert.GetString(this.masterData["SewingLineID"]);
            worksheet.Cells[7, 6] = style;
            worksheet.Cells[9, 6] = this.styleCPU;

            // 右下簽名位置
            worksheet.Cells[28, 4] = DateTime.Now.ToString("yyyy/MM/dd");
            worksheet.Cells[30, 4] = Env.User.UserName;

            // 左下表頭資料
            worksheet.Cells[36, 4] = this.masterData["Version"];
            worksheet.Cells[38, 4] = this.masterData["Workhour"];
            worksheet.Cells[40, 4] = currentOperators;
            #endregion
            string[] allMachine = this.operationCode.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["MachineTypeID"])).Select(s => s["MachineTypeID"].ToString()).Distinct().ToArray();
            var allMachineData = this.operationCode.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["MachineTypeID"]))
                .Select(s => new
                {
                    No = MyUtility.Convert.GetString(s["No"]),
                    MachineCount = MyUtility.Convert.GetBool(s["MachineCount"]),
                    MachineTypeID = MyUtility.Convert.GetString(s["MachineTypeID"]),
                    MasterPlusGroup = MyUtility.Convert.GetString(s["MasterPlusGroup"]),
                    Attachment = MyUtility.Convert.GetString(s["Attachment"]),
                    Template = MyUtility.Convert.GetString(s["Template"]),
                    SewingMachineAttachmentID = MyUtility.Convert.GetString(s["SewingMachineAttachmentID"]),
                });

            decimal chartDataEndRow = currentOperators + 1;
            #region 新增長條圖 2 GCtime chart

            // 新增長條圖
            Microsoft.Office.Interop.Excel.Worksheet chartData2 = gcTimeSheet;
            Microsoft.Office.Interop.Excel.Range chartRange2;
            object misValue2 = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.ChartObjects xlsCharts2 = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart2 = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts2.Add(378, 1082, 1000, 350);
            Microsoft.Office.Interop.Excel.Chart chartPage2 = myChart2.Chart;
            chartRange2 = chartData2.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            chartPage2.SetSourceData(chartRange2, misValue2);

            chartPage2.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

            // 新增折線圖
            Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection2 = chartPage2.SeriesCollection();
            Microsoft.Office.Interop.Excel.Series series2 = seriesCollection2.NewSeries();
            series2.Values = chartData2.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series2.XValues = chartData2.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series2.Name = "Total Cycle Time";

            // 折線圖的資料標籤不顯示
            series2.ApplyDataLabels(Microsoft.Office.Interop.Excel.XlDataLabelsType.xlDataLabelsShowNone, false, false);

            // 隱藏Sheet
            chartData2.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            #endregion

            #region 新增長條圖 1 CycleTime chart

            // 新增長條圖
            Microsoft.Office.Interop.Excel.Worksheet chartData = cycleTimeSheet;
            Microsoft.Office.Interop.Excel.Range chartRange;
            object misValue = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.ChartObjects xlsCharts = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlsCharts.Add(378, 718.5, 1000, 350);
            Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
            chartRange = chartData.get_Range("B1", string.Format("B{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            chartPage.SetSourceData(chartRange, misValue);

            chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;

            // 新增折線圖
            Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection = chartPage.SeriesCollection();
            Microsoft.Office.Interop.Excel.Series series1 = seriesCollection.NewSeries();
            series1.Values = chartData.get_Range("C2", string.Format("C{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series1.XValues = chartData.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series1.Name = "Takt time";
            series1.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

            // 新增折線圖
            Microsoft.Office.Interop.Excel.SeriesCollection seriesCollection_actTime = chartPage.SeriesCollection();
            Microsoft.Office.Interop.Excel.Series series1_actTime = seriesCollection_actTime.NewSeries();
            series1_actTime.Values = chartData.get_Range("D2", string.Format("D{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series1_actTime.XValues = chartData.get_Range("A2", string.Format("A{0}", MyUtility.Convert.GetString(chartDataEndRow)));
            series1_actTime.Name = "Act Cycle Time(average)";
            series1_actTime.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLine;

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

            Microsoft.Office.Interop.Excel.Range rngToCopy;
            #region Machine Type
            if (showMachineType)
            {
                var noPPA = this.machineTypeDT.AsEnumerable()
                            .Where(s => (s["IsShowinIEP03"].Equals(false) || s["IsHide"].Equals(true)) && s["OperationID"].ToString().Length >= 2 && !s["OperationID"].ToString().Substring(0, 2).EqualString("--"))
                            .Select(s => new
                            {
                                rn = s["rn"].ToString(),
                                MachineTypeID = s["MachineTypeID"].ToString(),
                                ArtWork = this.contentType == "A" ? MyUtility.Convert.GetString(s["Annotation"]).Trim() : MyUtility.Convert.GetString(s["DescEN"]).Trim(),
                                OperationID = s["OperationID"].ToString(),
                            })
                            .ToList();
                if (noPPA.Count() > 10)
                {
                    rngToCopy = worksheet.get_Range("A92:A92").EntireRow; // 選取要被複製的資料
                    for (int i = 10; i < noPPA.Count(); i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A93", Type.Missing).EntireRow; // 選擇要被貼上的位置
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                    }
                }

                int idxppa = 0;
                foreach (var item in noPPA)
                {
                    worksheet.Cells[93 + idxppa, 1] = item.OperationID;
                    worksheet.Cells[93 + idxppa, 5] = item.ArtWork;
                    worksheet.Cells[93 + idxppa, 10] = item.MachineTypeID;
                    worksheet.Cells[93 + idxppa, 4] = item.rn;
                    idxppa++;
                }
            }
            #endregion

            #region MACHINE
            decimal allct = Math.Ceiling((decimal)allMachine.Length / 3);

            // Machine table 只計算MachineCount有勾選的
            int machineCount = allMachineData
                .Where(o => o.MachineCount && !MyUtility.Check.Empty(o.MachineTypeID) && !MyUtility.Check.Empty(o.MasterPlusGroup) && !MyUtility.Check.Empty(o.No))
                .Select(o => new { o.MachineTypeID, o.MasterPlusGroup }).Distinct().Count();

            List<AttachmentData> tmp = new List<AttachmentData>();
            if (sheetName == "Line Mapping")
            {
                tmp = this.AttachmentDataList.Where(o => MyUtility.Convert.GetString(o.PPA) != "C" && !o.IsHide).ToList();
            }

            if (sheetName == "PPA & non-sewing")
            {
                tmp = this.AttachmentDataList.Where(o => MyUtility.Convert.GetString(o.PPA) == "C" && !o.IsHide).ToList();
            }

            var attachCount = tmp.Where(o => !string.IsNullOrEmpty(o.Attachment)).Select(o => new { o.No, o.STMC_Type, o.MachineGroup, o.Attachment, o.AttachmentPartID });
            var templateCount = tmp.Where(o => !string.IsNullOrEmpty(o.Template)).Select(o => new { o.No, o.STMC_Type, o.MachineGroup, o.Template, o.TemplateMoldID });
            int attachTemplateCount = attachCount.Count() + templateCount.Count();

            int copyCount = machineCount >= attachTemplateCount ? machineCount : attachTemplateCount;
            rngToCopy = worksheet.get_Range("A27:A27").EntireRow; // 選取要被複製的資料
            int maxhineEndRow = 27;
            for (int i = 0; i < copyCount - 1; i++)
            {
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A27", Type.Missing).EntireRow; // 選擇要被貼上的位置
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                maxhineEndRow++;
            }

            var mmData = allMachineData.Where(o => o.MachineCount && !MyUtility.Check.Empty(o.MachineTypeID) && !MyUtility.Check.Empty(o.MasterPlusGroup) && !MyUtility.Check.Empty(o.No))
                .Distinct() // [No.]+[ST/MC type]+[Machine group]+[Attachment]+[Part ID]+[Template]皆相同則只為一筆
                .GroupBy(o => new { o.MachineTypeID, o.MasterPlusGroup })
                .Select(o => new { o.Key.MachineTypeID, o.Key.MasterPlusGroup, Count = o.Count() });

            worksheet.Cells[24, 13] = $@"=SUM(M27:M{maxhineEndRow})";

            int surow = 0;
            foreach (var item in mmData)
            {
                worksheet.Cells[27 + surow, 10] = item.MachineTypeID;
                worksheet.Cells[27 + surow, 11] = item.MasterPlusGroup;
                worksheet.Cells[27 + surow, 13] = item.Count; // [No.]+[ST/MC type]+[Machine group]+[Attachment]+[Part ID]+[Template]皆相同則只為一筆
                surow++;
            }

            worksheet.Cells[24, 17] = attachCount.Count();
            worksheet.Cells[25, 17] = templateCount.Count();

            surow = 0;

            foreach (var item in attachCount)
            {
                worksheet.Cells[27 + surow, 16] = item.Attachment;
                worksheet.Cells[27 + surow, 17] = item.No;
                worksheet.Cells[27 + surow, 19] = item.AttachmentPartID;

                surow++;
            }

            foreach (var item in templateCount)
            {
                worksheet.Cells[27 + surow, 16] = item.Template;
                worksheet.Cells[27 + surow, 17] = item.No;
                worksheet.Cells[27 + surow, 19] = item.TemplateMoldID;

                surow++;
            }

            #endregion

            #region 預設站數為2站，當超過2站就要新增
            decimal no_count = MyUtility.Convert.GetDecimal(nodist.Rows.Count);
            int ttlLineRowCnt = MyUtility.Convert.GetInt(Math.Ceiling(no_count / 2));
            if (no_count > 2)
            {
                rngToCopy = worksheet.get_Range("A17:A21").EntireRow; // 選取要被複製的資料
                for (int j = 1; j < ttlLineRowCnt; j++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A17", Type.Missing).EntireRow; // 選擇要被貼上的位置
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing)); // 貼上
                }
            }
            #endregion

            int norow = 17 + (ttlLineRowCnt * 5) - 5; // No格子上的位置Excel Y軸
            int nocolumn = 9;

            // 計錄各站點公式放入GCtime sheet資料來源
            List<GCTimeChartData> list_GCTimeChartData = new List<GCTimeChartData>();
            List<CycleTimeChart> list_CycleTimeChart = new List<CycleTimeChart>();

            // Sheet3 => IsPPa = 1
            bool isSheet3 = showMachineType;

            #region U_Left字型列印
            if (this.display == "U_Left")
            {
                int maxct = 3;
                int di = nodist.Rows.Count;
                int addct = 0;
                bool flag = true;
                decimal dd = Math.Ceiling((decimal)di / 2);
                List<int> listMax_ct = new List<int>();
                for (int i = 0; i < dd; i++)
                {
                    int a = MyUtility.Convert.GetInt(nodist.Rows[i]["ct"]);
                    int d = 0;
                    if (di % 2 == 1 && flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (di % 2 == 1)
                        {
                            d = MyUtility.Convert.GetInt(nodist.Rows[di - i]["ct"]);
                        }
                        else
                        {
                            d = MyUtility.Convert.GetInt(nodist.Rows[di - 1 - i]["ct"]);
                        }
                    }

                    maxct = a > d ? a : d;
                    maxct = maxct > 3 ? maxct : 3;
                    listMax_ct.Add(maxct);
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                    for (int k = 3; k < maxct; k++)
                    {
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        if (isDetail)
                        {
                            worksheet.get_Range(string.Format("T{0}:V{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("E{0}:I{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        }
                        else
                        {
                            worksheet.get_Range(string.Format("S{0}:V{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("D{0}:I{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        }

                        addct++;
                    }

                    // 將公式填入對應的格子中
                    for (int q = 1; q <= maxct; q++)
                    {
                        this.AddLineMappingFormula(worksheet, norow + q + 1);
                    }

                    norow = norow - 5;
                    maxct = 3;
                }

                bool leftDirection = true;

                // excel 範圍別名宣告 公式使用 for MACHINE INVENTORY計算用
                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();
                worksheet.Names.Add("MachineINV1", worksheet.Range["AA17", "AA" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV2", worksheet.Range["AB17", "AB" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV3", worksheet.Range["AC17", "AC" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV4", worksheet.Range["AD17", "AD" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateL", worksheet.Range["AC19", "AE" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateR", worksheet.Range["U19", "V" + endRow], Type.Missing);
                worksheet.Names.Add("TtlTMS", $"=ROUND((SUM('{worksheet.Name}'!$B$17:$B${endRow})+SUM('{worksheet.Name}'!$X$17:$X${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("TtlGSD", $"=ROUND((SUM('{worksheet.Name}'!$C$17:$C${endRow})+SUM('{worksheet.Name}'!$W$17:$W${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("MaxTMS", $"=Max('{worksheet.Name}'!$B$17:$B${endRow},'{worksheet.Name}'!$X$17:$X${endRow})", Type.Missing);
                int m = 0;

                if (listMax_ct.Count == 0)
                {
                    listMax_ct.Add(0);
                }

                norow = MyUtility.Convert.GetInt(endRow) - (listMax_ct[0] + 1);
                foreach (DataRow nodr in nodist.Rows)
                {
                    list_GCTimeChartData.Add(new GCTimeChartData()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        TotalGSDFormula = $"='{worksheet.Name}'!{(leftDirection ? "C" : "W")}{norow}",
                        TotalCycleFormula = $"='{worksheet.Name}'!{(leftDirection ? "B" : "X")}{norow}",
                    });

                    list_CycleTimeChart.Add(new CycleTimeChart()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        ActCycleFormula = $"='{worksheet.Name}'!{(leftDirection ? "K" : "R")}{norow}",
                        ActCycleTime = MyUtility.Convert.GetString(nodr["ActCycleTime(average)"]),
                        TaktFormula = $"=E1",
                    });

                    if (leftDirection)
                    {
                        nocolumn = 10;
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        worksheet.Cells[norow, nocolumn - 4] = MyUtility.Convert.GetString(nodr["Name"]); // 名稱 : ID + LastName + FirstName 6
                        DataRow[] nodrs = this.operationCode.Select($@"no = '{MyUtility.Convert.GetString(nodr["No"])}' and IsHide = 0 and {(isSheet3 ? (this.isPPA ? "PPA = 'C' " : "PPA <> 'C' ") : "PPA <> 'C' ")}")
                                .OrderBy(x => x.Field<string>("OtherBy")).ThenBy(x => x.Field<int>("GroupKey")).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        int row = norow + ridx;
                        foreach (DataRow item in nodrs)
                        {
                            // 左邊排的No. Column:35
                            worksheet.Cells[norow + ridx, 35] = item["rn"].ToString();
                            ridx++;
                        }

                        m++;
                        if (m == dd)
                        {
                            leftDirection = false;
                            m--;
                            continue;
                        }

                        norow = norow - 5 - (listMax_ct[m] - 3);
                    }
                    else
                    {
                        nocolumn = 17;
                        worksheet.Cells[norow, nocolumn + 4] = MyUtility.Convert.GetString(nodr["Name"]); // 名稱 : ID + LastName + FirstName
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select($@"no = '{MyUtility.Convert.GetString(nodr["No"])}' and IsHide = 0 and {(isSheet3 ? (this.isPPA ? "PPA = 'C' " : "PPA <> 'C' ") : "PPA <> 'C' ")}")
                                .OrderBy(x => x.Field<string>("OtherBy")).ThenBy(x => x.Field<int>("GroupKey")).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        int row = norow + ridx;
                        foreach (DataRow item in nodrs)
                        {
                            // 右邊排的No.; Column:36
                            worksheet.Cells[norow + ridx, 36] = item["rn"].ToString();
                            ridx++;
                        }

                        norow = norow + 5 + (listMax_ct[m] - 3);
                        m--;
                    }
                }

            }
            #endregion

            #region U_Right字型列印
            if (this.display == "U_Right")
            {
                int maxct = 3;
                int di = nodist.Rows.Count;
                int addct = 0;
                bool flag = true;
                decimal dd = Math.Ceiling((decimal)di / 2);
                List<int> listMax_ct = new List<int>();
                for (int i = 0; i < dd; i++)
                {
                    int a = MyUtility.Convert.GetInt(nodist.Rows[i]["ct"]);
                    int d = 0;
                    if (di % 2 == 1 && flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (di % 2 == 1)
                        {
                            d = MyUtility.Convert.GetInt(nodist.Rows[di - i]["ct"]);
                        }
                        else
                        {
                            d = MyUtility.Convert.GetInt(nodist.Rows[di - 1 - i]["ct"]);
                        }
                    }

                    maxct = a > d ? a : d;
                    maxct = maxct > 3 ? maxct : 3;
                    listMax_ct.Add(maxct);
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                    for (int k = 3; k < maxct; k++)
                    {
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        if (isDetail)
                        {
                            worksheet.get_Range(string.Format("T{0}:V{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("E{0}:I{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        }
                        else
                        {
                            worksheet.get_Range(string.Format("S{0}:V{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                            worksheet.get_Range(string.Format("D{0}:I{0}", MyUtility.Convert.GetString(norow + k))).Merge(false); // 合併儲存格
                        }

                        addct++;
                    }

                    // 將公式填入對應的格子中
                    for (int q = 1; q <= maxct; q++)
                    {
                        this.AddLineMappingFormula(worksheet, norow + q + 1);
                    }

                    norow = norow - 5;
                    maxct = 3;
                }

                bool rightDirection = true;

                // excel 範圍別名宣告 公式使用 for MACHINE INVENTORY計算用
                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();
                worksheet.Names.Add("MachineINV1", worksheet.Range["AA17", "AA" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV2", worksheet.Range["AB17", "AB" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV3", worksheet.Range["AC17", "AC" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV4", worksheet.Range["AD17", "AD" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateL", worksheet.Range["AC19", "AE" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateR", worksheet.Range["U19", "V" + endRow], Type.Missing);
                worksheet.Names.Add("TtlTMS", $"=ROUND((SUM('{worksheet.Name}'!$B$17:$B${endRow})+SUM('{worksheet.Name}'!$X$17:$X${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("TtlGSD", $"=ROUND((SUM('{worksheet.Name}'!$C$17:$C${endRow})+SUM('{worksheet.Name}'!$W$17:$W${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("MaxTMS", $"=Max('{worksheet.Name}'!$B$17:$B${endRow},'{worksheet.Name}'!$X$17:$X${endRow})", Type.Missing);
                int m = 0;

                if (listMax_ct.Count == 0)
                {
                    listMax_ct.Add(0);
                }

                norow = MyUtility.Convert.GetInt(endRow) - (listMax_ct[0] + 1);
                foreach (DataRow nodr in nodist.Rows)
                {
                    list_GCTimeChartData.Add(new GCTimeChartData()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        TotalGSDFormula = $"='{worksheet.Name}'!{(rightDirection ? "W" : "C")}{norow}",
                        TotalCycleFormula = $"='{worksheet.Name}'!{(rightDirection ? "X" : "B")}{norow}",
                    });

                    list_CycleTimeChart.Add(new CycleTimeChart()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        ActCycleFormula = $"='{worksheet.Name}'!{(rightDirection ? "R" : "K")}{norow}",
                        ActCycleTime = MyUtility.Convert.GetString(nodr["ActCycleTime(average)"]),
                        TaktFormula = $"=E1",
                    });

                    if (rightDirection)
                    {
                        nocolumn = 17;
                        worksheet.Cells[norow, nocolumn + 4] = MyUtility.Convert.GetString(nodr["Name"]); // 名稱 : ID + LastName + FirstName
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select($@"no = '{MyUtility.Convert.GetString(nodr["No"])}' and IsHide = 0 and {(isSheet3 ? (this.isPPA ? "PPA = 'C' " : "PPA <> 'C' ") : "PPA <> 'C' ")}")
                                    .OrderBy(x => x.Field<string>("OtherBy")).ThenBy(x => x.Field<int>("GroupKey")).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        int row = norow + ridx;
                        foreach (DataRow item in nodrs)
                        {
                            // 右邊排的No.; Column:36
                            worksheet.Cells[norow + ridx, 36] = item["rn"].ToString();
                            ridx++;
                        }

                        m++;
                        if (m == dd)
                        {
                            rightDirection = false;
                            m--;
                            continue;
                        }

                        norow = norow - 5 - (listMax_ct[m] - 3);
                    }
                    else
                    {
                        nocolumn = 10;
                        worksheet.Cells[norow, nocolumn - 4] = MyUtility.Convert.GetString(nodr["Name"]); // 名稱 : ID + LastName + FirstName
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select($@"no = '{MyUtility.Convert.GetString(nodr["No"])}' and IsHide = 0 and {(isSheet3 ? (this.isPPA ? "PPA = 'C' " : "PPA <> 'C' ") : "PPA <> 'C' ")}")
                                    .OrderBy(x => x.Field<string>("OtherBy")).ThenBy(x => x.Field<int>("GroupKey")).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        int row = norow + ridx;
                        foreach (DataRow item in nodrs)
                        {
                            // 左邊排的No. Column:35
                            worksheet.Cells[norow + ridx, 35] = item["rn"].ToString();
                            ridx++;
                        }

                        norow = norow + 5 + (listMax_ct[m] - 3);
                        m--;
                    }
                }
            }
            #endregion

            #region Z_Left字型列印
            if (this.display == "Z_Left")
            {
                int maxct = 3;
                int ct = 0;
                int addct = 0;
                int indx = 1;
                List<int> listMax_ct = new List<int>();
                for (int l = 0; l < nodist.Rows.Count + 1; l++)
                {
                    if (l < nodist.Rows.Count)
                    {
                        maxct = MyUtility.Convert.GetInt(nodist.Rows[l]["ct"]) > maxct ? MyUtility.Convert.GetInt(nodist.Rows[l]["ct"]) : maxct;
                    }

                    ct++;
                    if (ct == 2)
                    {
                        listMax_ct.Add(maxct);
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                        for (int i = 3; i < maxct; i++)
                        {
                            rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                            if (isDetail)
                            {
                                worksheet.get_Range(string.Format("T{0}:V{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("E{0}:I{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                            }
                            else
                            {
                                worksheet.get_Range(string.Format("S{0}:V{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("D{0}:I{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                            }
                            addct++;
                        }

                        // 將公式填入對應的格子中
                        for (int q = 1; q <= maxct; q++)
                        {
                            this.AddLineMappingFormula(worksheet, norow + q + 1);
                        }

                        norow = norow - 5;
                        ct = 0;
                        maxct = 3;
                    }
                }

                // excel 範圍別名宣告 公式使用 for MACHINE INVENTORY計算用
                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();
                worksheet.Names.Add("MachineINV1", worksheet.Range["AA17", "AA" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV2", worksheet.Range["AB17", "AB" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV3", worksheet.Range["AC17", "AC" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV4", worksheet.Range["AD17", "AD" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateL", worksheet.Range["AC19", "AE" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateR", worksheet.Range["U19", "V" + endRow], Type.Missing);
                worksheet.Names.Add("TtlTMS", $"=ROUND((SUM('{worksheet.Name}'!$B$17:$B${endRow})+SUM('{worksheet.Name}'!$X$17:$X${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("TtlGSD", $"=ROUND((SUM('{worksheet.Name}'!$C$17:$C${endRow})+SUM('{worksheet.Name}'!$W$17:$W${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("MaxTMS", $"=Max('{worksheet.Name}'!$B$17:$B${endRow},'{worksheet.Name}'!$X$17:$X${endRow})", Type.Missing);

                int leftright_count = 2;
                bool leftDirection = true;
                indx = 0;
                norow = MyUtility.Convert.GetInt(endRow) + 1;
                foreach (DataRow nodr in nodist.Rows)
                {
                    if (nodist.Rows.IndexOf(nodr) % 2 == 0)
                    {
                        norow = norow - (listMax_ct[indx] + 2);
                        indx++;
                    }

                    list_GCTimeChartData.Add(new GCTimeChartData()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        TotalGSDFormula = $"='{worksheet.Name}'!{(leftDirection ? "C" : "W")}{norow}",
                        TotalCycleFormula = $"='{worksheet.Name}'!{(leftDirection ? "B" : "X")}{norow}",
                    });

                    list_CycleTimeChart.Add(new CycleTimeChart()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        ActCycleFormula = $"='{worksheet.Name}'!{(leftDirection ? "K" : "R")}{norow}",
                        ActCycleTime = MyUtility.Convert.GetString(nodr["ActCycleTime(average)"]),
                        TaktFormula = $"=E1",
                    });

                    if (leftDirection)
                    {
                        nocolumn = 10;
                        worksheet.Cells[norow, nocolumn - 4] = MyUtility.Convert.GetString(nodr["Name"]); // 名稱 : ID + LastName + FirstName
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select($@"no = '{MyUtility.Convert.GetString(nodr["No"])}' and IsHide = 0 and {(isSheet3 ? (this.isPPA ? "PPA = 'C' " : "PPA <> 'C' ") : "PPA <> 'C' ")}")
                                    .OrderBy(x => x.Field<string>("OtherBy")).ThenBy(x => x.Field<int>("GroupKey")).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        foreach (DataRow item in nodrs)
                        {
                            // 左邊排的No. Column:35
                            worksheet.Cells[norow + ridx, 35] = item["rn"].ToString();
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
                        nocolumn = 17;
                        worksheet.Cells[norow, nocolumn + 4] = MyUtility.Convert.GetString(nodr["Name"]); // 名稱 : ID + LastName + FirstName
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]); // Ryan
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select($@"no = '{MyUtility.Convert.GetString(nodr["No"])}' and IsHide = 0 and {(isSheet3 ? (this.isPPA ? "PPA = 'C' " : "PPA <> 'C' ") : "PPA <> 'C' ")}")
                                    .OrderBy(x => x.Field<string>("OtherBy")).ThenBy(x => x.Field<int>("GroupKey")).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        foreach (DataRow item in nodrs)
                        {
                            // 右邊排的No.; Column:36
                            worksheet.Cells[norow + ridx, 36] = item["rn"].ToString();
                            ridx++;
                        }

                        leftright_count++;
                        if (leftright_count > 2)
                        {
                            leftright_count = 1;
                            leftDirection = true;
                        }
                    }
                }
            }
            #endregion

            #region Z_Right字型列印
            if (this.display == "Z_Right")
            {
                int maxct = 3;
                int ct = 0;
                int addct = 0;
                int indx = 1;
                List<int> listMax_ct = new List<int>();
                for (int l = 0; l < nodist.Rows.Count + 1; l++)
                {
                    if (l < nodist.Rows.Count)
                    {
                        maxct = MyUtility.Convert.GetInt(nodist.Rows[l]["ct"]) > maxct ? MyUtility.Convert.GetInt(nodist.Rows[l]["ct"]) : maxct;
                    }

                    ct++;
                    if (ct == 2)
                    {
                        listMax_ct.Add(maxct);
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(norow + 3)), Type.Missing).EntireRow;
                        for (int i = 3; i < maxct; i++)
                        {
                            rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                            if (isDetail)
                            {
                                worksheet.get_Range(string.Format("T{0}:V{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("E{0}:I{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                            }
                            else
                            {
                                worksheet.get_Range(string.Format("S{0}:V{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                                worksheet.get_Range(string.Format("D{0}:I{0}", MyUtility.Convert.GetString(norow + i))).Merge(false); // 合併儲存格
                            }
                            addct++;
                        }

                        // 將公式填入對應的格子中
                        for (int q = 1; q <= maxct; q++)
                        {
                            this.AddLineMappingFormula(worksheet, norow + q + 1);
                        }

                        norow = norow - 5;
                        ct = 0;
                        maxct = 3;
                    }
                }

                // excel 範圍別名宣告 公式使用 for MACHINE INVENTORY計算用
                string endRow = (16 + (ttlLineRowCnt * 5) + addct).ToString();
                worksheet.Names.Add("MachineINV1", worksheet.Range["AA17", "AA" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV2", worksheet.Range["AB17", "AB" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV3", worksheet.Range["AC17", "AC" + endRow], Type.Missing);
                worksheet.Names.Add("MachineINV4", worksheet.Range["AD17", "AD" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateL", worksheet.Range["AC19", "AE" + endRow], Type.Missing);
                worksheet.Names.Add("MachineAttachmentTemplateR", worksheet.Range["U19", "V" + endRow], Type.Missing);
                worksheet.Names.Add("TtlTMS", $"=ROUND((SUM('{worksheet.Name}'!$B$17:$B${endRow})+SUM('{worksheet.Name}'!$X$17:$X${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("TtlGSD", $"=ROUND((SUM('{worksheet.Name}'!$C$17:$C${endRow})+SUM('{worksheet.Name}'!$W$17:$W${endRow}))/2,0)", Type.Missing);
                worksheet.Names.Add("MaxTMS", $"=Max('{worksheet.Name}'!$B$17:$B${endRow},'{worksheet.Name}'!$X$17:$X${endRow})", Type.Missing);

                int leftright_count = 2;
                bool leftDirection = false;
                indx = 0;
                norow = MyUtility.Convert.GetInt(endRow) + 1;
                foreach (DataRow nodr in nodist.Rows)
                {
                    if (nodist.Rows.IndexOf(nodr) % 2 == 0)
                    {
                        norow = norow - (listMax_ct[indx] + 2);
                        indx++;
                    }

                    list_GCTimeChartData.Add(new GCTimeChartData()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        TotalGSDFormula = $"='{worksheet.Name}'!{(leftDirection ? "C" : "W")}{norow}",
                        TotalCycleFormula = $"='{worksheet.Name}'!{(leftDirection ? "B" : "X")}{norow}",
                    });

                    list_CycleTimeChart.Add(new CycleTimeChart()
                    {
                        OperatorNo = MyUtility.Convert.GetString(nodr["No"]),
                        ActCycleFormula = $"='{worksheet.Name}'!{(leftDirection ? "K" : "R")}{norow}",
                        ActCycleTime = MyUtility.Convert.GetString(nodr["ActCycleTime(average)"]),
                        TaktFormula = $"=E1",
                    });

                    if (leftDirection)
                    {
                        nocolumn = 10;
                        worksheet.Cells[norow, nocolumn - 4] = MyUtility.Convert.GetString(nodr["Name"]); // 名稱 : ID + LastName + FirstName
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select($@"no = '{MyUtility.Convert.GetString(nodr["No"])}' and IsHide = 0 and {(isSheet3 ? (this.isPPA ? "PPA = 'C' " : "PPA <> 'C' ") : "PPA <> 'C' ")}")
                                    .OrderBy(x => x.Field<string>("OtherBy")).ThenBy(x => x.Field<int>("GroupKey")).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        foreach (DataRow item in nodrs)
                        {
                            // 左邊排的No. Column:35
                            worksheet.Cells[norow + ridx, 35] = item["rn"].ToString();
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
                        nocolumn = 17;
                        worksheet.Cells[norow, nocolumn + 4] = MyUtility.Convert.GetString(nodr["Name"]); // 名稱 : ID + LastName + FirstName
                        worksheet.Cells[norow, nocolumn] = MyUtility.Convert.GetString(nodr["No"]);
                        worksheet.Cells[norow, nocolumn + 1] = MyUtility.Convert.GetString(nodr["ActCycle"]);
                        DataRow[] nodrs = this.operationCode.Select($@"no = '{MyUtility.Convert.GetString(nodr["No"])}' and IsHide = 0 and {(isSheet3 ? (this.isPPA ? "PPA = 'C' " : "PPA <> 'C' ") : "PPA <> 'C' ")}")
                                    .OrderBy(x => x.Field<string>("OtherBy")).ThenBy(x => x.Field<int>("GroupKey")).ToArray();
                        int ridx = 2;
                        string machinetype = string.Empty;
                        string machinetypeL = string.Empty;
                        foreach (DataRow item in nodrs)
                        {
                            // 右邊排的No.; Column:36
                            worksheet.Cells[norow + ridx, 36] = item["rn"].ToString();
                            ridx++;
                        }

                        leftright_count++;
                        if (leftright_count > 2)
                        {
                            leftright_count = 1;
                            leftDirection = true;
                        }
                    }
                }
            }
            #endregion

            #region 長條圖資料

            // 填act Cycle Time
            worksheet = cycleTimeSheet;
            int intRowsStart = 2;
            object[,] objArray = new object[1, 4];
            foreach (CycleTimeChart dr in list_CycleTimeChart)
            {
                objArray[0, 0] = dr.OperatorNo;
                objArray[0, 1] = dr.ActCycleFormula;
                objArray[0, 2] = dr.TaktFormula;
                objArray[0, 3] = dr.ActCycleTime;
                worksheet.Range[string.Format("A{0}:D{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            // 填 TotalGSD
            worksheet = gcTimeSheet;
            intRowsStart = 2;
            objArray = new object[1, 3];
            foreach (GCTimeChartData dr in list_GCTimeChartData)
            {
                objArray[0, 0] = dr.OperatorNo;
                objArray[0, 1] = dr.TotalGSDFormula;
                objArray[0, 2] = dr.TotalCycleFormula;
                worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            #endregion

            #endregion
        }

#pragma warning disable SA1516 // Elements should be separated by blank line
        private class AttachmentData
        {
            public string No { get; set; }
            public string STMC_Type { get; set; }
            public string MachineGroup { get; set; }
            public string AttachmentCount { get; set; }
            public string Attachment { get; set; }
            public string AttachmentPartID { get; set; }
            public string Template { get; set; }
            public string TemplateMoldID { get; set; }
            public string PPA { get; set; }
            public bool IsHide { get; set; }
        }
#pragma warning restore SA1516 // Elements should be separated by blank line

        private class GCTimeChartData
        {
            private string operatorNo;
            private string totalGSDFormula;
            private string totalCycleFormula;

            public string OperatorNo
            {
                get
                {
                    return this.operatorNo;
                }

                set
                {
                    this.operatorNo = value;
                }
            }

            public string TotalGSDFormula
            {
                get
                {
                    return this.totalGSDFormula;
                }

                set
                {
                    this.totalGSDFormula = value;
                }
            }

            public string TotalCycleFormula
            {
                get
                {
                    return this.totalCycleFormula;
                }

                set
                {
                    this.totalCycleFormula = value;
                }
            }
        }

        private class CycleTimeChart
        {
            private string operatorNo;
            private string actCycleFormula;
            private string actCycleTime;
            private string taktFormula;

            public string OperatorNo
            {
                get
                {
                    return this.operatorNo;
                }

                set
                {
                    this.operatorNo = value;
                }
            }

            public string ActCycleFormula
            {
                get
                {
                    return this.actCycleFormula;
                }

                set
                {
                    this.actCycleFormula = value;
                }
            }

            public string ActCycleTime
            {
                get
                {
                    return this.actCycleTime;
                }

                set
                {
                    this.actCycleTime = value;
                }
            }

            public string TaktFormula
            {
                get
                {
                    return this.taktFormula;
                }

                set
                {
                    this.taktFormula = value;
                }
            }
        }
    }
}
