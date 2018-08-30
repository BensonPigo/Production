CREATE VIEW dbo.View_PackingP03
AS
SELECT          BuyerDelivery =
                                (SELECT          min(Orders.BuyerDelivery)
                                  FROM               PackingList_Detail INNER JOIN
                                                              Orders ON PackingList_Detail.OrderID = Orders.ID
                                  WHERE           PackingList_Detail.id = PackingList.id), SP = STUFF
                                ((SELECT          CONCAT(',', OrderID)
                                    FROM              (SELECT DISTINCT PackingList_Detail.OrderID
                                                                FROM               PackingList_Detail
                                                                WHERE           PackingList_Detail.id = PackingList.id) s FOR xml path('')), 1, 1, ''), [Cancel] =
                                (SELECT          [Cancel] = CASE WHEN count(*) > 0 THEN 'Y' ELSE 'N' END
                                  FROM               orders o INNER JOIN
                                                              PackingList_Detail p2 ON o.ID = p2.OrderID
                                  WHERE           o.Junk = 1 AND p2.ID = PackingList.ID), factory =
                                (SELECT          STUFF
                                                                  ((SELECT          CONCAT(',', FtyGroup)
                                                                      FROM              (SELECT DISTINCT FtyGroup
                                                                                                  FROM               orders o
                                                                                                  WHERE           o.id IN
                                                                                                                                  (SELECT DISTINCT PackingList_Detail.OrderID
                                                                                                                                    FROM               PackingList_Detail
                                                                                                                                    WHERE           PackingList_Detail.id = PackingList.id)) s FOR 
                                                                                                  xml path('')), 1, 1, '')), PONO = STUFF
                                ((SELECT          CONCAT(',', CustPONo)
                                    FROM              (SELECT DISTINCT Orders.CustPONo
                                                                FROM               PackingList_Detail INNER JOIN
                                                                                            Orders ON PackingList_Detail.OrderID = Orders.ID
                                                                WHERE           PackingList_Detail.id = PackingList.id AND Orders.CustPONo IS NOT NULL AND 
                                                                                            Orders.CustPONo != '') s FOR xml path('')), 1, 1, ''), Concat(PackingList.NW, '') AS NW2, 
                            Concat(PackingList.GW, '') AS GW2, Concat(PackingList.CBM, '') AS CBM2, 
                            PurchaseCTN = IIF(PackingList.LocalPoid = '' OR
                            PackingList.LocalPoid IS NULL, '', 'Y'), *
FROM              PackingList
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'View_PackingP03';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[34] 4[3] 2[38] 3) )"
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
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'View_PackingP03';





