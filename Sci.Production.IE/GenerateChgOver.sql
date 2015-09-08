--�R�����s�bSewing Schedule�����
delete from ChgOver 
where --Status <> 'Closed' and
      not exists (select 1 
            from SewingSchedule s 
			where s.APSNo = ChgOver.APSNo 
			and s.OrderID = ChgOver.OrderID 
			and s.ComboType = ChgOver.ComboType
			and s.FactoryID = ChgOver.FactoryID
			and s.SewingLineID = ChgOver.SewingLineID);

--��s�{�����
update ChgOver set Inline = s.Inline,AlloQty = s.AlloQty,StandardOutput = s.StandardOutput,TotalSewingTime = s.TotalSewingTime
from SewingSchedule s where s.APSNo = ChgOver.APSNo 
					  and s.OrderID = ChgOver.OrderID
					  and s.ComboType = ChgOver.ComboType
					  and s.FactoryID = ChgOver.FactoryID
					  and s.SewingLineID = ChgOver.SewingLineID;
					  --and ChgOver.Status <> 'Closed';
update ChgOver set StyleID = o.StyleID,CDCodeID = o.CdCodeID
from Orders o where o.ID = ChgOver.OrderID; --and ChgOver.Status <> 'Closed';

--����ChgOver���
Declare cursor_tmpSewing Cursor for
select s.FactoryID,s.SewingLineID,s.Inline,s.APSNo,s.ComboType,s.AlloQty,s.TotalSewingTime,s.StandardOutput,
isnull(o.StyleID,'') as StyleID,isnull(o.SeasonID,'') as SeasonID,o.CdCodeID,s.OrderID,
LAG(isnull(o.StyleID,'')+s.ComboType,1,'') OVER (Partition by s.FactoryID,s.SewingLineID Order by s.FactoryID,s.SewingLineID,s.Inline) as Compare
from SewingSchedule s
left join Orders o on s.OrderID = o.ID
where s.Inline is not null and s.Offline > DATEADD(MONTH,-1,GETDATE()) order by s.FactoryID,s.SewingLineID,s.Inline;


--�ŧi�ܼ�: �O���{���������
DECLARE @factoryid VARCHAR(8), --�u�t�O
		@sewinglineid VARCHAR(2), --Sewing Line ID
        @inline DATETIME, --�W�u�� 
		@apsno INT, --APS�t��Sewing Schedule��ID
		@combotype VARCHAR(1), --�զX���A
		@alloqty INT, --�Ͳ��ƶq
		@ttlsewingtime INT, --�`���
		@stdoutput INT, --Standard Output 
		@styleid VARCHAR(15), --�ڦ�
		@seasonid VARCHAR(10), --�u�`
		@cdcodeid VARCHAR(6), --CD Code
		@orderid VARCHAR(13), --�q��s��
		@compare VARCHAR(26), --�����W�@����StyleID+ComboType
		@type VARCHAR(1), --New/Repeat
		@chgoverid INT, --����ChgOver.ID
		@chgoverinline DATETIME, --����ChgOver.Inline
		@chgoverstatus VARCHAR(15) --����ChgOver.Status

--�}�lrun cursor
OPEN cursor_tmpSewing
--�N�Ĥ@����ƶ�J�ܼ�
FETCH NEXT FROM cursor_tmpSewing INTO @factoryid,@sewinglineid,@inline,@apsno,@combotype,@alloqty,@ttlsewingtime,@stdoutput,@styleid,@seasonid,@cdcodeid,@orderid,@compare
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @compare <> ''
		BEGIN
			IF @styleid+@combotype <> @compare --���ڦ�
				BEGIN
					IF EXISTS(select 1 from ChgOver where APSNo = @apsno and FactoryID = @factoryid and SewingLineID = @sewinglineid and OrderID = @orderid and ComboType = @combotype)
						BEGIN
							select @chgoverid = ID,@chgoverinline = Inline,@chgoverstatus = Status 
							from ChgOver 
							where FactoryID = @factoryid and SewingLineID = @sewinglineid and StyleID = @styleid and ComboType = @combotype;
							IF @chgoverinline < @inline
								SET @type = 'R'
							ELSE
								BEGIN
									SET @type = 'N'
									IF @chgoverstatus <> 'Closed'
										update ChgOver set Type = 'R' where ID = @chgoverid
								END
							select @chgoverid = ID from ChgOver where APSNo = @apsno and FactoryID = @factoryid and SewingLineID = @sewinglineid and OrderID = @orderid and ComboType = @combotype
							update ChgOver set Type = @type, Inline = @inline where ID = @chgoverid
						END
					ELSE
						BEGIN
							select @chgoverid = ID,@chgoverinline = Inline,@chgoverstatus = Status 
							from ChgOver 
							where FactoryID = @factoryid and SewingLineID = @sewinglineid and StyleID = @styleid and ComboType = @combotype;
							IF @chgoverinline < @inline
								SET @type = 'R'
							ELSE
								BEGIN
									SET @type = 'N'
									IF @chgoverstatus <> 'Closed'
										update ChgOver set Type = 'R' where ID = @chgoverid
								END
							insert into ChgOver (OrderID,ComboType,FactoryID,StyleID,SeasonID,SewingLineID,CDCodeID,Inline,TotalSewingTime,AlloQty,StandardOutput,Type,Status,AddDate)
							values (@orderid,@combotype,@factoryid,@styleid,@seasonid,@sewinglineid,@cdcodeid,@inline,@ttlsewingtime,@alloqty,@stdoutput,@type,'NEW',GETDATE())
						END
				END
		END
	FETCH NEXT FROM cursor_tmpSewing INTO @factoryid,@sewinglineid,@inline,@apsno,@combotype,@alloqty,@ttlsewingtime,@stdoutput,@styleid,@seasonid,@cdcodeid,@orderid,@compare
END

--�R���s��StyleID+ComboType
select *
from (select ID,StyleID,ComboType,FactoryID,SewingLineID,StyleID+ComboType as CurrentRec,LAG(StyleID+ComboType,1,'') OVER (Partition by FactoryID,SewingLineID Order by FactoryID,SewingLineID,Inline,ID) as LastRec
	  from ChgOver) a
where a.LastRec <> ''
and a.CurrentRec = a.LastRec;

--�R���w���s�bChgOver��ChgOver_Check & ChgOver_Problem
delete from ChgOver_Check where not exists (select 1 from ChgOver where ChgOver.ID = ChgOver_Check.ID);
delete from ChgOver_Problem where not exists (select 1 from ChgOver where ChgOver.ID = ChgOver_Problem.ID);


--��Category����
update ChgOver set Category = c.Category
from (select ID, (case when b.ProductionType <> b.LastProdType and b.FabricType <> b.LastFabType then 'A'
					   when b.ProductionType <> b.LastProdType and b.FabricType = b.LastFabType then 'B'
					   when b.ProductionType = b.LastProdType and b.FabricType <> b.LastFabType then 'C'
					   when b.ProductionType = b.LastProdType and b.FabricType = b.LastFabType then 'D'
					   else ''
				  end) as Category
	  from (select ID,ProductionType,FabricType,LAG(ProductionType,1,'') OVER (Partition by a.FactoryID,a.SewingLineID order by a.FactoryID,a.SewingLineID,a.Inline) as LastProdType,
				LAG(FabricType,1,'') OVER (Partition by a.FactoryID,a.SewingLineID order by a.FactoryID,a.SewingLineID,a.Inline) as LastFabType
			from (select co.ID,co.FactoryID,co.SewingLineID,co.StyleID,co.ComboType,co.Inline,
					  isnull(case when co.ComboType = 'T' then cc.TopProductionType when co.ComboType = 'B' then cc.BottomProductionType when co.ComboType = 'I' then cc.InnerProductionType when co.ComboType = 'O' then cc.OuterProductionType else '' end,'') as ProductionType,
					  isnull(case when co.ComboType = 'T' then cc.TopFabricType when co.ComboType = 'B' then cc.BottomFabricType when co.ComboType = 'I' then cc.InnerFabricType when co.ComboType = 'O' then cc.OuterFabricType else '' end,'') as FabricType
				  from ChgOver co
				  left join CDCode_Content cc on co.CDCodeID = cc.ID) a) b
	  where b.LastProdType <> '') c
where c.ID = ChgOver.ID
