
CREATE view [dbo].[View_TransactionList]
AS
SELECT          'Receiving' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            b.StockQty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty, iif(b.StockType = 'B', b.StockQty, 0.0) AS AInqty, 0.00 AOutqty, 
                            0.00 AS AAdjustQty, iif(b.StockType = 'I', b.StockQty, 0.0) AS BInqty, 0.00 BOutqty, 0.00 AS BAdjustQty, 0.0 AS CInqty, 
                            0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.Receiving a INNER JOIN
                            dbo.Receiving_Detail b ON b.id = a.id
UNION ALL
SELECT          iif(a.type < 'E', 'Issue', 'RequestCrossM-Issue'), a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, 
                            b.seq2, b.Roll, b.Dyelot, 0.0 AS inqty, b.Qty AS outqty, 0.0 AS adjustqty, 0.0 AS AInqty, iif(b.StockType = 'B', b.Qty, 
                            0.00) AS AOutqty, 0.00 AS AAdjustQty, 0.0 AS BInqty, iif(b.StockType = 'I', b.Qty, 0.00) AS BOutqty, 0.00 AS BAdjustQty, 
                            0.0 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.Issue a INNER JOIN
                            dbo.Issue_Detail b ON b.id = a.id
UNION ALL
SELECT          'IssueReturn' tbname, '', a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, 0 - b.Qty AS outqty, 0.0 AS adjustqty, 0.0 AS AInqty, iif(b.StockType = 'B', 0 - b.Qty, 0.00) AS AOutqty, 
                            0.00 AS AAdjustQty, 0.0 AS BInqty, iif(b.StockType = 'I', 0 - b.Qty, 0.00) AS BOutqty, 0.00 AS BAdjustQty, 0.0 AS CInqty, 
                            0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.IssueReturn a INNER JOIN
                            dbo.IssueReturn_Detail b ON b.id = a.id
UNION ALL
SELECT          'SubTransfer-From' tbname, a.Type, a.Status, a.EditDate, b.Id, b.FromMDivisionID, b.FromPoId, b.Fromseq1, 
                            b.Fromseq2, b.FromRoll, b.FromDyelot, 0.0 AS inqty, 0.0 AS outqty, 0.0 AS adjustqty, 0.00 AS AInqty, b.Qty AS AOutqty,
                             0.00 AS AAdjustQty, 0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 0.00 AS CInqty, 0.00 AS COutqty, 
                            0.00 AS CAdjustQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type = 'A'
UNION ALL

SELECT          'SubTransfer-From' tbname, a.Type, a.Status, a.EditDate, b.Id, b.FromMDivisionID, b.FromPoId, b.Fromseq1, 
                            b.Fromseq2, b.FromRoll, b.FromDyelot, 
							0.0 AS inqty, b.Qty AS outqty, 0.0 AS adjustqty, 
							0.0 AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty, 
							0.0 AS BInqty, b.Qty AS BOutqty, 0.00 AS BAdjustQty, 
							0.0 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type IN ('B', 'E')

UNION ALL

SELECT          'SubTransfer-From' tbname, a.Type, a.Status, a.EditDate, b.Id, b.FromMDivisionID, b.FromPoId, b.Fromseq1, 
                            b.Fromseq2, b.FromRoll, b.FromDyelot, 
							0.0 AS inqty, b.Qty AS outqty, 0.0 AS adjustqty, 
							0.0 AS AInqty, b.Qty AS AOutqty, 0.00 AS AAdjustQty, 
							0.0 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 
							0.0 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type ='D'

UNION ALL

SELECT          'SubTransfer-From' tbname, a.Type, a.Status, a.EditDate, b.Id, b.FromMDivisionID, b.FromPoId, b.Fromseq1, 
                            b.Fromseq2, b.FromRoll, b.FromDyelot, 
							0.0 AS inqty, 0 - b.Qty AS outqty, 0.0 AS adjustqty, 
							0.0 AS AInqty,0.00 AS AOutqty, 0.00 AS AAdjustQty, 
							0.0 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 
							0.0 AS CInqty, b.Qty AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type = 'C'

UNION ALL

SELECT          'SubTransfer-To' tbname, a.Type, a.Status, a.EditDate, b.Id, b.ToMDivisionID, b.ToPoId, b.ToSeq1, b.ToSeq2, b.ToRoll, 
                            b.ToDyelot, 
							0.00 AS inqty, 0.00 AS outqty, 0.0 AS adjustqty, 
							0.00 AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty, 
                            b.Qty AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 
							0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type IN ('A', 'C')

UNION ALL

SELECT          'SubTransfer-To' tbname, a.Type, a.Status, a.EditDate, b.Id, b.ToMDivisionID, b.ToPoId, b.ToSeq1, b.ToSeq2, b.ToRoll, 
                            b.ToDyelot, 
							0.00 AS inqty, 0.00 AS outqty, 0.0 AS adjustqty, 
							0.00 AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty, 
                            0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 
							b.Qty AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type IN ('D', 'E')

UNION ALL

SELECT          'SubTransfer-To' tbname, a.Type, a.Status, a.EditDate, b.Id, b.ToMDivisionID, b.ToPoId, b.ToSeq1, b.ToSeq2, b.ToRoll, 
                            b.ToDyelot, b.Qty AS inqty, 0.00 AS outqty, 0.0 AS adjustqty, b.Qty AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty, 
                            0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type = 'B'

UNION ALL
SELECT          'TransferIn' tbname, '', a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            b.Qty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty, iif(b.StockType = 'B', b.Qty, 0.00) AS AInqty, 0.00 AS AOutqty, 
                            0.00 AS AAdjustQty, iif(b.StockType = 'I', b.Qty, 0.00) AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 0.00 AS CInqty, 
                            0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.TransferIn a INNER JOIN
                            dbo.TransferIn_Detail b ON b.id = a.id
UNION ALL
SELECT          'TransferOut' tbname, '', a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, b.Qty AS outqty, 0.0 AS adjustqty, 0.00 AS AInqty, iif(b.StockType = 'B', b.Qty, 0.00) AS AOutqty, 
                            0.00 AS AAdjustQty, 0.00 AS BInqty, iif(b.StockType = 'I', b.Qty, 0.00) AS BOutqty, 0.00 AS BAdjustQty, 0.00 AS CInqty, 
                            0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.TransferOut a INNER JOIN
                            dbo.TransferOut_Detail b ON b.id = a.id
UNION ALL
SELECT          'Adjust' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, 0.0 AS outqty, b.qtyafter - b.qtybefore AS adjustqty, 0.00 AS AInqty, 0.00 AS AOutqty, 
                            b.qtyafter - b.qtybefore AS AAdjustQty, 0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 0.00 AS CInqty, 
                            0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.Adjust a INNER JOIN
                            dbo.Adjust_Detail b ON b.id = a.id
WHERE          a.Type = 'A'
UNION ALL
SELECT          'Adjust' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, 0.0 AS outqty, b.qtyafter - b.qtybefore AS adjustqty, 0.00 AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty, 
                            0.00 AS BInqty, 0.00 AS BOutqty, b.qtyafter - b.qtybefore AS BAdjustQty, 0.00 AS CInqty, 0.00 AS COutqty, 
                            0.00 AS CAdjustQty
FROM              dbo.Adjust a INNER JOIN
                            dbo.Adjust_Detail b ON b.id = a.id
WHERE          a.Type = 'B'
UNION ALL
SELECT          'BorrowBack-From' tbname, a.Type, a.Status, a.EditDate, b.Id, b.FromMDivisionID, b.FromPoId, b.Fromseq1, 
                            b.Fromseq2, b.FromRoll, b.FromDyelot, 0.0 AS inqty, b.qty AS outqty, 0.0 AS adjustqty, 0.00 AS AInqty, 
                            iif(b.FromStockType = 'B', b.Qty, 0.00) AS AOutqty, 0.00 AS AAdjustQty, 0.00 AS BInqty, iif(b.FromStockType = 'I', b.Qty,
                             0.00) AS BOutqty, 0.00 AS BAdjustQty, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.BorrowBack a INNER JOIN
                            dbo.BorrowBack_Detail b ON b.id = a.id
UNION ALL
SELECT          'BorrowBack-To' tbname, a.Type, a.Status, a.EditDate, b.Id, b.ToMDivisionID, b.ToPoId, b.ToSeq1, b.ToSeq2, b.ToRoll, 
                            b.ToDyelot, b.Qty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty, b.Qty AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty, 
                            0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.BorrowBack a INNER JOIN
                            dbo.BorrowBack_Detail b ON b.id = a.id
UNION ALL
SELECT          'IssueLack' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, b.Qty AS outqty, 0.0 AS adjustqty, 0.00 AS AInqty, b.Qty AS AOutqty, 0.00 AS AAdjustQty, 0.00 AS BInqty, 
                            0.00 AS BOutqty, 0.00 AS BAdjustQty, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.IssueLack a INNER JOIN
                            dbo.IssueLack_Detail b ON b.id = a.id
WHERE          a.Type = 'R'
UNION ALL
SELECT          'RequestCrossM-Receive' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, 
                            b.Dyelot, b.Qty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty, b.Qty AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty, 
                            0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.RequestCrossM a INNER JOIN
                            dbo.RequestCrossM_Receive b ON b.id = a.id
WHERE          a.Type IN ('B', 'D')
UNION ALL
SELECT          'RequestCrossM-Receive' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, 
                            b.Dyelot, b.Qty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty, iif(b.StockType = 'B', b.Qty, 0.00) AS AInqty, 0.00 AS AOutqty, 
                            0.00 AS AAdjustQty, iif(b.StockType = 'I', b.Qty, 0.00) AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty, 0.00 AS CInqty, 
                            0.00 AS COutqty, 0.00 AS CAdjustQty
FROM              dbo.RequestCrossM a INNER JOIN
                            dbo.RequestCrossM_Receive b ON b.id = a.id
WHERE          a.Type = 'G'
UNION ALL
SELECT          'ReturnReceipt' tbname, '', a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0 - b.Qty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty, iif(b.StockType = 'B', 0 - b.Qty, 0.0) AS AInqty, 0.00 AOutqty, 
                            0.00 AS AAdjuqtQty, iif(b.StockType = 'I', 0 - b.Qty, 0.0) AS BInqty, 0.00 BOutqty, 0.00 AS BAdjuqtQty, 0.0 AS CInqty, 
                            0.00 COutqty, 0.00 CAdjuqtQty
FROM              dbo.ReturnReceipt a INNER JOIN
                            dbo.ReturnReceipt_Detail b ON b.id = a.id