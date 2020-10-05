
CREATE FUNCTION [dbo].[CheckShippingMarkHTMLSetting]
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
			, @PictureUploadNotYet bit = 0
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
		02 貼標組合底下涵蓋的標籤種類
	*/
	insert into @ShippingMarkTypeList (ShippingMarkCombinationUkey, ShippingMarkTypeUkey)
	select smcd.ShippingMarkCombinationUkey
			, smcd.ShippingMarkTypeUkey
	from [Production].[dbo].ShippingMarkCombination_Detail smcd
	where smcd.ShippingMarkCombinationUkey = @ShippingMarkCombinationUkey

	---- 若組合沒有標籤種類則回傳 0
	select @CheckBasicSetting = iif (count(*) > 0, @BasicSettingAlready, @BasicSettingNotYet)
	from @ShippingMarkTypeList

	if (@CheckBasicSetting = @BasicSettingNotYet)
		return @BasicSettingNotYet
		

	/*
		03 確認該紙箱 + 貼標組合 + 貼標種類是否都有設定位置基本檔，且貼標範本都有上傳
	*/
	select @CheckBasicSetting = iif (count(*) > 0, @BasicSettingNotYet, @BasicSettingAlready)
	from @ShippingMarkTypeList smtl
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
	
	---- 若紙箱沒有設定標籤位置則回傳 0
	if (@CheckBasicSetting = @BasicSettingNotYet)
		return @BasicSettingNotYet

		
	/*
		04 確認每個標籤種類都已經上傳HTML
	*/
	select @CheckBasicSetting = iif (count(*) > 0, @PictureUploadNotYet, @PictureUploadAlready)
	from @ShippingMarkTypeList smtl
	where not exists (
				select *
				from [Production].[dbo].ShippingMarkStamp smp
				inner join [Production].[dbo].ShippingMarkStamp_Detail smpd on smp.PackingListID = smpd.PackingListID
				where smp.PackingListID = @PackingListID
						and smpd.SCICtnNo = @SCICtnNo
						and smpd.ShippingMarkTypeUkey = smtl.ShippingMarkTypeUkey
						and smpd.FilePath  != ''
						and smpd.FileName != ''
			)

	if (@CheckBasicSetting = @PictureUploadNotYet)
		return @PictureUploadNotYet

	-- 以上都成功代表基本檔 + 圖檔皆已完成
	RETURN @Finished;
END
