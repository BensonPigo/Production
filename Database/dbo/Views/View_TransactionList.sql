CREATE  VIEW [dbo].[View_TransactionList]
AS
SELECT          'Receiving' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            b.StockQty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty
							, iif(b.StockType = 'B', b.StockQty, 0.0) AS AInqty, 0.00 AOutqty, 0.00 AS AAdjustQty
							, iif(b.StockType = 'I', b.StockQty, 0.0) AS BInqty, 0.00 BOutqty, 0.00 AS BAdjustQty
							, 0.0 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.Receiving a INNER JOIN
                            dbo.Receiving_Detail b ON b.id = a.id
UNION ALL
SELECT          iif(a.type < 'E', 'Issue', 'RequestCrossM-Issue'), a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, 
                            b.seq2, b.Roll, b.Dyelot, 0.0 AS inqty, b.Qty AS outqty, 0.0 AS adjustqty
							, 0.0 AS AInqty, iif(b.StockType = 'B', b.Qty,  0.00) AS AOutqty, 0.00 AS AAdjustQty
							, 0.0 AS BInqty, iif(b.StockType = 'I', b.Qty, 0.00) AS BOutqty, 0.00 AS BAdjustQty
							, 0.0 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.Issue a INNER JOIN
                            dbo.Issue_Detail b ON b.id = a.id
UNION ALL
SELECT          'IssueReturn' tbname, '', a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, 0 - b.Qty AS outqty, 0.0 AS adjustqty
							, 0.0 AS AInqty, iif(b.StockType = 'B', 0 - b.Qty, 0.00) AS AOutqty, 0.00 AS AAdjustQty
							, 0.0 AS BInqty, iif(b.StockType = 'I', 0 - b.Qty, 0.00) AS BOutqty, 0.00 AS BAdjustQty
							, 0.0 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.IssueReturn a INNER JOIN
                            dbo.IssueReturn_Detail b ON b.id = a.id
UNION ALL
SELECT          'SubTransfer-From' tbname, a.Type, a.Status, a.EditDate, b.Id, b.FromMDivisionID, b.FromPoId, b.Fromseq1, 
                            b.Fromseq2, b.FromRoll, b.FromDyelot, 0.0 AS inqty, 0.0 AS outqty, 0.0 AS adjustqty
							, 0.00 AS AInqty, b.Qty AS AOutqty, 0.00 AS AAdjustQty
							, 0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type = 'A'
UNION ALL
SELECT          'SubTransfer-From' tbname, a.Type, a.Status, a.EditDate, b.Id, b.FromMDivisionID, b.FromPoId, b.Fromseq1, 
                            b.Fromseq2, b.FromRoll, b.FromDyelot, 0.0 AS inqty, b.Qty AS outqty, 0.0 AS adjustqty
							, 0.0 AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, 0.0 AS BInqty, b.Qty AS BOutqty, 0.00 AS BAdjustQty
							, 0.0 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type IN ('B', 'E')
UNION ALL
SELECT          'SubTransfer-From' tbname, a.Type, a.Status, a.EditDate, b.Id, b.FromMDivisionID, b.FromPoId, b.Fromseq1, 
                            b.Fromseq2, b.FromRoll, b.FromDyelot, 0.0 AS inqty, b.Qty AS outqty, 0.0 AS adjustqty
							, 0.0 AS AInqty, b.Qty AS AOutqty, 0.00 AS AAdjustQty
							, 0.0 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.0 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type = 'D'
UNION ALL
SELECT          'SubTransfer-From' tbname, a.Type, a.Status, a.EditDate, b.Id, b.FromMDivisionID, b.FromPoId, b.Fromseq1, 
                            b.Fromseq2, b.FromRoll, b.FromDyelot, 0.0 AS inqty, 0 - b.Qty AS outqty, 0.0 AS adjustqty
							, 0.0 AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, 0.0 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.0 AS CInqty, b.Qty AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type = 'C'
UNION ALL
SELECT          'SubTransfer-To' tbname, a.Type, a.Status, a.EditDate, b.Id, b.ToMDivisionID, b.ToPoId, b.ToSeq1, b.ToSeq2, b.ToRoll, 
                            b.ToDyelot, 0.00 AS inqty, 0.00 AS outqty, 0.0 AS adjustqty
							, 0.00 AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, b.Qty AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type IN ('A', 'C')
UNION ALL
SELECT          'SubTransfer-To' tbname, a.Type, a.Status, a.EditDate, b.Id, b.ToMDivisionID, b.ToPoId, b.ToSeq1, b.ToSeq2, b.ToRoll, 
                            b.ToDyelot, 0.00 AS inqty, 0.00 AS outqty, 0.0 AS adjustqty
							, 0.00 AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, 0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, b.Qty AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type IN ('D', 'E')
UNION ALL
SELECT          'SubTransfer-To' tbname, a.Type, a.Status, a.EditDate, b.Id, b.ToMDivisionID, b.ToPoId, b.ToSeq1, b.ToSeq2, b.ToRoll, 
                            b.ToDyelot, b.Qty AS inqty, 0.00 AS outqty, 0.0 AS adjustqty
							, b.Qty AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, 0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.SubTransfer a INNER JOIN
                            dbo.SubTransfer_Detail b ON b.id = a.id
WHERE          a.Type = 'B'
UNION ALL
SELECT          'TransferIn' tbname, '', a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            b.Qty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty
							, iif(b.StockType = 'B', b.Qty, 0.00) AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, iif(b.StockType = 'I', b.Qty, 0.00) AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty,  0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.TransferIn a INNER JOIN
                            dbo.TransferIn_Detail b ON b.id = a.id
UNION ALL
SELECT          'TransferOut' tbname, '', a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, b.Qty AS outqty, 0.0 AS adjustqty
							, 0.00 AS AInqty, iif(b.StockType = 'B', b.Qty, 0.00) AS AOutqty, 0.00 AS AAdjustQty
							, 0.00 AS BInqty, iif(b.StockType = 'I', b.Qty, 0.00) AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.TransferOut a INNER JOIN
                            dbo.TransferOut_Detail b ON b.id = a.id
UNION ALL
SELECT          'Adjust' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, 0.0 AS outqty, b.qtyafter - b.qtybefore AS adjustqty
							, 0.00 AS AInqty, 0.00 AS AOutqty, b.qtyafter - b.qtybefore AS AAdjustQty
							, 0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.Adjust a INNER JOIN
                            dbo.Adjust_Detail b ON b.id = a.id
WHERE          a.Type = 'A'
UNION ALL
SELECT          'Adjust' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, 0.0 AS outqty, b.qtyafter - b.qtybefore AS adjustqty
							, 0.00 AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, 0.00 AS BInqty, 0.00 AS BOutqty, b.qtyafter - b.qtybefore AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.Adjust a INNER JOIN
                            dbo.Adjust_Detail b ON b.id = a.id
WHERE          a.Type = 'B'
UNION ALL
SELECT          'Adjust' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, 0.0 AS outqty, 0 AS adjustqty
							, 0.00 AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, 0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, b.qtyafter - b.qtybefore AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.Adjust a INNER JOIN
                            dbo.Adjust_Detail b ON b.id = a.id
WHERE          b.StockType = 'O'
UNION ALL
SELECT          'BorrowBack-From' tbname, a.Type, a.Status, a.EditDate, b.Id, b.FromMDivisionID, b.FromPoId, b.Fromseq1, 
                            b.Fromseq2, b.FromRoll, b.FromDyelot, 0.0 AS inqty, b.qty AS outqty, 0.0 AS adjustqty
							, 0.00 AS AInqty, iif(b.FromStockType = 'B', b.Qty, 0.00) AS AOutqty, 0.00 AS AAdjustQty
							, 0.00 AS BInqty, iif(b.FromStockType = 'I', b.Qty, 0.00) AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.BorrowBack a INNER JOIN
                            dbo.BorrowBack_Detail b ON b.id = a.id
UNION ALL
SELECT          'BorrowBack-To' tbname, a.Type, a.Status, a.EditDate, b.Id, b.ToMDivisionID, b.ToPoId, b.ToSeq1, b.ToSeq2, b.ToRoll, 
                            b.ToDyelot, b.Qty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty
							, iif(b.ToStockType = 'B', b.Qty, 0.00) AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, iif(b.ToStockType = 'I', b.Qty, 0.00) AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.BorrowBack a INNER JOIN
                            dbo.BorrowBack_Detail b ON b.id = a.id
UNION ALL
SELECT          'IssueLack' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0.0 AS inqty, b.Qty AS outqty, 0.0 AS adjustqty
							, 0.00 AS AInqty, b.Qty AS AOutqty, 0.00 AS AAdjustQty
							, 0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.IssueLack a INNER JOIN
                            dbo.IssueLack_Detail b ON b.id = a.id
WHERE          a.Type = 'R'
UNION ALL
SELECT          'RequestCrossM-Receive' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, 
                            b.Dyelot, b.Qty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty
							, b.Qty AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, 0.00 AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.RequestCrossM a INNER JOIN
                            dbo.RequestCrossM_Receive b ON b.id = a.id
WHERE          a.Type IN ('B', 'D')
UNION ALL
SELECT          'RequestCrossM-Receive' tbname, a.Type, a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, 
                            b.Dyelot, b.Qty AS inqty, 0.0 AS outqty, 0.0 AS adjustqty
							, iif(b.StockType = 'B', b.Qty, 0.00) AS AInqty, 0.00 AS AOutqty, 0.00 AS AAdjustQty
							, iif(b.StockType = 'I', b.Qty, 0.00) AS BInqty, 0.00 AS BOutqty, 0.00 AS BAdjustQty
							, 0.00 AS CInqty, 0.00 AS COutqty, 0.00 AS CAdjustQty
							, 0.00 AS ReturnQty, 0.00 as AReturnQty, 0.00 as BReturnQty, 0.00 as CReturnQty
FROM              dbo.RequestCrossM a INNER JOIN
                            dbo.RequestCrossM_Receive b ON b.id = a.id
WHERE          a.Type = 'G'
UNION ALL
SELECT          'ReturnReceipt' tbname, '', a.Status, a.EditDate, b.Id, b.MDivisionID, b.PoId, b.seq1, b.seq2, b.Roll, b.Dyelot, 
                            0 AS inqty, 0.0 AS outqty, 0.0 AS adjustqty, 0.0 AS AInqty, 0.00 AOutqty, 
                            0.00 AS AAdjuqtQty, 0.00 AS BInqty, 0.00 BOutqty, 0.00 AS BAdjuqtQty, 0.0 AS CInqty, 
                            0.00 COutqty, 0.00 CAdjuqtQty
							, [ReturnQty] = b.Qty
							, [AReturnQty] = iif(b.StockType = 'B', b.Qty, 0.00)
							, [BReturnQty] = iif(b.StockType = 'I', b.Qty, 0.00)
							, [CReturnQty] = 0.00
FROM              dbo.ReturnReceipt a INNER JOIN
                            dbo.ReturnReceipt_Detail b ON b.id = a.id



GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'View_TransactionList';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[7] 4[11] 2[64] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'View_TransactionList';

