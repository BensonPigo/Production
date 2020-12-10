using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P03_Transaction : Win.Subs.Base
    {
        private readonly DataRow dr;
        private readonly bool _byroll;   // 從p20呼叫時，會傳入true

        /// <inheritdoc/>
        public P03_Transaction(DataRow data, bool byRoll = false)
        {
            this.InitializeComponent();
            this.dr = data;
            this._byroll = byRoll;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.btnReCalculate.Enabled = !this._byroll;     // 從p20呼叫時，不開啟。
            this.btnReCalculate.Visible = !this._byroll;     // 從p20呼叫時，不開啟。

            if (this._byroll)
            {
                this.Text += string.Format(@" ({0}-{1}-{2}-{3}-{4})", this.dr["id"], this.dr["seq1"], this.dr["seq2"], this.dr["roll"], this.dr["dyelot"]);
            }
            else
            {
                this.Text += string.Format(@" ({0}-{1}-{2})", this.dr["id"], this.dr["seq1"], this.dr["seq2"]);
            }

            #region sql command
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(
                @"select *,
            --sum(TMP.inqty - TMP.outqty+tmp.adjust) over ( order by tmp.addDate,TMP.inqty desc, TMP.outqty,tmp.adjust) as [balance] 
            sum(TMP.inqty - TMP.outqty+tmp.adjust) over (order by IssueDate,tmp.addDate, name) as [balance] 
            from (
 			select 	a.IssueDate
					, a.id
                	, name = Case type 
            					when 'A' then 'P35. Adjust Bulk Qty' 
    							when 'B' then 'P34. Adjust Stock Qty' 
						     end
                	,inqty = 0 
                	,outqty = 0
                	,adjust = adjust.Qty
                	,remark 
                	,location = MtlLocation.location 
                	,AddDate
            from Adjust a WITH (NOLOCK)
            outer apply (
	                    	select qty = sum (QtyAfter - QtyBefore)
	                    	from Adjust_Detail b WITH (NOLOCK)
	                    	where a.id = b.id and  b.poid='{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' 
	                    ) adjust
            outer apply(
				select  location = stuff((select ',' + x.location								
								          from(select location = stuff((select ',' + t.MtlLocationID 
															            from
																            (SELECT MtlLocationID 
                                                                             from FtyInventory fty WITH (NOLOCK) 
																             join FtyInventory_Detail fty_D WITH (NOLOCK) on fty.ukey = fty_D.ukey
																             Where  ad.poid = fty.poid and 
                                                                                    ad.seq1 = fty.seq1 and 
                                                                                    ad.seq2 = fty.seq2 and 
                                                                                    ad.Roll = fty.Roll and 
                                                                                    ad.Dyelot = fty.Dyelot and
																	                MtlLocationID != '' and 
                                                                                    MtlLocationID is not null) t 
															            for xml path('')),1,1,'') 
									           from Adjust_Detail ad WITH (NOLOCK)
									           where a.id = ad.id and  poid='{0}' and seq1 = '{1}'and seq2 = '{2}'
								          )x			
								          for xml path('')),1,1,'') 
			) MtlLocation            
            where a.Status = 'Confirmed' and a.Type not in  ('R','O')
                    and exists (
			                    	select 1
			                    	from Adjust_Detail b WITH (NOLOCK)
			                    	where a.Id = b.Id
			                    			and poid = '{0}'
			                    			and seq1 = '{1}'
			                    			and seq2 = '{2}'
			                    )",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));

            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
            select 	a.IssueDate
            		, a.id
            		,name = 'P31. Material Borrow out' 
            		,inqty = 0
            		,released = released.qty
            		,adjust = 0
            		,remark 
            		,location = MtlLocation.location
            		,AddDate
            from BorrowBack a WITH (NOLOCK)
            outer apply (
	                    	select qty = sum (qty)
	                    	from BorrowBack_Detail b WITH (NOLOCK)
	                    	where a.id = b.id and  b.FromPoId='{0}' and b.FromSeq1 = '{1}' and b.FromSeq2 = '{2}' 
	                    ) released
            outer apply(
				select  location = stuff((select ',' + x.location								
								          from(select location = stuff((select ',' + t.MtlLocationID 
															            from
																            (SELECT MtlLocationID 
                                                                                from FtyInventory fty WITH (NOLOCK)
																                join FtyInventory_Detail fty_D WITH (NOLOCK) on fty.ukey = fty_D.ukey
																                 Where  BBD.FromPOID = fty.poid and 
                                                                                        BBD.Fromseq1 = fty.seq1 and 
                                                                                        BBD.Fromseq2 = fty.seq2 and 
                                                                                        BBD.FromRoll = fty.Roll and
                                                                                        BBD.FromDyelot = fty.Dyelot and 
                                                                                        MtlLocationID != '' and 
                                                                                        MtlLocationID is not null)t 
															                    for xml path('')),1,1,'') 
									           from BorrowBack_Detail BBD WITH (NOLOCK)
									           where a.id = BBD.id and Frompoid='{0}' and Fromseq1 = '{1}'and Fromseq2 = '{2}'
								          )x			
								          for xml path('')),1,1,'') 
			) MtlLocation             
            where type='A' and Status='Confirmed'  
                        and exists (
			                    	select 1
			                    	from BorrowBack_Detail b WITH (NOLOCK)
			                    	where a.Id = b.Id
			                    			and b.Frompoid = '{0}'
			                    			and b.Fromseq1 = '{1}'
			                    			and b.Fromseq2 = '{2}')",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));

            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and fromroll='{0}' and fromdyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
			select 	issuedate
					,a.id
            		,name = 'P31. Material Borrow In' 
            		,arrived = arrived.qty
            		,ouqty = 0 
            		,adjust = 0 
            		,remark 
            		,location = MtlLocation.location
            		,AddDate
            from BorrowBack a WITH (NOLOCK)
            outer apply (
	                    	select qty = sum (qty)
	                    	from BorrowBack_Detail b WITH (NOLOCK)
	                    	where a.id = b.id and  b.ToPoId='{0}' and b.ToSeq1 = '{1}' and b.ToSeq2 = '{2}' 
	                    ) arrived
            outer apply(
				select  location = stuff((select ',' + x.location								
								          from(select location = stuff((select ',' + t.MtlLocationID 
															            from
																            (SELECT MtlLocationID 
                                                                                from FtyInventory fty WITH (NOLOCK)
																                join FtyInventory_Detail fty_D WITH (NOLOCK) on fty.ukey = fty_D.ukey
																                 Where  BBD.ToPoId = fty.poid and 
                                                                                        BBD.ToSeq1 = fty.seq1 and 
                                                                                        BBD.ToSeq2 = fty.seq2 and 
                                                                                        BBD.FromRoll = fty.Roll and
                                                                                        BBD.FromDyelot = fty.Dyelot and 
                                                                                        MtlLocationID != '' and 
                                                                                        MtlLocationID is not null)t 
															                    for xml path('')),1,1,'') 
									           from BorrowBack_Detail BBD WITH (NOLOCK)
									           where a.id = BBD.id and ToPoId='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'
								          )x			
								          for xml path('')),1,1,'') 
			) MtlLocation             
            where type='A' and Status='Confirmed'  
                        and exists (
			                    	select 1
			                    	from BorrowBack_Detail b WITH (NOLOCK)
			                    	where a.Id = b.Id
			                    			and b.ToPoId = '{0}'
			                    			and b.ToSeq1 = '{1}'
			                    			and b.ToSeq2 = '{2}') ",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and Toroll='{0}' and Todyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
            select 	a.IssueDate
            		,a.id
            		,name = 'P32. Return Borrowing out' 
            		,inqty = 0
            		,released = released.qty
            		,adjust = 0
            		,remark 
            		,location = MtlLocation.location
            		,AddDate
            from BorrowBack a WITH (NOLOCK)
            outer apply (
	                    	select qty = sum (qty)
	                    	from BorrowBack_Detail b WITH (NOLOCK)
	                    	where a.id = b.id and  b.FromPoId='{0}' and b.FromSeq1 = '{1}' and b.FromSeq2 = '{2}' 
	                    ) released
            outer apply(
				select  location = stuff((select ',' + x.location								
								          from(select location = stuff((select ',' + t.MtlLocationID 
															            from
																            (SELECT MtlLocationID 
                                                                                from FtyInventory fty WITH (NOLOCK)
																                join FtyInventory_Detail fty_D WITH (NOLOCK) on fty.ukey = fty_D.ukey
																                 Where  BBD.FromPOID = fty.poid and 
                                                                                        BBD.Fromseq1 = fty.seq1 and 
                                                                                        BBD.Fromseq2 = fty.seq2 and 
                                                                                        BBD.FromRoll = fty.Roll and
                                                                                        BBD.FromDyelot = fty.Dyelot and 
                                                                                        MtlLocationID != '' and 
                                                                                        MtlLocationID is not null)t 
															                    for xml path('')),1,1,'') 
									           from BorrowBack_Detail BBD WITH (NOLOCK)
									           where a.id = BBD.id and Frompoid='{0}' and Fromseq1 = '{1}'and Fromseq2 = '{2}'
								          )x			
								          for xml path('')),1,1,'') 
			) MtlLocation             
            where type='B' and Status='Confirmed'  
                        and exists (
			                    	select 1
			                    	from BorrowBack_Detail b WITH (NOLOCK)
			                    	where a.Id = b.Id
			                    			and b.Frompoid = '{0}'
			                    			and b.Fromseq1 = '{1}'
			                    			and b.Fromseq2 = '{2}')",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));

            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and fromroll='{0}' and fromdyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
            select 	issuedate
            		,a.id
            		,name = 'P32. Return Borrowing In' 
            		,arrived = arrived.qty
            		,ouqty = 0
            		,adjust = 0
            		,remark 
            		,location = MtlLocation.location
            		,AddDate
            from BorrowBack a WITH (NOLOCK)
            outer apply (
	                    	select qty = sum (qty)
	                    	from BorrowBack_Detail b WITH (NOLOCK)
	                    	where a.id = b.id and  b.ToPoId='{0}' and b.ToSeq1 = '{1}' and b.ToSeq2 = '{2}' 
	                    ) arrived
            outer apply(
				select  location = stuff((select ',' + x.location								
								          from(select location = stuff((select ',' + t.MtlLocationID 
															            from
																            (SELECT MtlLocationID 
                                                                                from FtyInventory fty WITH (NOLOCK)
																                join FtyInventory_Detail fty_D WITH (NOLOCK) on fty.ukey = fty_D.ukey
																                 Where  BBD.ToPoId = fty.poid and 
                                                                                        BBD.ToSeq1 = fty.seq1 and 
                                                                                        BBD.ToSeq2 = fty.seq2 and 
                                                                                        BBD.FromRoll = fty.Roll and
                                                                                        BBD.FromDyelot = fty.Dyelot and 
                                                                                        MtlLocationID != '' and 
                                                                                        MtlLocationID is not null)t 
															                    for xml path('')),1,1,'') 
									           from BorrowBack_Detail BBD WITH (NOLOCK)
									           where a.id = BBD.id and ToPoId='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'
								          )x			
								          for xml path('')),1,1,'') 
			) MtlLocation             
            where type='B' and Status='Confirmed'  
                        and exists (
			                    	select 1
			                    	from BorrowBack_Detail b WITH (NOLOCK)
			                    	where a.Id = b.Id
			                    			and b.ToPoId = '{0}'
			                    			and b.ToSeq1 = '{1}'
			                    			and b.ToSeq2 = '{2}') ",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and Toroll='{0}' and Todyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
	            select issuedate
                        , a.id
                	    , name = case type 
                					when 'A' then 'P10. Issue Fabric to Cutting Section' 
                					when 'B' then 'P11. Issue Sewing Material by Transfer Guide' 
                					when 'C' then 'P12. Issue Packing Material by Transfer Guide' 
                					when 'D' then 'P13. Issue Material by Item'
                					when 'E' then 'P33. Issue Thread'
                					when 'I' then 'P62. Issue Fabric for Cutting Tape'
                                end 
                	    , inqty = 0 
                		, Released.qty
                        , adjust = 0
                		, remark
                        , location = MtlLocation.location
                        , AddDate
                    from Issue a WITH (NOLOCK)
                	outer apply (
                		select qty = sum (qty)
                		from issue_detail is_d WITH (NOLOCK)
                		where a.id = is_d.id and  poid='{0}' and seq1 = '{1}'and seq2 = '{2}' 
                	) released
                    outer apply(
                		select  location = stuff((select ',' + x.location								
                								    from(select distinct location = fty_d.MtlLocationID
                									    from issue_detail is_d WITH (NOLOCK)
                										inner join ftyinventory fty WITH (NOLOCK) on is_d.poid = fty.poid 
                																		and is_d.seq1 = fty.seq1 
                																		and is_d.seq2 = fty.seq2 
                																		and is_d.roll = fty.roll 
                																		and is_d.dyelot = fty.dyelot 
                										inner join ftyinventory_detail fty_d WITH (NOLOCK) on fty.ukey = fty_d.ukey
                										where a.id = is_d.id 
                												and is_d.poid = '{0}' 
                												and is_d.seq1 = '{1}'
                												and is_d.seq2 = '{2}'
                												and fty_d.mtllocationid != '' 
                												and fty_d.mtllocationid is not null
                																	
                								    )x			
                								    for xml path('')),1,1,'') 
                	) MtlLocation
                    where Status='Confirmed' 
                			and exists (
                				select 1
                				from Issue_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.poid = '{0}' 
                						and b.seq1 = '{1}'
                						and b.seq2 = '{2}'
                			) ",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
            select 	issuedate
            		,a.id
            		,name = case FabricType 
		            			when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
		           				when 'F' then 'P16. Issue Fabric Lacking & Replacement' 
		       		 end 
        			,inqty = 0
        			,outqty = released.qty
        			,adjust = 0 
        			,a.remark 
        			,location = MtlLocation.location
        			,AddDate
            from IssueLack a WITH (NOLOCK)
            outer apply (
                		select qty = sum (qty)
                		from IssueLack_Detail b WITH (NOLOCK)
                		where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' 
                	) released
            outer apply(
				select  location = stuff((select ',' + x.location								
								          from(select location = stuff((select ',' + t.MtlLocationID 
															            from
																            (SELECT MtlLocationID 
                                                                             from FtyInventory fty WITH (NOLOCK)
																             join FtyInventory_Detail fty_D WITH (NOLOCK) on fty.ukey = fty_D.ukey
																             Where  IL_D.POID = fty.poid and 
                                                                                    IL_D.seq1 = fty.seq1 and 
                                                                                    IL_D.seq2 = fty.seq2 and 
                                                                                    IL_D.Roll = fty.Roll and 
                                                                                    IL_D.Dyelot = fty.Dyelot and 
                                                                                    MtlLocationID != '' and 
                                                                                    MtlLocationID is not null)t 
															            for xml path('')),1,1,'') 
									           from IssueLack_Detail IL_D WITH (NOLOCK)
									           where a.id = IL_D.id and  poid='{0}' and seq1 = '{1}'and seq2 = '{2}'
								          )x			
								          for xml path('')),1,1,'') 
			) MtlLocation 
            where a.Status in ('Confirmed','Closed') and a.Type='R'
                			and exists (
                				select 1
                				from IssueLack_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.poid = '{0}' 
                						and b.seq1 = '{1}'
                						and b.seq2 = '{2}'
                			) ",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
            select 	issuedate
            		,a.id
            		,name = 'P17. R/Mtl Return' 
            		,inqty = 0
            		,released = released.qty
            		,adjust = 0
            		,remark
            		,location = MtlLocation.location
            		,AddDate
            from IssueReturn a WITH (NOLOCK)
            outer apply (
                		select qty = sum (-b.Qty)
                		from IssueReturn_Detail b WITH (NOLOCK)
                		where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' 
                	) released
            outer apply(
				select  location = stuff((select ',' + x.location								
								          from(select location = stuff((select ',' + t.MtlLocationID 
															            from
																            (SELECT MtlLocationID 
                                                                             from FtyInventory fty WITH (NOLOCK)
																             join FtyInventory_Detail fty_D WITH (NOLOCK) on fty.ukey = fty_D.ukey
																             Where  IR_D.POID = fty.poid and 
                                                                                    IR_D.seq1 = fty.seq1 and 
                                                                                    IR_D.seq2 = fty.seq2 and 
                                                                                    IR_D.Roll = fty.Roll and 
                                                                                    IR_D.Dyelot = fty.Dyelot and 
                                                                                    MtlLocationID != '' and 
                                                                                    MtlLocationID is not null)t 
															            for xml path('')),1,1,'') 
									           from IssueReturn_Detail IR_D WITH (NOLOCK)
									           where a.id = IR_D.id and  poid='{0}' and seq1 = '{1}'and seq2 = '{2}'
								          )x			
								          for xml path('')),1,1,'') 
			) MtlLocation   
            where status='Confirmed' 
                			and exists (
                				select 1
                				from IssueReturn_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.poid = '{0}' 
                						and b.seq1 = '{1}'
                						and b.seq2 = '{2}'
                			) ",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
            select  issuedate = case type when 'A' then a.eta else a.WhseArrival end
                    ,a.id
                    ,name = case type when 'A' then 'P07. Material Receiving' 
                                      when 'B' then 'P08. Warehouse Shopfloor Receiving' end 
                    ,arrived = arrived.qty
                    ,ouqty = 0
                    ,adjust = 0
                    ,remark = '' 
                    ,X.Location
                    ,AddDate
            from Receiving a WITH (NOLOCK)
            outer apply (
                		select qty = sum (b.StockQty)
                		from Receiving_Detail b WITH (NOLOCK)
                		where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' 
                	) arrived
			outer apply(
				select Location = (
					select distinct concat(c.Location,'')
					from Receiving_Detail c WITH (NOLOCK) 
					where c.Id = a.Id and c.Seq1 = '{1}' and c.Seq2 = '{2}'
					for xml path('')
				)
			)X
            where Status='Confirmed'
                    and exists (
                				select 1
                				from Receiving_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.poid = '{0}' 
                						and b.seq1 = '{1}'
                						and b.seq2 = '{2}'
                			) ",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
            select 	issuedate
            		,a.id
            		,name = 'P37. Return Receiving Material'             
            		,inqty = 0
            		,released = released.qty
            		,adjust = 0
            		,remark
            		,location = MtlLocation.location
            		,AddDate
            from ReturnReceipt a WITH (NOLOCK)
            outer apply (
                		select qty = sum (b.Qty)
                		from ReturnReceipt_Detail b WITH (NOLOCK)
                		where a.id = b.id and b.poid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}' 
                	) released
            outer apply(
				select  location = stuff((select ',' + x.location								
								          from(select location = stuff((select ',' + t.MtlLocationID 
															            from
																            (SELECT MtlLocationID 
                                                                             from FtyInventory fty WITH (NOLOCK)
																             join FtyInventory_Detail fty_D WITH (NOLOCK) on fty.ukey = fty_D.ukey
																             Where  RR_D.POID = fty.poid and 
                                                                                    RR_D.seq1 = fty.seq1 and 
                                                                                    RR_D.seq2 = fty.seq2 and 
                                                                                    RR_D.Roll = fty.Roll and 
                                                                                    RR_D.Dyelot = fty.Dyelot and 
                                                                                    MtlLocationID != '' and 
                                                                                    MtlLocationID is not null)t 
															            for xml path('')),1,1,'') 
									           from ReturnReceipt_Detail RR_D WITH (NOLOCK)
									           where a.id = RR_D.id and  poid='{0}' and seq1 = '{1}'and seq2 = '{2}'
								          )x			
								          for xml path('')),1,1,'') 
			) MtlLocation     
            where Status='Confirmed' 
                    and exists (
                				select 1
                				from ReturnReceipt_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.poid = '{0}' 
                						and b.seq1 = '{1}'
                						and b.seq2 = '{2}'
                			) ",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"
                                                                              
            union all
	        select issuedate
                    , a.id
	                , name = 'P23. Transfer Inventory to Bulk'
	                , inqty = 0
                    , released = released.qty
                    , adjust = 0 
                    , remark = ''  
                    , X.Location
                    , AddDate
            from SubTransfer a WITH (NOLOCK)
            outer apply (
                		select qty = sum (b.Qty)
                		from SubTransfer_Detail b WITH (NOLOCK)
                		where a.id = b.id and b.Frompoid = '{0}' and b.Fromseq1 = '{1}' and b.FromSeq2 = '{2}' 
                	) released
			outer apply(
				select Location = (
					select distinct concat(c.toLocation,'')
					from SubTransfer_Detail c WITH (NOLOCK) 
					where c.Id = a.Id  and c.Fromseq1 = '{1}' and c.FromSeq2 = '{2}'
					for xml path('')
				)
			)X
            where a.Status='Confirmed' and a.type = 'B'
                    and exists (
                				select 1
                				from SubTransfer_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.Frompoid = '{0}' 
                						and b.Fromseq1 = '{1}'
                						and b.FromSeq2 = '{2}'
                			)",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and fromroll='{0}' and fromdyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"                                                                          
            union all
	            select  issuedate
                        , a.id
                        ,'P23. Transfer Inventory to Bulk' name
                        , arrived.qty arrived
                        ,0 as ouqty
                        ,0 as adjust
                        , remark
	                    ,ToLocation = (Select cast(tmp.ToLocation as nvarchar)+',' 
                                       from (select b1.ToLocation 
                                                   from SubTransfer a1 WITH (NOLOCK) 
                                                   inner join SubTransfer_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
                                                   where a1.status = 'Confirmed' and (b1.ToLocation is not null or b1.ToLocation !='')
                                                       and b1.ToPoid = '{0}'
                                                       and b1.ToSeq1 = '{1}'
                                                       and b1.ToSeq2 = '{2}'  group by b1.ToLocation) tmp 
                                       for XML PATH(''))
                        ,AddDate
            from SubTransfer a WITH (NOLOCK)
            outer apply (
                		select qty = sum (b.Qty)
                		from SubTransfer_Detail b WITH (NOLOCK)
                		where a.id = b.id and b.ToPoid = '{0}' and b.ToSeq1 = '{1}' and b.ToSeq2 = '{2}' 
                	) arrived
            where a.Status = 'Confirmed' and a.type = 'B'  
                    and exists (
                				select 1
                				from SubTransfer_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.ToPoid = '{0}' 
                						and b.ToSeq1 = '{1}'
                						and b.ToSeq2 = '{2}'
                			)
            ",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and Toroll='{0}' and Todyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
	            select  issuedate
                        , a.id
                        ,'P18. TransferIn' name
                        , arrived.qty arrived
                        , 0 as ouqty
                        , 0 as adjust
                        , a.remark
	                    , (Select cast(tmp.Location as nvarchar)+',' 
                                    from (select b1.Location 
                                                from TransferIn a1 WITH (NOLOCK) 
                                                inner join TransferIn_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
                                                where a1.status = 'Confirmed' and (b1.Location is not null or b1.Location !='')
                                                    and b1.Poid = '{0}'
                                                    and b1.Seq1 = '{1}'
                                                    and b1.Seq2 = '{2}' group by b1.Location) tmp 
                                    for XML PATH('')) as Location
                        , AddDate
            from TransferIn a WITH (NOLOCK) 
            outer apply (
                		select qty = sum (b.Qty)
                		from TransferIn_Detail b WITH (NOLOCK)
                		where a.id = b.id and b.Poid = '{0}' and b.Seq1 = '{1}' and b.Seq2 = '{2}' 
                	) arrived
            where Status='Confirmed' 
                    and exists (
                				select 1
                				from TransferIn_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.Poid = '{0}' 
                						and b.Seq1 = '{1}'
                						and b.Seq2 = '{2}'
                			)",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
	        select 	issuedate
	        		,a.id
            		,name = 'P19. TransferOut' 
            		,inqty = 0
            		,released = released.qty
            		,adjust = 0
            		,remark
            		,location = MtlLocation.location
            		,AddDate
            from TransferOut a WITH (NOLOCK)
            outer apply (
                		select qty = sum (b.Qty) 
                		from TransferOut_Detail b WITH (NOLOCK)
                		where a.id = b.id and b.Poid = '{0}' and b.Seq1 = '{1}' and b.Seq2 = '{2}' 
                	) released
            outer apply(
				select  location = stuff((select ',' + x.location								
								          from(select location = stuff((select ',' + t.MtlLocationID 
															            from
																            (SELECT MtlLocationID 
                                                                             from FtyInventory fty WITH (NOLOCK)
																             join FtyInventory_Detail fty_D WITH (NOLOCK) on fty.ukey = fty_D.ukey
																             Where  TfD_D.POID = fty.poid and 
                                                                                    TfD_D.seq1 = fty.seq1 and 
                                                                                    TfD_D.seq2 = fty.seq2 and 
                                                                                    TfD_D.Roll = fty.Roll and 
                                                                                    TfD_D.Dyelot = fty.Dyelot and 
                                                                                    MtlLocationID != '' and 
                                                                                    MtlLocationID is not null)t 
															            for xml path('')),1,1,'') 
									           from TransferOut_Detail TfD_D WITH (NOLOCK)
									           where a.id = TfD_D.id and  poid='{0}' and seq1 = '{1}'and seq2 = '{2}'
								          )x			
								          for xml path('')),1,1,'') 
			) MtlLocation   
            where Status='Confirmed' 
                    and exists (
                				select 1
                				from TransferOut_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.Poid = '{0}' 
                						and b.Seq1 = '{1}'
                						and b.Seq2 = '{2}'
                			)",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @" 

            union all
	            select issuedate
                , a.id
	            ,'P36. Transfer Scrap to Inventory' as name
	            , inqty.qty as inqty
                , 0 as released
                , 0 as adjust 
                , isnull(a.remark,'') remark 
				,(Select cast(tmp.ToLocation as nvarchar)+',' 
                                    from (select b1.ToLocation 
                                                from SubTransfer a1 WITH (NOLOCK) 
                                                inner join SubTransfer_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
                                                where a1.status = 'Confirmed' and (b1.ToLocation is not null or b1.ToLocation !='')
                                                    and b1.ToPoid = '{0}'
                                                    and b1.ToSeq1 = '{1}'
                                                    and b1.ToSeq2 = '{2}' group by b1.ToLocation) tmp 
                                    for XML PATH('')) as ToLocation
				,AddDate
            from SubTransfer a WITH (NOLOCK)
            outer apply (
                		select qty = sum (b.Qty)
                		from SubTransfer_Detail b WITH (NOLOCK)
                		where a.id = b.id and b.topoid = '{0}' and b.toseq1 = '{1}' and b.toSeq2 = '{2}' 
                	) inqty
            where type='C' and Status='Confirmed'
                    and exists (
                				select 1
                				from SubTransfer_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.topoid = '{0}' 
                						and b.toseq1 = '{1}'
                						and b.toSeq2 = '{2}'
                			)",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                // 0000584: WAREHOUSE_P20_Detail_Transaction detail
                // selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
                selectCommand1.Append(string.Format(@" and ToRoll='{0}' and ToDyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"
            
            union all
            select  issuedate
                    , a.id
	                , case type  when 'B' then 'P73. Transfer Inventory to Bulk cross M (Receive)' 
			                     when 'D' then 'P76. Material Borrow cross M (Receive)' 
			                     when 'G' then 'P78. Material Return Back cross M (Receive)'  end name
	                , inqty.qty as inqty
                    , 0 released
                    , 0 as adjust
                    , remark
                    , X.location
                    , AddDate
            from RequestCrossM a WITH (NOLOCK) 
            outer apply (
                		select qty = sum (b.Qty)
                		from RequestCrossM_Receive b WITH (NOLOCK)
                		where a.id = b.id and b.POID = '{0}' and b.Seq1 = '{1}' and b.Seq2 = '{2}' 
                	) inqty
			outer apply(
				select Location = (
					select distinct concat(c.Location,',')
					from RequestCrossM_Receive c WITH (NOLOCK) 
					where c.Id = a.Id and c.Seq1 = '{1}'  and c.Seq2 = '{2}' 
					for xml path('')
				)
			)X
            where Status='Confirmed' 
                    and exists (
                				select 1
                				from RequestCrossM_Receive b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.POID = '{0}' 
                						and b.Seq1 = '{1}'
                						and b.Seq2 = '{2}'
                			)",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(string.Format(
                @"

            union all
            select  issuedate
                    , a.id
                    ,case type  when 'D' then 'P25. Transfer Bulk to Scrap' 
                                when 'E' then 'P24. Transfer Inventory to Scrap'
                                end as name
                    , 0 as inqty
                    , released.qty released
                    , 0 as adjust 
                    , isnull(a.remark,'') remark 
			        , (Select cast(tmp.ToLocation as nvarchar)+',' 
                                    from (select b1.ToLocation 
                                                from SubTransfer a1 WITH (NOLOCK) 
                                                inner join SubTransfer_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
                                                where a1.status = 'Confirmed' and (b1.ToLocation is not null or b1.ToLocation !='')
                                                    and b1.ToPoid = '{0}' 
                                                    and b1.ToSeq1 = '{1}' 
                                                    and b1.ToSeq2 = '{2}'  group by b1.ToLocation) tmp 
                                    for XML PATH('')) as ToLocation
					 , AddDate
            from SubTransfer a WITH (NOLOCK)
            outer apply (
                		select qty = sum (b.Qty)
                		from SubTransfer_Detail b WITH (NOLOCK)
                		where a.id = b.id and b.Frompoid = '{0}' and b.Fromseq1 = '{1}' and b.FromSeq2 = '{2}' 
                	) released
            where (type='D' or type='E') and Status='Confirmed' 
                    and exists (
                				select 1
                				from SubTransfer_Detail b WITH (NOLOCK)
                				where a.Id = b.Id
                						and b.Frompoid = '{0}' 
                						and b.Fromseq1 = '{1}'
                						and b.FromSeq2 = '{2}'
                			)",
                this.dr["id"].ToString(),
                this.dr["seq1"].ToString(),
                this.dr["seq2"].ToString()));
            if (this._byroll)
            {
                // 0000584: WAREHOUSE_P20_Detail_Transaction detail
                // selectCommand1.Append(string.Format(@" and roll='{0}' and dyelot = '{1}'", dr["roll"], dr["dyelot"]));
                selectCommand1.Append(string.Format(@" and FromRoll='{0}' and FromDyelot = '{1}'", this.dr["roll"], this.dr["dyelot"]));
            }

            selectCommand1.Append(@"
                ) tmp
                group by IssueDate,inqty,outqty,adjust,id,Remark,location,tmp.name, AddDate
                order by IssueDate,tmp.addDate,name");

            #endregion

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out DataTable selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }
            else if (selectDataTable1.Rows.Count > 0)
            {
                object inqty = selectDataTable1.Compute("sum(inqty)", null);
                object outqty = selectDataTable1.Compute("sum(outqty)", null);
                object adjust = selectDataTable1.Compute("sum(adjust)", null);
                object balance = Convert.ToDecimal(inqty) - Convert.ToDecimal(outqty) + Convert.ToDecimal(adjust);
                this.numTotal1.Value = !MyUtility.Check.Empty(inqty) ? decimal.Parse(inqty.ToString()) : 0m;
                this.numTotal2.Value = !MyUtility.Check.Empty(outqty) ? decimal.Parse(outqty.ToString()) : 0m;
                this.numTotal3.Value = !MyUtility.Check.Empty(adjust) ? decimal.Parse(adjust.ToString()) : 0m;
                this.numTotal4.Value = !MyUtility.Check.Empty(balance) ? decimal.Parse(balance.ToString()) : 0m;
            }

            this.bindingSource1.DataSource = selectDataTable1;

            #region 開窗
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.CellMouseDoubleClick += (s, e) =>
            {
                var frm = new Win.Tems.Input6(null);
                var dr2 = this.gridTransactionDetail.GetDataRow<DataRow>(e.RowIndex);
                if (dr2 == null)
                {
                    return;
                }

                switch (dr2["name"].ToString().Substring(0, 3))
                {
                    case "P07":
                        // P07
                        frm = new P07(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P08":
                        // P08
                        frm = new P08(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P09":
                        // P09
                        // frm = new Sci.Production.Warehouse.P09(null, dr2["id"].ToString());
                        // frm.ShowDialog(this);
                        break;
                    case "P37":
                        // P37
                        break;
                    case "P10":
                        // P10
                        frm = new P10(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P11":
                        // P11
                        frm = new P11(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P12":
                        // P12
                        frm = new P12(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P13":
                        // P13
                        frm = new P13(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    case "P15":
                        // P15
                        frm = new P15(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P16":
                        // P16
                        frm = new P16(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    case "P17":
                        // P17
                        frm = new P17(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    case "P18":
                        // P18
                        frm = new P18(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P19":
                        // P19
                        frm = new P19(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    case "P22":
                        // P22
                        frm = new P22(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P23":
                        // P23
                        frm = new P23(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    case "P24":
                        // P24
                        frm = new P24(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P25":
                        // P25
                        frm = new P25(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    case "P31":
                        // P31 borrow
                        frm = new P31(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P32":
                        // P32 give back
                        frm = new P32(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    case "P34":
                        // P34 adjust Inventory
                        frm = new P34(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P35":
                        // P35 adjust Bulk
                        frm = new P35(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;

                    case "P36":
                        // P36
                        frm = new P36(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P62":
                        // P62
                        frm = new P62(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                }

                // var frm = new Sci.Production.Subcon.P01_FarmInList(dr);
                // frm.ShowDialog(this);
            };
            #endregion

            // 設定Grid1的顯示欄位
            this.gridTransactionDetail.IsEditingReadOnly = true;
            this.gridTransactionDetail.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridTransactionDetail)
                 .Date("IssueDate", header: "Date", width: Widths.AnsiChars(10))
                 .Text("ID", header: "Transaction#", width: Widths.AnsiChars(15), settings: ts2)
                .Text("Name", header: "Name", width: Widths.AnsiChars(25))
                 .Numeric("InQty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("OutQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Adjust", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Text("Location", header: "Location", width: Widths.AnsiChars(20))
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(20));
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void BtnReCalculate_Click(object sender, EventArgs e)
        {
            if (this.dr == null)
            {
                return;
            }

            DualResult result;
            #region store procedure parameters
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            System.Data.SqlClient.SqlParameter sp_StocktakingID = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@Ukey",
                Value = this.dr["ukey"].ToString(),
            };
            cmds.Add(sp_StocktakingID);

            System.Data.SqlClient.SqlParameter sp_poid = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@poid",
                Value = this.dr["id"].ToString(),
            };
            cmds.Add(sp_poid);

            System.Data.SqlClient.SqlParameter sp_seq1 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@seq1",
                Value = this.dr["seq1"].ToString(),
            };
            cmds.Add(sp_seq1);

            System.Data.SqlClient.SqlParameter sp_seq2 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@seq2",
                Value = this.dr["seq2"].ToString(),
            };
            cmds.Add(sp_seq2);

            #endregion
            if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.usp_SingleItemRecaculate", cmds)))
            {
                // MyUtility.Msg.WarningBox(result.Messages[1].ToString());
                Exception ex = result.GetException();
                MyUtility.Msg.WarningBox(ex.Message);
                return;
            }

            MyUtility.Msg.InfoBox("Finished!!");
            this.Dispose();  // 重算完自動關閉視窗
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(string.Empty);

            // MyUtility.Excel.CopyToXls(grid1.GetTable(), "", "", headerRow: 3, showExcel: false, showSaveMsg: false, excelApp: objApp);
            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Columns[1].NumberFormat = "yyyy-MM-dd";
            worksheet.Columns[2].NumberFormat = "@";
            worksheet.Columns[3].NumberFormat = "@";
            worksheet.Columns[4].NumberFormat = "0.00";
            worksheet.Columns[5].NumberFormat = "0.00";
            worksheet.Columns[6].NumberFormat = "0.00";
            worksheet.Columns[7].NumberFormat = "0.00";
            worksheet.Columns[8].NumberFormat = "@";
            worksheet.Columns[9].NumberFormat = "@";

            for (int i = 1; i <= this.gridTransactionDetail.Columns.Count; i++)
            {
                worksheet.Cells[1, i] = this.gridTransactionDetail.Columns[i - 1].Name;
            }

            for (int i = 0; i < this.gridTransactionDetail.Rows.Count; i++)
            {
                for (int j = 0; j < this.gridTransactionDetail.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = this.gridTransactionDetail.Rows[i].Cells[j].Value;
                }
            }

            worksheet.Rows.AutoFit();
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_P03_Transaction");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
        }
    }
}
