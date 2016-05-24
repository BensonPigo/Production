
CREATE view [dbo].[View_TransactionList]
as
select 'Receiving' tbname,a.Type,a.Status,a.EditDate,b.Id,b.MDivisionID,b.PoId,b.seq1,b.seq2,b.Roll,b.Dyelot
,b.StockQty as inqty							,0.0 as outqty													,0.0 as adjustqty 
,iif(b.StockType='B',b.StockQty,0.0) as AInqty	,0.00 AOutqty													,0.00 as AAdjustQty
,iif(b.StockType='I',b.StockQty,0.0) as BInqty	,0.00 BOutqty													,0.00 as BAdjustQty
,0.0 as CInqty									,0.00 COutqty													,0.00 CAdjustQty
from dbo.Receiving a inner join dbo.Receiving_Detail b on b.id = a.id 
union all
select iif(a.type <'E','Issue','RequestCrossM-Issue'),a.Type,a.Status,a.EditDate,b.id,b.MDivisionID,b.POID,b.seq1,b.seq2,b.Roll,b.Dyelot
,0.0 as inqty									,b.Qty as outqty												,0.0 as adjustqty 
,0.0 as AInqty									,iif(b.StockType = 'B',b.Qty,0.00) as AOutQty					,0.00 AAdjustQty
,0.0 as BInqty									,iif(b.StockType = 'I',b.Qty,0.00) as BOutQty					,0.00 BAdjustQty
,0.0 AS CInqty									,0.00 COutQty													,0.00 CAdjustQty
from dbo.Issue a inner join dbo.Issue_Detail b on b.id = a.id 
union all
select 'IssueReturn','',a.Status,a.EditDate,b.ID,b.MDivisionID,b.POID,b.Seq1,b.Seq2,b.Roll,b.Dyelot
,0.0 as inqty								,0.0 as outqty														,iif(b.StockType!='O',b.qty,0.0) as adjustqty
,0.0 as AInqty								,0.00 as AOutqty													,iif(b.StockType='B',b.qty,0.00) as AAdjuqtQty
,0.0 as BInqty								,0.00 as BOutqty													,iif(b.StockType='I',b.qty,0.00) as BAdjuqtQty
,0.0 as CInqty								,0.00 as COutqty													,iif(b.StockType='O',b.qty,0.00) as CAdjuqtQty
from dbo.IssueReturn a inner join dbo.IssueReturn_Detail b on b.id = a.id 
union all
select 'SubTransfer-From',a.Type,a.Status,a.EditDate,b.Id,b.FromMDivisionID,b.FromPoId,b.Fromseq1,b.Fromseq2,b.FromRoll,b.FromDyelot 
,iif(b.FromStockType = 'O',b.Qty,0.00) as inqty	,b.Qty as outqty												,0.0 as adjustqty 
,0.0 as AInqty									,iif(b.FromStockType = 'B',b.Qty,0.00) as AOutQty				,0.00 AAdjustQty
,0.0 as BInqty									,iif(b.FromStockType = 'I',b.Qty,0.00) as BOutQty				,0.00 BAdjustQty
,0.0 AS CInqty									,iif(b.FromStockType = 'O',b.Qty,0.00) as COutQty				,0.00 CAdjustQty
from dbo.SubTransfer a inner join dbo.SubTransfer_Detail b on b.id = a.id 
union all
select 'SubTransfer-To',a.Type,a.Status,a.EditDate,b.Id,b.ToMDivisionID,b.ToPoId,b.ToSeq1,b.ToSeq2,b.ToRoll,b.ToDyelot 
,iif(b.ToStockType!='O',b.Qty,0.0) as inqty		,0.0 as outqty													,0.0 as adjustqty 
,iif(b.ToStockType='B',b.Qty,0.0) as AInqty		,0.00 as AOutqty												,0.00 AAdjuqtQty
,iif(b.ToStockType='I',b.Qty,0.0) as BInqty		,0.00 as BOutqty												,0.00 BAdjuqtQty
,iif(b.ToStockType='O',b.Qty,0.0) as CInqty		,0.00 as COutqty												,0.00 CAdjuqtQty
from dbo.SubTransfer a inner join dbo.SubTransfer_Detail b on b.id = a.id 
union all
select 'TransferIn','',a.Status,a.EditDate,b.id,b.MDivisionID,b.POID,b.seq1,b.seq2,b.Roll,b.Dyelot
,iif(b.StockType!='O',b.Qty,0.0) as inqty	,0.0 as outqty														,0.0 as adjustqty 
,iif(b.StockType='B',b.Qty,0.0) as AInqty	,0.00 as AOutqty													,0.00 as AAdjuqtQty
,iif(b.StockType='I',b.Qty,0.0) as BInqty	,0.00 as BOutqty													,0.00 as BAdjuqtQty
,iif(b.StockType='O',b.Qty,0.0) as CInqty	,0.00 as COutqty													,0.00 as CAdjuqtQty
from dbo.TransferIn a inner join dbo.TransferIn_Detail b on b.ID = a.id 
union all
select 'TransferOut','',a.Status,a.EditDate,b.id,b.MDivisionID,b.POID,b.seq1,b.seq2,b.Roll,b.Dyelot
,0.0 as inqty								,iif(b.stocktype!='O',b.Qty,0.0) as outqty							,0.0 as adjustqty 
,0.0 as AInqty								,iif(b.StockType='B',b.Qty,0.00) as AOutqty							,0.00 as AAdjuqtQty
,0.0 as BInqty								,iif(b.StockType='I',b.Qty,0.00) as BOutqty							,0.00 as BAdjuqtQty
,0.0 as CInqty								,iif(b.StockType='O',b.Qty,0.00) as COutqty							,0.00 as CAdjuqtQty
from dbo.TransferOut a inner join dbo.TransferOut_Detail b on b.ID = a.id 
union all
select 'Adjust',a.Type,a.Status,a.EditDate,b.ID,b.MDivisionID,b.POID,b.Seq1,b.Seq2,b.Roll,b.Dyelot
,0.0 as inqty								,0.0 as outqty														,iif(b.StockType!='O',b.qtyafter - b.qtybefore,0.0) as adjustqty
,0.0 as AInqty								,0.00 as AOutqty													,iif(b.StockType='B',b.qtyafter - b.qtybefore,0.00) as AAdjuqtQty
,0.0 as BInqty								,0.00 as BOutqty													,iif(b.StockType='I',b.qtyafter - b.qtybefore,0.00) as BAdjuqtQty
,0.0 as CInqty								,0.00 as COutqty													,iif(b.StockType='O',b.qtyafter - b.qtybefore,0.00) as CAdjuqtQty
from dbo.Adjust a inner join dbo.Adjust_Detail b on b.id = a.id 
union all
select 'BorrowBack-From',a.Type,a.Status,a.EditDate,b.Id,b.FromMDivisionID,b.FromPoId,b.Fromseq1,b.Fromseq2,b.FromRoll,b.FromDyelot 
,iif(b.FromStockType = 'O',b.Qty,0.00) as inqty	,b.Qty as outqty												,0.0 as adjustqty 
,0.0 as AInqty									,iif(b.FromStockType = 'B',b.Qty,0.00) as AOutQty				,0.00 AAdjustQty
,0.0 as BInqty									,iif(b.FromStockType = 'I',b.Qty,0.00) as BOutQty				,0.00 BAdjustQty
,0.0 AS CInqty									,iif(b.FromStockType = 'O',b.Qty,0.00) as COutQty				,0.00 CAdjustQty
from dbo.BorrowBack a inner join dbo.BorrowBack_Detail b on b.id = a.id 
union all
select 'BorrowBack-To',a.Type,a.Status,a.EditDate,b.Id,b.ToMDivisionID,b.ToPoId,b.ToSeq1,b.ToSeq2,b.ToRoll,b.ToDyelot 
,iif(b.ToStockType!='O',b.Qty,0.0) as inqty		,0.0 as outqty													,0.0 as adjustqty 
,iif(b.ToStockType='B',b.Qty,0.0) as AInqty		,0.00 as AOutqty												,0.00 AAdjuqtQty
,iif(b.ToStockType='I',b.Qty,0.0) as BInqty		,0.00 as BOutqty												,0.00 BAdjuqtQty
,iif(b.ToStockType='O',b.Qty,0.0) as CInqty		,0.00 as COutqty												,0.00 CAdjuqtQty
from dbo.BorrowBack a inner join dbo.BorrowBack_Detail b on b.id = a.id 
union all
select 'IssueLack',a.Type,a.Status,a.EditDate,b.id,b.MDivisionID,b.POID,b.seq1,b.seq2,b.Roll,b.Dyelot
,0.0 as inqty									,b.Qty as outqty												,0.0 as adjustqty 
,0.0 as AInqty									,iif(b.StockType = 'B',b.Qty,0.00) as AOutQty					,0.00 AAdjustQty
,0.0 as BInqty									,iif(b.StockType = 'I',b.Qty,0.00) as BOutQty					,0.00 BAdjustQty
,0.0 AS CInqty									,0.00 COutQty													,0.00 CAdjustQty
from dbo.IssueLack a inner join dbo.IssueLack_Detail b on b.id = a.id where a.Type='R'
union all
select 'RequestCrossM-Receive' tbname,a.Type,a.Status,a.EditDate,b.Id,b.MDivisionID,b.PoId,b.seq1,b.seq2,b.Roll,b.Dyelot
,b.Qty as inqty									,0.0 as outqty													,0.0 as adjustqty 
,iif(b.StockType='B',b.Qty,0.0) as AInqty		,0.00 AOutqty													,0.00 as AAdjuqtQty
,iif(b.StockType='I',b.Qty,0.0) as BInqty		,0.00 BOutqty													,0.00 as BAdjuqtQty
,0.0 as CInqty									,0.00 COutqty													,0.00 CAdjuqtQty
from dbo.RequestCrossM a inner join dbo.RequestCrossM_Receive b on b.id = a.id