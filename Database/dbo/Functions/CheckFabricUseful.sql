Create Function [dbo].[CheckFabricUseful]
(
	  @SCIRefno	    VarChar(30)				--
	 ,@SeasonId		VarChar(10)				-- Source seasonid, ex : Order 的 seasonid
	 ,@SuppID		VarChar(6)				-- 預設空白只查Fabric、且不判斷Lock
)
Returns Table
As
RETURN 
(
	Select Junk = iif(GetJunk.Junk = 1 and (ISNULL(GetMonth.Month,'') = '' or ISNULL(ss.Month,'') = '' or GetMonth.Month >= ss.Month), 1, 0)
	,Lock = GetLock.Lock
	,Msg = Case When isnull(GetExists.isExists, 0) = 1 and (GetJunk.Junk = 1 and (ISNULL(ss.Month,'') = '' or ISNULL(ss.Month,'') = '' OR GetMonth.Month >= ss.Month)) and GetLock.Lock = 1 Then '[Refno:' + f.Refno + ']isJunk and Lock.'
				When isnull(GetExists.isExists, 0) = 1 and GetLock.Lock = 1 Then '[Refno:' + f.Refno + '] is Lock.'
				When isnull(GetExists.isExists, 0) = 1 and GetJunk.Junk = 1 and (ISNULL(ss.Month,'') = '' OR GetMonth.Month >= ss.Month) Then '[Refno:' + f.Refno + '] is Junk.'
				When isnull(GetExists.isExists, 0) = 0 Then '[Refno:' + @SCIRefno + ', Supplier:' + @SuppID + '] is not found.'
				Else '' END
	from Fabric f
	outer apply ( select 1 as isexists,* from Fabric_Supp fs where fs.SCIRefno = f.SCIRefno and fs.SuppID = @SuppID) fs
	outer apply ( Select Month From Season Where fs.BrandID = Season.BrandID and fs.SeasonID = Season.ID ) ss
	outer apply ( select Junk = iif( @SuppID != '', fs.Junk, 0)) GetJunk
	outer apply ( select Lock = iif( @SuppID != '', fs.Lock, 0)) GetLock
	outer apply ( select isExists = iif(@SuppID != '', isnull(fs.isexists,0), 1)) GetExists
	outer apply ( select Month = month From Season Where id = @SeasonId and Brandid = f.BrandID) GetMonth
	Where f.SCIRefno = @SCIRefno
)