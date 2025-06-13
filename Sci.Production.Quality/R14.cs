using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Ict.Win.UI.DataGridView;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R14 : Win.Tems.PrintForm
    {
        private List<string> sqlWherelist;
        private List<SqlParameter> lisSqlParameter;
        private string strSQLWhere = string.Empty;
        private DataTable printTable;

        /// <inheritdoc/>
        public R14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboShift.SetDataSource();
            this.comboMDivision.SetDefalutIndex(true);
            this.comboFactory.SetDataSource(this.comboMDivision.Text);
            this.radioSummary.Checked = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboFactory.Text = Env.User.Factory;
            this.comboShift.Text = this.ChangShift(Env.User.Factory);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.strSQLWhere = string.Empty;
            this.sqlWherelist = new List<string>();
            this.lisSqlParameter = new List<SqlParameter>();

            if (MyUtility.Check.Empty(this.dateLastInsDate.Value1) &&
                MyUtility.Check.Empty(this.dateLastInsDate.Value2) &&
                MyUtility.Check.Empty(this.txtInsCutRef.Text) &&
                MyUtility.Check.Empty(this.txtWorkOrderCuRef.Text) &&
                MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("[Last Ins. Date], [Work Order CutRef#], [Ins. CutRef#], [SP#] cannot be all empty.");
                return false;
            }

            if (!MyUtility.Check.Empty(this.dateLastInsDate.Value1) &&
                !MyUtility.Check.Empty(this.dateLastInsDate.Value2))
            {
                this.sqlWherelist.Add("s.LastInspectionDate between @date1 and @date2");
                this.lisSqlParameter.Add(new SqlParameter("@date1", this.dateLastInsDate.Value1));
                this.lisSqlParameter.Add(new SqlParameter("@date2", this.dateLastInsDate.Value2));
            }

            if (!MyUtility.Check.Empty(this.txtInsCutRef.Text))
            {
                this.sqlWherelist.Add("si.CutRef = @InsCutRef");
                this.lisSqlParameter.Add(new SqlParameter("@InsCutRef", this.txtInsCutRef.Text));
            }

            if (!MyUtility.Check.Empty(this.txtWorkOrderCuRef.Text))
            {
                this.sqlWherelist.Add("so.CutRef = @WorkOrderCutRef");
                this.lisSqlParameter.Add(new SqlParameter("@WorkOrderCutRef", this.txtWorkOrderCuRef.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                this.sqlWherelist.Add(@"exists(select 1
                                       from SciProduction_WorkOrderForOutput_Distribute swd
			                           where swd.WorkOrderForOutputUkey=pms_wo.Ukey and swd.orderid = @SP)
                                       ");
                this.lisSqlParameter.Add(new SqlParameter("@SP", this.txtSP.Text));
                this.sqlWherelist.Add($" sp.val like '{this.txtSP.Text}%' ");
            }

            if (!MyUtility.Check.Empty(this.comboShift.SelectedValue))
            {
                if (this.radioSummary.Checked)
                {
                    this.sqlWherelist.Add("siidata.Shift = @Shift");
                }
                else
                {
                    this.sqlWherelist.Add("sii.Shift = @Shift");
                }
                this.lisSqlParameter.Add(new SqlParameter("@Shift", this.comboShift.SelectedValue));
            }

            if (!MyUtility.Check.Empty(this.comboMDivision.SelectedValue))
            {
                this.sqlWherelist.Add("pms_wo.MDivisionId = @MDivisionId");
                this.lisSqlParameter.Add(new SqlParameter("@MDivisionId", this.comboMDivision.SelectedValue));
            }

            if (!MyUtility.Check.Empty(this.comboFactory.SelectedValue))
            {
                this.sqlWherelist.Add("pms_wo.FactoryID = @FactoryID");
                this.lisSqlParameter.Add(new SqlParameter("@FactoryID", this.comboFactory.SelectedValue));
            }

            this.strSQLWhere = string.Join(" and ", this.sqlWherelist);
            if (this.sqlWherelist.Count != 0)
            {
                this.strSQLWhere = " where " + this.strSQLWhere;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd = string.Empty;
            if (this.radioSummary.Checked)
            {
                sqlcmd = $@"select 
                [Factory] = pms_wo.FactoryID,
                [Cut#] = Isnull(m.CutNo,'N/A'),
                [OuiCutRef] = oui.[ouiCutRef],
                [SubCutRef] = si.CutRef,
                [Activate] = case when soc.[count] > sic.[count] then 'Combine'
			                        when soc.[count] < sic.[count]  then 'Separate' 
				                    else '' end,
                [InspectDate] = isnull( siidata.MaxEditDate,siidata.MaxAddDate),
                [RFT] = RFT.val,
                [Shift] = sii.Shift,
                [SP] = SP.val,
                [Style#] = sty.StyleID,
                [Marker#] = m.MarkerNo,
                [Fabric Description] = pms_f.[Description],
                [Color] = color.val,
                [Size] = size.val,
                [Spreading Machine#] = si.SpreadingNoID,
                [Fabric Roll] = FabricRoll.val,
                [MarkerCheck] = isnull(ip_mc.Result,'N/A'),
                [HandFeel] = isnull(ip_hf.Result,'N/A'),
                [WrongFaceSide] = isnull(ip_ws.Result,'N/A'),
                [ShortWidth] = isnull(ip_sw.Result,'N/A'),
                [MachineTension] = isnull(ip_mt.Result,'N/A'),
                [FabricColor] = isnull(ip_fc.Result,'N/A'),
                [InsertPaper] = isnull(ip_ip.Result,'N/A'),
                [FabricDefect] = isnull(ip_fd.Result,'N/A'),
                [FabricHeight] = isnull(ip_fh.Result,'N/A'),
                [FabricRelaxation] = isnull(ip_fr.Result,'N/A'),
                [Bow/Skew] = isnull(ip_bs.Result,'N/A'),
                [Check grouping shade/roll to roll shading] = isnull(ip_cr.Result,'N/A'),
                [End aligment] = isnull(ip_ea.Result,'N/A'),
                [Mold] = isnull(ip_m.Result,'N/A'),
                [Trim Card with Maker(color/Item fabric)] = isnull(ip_tc.Result,'N/A'),
                [RejectPointQty] = siifc.[count],
                [Inspector] =p.Name,
                [StartingTime] = siidata.MaxAddDate,
                [Remark] = si.Remark,
                [InspectedTime] = DATEDIFF(second,siidata.minAddDate,isnull(isnull(MinEditDate,MaxEditDate),isnull(MaxAddDate,isnull(MinEditDate,MaxEditDate))))
                from SpreadingInspection s 
                left join SpreadingInspection_InsCutRef si with(nolock) on s.ID = si.ID
                left join SpreadingInspection_OriCutRef so with(nolock) on s.id = so.id
                left join SciProduction_WorkOrderForOutput pms_wo with(nolock)  on so.WorkOrderForOutputUkey = pms_wo.Ukey
                left join SciProduction_Fabric pms_f with(nolock) on pms_wo.SCIRefno = pms_f.SCIRefno

                outer apply(select top 1 * from SciProduction_WorkOrderForOutput where so.WorkOrderForOutputUkey = Ukey ) m
                outer apply(select top 1 * from SciProduction_Orders where pms_wo.ID = ID ) sty
                outer apply(select [count] = count(*) from SpreadingInspection_OriCutRef where  so.ID = ID ) soc
                outer apply(select [count] = count(*) from SpreadingInspection_InsCutRef where  si.ID = ID  ) sic
                outer apply(select [count] = Count(*) from SpreadingInspection_InsCutRef_Inspection where si.Ukey = SpreadingInspectionInsCutRefUkey and Result='Fail') siifc
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Marker Check')ip_mc
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Hand Feel')ip_hf
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Face Side')ip_ws
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Uneven Width')ip_sw
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Machine Tension')ip_mt
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Fabric Color')ip_fc
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Insert Paper')ip_ip
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Fabric Defect')ip_fd
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Fabric Height')ip_fh
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Fabric Relaxation')ip_fr
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Bow/Skew')ip_bs
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Check grouping shade/roll to roll shading')ip_cr
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'End aligment')ip_ea
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Mold')ip_m
                outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Trim Card with Maker(color/Item fabric)')ip_tc
                outer apply
                (
	                select 
	                [MinAddDate] = MIN(AddDate),
	                [MaxAddDate] = MAX(AddDate),
	                [MaxEditDate] = Max(EditDate),
	                [MinEditDate] = Min(EditDate),
	                [Shift]
	                from SpreadingInspection_InsCutRef_Inspection 
	                where SpreadingInspectionInsCutRefUkey = si.Ukey
	                group by Shift
                )siidata
                outer apply
                (
	                select 
	                AddName,
	                EditName
	                from SpreadingInspection_InsCutRef_Inspection 
	                where isnull(EditDate,AddDate) = isnull(siidata.MaxEditDate,siidata. MaxAddDate)
                )siiMaxName
                outer apply
                (

					select [Name] = stuff((
	                select concat(char(10),tmp.Name) from
	                (
		                select 
						Name
						from Pass1 p 
						inner join SpreadingInspection_InsCutRef_Inspection s on p.ID =s.EditName 
						where SpreadingInspectionInsCutRefUkey = si.Ukey and s.EditName != ''
						union
						select 
						 Name
						from Pass1 p 
						inner join SpreadingInspection_InsCutRef_Inspection s on p.ID =s.AddName 
						where SpreadingInspectionInsCutRefUkey = si.Ukey and s.AddName != ''

	                ) tmp for xml path('')),1,1,'')

                )p
                outer apply
                (
	                select [ouiCutRef] = stuff((
	                select concat('/',tmp.CutRef) from
	                (
		                select distinct CutRef
		                from SpreadingInspection_OriCutRef
                        where id = so.id
	                ) tmp for xml path('')),1,1,'')
                )oui
                outer apply
                (
	                select val = ROUND(CAST(count(*) as float) / CAST( 10 as float),2) 
	                from SpreadingInspection_InsCutRef_Inspection
	                where SpreadingInspectionInsCutRefUkey = si.Ukey and Result = 'Pass'
                )RFT
                outer apply
                (
	                SELECT [Shift] = stuff(
	                (
	                select concat('/',tmp.[Shift])
	                FROM
	                (
		                SELECT  [Shift] 
		                FROM SpreadingInspection_InsCutRef_Inspection  
		                where SpreadingInspectionInsCutRefUkey = si.Ukey
                        group by [Shift]
	                )
	                tmp for xml path('')),1,1,'')
                )sii
                outer apply
                (
	                select val = dbo.SciProduction_GetSinglelineSP(
	                (
                        select distinct S.OrderID
                        from SciProduction_WorkOrderForOutput_Distribute S with(nolock)
                        where S.OrderID!='EXCESS'
                        AND WorkOrderForOutputUkey in (select WorkOrderForOutputUkey from SpreadingInspection_OriCutRef sioc where sioc.id = si.id )
                        order by OrderID
                        for XML RAW)
	                )
                )SP
                outer apply
                (
	                select val = stuff(
	                (
		                select concat('/',tmp.Colorid) 
		                from
		                (
			                select distinct w.Colorid 
			                from SciProduction_WorkOrderForOutput w
			                where Ukey IN (select WorkOrderForOutputUkey from SpreadingInspection_OriCutRef sioc where sioc.id = si.id)
	                )
	                tmp for xml path('')),1,1,'')
                )color
                outer apply
                (
	                select val = stuff(
	                (
		                select  concat(char(10),tmp.DataList) 
		                from
		                (
			                select distinct [DataList] = concat(S.SizeCode ,'/', S.Qty)
			                from SciProduction_WorkOrderForOutput_SizeRatio S
			                where  WorkOrderForOutputUkey IN (select WorkOrderForOutputUkey from SpreadingInspection_OriCutRef sioc where sioc.id = si.id)
	                ) 
	                tmp for xml path('')),1,1,'')
                )size
                outer apply
                (
                    select val = stuff((select concat(';','(' + tmp.Roll + ')(' + tmp.Dyelot + ')(' + Seq1 + ' ' + tmp.Seq2 + ')')
                    from
                    (
                        select sif.Roll, sid.Dyelot, sid.Seq1, sid.Seq2 from SpreadingInspection_InsCutRef_Fabric sif
                        Left join SciProduction_Issue_Detail sid on sid.Ukey = sif.IssueDetailUkey
                        where sif.SpreadingInspectionInsCutRefUkey = si.Ukey
                    ) 
                    tmp for xml path('')),1,1,'')
                )FabricRoll
                {this.strSQLWhere}
                Group by pms_wo.FactoryID,m.CutNo,oui.[ouiCutRef],si.CutRef,soc.[count],sic.[count],RFT.val,sii.Shift,SP.val,sty.StyleID,m.MarkerNo,pms_f.[Description]
                ,color.val,size.val,si.[SpreadingNoID],FabricRoll.val,ip_mc.Result,ip_hf.Result,ip_ws.Result,ip_sw.Result,ip_mt.Result
                ,ip_fc.Result,ip_ip.Result,ip_fd.Result,ip_fh.Result,ip_fr.Result,ip_bs.Result,ip_cr.Result,ip_ea.Result,ip_m.Result,ip_tc.Result,siifc.count,siidata.MaxAddDate,siidata.MinEditDate
                ,si.Remark,siidata.MaxEditDate,siidata.MinAddDate,si.ukey,p.Name
                ";
            }
            else if (this.radioDetail.Checked)
            {
                sqlcmd = $@"select 
                [Factory] = pms_wo.FactoryID,
				[Cut#] = isNull(m.[CutNo], 'N/A'),
                [OuiCutRef] = oui.[ouiCutRef],
                [SubCutRef] = si.CutRef,
                [Activate] = case when soc.[count] > sic.[count] then 'Combine'
			                    when soc.[count] < sic.[count]  then 'Separate' 
				                else '' end,
                [InspectDate] = iif(sii.Result = '',null,convert(date,IsNULL(sii.EditDate,sii.AddDate))),
                [RFT] = RFT.Per ,
                [Shift] = sii.[Shift],
                [SP] = SP.val ,
                [Style#] = sty.StyleID,
                [Marker#] = m.MarkerNo,
                [Fabric Description] = pms_f.[Description],
                [Color] = color.val ,
                [Size] = size.val,
                [Spreading Machine#] = si.SpreadingNoID,
                [Fabric Roll] = FabricRoll.val,
                [InspectedPointQty] = siic.[count],
                [RejectPointQty] = siifc.[count],
                [InspectPoint] = sii.Item ,
                [Result] = sii.Result,
                [DefectCode] = case when sii.Item = 'Marker Check' then  + isnull(MC.val,'')
				                when sii.Item = 'Uneven Width' then 'Fabric Width: ' + CONVERT( varchar, pms_f.Width) + CHAR(10) + 'Actual Width:' + siid.val
				                when sii.Item = 'Machine Tension' then  isnull('Speed:'+ mt.val,'')
				                when sii.Item = 'Fabric Defect' then isnull( fd.val,'')
								when sii.Item = 'Fabric Relaxation' then isnull('hrs:' + FR.val, '')
				                else '' end ,
                [LastInspectionDate] = iif(sii.Result = '',null, IsNull(sii.EditDate, sii.AddDate)),
                [Inspector] =iif(sii.Result = '',null,p.[Name]),
                [StartingTime] = iif(sii.Result = '',null,st.val),
                [Remark] = si.Remark
                from SpreadingInspection s 
                left join SpreadingInspection_InsCutRef si with(nolock) on s.ID = si.ID
                left join SpreadingInspection_InsCutRef_Inspection sii with(nolock) on si.Ukey = sii.SpreadingInspectionInsCutRefUkey 
                left join SpreadingInspection_OriCutRef so with(nolock) on s.id = so.id
                --left join SpreadingInspection_InsCutRef_Inspection_Detail siid with(nolock) on siid.SpreadingInspectionInsCutRefInspectionUkey = sii.Ukey
                left join Pass1 p with(nolock) on p.ID = iif(sii.EditName='',sii.AddName,sii.EditName)
                outer apply
                (
	                select val = Stuff((
		                select (char(10) + ColumnValue)
		                from (
				                select ColumnValue
				                from SpreadingInspection_InsCutRef_Inspection_Detail with(nolock)
				                where SpreadingInspectionInsCutRefInspectionUkey = sii.Ukey
			                ) s
		                for xml path ('')
	                ) , 1, 1, '')
                )siid
                outer apply( select [count] = count(*) from SpreadingInspection_OriCutRef where  si.ID = ID ) soc
                outer apply( select [count] = count(*) from SpreadingInspection_InsCutRef where  si.ID = ID  ) sic
                outer apply 
                (
	                select distinct  w.ID,FactoryID,SCIRefno,MDivisionId
	                from SciProduction_WorkOrderForOutput w 
	                left join SpreadingInspection_OriCutRef so with(nolock) on  w.Ukey = so.WorkOrderForOutputUkey
	                where so.id = si.id
                )pms_wo
                left join SciProduction_Fabric pms_f with(nolock) on pms_wo.SCIRefno = pms_f.SCIRefno
                outer apply( 
	                select top 1 [MarkerNo] = w.MarkerNo, [CutNo] = w.Cutno
	                from SciProduction_WorkOrderForOutput w 
	                left join SpreadingInspection_OriCutRef so with(nolock) on  w.Ukey = so.WorkOrderForOutputUkey
	                where so.id = si.id
                ) m
                outer apply( select top 1 * from SciProduction_Orders where pms_wo.ID = ID ) sty
                outer apply( select [count] = count(*) from SpreadingInspection_InsCutRef_Inspection where SpreadingInspectionInsCutRefUkey = sii.SpreadingInspectionInsCutRefUkey and Result != '') siic
                outer apply( select [count] = Count(*) from SpreadingInspection_InsCutRef_Inspection where SpreadingInspectionInsCutRefUkey = sii.SpreadingInspectionInsCutRefUkey and Result ='Fail') siifc
                outer apply( select val = MIN(AddDate) from SpreadingInspection_InsCutRef_Inspection ms where SpreadingInspectionInsCutRefUkey = sii.SpreadingInspectionInsCutRefUkey) st
                outer apply
                (
                select [ouiCutRef] = stuff((
                select concat('/',tmp.CutRef) from
                (
	                select distinct CutRef
	                from SpreadingInspection_OriCutRef
                    where id = si.id
                ) tmp for xml path('')),1,1,'')
                )oui
                outer apply
                (
                select Per = ROUND(CAST(count(*) as float) / CAST( 10 as float),2) 
                from SpreadingInspection_InsCutRef_Inspection
                where sii.SpreadingInspectionInsCutRefUkey = SpreadingInspectionInsCutRefUkey and Result = 'Pass'
                )
                RFT
                outer apply
                (
	                select val = dbo.SciProduction_GetSinglelineSP(
	                (
		                select distinct S.OrderID
		                from SciProduction_WorkOrderForOutput_Distribute S with(nolock)
		                where S.OrderID!='EXCESS'
		                AND WorkOrderForOutputUkey in (select WorkOrderForOutputUkey from SpreadingInspection_OriCutRef sioc where sioc.id = si.id)
		                order by OrderID
		                for XML RAW)
	                )
                )SP
                outer apply
                (
                select val = stuff(
                (
	                select concat('/',tmp.Colorid) 
	                from
	                (
		                select distinct w.Colorid 
		                from SciProduction_WorkOrderForOutput w
		                where Ukey IN (select WorkOrderForOutputUkey from SpreadingInspection_OriCutRef sioc where sioc.id = si.id
	                )
                )
                tmp for xml path('')),1,1,'')
                )
                color
                outer apply
                (
                select val = stuff(
                (
	                select  concat(char(10),tmp.DataList) 
	                from
	                (
		                select distinct [DataList] = concat(S.SizeCode ,'/', S.Qty)
		                from SciProduction_WorkOrderForOutput_SizeRatio S
		                where  WorkOrderForOutputUkey IN (select WorkOrderForOutputUkey from SpreadingInspection_OriCutRef sioc where sioc.id = si.id
	                )
                ) 
                tmp for xml path('')),1,1,'')
                )size
                outer apply
                (
                    select val = stuff((select concat(';','(' + tmp.Roll + ')(' + tmp.Dyelot + ')(' + Seq1 + ' ' + tmp.Seq2 + ')')
                    from
                    (
                        select sif.Roll, sid.Dyelot, sid.Seq1, sid.Seq2 from SpreadingInspection_InsCutRef_Fabric sif
                        Left join SciProduction_Issue_Detail sid on sid.Ukey = sif.IssueDetailUkey
                        where sif.SpreadingInspectionInsCutRefUkey = si.Ukey
                    ) 
                    tmp for xml path('')),1,1,'')
                )FabricRoll
                outer apply
                (
                select val = Stuff((
	                select concat(char(10), s.[Description])
	                from (
			                select [Description]
			                from SpreadingDefectCode sc
			                where EXISTS (select 1 from SpreadingInspection_InsCutRef_Inspection_Detail siid2 with(nolock) 
							                where siid2.SpreadingInspectionInsCutRefInspectionUkey = sii.Ukey
							                and sc.ID = siid2.ColumnValue )
			                and [Type] ='MC'
		                ) s
	                for xml path ('')
                ) , 1, 1, '')
                )MC
                outer apply
                (
                select val = Stuff((
	                select concat(char(10), s.[Description])
	                from (
			                select [Description]
			                from SpreadingDefectCode sc
			                where EXISTS (select 1 from SpreadingInspection_InsCutRef_Inspection_Detail siid2 with(nolock) 
							                where siid2.SpreadingInspectionInsCutRefInspectionUkey = sii.Ukey
							                and sc.ID = siid2.ColumnValue )
			                and [Type] ='MT'
		                ) s
	                for xml path ('')
                ) , 1, 1, '')
                )MT
                outer apply
                (
                select val= STUFF((
					select concat(CHAR(10),Concat(pms_fd.[Type] + '-' + pms_fd.DescriptionEN,Concat('(Roll:',Roll.val,')'))) 
                    from SpreadingInspection_InsCutRef_Inspection_Detail siid2
					Left join SciProduction_FabricDefect pms_fd on pms_fd.ID = siid2.ColumnValue 
					outer apply(
						select val= STUFF((
							select Concat(',',siidr.Roll) 
                            from SpreadingInspection_InsCutRef_Inspection_Detail_Roll siidr 
							where siidr.SpreadingInspectionInsCutRefInspectionDetailUkey = siid2.Ukey
						for xml path ('')
						 ) , 1, 1, '')
					)Roll
					where siid2.SpreadingInspectionInsCutRefInspectionUkey = sii.Ukey
				for xml path ('')
				) , 1, 1, '')
                )FD
				outer apply
                (
                select val = Stuff((
	                select concat(char(10), s.[Description])
	                from (
			                select [Description]
			                from SpreadingDefectCode sc
			                where EXISTS (select 1 from SpreadingInspection_InsCutRef_Inspection_Detail siid2 with(nolock) 
							                where siid2.SpreadingInspectionInsCutRefInspectionUkey = sii.Ukey
							                and sc.ID = siid2.ColumnValue )
			                and [Type] ='FR'
		                ) s
	                for xml path ('')
                ) , 1, 1, '')
                )FR
                outer apply(
					 select min(convert(date,lista.AddDate)) as date1 
					 from SpreadingInspection_InsCutRef_Inspection lista
					 where sii.SpreadingInspectionInsCutRefUkey = lista.SpreadingInspectionInsCutRefUkey
				) firstinspdate
                {this.strSQLWhere}
                order by firstinspdate.date1,  [SubCutRef],sii.Seq";
            }

            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, this.lisSqlParameter, out this.printTable);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printTable == null)
            {
                return false;
            }

            string excelName = this.radioSummary.Checked ? "Quality_R14_Summary" : "Quality_R14_Detail";
            int excelRow = this.radioSummary.Checked ? 2 : 1;

            this.SetCount(this.printTable.Rows.Count);
            if (this.printTable == null || this.printTable.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + $"\\{excelName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printTable, string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: excelRow, excelApp: objApp);
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(excelName);

            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            strExcelName.OpenFile();
            #endregion

            return true;
        }

        /// <summary>
        /// ChangShift
        /// </summary>
        /// <param name="strFactory">strFactory</param>
        /// <returns>string</returns>
        public string ChangShift(string strFactory)
        {
            string sqlCmd = $@"SELECT TOP 1 Shift FROM Shift
                            WHERE FactoryID= '{strFactory}'
                            and BeginTime <= CONVERT (time, GETDATE())
                            and EndTime >= CONVERT (time, GETDATE())
                            ORDER BY StartDate DESC
                            ";
            return MyUtility.GetValue.Lookup(sqlCmd, "ManufacturingExecution");
        }

        private void ComboFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboShift.Text = this.ChangShift(this.comboFactory.Text);
        }
    }
}
