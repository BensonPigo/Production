

/*Check Current CTN is completed (Packng P27 Generate Query)*/
CREATE FUNCTION [dbo].[CheckShippingMarkStampSetting]
(
	@PackingListID varchar(15),
	@SCICtnNo varchar(15),
	@CtnRefno varchar(21),
	@CustCDID varchar(16),
	@BrandID varchar(8)
)
RETURNS INT
AS
BEGIN
	declare @BasicSettingNotYet bit = 0
			, @BasicSettingAlready bit = 1
			, @Finished bit = 1;

	DECLARE @ShippingMarkCombinationUkey bigint = null
			, @CheckBasicSetting bit = 0


	DECLARE @Std_ShippingMarkTypeList Table
		( ShippingMarkCombinationUkey bigint
			, ShippingMarkTypeUkey bigint
		);
		
	/*
		01 取得唯一的貼標組合
	*/
	select @ShippingMarkCombinationUkey = ISNULL(cc.StampCombinationUkey , def.Ukey)
	from [Production].[dbo].CustCD cc
	outer apply (
		select ukey = smc.Ukey
		from [Production].[dbo].ShippingMarkCombination smc
		where smc.BrandID = @BrandID
				and smc.Category = 'HTML'
				and smc.IsDefault = 1				
	) def
	where cc.ID = @CustCDID
			and cc.BrandID = @BrandID

	---- 若沒有設定貼標組合則回傳 0
		if (@ShippingMarkCombinationUkey is null)
			return @BasicSettingNotYet

	/*
		02 貼標組合底下涵蓋的標籤種類，作為標準值
	*/
	insert into @Std_ShippingMarkTypeList (ShippingMarkCombinationUkey, ShippingMarkTypeUkey)
	select smcd.ShippingMarkCombinationUkey
			, smcd.ShippingMarkTypeUkey
	from [Production].[dbo].ShippingMarkCombination_Detail smcd
	where smcd.ShippingMarkCombinationUkey = @ShippingMarkCombinationUkey

	---- 若組合沒有標籤種類則回傳 0
	select @CheckBasicSetting = iif (count(*) > 0, @BasicSettingAlready, @BasicSettingNotYet)
	from @Std_ShippingMarkTypeList

	if (@CheckBasicSetting = @BasicSettingNotYet)
		return @BasicSettingNotYet
		

	/*
		03 與標準相比，確認B03是否有"少"設定MarkType
	*/
	select @CheckBasicSetting = iif (count(*) > 0, @BasicSettingNotYet, @BasicSettingAlready)
	from @Std_ShippingMarkTypeList smtl
	where not exists (
				select *
				from [Production].[dbo].ShippingMarkPicture smp
				inner join [Production].[dbo].ShippingMarkPicture_Detail smpd on smp.Ukey = smpd.ShippingMarkPictureUkey
				INNER JOIN [Production].[dbo].ShippingMarkType_Detail td ON td.ShippingMarkTypeUkey = smpd.ShippingMarkTypeUkey and td.StickerSizeID = smpd.StickerSizeID
				where smp.Category = 'HTML'
						and smp.BrandID = @BrandID
						and smp.CTNRefno = @CtnRefno
						and smp.ShippingMarkCombinationUkey = smtl.ShippingMarkCombinationUkey
						and smpd.ShippingMarkTypeUkey = smtl.ShippingMarkTypeUkey
						AND td.TemplateName != ''
			)
	
	---- 有"少"設定則回傳 0
	if (@CheckBasicSetting = @BasicSettingNotYet)
		return @BasicSettingNotYet

		
	/*
		04 與標準相比，確認B03是否有"多"設定MarkType
	*/
	select @CheckBasicSetting = iif (count(*) > 0, @BasicSettingNotYet, @BasicSettingAlready)
	from [Production].[dbo].ShippingMarkPicture smp
	inner join [Production].[dbo].ShippingMarkPicture_Detail smpd on smp.Ukey = smpd.ShippingMarkPictureUkey
	INNER JOIN [Production].[dbo].ShippingMarkType_Detail td ON td.ShippingMarkTypeUkey = smpd.ShippingMarkTypeUkey and td.StickerSizeID = smpd.StickerSizeID
	where smp.Category = 'HTML'
			and smp.BrandID = @BrandID
			and smp.CTNRefno = @CtnRefno
			and smp.ShippingMarkCombinationUkey IN (select ShippingMarkCombinationUkey from @Std_ShippingMarkTypeList)
			AND td.TemplateName != ''
			AND NOT EXISTS(
				SELECT 1 FROM @Std_ShippingMarkTypeList
				WHERE ShippingMarkCombinationUkey = smp.ShippingMarkCombinationUkey  AND ShippingMarkTypeUkey= smpd.ShippingMarkTypeUkey
			)
	
			
	---- 有"多"設定則回傳 0
	if (@CheckBasicSetting = @BasicSettingNotYet)
		return @BasicSettingNotYet

	-- 以上都成功代表基本檔 + 圖檔皆已完成
	RETURN @Finished;
END
