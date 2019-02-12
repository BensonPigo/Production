-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Modify Pass1 && Pass2>
-- =============================================
Create PROCEDURE [dbo].[Update_PassFunction]

	
AS
BEGIN

	SET NOCOUNT ON;

--1.更新Pass1.Position
update Pass1
set Pass1.Position=pass0.ID
from dbo.Pass1 
inner join pass0  on Pass1.FKPass0=pass0.PKey 

--更新Pass2 
/*
Pass2.FKMenu=MenuDetail.Pkey 
1.調整Pass2 MenuName,BarPrompt(MenuDetail_Name)
2.如果MenuDetail 未開放的功能,權限Pass2也需要與 MenuDetail相同
*/
Begin 
	Merge dbo.Pass2 as T
	using (SELECT Menu.MenuName, Menu.MenuNo, MenuDetail.*
	FROM Menu, MenuDetail
	WHERE Menu.PKey = MenuDetail.UKey AND MenuDetail.ForMISOnly = 0 AND MenuDetail.ObjectCode = 0) as S
	on T.FKMenu=S.PKey
	when matched then
	update set
		T.MenuName=S.MenuName,
		T.BarPrompt=S.BarPrompt,
		T.CanNew=iif(S.CanNew=0 and T.CanNew=1,0,T.CanNew),
		T.CanEdit=iif(S.CanEdit=0 and T.CanEdit=1,0,T.CanEdit),
		T.CanDelete=iif(S.CanDelete=0 and T.CanDelete=1,0,T.CanDelete),
		T.CanPrint=iif(S.CanPrint=0 and T.CanPrint=1,0,T.CanPrint),
		T.CanConfirm=iif(S.CanConfirm=0 and T.CanConfirm=1,0,T.CanConfirm),
		T.CanUnConfirm=iif(S.CanUnConfirm=0 and T.CanUnConfirm=1,0,T.CanUnConfirm),
		T.CanSend=iif(S.CanSend=0 and T.CanSend=1,0,T.CanSend),
		T.CanRecall=iif(S.CanRecall=0 and T.CanRecall=1,0,T.CanRecall),
		T.CanCheck=iif(S.CanCheck=0 and T.CanCheck=1,0,T.CanCheck),
		T.CanUnCheck=iif(S.CanUnCheck=0 and T.CanUnCheck=1,0,T.CanUnCheck),
		T.CanClose=iif(S.CanClose=0 and T.CanClose=1,0,T.CanClose),
		T.CanUnClose=iif(S.CanUnClose=0 and T.CanUnClose=1,0,T.CanUnClose),
		T.CanReceive=iif(S.CanReceive=0 and T.CanReceive=1,0,T.CanReceive),
		T.CanReturn=iif(S.CanReturn=0 and T.CanReturn=1,0,T.CanReturn),
		T.CanJunk=iif(S.CanJunk=0 and T.CanJunk=1,0,T.CanJunk);
End

/*
不存在於Pass2 的FKMenu,就需要insert資料進Pass2
*/
if exists(
SELECT 1
FROM Menu, MenuDetail
WHERE Menu.PKey = MenuDetail.UKey AND MenuDetail.ForMISOnly = 0 AND MenuDetail.ObjectCode = 0
and not exists (select 1 from Pass2 where FKMenu=MenuDetail.PKey)
)
Begin
	insert into Pass2([FKPass0]
		  ,[MenuName]
		  ,[FKMenu]
		  ,[BarPrompt]
		  ,[Used])
	SELECT pass0.PKey,Menu.MenuName,MenuDetail.PKey,MenuDetail.BarPrompt,'' Used
	FROM pass0,Menu, MenuDetail
	WHERE Menu.PKey = MenuDetail.UKey AND MenuDetail.ForMISOnly = 0 AND MenuDetail.ObjectCode = 0
	and not exists (select 1 from Pass2 where FKMenu=MenuDetail.PKey)
End

/*
異動到原本的menudetail.Pkey ****並且未修改MenuName****
ex:修改name 並且原本的form 移動到Submenu or 其他模組

刪除Pass2.FKMenu 不存在於meunDetail的資料
如果有相同的MenuName && FKPass0 就將原本的Pass2權限update到新資料

*/
if exists(
select 1
from Pass2
where not exists (select 1 from MenuDetail where PKey=pass2.FKMenu and BarPrompt=pass2.BarPrompt)
)
Begin 
	--backup 
	select * 
	into #Pass2Delete
	from Pass2
	where not exists (select 1 from MenuDetail where PKey=pass2.FKMenu)

	delete from Pass2
	where not exists (select 1 from MenuDetail where PKey=pass2.FKMenu)

	--將舊的權限補上
	merge pass2 as t
	using(select * from #Pass2Delete) as s
	on t.BarPrompt=s.BarPrompt and t.FKPass0=s.FKPass0
	when matched then
	update set 
	t.Used= s.Used,
	t.CanNew= s.CanNew,
	t.CanEdit= s.CanEdit,
	t.CanDelete= s.CanDelete,
	t.CanPrint= s.CanPrint,
	t.CanConfirm= s.CanConfirm,
	t.CanUnConfirm= s.CanUnConfirm,
	t.CanSend= s.CanSend,
	t.CanRecall= s.CanRecall,
	t.CanCheck= s.CanCheck,
	t.CanUnCheck= s.CanUnCheck,
	t.CanClose= s.CanClose;

	drop table #Pass2Delete

End

END