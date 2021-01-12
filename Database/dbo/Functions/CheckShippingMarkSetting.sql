USE [Production]
GO

CREATE FUNCTION [dbo].[CheckShippingMarkSetting]
(
	@PackingListID varchar(15),
	@SCICtnNo varchar(15),
	@CtnRefno varchar(21),
	@IsMixPack bit,
	@CustCDID varchar(16),
	@BrandID varchar(8)
)
RETURNS int
AS
BEGIN
	declare @BasicSettingNotYet bit = 0
			, @BasicSettingAlready bit = 1
			--, @PictureUploadNotYet bit = 0
			, @PictureUploadAlready bit = 1
			, @Finished bit = 1;

	DECLARE @ShippingMarkCombinationUkey bigint = null
			, @CheckBasicSetting bit = 0
			, @CheckPicture bit = 0;


	DECLARE @ShippingMarkTypeList Table
		( ShippingMarkCombinationUkey bigint
			, ShippingMarkTypeUkey bigint
		);
	
	/*
		01 取得為一個貼標組合
	*/
	select @ShippingMarkCombinationUkey = (case @IsMixPack
												when 1 then iif (cc.StickerCombinationUkey_MixPack is null, def.Ukey, cc.StickerCombinationUkey_MixPack)
												when 0 then iif (cc.StickerCombinationUkey is null, def.Ukey, cc.StickerCombinationUkey)
												else null
											end)
	from CustCD cc
	outer apply (
		select ukey = smc.Ukey
		from ShippingMarkCombination smc
		where smc.BrandID = @BrandID
				and smc.Category = 'PIC'
				and smc.IsMixPack = @IsMixPack
				and smc.IsDefault = 1				
	) def
	where cc.ID = @CustCDID
			and cc.BrandID = @BrandID

	---- 若沒有設定貼標組合則回傳 0
		if (@ShippingMarkCombinationUkey is null)
			return @BasicSettingNotYet

	/*
		02 貼標組合底下涵蓋的標籤種類
	*/
	insert into @ShippingMarkTypeList (ShippingMarkCombinationUkey, ShippingMarkTypeUkey)
	select smcd.ShippingMarkCombinationUkey
			, smcd.ShippingMarkTypeUkey
	from ShippingMarkCombination_Detail smcd
	where smcd.ShippingMarkCombinationUkey = @ShippingMarkCombinationUkey

	---- 若組合沒有標籤種類則回傳 0
	select @CheckBasicSetting = iif (count(*) > 0, @BasicSettingAlready, @BasicSettingNotYet)
	from @ShippingMarkTypeList

	if (@CheckBasicSetting = @BasicSettingNotYet)
		return @BasicSettingNotYet

	/*
		03 確認該紙箱 + 貼標組合 + 貼標種類是否都有設定位置基本檔
	*/
	select @CheckBasicSetting = iif (count(*) > 0, @BasicSettingNotYet, @BasicSettingAlready)
	from @ShippingMarkTypeList smtl
	where not exists (
				select *
				from ShippingMarkPicture smp
				inner join ShippingMarkPicture_Detail smpd on smp.Ukey = smpd.ShippingMarkPictureUkey
				where smp.Category = 'PIC'
						and smp.BrandID = @BrandID
						and smp.CTNRefno = @CtnRefno
						and smp.ShippingMarkCombinationUkey = smtl.ShippingMarkCombinationUkey
						and smpd.ShippingMarkTypeUkey = smtl.ShippingMarkTypeUkey
			)
	
	---- 若紙箱沒有設定標籤位置則回傳 0
	if (@CheckBasicSetting = @BasicSettingNotYet)
		return @BasicSettingNotYet

	/*
		04 確認TForMer 的範本是否都有上傳(僅檢查FromTemplate=1)
	*/
	SELECT @CheckBasicSetting = iif (count(*) > 0, @BasicSettingNotYet, @BasicSettingAlready)
	FROM ShippingMarkPicture pict
	INNER JOIN ShippingMarkPicture_Detail pictD ON pict.Ukey = pictD.ShippingMarkPictureUkey
	LEFT JOIN ShippingMarkType t ON t.Ukey = pictD.ShippingMarkTypeUkey
	LEFT JOIN ShippingMarkType_Detail td ON t.Ukey = td.ShippingMarkTypeUkey AND td.StickerSizeID = pictD.StickerSizeID
	WHERE td.TemplateName = '' 
		AND t.FromTemplate = 1
		AND pict.Category='PIC' 
		AND pict.BrandID = @BrandID
		AND pict.CTNRefno = @CtnRefno 
		AND pict.ShippingMarkCombinationUkey = @ShippingMarkCombinationUkey
		
		
	---- 若範本沒有上傳則回傳 0
	if (@CheckBasicSetting = @BasicSettingNotYet)
		return @BasicSettingNotYet

	-- 以上都成功代表基本檔皆已完成
	RETURN @Finished;
END
