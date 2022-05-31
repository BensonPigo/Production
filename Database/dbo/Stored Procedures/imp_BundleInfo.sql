CREATE PROCEDURE imp_BundleInfo
AS
BEGIN

	SET NOCOUNT ON;

	insert into [production].[dbo].[RFID_Combine]
	select src.RFIDCardNo
		   , src.MainID
		   , src.CreateTime
		   , src.UpdateTime
	from [RFID_Middle].[PMS_TO_RFID].[dbo].[RFID_Combine] src
	left join [production].[dbo].[RFID_Combine] tg on tg.RFUID=src.RFIDCardNo collate Chinese_Taiwan_Stroke_CI_AS
	where src.CardType=2
	and tg.RFUID is null
	
	update tg
	set  tg.RFUID=src.RFIDCardNo
		   , tg.BundleNo=src.MainID
		   , tg.AddDate=src.CreateTime
		   , tg.EditDate=src.UpdateTime
	from [production].[dbo].[RFID_Combine] tg
	inner join [RFID_Middle].[PMS_TO_RFID].[dbo].[RFID_Combine] src on tg.RFUID=src.RFIDCardNo collate Chinese_Taiwan_Stroke_CI_AS
	where src.CardType=2

END